using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace UnityEngine.XR.iOS
{
	public class ARKitHitCheck : MonoBehaviour {

		Vector2 screenPosition;
		private Transform tmpTransform;
		private GameObject empGO;

	    // Use this for initialization
		void Start () {
			empGO = new GameObject ();
		}
	
		// Update is called once per frame
		void Update () {
			
		}
		void Awake(){
		
		}


		public Transform HitLoc(Vector2 tPosition){
			tmpTransform = empGO.transform;
			screenPosition = Camera.main.ScreenToViewportPoint(tPosition);
			ARPoint point = new ARPoint {
				x = screenPosition.x,
				y = screenPosition.y
			};

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
					//Debug.Log ("Hit ST!");
					return tmpTransform;

				}
			}
			Debug.Log ("DON'T HIT");
			return null;
		}

		public Transform HitLoc(Touch touch){
			tmpTransform = empGO.transform;
			screenPosition = Camera.main.ScreenToViewportPoint(touch.position);
			ARPoint point = new ARPoint {
				x = screenPosition.x,
				y = screenPosition.y
			};

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
					//Debug.Log ("Hit ST!");
					return tmpTransform;

				}
			}
			Debug.Log ("DON'T HIT");
			return null;
		}

		private bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					//Debug.Log ("Got hit!");
					tmpTransform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					tmpTransform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
					//DrawingPad.transform.parent = gameObject.transform;
					//DrawingPad.transform.position = UnityARMatrixOps.GetPosition (hitResult.worldTransform);
					//DrawingPad.transform.rotation = UnityARMatrixOps.GetRotation (hitResult.worldTransform);
					//Debug.Log (string.Format ("x:{0:0.######} y:{1:0.######} z:{2:0.######}", DrawingPad.transform.position.x, DrawingPad.transform.position.y, DrawingPad.transform.position.z));
					return true;
				}
			}
			return false;
		}


	}
}
