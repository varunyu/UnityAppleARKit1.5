using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class TranslationAndIntial : MonoBehaviour {

	// Use this for initialization
	private ARKitHitCheck ARKitHitScript;
	public GameObject parentObject;
	private Transform emptyTran;

	void Awake(){
		ARKitHitScript = (ARKitHitCheck)gameObject.GetComponent(typeof(ARKitHitCheck));
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public GameObject ObjectInstantiate(Touch t,GameObject prefab){
		emptyTran = ARKitHitScript.HitLoc (t);
		return (Instantiate (prefab, emptyTran.position, emptyTran.rotation, parentObject.transform));
	}

	public Vector3 GetRealWorldPos(Vector2 t){
		return ARKitHitScript.HitLoc (t).position;
	}

	public Vector3 GetRealWorldPos(Touch t){
		return ARKitHitScript.HitLoc (t).position;
	}

}
