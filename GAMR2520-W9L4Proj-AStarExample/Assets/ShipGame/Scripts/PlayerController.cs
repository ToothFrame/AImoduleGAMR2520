using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Move requires the GameObject to have a Rigidbody component
[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{
    private void Start()
    {
        Application.targetFrameRate = 30;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(15, 0, 0));
        }
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(-15, 0, 0));
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 15));
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -15));
        }
    }
}
