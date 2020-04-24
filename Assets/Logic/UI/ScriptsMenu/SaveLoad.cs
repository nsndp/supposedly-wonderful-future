using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class SaveLoad : MonoBehaviour {
	public Texture2D[] autosaveScreens;
	SaveNames SN; bool[] isEmpty; string curName; int curLevel;
	string SF; bool isLoad; int page, listLength; int activeSlot = -10; int activePopup = -1;
	bool AS = false; bool QS = false; float lastClickTimeAuto = 0; float lastClickTimeQuick = 0;
	int ri, riMax; bool rwHot = false;
	Image LCNext, LCPrev, RWNext, RWPrev;
	GameObject BS, BL, BR, BD, BB, Dt, NoDt, LCText, PB, P0, P1, P2, P3;
	Text PIY, PIN, POY, PON, PDY, PDN, PRY, PRN; //POY = Popup Overwrite Yes
	Transform L; InputField IF; Text DtName, DtChap, DtLoc, DtTime, RewindCPName; Image DtImg;
	Color defaultBlue = new Color(0.188F, 0.647F, 1); Color brighterBlue = new Color(0.718F, 0.878F, 1);
	GUIStyle G; string nmPrev; float IFW; string dashes = "----------------";
	MenuControl MC; DataControl DC;

	System.Globalization.CultureInfo culture;
	[System.NonSerialized] public string autoSTR = "Autosave";
	[System.NonSerialized] public string quickSTR = "Quick Save";
	[System.NonSerialized] public string defSTR = "New Save";
	[System.NonSerialized] public string pageSTR = "Page [X]";
	[System.NonSerialized] public string[] chapterSTR = new string[] {"Prologue", "Chapter [X]"};
	[System.NonSerialized] public string[] locationSTR = new string[] {
		"Office 2018", "Dimly Lit House", "Cyberbullying Investigation Unit",
		"Underground Facilities", "Memory Corner", "CEO's Office", "LIFE+ HQ"
	};
	[System.NonSerialized] public string[] rewindCP = new string[] {
		"Day 1 - Office", "Day 1 - Dimly Lit House", "Day 1 - LIFE+ HQ", "Day 2 - Investigation Unit",
		"Day 2 - Return to HQ", "Day 3 - Elevator Ride", "Day 3 - Return to HQ", "Day 4 - Memory Corner",
		"Day 4 - Return to HQ", "Day 5 - CEO's office"
	};

	void Init() {
		L = transform.Find("List");
		BL = transform.Find("Buttons/Load").gameObject; BS = transform.Find("Buttons/Save").gameObject;
		BD = transform.Find("Buttons/Delete").gameObject; BB = transform.Find("Buttons/Back").gameObject;
		BR = transform.Find("Buttons/Rewind").gameObject;
		IF = transform.Find("InputField").GetComponent<InputField>();
		Dt = transform.Find("Details").gameObject; NoDt = transform.Find("NoDetails").gameObject;
		DtName = transform.Find("Details/Main/Name").GetComponent<Text>();
		DtChap = transform.Find("Details/Main/Chapter").GetComponent<Text>();
		DtLoc = transform.Find("Details/Main/Location").GetComponent<Text>();
		DtTime = transform.Find("Details/Main/DateTime").GetComponent<Text>();
		DtImg = transform.Find("Details/Main/Image").GetComponent<Image>();
		RewindCPName = transform.Find("PopupRewind/Checkpoint").GetComponent<Text>();
		PB = transform.Find("PopupBckg").gameObject;
		P0 = transform.Find("PopupInput").gameObject; P1 = transform.Find("PopupOverwrite").gameObject;
		P2 = transform.Find("PopupDelete").gameObject; P3 = transform.Find("PopupRewind").gameObject;
		LCText = transform.Find("ListControl/Text").gameObject;
		LCNext = transform.Find("ListControl/Next").GetComponent<Image>(); LCPrev = transform.Find("ListControl/Prev").GetComponent<Image>();
		RWNext = transform.Find("PopupRewind/Next").GetComponent<Image>(); RWPrev = transform.Find("PopupRewind/Prev").GetComponent<Image>();
		PIY = transform.Find("PopupInput/Confirm").GetComponent<Text>(); PIN = transform.Find("PopupInput/Cancel").GetComponent<Text>();
		POY = transform.Find("PopupOverwrite/Yes").GetComponent<Text>(); PON = transform.Find("PopupOverwrite/No").GetComponent<Text>();
		PDY = transform.Find("PopupDelete/Yes").GetComponent<Text>(); PDN = transform.Find("PopupDelete/No").GetComponent<Text>();
		PRY = transform.Find("PopupRewind/Confirm").GetComponent<Text>(); PRN = transform.Find("PopupRewind/Cancel").GetComponent<Text>();
		G = new GUIStyle(); G.font = IF.textComponent.font; G.fontSize = IF.textComponent.fontSize;
		G.fontStyle = IF.textComponent.fontStyle; G.wordWrap = true; G.richText = true;
		MC = transform.root.Find("Menu").GetComponent<MenuControl>();
		DC = GameObject.Find("Data").GetComponent<DataControl>();
		SF = COMMON.saveFolder;
		SN = SaveNames.Load(COMMON.saveFolder + "SaveNames.xml"); if (SN == null) SN = new SaveNames();
	}

	bool IfStartedNewChapter(SaveGame S) {
		if (S.SH != null && (S.levelID == 11 && S.SH.startedCH2 || S.levelID == 12 && S.SH.startedCH3 || S.levelID == 14 && S.SH.startedCH5)) return true;
		return false;
	}
	string GetChapter(int lid, bool ChSw) {
		if (lid == 0) return chapterSTR[0];
		int c;
		if (lid <= 5) c = lid;
		else if (lid == 11 && ChSw) c = 2; else if (lid == 11) c = 1;
		else if (lid == 12 && ChSw) c = 3; else if (lid == 12) c = 2;
		else if (lid == 13) c = 3;
		else if (lid == 14 && ChSw) c = 5; else c = 4;
		return chapterSTR[1].Replace("[X]", c.ToString());
	}
	string GetLocation(int lid) {
		if (lid <= 5) return locationSTR[lid];
		return locationSTR[6];
	}

	/* ---------------------------------- NAVIGATION ----------------------------------------- */
	void LoadPage(int autoselect = -10) {
		bool updateSN = false;
		for (int i = 0; i < listLength; i++) {
			int k = page * listLength + i + 1;
			string s; if (k < 10) s = "Save0" + k + ".bin"; else s = "Save" + k + ".bin";
			if (!File.Exists(SF + s)) {
				isEmpty[i] = true;
				L.transform.GetChild(i+2).GetComponent<Text>().text = k + ". " + dashes;
			} else {
				isEmpty[i] = false; var namae = SN.GetName(k);
				if (namae == null) {
					var S = SaveGame.Load(SF + s); namae = S.Name;
					SN.list.Add(new Namae(k, namae)); updateSN = true;
				}
				L.transform.GetChild(i+2).GetComponent<Text>().text = k + ". " + namae;
			}
		}
		LCText.GetComponent<Text>().text = pageSTR.Replace("[X]", (page + 1).ToString());
		if (updateSN) SN.Save(COMMON.saveFolder + "SaveNames.xml");

		if (autoselect >= -2) SelectSlot(autoselect, true);
		else {
			if (activeSlot >= -2) L.GetChild(activeSlot+2).GetComponent<Text>().color = Color.white;
			BL.SetActive(false); BS.SetActive(false); BR.SetActive(false); BD.SetActive(false);
			Dt.SetActive(false); NoDt.SetActive(false);
			activeSlot = -10;
		}
	}
	void SelectSlot(int ind, bool ignoreSound = false) {
		//var now = System.DateTime.Now;
		if (!ignoreSound) MC.PlaySFX(uisounds.click1);
		if (activeSlot >= -2) L.GetChild(activeSlot+2).GetComponent<Text>().color = Color.white;
		activeSlot = ind; curName = null;
		L.GetChild(ind+2).GetComponent<Text>().color = defaultBlue;

		Dt.SetActive(ind < 0 || !isEmpty[ind]); NoDt.SetActive(!Dt.activeSelf);
		BL.SetActive(isLoad && Dt.activeSelf); BR.SetActive(isLoad && Dt.activeSelf);
		BS.SetActive(!isLoad); BD.SetActive(ind >= 0 && !isEmpty[ind]);

		if (ind < 0 || !isEmpty[ind]) {
			string filename = ind == -2 ? "Autosave" : (ind == -1 ? "QuickSave" : "");
			if (ind >= 0) {
				int k = page * listLength + ind + 1;
				if (k < 10) filename = "Save0" + k; else filename = "Save" + k;
			}
			var S = SaveGame.Load(SF + filename + ".bin");
			curName = ind >= 0 ? S.Name : (ind == -2 ? autoSTR : quickSTR); curLevel = S.levelID;
			DtName.text = curName; DtTime.text = File.GetLastWriteTime(SF + filename + ".bin").ToString("MMMM dd, yyyy, H:mm:ss", culture);
			DtChap.text = GetChapter(curLevel, IfStartedNewChapter(S)); DtLoc.text = GetLocation(curLevel);

			if (COMMON.demoVersion) {
				bool fullOnly = !(S.SH == null || !S.SH.startedCH2);
				Dt.transform.Find("Main").gameObject.SetActive(!fullOnly);
				Dt.transform.Find("Demo").gameObject.SetActive(fullOnly);
				if (fullOnly) { BL.SetActive(false); BR.SetActive(false); BD.SetActive(false); return; }
			}

			Texture2D t = null;
			if (ind == -2) {
				int I = curLevel; if (curLevel == 11) I = 6; else if (curLevel > 11) I = 7;
				t = autosaveScreens[I];
			}
			else if (File.Exists(SF + "Images/" + filename + ".jpg")) {
				t = new Texture2D(2, 2); t.LoadImage(File.ReadAllBytes(SF + "Images/" + filename + ".jpg"));
			}
			if (t == null) { DtImg.color = Color.black; DtImg.sprite = null; }
			else {
				DtImg.color = Color.white;
				DtImg.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
				var h = Mathf.RoundToInt(DtImg.GetComponent<RectTransform>().rect.height);
				var w = Mathf.RoundToInt(t.width * h * 1.0F / t.height);
				DtImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
			}
		}
		//var ts = System.DateTime.Now - now; Debug.Log(ts.Seconds + "." + ts.Milliseconds);
	}

	/* ----------------------------- on choosing Save/Load in menu -------------------------------------- */
	public void Show(bool isLoad) {
		if (L == null) Init();
		culture = new System.Globalization.CultureInfo(LanguageControl.CultureCode(COMMON.U.languageID));
		BD.GetComponent<RectTransform>().anchorMin = new Vector2(isLoad ? 0.625F : 0.5F, 0.5F);
		BD.GetComponent<RectTransform>().anchorMax = BD.GetComponent<RectTransform>().anchorMin;
		BB.GetComponent<RectTransform>().anchorMin = new Vector2(isLoad ? 0.875F : 0.834F, 0.5F);
		BB.GetComponent<RectTransform>().anchorMax = BB.GetComponent<RectTransform>().anchorMin;
		BL.SetActive(isLoad); BR.SetActive(isLoad); BS.SetActive(!isLoad);
		
		listLength = L.childCount - 2; isEmpty = new bool[listLength];
		AS = File.Exists(SF + "Autosave.bin");
		QS = File.Exists(SF + "QuickSave.bin");
		if (isLoad && AS) L.GetChild(0).gameObject.SetActive(true); else L.GetChild(0).gameObject.SetActive(false);
		if (isLoad && QS) L.GetChild(1).gameObject.SetActive(true); else L.GetChild(1).gameObject.SetActive(false);

		IFW = Mathf.RoundToInt(transform.GetComponent<RectTransform>().sizeDelta.x * 0.34F);
		IFW -= G.CalcSize(new GUIContent("100. ")).x + 20;
		var s = IF.GetComponent<RectTransform>().sizeDelta; s.x = IFW; IF.GetComponent<RectTransform>().sizeDelta = s;
		dashes = ""; while (G.CalcSize(new GUIContent(dashes + "-")).x <= IFW) dashes += "-";

		this.isLoad = isLoad;
		//auto-selecting the most recent save
		var info = new DirectoryInfo(SF).GetFiles(isLoad ? "*Save*.bin" : "Save*.bin");
		System.Array.Sort(info, Comparison);
		if (info.Length == 0) { page = 0; LoadPage(); }
		else if (info[0].Name.ToLowerInvariant() == "autosave.bin") { page = 0; LoadPage(-2); }
		else if (info[0].Name.ToLowerInvariant() == "quicksave.bin") { page = 0; LoadPage(-1); }
		else if (info[0].Name.ToLowerInvariant() == "save100.bin") { page = 99 / listLength; LoadPage(listLength - 1); }
		else {
			//Debug.Log(info[0].Name);
			int k = System.Int32.Parse(info[0].Name.Substring(4, 2));
			page = (k-1) / listLength;
			LoadPage(k-1-listLength*page);
		}
	}
	public int Comparison(FileInfo A, FileInfo B) {
		if (A.LastWriteTime > B.LastWriteTime) return - 1;
		if (A.LastWriteTime < B.LastWriteTime) return 1;
		return 0;
	}

	/* ---------------------------------- BUTTONS ----------------------------------------- */
	public void OnNext(BaseEventData d) { page++; if (page > 99 / listLength) page = 0; LoadPage(); MC.PlaySFX(uisounds.click1); }
	public void OnPrev(BaseEventData d) { page--; if (page < 0) page = 99 / listLength; LoadPage(); MC.PlaySFX(uisounds.click1); }
	public void OnSlot(int ind) { if (activeSlot != ind) SelectSlot(ind); }
	public void OnSlotDouble() { if (!isLoad) OnSave(null); else if (BL.activeSelf) Load(activeSlot); }
	public void OnQuick(BaseEventData d) {
		if (activeSlot != -1) SelectSlot(-1);
		if (Time.time - lastClickTimeQuick < 0.5F) Load(-1); else lastClickTimeQuick = Time.time;
	}
	public void OnAuto(BaseEventData d) {
		if (activeSlot != -2) SelectSlot(-2);
		if (Time.time - lastClickTimeAuto < 0.5F) Load(-2); else lastClickTimeAuto = Time.time;
	}

	public void OnPrevEnter(BaseEventData d) { LCPrev.color = brighterBlue; }
	public void OnPrevExit(BaseEventData d) { LCPrev.color = Color.white; }
	public void OnNextEnter(BaseEventData d) { LCNext.color = brighterBlue; }
	public void OnNextExit(BaseEventData d) { LCNext.color = Color.white; }
	public void OnPrevRewindEnter(BaseEventData d) { RWPrev.color = brighterBlue; }
	public void OnPrevRewindExit(BaseEventData d) { RWPrev.color = Color.white; }
	public void OnNextRewindEnter(BaseEventData d) { RWNext.color = brighterBlue; }
	public void OnNextRewindExit(BaseEventData d) { RWNext.color = Color.white; }

	public void OnSaveEnter(BaseEventData d) { BS.GetComponent<Text>().color = defaultBlue; }
	public void OnSaveExit(BaseEventData d) { BS.GetComponent<Text>().color = Color.white; }
	public void OnLoadEnter(BaseEventData d) { BL.GetComponent<Text>().color = defaultBlue; }
	public void OnLoadExit(BaseEventData d) { BL.GetComponent<Text>().color = Color.white; }
	public void OnRewindEnter(BaseEventData d) { BR.GetComponent<Text>().color = defaultBlue; }
	public void OnRewindExit(BaseEventData d) { BR.GetComponent<Text>().color = Color.white; }
	public void OnDeleteEnter(BaseEventData d) { BD.GetComponent<Text>().color = defaultBlue; }
	public void OnDeleteExit(BaseEventData d) { BD.GetComponent<Text>().color = Color.white; }
	public void OnBackEnter(BaseEventData d) { BB.GetComponent<Text>().color = defaultBlue; }
	public void OnBackExit(BaseEventData d) { BB.GetComponent<Text>().color = Color.white; }

	public void OnOverwriteNoEnter(BaseEventData d) { PON.color = defaultBlue; POY.color = Color.white; }
	public void OnOverwriteNoExit(BaseEventData d) { PON.color = Color.white; }
	public void OnOverwriteYesEnter(BaseEventData d) { POY.color = defaultBlue; PON.color = Color.white; }
	public void OnOverwriteYesExit(BaseEventData d) { POY.color = Color.white; }
	public void OnDeleteNoEnter(BaseEventData d) { PDN.color = defaultBlue; PDY.color = Color.white; }
	public void OnDeleteNoExit(BaseEventData d) { PDN.color = Color.white; }
	public void OnDeleteYesEnter(BaseEventData d) { PDY.color = defaultBlue; PDN.color = Color.white; }
	public void OnDeleteYesExit(BaseEventData d) { PDY.color = Color.white; }
	public void OnInputCancelEnter(BaseEventData d) { PIN.color = defaultBlue; }
	public void OnInputCancelExit(BaseEventData d) { PIN.color = Color.white; }
	public void OnInputConfirmEnter(BaseEventData d) { PIY.color = defaultBlue; }
	public void OnInputConfirmExit(BaseEventData d) { PIY.color = Color.white; }
	public void OnRewindCancelEnter(BaseEventData d) { PRN.color = defaultBlue; PRY.color = Color.white; }
	public void OnRewindCancelExit(BaseEventData d) { PRN.color = Color.white; }
	public void OnRewindConfirmEnter(BaseEventData d) { PRY.color = defaultBlue; PRN.color = Color.white; }
	public void OnRewindConfirmExit(BaseEventData d) { PRY.color = Color.white; }

	//-------------------- LOAD-BACK-DELETE --------------------
	public void OnLoad(BaseEventData d) { MC.PlaySFX(uisounds.go); Load(activeSlot); }
	public void OnBack(BaseEventData d) {
		gameObject.SetActive(false); MC.submenuOpened = false; MC.PlaySFX(uisounds.back);
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
			transform.root.Find("Menu/Pause").gameObject.SetActive(true);
		else transform.root.Find("Menu/Main").gameObject.SetActive(true);
		BB.GetComponent<Text>().color = Color.white; LCNext.color = Color.white; LCPrev.color = Color.white;
		BD.GetComponent<Text>().color = Color.white; BR.GetComponent<Text>().color = Color.white;
		BL.GetComponent<Text>().color = Color.white; BS.GetComponent<Text>().color = Color.white;
	}
	public void OnBackPopup(BaseEventData d) {
		PB.SetActive(false); MC.PlaySFX(uisounds.back);
		if (activeSlot >= 0 && isEmpty[activeSlot]) NoDt.SetActive(true); else Dt.SetActive(true);
		if (activePopup == 0) { P0.SetActive(false); PIN.color = Color.white; }
		else if (activePopup == 1) { P1.SetActive(false); PON.color = Color.white; POY.color = Color.white; }
		else if (activePopup == 2) { P2.SetActive(false); PDN.color = Color.white; PDY.color = Color.white; }
		else if (activePopup == 3) { P3.SetActive(false); PRN.color = Color.white; PRY.color = Color.white; RWNext.color = Color.white; RWPrev.color = Color.white; }
		if (activePopup == 0) {
			IF.DeactivateInputField(); IF.gameObject.SetActive(false);
			var T = L.GetChild(activeSlot+2).GetComponent<Text>();
			if (curName == null) T.text += dashes; //cancel when naming a new file
			else T.text += curName; //cancel when overwriting an existing file
			T.color = defaultBlue;
		}
		activePopup = -1;
	}
	public void OnDelete(BaseEventData d) {
		MC.PlaySFX(uisounds.click2);
		Dt.SetActive(false); NoDt.SetActive(false);
		PB.SetActive(true); P2.SetActive(true); activePopup = 2;
	}
	public void OnDeleteConfirm(BaseEventData d) { 
		MC.PlaySFX(uisounds.delete);
		P2.SetActive(false); PDY.color = Color.white;
		PB.SetActive(false); activePopup = -1;
		int k = page * listLength + activeSlot + 1;
		string f; if (k < 10) f = "Save0" + k; else f = "Save" + k;
		File.Delete(SF + f + ".bin");
		File.Delete(SF + "Images/" + f + ".jpg");
		SN.list.Remove(SN.Get(k)); SN.Save(COMMON.saveFolder + "SaveNames.xml");
		isEmpty[activeSlot] = true;
		L.GetChild(activeSlot+2).GetComponent<Text>().text = k + ". " + dashes;
		BL.SetActive(false); BR.SetActive(false); BD.SetActive(false);
		Dt.SetActive(false); NoDt.SetActive(true);
	}
	//-------------------- SAVE --------------------
	public void OnSave(BaseEventData d) {
		MC.PlaySFX(uisounds.click2);
		PB.SetActive(true); Dt.SetActive(false); NoDt.SetActive(false);
		if (!isEmpty[activeSlot]) { P1.SetActive(true); activePopup = 1; }
		else { P0.SetActive(true); activePopup = 0; InputActivate(defSTR); }
	}
	public void OnOverwrite(BaseEventData d) {
		MC.PlaySFX(uisounds.click2);
		P1.SetActive(false); POY.color = Color.white;
		P0.SetActive(true); activePopup = 0;
		InputActivate(curName);
	}
	public void OnSaveOK(BaseEventData d) {
		MC.PlaySFX(uisounds.save);
		IF.DeactivateInputField(); IF.gameObject.SetActive(false);
		P0.SetActive(false); PIY.color = Color.white;
		PB.SetActive(false); activePopup = -1;
		Save(activeSlot);
	}

	//------------------- REWIND --------------------
	public void OnRewind(BaseEventData d) {
		MC.PlaySFX(uisounds.click2);
		switch (curLevel) { case 0: riMax = 0; break; case 1: riMax = 1; break;
		case 11: riMax = 2; break; case 2: riMax = 3; break; case 12: riMax = 4; break;
		case 3: riMax = 5; break; case 13: riMax = 6; break; case 4: riMax = 7; break;
		case 14: riMax = 8; break; case 5: riMax = 9; break; default: break;
		}
		ri = riMax; RewindCPName.text = rewindCP[ri];
		RWPrev.gameObject.SetActive(ri > 0); RWNext.gameObject.SetActive(false);
		PB.SetActive(true); Dt.SetActive(false); P3.SetActive(true); activePopup = 3;
	}
	public void OnRewindPrev(BaseEventData d) {
		MC.PlaySFX(uisounds.click1);
		ri--; RewindCPName.text = rewindCP[ri];
		RWPrev.gameObject.SetActive(ri > 0); if (ri == 0) RWPrev.color = Color.white;
		RWNext.gameObject.SetActive(true);
	}
	public void OnRewindNext(BaseEventData d) {
		MC.PlaySFX(uisounds.click1);
		ri++; RewindCPName.text = rewindCP[ri];
		RWPrev.gameObject.SetActive(true);
		RWNext.gameObject.SetActive(ri < riMax); if (ri == riMax) RWNext.color = Color.white;
	}
	public void OnRewindConfirm(BaseEventData d) {
		MC.PlaySFX(uisounds.go);
		string nm; if (activeSlot == -2) nm = "Autosave.bin"; else if (activeSlot == -1) nm = "QuickSave.bin";
		else {
			int k = page * listLength + activeSlot + 1;
			if (k < 10) nm = "Save0" + k + ".bin"; else nm = "Save" + k + ".bin";
		}
		var S = SaveGame.Load(SF + nm);
		if (ri == 9) { S.SC5 = null; }
		else if (ri == 8) { S.levelID = 14; S.SH = new SaveHub(S.SHcp13end); S.SC5 = null; }
		else if (ri == 7) { S.levelID = 4; S.SH = new SaveHub(S.SHcp13end); S.SC4 = null; S.SC5 = null; }
		else if (ri == 6) { S.levelID = 13; S.SH = new SaveHub(S.SHcp12end); S.SC4 = null; S.SC5 = null; }
		else if (ri == 5) { S.levelID = 3; S.SH = new SaveHub(S.SHcp12end); S.SC3 = null; S.SC4 = null; S.SC5 = null; }
		else if (ri == 4) { S.levelID = 12; S.SH = new SaveHub(S.SHcp11end); S.SC3 = null; S.SC4 = null; S.SC5 = null; }
		else if (ri == 3) { S.levelID = 2; S.SH = new SaveHub(S.SHcp11end); S.SC2 = null; S.SC3 = null; S.SC4 = null; S.SC5 = null; }
		else if (ri == 2) { S.levelID = 11; S.SH = null; S.SC2 = null; S.SC3 = null; S.SC4 = null; S.SC5 = null; }
		else if (ri == 1) { S.levelID = 1; S.SH = null; S.SC1 = null; S.SC2 = null; S.SC3 = null; S.SC4 = null; S.SC5 = null; }
		if (ri == 0)  { S = new SaveGame(); S.levelID = 0; }
		else { S.inDialogue = -1; S.inNarration = false; S.NID = 0; }
		S.Save(SF + "Rewind.bin"); COMMON.saveToLoad = "Rewind.bin";
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
			MC.LoadLevel(S.levelID);
		else {
			gameObject.SetActive(false);
			((DataControlMainMenu)DC).LoadScreen(COMMON.saveToLoad);
		}
	}

	/* ---------------------------------- MAIN ACTIONS ----------------------------------------- */
	void InputActivate(string nm) {
		var T = L.GetChild(activeSlot+2).GetComponent<Text>(); T.color = Color.white;
		T.text = (page * listLength + activeSlot + 1) + ". ";
		Canvas.ForceUpdateCanvases();
		IF.gameObject.SetActive(true);
		IF.GetComponent<RectTransform>().anchoredPosition = new Vector2(
			T.GetComponent<RectTransform>().rect.width + 10,
			T.GetComponent<RectTransform>().anchoredPosition.y + 7);
		nmPrev = nm; StartCoroutine(IFF(nm));
	}
	IEnumerator IFF(string nm) {
		IF.gameObject.SetActive(true);
		IF.ActivateInputField();
		IF.text = nm;
		IF.selectionColor = new Color(0, 0, 0, 0); //to make the initial frame with selection unnoticable
		yield return new WaitForEndOfFrame();
		IF.MoveTextEnd(true); //remove selection
	}
	public void IFFValueChanged(string v) {
		if (G.CalcSize(new GUIContent(v)).x > IFW) IF.text = nmPrev; else nmPrev = v;
	}

	void Save(int ind) {
		int k = page * listLength + ind + 1;
		string filename; if (k < 10) filename = "Save0" + k; else filename = "Save" + k;
		DC.S.Name = IF.text; curName = IF.text; curLevel = DC.S.levelID; isEmpty[ind] = false;
		DC.S.Save(SF + filename + ".bin");
		var NM = SN.Get(k); if (NM == null) SN.list.Add(new Namae(k, curName)); else NM.nm = curName;
		SN.Save(COMMON.saveFolder + "SaveNames.xml");
		//updating fields
		L.GetChild(ind+2).GetComponent<Text>().text = k + ". " + IF.text;
		L.GetChild(ind+2).GetComponent<Text>().color = defaultBlue;
		activeSlot = ind;
		Dt.SetActive(true); NoDt.SetActive(false); BD.SetActive(true);
		DtName.text = curName; DtTime.text = File.GetLastWriteTime(SF + filename + ".bin").ToString("MMMM dd, yyyy, H:mm:ss");
		DtChap.text = GetChapter(curLevel, IfStartedNewChapter(DC.S)); DtLoc.text = GetLocation(curLevel);
		if (COMMON.demoVersion) { Dt.transform.Find("Main").gameObject.SetActive(true); Dt.transform.Find("Demo").gameObject.SetActive(false); }
		//screenshot
		var t = TakeScreenshot(filename);
		DtImg.color = Color.white;
		DtImg.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero);
		var h = Mathf.RoundToInt(DtImg.GetComponent<RectTransform>().rect.height);
		var w = Mathf.RoundToInt(t.width * h * 1.0F / t.height);
		DtImg.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
	}

	public Texture2D TakeScreenshot(string f) {
		var path = COMMON.saveFolder + "Images"; Directory.CreateDirectory(path); path += "/" + f + ".jpg";
		Camera.main.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
		Camera.main.targetTexture.antiAliasing = 8;
		RenderTexture.active = Camera.main.targetTexture; Camera.main.Render();
		//capturing render texture to 2d texture
		var shot1 = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
		shot1.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0); shot1.Apply();
		var shot2 = Screen.height >= 800 ? new Texture2D(shot1.width/4, shot1.height/4) : new Texture2D(shot1.width/2, shot1.height/2);
		shot2.SetPixels(shot1.GetPixels(Screen.height >= 800 ? 2 : 1)); shot2.Apply();
		//restoring and saving
		RenderTexture.active = null; Camera.main.targetTexture.Release(); Camera.main.targetTexture = null;
		var bytes = shot2.EncodeToJPG(90);
		System.IO.File.WriteAllBytes(path, bytes);
		return shot2;
	}

	void Load(int ind) {
		if (ind == -2) COMMON.saveToLoad = "Autosave.bin";
		else if (ind == -1) COMMON.saveToLoad = "QuickSave.bin";
		else {
			int k = page * listLength + ind + 1;
			if (k < 10) COMMON.saveToLoad = "Save0" + k + ".bin";
			else COMMON.saveToLoad = "Save" + k + ".bin";
		}
		if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 0)
			MC.LoadLevel(curLevel);
		else {
			gameObject.SetActive(false);
			((DataControlMainMenu)DC).LoadScreen(COMMON.saveToLoad);
		}
	}

	/* ---------------------------------- KEYBOARD CONTROLS ----------------------------------------- */
	void Update() {
		if (activePopup == -1 && Input.GetKeyDown(KeyCode.Escape)) OnBack(null);
		else if (activePopup >= 0 && Input.GetKeyDown(KeyCode.Escape)) OnBackPopup(null);
		else if (activePopup == 0 && Input.GetKeyDown(KeyCode.Return)) OnSaveOK(null);

		else if (activePopup > 0) {
			if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (activePopup == 1) { POY.color = defaultBlue; PON.color = Color.white; }
				else if (activePopup == 2) { PDY.color = defaultBlue; PDN.color = Color.white; }
				else if (rwHot) { PRY.color = defaultBlue; PRN.color = Color.white; }
				else if (RWPrev.gameObject.activeSelf) OnRewindPrev(null);
			}
			else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				if (activePopup == 1) { POY.color = Color.white; PON.color = defaultBlue; }
				else if (activePopup == 2) { PDY.color = Color.white; PDN.color = defaultBlue; }
				else if (rwHot) { PRY.color = Color.white; PRN.color = defaultBlue; }
				else if (RWNext.gameObject.activeSelf) OnRewindNext(null);
			}
			else if (activePopup == 3 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
				rwHot = false; PRY.color = Color.white; PRN.color = Color.white;
			}
			else if (activePopup == 3 && !rwHot && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) {
				rwHot = true; PRY.color = defaultBlue; PRN.color = Color.white;
			}
			else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
				if (activePopup == 1 && POY.color == defaultBlue) OnOverwrite(null);
				else if (activePopup == 1 && PON.color == defaultBlue) OnBackPopup(null);
				else if (activePopup == 2 && PDY.color == defaultBlue) OnDeleteConfirm(null);
				else if (activePopup == 2 && PDN.color == defaultBlue) OnBackPopup(null);
				else if (activePopup == 3 && PRY.color == defaultBlue) OnRewindConfirm(null);
				else if (activePopup == 3 && PRN.color == defaultBlue) OnBackPopup(null);
			}
		}

		else if (activePopup == -1) {
			if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
				if (activeSlot < -2) { if (isLoad && AS) SelectSlot(-2); else if (isLoad && QS) SelectSlot(-1); else SelectSlot(0); }
				else if (activeSlot == -2) { if (isLoad && QS) SelectSlot(-1); else SelectSlot(0); }
				else if (activeSlot < listLength - 1) SelectSlot(activeSlot + 1);
			}
			else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
				if (activeSlot < -2) { if (isLoad && AS) SelectSlot(-2); else if (isLoad && QS) SelectSlot(-1); else SelectSlot(0); }
				else if (activeSlot == -1) { if (isLoad && AS) SelectSlot(-2); }
				else if (activeSlot == 0) { if (isLoad && QS) SelectSlot(-1); else if (isLoad && AS) SelectSlot(-2); }
				else if (activeSlot > 0) SelectSlot(activeSlot - 1);
			}
			else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
				if (activeSlot >= 0) { page--; if (page < 0) page = 99 / listLength; LoadPage(0); MC.PlaySFX(uisounds.click1); }
			}
			else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
				if (activeSlot >= 0) { page++; if (page > 99 / listLength) page = 0; LoadPage(0); MC.PlaySFX(uisounds.click1); }
			}

			else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)) {
				if (isLoad && BL.activeSelf) OnLoad(null);
				else if (!isLoad && BS.activeSelf) OnSave(null);
			}
			else if (Input.GetKeyDown(KeyCode.R) && BR.activeSelf) OnRewind(null);
			else if (Input.GetKeyDown(KeyCode.Delete) && BD.activeSelf) OnDelete(null);
		}
	}
}