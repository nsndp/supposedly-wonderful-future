using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighlightHints : MonoBehaviour {

	public Transform[] NPS = new Transform[1]; //next plot step
	DataControl DC;
	Color main; Color red = new Color(1, 0.141F, 0.141F);
	Color yellow = new Color(1, 0.408F, 0.141F); //yellow is for when red is not visible enough on the background, which is true only in CbIU for suspects' screens
	public int state = 0; bool inZoom = false;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControl>();
	}

	void SetHL(Transform T, bool state) {
		for (int i = 0; i < T.transform.childCount; i++) {
			var C = T.GetChild(i);
			if (C.gameObject.activeSelf) {
				var scr = C.GetComponent<HighlightOnHover>();
				if (scr != null) scr.HighlightEffect.SetActive(state);
				else if (state) C.GetComponent<CharacterOnClick>().MsEnter();
				else C.GetComponent<CharacterOnClick>().MsExit();
			}
		}
	}
	void Additional(bool state) {
		if (DC.S.levelID == 1 && DC.S.SC1.currentRoom != 0) {
			if (DC.S.SC1.currentRoom == 2) ((DataControlChapter1)DC).ReturnLeft.SetActive(state);
			else ((DataControlChapter1)DC).ReturnRight.SetActive(state);
		}
	}

	void Update() {
		if (DC.S.inDialogue == -1 && !DC.S.inNarration) {
			if (Input.GetKeyDown(KeyCode.H) && state == 0 && (DC.bMenu.activeSelf || DC.bReturn.activeSelf)) {
				state = 1; Cursor.visible = false; DC.UIC.Col(false);
				inZoom = DC.bReturn.activeSelf; DC.bMenu.SetActive(false); DC.bReturn.SetActive(false);
				SetHL(DC.currentColliders.transform, true); Additional(true);
			}
			else if (Input.GetKeyUp(KeyCode.H) && state == 1) {
				SetHL(DC.currentColliders.transform, false); Additional(false);
				if (!inZoom) DC.bMenu.SetActive(true); else DC.bReturn.SetActive(true);
				state = 0; Cursor.visible = true; DC.UIC.Col(true);
			}
			else if (Input.GetKeyDown(KeyCode.N) && state == 0 && NPS != null && (DC.bMenu.activeSelf || DC.bReturn.activeSelf)) {
				state = 2; Cursor.visible = false; DC.UIC.Col(false);
				inZoom = DC.bReturn.activeSelf; DC.bMenu.SetActive(false); DC.bReturn.SetActive(false);
				if (NPS[0].gameObject.layer == 5) { //UI
					NPS[0].GetComponent<Image>().color = new Color(1, 0.318F, 0.318F);
					NPS[0].Find("Arrow").GetComponent<Image>().color = new Color(1, 0.553F, 0.553F);
					NPS[0].gameObject.SetActive(true);
				}
				else if (NPS[0].GetComponent<CharacterOnClick>() != null) {
					NPS[0].GetComponent<CharacterOnClick>().RedHL(true);
				}
				else for (int i = 0; i < NPS.Length; i++) {
					var o = NPS[i].GetComponent<HighlightOnHover>().HighlightEffect;
					main = o.GetComponent<Renderer>().material.GetColor("_TintColor");
					o.GetComponent<Renderer>().material.SetColor("_TintColor", red);
					if (DC.S.levelID == 2 && NPS[i] == ((DataControlChapter2)DC).ppScreens) o.GetComponent<Renderer>().material.SetColor("_TintColor", yellow);
					o.SetActive(true);
				}
			}
			else if (Input.GetKeyUp(KeyCode.N) && state == 2) {
				if (NPS[0].gameObject.layer == 5) { //UI
					NPS[0].gameObject.SetActive(false);
					NPS[0].GetComponent<Image>().color = new Color(0.427F, 0.69F, 1);
					NPS[0].Find("Arrow").GetComponent<Image>().color = new Color(0.612F, 0.788F, 1);
				}
				else if (NPS[0].GetComponent<CharacterOnClick>() != null) {
					NPS[0].GetComponent<CharacterOnClick>().RedHL(false);
				}
				else for (int i = 0; i < NPS.Length; i++) {
					var o = NPS[i].GetComponent<HighlightOnHover>().HighlightEffect;
					o.SetActive(false); o.GetComponent<Renderer>().material.SetColor("_TintColor", main);
				}
				if (!inZoom) DC.bMenu.SetActive(true); else DC.bReturn.SetActive(true);
				state = 0; Cursor.visible = true; DC.UIC.Col(true);
			}
		}
	}
}
