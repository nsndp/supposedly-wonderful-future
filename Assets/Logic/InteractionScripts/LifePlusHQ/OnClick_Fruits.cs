using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick_Fruits : MonoBehaviour {

	DataControlHub DC; int phase = 0, CID;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
	}

	void OnMouseDown() {
		if (DC.S.levelID == 12 && DC.S.SH.fruitsCounter >= 1) { phase = 1; CID = 20 + (3 - DC.S.SH.fruitsCounter); }
		if (DC.S.levelID == 13 && DC.S.SH.fruitsCounter == 3) { phase = 1; CID = 23; }
		DC.Sound.clip = DC.fruitEating; DC.Sound.Play();
		DC.S.SH.fruitsCounter--;
		DC.room.Find("Fruits/" + (DC.S.SH.fruitsCounter == 2 ? "Banana" : (DC.S.SH.fruitsCounter == 1 ? "Apple" : "Orange"))).gameObject.SetActive(false);
		if (DC.S.SH.fruitsCounter == 0) {
			DC.activeHL.SetActive(false); DC.activeHL = null;
			if (phase == 0) gameObject.SetActive(false);
		}
	}

	void Update() {
		if (phase == 1) {
			Cursor.visible = false; DC.bMenu.SetActive(false); DC.UIC.Col(false);
			DC.UIC.DisplayComment(DC.Comments.GetLine(CID));
			phase = 2;
		}
		else if (phase == 2 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))) {
			Cursor.visible = true; DC.bMenu.SetActive(true); DC.UIC.Col(true);
			DC.UIC.HideComment(); phase = 0;
			if (CID == 22) gameObject.SetActive(false);
		}
	}
}
