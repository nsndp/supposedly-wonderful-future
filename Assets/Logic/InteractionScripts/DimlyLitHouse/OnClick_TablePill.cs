using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_TablePill : MonoBehaviour {

	public Transform Table;
	DataControlChapter1 DC;
	int phase = 0;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
	}

	void OnMouseDown() {
		if (DC.S.SC1.choseEuthanasia) {
			Events.Trigger(DC.S, (int)events1.pickedPill);
			Table.Find("Pill").gameObject.SetActive(false);
			Table.Find("Box").gameObject.SetActive(false);
			Table.Find("HighlightBox").gameObject.SetActive(false);
			gameObject.SetActive(false);
		}
		else if (phase == 0) {
			if (!DC.S.SC1.foundPill) Events.Trigger(DC.S, (int)events1.foundPill);
			phase = 1;
		}
	}

	void Update() {
		if (phase == 1) {
			DC.UIC.DisplayComment(DC.Comments.GetLine(DC.S.SC1.CCID[11]));
			Cursor.visible = false; DC.bReturn.SetActive(false);
			DC.UIC.Col(false); phase = 2;
		}
		else if (phase == 2 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))) {
			Cursor.visible = true; DC.bReturn.SetActive(true);
			DC.UIC.Col(true); phase = 0;
			DC.UIC.HideComment();
			if (DC.S.SC1.CCID[11] < 102) DC.S.SC1.CCID[11]++;
		}
	}
}
