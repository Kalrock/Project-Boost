using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector;

    //todo remove from inspector later
    [Range(0,1)] [SerializeField] float movementFactor; // 0 for not moved, 1 for fully moved
    [SerializeField] float period = 2f;

    Vector3 startingPos;
    Vector3 maxOffset;
    const float tau = Mathf.PI * 2f;

    // Use this for initialization
    void Start ()
    {
        startingPos = transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        MovePosition();
    }

    private void MovePosition()
    {
        if (period <= Mathf.Epsilon) { return; }

        GetMovement();

        Vector3 offset = movementFactor * movementVector;

        transform.position = startingPos + offset;
    }

    private void GetMovement()
    {

        float cycles = Time.time / period; // grows continually from 0 

        float rawSineWave = Mathf.Sin(cycles * tau);

        movementFactor = rawSineWave / 2f;
    }
}
