using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class FreeDrawingController : MonoBehaviour {

	public GameObject freeLine;

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			var touch = Input.GetTouch (0);
			if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
				Instantiate (freeLine, new Vector3(0,0,0), Quaternion.Euler (0.0f, 0.0f, 0.0f), gameObject.transform);
			}
		}
	}

	public void RemoveAllLine(){
		foreach(Transform child in gameObject.transform){
			Destroy (child.gameObject);
		}
	}
}

