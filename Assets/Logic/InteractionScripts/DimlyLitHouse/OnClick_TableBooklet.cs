using UnityEngine;
using System.Collections;

public class OnClick_TableBooklet : MonoBehaviour {

	TableZoom TZ;

	void Start() {
		TZ = GameObject.Find("Data").GetComponent<TableZoom>();
	}

	void OnMouseDown() {
		if (TZ.phase == 2) TZ.phase = 4;
	}

}
