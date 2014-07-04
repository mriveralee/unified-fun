using UnityEngine;
using System.Collections;

public class MoveRight : MonoBehaviour {
	public AudioClip mCompleteSound;
	public Vector3 mMoveSpeed;

	private bool mIsMoving = false;
	private GameObject[] mScene;
	private GameObject mBackground;

	private GameObject[] mButtons;
	private GameObject mCompleteText;

	private bool mHasEnded = false;
	public Font mGoodDog;

	
	
	// Use this for initialization
	void Start () {
		mIsMoving = false;
		mHasEnded = false;
		mMoveSpeed = new Vector3(0.1f, 0f, 0f);
		mScene = GameObject.FindGameObjectsWithTag("Moveable");
		mBackground = GameObject.Find("Background");
		mButtons = GameObject.FindGameObjectsWithTag("Buttons");
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
			} else if (Input.GetMouseButtonUp(0)) {
				touchPhase = TouchPhase.Ended;
			}
			touchPos = Input.mousePosition;
			hasTouch = true;
		}
		// Check the touch 
		if (hasTouch) CheckTouch(touchPos, touchPhase);
		
		if (mIsMoving && mBackground.transform.position.x > -4.8f) {
			for (int i = 0; i < mScene.Length; i++) {
				GameObject s = mScene[i];
				if (s == null) continue;
				s.transform.position += mMoveSpeed;
				if (s.transform.position.x < -6.6f) 
					s.transform.position.Set(-6.6f, s.transform.position.y, s.transform.position.z);
			}
		}

		// Level Completed
		if (mBackground.transform.position.x <= -4.8f && !mHasEnded) {
			Alert("complete");
		}
		
	}
	
	/* Check the current touch coordinate on the screen
	 * @param pos - the position touched
	 * @param phase - the phase of 
	 */ 
	void CheckTouch (Vector3 pos, TouchPhase phase) {
		if (phase != TouchPhase.Began && phase != TouchPhase.Ended) {
			return;
		}
		
		// Get world position of the hit point
		Vector3 wp = Camera.main.ScreenToWorldPoint(pos);
		Vector2 touchPos = new Vector2(wp.x, wp.y);
		Collider2D hitPt = Physics2D.OverlapPoint(touchPos);

		if (hitPt == null) {
			return;
		}

		if (hitPt.gameObject.name == "RightDirectionButton") {
			if (phase == TouchPhase.Began) mIsMoving = true;
			else if (phase == TouchPhase.Ended) mIsMoving = false;
		}
	}

	/*
	 * Restart the Game 
	 */
	void Restart () {
		Application.LoadLevel(Application.loadedLevel);
	}

	/*
	 * Alert game completed
	 */
	public void Alert (string action) {
		mHasEnded = true;

		mCompleteText = new GameObject();
		mCompleteText.AddComponent("GUIText");
		mCompleteText.guiText.font = mGoodDog;
		mCompleteText.guiText.fontSize = 50;
		mCompleteText.guiText.color = new Color(255, 0, 0);

		if (action == "complete") {
			AudioSource.PlayClipAtPoint(mCompleteSound, transform.position);
			mCompleteText.guiText.text = "Level Complete!";
			mCompleteText.guiText.transform.position = new Vector3(0.24f, 0.88f, 0);
		} else {
			mCompleteText.guiText.text = "Game Over!";
			mCompleteText.guiText.transform.position = new Vector3(0.36f, 0.88f, 0);
		}

		//mBackground.sStop();

		for (int i = 0; i < mButtons.Length; i++) {
			mButtons[i].renderer.enabled = false;
			Invoke("Restart", 2);
		}
	
	}
}
