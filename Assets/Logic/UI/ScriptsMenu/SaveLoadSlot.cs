using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SaveLoadSlot : MonoBehaviour, IPointerDownHandler, IPointerClickHandler {

	float lastClickTime = 0;

	public void OnPointerDown(PointerEventData d) {
		transform.parent.parent.GetComponent<SaveLoad>().OnSlot(transform.GetSiblingIndex()-2);
	}

	public void OnPointerClick(PointerEventData d) {
		//it's a double click
		if (Time.time - lastClickTime < 0.5F)
			transform.parent.parent.GetComponent<SaveLoad>().OnSlotDouble();
		else lastClickTime = Time.time;
	}
}
