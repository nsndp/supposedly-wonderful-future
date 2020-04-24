using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LanguageSelect : MonoBehaviour {

	Text EN, RU; MenuControl MC;
	int langActivator = 0;

	void Init() {
		MC = transform.parent.parent.GetComponent<MenuControl>();
		EN = transform.Find("EN").GetComponent<Text>();
		RU = transform.Find("RU").GetComponent<Text>();
	}

	public void OnAreaEnter(BaseEventData d) {
		transform.GetChild(0).gameObject.SetActive(true);
	}

	public void OnAreaExit(BaseEventData d) {
		if (COMMON.U.languageID == 0 && EN.transform.GetSiblingIndex() != 1) EN.transform.SetAsLastSibling();
		else if (COMMON.U.languageID == 1 && RU.transform.GetSiblingIndex() != 1) RU.transform.SetAsLastSibling();
		transform.GetChild(0).gameObject.SetActive(false);
		RestoreColorsAfterHovering();
	}

	public void SetOnLoad(int langID) {
		Init();
		var c = EN.color; c.a = langID == 0 ? 1 : 0.5F; EN.color = c;
		c = RU.color; c.a = langID == 1 ? 1 : 0.5F; RU.color = c;
		EN.gameObject.SetActive(langID == 0); if (langID == 0) EN.transform.SetAsLastSibling();
		RU.gameObject.SetActive(langID == 1); if (langID == 1) RU.transform.SetAsLastSibling();
	}

	private void RestoreColorsAfterHovering() {
		var c = RU.color; c.r = 0.733F; c.g = 0.878F; c.b = 1; RU.color = c;
		c = EN.color; c.r = 0.733F; c.g = 0.878F; c.b = 1; EN.color = c;
	}

	public void OnEnterRU(BaseEventData e) { if (COMMON.U.languageID != 1) { var c = RU.color; c.r = 0.188F; c.g = 0.647F; c.b = 1; RU.color = c; } }
	public void OnExitRU(BaseEventData d) { if (COMMON.U.languageID != 1) {	var c = RU.color; c.r = 0.733F; c.g = 0.878F; c.b = 1; RU.color = c; } }
	public void OnEnterEN(BaseEventData e) { if (COMMON.U.languageID != 0) { var c = EN.color; c.r = 0.188F; c.g = 0.647F; c.b = 1; EN.color = c; } }
	public void OnExitEN(BaseEventData e) {	if (COMMON.U.languageID != 0) {	var c = EN.color; c.r = 0.733F; c.g = 0.878F; c.b = 1; EN.color = c; } }

	public void OnClickRU(BaseEventData d) {
		if (COMMON.U.languageID != 1) {
			MC.PlaySFX(uisounds.save);
			LanguageControl.Translate(1, -1);
			COMMON.U.languageID = 1; COMMON.U.Save(COMMON.saveFolder + "UserSettings.bin");
			var c = RU.color; c.a = 1; RU.color = c; c = EN.color; c.a = 0.5F; EN.color = c;
			RestoreColorsAfterHovering();
		}
	}

	public void OnClickEN(BaseEventData d) {
		if (COMMON.U.languageID != 0) {
			MC.PlaySFX(uisounds.save);
			LanguageControl.RestoreENG();
			COMMON.U.languageID = 0; COMMON.U.Save(COMMON.saveFolder + "UserSettings.bin");
			var c = EN.color; c.a = 1; EN.color = c; c = RU.color; c.a = 0.5F; RU.color = c;
			RestoreColorsAfterHovering();
		}
	}

	void Update() {
		//language switch testing
		/*if (Input.GetKey(KeyCode.LeftShift)) {
			if (Input.GetKeyDown(KeyCode.L) && langActivator == 0) langActivator = 1;
			else if (Input.GetKeyDown(KeyCode.A) && langActivator == 1) langActivator = 2;
			else if (Input.GetKeyDown(KeyCode.N) && langActivator == 2) langActivator = 3;
			else if (Input.GetKeyDown(KeyCode.G) && langActivator == 3) {
				langActivator = 0;
				if (COMMON.U.languageID == 0) OnClickRU(null); else OnClickEN(null);
				SetOnLoad(COMMON.U.languageID);
				COMMON.U.Save(COMMON.saveFolder + "UserSettings.bin");
			}
		}*/
	}
}
