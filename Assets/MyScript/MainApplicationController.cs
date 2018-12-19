using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainApplicationController : MonoBehaviour {


	public GameObject freeDrawingController;
	public GameObject untitleProjectController;
	public GameObject slidARPlusController;
	public GameObject pannel1;
	public GameObject pannel2;
	public GameObject pannel3;
	// Use this for initialization
	void Start () {
		DeactiveAll ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void DeactiveAll(){
		freeDrawingController.SetActive (false);
		untitleProjectController.SetActive (false);
		slidARPlusController.SetActive (false);
		pannel1.SetActive (false);
		pannel2.SetActive (false);
		pannel3.SetActive (false);
	}

	public void FreeDrawing(){
		DeactiveAll ();
		freeDrawingController.SetActive (true);
		pannel1.SetActive (true);
	}
	public void UntitleProject(){
		DeactiveAll ();
		untitleProjectController.SetActive (true);
		pannel2.SetActive (true);
	}

	public void SlidARPlus(){
		DeactiveAll ();
		slidARPlusController.SetActive (true);
		pannel3.SetActive (true);
	}
}
