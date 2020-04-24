using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MenuControlItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	MenuControl M;
	void Start() {
		M = transform.parent.parent.parent.GetComponent<MenuControl>();
	}

	public void OnPointerEnter(PointerEventData d) { M.OnItemEnter(transform.GetSiblingIndex()); }
	public void OnPointerExit(PointerEventData d) { M.OnItemExit(); }
}
