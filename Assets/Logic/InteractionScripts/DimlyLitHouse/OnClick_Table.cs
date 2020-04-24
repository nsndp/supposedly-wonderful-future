using UnityEngine;
using System.Collections;

public class OnClick_Table : MonoBehaviour {

	DataControlChapter1 DC;
	TableZoom TZ;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		TZ = DC.GetComponent<TableZoom>();
	}

	void OnMouseDown() {
		if (TZ.phase == 0) {
			if (DC.S.SC1.storyFinished) TZ.phase = 9;
			else {
				DC.CursorLock(true); DC.bMenu.SetActive(false); DC.UIC.Col(false);
				TZ.phase = 1; TZ.tc = 0;
			}
		}
	}
}
