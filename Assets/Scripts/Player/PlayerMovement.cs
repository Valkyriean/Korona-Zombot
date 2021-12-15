using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float WalkSpeed;
    [SerializeField] private float JumpIniSpeed;
    [SerializeField] private GameObject Player, playerController;
    [SerializeField] private CalculatePivot Pivot;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private HealthController healthController;
    [SerializeField] float yawSpeed = 3f;
    [SerializeField] float pitchSpeed = 3f;
    
    // for player direction
    private float yaw;
    private float pitch = 0;

    // for checking grounded
    private float groundedTimePassed = 0;
    private float distToGround;
    [SerializeField] private float CheckGroundedInterval = 0.1f;
    private float lastVelocityY = 0;
    public bool isGrounded;
    private float playerControllerY;
    private float isDashing, dashingCooldown;
    private AudioSource dashSound;


    // Start is called before the first frame update
    void Start()
    {
        distToGround = transform.position.y;
        playerControllerY = playerController.transform.localPosition.y;
        yaw = Player.transform.eulerAngles.y;
    }

    // Update is called once per frame
    void Update()
    {
        checkStartDash();
        checkStopDash();
        coolDownDash();
        checkGrounded();
        Rotate();
        Move();
    }

    private void checkGrounded()
    {
        groundedTimePassed += Time.deltaTime;
        if (groundedTimePassed >= CheckGroundedInterval){
            float deltaVelocityY = lastVelocityY - Player.GetComponent<Rigidbody>().velocity.y;
            lastVelocityY = Player.GetComponent<Rigidbody>().velocity.y;
            float acc = deltaVelocityY / groundedTimePassed;
            // Debug.Log(acc);
            // detect landing
            if (Math.Abs(acc) <= 5) 
            {
                //  Debug.Log("landing");
                isGrounded = true;
            } 
            // detect fall off
            if (Math.Abs(acc) > 5 && !Physics.Raycast(transform.position, Vector3.down, distToGround + 0.1f)) {
                //  Debug.Log("falling");
                isGrounded = false;
            }


            groundedTimePassed = 0;
        }
    }
    
    private void checkGroundedReset()
    {
        groundedTimePassed = 0;
        lastVelocityY = Player.GetComponent<Rigidbody>().velocity.y;
    }

    private void Rotate()
    {
        yaw += yawSpeed * Input.GetAxis("Mouse X") * Time.timeScale;
        pitch -= pitchSpeed * Input.GetAxis("Mouse Y") * Time.timeScale;
        pitch = Mathf.Clamp(pitch, -60f, 90f);
        this.transform.eulerAngles = new Vector3(pitch, yaw, 0.0f) + healthController.cameraOffset;
        Player.transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f) + healthController.cameraOffset;
    }

    private void Move()
    {
        double yawR = (double)yaw * Math.PI / 180.0;
        float interpertedX, interpertedY;
        float inSpeed2 = Input.GetAxis("Horizontal") * Input.GetAxis("Horizontal") + Input.GetAxis("Vertical") * Input.GetAxis("Vertical");
        float inspeed;
        if (inSpeed2 <= 1) 
        {
            interpertedX = Input.GetAxis("Vertical");
            interpertedY = Input.GetAxis("Horizontal");
        } else {
            inspeed = (float)Math.Sqrt((double)inSpeed2);
            interpertedX = Input.GetAxis("Vertical") / inspeed;
            interpertedY = Input.GetAxis("Horizontal") / inspeed;
        }
        Vector3 dir = new Vector3((float)Math.Cos(yawR),0,-(float)Math.Sin(yawR));
        Vector3 dirside = new Vector3(-(float)Math.Cos(yawR + Math.PI / 2),0,(float)Math.Sin(yawR + Math.PI / 2));
        Vector3 velocityXZ = new Vector3((dir * interpertedY * WalkSpeed + dirside * interpertedX * WalkSpeed).x, 0, (dir * interpertedY * WalkSpeed + dirside * interpertedX * WalkSpeed).z);
        velocityXZ += new Vector3(Pivot.front.x, 0, Pivot.front.z) * isDashing * 15;
        if (isDashing <= 0.2 && isDashing != 0) velocityXZ = velocityXZ * (0.5f + isDashing);
        Player.GetComponent<Rigidbody>().velocity = velocityXZ + new Vector3 (0, Player.GetComponent<Rigidbody>().velocity.y, 0);
        // Player.transform.position += dir * interpertedY * Time.deltaTime * WalkSpeed;
        // Player.transform.position += dirside * interpertedX * Time.deltaTime * WalkSpeed;
        
        if (Input.GetButtonDown("Jump") && isGrounded) {
            Player.GetComponent<Rigidbody>().velocity += new Vector3(0,JumpIniSpeed,0);
            isGrounded = false;
            checkGroundedReset();
        }
    }

    private float camerOffset() {
        if (isDashing <= 0.0f) {
            return 0;
        } else if (isDashing <= 0.1f) {
            return -0.2f * isDashing * 10f;
        } else if (isDashing >= 0.65f){
            return -0.2f * (0.7f - isDashing) * 20f;
        } else {
            return -0.2f;
        }
    }

    private void checkStartDash(){
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashingCooldown == 0) {
            dashSound = GetComponents<AudioSource>()[0];
            dashSound.Play();

            isDashing = 0.7f;
            dashingCooldown = 3.0f;
        }
    }

    private void checkStopDash(){
        if (Input.GetButtonDown("Fire2") && isDashing > 0.2f) isDashing = 0.2f;
        playerController.transform.localPosition = new Vector3(0, playerControllerY + camerOffset(), 0);
        playerCamera.fieldOfView = 60 ;
        if (isDashing > 0f) {
            isDashing -= Time.deltaTime;
        } else {
            isDashing = 0f;
        }
    }

    private void coolDownDash(){
        if (dashingCooldown > 0f) {
            dashingCooldown -= Time.deltaTime;
        } else {
            dashingCooldown = 0f;
        }
    }
}
