using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class ObjectController : MonoBehaviour {

	public GameObject[] objectlists = {
	};
	/*
	public GameObject obj1;
	public GameObject obj2;
	public GameObject obj3;
	public GameObject obj4;
	public GameObject obj5;
*/


	private int index;
	public GameObject parentObject;
	private GameObject selectedObject;
	private ARKitHitCheck ARKitHitScript;
	private SlidARScript SARS;
	private ObjectInfo OinfoScript;
	private bool SlidAROn;
	private Transform emptyTran;

	public enum EditModeState{
		NORMAL,
		SlidAR,
		ROTATION
		/*
		 * NORMAL = 0
		 * SlidAR = 1
		 * ROTATION = 2
		 * */
	}
	private EditModeState EMS;

	// Use this for initialization
	void Start () {
		index = 0;
		ARKitHitScript = (ARKitHitCheck)gameObject.GetComponent(typeof(ARKitHitCheck));
		emptyTran = new GameObject ().transform;
		EMS = EditModeState.NORMAL; 
		SARS = (SlidARScript)gameObject.GetComponent(typeof(SlidARScript));
		SARS.enabled = false;
		SlidAROn = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	private Vector3 initCam;
	private Vector3 initPos;

	public void OnOffSlidAR(bool onOff){
		
		if (onOff) {

			if (selectedObject == null) {
				return;
			}
			if (!SlidAROn) {
				initCam = OinfoScript.GetInitCam ();
				initPos = OinfoScript.GetInitPos ();

				SARS.enabled = true;
				SARS.SetTmpAnnoPos (initPos);
				SARS.SetTmpCamPos (initCam);
				//ChangeEditState (1);
				SlidAROn = onOff;
			}

		} else {
			if (SlidAROn) {
				SARS.enabled = false;
				//ChangeEditState (0);
				SlidAROn = onOff;
			}

		}

	}

	public void ChangeEditState(int i){
		EMS = (EditModeState)i;
		if (i == 1) {
			OnOffSlidAR (true);
		} else {
			OnOffSlidAR (false);
		}
	}


	public void SetCreateObject(int i){
		index = i;
	}
	public void CreateObjectNotSlidAR(Touch t){
		emptyTran = ARKitHitScript.HitLoc (t);

		SetSelectedObject (Instantiate (objectlists [index], emptyTran.position, new GameObject().transform.rotation, parentObject.transform));
	}

	public GameObject ReturnCreatedObject(){
		return selectedObject;
	}
	public void CreateObject(Touch t){
		/*if (t != null) {
			Instantiate (objectlists [index], t.position, t.rotation, parentObject.transform);
		}*/
		emptyTran = ARKitHitScript.HitLoc (t);
		if (emptyTran != null) {
			OinfoScript = (ObjectInfo)Instantiate (objectlists [index], emptyTran.position, new GameObject().transform.rotation, parentObject.transform).GetComponent(typeof(ObjectInfo));
			//OinfoScript.SetInitCam (Camera.main.transform.position);
			//OinfoScript.SetInitPos (emptyTran.position);
			SetInitData();
			SetSelectedObject (OinfoScript.gameObject);
		}

	}

	public void SetInitData(){
		OinfoScript.SetInitCam (Camera.main.transform.position);
		OinfoScript.SetInitPos (emptyTran.position);
	
	}
	public void EditObject(Touch t){
		/*if (t == null) {
			return;
		}
		selectedObject.transform.position = t.position;
		selectedObject.transform.rotation = t.rotation;*/

		if (selectedObject != null) {
			
			switch (EMS) {
			case EditModeState.NORMAL:
				{
					emptyTran = ARKitHitScript.HitLoc (t);
					if (emptyTran != null) {
						selectedObject.transform.position = emptyTran.position;
						//selectedObject.transform.rotation = emptyTran.rotation;
						/*if (t.phase == TouchPhase.Ended) {
							OinfoScript.SetInitCam (Camera.main.transform.position);
							OinfoScript.SetInitPos (emptyTran.position);
						}*/
					}
					break;
				}
			case EditModeState.SlidAR:
				{
					
					SlidAR (t.position);
					break;
				}
			case EditModeState.ROTATION:
				{
					RotateObject (t);
					break;
				}
			default:
				break;
				
			}

		}

	}
	private float speed = 1.0f;
	//private float angle;
	public void RotateObject(Touch t){

		//angle = ;
		selectedObject.transform.RotateAround (selectedObject.transform.up,t.deltaPosition.x*speed*Time.deltaTime*-1);
	}

	public void RemoveObject(){
		if(selectedObject != null)
		{
			Destroy (selectedObject);
		}
	}

	public void SetSelectedObject(GameObject go){
		selectedObject = go;
		OinfoScript = (ObjectInfo)selectedObject.GetComponent (typeof(ObjectInfo));
	}

	private void SlidAR(Vector3 pos){
		Vector3 intCamToSc = Camera.main.WorldToScreenPoint (initCam);
		Vector3 objToSc = Camera.main.WorldToScreenPoint (initPos);

		float m = (intCamToSc.y-objToSc.y)/(intCamToSc.x-objToSc.x);
		float c = intCamToSc.y - (m*intCamToSc.x);

		float lx = pos.x + (m*pos.x-pos.y+c)/(m*m+1)*m;
		float ly = pos.y + (m*pos.x-pos.y+c)/(m*m+1); 


		Vector3 touchPos = new Vector3 (lx,ly,pos.z+1f);

		touchPos = Camera.main.ScreenToWorldPoint (touchPos);

		Vector3 cCamPos = Camera.main.transform.position;

		Vector3 V1 = initPos - initCam;
		Vector3 V2 =  touchPos - cCamPos ;

		float[][] input = new float[3][];
		input[0] = new float[3] { -V1.x, V2.x, initCam.x - cCamPos.x };
		input[1] = new float[3] { -V1.y, V2.y, initCam.y - cCamPos.y };
		input[2] = new float[3] { -V1.z, V2.z, initCam.z - cCamPos.z };

		float[] result = guassianElim(input);
		float d = result[0];

		selectedObject.transform.position = initCam + (d * V1);
	}
	public float[] guassianElim(float[][] rows)
	{
		int length = rows[0].Length;

		for (int i = 0; i < rows.Length - 1; i++)
		{
			if (rows[i][i] == 0 && !Swap(rows, i, i))
			{
				return null;
			}

			for (int j = i; j < rows.Length; j++)
			{
				float[] d = new float[length];
				for (int x = 0; x < length; x++)
				{
					d[x] = rows[j][x];
					if (rows[j][i] != 0)
					{
						d[x] = d[x] / rows[j][i];
					}
				}
				rows[j] = d;
			}

			for (int y = i + 1; y < rows.Length; y++)
			{
				float[] f = new float[length];
				for (int g = 0; g < length; g++)
				{
					f[g] = rows[y][g];
					if (rows[y][i] != 0)
					{
						f[g] = f[g] - rows[i][g];
					}

				}
				rows[y] = f;
			}
		}

		return CalculateResult(rows);
	}

	private bool Swap(float[][] rows, int row, int column)
	{
		bool swapped = false;
		for (int z = rows.Length - 1; z > row; z--)
		{
			if (rows[z][row] != 0)
			{
				float[] temp = new float[rows[0].Length];
				temp = rows[z];
				rows[z] = rows[column];
				rows[column] = temp;
				swapped = true;
			}
		}

		return swapped;
	}

	private float[] CalculateResult(float[][] rows)
	{
		float val = 0;
		int length = rows[0].Length;
		float[] result = new float[rows.Length];
		for (int i = rows.Length - 1; i >= 0; i--)
		{
			val = rows[i][length - 1];
			for (int x = length - 2; x > i - 1; x--)
			{
				val -= rows[i][x] * result[x];
			}
			result[i] = val / rows[i][i];

			if (!IsValidResult(result[i]))
			{
				return null;
			}
		}
		return result;
	}

	private bool IsValidResult(double result)
	{
		return result.ToString() != "NaN" || !result.ToString().Contains("Infinity");
	}

}
