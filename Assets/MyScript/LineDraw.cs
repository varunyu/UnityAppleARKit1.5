using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw : MonoBehaviour {
	private LineRenderer line;
	private Vector3 mousePos;
	//public GameObject drawingPad;
	// Use this for initialization
	void Start () {
		line = GetComponent<LineRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButton(0)){

			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			foreach (RaycastHit hit in Physics.RaycastAll(ray)){
				if(hit.collider.tag.Equals("DrawUI")){
					//print("Hit");
					mousePos = gameObject.transform.InverseTransformPoint(hit.point);
					//mousePos.z = 0;

					//mousePos = hit.point;
					line.positionCount++;
					line.SetPosition (line.positionCount - 1, mousePos);
				}
			}
			/*
			mousePos = Input.mousePosition;
			Debug.Log (" "+Input.mousePosition);
			line.positionCount++;
			line.SetPosition (line.positionCount - 1, mousePos);*/

		} 
		else{
			line.Simplify (0.01f);
			enabled = false;
		}
	}
}
