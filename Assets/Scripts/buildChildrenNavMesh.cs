using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class buildChildrenNavMesh : MonoBehaviour {

	// Use this for initialization
	void Start () {
        foreach (Transform child in transform)
        {
            child.gameObject.AddComponent<NavMeshSurface>();
            child.gameObject.GetComponent<NavMeshSurface>().BuildNavMesh();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
