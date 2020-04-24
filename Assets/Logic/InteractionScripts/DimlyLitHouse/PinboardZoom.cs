using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PinboardZoom : MonoBehaviour {

	public Camera cam;
	DataControlChapter1 DC;
	GameObject PB; GameObject[] Drawings;
	Vector3 DstPos = new Vector3(0.18F, 1.577F, 0); Vector3 DstRotEuler = Vector3.zero;
	Vector3 SrcPos; Quaternion SrcRot, DstRot;
	public int phase = -1; float tc;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		SrcPos = cam.transform.localPosition; SrcRot = cam.transform.rotation;
		DstRot = Quaternion.Euler(DstRotEuler);
		PB = GameObject.Find("Livingroom/Colliders").transform.Find("Pinboard").gameObject;
		Drawings = new GameObject[] {
			GameObject.Find("Livingroom/Colliders").transform.Find("Drawing1").gameObject,
			GameObject.Find("Livingroom/Colliders").transform.Find("Drawing2").gameObject,
			GameObject.Find("Livingroom/Colliders").transform.Find("Drawing3").gameObject
		};
	}


	void Update() {
		if (phase == 0) {
			DC.UIC.DisplayComment(DC.Comments.GetLine(DC.S.SC1.CCID[0]));
			Cursor.visible = false; DC.bMenu.SetActive(false); DC.UIC.Col(false);
			phase = 1;
		}
		else if (phase == 1 && Input.GetMouseButtonDown(0)) {
			DC.UIC.HideComment();
			Cursor.visible = true; DC.bMenu.SetActive(true); DC.UIC.Col(true); phase = -1;
			DC.S.SC1.checkedPinboard = true;
		}

		else if (phase == 2) {
			tc = 0; phase = 3;
			DC.CursorLock(true); DC.bMenu.SetActive(false); DC.UIC.Col(false);
		}
		else if (phase == 3 && tc <= 1) {
			tc += 0.01F * Time.deltaTime * 60;
			cam.transform.localPosition = Vector3.Lerp(SrcPos, DstPos, tc);
			cam.transform.rotation = Quaternion.Lerp(SrcRot, DstRot, tc);
		}
		else if (phase == 3 && tc > 1) {
			phase = 4;
			DC.CursorLock(false); DC.bReturn.SetActive(true); DC.UIC.Col(true);
			PB.SetActive(false); Drawings[0].SetActive(true); Drawings[1].SetActive(true); Drawings[2].SetActive(true);
		}
		else if (phase == 4 && DC.bReturn.activeSelf && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			tc = 0; phase = 5;
			DC.CursorLock(true); DC.bReturn.SetActive(false); DC.UIC.Col(false);
			DC.bReturn.GetComponent<ButtonArrow>().clicked = false;
		}
		else if (phase == 5 && tc <= 1) {
			tc += 0.01F * Time.deltaTime * 60;
			cam.transform.localPosition = Vector3.Lerp(DstPos, SrcPos, tc);
			cam.transform.rotation = Quaternion.Lerp(DstRot, SrcRot, tc);
		}
		else if (phase == 5 && tc > 1) {
			phase = -1;
			DC.CursorLock(false); DC.bMenu.SetActive(true); DC.UIC.Col(true);
			PB.SetActive(true); Drawings[0].SetActive(false); Drawings[1].SetActive(false); Drawings[2].SetActive(false);
		}

		else if (phase == 6) {
			DC.UIC.DisplayComment(DC.Comments.GetLine(80));
			Cursor.visible = false; DC.bMenu.SetActive(false); DC.UIC.Col(false);
			phase = 7;
		}
		else if (phase == 7 && Input.GetMouseButtonDown(0)) {
			DC.UIC.HideComment();
			Cursor.visible = true; DC.bMenu.SetActive(true); DC.UIC.Col(true);
			phase = -1; PB.SetActive(false);
			DC.S.SC1.postCheckedPinboard = true;
		}
	}
}