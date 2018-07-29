using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSurfaceLayer : MonoBehaviour {
	private float life = 0;

	void Update () {
		// fade out over time
		var material = GetComponent<Renderer>().material;

		material.SetColor("_EmissionColor", Color.Lerp(Color.white * 2, Color.black,life));
		
		life += Time.deltaTime * 2 ;

		transform.localScale *= 1-Time.deltaTime;

		// destroy if old enough
		if (life > .9){
			GameObject.Destroy(gameObject);
		}
	}
}
