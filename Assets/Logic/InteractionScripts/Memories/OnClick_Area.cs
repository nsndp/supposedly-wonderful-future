using UnityEngine;
using System.Collections;

public class OnClick_Area : MonoBehaviour {

	DataControlChapter4 DC;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter4>();
	}

	void OnMouseDown() {
		DC.GetComponent<AreaZoom>().phase = 1;
	}
}
