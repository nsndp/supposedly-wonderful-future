using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewsFont : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {

	public bool isMinus = false;
	public NewsExpand NE;

	public void OnPointerEnter(PointerEventData d) {
		var c = GetComponent<Image>().color; c.a = 1; GetComponent<Image>().color = c;
	}
	public void OnPointerExit(PointerEventData d) {
		var c = GetComponent<Image>().color; c.a = 0.75F; GetComponent<Image>().color = c;
	}
	public void OnPointerDown(PointerEventData d) {
		NE.ChangeFont(isMinus);
	}
}
