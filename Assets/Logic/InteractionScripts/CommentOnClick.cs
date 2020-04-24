using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CommentOnClick : MonoBehaviour {

	DataControl DC; GameObject b; int phase = 0;

	public int ChapterNumber;//don't need?
	public int ObjectIndex;
	public int StartingCommentID;
	public int EndingCommentID;
	public int LoopMode; //0 - none, 1 - loop last, 2 - loop all
	public int? triggersEvent; //only in loopmode = 0

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControl>();
	}

	void OnMouseDown() { phase = 1; }

	void Update() {
		int[] CCID = DC.GetCCID();
		if (phase == 1) {
			var i = CCID[ObjectIndex];
			DC.UIC.DisplayComment(DC.Comments.GetLine(i));
			CCID[ObjectIndex]++;
			if (i == EndingCommentID)
				if (LoopMode == 1) CCID[ObjectIndex] = i;
				else if (LoopMode == 2) CCID[ObjectIndex] = StartingCommentID;
			if (DC.bMenu.activeSelf) b = DC.bMenu; else b = DC.bReturn; b.SetActive(false);
			Cursor.visible = false; DC.UIC.Col(false); phase = 2;
		}
		else if (phase == 2 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))) phase = 3;
		else if (phase == 3) {
			Cursor.visible = true; DC.UIC.Col(true); b.SetActive(true);
			phase = 0; DC.UIC.HideComment();
			if (LoopMode == 0 && CCID[ObjectIndex] == EndingCommentID + 1) {
				if (triggersEvent != null) Events.Trigger(DC.S, (int)triggersEvent);
				gameObject.SetActive(false);
			}
		}
	}
}
