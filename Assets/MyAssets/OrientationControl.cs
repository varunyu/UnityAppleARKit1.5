using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationControl : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public float ARCBALLX(Touch t){

		return (t.deltaPosition.x * 17.0f * Time.deltaTime * -1);
		
	}
	public float ARCBALLY(Touch t){
		return (t.deltaPosition.y * 17.0f * Time.deltaTime);
	}
	/*
	Vector2 t1PrevPos;
	Vector2 t2PrevPos;
	Vector2 prevDir;
	Vector2 currentDir;*/
	public float ZTwistGesture(Vector2 prevDir, Vector2 currentDir){
		/*
		t1PrevPos = finger1.position - finger1.deltaPosition;
		t2PrevPos = finger2.position - finger2.deltaPosition;

		prevDir = t2PrevPos - t1PrevPos;
		currentDir = finger2.position - finger1.position;*/
		float angle = Vector2.Angle (prevDir, currentDir);
		Vector3 LR = Vector3.Cross (prevDir, currentDir);

		if (LR.z > 0) {
			return (angle*8f);
		} else {
			return (-angle*8f);
		}
	}

	public float ZDisGesture(Touch finger1, Touch finger2){
		return 0f;
	}

	public Vector3 GetGravityVector(){
		Vector3 camAngle = Camera.main.transform.eulerAngles;

		Quaternion rotation = Quaternion.Euler (camAngle);
		Vector3 grav = new Vector3 (Input.acceleration.x,Input.acceleration.y,-Input.acceleration.z);


		Vector3 rotaGrav = rotation * grav;
		return rotaGrav;
	}

	public float HorizontalSwipe(Touch t){
		return (t.deltaPosition.x * 17.0f * Time.deltaTime * -1);
	}
}
