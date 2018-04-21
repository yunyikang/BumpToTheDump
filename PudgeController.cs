using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;


public class PudgeController : NetworkBehaviour
{
    // main controller
    public Camera cam;
    private float gravity = 3 * 9.81f;
    public float speed;
    public float maxSpeed = 6.0f;  // max speed for pudge
    public Rigidbody rb;
    private Transform myTransform;
    private float rotationSpeed = 10.0f;
    private Vector3 facingDirection;
    public GameObject punchPrefab;

    public int numberOfLives;
    private GameObject lastTouch;
    public float smashDistance;
    public float dashBuffDuration = 2.0f;
    public float cdTime = 0.0f;

    // Ability
    public DashState currentState;

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
        this.lastTouch = this.gameObject;
        this.numberOfLives = 3;
        this.smashDistance = 100.0f;
        this.dashBuffDuration = 2.0f;
        this._playerSpawnPointpos = transform.position;

        GameObject.FindWithTag("GameOver").GetComponent<Text>().text = "";
        GameObject.FindWithTag("Lives").GetComponent<Text>().text = string.Format("Lives: {0}", numberOfLives);


    }


    // Update is called once per frame
    void Update()
    {
        // current coordinates of player - for use later
        var position = transform.position;
        
        // check if localPlayer (Networking)
        if (!isLocalPlayer)
        {
            return;
        }

        if (this.gameObject.transform.position.y <= 9.0f)
        {
            this.respawn();
        }

        // more gravity
        rb.AddForce(0, -gravity, 0);

        // Moves Pudge
        Movement_Controls();

        // checks cooldown
        //Skill_CD_Timer();

        // on skill button press
        if (SkillButtonBehaviour.Dashing)
        {
            Use_Skill();
            SkillButtonBehaviour.Dashing = false;
        }
        
        

        
    }

    void Movement_Controls()
    {
        // movement with joystick
        var direction = FixedJoystick.movePostition;
        var FJ_speed = 2.0f;

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
            Quaternion lookRotation = Quaternion.LookRotation(-push);
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, lookRotation,
                rotationSpeed * Time.deltaTime);
        }
    }

    // decrements skill cd timer
    //void Skill_CD_Timer()
    //{
    //    if (dashTimer < 5.0f && dashTimer > 5.0f - this.dashBuffDuration)
    //    {
    //        currentState = DashState.Dashing;
    //        dashTimer -= Time.deltaTime;
    //    }
    //    else if (dashTimer > 0)
    //    {
    //        currentState = DashState.Cooldown;
    //        dashTimer -= Time.deltaTime;
    //        //Debug.Log(dashTimer);
    //    }
    //    if (dashTimer <= 0)
    //    {
    //        currentState = DashState.Ready;
    //        //Debug.Log("Skill ready!");
    //    }

    //    if (currentState == DashState.Cooldown || currentState == DashState.Dashing)
    //    {
    //        GameObject.Find("SkillButton").GetComponentInChildren<Text>().text = dashTimer.ToString();
    //    }
    //    if (currentState == DashState.Ready)
    //    {
    //        GameObject.Find("SkillButton").GetComponentInChildren<Text>().text = "Skill";
    //    }
    //}



    [Command]
    void CmdUseSkill()
    {
        var bullet = (GameObject)Instantiate(
                    punchPrefab,
                    transform.position,
                    transform.rotation);

        bullet.gameObject.transform.position = this.gameObject.transform.position -
            bullet.transform.forward;

        NetworkServer.Spawn(bullet);

    }
    

    void Use_Skill()
    {
        CmdUseSkill();
        //switch (currentState)
        //{
        //    case DashState.Ready:
        //        // dashing state
        //        currentState = DashState.Dashing;
        //        CmdUseSkill();

        //        // reset dash timer
        //        dashTimer = this.cdTime;
        //        Debug.Log("Dash!");
        //        break;
        //    case DashState.Dashing:
        //        Debug.Log("Already dashing!");
        //        break;
        //    case DashState.Cooldown:
        //        Debug.Log("Skill still on cooldown!");
        //        break;
        //    default:
        //        break;
        //}
    }

    // respawn script
    void respawn()
    {
        //Debug.Log(this.numberOfLives);
        // cannot be itself (initial value)
        if (this.lastTouch.name == this.gameObject.name)
        {
            this.transform.position = _playerSpawnPointpos;
        }
        else
        {
            // else if someone killed him
            this.numberOfLives--;
            GameObject.FindWithTag("Lives").GetComponent<Text>().text = string.Format("Lives: {0}", numberOfLives);
            if (this.numberOfLives <= 0)
            {
                numberOfLives = 0;
                // gameover for this player
                Debug.Log("Gameover for" + this.gameObject.name);
                GameObject.FindWithTag("GameOver").GetComponent<Text>().text = "Game over!";
                Destroy(GameObject.FindWithTag("Objectives"));
                Destroy(this.gameObject);

            }
            else
            {
                this.transform.position = _playerSpawnPointpos;
            }
        }

    }


    // Collision script
    private void OnCollisionEnter(Collision c)
    {
        // only considers if hit other players
        if (c.gameObject.CompareTag("Player"))
        {
            
            this.lastTouch = c.gameObject;
            //CmdChangeLastTouch(c.gameObject);
            
            
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
                case "Projectile 1(Clone)":
                    transformValue = 5.0f;
                    break;

                // Punch interaction
                // PUJ no effect
                case "PujPuj(Clone)":
                    transformValue = 0;
                    break;
                default:
                    transformValue = 0;
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


public enum DashState
{
    Ready,
    Dashing,
    Cooldown
}
