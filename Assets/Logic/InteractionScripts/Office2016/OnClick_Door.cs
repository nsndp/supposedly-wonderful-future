using UnityEngine;
using System.Collections;

public class OnClick_Door : MonoBehaviour {
	void OnMouseDown() {
		this.GetComponent<HighlightOnHover>().HighlightEffect.SetActive(false);
		GameObject.Find("Colliders/Jackie").GetComponent<OnClick_Jackie>().phase = 1;
	}
}
