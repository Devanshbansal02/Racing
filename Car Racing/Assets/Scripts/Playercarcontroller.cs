using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercarcontroller : MonoBehaviour
{
    [Header("wheels collider")]
    public WheelCollider frontRightc;
    public WheelCollider frontLeftc;
    public WheelCollider backLeftc;
    public WheelCollider backRightc;

    [Header("wheels transform")]
    public Transform frontRight;
    public Transform frontLeft;
    public Transform backRight;
    public Transform backLeft;

    [Header("car engine")]
    public float accelerationforce = 300f;
    public float breakingforce = 3000f;
    public float presentbreakingforce = 0f;
    public float presentacceleration = 0f;

    [Header("car steering")]
    public float Torque = 35f;
    public float presentTurnAngle = 0f;
    private void Update()
    {
        Movecar();
        Steering();
    }
    private void Movecar()
    {
        //wheel drive system
        frontLeftc.motorTorque = presentacceleration;
        frontRightc.motorTorque = presentbreakingforce;
        backLeftc.motorTorque = presentacceleration;
        backRightc.motorTorque = presentbreakingforce;
        presentacceleration = accelerationforce * Input.GetAxis("Vertical");
    }

    private void Steering()
    {
        presentTurnAngle = Torque * Input.GetAxis("Horizontal");
        frontLeftc.steerAngle = presentTurnAngle;
        frontRightc.steerAngle = presentTurnAngle;
    }
}

