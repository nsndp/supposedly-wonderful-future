using UnityEngine;
using System.Collections;

public class OnClick_BlindsControl : MonoBehaviour {

	public Animation Blinds;
	public GameObject Col, ColDrawn;
	DataControlPrologue DC;
	public int phase = 0;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
	}

	void Update() {
		if (phase == 1) {
			Blinds.Play("BlindsA1"); phase = 2;
			DC.Sound.clip = DC.blinds1; DC.Sound.Play();
			Cursor.visible = false; DC.UIC.Col(false); DC.bMenu.SetActive(false);
		}
		else if (phase == 2 && !Blinds.isPlaying) {
			DC.S.SP.blindsState = 1; phase = 0;
			Cursor.visible = true; DC.UIC.Col(true); DC.bMenu.SetActive(true);
		}

		else if (phase == 3) {
			Blinds.Play("BlindsA2"); phase = 4;
			DC.Sound.clip = DC.blinds2; DC.Sound.Play();
			Cursor.visible = false; DC.UIC.Col(false); DC.bMenu.SetActive(false);
		}
		else if (phase == 4 && !Blinds.isPlaying) {
			DC.S.SP.blindsState = 2; phase = 0;
			Cursor.visible = true; DC.UIC.Col(true); DC.bMenu.SetActive(true);
			Col.SetActive(false); ColDrawn.SetActive(true);
		}

		else if (phase == 5) {
			Blinds.Play("BlindsA3"); phase = 6;
			DC.Sound.clip = DC.blinds3; DC.Sound.Play();
			Cursor.visible = false; DC.UIC.Col(false); DC.bMenu.SetActive(false);
		}
		else if (phase == 6 && !Blinds.isPlaying) {
			DC.S.SP.blindsState = 0; phase = 0;
			Cursor.visible = true; DC.UIC.Col(true); DC.bMenu.SetActive(true);
			Col.SetActive(true); ColDrawn.SetActive(false);
		}
	}
}
