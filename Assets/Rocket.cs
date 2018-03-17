using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    Rigidbody ridgeBody;

	// Use this for initialization
	void Start () {
        ridgeBody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        ProcessInput();
	}

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ridgeBody.AddRelativeForce(10*Vector3.up);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.back);
        }
    }
}
