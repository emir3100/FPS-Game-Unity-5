using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    [Header("Assingables")]
    public Transform playerCam;
    public Transform orientation;
    public Transform Legs;
    public MoveLegs MoveLegsScript;
    private CameraFOV CameraFov;

    [HideInInspector]
    public Rigidbody rb;

    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    [Header("Movement")]
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask whatIsGround;
    public float PlayerVelocity;
    public ParticleSystem SpeedEffect;

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    [Header("Slide")]
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    private Vector3 legsScale;

    [Header("Jumping")]
    public float jumpForce = 550f;

    public bool readyToJump = true;
    private float jumpCooldown = 0.25f;

    float x, y;
    bool jumping, sprinting, crouching;

    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    private float desiredX;
    private bool cancellingGrounded;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start() {
        CameraFov = Camera.main.GetComponent<CameraFOV>();
        playerScale = transform.localScale;
        legsScale = Legs.transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    private void FixedUpdate() {
        Movement();
    }

    private void Update() {
        MyInput();
        Look();
        PlayerVelocity = rb.velocity.magnitude;

        if (PlayerVelocity > 35)
        {
            SpeedEffect.Play();
            SpeedEffect.gameObject.SetActive(true);
            FindObjectOfType<AudioManager>().Play("Speed");
            CameraFov.SetCameraFov(60f + (PlayerVelocity*0.3f));
        }
        else
        {
            SpeedEffect.gameObject.SetActive(false);
            SpeedEffect.Stop();
            FindObjectOfType<AudioManager>().Stop("Speed");
            CameraFov.SetCameraFov(60f);
        }
        var main = SpeedEffect.main;
        main.simulationSpeed = PlayerVelocity / 30f;

        if (main.simulationSpeed < 0 || PlayerVelocity < 0 || PlayerVelocity == 4.768372e-06)
        {
            SpeedEffect.gameObject.SetActive(false);
            SpeedEffect.Stop();
            CameraFov.SetCameraFov(60f);
        }
    }

    public bool IsMoving()
    {
        if (x != 0 || y != 0)
            return true;
        else
            return false;
    }
    private void MyInput() {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetKey(KeyCode.LeftControl);
      
        if (Input.GetKeyDown(KeyCode.LeftControl))
            StartCrouch();
        if (Input.GetKeyUp(KeyCode.LeftControl))
            StopCrouch();
    }

    private void StartCrouch() {
        MoveLegsScript.LegsY = 0.50f;
        transform.localScale = crouchScale;
        Legs.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f) {
            if (grounded) {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }
    private void StopCrouch() {
        MoveLegsScript.LegsY = 1.51f;
        transform.localScale = playerScale;
        Legs.localScale = legsScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement() {
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        CounterMovement(x, y, mag);
        
        if (readyToJump && jumping) Jump();

        float maxSpeed = this.maxSpeed;
        
        if (crouching && grounded && readyToJump) {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }
        
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        float multiplier = 1f, multiplierV = 1f;
        
        if (!grounded) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        
        if (grounded && crouching) multiplierV = 0f;

        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump() {
        if (grounded && readyToJump) {
            readyToJump = false;

            FindObjectOfType<AudioManager>().Play("Jump");
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void ResetJump() {
        readyToJump = true;
    }
    
    private void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!grounded || jumping) return;

        if (crouching) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }

        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private void OnCollisionStay(Collision collision) {
        int layer = collision.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        for (int i = 0; i < collision.contactCount; i++) {
            Vector3 normal = collision.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() {
        grounded = false;
    }
    
}
