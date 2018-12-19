using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlidARPPController : MonoBehaviour {

	public enum AppState{
		NONE,
		ADD,
		AUTORING,
		EDIT,
		SLIDAR,
		ROTATION
		/*
		 NONE =0
		 ADD =1
		 AUTORING =2
		 EDIT =3
		 SLIDAR =4
		 ROTATION =5
		 * */
	}
	private AppState currState;
	private GameObject sObject;
	private TranslationAndIntial traAIni;
	private OrientationControl orienCont;
	private SlidARScript slidAR;
	private ObjectInfo oInfo;

	private bool isObjectVerticalToG;
	//private int arObjectIndex;

	public GameObject[] arObjectList;
	private GameObject tmp;
	public GameObject confirmButton;
	public GameObject editPannel;

	void Awake(){
		currState = AppState.NONE;
		traAIni = (TranslationAndIntial)gameObject.GetComponent(typeof(TranslationAndIntial));
		orienCont = (OrientationControl)gameObject.GetComponent(typeof(OrientationControl));
		slidAR = (SlidARScript)gameObject.GetComponent (typeof(SlidARScript));
		isObjectVerticalToG = true;
	}
	// Use this for initialization
	void Start () {
		
	}

	public void SetIsObjectVerticalToG(bool t){
		isObjectVerticalToG = t;
	}

	public void SelectObjectToCreate(int i){
		tmp = arObjectList [i];
	}

	// Update is called once per frame

	private Touch touch1;
	private Touch touch2;
	private int nFinger;
	private Vector2 t1PrevPos;
	private Vector2 t2PrevPos;
	private Vector2 prevDir;
	private Vector2 currentDir;
	float prevMagnitude;
	float cMagnitude;
	float diffMagnitude;
	private float minPitcgDis = 30f;



	void Update () {

		nFinger = Input.touchCount;
		if (nFinger > 0) {

			touch1 = Input.GetTouch (0);

			if (EventSystem.current.IsPointerOverGameObject (touch1.fingerId)) {
				return;
			}

			switch (currState) {
			case AppState.NONE:
				{
					if (touch1.phase == TouchPhase.Began) {
						if (GetSelectedObject (touch1)) {
							editPannel.SetActive (true);
							ChangeState (3);
						}

					}
					break;
				}
			case AppState.ADD:
				{
					if (touch1.phase == TouchPhase.Began) {
						sObject = traAIni.ObjectInstantiate (touch1,tmp);
						oInfo = (ObjectInfo)sObject.GetComponent(typeof(ObjectInfo));
						SetInitialOrientation ();
						confirmButton.SetActive (true);
						ChangeState (2);
					}
					break;
				}
			case AppState.AUTORING:
				{
					if (touch1.phase == TouchPhase.Began || touch1.phase == TouchPhase.Moved) {
						var tmpTouch = touch1.position;
						tmpTouch.y += 100;
						sObject.transform.position = traAIni.GetRealWorldPos (tmpTouch);
					}
					break;
				}
			case AppState.EDIT:
				{
					
					break;
				}
			case AppState.SLIDAR:
				{
					if (nFinger == 1) {
						if (touch1.phase == TouchPhase.Began || touch1.phase == TouchPhase.Moved) {
							sObject.transform.position = slidAR.SlidAR (touch1.position);
						}
					} else {
						slidAR.ShowSlidARLine(false);
						if (touch1.phase == TouchPhase.Began || touch1.phase == TouchPhase.Moved) {
							var tmpTouch = touch1.position;
							tmpTouch.y += 100;
							sObject.transform.position = traAIni.GetRealWorldPos (tmpTouch);

						}else if(touch1.phase == TouchPhase.Ended){
							SaveInitialData ();
							PrepareSlidARData ();
							slidAR.ShowSlidARLine (true);
						}
					}

					break;
				}
			case AppState.ROTATION:
				{
					if (nFinger == 1) {

						sObject.transform.RotateAround (sObject.transform.position,orienCont.GetGravityVector(),
							touch1.deltaPosition.x * 17.0f * Time.deltaTime);
						/*
						if (Mathf.Abs (touch1.deltaPosition.x) > Mathf.Abs (touch1.deltaPosition.y)) {
							sObject.transform.Rotate (Camera.main.transform.up,orienCont.ARCBALLX(touch1));
						} else {
							sObject.transform.Rotate (Camera.main.transform.right, orienCont.ARCBALLY (touch1));
						}*/
					} else {
						touch2 = Input.GetTouch (1);

						//Debug.Log ("2Finger distance: "+ Mathf.Abs(Vector2.Distance(touch1.position,touch2.position) ));

						if (touch1.phase == TouchPhase.Moved) {

							if (Mathf.Abs (Vector2.Distance (touch1.position, touch2.position)) <= 270f) {


								if (Mathf.Abs (touch1.deltaPosition.x) >= 2.3f) {
									sObject.transform.RotateAround (sObject.transform.position, Camera.main.transform.up,
										touch1.deltaPosition.x * 17.0f * Time.deltaTime * -1);
								}

								if (Mathf.Abs (touch1.deltaPosition.y) >= 2.3f) {
									//selectedObject.transform.Rotate (Camera.main.transform.up,touch.deltaPosition.y*rotaSpeed*Time.deltaTime);
									sObject.transform.RotateAround (sObject.transform.position, Camera.main.transform.right,
										touch1.deltaPosition.y * 17.0f * Time.deltaTime);
								}
							} else {


								t1PrevPos = touch1.position - touch1.deltaPosition;
								t2PrevPos = touch2.position - touch2.deltaPosition;

								prevMagnitude = (t1PrevPos - t2PrevPos).magnitude;
								cMagnitude = (touch1.position - touch2.position).magnitude;

								diffMagnitude = (prevMagnitude - cMagnitude);
								//print (diffMagnitude);

								if (Mathf.Abs (diffMagnitude) >= minPitcgDis) {
									Debug.Log ("Scale : "+ (diffMagnitude * 0.003f));
									var tmpScale = sObject.transform.localScale;
									tmpScale.x+=(diffMagnitude * 0.003f);
									tmpScale.y+=(diffMagnitude * 0.003f);
									tmpScale.z+=(diffMagnitude * 0.003f);

									sObject.transform.localScale = tmpScale;
									//sObject.transform.localScale *= diffMagnitude * 0.00005f;
									//Pitch finger
								} else {
									prevDir = t2PrevPos - t1PrevPos;
									currentDir = touch2.position - touch1.position;

									sObject.transform.Rotate (Camera.main.transform.forward, orienCont.ZTwistGesture (prevDir, currentDir));
								}
							}
						}


					}

					break;
				}
			default:
				break;
			}
		}

	}

	private void SetInitialOrientation(){
		sObject.transform.rotation = Quaternion.FromToRotation (Vector3.down,orienCont.GetGravityVector());
		if (!isObjectVerticalToG) {
			sObject.transform.Rotate (Vector3.right, 90f);
		} 
	}

	public void SaveInitialData(){
		//Debug.Log (Camera.main.transform.position);
		//Debug.Log (sObject.transform.position);
		oInfo.SetInitCam (Camera.main.transform.position);
		oInfo.SetInitPos (sObject.transform.position);

	}

	public void PrepareSlidARData(){
		slidAR.SetTmpCamPos (oInfo.GetInitCam());
		slidAR.SetTmpAnnoPos (oInfo.GetInitPos ());

	}

	public void ChangeState(int i){
		currState = (AppState)i;
	}
	/*
	private void ChangeRotation(Transform t){
		sObject.transform.rotation = t.rotation;
	}*/

	private Ray ray;
	private bool GetSelectedObject(Touch t){
		ray = Camera.main.ScreenPointToRay (t.position);
		foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
			if (hit.collider.tag.Equals ("3DModel") || hit.collider.tag.Equals ("Annotation")) {
				Debug.Log ("HIT !!!! :  "+ hit.collider.tag);
				//OBJCScript.SetSelectedObject (hit.collider.gameObject);	
				//ChangeAppState (2);
				sObject = hit.collider.gameObject;
				oInfo = (ObjectInfo)sObject.GetComponent(typeof(ObjectInfo));
				PrepareSlidARData ();

				return true;
			}
		}
		return false;
	}

	public void RemoveOjbect(){
		Destroy (sObject);
	}
}
