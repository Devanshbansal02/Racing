using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InpuManager : MonoBehaviour
{
    public float verticle;
    public float horizontal;
    public bool handbrake;

    private void FixedUpdate()
    {
        verticle = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        handbrake = (Input.GetAxis("Jump") != 0) ? true : false;
    }
}
