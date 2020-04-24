using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_Archway : MonoBehaviour {

	public bool isBedroom;
	public GameObject cameraA, cameraB;

	DataControlChapter2 DC; Image img;
	GameObject B, L, ColB, ColL; OnClick_Desk DSK;
	int phase = 0;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		img = GameObject.Find("Interface/UI").transform.Find("BlackScreen").GetComponent<Image>();
		B = GameObject.Find("BedroomRender").gameObject; L = GameObject.Find("LivingRoomRender").gameObject;
		ColB = GameObject.Find("CollidersB").gameObject; ColL = GameObject.Find("CollidersL").gameObject;
	}

	void OnMouseDown() {
		var c = img.color; c.a = 0; img.color = c; img.gameObject.SetActive(true);
		DC.UIC.Col(false); DC.CursorLock(true); DC.bMenu.SetActive(false); phase = 1;
	}

	void Update() {
		if (phase == 1 && img.color.a < 1) {
			var c = img.color; c.a += 0.02F * Time.deltaTime * 60; img.color = c;
		}
		else if (phase == 1 && img.color.a >= 1) {
			cameraA.SetActive(isBedroom);
			cameraA.GetComponent<Camera>().enabled = isBedroom;
			cameraA.GetComponent<AudioListener>().enabled = isBedroom;
			cameraB.SetActive(!isBedroom);
			cameraB.GetComponent<Camera>().enabled = !isBedroom;
			cameraB.GetComponent<AudioListener>().enabled = !isBedroom;
			L.SetActive(isBedroom);
			B.SetActive(!isBedroom);
			phase = 2;
		}
		else if (phase == 2 && img.color.a > 0) {
			var c = img.color; c.a -= 0.02F * Time.deltaTime * 60; img.color = c;
		}
		else if (phase == 2 && img.color.a <= 0) {
			DC.S.SC2.currentRoom = isBedroom ? 0 : 1;
			DC.currentColliders = isBedroom ? ColL : ColB;
			DC.UIC.Col(true); DC.UPP();
			if (!DC.S.SC2.queueTheReveal) {
				DC.CursorLock(false); DC.bMenu.SetActive(true); phase = 0;
			} else {
				DC.S.SC2.queueTheReveal = false; DC.S.SC2.hackerRevealed = true;
				DC.S.SC2.DSScreens.R[4] = new int[] {129};
				if (DC.S.SC2.deskUnfolded) DSK = ColB.transform.Find("DeskU").GetComponent<OnClick_Desk>();
				else DSK = ColB.transform.Find("DeskF").GetComponent<OnClick_Desk>();
				DSK.RevealSuspectF(); phase = 3;
			}
		}
		else if (phase == 3 && !DSK.D.GetComponent<Animation>().IsPlaying("RevealSuspectF")) {
			DC.CursorLock(false); DC.bMenu.SetActive(true); phase = 0;
		}
	}
}
