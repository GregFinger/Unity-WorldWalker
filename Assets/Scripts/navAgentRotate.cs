using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class navAgentRotate : MonoBehaviour {

    Vector3 upVector;
    [HideInInspector] public float hitDistance;

    public float turnSpeed = 35f;
    public rotateWorld rotateWorldScript;

	// Use this for initialization
	void Start () {
		
        upVector = this.transform.up;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity))
        {
            hitDistance = hit.distance;
            if (hit.normal != upVector)
            {
                upVector = hit.normal;
                float startTime = Time.time;
                rotateWorldScript.RotateBegin(startTime, upVector);
            }
        }

        if (Input.GetKey("a"))
        {
            transform.RotateAround(transform.position, transform.up, -turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            transform.RotateAround(transform.position, transform.up, turnSpeed * Time.deltaTime);
        }
	}
}
