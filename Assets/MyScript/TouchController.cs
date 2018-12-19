using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using UnityEngine.XR.iOS;

public class TouchController : MonoBehaviour {

	public enum AppState{
		NONE,
		ADD,
		EDIT
		/*
		 * NONE = 0
		 * ADD = 1
		 * EDIT = 2
		 * */
	}

	private AppState appState;
	//private ARKitHitCheck ARKitHitScript;
	private ObjectController OBJCScript;

	// Use this for initialization
	void Start () {
		
	}
	void Awake(){
		appState = AppState.NONE;
		//ARKitHitScript = (ARKitHitCheck)gameObject.GetComponent(typeof(ARKitHitCheck));
		OBJCScript = (ObjectController)gameObject.GetComponent(typeof(ObjectController));
	}

	public void ChangeAppState(int i){
		appState = (AppState)i;
		Debug.Log ("Change state " + appState);
	}
	 

	// Update is called once per frame
	void Update () {
		
		if (Input.touchCount > 0) {

			var touch = Input.GetTouch(0);

			if (EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
				return;
			}

			//if (!EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
				
				switch(appState){
				case AppState.NONE:
				{
						if (touch.phase == TouchPhase.Began) {
							Ray ray = Camera.main.ScreenPointToRay (touch.position);

							foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
								if (hit.collider.tag.Equals ("3DModel") || hit.collider.tag.Equals ("Annotation")) {
									Debug.Log ("HIT !!!! :  "+ hit.collider.tag);
									OBJCScript.SetSelectedObject (hit.collider.gameObject);	
									//ChangeAppState (2);
								}
							}
					

						}
						break;
				}	
				case AppState.ADD:
				{
					if (touch.phase == TouchPhase.Began) {
						//OBJCScript.CreateObject (ARKitHitScript.HitLoc(touch));
							OBJCScript.CreateObject(touch);
							//ChangeAppState (0);
					}
					break;
				}
				case AppState.EDIT:
				{
						if (touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) {
							OBJCScript.EditObject (touch);
						}
					else if(touch.phase == TouchPhase.Ended){
						OBJCScript.SetInitData ();
					}
						//OBJCScript.MoveObject (ARKitHitScript.HitLoc(touch));
					break;
				}
				default:
					break;
				
				}
			}
		//}
		/*
		if (Input.GetMouseButtonDown (0)) {
			if(EventSystem.current.IsPointerOverGameObject()){
				Debug.Log ("UI!!!");
				return;
			}
			Debug.Log ("NOT UI");
		}*/
	}


}

