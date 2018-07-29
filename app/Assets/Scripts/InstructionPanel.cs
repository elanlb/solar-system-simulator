using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour {

    private InterfaceAnimManager animManager;
	private Vector3 panelP;
    public AudioSource audioSource;

	private bool active = false;
	private float closeTime = -1000;

	// Use this for initialization
	void Start () {
		animManager = transform.GetChild(0).GetComponent<InterfaceAnimManager>();
		panelP = transform.position;
		audioSource = animManager.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		var c2p = panelP - Camera.main.transform.position;
		var dot = Vector3.Dot(c2p.normalized, Camera.main.transform.forward);
		
		if (!active && (Time.time - closeTime > 2) && dot > .65f){
			// activate panel
			active = true;
			animManager.startAppear();
			animManager.gameObject.SetActive(true);
			playSound();
		}
		else if (active && dot < .5f){
			// close panel
			active = false;
			animManager.startDisappear();
			closeTime = Time.time;
		}
		
	}
    private void playSound() {
        if (audioSource && audioSource.enabled)
            audioSource.Play();
    }
}
