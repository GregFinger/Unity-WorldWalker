using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navAgentWalk : MonoBehaviour {

    private NavMeshAgent agent;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
	}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey("w"))
        {
            agent.Move(this.gameObject.transform.GetChild(0).transform.forward * 0.1f);
        }
        if (Input.GetKey("s"))
        {
            agent.Move(this.gameObject.transform.GetChild(0).transform.forward * -0.1f);
        }
    }
}