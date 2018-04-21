using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using Prototype.NetworkLobby;

public class SudowudoController : NetworkBehaviour
{
    // main controller
    public Camera cam;
    private float gravity = 3 * 9.81f;
    public float speed;
    public float maxSpeed = 6.0f;  // max speed for sudo
    public Rigidbody rb;
    private Transform myTransform;
    private float rotationSpeed = 10.0f;
    public Vector3 facingDirection;
    public float mass = 5.0f;
    public GameObject projectilePrefab;

    public int numberOfLives;
    [SyncVar]
    private GameObject lastTouch;



    // Ability
    public ShootState currentState;
    private float dashTimer = 0.0f;
    public float maxCDTime = 1.0f;
    public float dashSpeed = 20.0f;
    public Vector2 savedVelocity;

    // network spawning
    private Vector3 _playerSpawnPointpos; // player to spawn here


    // Use this for initialization
    void Start()
    {
        // prevents rolling
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // initialise rigidbody
        this.rb = GetComponent<Rigidbody>();
        // gets current transform
        myTransform = transform;
        this._playerSpawnPointpos = transform.position;
        numberOfLives = 3;
        lastTouch = this.gameObject;
        GameObject.FindWithTag("GameOver").GetComponent<Text>().text = "";
        GameObject.FindWithTag("Lives").GetComponent<Text>().text = string.Format("Lives: {0}", numberOfLives);

    }


    // Update is called once per frame
    void Update()
    {
        // check if localPlayer (Networking)
        if (!isLocalPlayer)
        {
            return;
        }

        if (this.gameObject.transform.position.y <= 9.0f)
        {
            this.respawn();
        }

        // Moves Sudo

        Movement_Controls();

        // more gravity
        rb.AddForce(0, -gravity, 0);
        

        // checks cooldown
        Skill_CD_Timer();

        // on skill button press
        if (SkillButtonBehaviour.Dashing)
        {
            Use_Dash_Skill();
            SkillButtonBehaviour.Dashing = false;
        }


    }

    void Movement_Controls()
    {
        // movement with joystick
        var direction = FixedJoystick.movePostition;
        var FJ_speed = 1.0f;

        // set limits
        var joy_limit = 100;
        if (direction.x > joy_limit)
        {
            direction.x = joy_limit;
        }
        if (direction.x < -joy_limit)
        {
            direction.x = -joy_limit;
        }
        if (direction.y > joy_limit)
        {
            direction.y = joy_limit;
        }
        if (direction.y < -joy_limit)
        {
            direction.y = -joy_limit;
        }

        Vector3 push = new Vector3(direction.x, 0.0f, direction.y);
        facingDirection = push;

        rb.AddForce(push * FJ_speed);
        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }

        // makes char look in direction
        if (push != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(push);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    // respawn script
    void respawn()
    {
        Debug.Log(this.numberOfLives);
        // cannot be itself (initial value)
        if (this.lastTouch.name == this.gameObject.name)
        {
            this.transform.position = _playerSpawnPointpos;
        }
        else
        {
            // else if someone killed him
            numberOfLives--;
            GameObject.FindWithTag("Lives").GetComponent<Text>().text = string.Format("Lives: {0}", numberOfLives);



            if (this.numberOfLives <= 0)
            {
                numberOfLives = 0;
                // gameover for this player
                Debug.Log("Gameover for" + this.gameObject.name);
                GameObject.FindWithTag("GameOver").GetComponent<Text>().text = "Game over!";
                Destroy(GameObject.FindWithTag("Objectives"));

                //Network.Destroy(this.gameObject);
                Destroy(this.gameObject);
            }
            else
            {
                this.transform.position = _playerSpawnPointpos;
            }
        }

    }

    // decrements skill cd timer
    void Skill_CD_Timer()
    {
        if (dashTimer > 0)
        {
            currentState = ShootState.Cooldown;
            dashTimer -= Time.deltaTime;
            //Debug.Log(dashTimer);
        }
        if (dashTimer <= 0)
        {
            currentState = ShootState.Ready;
            //Debug.Log("Skill ready!");
        }

        if (currentState == ShootState.Cooldown)
        {
            GameObject.Find("SkillButton").GetComponentInChildren<Text>().text = dashTimer.ToString();
        }
        if (currentState == ShootState.Ready)
        {
            GameObject.Find("SkillButton").GetComponentInChildren<Text>().text = "Skill";
        }
    }

    

    [Command]
    void CmdChangeLastTouch(GameObject g)
    {
        lastTouch = g;
    }


    [Command]
    void CmdShoot()
    {
        var bullet = (GameObject)Instantiate(
                    projectilePrefab,
                    transform.position,
                    transform.rotation);

        bullet.gameObject.transform.position = this.gameObject.transform.position +
            bullet.transform.forward;

        NetworkServer.Spawn(bullet);

        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * -10;

        Destroy(bullet, 3.0f);
    }

    
    void Use_Dash_Skill()
    {

        switch (currentState)
        {
            case ShootState.Ready:

                //perform shoot 
                CmdShoot();

                // reset dash timer
                dashTimer = maxCDTime;
                Debug.Log("Shoot!");
                break;
            case ShootState.Cooldown:
                Debug.Log("Skill still on cooldown!");
                break;
            default:
                break;
        }
    }



    // Collision script
    private void OnCollisionEnter(Collision c)
    {

        // On hit with other players
        if (c.gameObject.CompareTag("Player"))
        {
            // determines last touch
            this.lastTouch = c.gameObject;
            CmdChangeLastTouch(c.gameObject);

        }

        // Skill interaction scripts
        if (c.gameObject.CompareTag("SkillObject"))
        {
            Vector3 oPos = c.rigidbody.transform.position;
            Vector3 tPos = transform.position;
            float transformValue;
            Vector3 difference;

            switch (c.gameObject.name)
            {
                // Bomb interaction 
                case "SMB_Bomb(Clone)":
                    transformValue = 12.0f;
                    break;

                // Shoot interaction
                // SUDO no effect
                case "Projectile 1(Clone)":
                    return;

                // Punch interaction
                default:
                    transformValue = 3.0f;
                    break;
            }
            difference = new Vector3(
                        tPos.x + transformValue * (tPos.x - oPos.x),
                        tPos.y,
                        tPos.z + transformValue * (tPos.z - oPos.z));

            gameObject.transform.position = difference;

        }

    }


    public override void OnStartLocalPlayer()
    {
        Camera.main.GetComponent<CameraFollow>().setTarget(gameObject.transform);
    }


}


public enum ShootState
{
    Ready,
    Cooldown
}
