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
        rb.AddForce(force.normalized * 3 * getGravityAmplitude(force));
    }

	float getGravityAmplitude(Vector3 force){
		float magnitude = force.magnitude;
		return 1/(magnitude*magnitude);
	}

	void OnCollisionEnter(Collision collision){
		print(collision.collider.gameObject.name);
		if (collision.collider.gameObject.name.Equals("Sun")){
			// sun collision
			// explode
			GameObject.Destroy(gameObject, .5f);

			var explosion = GameObject.Instantiate(GameObject.Find("Explosion"));

			explosion.transform.position = transform.position;
			for (int i = 0; i < transform.childCount; i++){
				explosion.transform.GetChild(i).gameObject.SetActive(true);
			}

		}
	}
}
