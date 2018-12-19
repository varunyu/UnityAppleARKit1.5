using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace UnityEngine.XR.iOS
{
	public class ApplicationManager : MonoBehaviour {


		public float maxRayDistance = 300.0f;
		public LayerMask collisionLayer = 1 << 10;

		[SerializeField] public GameObject line;
		public GameObject DrawingPad;
		public GameObject Ref;
		private enum DrawingState
		{
			Drawing,
			None
		}

		private DrawingState dState ;
		// Use this for initialization
		void Start () {
			dState = DrawingState.Drawing;
		}

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					Debug.Log ("Got hit!");
					DrawingPad.transform.parent = gameObject.transform;
						DrawingPad.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
						DrawingPad.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
					Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", DrawingPad.transform.position.x, DrawingPad.transform.position.y, DrawingPad.transform.position.z));
					return true;
				}
			}
			return false;
		}

		void Awake (){
			//DrawPadPos = DrawingPad.transform;
			//DrawingPad.transform.position = Camera.main.transform.TransformPoint(Ref.transform.position);
			//DrawingPad.transform.rotation = new Quaternion (0.0f, Camera.main.transform.rotation.y, 0.0f, Camera.main.transform.rotation.w);
			dState = DrawingState.Drawing;
			Reset ();
		}

		public void Rotate(){
			DrawingPad.transform.Rotate (90f,0f,0f);
		}

		public void Reset(){

			//GameObject[] Children = GetComponentInChildren();
			foreach(Transform child in DrawingPad.transform){
				Destroy (child.gameObject);
			}
			DrawingPad.transform.SetParent (Camera.main.transform);
			DrawingPad.transform.position = Ref.transform.position;
			DrawingPad.transform.rotation = Ref.transform.rotation;
			//DrawingPad.transform.localScale = Ref.localScale;
			dState = DrawingState.Drawing;
		}

		public void Done(){
			DrawingPad.transform.parent = null;
			dState = DrawingState.None;
			Debug.Log ("None");
		}
		// Update is called once per frame
		void Update () {
			
			#if UNITY_EDITOR   //we will only use this script on the editor side, though there is nothing that would prevent it from working on device
			if (Input.GetMouseButtonDown (0)) {

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			//RaycastHit hit;

			foreach (RaycastHit hit in Physics.RaycastAll(ray)){
				if(hit.collider.tag.Equals("DrawUI")){
					print("Hit");
				Instantiate(line, Input.mousePosition, Quaternion.Euler(0.0f,0.0f,0.0f),DrawingPad.transform);
				}

			}
			}
			
			#else
			if(Input.touchCount > 0)
			{
				var touch = Input.GetTouch(0);
				if(touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) ){

					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);


					if(dState == DrawingState.Drawing){
							Ray ray = Camera.main.ScreenPointToRay(touch.position);

							foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
								if (hit.collider.tag.Equals ("DrawUI")) {
								
									Instantiate (line, hit.point, Quaternion.Euler (0.0f, 0.0f, 0.0f), DrawingPad.transform);
								}
							}

					}
					else if(dState == DrawingState.None){
					
							//if(touch.phase == TouchPhase.Moved){
								ARPoint point = new ARPoint {
									x = screenPosition.x,
									y = screenPosition.y
								};

								// prioritize reults types
								ARHitTestResultType[] resultTypes = {
									ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingGeometry,
									ARHitTestResultType.ARHitTestResultTypeExistingPlaneUsingExtent, 
									// if you want to use infinite planes use this:
									ARHitTestResultType.ARHitTestResultTypeExistingPlane,
									ARHitTestResultType.ARHitTestResultTypeEstimatedHorizontalPlane, 
									ARHitTestResultType.ARHitTestResultTypeEstimatedVerticalPlane, 
									ARHitTestResultType.ARHitTestResultTypeFeaturePoint
								}; 

								foreach (ARHitTestResultType resultType in resultTypes)
								{
									if (HitTestWithResultType (point, resultType))
									{
										return;
									}
								}
							//}
							
					}
				}

			}
			#endif
	

		}
	}
}
