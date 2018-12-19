using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidARScript : MonoBehaviour {
	
	private LineRenderer lr;
	public Material lineMat;

	private Vector3 tmpcam2;
	private Vector3 tmpAnno2;

	private Vector3 initCam;
	private Vector3 initPos;

	float scHeight;
	float scWidth;

	private bool slidARRenderLine;

	// Use this for initialization
	void Start () {
		lr = gameObject.AddComponent<LineRenderer>();
		lr.enabled = false;

		slidARRenderLine = false;

		scHeight = Screen.height;
		scWidth = Screen.width;
	}


	public void ShowSlidARLine(bool b){
		slidARRenderLine = b;
		lr.enabled = b;
	}

	// Update is called once per frame
	void Update () {
		if (slidARRenderLine) {
			
			DrawSlidAR ();
		}
	
	}
	void OnDisable(){
		if (lr != null) {
			lr.enabled = false;
			slidARRenderLine = false;
		}
	}

	public void SetTmpCamPos(Vector3 pos){
		initCam = pos;
	}

	public void SetTmpAnnoPos(Vector3 pos){
		initPos = pos;
	}

	private void DrawSlidAR(){
		
		//lr.enabled = true;
		int count = 2;
		bool isCamOnSc = false;
		bool isAnnoOnSc = false;

		lr.material = lineMat;
		//lr.SetColors (Color.red,Color.red);
		lr.startColor = Color.red;
		lr.endColor = Color.red;
		lr.startWidth = 0.005f;
		lr.endWidth = 0.005f;



		tmpcam2 = Camera.main.WorldToScreenPoint (initCam);
		tmpAnno2 = Camera.main.WorldToScreenPoint (initPos);

		Vector2 camOnScr = new Vector2 (tmpcam2.x,tmpcam2.y); 
		Vector2 annoOnScr = new Vector2 (tmpAnno2.x,tmpAnno2.y); 
		Vector2 vCamToAnno = annoOnScr - camOnScr;

		if(camOnScr.x >=0 || camOnScr.x < scWidth)
		{
			if(camOnScr.y >=0 || camOnScr.y < scHeight)
			{
				//print ("1");
				isCamOnSc = true;
				count++;
			}
		}
		if(annoOnScr.x>=0||annoOnScr.x < scWidth)
		{
			if(annoOnScr.y >=0 || annoOnScr.y < scHeight)
			{
				//print ("2");
				isAnnoOnSc = true;
				count++;
			}
		}

		lr.positionCount = count;
		//lr.SetVertexCount (count);	

		if(!isCamOnSc)
		{
			lr.SetPosition (0, Camera.main.ScreenToWorldPoint(new Vector3(camOnScr.x,camOnScr.y,1f)));
			lr.SetPosition (1, Camera.main.ScreenToWorldPoint(new Vector3(annoOnScr.x,annoOnScr.y,1f)));
		}
		else
		{
			Vector2 fpoint = camOnScr - (2*vCamToAnno);
			lr.SetPosition (0, Camera.main.ScreenToWorldPoint(new Vector3(fpoint.x,fpoint.y,1f)));
			lr.SetPosition (1, Camera.main.ScreenToWorldPoint(new Vector3(camOnScr.x,camOnScr.y,1f)));
			lr.SetPosition (2, Camera.main.ScreenToWorldPoint(new Vector3(annoOnScr.x,annoOnScr.y,1f)));
		}
		if(isAnnoOnSc)
		{
			Vector2 lpoint = camOnScr + (2*vCamToAnno);
			lr.SetPosition (count-1, Camera.main.ScreenToWorldPoint(new Vector3(lpoint.x,lpoint.y,1f)));
		}
	}

	/// <summary>
	/// //////////
	/// </summary>
	/// <param name="pos">Position.</param>

	public Vector3 SlidAR(Vector2 pos){
		//Debug.Log ("POS !!!!!!" + pos);

		Vector3 intCamToSc = Camera.main.WorldToScreenPoint (initCam);
		Vector3 objToSc = Camera.main.WorldToScreenPoint (initPos);

		float m = (intCamToSc.y-objToSc.y)/(intCamToSc.x-objToSc.x);
		float c = intCamToSc.y - (m*intCamToSc.x);

		float lx = pos.x + (m*pos.x-pos.y+c)/(m*m+1)*m;
		float ly = pos.y + (m*pos.x-pos.y+c)/(m*m+1); 


		Vector3 touchPos = new Vector3 (lx,ly,1f);

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

		return initCam + (d * V1);
		//selectedObject.transform.position = initCam + (d * V1);
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
