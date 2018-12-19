using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Annotation : MonoBehaviour {


	private Vector3 initCamPos;
	private Vector3 initObjectPos;
	private bool beSelected;


	void Awake(){
		Vector3 rotaGrav = GetGravityVector ();
		gameObject.transform.rotation = Quaternion.FromToRotation (Vector3.down,rotaGrav);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetCamPos(Vector3 cam){
		initCamPos = cam;
	}
	public Vector3 GetCamPos(){
		return initCamPos;
	}
	public void SetObjectPos(Vector3 obj){
		initObjectPos = obj;
	}
	public Vector3 GetObjectPos(){
		return initObjectPos;
	}
	public void Selected(bool select){
		beSelected = select;
		if (select) {
			gameObject.transform.GetChild (0).gameObject.SetActive (true);
			gameObject.transform.GetChild (1).gameObject.SetActive (false);
		} else {
			gameObject.transform.GetChild (0).gameObject.SetActive (false);
			gameObject.transform.GetChild (1).gameObject.SetActive (true);
		}
	}
	public bool BeingSelected(){
		return beSelected;
	}
	private Vector3 GetGravityVector(){
		Vector3 camAngle = Camera.main.transform.eulerAngles;

		Quaternion rotation = Quaternion.Euler (camAngle);
		Vector3 grav = new Vector3 (Input.acceleration.x,Input.acceleration.y,-Input.acceleration.z);


		Vector3 rotaGrav = rotation * grav;
		return rotaGrav;
	}
}
