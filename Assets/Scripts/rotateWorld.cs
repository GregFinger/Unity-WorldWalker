using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotateWorld : MonoBehaviour {

    private float startTime = 0f;
    private bool startRotate = false;
    private bool ignore = false;
    public float transitionDuration = 0.5f;
    Vector3 hitNormal;

    buildNavMeshTrigger navMeshTriggerScript;

	// Use this for initialization
	void Start () {

        navMeshTriggerScript = GetComponentInParent<buildNavMeshTrigger>();

	}
	
	// Update is called once per frame
	void Update () {
        
        if (startRotate)
        {
            Quaternion endRotation = Quaternion.FromToRotation(hitNormal,Vector3.up);

            float playback = (Time.time - startTime) / transitionDuration;

            //makes sure you do the shortest rotation
            if (Quaternion.Dot(transform.rotation, endRotation) < 0f)
            {
                endRotation = endRotation * Quaternion.AngleAxis(180f, Vector3.up);
            }

            if (playback < 1f)
            {
                ignore = true;
                transform.rotation = Quaternion.Lerp(transform.rotation, endRotation, playback);
            }
            else
            {
                ignore = false;
                startRotate = false;
                navMeshTriggerScript.BuildMesh();
            }
        }

	}

    public void RotateBegin(float time, Vector3 endRot){
        if (ignore == false)
        {
            hitNormal = endRot;
            startTime = time;
            startRotate = true;
        }
    }
}
