using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForceOnInput : MonoBehaviour {


    public float force;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {

        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.forward * force);
        }

		
	}
}
