using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.XR.iOS
{
	public class SlidARPlusController : MonoBehaviour {

		private GameObject selectedGameObject;
		private SlidARState sARState;
		private GameObject createObject;
		private enum SlidARState
		{
			Create,
			None,
			Edit,
			Adjust
		}


		private ARPoint point;


		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					Debug.Log ("Got hit!");
					selectedGameObject.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					selectedGameObject.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);

					return true;
				}
			}
			return false;
		}

		// Use this for initialization
		void Start () {
			sARState = SlidARState.None;

		}
	
		// Update is called once per frame
		void Update () {

			if(Input.touchCount > 0){
				var touch = Input.GetTouch(0);


				if ((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) && !EventSystem.current.IsPointerOverGameObject (touch.fingerId)) {
					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
					Ray ray = Camera.main.ScreenPointToRay(touch.position);

					point.x = screenPosition.x;
					point.y = screenPosition.y;

					switch(sARState){
					case SlidARState.None:
						{
							if(touch.phase == TouchPhase.Began){
								

								foreach (RaycastHit hit in Physics.RaycastAll(ray)) {
									if (hit.collider.tag.Equals ("Annotation")) {

										selectedGameObject = hit.collider.gameObject;
									}
								}
							}
							break;
						}
					case SlidARState.Edit:
						{
							break;
						}
					case SlidARState.Create:
						{
							if(touch.phase == TouchPhase.Began && ReadyToCreate()){

								Instantiate (createObject,new Vector3(0,0,0),Quaternion.Euler (0.0f, 0.0f, 0.0f),gameObject.transform);
								selectedGameObject = createObject;
								SetPosition ();
								createObject = null;
								sARState = SlidARState.Adjust;
							}
							break;
						}
					case SlidARState.Adjust:
						{
							if(IsObjectSelected()){
								SetPosition ();
							}

							break;
						}
					}
				}
			}

		}
		private bool IsObjectSelected(){
			if (selectedGameObject != null) {
				return true;
			} 
			else {
				return false;
			}
		}
		private bool ReadyToCreate(){
			if(createObject!= null){
				return true;
			}
			return false;
		}
		public void RemoveObject(){
			if(IsObjectSelected()){
				Destroy (selectedGameObject);
				selectedGameObject = null;
			}
		}
		public void ChangeState(int i){
			switch (i) {
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			}
		}
		private void SetPosition(){
			
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
		}



		////SlidAR Script
		/// 
		/// 


	}
}
