using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnityEngine.XR.iOS
{
	public class FreeLine : MonoBehaviour {


		public float maxRayDistance = 300.0f;
		public LayerMask collisionLayer = 1 << 10;
		private LineRenderer line;
		// Use this for initialization
		void Start () {
			line = GetComponent<LineRenderer> ();
		}
	
		// Update is called once per frame
		void Update () {

			if(Input.touchCount > 0)
			{
				var touch = Input.GetTouch(0);
				if((touch.phase == TouchPhase.Began || touch.phase == TouchPhase.Moved) && !EventSystem.current.IsPointerOverGameObject(touch.fingerId) ){

					var screenPosition = Camera.main.ScreenToViewportPoint(touch.position);

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
				
				
				}

			}
			else{
				line.Simplify (0.01f);
				enabled = false;
			}

		}

		bool HitTestWithResultType (ARPoint point, ARHitTestResultType resultTypes)
		{
			List<ARHitTestResult> hitResults = UnityARSessionNativeInterface.GetARSessionNativeInterface ().HitTest (point, resultTypes);
			if (hitResults.Count > 0) {
				foreach (var hitResult in hitResults) {
					Debug.Log ("Got hit!");
					line.positionCount++;
					line.SetPosition (line.positionCount - 1, UnityARMatrixOps.GetPosition (hitResult.worldTransform));

				return true;
				}
			}
			return false;
		}
	}
}
