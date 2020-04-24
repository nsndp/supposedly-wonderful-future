using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_Darts : MonoBehaviour {

	public Camera cam;
	public GameObject DartGreen1;
	public GameObject DartGreen2;
	public GameObject DartGreen3;
	public GameObject DartBlue1;
	public GameObject DartBlue2;
	public GameObject DartBlue3;
	public GameObject DartRed1;
	public GameObject DartRed2;
	public GameObject DartRed3;

	Vector3[] coordinates = new Vector3[] {
		new Vector3(-0.051F, 1.46F, 1.33F), new Vector3(-0.051F, 1.46F, 1.215F), new Vector3(-0.051F, 1.503F, 1.202F),
		new Vector3(-0.051F, 1.495F, 1.185F), new Vector3(-0.051F, 1.59F, 1.08F), new Vector3(-0.051F, 1.58F, 1.14F),
		new Vector3(-0.051F, 1.365F, 1.1F), new Vector3(-0.051F, 1.57F, 1.2F), new Vector3(-0.051F, 1.68F, 1.02F)
	};
	int[] rotations = new int[] {25, 10, 50, 45, 0, 5, 30, 55, 40};
	Quaternion[] rotationsOnShelf;
	Vector3[] coordinatesOnShelf;

	int ObjectIndex = 1;
	int phase = 0; float tc;
	Vector3 SrcPos, DstPos, RndDartPos;
	Quaternion SrcRot, DstRot, RndDartRot;
	GameObject[] darts; Vector3 startOffset = new Vector3(-1, 0, 0);
	DataControlPrologue DC;
	
	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
		SrcPos = DC.camPosM; DstPos = new Vector3(1.4F, 1.5F, 0.7F);
		SrcRot = DC.camRotM; DstRot = Quaternion.Euler(new Vector3(0, 90, 0));
		darts = new GameObject[] {DartGreen1, DartGreen2, DartGreen3, DartBlue1, DartBlue2, DartBlue3, DartRed1, DartRed2, DartRed3};
		coordinatesOnShelf = new Vector3[] {
			DartGreen1.transform.localPosition, DartGreen2.transform.localPosition, DartGreen3.transform.localPosition,
			DartBlue1.transform.localPosition, DartBlue2.transform.localPosition, DartBlue3.transform.localPosition,
			DartRed1.transform.localPosition, DartRed2.transform.localPosition, DartRed3.transform.localPosition
		};
		rotationsOnShelf = new Quaternion[] {
			DartGreen1.transform.localRotation, DartGreen2.transform.localRotation, DartGreen3.transform.localRotation,
			DartBlue1.transform.localRotation, DartBlue2.transform.localRotation, DartBlue3.transform.localRotation,
			DartRed1.transform.localRotation, DartRed2.transform.localRotation, DartRed3.transform.localRotation
		};
	}

	void OnMouseDown() {
		if (phase == 0) phase = 1;
		else if (phase == 3) if (!DC.S.SP.wallPierced) phase = 5; else phase = 9;
	}

	IEnumerator Waiting(float sec, int ph) {
		phase = -1; yield return new WaitForSeconds(sec); phase = ph;
	}

	void Update() {
		//zoom in
		if (phase == 1) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			tc = 0; phase = 2;
		}
		else if (phase == 2 && tc <= 1) {
			tc += 0.006666F * Time.deltaTime * 60; //tc += 0.008333F;
			cam.transform.position = Vector3.Lerp(SrcPos, DstPos, Mathf.SmoothStep(0, 1, tc));
			cam.transform.rotation = Quaternion.Lerp(SrcRot, DstRot, Mathf.SmoothStep(0, 1, tc));
		}
		else if (phase == 2 && tc > 1) {
			DC.CursorLock(false); DC.UIC.Col(true); DC.bReturn.SetActive(true); phase = 3;
		}
		//zoom out
		else if (phase == 3 && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.bReturn.SetActive(false); DC.bReturn.GetComponent<ButtonArrow>().clicked = false;
			DC.CursorLock(true); DC.UIC.Col(false); tc = 0; phase = 4;
		}
		else if (phase == 4 && tc <= 1) {
			tc += 0.006666F * Time.deltaTime * 60;
			cam.transform.position = Vector3.Lerp(DstPos, SrcPos, Mathf.SmoothStep(0, 1, tc));
			cam.transform.rotation = Quaternion.Lerp(DstRot, SrcRot, Mathf.SmoothStep(0, 1, tc));
		}
		else if (phase == 4 && tc > 1) {
			DC.CursorLock(false); DC.UIC.Col(true); DC.bMenu.SetActive(true); phase = 0;
		}
		//playing darts scripted
		if (phase == 5) {
			tc = 0; phase = 6;
			darts[DC.S.SP.di].transform.localPosition = coordinates[DC.S.SP.di] + startOffset;
			darts[DC.S.SP.di].transform.localRotation = Quaternion.Euler(rotations[DC.S.SP.di], 0, 0);
			Cursor.visible = false; DC.UIC.Col(false); DC.bReturn.SetActive(false);
		}
		else if (phase == 6 && tc <= 1) {
			tc += 0.05F * Time.deltaTime * 60;
			darts[DC.S.SP.di].transform.localPosition = Vector3.Lerp(coordinates[DC.S.SP.di] + startOffset, coordinates[DC.S.SP.di], tc);
		}
		else if (phase == 6 && tc > 1) {
			DC.Sound.clip = DC.S.SP.di != 8 ? DC.dart : DC.dartWall; DC.Sound.Play();
			StartCoroutine(Waiting(0.33F, 7));
		}
		else if (phase == 7) {
			DC.UIC.DisplayComment(DC.Comments.GetLine(DC.S.SP.CCID[ObjectIndex]));
			DC.S.SP.CCID[ObjectIndex]++; phase = 8;
		}
		else if (phase == 8 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))) {
			Cursor.visible = true; DC.UIC.Col(true); DC.bReturn.SetActive(true);
			DC.UIC.HideComment(); phase = 3; DC.S.SP.di++;
			if (DC.S.SP.di > 8) DC.S.SP.wallPierced = true;
		}
		//playing darts randomized
		else if (phase == 9) {
			tc = 0; phase = 10;
			if (DC.S.SP.di >= 9) {
				DC.S.SP.di = 0;
				for (int i = 0; i < 9; i++) {
					darts[i].transform.localPosition = coordinatesOnShelf[i];
					darts[i].transform.localRotation = rotationsOnShelf[i];
				}
			}
			RndDartPos = GenDartPos();
			RndDartRot = Quaternion.Euler(Random.Range(0, 90), 0, 0);
			darts[DC.S.SP.di].transform.localPosition = RndDartPos + startOffset;
			darts[DC.S.SP.di].transform.localRotation = RndDartRot;
			Cursor.visible = false; DC.UIC.Col(false); DC.bReturn.SetActive(false);
		}
		else if (phase == 10 && tc <= 1) {
			tc += 0.05F * Time.deltaTime * 60;
			darts[DC.S.SP.di].transform.localPosition = Vector3.Lerp(RndDartPos + startOffset, RndDartPos, tc);
		}
		else if (phase == 10 && tc > 1) {
			DC.Sound.clip = DC.dart; DC.Sound.Play();
			StartCoroutine(Waiting(0.33F, 11));
		}
		else if (phase == 11) {
			Cursor.visible = true; DC.UIC.Col(true); DC.bReturn.SetActive(true);
			DC.S.SP.di++; phase = 3;
		}
	}

	Vector3 GenDartPos() { //this is to rule out generating 2 darts too close to each other so they overlap
		Vector3 v; bool tooClose;
		do {
			v = new Vector3(-0.051F, Random.Range(1.35F, 1.65F), Random.Range(1.05F, 1.35F));
			tooClose = false;
			for (int i = 0; i < DC.S.SP.di; i++)
				if ((Mathf.Abs(darts[i].transform.localPosition.y - v.y) < 0.02F) && 
			    	(Mathf.Abs(darts[i].transform.localPosition.z - v.z) < 0.02F)) tooClose = true;
		}
		while (tooClose);
		return v;
	}
}
