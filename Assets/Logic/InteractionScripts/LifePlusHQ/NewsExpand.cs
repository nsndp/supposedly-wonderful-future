using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NewsExpand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	int day, story;
	DataControlHub DC; AccessPoint AP; NewsControl NC; Image I, C; GameObject P; NewsFont ND, NI;
	Transform ArrowL, ArrowR; bool turnBackArrowL = false, turnBackArrowR = false;
	int phase = 0; float tc; Color c;

	public void Init() {
		day = System.Int32.Parse(transform.parent.parent.name.Replace("News", "")) - 1;
		story = System.Int32.Parse(transform.name.Replace("Panel", "")) - 1;
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		AP = DC.room.Find("Colliders/AccessPoint").GetComponent<AccessPoint>();
		NC = DC.room.Find("Screens").GetComponent<NewsControl>();
		I = GetComponent<Image>();
		C = transform.parent.Find("Cover").GetComponent<Image>();
		P = transform.parent.Find("PanelExpanded").gameObject;
		NI = P.transform.Find("FullArea/FontIncrease").GetComponent<NewsFont>();
		ND = P.transform.Find("FullArea/FontDecrease").GetComponent<NewsFont>();
		ArrowL = transform.parent.parent.Find("Header/Date/Prev");
		ArrowR = transform.parent.parent.Find("Header/Date/Next");
	}

	public void OnPointerEnter(PointerEventData d) { if (phase < 3) phase = 1; }
	public void OnPointerExit(PointerEventData d) { if (phase < 3) phase = 2; }
	public void OnPointerClick(PointerEventData d) {
		if (!AP.newsInteraction && AP.phase == 10) {
			NC.LoadStory(day, story); AP.newsInteraction = true;
			NI.gameObject.SetActive(DC.S.SH.newsFontSize < 3); NI.NE = GetComponent<NewsExpand>();
			ND.gameObject.SetActive(DC.S.SH.newsFontSize > 0); ND.NE = GetComponent<NewsExpand>();
			if (ArrowL.gameObject.activeSelf) { ArrowL.gameObject.SetActive(false); turnBackArrowL = true; }
			if (ArrowR.gameObject.activeSelf) { ArrowR.gameObject.SetActive(false); turnBackArrowR = true; }
			c = C.color; c.a = 0; C.color = c; C.gameObject.SetActive(true);
			DC.bReturn.SetActive(false); phase = 3;
		}
	}

	public void ChangeFont(bool isMinus) {
		if (phase == 5 && (isMinus && DC.S.SH.newsFontSize > 0 || !isMinus && DC.S.SH.newsFontSize < 3)) {
			if (!isMinus) {
				DC.S.SH.newsFontSize++;
				c = NI.GetComponent<Image>().color; c.a = 0.75F; NI.GetComponent<Image>().color = c;
			} else {
				DC.S.SH.newsFontSize--;
				c = ND.GetComponent<Image>().color; c.a = 0.75F; ND.GetComponent<Image>().color = c;
			}
			c = C.color; c.a = 0; C.color = c; C.gameObject.SetActive(true);
			DC.bReturn.SetActive(false); phase = 8;
		}
	}

	void Update() {
		if (!AP.newsInteraction && phase == 1 && I.color.a < 0.35F) {
			c = I.color; c.a += 0.014F * Time.deltaTime * 60; I.color = c;
		}
		else if (!AP.newsInteraction && phase == 2 && I.color.a > 0) {
			c = I.color; c.a -= 0.014F * Time.deltaTime * 60; I.color = c;
		}
		else if (phase == 1 && I.color.a >= 0.35F) phase = 0;
		else if (phase == 2 && I.color.a <= 0) phase = 0;
		else if (phase == 3 && C.color.a < 1) {
			c = C.color; c.a += 0.025F * Time.deltaTime * 60; C.color = c;
			if (I.color.a > 0) { c = I.color; c.a -= 0.014F * Time.deltaTime * 60; I.color = c; }
		}
		else if (phase == 3) {
			P.gameObject.SetActive(true); phase = 4;
		}
		else if (phase == 4 && C.color.a > 0) {
			c = C.color; c.a -= 0.025F * Time.deltaTime * 60; C.color = c;
		}
		else if (phase == 4) {
			C.gameObject.SetActive(false); DC.bReturn.SetActive(true);
			if (!DC.S.SH.readNews[day][story]) {
				DC.S.SH.readNews[day][story] = true;
				var all = true; for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) if (!DC.S.SH.readNews[i][j]) all = false;
				if (all) DC.MC.STEAM.Achievement("ACH_NEWS");
			}
			if (day == 0 && story == 0) DC.S.SH.DSJChats.Locked[375] = false;
			phase = 5;
		}
		else if (phase == 5 && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) { 
			C.gameObject.SetActive(true);
			DC.bReturn.GetComponent<ButtonArrow>().clicked = false;
			DC.bReturn.SetActive(false);
			phase = 6;
		}
		else if (phase == 6 && C.color.a < 1) {
			c = C.color; c.a += 0.025F/*0.04F*/ * Time.deltaTime * 60; C.color = c;
		}
		else if (phase == 6) {
			P.gameObject.SetActive(false); phase = 7;
		}
		else if (phase == 7 && C.color.a > 0) {
			c = C.color; c.a -= 0.025F/*0.04F*/ * Time.deltaTime * 60; C.color = c;
		}
		else if (phase == 7) {
			C.gameObject.SetActive(false);
			if (turnBackArrowL) ArrowL.gameObject.SetActive(true);
			if (turnBackArrowR) ArrowR.gameObject.SetActive(true);
			DC.bReturn.SetActive(true); phase = 0; AP.newsInteraction = false;
		}

		//font change
		else if (phase == 8 && C.color.a < 1) {
			c = C.color; c.a += 0.025F * Time.deltaTime * 60; C.color = c;
		}
		else if (phase == 8 && C.color.a >= 1) {
			NI.gameObject.SetActive(DC.S.SH.newsFontSize < 3);
			ND.gameObject.SetActive(DC.S.SH.newsFontSize > 0);
			NC.LoadStory(day, story); P.gameObject.SetActive(true); phase = 9;
		}
		else if (phase == 9 && C.color.a > 0) {
			c = C.color; c.a -= 0.025F * Time.deltaTime * 60; C.color = c;
		}
		else if (phase == 9 && C.color.a <= 0) {
			C.gameObject.SetActive(false); DC.bReturn.SetActive(true); phase = 5;
		}
	}
}