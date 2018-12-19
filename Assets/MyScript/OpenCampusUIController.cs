using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCampusUIController : MonoBehaviour {

	public GameObject menuPannel;
	public GameObject addPannel;
	public GameObject TestPannel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void TriggerMenu(){
		menuPannel.SetActive (!menuPannel.activeInHierarchy);
	}
	public void TriggerAddPannel(){
		addPannel.SetActive (!addPannel.activeInHierarchy);
	}
	public void TriggerAddPannel(bool b){
		addPannel.SetActive (b);
	}
	public void TriggerTestPannel(){
		TestPannel.SetActive (!TestPannel.activeSelf);
	}
}
