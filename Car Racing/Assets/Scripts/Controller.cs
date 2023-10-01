using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    internal enum drivetype
    {
        frontwheelDrive,
        rearWheelDrive,
        allWheelDrive,
    };
    
    [SerializeField]private drivetype drive;

    public InpuManager IM;
    public WheelCollider[] wc = new WheelCollider[4];
    public float Torque = 200f;
    public float steeringMax = 20;
    public GameObject[] wm = new GameObject[4];
    public float radius = 6f;
    public float DownForceValue = 50f;
    private Rigidbody rb;
    public GameObject com;
    public float breakPower = 10000f;
    public float[] slip = new float[4];

    // Start is called before the first frame update
    void Start()
    {
        getObjects();
    }

    private void FixedUpdate()
    {
        animateWheels();
        moveVehicle();
        steercar();
        addDownForce();
        getObjects();
        getFriction();
    }

    private void steercar()
    {
        //acerman steering formula
        //steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * horizontalInput;
        if (IM.horizontal > 0)
        {
            //rear tracks size is set to 1.5f wheel base has been set to 2.55f
            wc[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * IM.horizontal;
            wc[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * IM.horizontal;
        }
        else if (IM.horizontal < 0)
        {
            wc[0].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius - (1.5f / 2))) * IM.horizontal;
            wc[1].steerAngle = Mathf.Rad2Deg * Mathf.Atan(2.55f / (radius + (1.5f / 2))) * IM.horizontal;
            //transform.Rotate(Vector3.up * steerHelping);
        }
        else
        {
            wc[0].steerAngle = 0;
            wc[1].steerAngle = 0;
        }
    }
    private void moveVehicle()
    {
        float totalPower;
        if(drive == drivetype.allWheelDrive) 
        {
            for (int i = 0; i < wc.Length; i++)
            {
                wc[i].motorTorque = IM.verticle * (Torque/4);
            }
        }
        else if(drive == drivetype.rearWheelDrive)
        {
            for (int i = 2; i < wc.Length; i++)
            {
                wc[i].motorTorque = IM.verticle * (Torque / 4);
            }
        }
        if (IM.handbrake)
        {
            wc[2].brakeTorque = wc[3].brakeTorque = breakPower;
        }
        else
        {
            wc[2].brakeTorque = wc[3].brakeTorque = 0;
        }
    }
        void animateWheels()
        {
            Vector3 wheelposition = Vector3.zero;
            Quaternion wheelrotation = Quaternion.identity;

            for (int i = 0; i < 4; i++)
            {
                wc[i].GetWorldPose(out wheelposition, out wheelrotation);
                wm[i].transform.position = wheelposition;
                wm[i].transform.rotation = wheelrotation;
            }
        }
    private void getObjects()
    {
        IM = GetComponent<InpuManager>();
        com = GameObject.Find("mass");
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com.transform.localPosition;
    }

    private void addDownForce()
    {
        rb.AddForce(-transform.up * DownForceValue * rb.velocity.magnitude);
    }
    
    private void getFriction()
    {
        for (int i = 0; i < wc.Length; i++)
        {
            WheelHit wh;
            wc[i].GetGroundHit(out wh);
            slip[i] = wh.sidewaysSlip;
        }
    }
}
