using UnityEngine;
using System.Collections;

public class AreaZoom : MonoBehaviour {

	DataControlChapter4 DC;
	public int phase = 0; float tc;

	void Start () {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter4>();
	}

	void Update() {
		if (phase == 1) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			phase = 2; tc = 0;
		}
		else if (phase == 2 && tc <= 1) {
			DC.cam.position = Vector3.Lerp(DC.camPosOut, DC.camPos, Mathf.SmoothStep(0, 1, tc));
			DC.cam.rotation = Quaternion.Lerp(DC.camRotOut, DC.camRot, Mathf.SmoothStep(0, 1, tc));
			tc += 0.008333333F * Time.deltaTime * 60;
		}
		else if (phase == 2 && tc > 1) {
			DC.CursorLock(false); DC.Area.SetActive(false);
			if (!DC.S.SC4.startingZoom) {
				DC.S.SC4.startingZoom = true; DC.UPP(1);
				for (int i = 0; i < 3; i++) DC.M[i].gameObject.SetActive(true);
				DC.UIC.Col(true); DC.bMenu.SetActive(true); phase = 0;
				DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, DC.S.SC4.nextDID, false);
			} else {
				for (int i = 0; i < 8; i++) DC.M[i].gameObject.SetActive(true);
				DC.UIC.Col(true); DC.bReturn.SetActive(true); phase = 3;
			}
		}
		else if (phase == 3 && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bReturn.SetActive(false);
			tc = 0; phase = 4;
		}
		else if (phase == 4 && tc <= 1) {
			DC.cam.position = Vector3.Lerp(DC.camPos, DC.camPosOut, Mathf.SmoothStep(0, 1, tc));
			DC.cam.rotation = Quaternion.Lerp(DC.camRot, DC.camRotOut, Mathf.SmoothStep(0, 1, tc));
			tc += 0.008333333F * Time.deltaTime * 60;
		}
		else if (phase == 4 && tc > 1) {
			DC.CursorLock(false);
			DC.Area.SetActive(true); for (int i = 0; i < 8; i++) DC.M[i].gameObject.SetActive(false);
			DC.bMenu.SetActive(true); phase = 0;
			DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, DC.S.SC4.nextDID, false);
		}
	}
}
