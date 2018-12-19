using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo : MonoBehaviour {


	private Vector3 initCam;
	private Vector3 initPos;
	private string text;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Vector3 GetInitCam(){
		return initCam;
	}
	public void SetInitCam(Vector3 pos){
		initCam = pos;
	}

	public Vector3 GetInitPos(){
		return initPos;
	}
	public void SetInitPos(Vector3 pos){
		initPos = pos;
	}
}
