using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Rigidbody))]
public class rayWalk : MonoBehaviour
{
	Rigidbody rb;
	Vector3 upVector;
    Vector3 prevUpVector;
	Quaternion endRotation;
    RaycastHit hit;

	private float startTime = 0f;
	private bool startRotate = false;
    private float gravity = 10f;
    private bool colliding;
	public float transitionDuration = 0.5f;
	public float spinSpeed = 35f;
    [HideInInspector]public float hitDistance;


	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
		rb.useGravity = false;
        upVector = this.transform.up;

	}

    void FixedUpdate()
    {
        // adds a force downwards to imitate gravity
        rb.AddForce(-upVector * gravity);

        // spins left and right based on the up vector
        if (Input.GetKey("a"))
        {
            //transform.RotateAround(transform.position, transform.up, -spinSpeed * Time.deltaTime);
            //transform.Rotate(Vector3.up * -spinSpeed * Time.deltaTime, Space.Self);
            rb.MoveRotation(rb.rotation * Quaternion.Euler(new Vector3(0, -spinSpeed, 0) * Time.deltaTime));
        }

        if (Input.GetKey("d"))
        {
            transform.RotateAround(transform.position, transform.up, spinSpeed * Time.deltaTime);
        }

        //
        // Downwards raycast, can't raycast while rotating towards new surface.
        //
        if (Physics.Raycast(transform.position, -transform.up, out hit, Mathf.Infinity) && startRotate==false)
        {
            hitDistance = hit.distance;
            if (hit.normal != upVector)
            {
                prevUpVector = upVector;
                print("down");
                upVector = hit.normal;
                endRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                startTime = Time.time;
                startRotate = true;
            }
        }

        //
        // Forward raycast
        //
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            hitDistance = hit.distance;
            if (hit.normal != upVector)
            {
                prevUpVector = upVector;
                print("forward");
                upVector = hit.normal;
                endRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                startTime = Time.time;
                startRotate = true;
            }
        }

        //
        // This only does a backwards raycast if the rigidbody currently isn't on a collider
        // and if the hit surface isn't the previous surface that the rigidbody just
        // transitioned from
        //
        if (Physics.Raycast(transform.position, -transform.forward, out hit, 3f) && colliding == false)
        {
            hitDistance = hit.distance;

            if (hit.normal != upVector && hit.normal != prevUpVector)
            {
                print("backwards");
                upVector = hit.normal;
                endRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                startTime = Time.time;
                startRotate = true;
            }
        }

        //
        // Rotates rigidbody so its up vector matches the up vector of the
        // surface it's transitioning to
        //
        if (startRotate)
        {
            float playback = (Time.time - startTime) / transitionDuration;

            // makes sure you do the shortest rotation
            if (Quaternion.Dot(transform.rotation, endRotation) < 0f)
            {
                endRotation = endRotation * Quaternion.AngleAxis(180f, Vector3.up);
            }

			if (playback < 1f) {
                //transform.rotation = Quaternion.Slerp(transform.rotation, endRotation, playback);
                rb.MoveRotation(Quaternion.Slerp (transform.rotation, endRotation, playback));
			}
			else {
                startRotate = false;
			}
		}
	}

    void OnCollisionStay(Collision collision)
    {
        colliding = true;

        // Can only move foward and backward when making contact with a collider
        if (Input.GetKey("w"))
        {
            rb.AddForce(transform.forward * 10f);
        }
        if (Input.GetKey("s"))
        {
            rb.AddForce(transform.forward * -10f);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        colliding = false;
    }

    // old method using triggers to force rotations for
    // difficult surface orientations
    /*
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
    */
}