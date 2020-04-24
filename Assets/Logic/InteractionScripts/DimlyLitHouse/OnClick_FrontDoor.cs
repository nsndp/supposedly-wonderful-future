using UnityEngine;
using System.Collections;

public class OnClick_FrontDoor : MonoBehaviour {
	DataControlChapter1 DC;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
	}

	void OnMouseDown() {
		DC.UIC.StartDialogue(DC.DialogueOther, DC.S.SC1.DSOther, 2, DC.S.SC1.nextDIDFrontDoor, true);
	}
}
