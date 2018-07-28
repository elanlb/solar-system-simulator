using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMotion : MonoBehaviour {
	
	private Vector3 goal = Vector3.zero;
	private Rigidbody rb;
	private Vector3 force;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
	}
	
    void FixedUpdate()
    {
		force = goal - transform.position;
        rb.AddForce(force.normalized);
    }
}
