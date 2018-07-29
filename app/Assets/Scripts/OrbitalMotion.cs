using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalMotion : MonoBehaviour {
	
	private Vector3 goal = Vector3.zero;
	private Rigidbody rb;
	private Vector3 force;
	
	private bool exploding = false;

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
		if (exploding) return;

		if (collision.collider.gameObject.name.Equals("Sun")){
			// sun collision
			exploding = true;
			StartCoroutine(ExplosionRoutine());
		}
	}

	IEnumerator ExplosionRoutine(){
		var explosion = GameObject.Instantiate(GameObject.Find("Explosion"));

		explosion.transform.position = transform.position;

		// enable surface layer
		var surfaceLayer = explosion.transform.GetChild(1);
		var dia = GetComponent<SphereCollider>().radius *2;
		surfaceLayer.transform.localScale = new Vector3(dia, dia, dia);
		surfaceLayer.gameObject.SetActive(true);

		// enable particle system
		explosion.transform.GetChild(0).gameObject.SetActive(true);
		var flare = explosion.transform.GetChild(2);
		flare.transform.rotation = Random.rotation;
		flare.gameObject.SetActive(true);
		
		float explosiontime = 0;
		float planetScale = .9f;

		while (planetScale > .5f){
			explosiontime += Time.deltaTime;
			planetScale = planetScale-(explosiontime*explosiontime);

			transform.localScale = new Vector3(planetScale, planetScale, planetScale);
			yield return null;
		}

		GameObject.Destroy(gameObject);
	}
}
