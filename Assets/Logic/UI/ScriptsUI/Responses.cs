using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Responses : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public int chosenID;
	DataControl DC; UIControl s;
	float scale;
	bool inside;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControl>();
		s = GameObject.Find("UI").GetComponent<UIControl>();
		scale = 1; //Screen.width / 1280.0F;
	}

	public void OnPointerEnter(PointerEventData d) { }
	public void OnPointerExit(PointerEventData d) {
		if (GetComponent<Text>().color.a >= 1) {
			GetComponent<Text>().color = s.cMain;
			if (s.rcur != -1) { s.R[s.rcur].color = s.cActive; s.rcur = -1; }
			s.inHover = false;
		}
	}
	
	public void OnPointerDown(PointerEventData d) {
		if (GetComponent<Text>().color.a >= 1) {
			GetComponent<Text>().color = s.cMain;
			s.id = chosenID; s.phase = 9;
			s.inHover = false;
		}
	}

	void Update() {
		//this is on hover - like on enter, but also works when the object is becoming active with the cursor already over it
		if (!DC.paused && gameObject.activeSelf && GetComponent<Text>().color.a >= 1 &&
			Input.mousePosition.x > transform.position.x &&
			Input.mousePosition.x < transform.position.x + GetComponent<RectTransform>().rect.width * scale &&
			Input.mousePosition.y > transform.position.y - GetComponent<RectTransform>().rect.height * scale &&
			Input.mousePosition.y < transform.position.y &&
			!s.ArrowU.GetComponent<Arrows>().inside &&
			!s.ArrowD.GetComponent<Arrows>().inside &&
		    Mathf.RoundToInt(GetComponent<Text>().color.r * 255) == Mathf.RoundToInt(s.cMain.r * 255) &&
		    Mathf.RoundToInt(GetComponent<Text>().color.g * 255) == Mathf.RoundToInt(s.cMain.g * 255) &&
		    Mathf.RoundToInt(GetComponent<Text>().color.b * 255) == Mathf.RoundToInt(s.cMain.b * 255)) {
				GetComponent<Text>().color = s.cActive;
				if (s.rcur != -1) { s.R[s.rcur].color = s.cMain; s.rcur = -1; }
				s.inHover = true;
			}
	}
}
