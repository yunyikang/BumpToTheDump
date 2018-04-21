
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sot : MonoBehaviour
{
    /*
    public float speed;
    public Rigidbody rb;
    public GameObject shot;
    public Transform relative;
    private ParticleSystem boostSparkle;
    private Transform shotSpawn;
    


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        var x = Input.GetAxis("Horizontal") * Time.deltaTime * 150.0f;
        var z = Input.GetAxis("Vertical") * Time.deltaTime * 3.0f;

        transform.Rotate(0, x, 0);
        transform.Translate(0, 0, z);
        if (Input.GetKeyUp(KeyCode.Space))
        {

            //shotSpawn.position = relative.position + new Vector3(0F, 2F, 0F);
            Instantiate(shot, relative.position + new Vector3(0F, 2F, 1F), relative.rotation);


        }
    }

    void FixedUpdate()
    {
        /*
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rb.AddForce(movement * speed);
        

    }
    
    
    StartCoroutine(Example());
}

IEnumerator Example()
{
    //boostSparkle = GetComponentInChildren<ParticleSystem>();
    rb = GetComponent<Rigidbody>();
    gameObject.GetComponentInChildren<TrailRenderer>().enabled = false;
    //boostSparkle.gameObject.SetActive(false);
    print(Time.time);
    yield return new WaitForSeconds(5);
    //boostSparkle.gameObject.SetActive(true);

    gameObject.GetComponent<TrailRenderer>().enabled = true;
    rb.velocity = transform.forward * speed;
    shot.SetActive(true);
    print(Time.time);






}

}
using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
    */
    int dashHash = Animator.StringToHash("Dash");
    int knockHash = Animator.StringToHash("KnockBack");
    Animator anim;
    public float speed;
    public Rigidbody rb;
    public GameObject shot;
    public Transform relative;
    private ParticleSystem boostSparkle;
    private Transform shotSpawn;
    public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
       // public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
       // public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        //public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
       // public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        //private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks

        public float fireRate;

        private float nextFire;
    private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        }


        private void OnEnable()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {

            m_particleSystems[i].Stop();
            //m_particleSystems[i].Play();
                  print("enabled");
           }
        }


        private void OnDisable()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            print("disabled");
            }
        }


        private void Start()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source.
            //m_OriginalPitch = m_MovementAudio.pitch;
        }


        private void Update()
        {
            // Store the value of both input axes.
            m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
            m_TurnInputValue = Input.GetAxis(m_TurnAxisName);
        anim.SetFloat("speed", m_MovementInputValue);

            //EngineAudio();
            if (Input.GetKeyUp(KeyCode.Space) && Time.time > nextFire)
            {
                
                nextFire = Time.time + fireRate;
                StartCoroutine(waiter());
                anim.SetTrigger(dashHash);
            

        }
            if (Input.GetKeyUp(KeyCode.J))
        {
            anim.SetTrigger(knockHash);

            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                if (m_particleSystems[i].isPlaying) { 
                m_particleSystems[i].Stop();
                print("disabled");
            }
            }
        }
        }

        IEnumerator waiter()
        {
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {

                //m_particleSystems[i].Stop();
                m_particleSystems[i].Play();
                print("enabled");
            }
            //shotSpawn.position = relative.position + new Vector3(0F, 2F, 0F);
            Instantiate(shot, relative.position + new Vector3(0F, 2F, 1F), relative.rotation);
  

        //Wait for 2 seconds
            yield return new WaitForSeconds(2);
          for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
                print("disabled");
            }
        }



    private void FixedUpdate()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move();
            Turn();
        }


        private void Move()
        {
            
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }
    }
