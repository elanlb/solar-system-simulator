﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SteamVR_TrackedObject))]
public class PlanetThrow : MonoBehaviour {

	public GameObject[] planets;
	public Rigidbody attachPoint;

	public GameObject VelocityText; // will be instantiated and modified to display speed
	public Text PlanetText;
	

	SteamVR_TrackedObject trackedObj;
	FixedJoint joint;

	private int index = 0;
	private float planetChangeTimer = 0;

	void Awake()
	{
		trackedObj = GetComponent<SteamVR_TrackedObject>();
	}

	void FixedUpdate()
	{
		var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
        {
            Vector2 touch = (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis0));
            if (touch.y >= 0.5f) nextPlanet();
            else if (touch.y <= -0.5f) previousPlanet();
        }
        if (joint == null && device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
		{
			// spawn new object at controller

			var go = GameObject.Instantiate(planets[index]);

			go.transform.position = attachPoint.transform.position;
			
			joint = go.AddComponent<FixedJoint>();
			joint.connectedBody = attachPoint;
		}
		else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
		{
			// release object & throw (called once per throw)

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

			// get and show the velocity of the new object
			float velocity = rigidbody.velocity.magnitude;
			GameObject velocityText = Instantiate(VelocityText);
			velocityText.transform.position = transform.position;

			velocityText.transform.LookAt(Camera.main.transform); // look at the player
			velocityText.transform.eulerAngles += new Vector3(0, 180, 0);

			velocityText.GetComponent<TextMeshPro>().text = "Velocity:\n" + velocity.ToString("0.00");

			// destroy after a certain amount of time
			GameObject.Destroy(velocityText, 2.0f);
		}

		// switch planets
		planetChangeTimer += Time.deltaTime;

        if (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis2).y < -0.5f) {
            previousPlanet();
		} else if (device.GetAxis(Valve.VR.EVRButtonId.k_EButton_Axis2).y > 0.5f) {
            nextPlanet();
		}
		
	}

    void previousPlanet()
    {
        if (planetChangeTimer <= 0.3f) return;

        index = index - 1;
        if (index < 0)
        {
            index = planets.Length;
        }

        PlanetText.text = planets[index].name;

        planetChangeTimer = 0;
    }

    void nextPlanet()
    {
        if (planetChangeTimer <= 0.3f) return;

        index = (index + 1) % planets.Length;

        PlanetText.text = planets[index].name;

        planetChangeTimer = 0;
    }
}
