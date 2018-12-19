using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class HybridController : MonoBehaviour {

	private GameObject SelectedObject;
	public Image target;

	private float minPitcgDis = 15f;
	private float minAngle = 10f;

	private Vector2 t1PrevPos;
	private Vector2 t2PrevPos;
	private Vector2 prevDir;
	private Vector2 currentDir;

	float prevMagnitude;
	float cMagnitude;
	float diffMagnitude;



	private Vector3 rayEnd;
	private float rayDis;
	private Ray ray;
	private bool setUp;
	private ObjectController OBJCScript;

	private bool startCheck;
	private GameObject targetObject;

	public Text timerText;
	private float timerCount;
	private int count;

	// Use this for initialization
	void Start () {
		
		OBJCScript = (ObjectController)gameObject.GetComponent(typeof(ObjectController));
	}
	void OnEnable(){
		Debug.Log ("Wake up");
		setUp = false;
		startCheck = false;
		count = 0;
		timerCount = 0;
	}


	// Update is called once per frame
	void Update () {

		/*
		if (SelectedObject == null) {
			return;
		}*/
		/*
		if (startCheck) {
			timerCount += Time.deltaTime;
		}
		*/
		if (SelectedObject != null) {
			DeviceMovement ();

			if(startCheck){
				timerCount += Time.deltaTime;
				TestChecking ();
				if(count >= 4){
					TriggerChecking (false);
				}
			}
		}

		if (Input.touchCount > 0) {

			var touch = Input.GetTouch (0);

			if (EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
				return;
			}

			if (setUp) {
				//Debug.Log("SETUP YESSSS  :"+setUp.ToString());
				if (SelectedObject == null) {
					return;
				}

				if (Input.touchCount == 1) {
					Debug.Log ("1 Touch");

					if (touch.phase == TouchPhase.Moved) {
						if (Mathf.Abs (touch.deltaPosition.x) >= 2.3f) {
							SelectedObject.transform.RotateAround (SelectedObject.transform.position, Camera.main.transform.up,
								touch.deltaPosition.x * 17.0f * Time.deltaTime * -1);
						}

						if (Mathf.Abs (touch.deltaPosition.y) >= 2.3f) {
							//selectedObject.transform.Rotate (Camera.main.transform.up,touch.deltaPosition.y*rotaSpeed*Time.deltaTime);
							SelectedObject.transform.RotateAround (SelectedObject.transform.position, Camera.main.transform.right,
								touch.deltaPosition.y * 17.0f * Time.deltaTime);
						}

					}


				} else if (Input.touchCount == 2) {
					var touch2 = Input.GetTouch (1);

					 t1PrevPos = touch.position - touch.deltaPosition;
					t2PrevPos = touch2.position - touch2.deltaPosition;

					prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
					cMagnitude = (touch.position - touch2.position).magnitude;

					diffMagnitude = (prevMagnitude - cMagnitude);
					//print (diffMagnitude);

					if (Mathf.Abs (diffMagnitude) >= minPitcgDis) {
						rayDis += diffMagnitude * 0.0009f;
						//Pitch finger
					} else {

						prevDir = t2PrevPos - t1PrevPos;
						currentDir = touch2.position - touch.position;
						float angle = Vector2.Angle (prevDir, currentDir);
						Vector3 LR = Vector3.Cross (prevDir, currentDir);
						//if (Mathf.Abs(angle) >= minAngle) {

						if (LR.z > 0) {
							SelectedObject.transform.RotateAround (SelectedObject.transform.position, Camera.main.transform.forward,
								angle*8f);
						} else {
							SelectedObject.transform.RotateAround (SelectedObject.transform.position, Camera.main.transform.forward,
								-angle*8f);
						}
					}

				}
			} else if(!setUp){
				if (touch.phase == TouchPhase.Began) {
					OBJCScript.CreateObjectNotSlidAR(touch);
					//GameObject tmptargetObject = OBJCScript.ReturnCreatedObject ();
					targetObject = OBJCScript.ReturnCreatedObject ().transform.GetChild (0).gameObject;
					//SetupState (true);
					//Debug.Log("SETUP NOOOO :"+setUp.ToString());
					setUp = true;
				}
			}

		}

	}
	/*
	public void SetupState(bool b){
		setUp = b;
	}*/

	public void SetSelectedObject(GameObject go){
		SelectedObject = go;

	}
	public void ClearSelectedObject(){
		SelectedObject = null;
	}

	public void PickUpObject(){
		//Debug.Log ("HELLO");
		if (SelectedObject == null) {
			ray = Camera.main.ScreenPointToRay (target.transform.position);

			foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
				if (hit.collider.tag.Equals ("3DModel")) {
					Debug.Log ("HIT AT POS:  "+ hit.collider.gameObject.transform.position);
					rayDis = Vector3.Distance (Camera.main.transform.position, hit.collider.gameObject.transform.position);
					SetSelectedObject (hit.collider.gameObject);
					break;
				}
			}
			return;
		} else {
			ClearSelectedObject ();
		}
	
	}

	public void DeviceMovement(){
		var tmpray = Camera.main.ScreenPointToRay (target.transform.position);
		SelectedObject.transform.position = tmpray.GetPoint (rayDis);

	}

	public void TriggerChecking(bool b){
		startCheck = b;
		if (b) {
			InvokeRepeating ("UpdateTimer",0.00f,0.05f);
		} else if (!b) {
			CancelInvoke ("UpdateTimer");
		}
	}

	private float angDiff;
	private float disDiff;

	public void TestChecking(){
		angDiff = Quaternion.Angle (SelectedObject.transform.rotation,targetObject.transform.rotation);
		disDiff = Vector3.Distance (SelectedObject.transform.position,targetObject.transform.position);

		if(angDiff<=12.0f /*&& disDiff<= 0.01f*/){
			Destroy (SelectedObject);
			count++;
		}
	}
	private void UpdateTimer(){
		timerText.text = timerCount.ToString () + " s";
	}
}
