using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCamMove : MonoBehaviour {

	public GameObject lookAtObject;
	// Use this for initialization
	void Start () {
		
	}
	public void SetLookAtObject(GameObject go){
		lookAtObject = go;
		this.gameObject.transform.LookAt (go.transform.position,Vector3.up);
	}
	public void ClearLookAtObject(){
		lookAtObject = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.W)) {
			this.gameObject.transform.RotateAround (lookAtObject.transform.position, this.gameObject.transform.right, 40 * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.S)) {
			this.gameObject.transform.RotateAround (lookAtObject.transform.position, this.gameObject.transform.right, -40 * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.A)) {
			this.gameObject.transform.RotateAround (lookAtObject.transform.position, Vector3.up, 40 * Time.deltaTime);
		}
		if (Input.GetKey (KeyCode.D)) {
			this.gameObject.transform.RotateAround (lookAtObject.transform.position, Vector3.up, -40 * Time.deltaTime);
		}
	}
}
