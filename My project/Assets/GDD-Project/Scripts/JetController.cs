using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class JetController : MonoBehaviour
{
    public CameraController mainCamera;
    public GameObject[] landingGears;

    public float engineThrust = 500f;
    public float pitchSpeed = 10f;
    public float rollSpeed=17f;
    public float yawSpeed=7f;


    private Rigidbody rb;

    private float thrust;
    private float pitch;
    private float roll;
    private float yaw;

    internal float speed;
    internal float height;
    internal float throttle {get{ return thrust; } }
    private const float mTOKm = 3.6f;
    private const float kmToKnots = 0.539f;
    private const float aerodynamicEffect=0.1f;
    private bool landingGearsRetracted;

private void Awake()
{
    rb = GetComponent<Rigidbody>();
    if(rb==null) rb = gameObject.AddComponent<Rigidbody>();
    if(rb.mass==1)
    {
        rb.mass=20000;
        rb.drag=0.075f;
        rb.angularDrag=0.05f;
    }
     
}

    // Start is called before the first frame update 
    // Update is called once per frame
    void Update()
    {
        pitch=0f;
        roll=0f;
        yaw=0f;

    if(Input.GetKey(KeyCode.Q)) yaw = -1f;
    if(Input.GetKey(KeyCode.E)) yaw = 1f;

    if(Input.GetKey(KeyCode.A)) roll = 1f;
    if(Input.GetKey(KeyCode.D)) roll = -1f;

    if(Input.GetKey(KeyCode.W)) pitch = 1f;
    if(Input.GetKey(KeyCode.S)) pitch = -1f;

    UpdateThrottle();
    UpdateCamera();
    height = transform.position.y - 1f;

    if(height > 5 && !landingGearsRetracted)
    {
        RetractLandingGears();
    }


    }
    void RetractLandingGears()
    {
        landingGearsRetracted=true;
        for(int i=0;i<landingGears.Length;i++)
        {
            landingGears[i].SetActive(false);
        }
    }
    void UpdateThrottle()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket)) thrust = 30f;
        if (Input.GetKeyDown(KeyCode.RightBracket)) thrust = 60f;
        if (Input.GetKeyDown(KeyCode.Backspace)) thrust = 100f;
        if (Input.GetKeyDown(KeyCode.Backslash)) thrust = 0f;

        if(Input.GetKey(KeyCode.KeypadPlus)) thrust += 10f;
        if(Input.GetKey(KeyCode.KeypadMinus)) thrust -= -10f;

        thrust = Mathf.Clamp(thrust, 0f, 100f);

    }
    void UpdateCamera()
    {
        if(Input.GetMouseButton(1)) return;
        mainCamera.updatePosition(Input.GetAxis("Mouse X"),Input.GetAxis("Mouse Y"));
    }
    private void FixedUpdate()
    {
        transform.RotateAround(transform.position, transform.up , yaw * Time.fixedDeltaTime * yawSpeed);
        if(height > 2f)
           transform.RotateAround(transform.position, transform.forward , roll * Time.fixedDeltaTime * rollSpeed);

        if(rb.velocity.magnitude > 100f) 
           transform.RotateAround(transform.position, transform.right , pitch * Time.fixedDeltaTime * pitchSpeed);

        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        var localSpeed=Mathf.Max(0,localVelocity.z);
        speed=(localSpeed*mTOKm)*kmToKnots;

        // Borrowed from Unity standards Assets
        var aerofactor = Vector3.Dot(transform.forward,rb.velocity.normalized);
        aerofactor *= aerofactor;
        rb.velocity = Vector3.Lerp(rb.velocity,transform.forward * localSpeed, aerofactor * localSpeed * aerodynamicEffect * Time.fixedDeltaTime);
        rb.AddForce((thrust * engineThrust)* transform.forward);
    }
}
