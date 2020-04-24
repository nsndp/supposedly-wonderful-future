using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Arrows : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

	public bool isUp;
	UIControl s; Image I;
	public bool inside = false;
	bool pressed = false;
	float t = 0;
	float speed = 0.3F;

	void Start() {
		s = GameObject.Find("UI").GetComponent<UIControl>();
		I = GetComponent<Image>();
	}

	public void OnPointerEnter(PointerEventData d) { I.color = s.cActive; inside = true; }
	public void OnPointerExit(PointerEventData d) { I.color = s.cMain; inside = false; pressed = false; }
	public void OnPointerDown(PointerEventData d) { pressed = true; t = speed; }
	public void OnPointerUp(PointerEventData d) { pressed = false; }

	void Update() {
		if (inside && pressed) {
			t += Time.deltaTime;
			if (t >= speed) {
				t = 0;
				if (isUp && s.rtop > 0) { s.rtop--; s.SetResponsesVisibility(); }
				else if (!s.R[s.rcount-1].IsActive()) { s.rtop++; s.SetResponsesVisibility(); }
				if (!this.gameObject.activeSelf) { inside = false; pressed = false; }
			}
		}
	}
}
