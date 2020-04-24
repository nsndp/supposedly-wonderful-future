using UnityEngine;
using System.Collections;

public class OnClick_Guitar : MonoBehaviour {
	UIControl UIC;
	DataControlChapter2 DC;
	
	void Start() {
		UIC = GameObject.Find("Interface/UI").GetComponent<UIControl>();
		DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
	}
	
	void OnMouseDown() {
		DC.S.SC2.nextDID_Other = 54;
		UIC.StartDialogue(DC.DialogueOther, DC.S.SC2.DSOther, 4, 54, false);
	}
}
