using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewsLeave : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	Color c = Color.white; Color ca = new Color(1, 0.6078F, 0);
	int phase = 0; float tc; float hs, hf;
	DataControlHub DC; AccessPoint AP; RectTransform area;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		AP = GameObject.Find("Room/Colliders/AccessPoint").GetComponent<AccessPoint>();
		area = transform.parent.GetComponent<RectTransform>();
	}

	public void OnPointerEnter(PointerEventData d) { if (!AP.newsInteraction) GetComponent<Text>().color = ca; }
	public void OnPointerExit(PointerEventData d) { GetComponent<Text>().color = c; }
	public void OnPointerClick(PointerEventData d) {
		if (phase == 0 && !AP.newsInteraction) {
			GetComponent<Text>().enabled = false;
			hs = transform.parent.GetComponent<RectTransform>().rect.height;
			hf = transform.parent.parent.GetComponent<RectTransform>().rect.height;
			transform.parent.Find("Control").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -hf);
			var RT = transform.parent.Find("Input").GetComponent<RectTransform>(); var v = RT.anchoredPosition;
			v.y = Mathf.RoundToInt(-hf/2); RT.anchoredPosition = v;
			RT = transform.parent.Find("Globe").GetComponent<RectTransform>(); v = RT.anchoredPosition;
			v.y = Mathf.RoundToInt(-hf/2); RT.anchoredPosition = v;
			DC.bReturn.SetActive(false); phase = 1; tc = 0;
		}
	}

	IEnumerator Waiting(float sec, int ph) {
		yield return new WaitForSeconds(sec);
		phase = ph;
	}

	void Update() {
		if (phase == 1 && tc <= 1) {
			area.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(hs, hf, Mathf.SmoothStep(0, 1, tc)));
			tc += 0.01F * Time.deltaTime * 60;
		}
		else if (phase == 1 && tc > 1) {
			phase = 2; StartCoroutine(Waiting(0.25F, 3));
		}
		else if (phase == 3) {
			if (DC.S.SH.triedBrowsing) DC.UIC.StartNarration(5);
			else { DC.UIC.StartNarration(0); DC.S.SH.triedBrowsing = true; }
			tc = 0; phase = 4;
		}
		else if (phase == 4 && !DC.S.inNarration) {
			phase = 5; StartCoroutine(Waiting(0.25F, 6));
		}
		else if (phase == 6 && tc <= 1) {
			area.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.Lerp(hf, hs, Mathf.SmoothStep(0, 1, tc)));
			tc += 0.01F * Time.deltaTime * 60;
		}
		else if (phase == 6 && tc > 1) {
			GetComponent<Text>().enabled = true;
			DC.bReturn.SetActive(true);
			phase = 0;
		}
	}
}
