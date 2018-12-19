using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ODWMainController : MonoBehaviour {

	public Camera mainCam;
	public Camera vrCam;
	public GameObject annoParent;
	private GameObject cDrawObject;

	private SlidARScript SARS;

	// Use this for initialization
	void Awake(){
		SARS = (SlidARScript)gameObject.GetComponent(typeof(SlidARScript));
		SARS.enabled = (false);
	}

	void Start () {
		mainCam.enabled = true;
		vrCam.enabled = false;

		slidARMode = false;
	}
	
	// Update is called once per frame
	void Update () {

		if(mainCam.enabled){
			MainCamScript ();
		}

	}


	/// tmp info for slidAR 

	private Vector3 initCam;
	private Vector3 initPos;
	/// 

	/// AR mode script
	private bool slidARMode;

	private void MainCamScript(){
	
		if (Input.GetMouseButton (0)) {
			
			if (EventSystem.current.IsPointerOverGameObject())
				return;

			if (!slidARMode) {
				RaycastHit hit;
				Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

				annoParent.transform.position = ray.GetPoint (0.5f);
				initPos = annoParent.transform.position;
				initCam = mainCam.transform.position;

			} else {
				SlidAR (Input.mousePosition);
			}
		}
	}

	public void SlidARModeTrigger(){
		if(slidARMode){
			slidARMode = false;
			SARS.enabled =(false);
		}
		else{
			slidARMode = true;
			SARS.enabled =(true);
			SARS.SetTmpAnnoPos (initPos);
			SARS.SetTmpCamPos (initCam);
		}
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

		annoParent.transform.position = initCam + (d * V1);


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

	/// 

	public void SwitchCam(){
		if(mainCam.enabled){
			mainCam.enabled = false;
			vrCam.enabled = true;
			gameObject.transform.GetChild (1).gameObject.SetActive (false);
			gameObject.transform.GetChild (2).gameObject.SetActive (true);
			SetVRCam ();
		}
		else{
			mainCam.enabled = true;
			vrCam.enabled = false;
			gameObject.transform.GetChild (1).gameObject.SetActive (true);
			gameObject.transform.GetChild (2).gameObject.SetActive (false);
		}
	}

	private void SetVRCam(){
		vrCam.transform.position = mainCam.transform.position;
		vrCam.transform.LookAt (annoParent.transform.position);
	}
	public void ShowDrawingObject(int index){

		for(int child =0; child < annoParent.transform.childCount; child++) {
			if (child == index) {
				annoParent.transform.GetChild (child).gameObject.SetActive (true);
				cDrawObject = annoParent.transform.GetChild (child).gameObject;
			} else {
				annoParent.transform.GetChild (child).gameObject.SetActive (false);
			}
		}
			
	}
}
