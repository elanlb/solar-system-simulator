using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PlanetThrow : MonoBehaviour {

	public GameObject[] planets;
	public Rigidbody attachPoint;
	

	SteamVR_TrackedObject trackedObj;
	FixedJoint joint;

	private int index = 0;

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void FixedUpdate()
	{
		var device = SteamVR_Controller.Input((int)trackedObj.index);
		if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			// spawn new object at controller

			var go = GameObject.Instantiate(planets[index]);
			index = (index +1)%planets.Length;
			go.transform.position = attachPoint.transform.position;
			go.transform.localScale = new Vector3(.6f,.6f,.6f);

			joint = go.AddComponent<FixedJoint>();
			joint.connectedBody = attachPoint;
		}
		else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
		{
			// release object & throw

			var go = joint.gameObject;
			var rigidbody = go.GetComponent<Rigidbody>();
			Object.DestroyImmediate(joint);
			joint = null;

			// We should probably apply the offset between trackedObj.transform.position
			// and device.transform.pos to insert into the physics sim at the correct
			// location, however, we would then want to predict ahead the visual representation
			// by the same amount we are predicting our render poses.

			var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
			if (origin != null)
			{
				rigidbody.velocity = origin.TransformVector(device.velocity);
				rigidbody.angularVelocity = origin.TransformVector(device.angularVelocity);
			}
			else
			{
				rigidbody.velocity = device.velocity;
				rigidbody.angularVelocity = device.angularVelocity;
			}

			rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
			rigidbody.useGravity = false;
			rigidbody.mass = go.GetComponent<Planet>().mass;

			go.GetComponent<TrailRenderer>().enabled = true;

			go.AddComponent<OrbitalMotion>();
		}
	}
}
