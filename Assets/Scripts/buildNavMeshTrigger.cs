using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class buildNavMeshTrigger : MonoBehaviour {

    NavMeshSurface navMesh;

	// Use this for initialization
	void Start () {

        navMesh = this.GetComponent<NavMeshSurface>();
        BuildMesh();
		
	}

    public void BuildMesh(){
        navMesh.BuildNavMesh();
    }
}
