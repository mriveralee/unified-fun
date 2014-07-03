﻿using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

	public float mJumpForce;
	private GameObject mHero;



	// Use this for initialization
	void Start () {
		// Find Hero
		mHero = GameObject.Find("Hero");
	}
	
	// Update is called once per frame
	void Update () {
		bool hasTouch = false;
		Vector2 touchPos;
		TouchPhase touchPhase = TouchPhase.Canceled;
		if ((Application.platform == RuntimePlatform.IPhonePlayer
		     || Application.platform == RuntimePlatform.Android)
		    && Input.touchCount > 0) {
			Touch currentTouch = Input.GetTouch(0);
			touchPos = currentTouch.position;
			touchPhase = currentTouch.phase;
			hasTouch = true;
		}
		else if (Application.platform == RuntimePlatform.OSXEditor) {
			if (Input.GetMouseButtonDown(0)) {
			    touchPhase = TouchPhase.Began;
			} else if (Input.GetMouseButtonUp(0)) {
				touchPhase = TouchPhase.Ended;
			}
			touchPos = Input.mousePosition;
			hasTouch = true;
		}
		// Check the touch 
		if (hasTouch) CheckTouch(touchPos, touchPhase);
	
	}


	/* Check the current touch coordinate on the screen
	 * @param pos - the position touched
	 * @param phase - the phase of 
	 */ 
	void CheckTouch (Vector3 pos, TouchPhase phase) {
		if (phase != TouchPhase.Began || !pos) {
			return;
		}

		// Get world position of the hit point
		Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		Collider2D hitPt = Physics2D.OverlapPoint(touchPos);

		// Check if we have a hit point
		if (hitPt && hitPt.gameObject.name == "JumpButton") {
			mHero.rigidbody2D.AddForce(new Vector2(0f, mJumpForce));
			audio.Play();
		}
	}

}
