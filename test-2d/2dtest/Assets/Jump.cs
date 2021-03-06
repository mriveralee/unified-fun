﻿using UnityEngine;
using System.Collections;

public class Jump : MonoBehaviour {

	public float mJumpForce;
	private GameObject mHero;
	private float mDoubleJumpTime;
	public float mJumpWaitTime;
	private float mNextJumpTime;
	private float mNextDoubleJumpTime;



	// Use this for initialization
	void Start () {
		// Find Hero
		mHero = GameObject.Find("Hero");
		mJumpForce = 200f;
		mJumpWaitTime = 0.78f;
		mDoubleJumpTime = 0.5f;
		mNextJumpTime = 0f;
		mNextDoubleJumpTime = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		bool hasTouch = false;
		Vector3 touchPos = new Vector3();
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
				hasTouch = true;
			} else if (Input.GetMouseButtonUp(0)) {
				touchPhase = TouchPhase.Ended;
				hasTouch = true;
			}
			touchPos = Input.mousePosition;
		}
		// Check the touch 
		if (hasTouch) CheckTouch(touchPos, touchPhase);
	
	}


	/* Check the current touch coordinate on the screen
	 * @param pos - the position touched
	 * @param phase - the phase of 
	 */ 
	void CheckTouch (Vector3 pos, TouchPhase phase) {
		if (phase != TouchPhase.Began && phase != TouchPhase.Ended) {
			return;
		} if (phase == TouchPhase.Began && Time.time < mNextJumpTime) { 
			return;
		}

		// Get world position of the hit point
		Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		Collider2D hitPt = Physics2D.OverlapPoint(touchPos);

		if (hitPt == null) {
			return;
		}

		// Check if we have a hit point
		if (hitPt.gameObject.name == "JumpButton" && hitPt) {
			if (phase == TouchPhase.Began) {
				mHero.rigidbody2D.AddForce(new Vector2(0.0f, mJumpForce));
				audio.Play();
				mNextJumpTime = Time.time + mJumpWaitTime;
			} else if (phase == TouchPhase.Ended) {
				// Do nothing
			}
		}
	}

}
