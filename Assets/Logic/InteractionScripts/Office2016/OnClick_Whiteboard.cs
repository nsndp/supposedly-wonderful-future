using UnityEngine;
using System.Collections;

public class OnClick_Whiteboard : MonoBehaviour {

	WhiteboardZoom WZ;

	void Start() {
		WZ = GameObject.Find("Data").GetComponent<WhiteboardZoom>();
	}
	
	void OnMouseDown() {
		WZ.phase = 0;
	}
}
