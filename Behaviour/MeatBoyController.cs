using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Prototype.NetworkLobby;

public class MeatBoyController : NetworkBehaviour
{
    // main controller
    public Camera cam;
    private float gravity = 3 * 9.82f;
    public float speed;
    public float maxSpeed = 3.0f;  // max speed for SMB
    public Rigidbody rb;
    private Transform myTransform;
    private float rotationSpeed = 10.0f;
    public Vector3 facingDirection;
    public GameObject bombObject;

    private GameObject lastTouch;
    public int numberOfLives = 3;
    Vector3 myScale;

    // Ability
    public SMBState currentState = SMBState.READY;
    float cdTimer = 0.0f;
    float bigTimer = 0.0f;
    float cdTime = 5.0f;
    Vector3 originalSize;

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
        numberOfLives = 3;
        this.originalSize = transform.localScale;
        this.currentState = SMBState.READY;
        this.myScale = transform.localScale;
        this.lastTouch = this.gameObject;
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

        // Adds more gravity
        rb.AddForce(0, -gravity, 0);

        // Moves SMB
        this.Movement_Controls();

        // checks cooldown
        this.Skill_CD_Timer();

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
        var FJ_speed = 1.5f;

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
            myTransform.rotation = Quaternion.Slerp(myTransform.rotation, lookRotation,
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

    // decrements skill cd timer
    void Skill_CD_Timer()
    {
        if (cdTimer > 0)
        {
            cdTimer -= Time.deltaTime;
            //Debug.Log(cdTimer);
        }
        else
        {
            currentState = SMBState.READY;
            //Debug.Log("Skill ready!");
        }

        if (currentState != SMBState.READY)
        {
            GameObject.Find("SkillButton").GetComponentInChildren<Text>().text = cdTimer.ToString();
        }
        else
        {
            GameObject.Find("SkillButton").GetComponentInChildren<Text>().text = "Skill";
        }
        
    }



    // Uses skill
    void Use_Bomb()
    {
        var smbbomb = (GameObject)Instantiate(
                    bombObject,
                    transform.position,
                    transform.rotation);

        smbbomb.gameObject.transform.position = this.gameObject.transform.position +
            smbbomb.transform.forward;

        NetworkServer.Spawn(smbbomb);
    }

    

    void Use_Skill()
    {
        switch (currentState)
        {
            case SMBState.READY:

                // perform skill
                
                Use_Bomb();
                GetComponent<NetworkTransform>().transformSyncMode = NetworkTransform.TransformSyncMode.SyncTransform;


                // reset cd timer
                currentState = SMBState.BIG;
                this.cdTimer = this.cdTime;
                Debug.Log("BOMBING!");
                break;

            case SMBState.BIG:
                break;

            case SMBState.COOLDOWN:
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
                // Bomb interaction (Non-existent for SMB)
                case "SMB_Bomb(Clone)":
                    return;

                // Shoot interaction
                case "Projectile 1(Clone)":
                    transformValue = 5.0f;
                    break;

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

public enum SMBState
{
    READY,
    BIG,
    COOLDOWN
}
