using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRViewScript : MonoBehaviour {

	public GameObject line;
	public GameObject drawingObject;
	// Use this for initialization
	void Start () {
              
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetMouseButtonDown (0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			print (Camera.main.transform.ToString());
			//RaycastHit hit;

			foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
				if (hit.collider.tag.Equals ("DrawUI")) {
					print ("Hit");
					Instantiate (line, Input.mousePosition, Quaternion.Euler (0.0f, 0.0f, 0.0f), drawingObject.transform);
				}

			}
		}
		if(Input.GetMouseButton(1)){
			
		}
	}

	public void SetDawingTarget(GameObject obj){
		drawingObject = obj;
	}
	public void RemoveAllLine(){
		
		foreach (Transform child in drawingObject.transform) {
			Destroy (child.gameObject);
		}
	}
}
