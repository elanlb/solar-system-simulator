using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowTip : MonoBehaviour {

	public Transform Controller;
	// Use this for initialization
	void Start () {
		print("ThrowTip : Start");
		transform.SetParent(Controller);
		transform.localPosition = new Vector3(0,0,0);
	}
}
