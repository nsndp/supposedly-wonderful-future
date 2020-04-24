using UnityEngine;
using System.Collections;

public class OnClick_Blinds2 : MonoBehaviour {

	OnClick_BlindsControl BC;

	void Start() {
		BC = GameObject.Find("Data").GetComponent<OnClick_BlindsControl>();
	}
	
	void OnMouseDown() {
		BC.phase = 5;
	}
}
