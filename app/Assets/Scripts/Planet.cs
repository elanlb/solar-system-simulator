using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {
	public float mass = 1;
	public string displayName;
	void FixedUpdate () {
		if (Vector3.Distance(gameObject.transform.position, GameObject.Find("Sun").transform.position) > 100) {
			Destroy(gameObject);
		}
	}
}
