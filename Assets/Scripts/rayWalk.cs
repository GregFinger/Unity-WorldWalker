using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class rayWalk : MonoBehaviour
{
	Rigidbody rb;
	Vector3 upVector;
	Quaternion endRotation;

	private float startTime = 0f;
	private bool startRotate = false;
    private float gravity = 10f;
    private bool canRaycast = true;

	public float transitionDuration = 0.5f;
	public float turnSpeed = 20f;
    [HideInInspector]public float hitDistance;

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		rb.useGravity = false;
        upVector = this.transform.up;
	}

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity) && canRaycast==true)
        {
            hitDistance = hit.distance;
            if (hit.normal != upVector)
            {
                upVector = hit.normal;
                endRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                startTime = Time.time;
                startRotate = true;
                canRaycast = false;
            }
        }

        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            hitDistance = hit.distance;
            if (hit.normal != upVector)
            {
                upVector = hit.normal;
                endRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                startTime = Time.time;
                startRotate = true;
                canRaycast = false;
            }
        }

		rb.AddForce (-upVector * gravity);

        if (Input.GetKey("a"))
        {
            transform.RotateAround(transform.position, transform.up, -turnSpeed * Time.deltaTime);
        }

        if (Input.GetKey("d"))
        {
            transform.RotateAround(transform.position, transform.up, turnSpeed * Time.deltaTime);
        }

        if (startRotate)
        {
            float playback = (Time.time - startTime) / transitionDuration;

            //makes sure you do the shortest rotation
            if (Quaternion.Dot(transform.rotation, endRotation) < 0f)
            {
                endRotation = endRotation * Quaternion.AngleAxis(180f, Vector3.up);
            }

			if (playback < 1f) {
                transform.rotation = Quaternion.Lerp (transform.rotation, endRotation, playback);
			} 
			else {
                startRotate = false;
                canRaycast = true;
			}
		}
	}

	void OnTriggerEnter (Collider trigger)
	{
        if (this.gameObject.tag != trigger.tag)
        {
            upVector = trigger.transform.up;
            this.gameObject.tag = trigger.tag;

            endRotation = trigger.transform.rotation;

            //cancels all forward motion
            //rb.velocity = Vector3.zero;

			startTime = Time.time;
			startRotate = true;
            canRaycast = false;
		}
	}

	//Can only move foward and backward when making contact with a collider
	void OnCollisionStay (Collision collision)
	{
        if (Input.GetKey("w"))
        {
            rb.AddForce(transform.forward * 10f);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(transform.forward * -10f);
        }
	}

}