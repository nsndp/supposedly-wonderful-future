using UnityEngine;
using System.Collections;

public class HighlightOnHover : MonoBehaviour {

	public GameObject HighlightEffect;
	DataControl DC;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControl>();
	}

	void OnMouseEnter() {
		if (Cursor.visible) {
			HighlightEffect.SetActive(true);
			DC.activeHL = HighlightEffect;
		}
	}

	void OnMouseExit() {
		if (Cursor.visible) {
			HighlightEffect.SetActive(false);
			DC.activeHL = null;
		}
	}
}
