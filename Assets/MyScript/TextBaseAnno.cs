using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBaseAnno : MonoBehaviour {

	public GameObject TextPrefab;
	protected Button butt;
	protected InputField inputField;
	private GameObject tmp;
	private TouchScreenKeyboard tsk;

	// Use this for initialization
	void Start () {
		tsk = TouchScreenKeyboard.Open ("",TouchScreenKeyboardType.Default);
		tmp = Instantiate (TextPrefab, FindObjectOfType<Canvas> ().transform);
		butt = tmp.GetComponentInChildren<Button>();
		butt.gameObject.SetActive (false);
		inputField = tmp.GetComponentInChildren<InputField> ();

	}
	
	// Update is called once per frame
	void Update () {
		

		if ( (inputField.text.Length > 0) && ((tsk.done)||Input.GetKey(KeyCode.KeypadEnter)|| Input.GetKey(KeyCode.Return)) ){
			Debug.Log ("Enter");
			ChangeText (inputField.text);
			inputField.gameObject.SetActive (false);
		}

		tmp.transform.position = Camera.main.WorldToScreenPoint (transform.position + new Vector3(0f,0.04f,0f));
	}

	void OnDestroy(){
		Destroy (tmp);
	}
	public void ChangeText(string input){
		butt.gameObject.SetActive (true);
		butt.GetComponentInChildren<Text> ().text = input;
	}

}
