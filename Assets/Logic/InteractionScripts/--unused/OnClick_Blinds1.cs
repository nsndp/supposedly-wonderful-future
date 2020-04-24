using UnityEngine;
using System.Collections;

public class OnClick_Blinds1 : MonoBehaviour {

	DataControl DC;
	OnClick_BlindsControl BC;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
		BC = DC.GetComponent<OnClick_BlindsControl>();
	}
	
	void OnMouseDown() {
		if (DC.S.SP.blindsState == 0) BC.phase = 1; else BC.phase = 3;
	}
}
