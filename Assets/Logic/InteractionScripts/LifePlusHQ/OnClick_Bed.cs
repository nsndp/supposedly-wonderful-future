using UnityEngine;
using System.Collections;

public class OnClick_Bed : MonoBehaviour {

	DataControlHub D;
	UIControl UIC;

	void Start () {
		D = GameObject.Find("Data").GetComponent<DataControlHub>();
		UIC = GameObject.Find("Interface/UI").GetComponent<UIControl>();
	}
	
	void OnMouseDown() {
		UIC.StartDialogue(D.DialogueOther, D.S.SH.DSOther, 10, D.S.SH.nextDIDBed, true);
	}
}
