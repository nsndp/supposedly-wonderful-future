using UnityEngine;
using System.Collections;

public class OnClick_Pinboard : MonoBehaviour {

	DataControlChapter1 DC;
	PinboardZoom PZ;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		PZ = DC.GetComponent<PinboardZoom>();
	}
	
	void OnMouseDown() {
		if (PZ.phase == -1) {
			if (DC.S.SC1.storyFinished) PZ.phase = 6;
			else if (!DC.S.SC1.checkedPinboard) PZ.phase = 0;
			else PZ.phase = 2;
		}
	}
}
