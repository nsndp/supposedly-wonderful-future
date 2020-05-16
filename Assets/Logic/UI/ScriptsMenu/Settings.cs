using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Settings : MonoBehaviour {

	DataControl DC; MenuControl MC; string pth; bool isMM;
	int ri; int[] resX, resY; bool fullscr; int oldX, oldY;
	float volM, volS, ovlA; int tl, fs, ta, te; bool vlr;
	Text RES, DM, MV, SV, FS, L, A, Eff, Vid, OV; Slider MVS, SVS, OVS;
	Text BS, BR, BB, PSY, PSN, PRY, PRN, PBY, PBN, SaveConfirm, ResDlg;
	GameObject Overlay; bool scDis = false;
	bool resChange = false; float resC; int resCD;
	GameObject ResPrev, ResNext, DMPrev, DMNext, VidPrev, VidNext;
	GameObject FSPrev, FSNext, LPrev, LNext, APrev, ANext, EffPrev, EffNext;
	GameObject ResPrevOff, ResNextOff, DMPrevOff, DMNextOff, VidPrevOff, VidNextOff;
	GameObject FSPrevOff, FSNextOff, LPrevOff, LNextOff, APrevOff, ANextOff, EffPrevOff, EffNextOff;
	RectTransform DescBlock; Text Desc;
	int cur = -1; bool hotYes = false, hotNo = false;
	bool holdLeft = false, holdRight = false; float holdTime = 0; int holdCount = 0;
	Color blue = new Color(0.188F, 0.647F, 1);
	Color grey = new Color(0.275F, 0.275F, 0.275F);
	Color lblu = new Color(0.718F, 0.878F, 1);
	[System.NonSerialized] public string[] strDM = new string[] { "Windowed", "Fullscreen" };
	[System.NonSerialized] public string[] strVid = new string[] { "Low (360p)", "Default (720p)" };
	[System.NonSerialized] public string[] strFS = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
	[System.NonSerialized] public string[] strL = new string[] { "Classic", "Cinematic", "Sidelong" };
	[System.NonSerialized] public string[] strA = new string[] { "Off", "Responses only", "On" };
	[System.NonSerialized] public string[] strE = new string[] { "Off", "Shadow", "Outline", "Shadow & outline" };
	[System.NonSerialized] public string resConfirm = "Keep the new resolution? [X] sec";
	[System.NonSerialized] public string[] descrips = new string[] {
		"Resolution and screen mode can only\nbe changed from the main menu.",
		"Resolution and screen mode can only\nbe changed from the main menu.",
		null, null, "Controls resolution of video files. Try switching to Low\nif you experience performance problems in Chapter 3.",
		"Adjusts position of text boxes. Try \"Sidelong\" for shorter lines\nor \"Cinematic\" for a more spacious feel.",
		"Adjusts relative size of all in-game text,\nwhere 1 is the smallest and 8 is the largest.",
		"Controls whether main text and/or responses\nappear gradually or all at once.",
		"Adds a black shadow and/or outline to text. Both improve\nreadability but may significantly affect performance.",
		"Controls transparency of black rectangles behind text.\nTry lowering this value if you want to further improve readability."
	};
	string[] labels = new string[] {
		"Labels1/ResolutionLabel", "Labels1/DisplayLabel", "Labels1/MusicLabel", "Labels1/SoundLabel", "Labels1/VideoLabel",
		"Labels2/LayoutLabel", "Labels2/FontSizeLabel", "Labels2/AnimationLabel", "Labels2/EffectLabel", "Labels2/OverlayLabel"
	};

	void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControl>();
		MC = transform.root.Find("Menu").GetComponent<MenuControl>();
		pth = COMMON.saveFolder + "UserSettings.bin";
		isMM = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0;
		RES = transform.Find("Controls1/Resolution").GetComponent<Text>(); DM = transform.Find("Controls1/Display").GetComponent<Text>();
		MV = transform.Find("Controls1/MusicValue").GetComponent<Text>(); MVS = transform.Find("Controls1/MusicSlider").GetComponent<Slider>();
		SV = transform.Find("Controls1/SoundValue").GetComponent<Text>(); SVS = transform.Find("Controls1/SoundSlider").GetComponent<Slider>();
		Vid = transform.Find("Controls1/Video").GetComponent<Text>();
		FS = transform.Find("Controls2/FontSize").GetComponent<Text>(); L = transform.Find("Controls2/Layout").GetComponent<Text>();
		A = transform.Find("Controls2/Animation").GetComponent<Text>(); Eff = transform.Find("Controls2/Effects").GetComponent<Text>();
		OV = transform.Find("Controls2/OverlayValue").GetComponent<Text>(); OVS = transform.Find("Controls2/Overlay").GetComponent<Slider>();
		ResPrev = transform.Find("Controls1/Resolution/Prev").gameObject; ResNext = transform.Find("Controls1/Resolution/Next").gameObject;
		ResPrevOff = transform.Find("Controls1/Resolution/PrevOff").gameObject; ResNextOff = transform.Find("Controls1/Resolution/NextOff").gameObject;
		DMPrev = transform.Find("Controls1/Display/Prev").gameObject; DMNext = transform.Find("Controls1/Display/Next").gameObject;
		DMPrevOff = transform.Find("Controls1/Display/PrevOff").gameObject; DMNextOff = transform.Find("Controls1/Display/NextOff").gameObject;
		VidPrev = transform.Find("Controls1/Video/Prev").gameObject; VidNext = transform.Find("Controls1/Video/Next").gameObject;
		VidPrevOff = transform.Find("Controls1/Video/PrevOff").gameObject; VidNextOff = transform.Find("Controls1/Video/NextOff").gameObject;
		FSPrev = transform.Find("Controls2/FontSize/Prev").gameObject; FSNext = transform.Find("Controls2/FontSize/Next").gameObject;
		FSPrevOff = transform.Find("Controls2/FontSize/PrevOff").gameObject; FSNextOff = transform.Find("Controls2/FontSize/NextOff").gameObject;
		LPrev = transform.Find("Controls2/Layout/Prev").gameObject; LNext = transform.Find("Controls2/Layout/Next").gameObject;
		LPrevOff = transform.Find("Controls2/Layout/PrevOff").gameObject; LNextOff = transform.Find("Controls2/Layout/NextOff").gameObject;
		APrev = transform.Find("Controls2/Animation/Prev").gameObject; ANext = transform.Find("Controls2/Animation/Next").gameObject;
		APrevOff = transform.Find("Controls2/Animation/PrevOff").gameObject; ANextOff = transform.Find("Controls2/Animation/NextOff").gameObject;
		EffPrev = transform.Find("Controls2/Effects/Prev").gameObject; EffNext = transform.Find("Controls2/Effects/Next").gameObject;
		EffPrevOff = transform.Find("Controls2/Effects/PrevOff").gameObject; EffNextOff = transform.Find("Controls2/Effects/NextOff").gameObject;
		DescBlock = transform.Find("Desc").GetComponent<RectTransform>(); Desc = DescBlock.Find("Text").GetComponent<Text>();
		PSY = transform.Find("PopupResolution/Yes").GetComponent<Text>(); PSN = transform.Find("PopupResolution/No").GetComponent<Text>();
		PRY = transform.Find("PopupReset/Yes").GetComponent<Text>(); PRN = transform.Find("PopupReset/No").GetComponent<Text>();
		PBY = transform.Find("PopupBack/Yes").GetComponent<Text>(); PBN = transform.Find("PopupBack/No").GetComponent<Text>();
		BS = transform.Find("Buttons/Save").GetComponent<Text>();
		BR = transform.Find("Buttons/Reset").GetComponent<Text>();
		BB = transform.Find("Buttons/Back").GetComponent<Text>();
		SaveConfirm = transform.Find("SaveConfirm").GetComponent<Text>();
		ResDlg = PSY.transform.parent.GetComponent<Text>();
		Overlay = transform.Find("Overlay").gameObject;
		if (!isMM) {
			RES.color = grey; DM.color = grey;
			transform.Find("Labels1/ResolutionLabel").GetComponent<Text>().color = grey;
			transform.Find("Labels1/DisplayLabel").GetComponent<Text>().color = grey;
		} else {
			descrips[0] = null; descrips[1] = null;
		}

		if (Application.isEditor) {
			resX = new int[] {Screen.width}; resY = new int[] {Screen.height}; ri = 0;
		} else {
			var R = Screen.resolutions; int st = 0; while (R[st].width < 1024) st++;
			resX = new int[R.Length-st]; resY = new int[resX.Length]; ri = resX.Length;
			for (int i = 0; i < resX.Length; i++) { resX[i] = R[st+i].width; resY[i] = R[st+i].height; }
		}
	}

	public void Show() {
		if (FSPrev == null) Init();
		UpdateShownValues();
		SaveConfirm.gameObject.SetActive(false);
		foreach (var obj in new[] {ResPrev, ResNext, DMPrev, DMNext, VidPrev, VidNext, FSPrev, FSNext, LPrev, LNext, APrev, ANext, EffPrev, EffNext}) obj.GetComponent<Image>().color = Color.white;
		C();
	}
	void UpdateShownValues() {
		ri = 0; while (ri < resX.Length-1 && (resX[ri] != Screen.width || resY[ri] != Screen.height)) ri++;
		fullscr = Screen.fullScreen;
		volM = COMMON.U.volM; volS = COMMON.U.volS; vlr = COMMON.U.videosLowRes;
		fs = COMMON.U.fontSize; tl = COMMON.U.textLayout; ta = COMMON.U.textAnim;
		te = COMMON.U.textEffects; ovlA = COMMON.U.overlayAlpha;

		RES.text = resX[ri] + " x " + resY[ri];
		ResPrev.SetActive(isMM && ri > 0); ResPrevOff.SetActive(!ResPrev.activeSelf);
		ResNext.SetActive(isMM && ri < resX.Length-1); ResNextOff.SetActive(!ResNext.activeSelf);
		DM.text = strDM[Screen.fullScreen ? 1 : 0];
		DMPrev.SetActive(isMM && fullscr); DMPrevOff.SetActive(!DMPrev.activeSelf);
		DMNext.SetActive(isMM && !fullscr); DMNextOff.SetActive(!DMNext.activeSelf);
		MVS.value = volM; SVS.value = volS; OVS.value = 1-ovlA; FS.text = strFS[fs];
		FSPrev.SetActive(fs > 0); FSPrevOff.SetActive(!FSPrev.activeSelf);
		FSNext.SetActive(fs < 7); FSNextOff.SetActive(!FSNext.activeSelf);
		A.text = strA[ta]; APrev.SetActive(ta > 0); ANext.SetActive(ta < 2); APrevOff.SetActive(ta == 0); ANextOff.SetActive(ta == 2);
		Eff.text = strE[te]; EffPrev.SetActive(te > 0); EffNext.SetActive(te < 3); EffPrevOff.SetActive(te == 0); EffNextOff.SetActive(te == 3);
		L.text = strL[tl]; LPrev.SetActive(tl > 0); LNext.SetActive(tl < 2); LPrevOff.SetActive(tl == 0); LNextOff.SetActive(tl == 2);
		Vid.text = strVid[vlr ? 0 : 1]; VidPrev.SetActive(!vlr); VidNext.SetActive(vlr); VidPrevOff.SetActive(vlr); VidNextOff.SetActive(!vlr);
	}

	//------------------------------------ CONTROLS ------------------------------------------------------
	public void OnResPrevEnter(BaseEventData d) { C(); ResPrev.GetComponent<Image>().color = blue; }
	public void OnResPrevExit(BaseEventData d) { ResPrev.GetComponent<Image>().color = Color.white; }
	public void OnResNextEnter(BaseEventData d) { C(); ResNext.GetComponent<Image>().color = blue; }
	public void OnResNextExit(BaseEventData d) { ResNext.GetComponent<Image>().color = Color.white; }
	public void OnDMPrevEnter(BaseEventData d) { C(); DMPrev.GetComponent<Image>().color = blue; }
	public void OnDMPrevExit(BaseEventData d) { DMPrev.GetComponent<Image>().color = Color.white; }
	public void OnDMNextEnter(BaseEventData d) { C(); DMNext.GetComponent<Image>().color = blue; }
	public void OnDMNextExit(BaseEventData d) { DMNext.GetComponent<Image>().color = Color.white; }
	public void OnVidPrevEnter(BaseEventData d) { C(); VidPrev.GetComponent<Image>().color = blue; }
	public void OnVidPrevExit(BaseEventData d) { VidPrev.GetComponent<Image>().color = Color.white; }
	public void OnVidNextEnter(BaseEventData d) { C(); VidNext.GetComponent<Image>().color = blue; }
	public void OnVidNextExit(BaseEventData d) { VidNext.GetComponent<Image>().color = Color.white; }
	public void OnFSPrevEnter(BaseEventData d) { C(); FSPrev.GetComponent<Image>().color = blue; }
	public void OnFSPrevExit(BaseEventData d) { FSPrev.GetComponent<Image>().color = Color.white; }
	public void OnFSNextEnter(BaseEventData d) { C(); FSNext.GetComponent<Image>().color = blue; }
	public void OnFSNextExit(BaseEventData d) { FSNext.GetComponent<Image>().color = Color.white; }
	public void OnLPrevEnter(BaseEventData d) { C(); LPrev.GetComponent<Image>().color = blue; }
	public void OnLPrevExit(BaseEventData d) { LPrev.GetComponent<Image>().color = Color.white; }
	public void OnLNextEnter(BaseEventData d) { C(); LNext.GetComponent<Image>().color = blue; }
	public void OnLNextExit(BaseEventData d) { LNext.GetComponent<Image>().color = Color.white; }
	public void OnAPrevEnter(BaseEventData d) { C(); APrev.GetComponent<Image>().color = blue; }
	public void OnAPrevExit(BaseEventData d) { APrev.GetComponent<Image>().color = Color.white; }
	public void OnANextEnter(BaseEventData d) { C(); ANext.GetComponent<Image>().color = blue; }
	public void OnANextExit(BaseEventData d) { ANext.GetComponent<Image>().color = Color.white; }
	public void OnEffPrevEnter(BaseEventData d) { C(); EffPrev.GetComponent<Image>().color = blue; }
	public void OnEffPrevExit(BaseEventData d) { EffPrev.GetComponent<Image>().color = Color.white; }
	public void OnEffNextEnter(BaseEventData d) { C(); EffNext.GetComponent<Image>().color = blue; }
	public void OnEffNextExit(BaseEventData d) { EffNext.GetComponent<Image>().color = Color.white; }

	public void OnResPrev(BaseEventData d) {
		ri--; RES.text = resX[ri] + " x " + resY[ri]; MC.PlaySFX(uisounds.click1);
		ResNext.SetActive(true); ResNextOff.SetActive(false); ResNext.GetComponent<Image>().color = Color.white;
		ResPrev.SetActive(ri > 0); ResPrevOff.SetActive(!ResPrev.activeSelf);
	}
	public void OnResNext(BaseEventData d) {
		ri++; RES.text = resX[ri] + " x " + resY[ri]; MC.PlaySFX(uisounds.click1);
		ResPrev.SetActive(true); ResPrevOff.SetActive(false); ResPrev.GetComponent<Image>().color = Color.white;
		ResNext.SetActive(ri < resX.Length-1); ResNextOff.SetActive(!ResNext.activeSelf);
	}
	public void OnDMPrev(BaseEventData d) {
		fullscr = false; DM.text = strDM[0]; MC.PlaySFX(uisounds.click1);
		DMNext.SetActive(true); DMNextOff.SetActive(false); DMNext.GetComponent<Image>().color = Color.white;
		DMPrev.SetActive(false); DMPrevOff.SetActive(true);
	}
	public void OnDMNext(BaseEventData d) {
		fullscr = true; DM.text = strDM[1]; MC.PlaySFX(uisounds.click1);
		DMPrev.SetActive(true); DMPrevOff.SetActive(false); DMPrev.GetComponent<Image>().color = Color.white;
		DMNext.SetActive(false); DMNextOff.SetActive(true); 
	}
	public void OnVidPrev(BaseEventData d) {
		vlr = true; Vid.text = strVid[0]; MC.PlaySFX(uisounds.click1);
		VidNext.SetActive(true); VidNextOff.SetActive(false); VidNext.GetComponent<Image>().color = Color.white;
		VidPrev.SetActive(false); VidPrevOff.SetActive(true);
	}
	public void OnVidNext(BaseEventData d) {
		vlr = false; Vid.text = strVid[1]; MC.PlaySFX(uisounds.click1);
		VidPrev.SetActive(true); VidPrevOff.SetActive(false); VidPrev.GetComponent<Image>().color = Color.white;
		VidNext.SetActive(false); VidNextOff.SetActive(true);
	}

	public void OnFSPrev(BaseEventData d) {
		fs--; FS.text = strFS[fs]; MC.PlaySFX(uisounds.click1);
		FSNext.SetActive(true); FSNextOff.SetActive(false); FSNext.GetComponent<Image>().color = Color.white;
		FSPrev.SetActive(fs > 0); FSPrevOff.SetActive(!FSPrev.activeSelf);
	}
	public void OnFSNext(BaseEventData d) {
		fs++; FS.text = strFS[fs]; MC.PlaySFX(uisounds.click1);
		FSPrev.SetActive(true); FSPrevOff.SetActive(false); FSPrev.GetComponent<Image>().color = Color.white;
		FSNext.SetActive(fs < 7); FSNextOff.SetActive(!FSNext.activeSelf);
	}
	public void OnLPrev(BaseEventData d) {
		tl--; L.text = strL[tl]; MC.PlaySFX(uisounds.click1);
		LNext.SetActive(true); LNextOff.SetActive(false); LNext.GetComponent<Image>().color = Color.white;
		LPrev.SetActive(tl > 0); LPrevOff.SetActive(!LPrev.activeSelf);
	}
	public void OnLNext(BaseEventData d) {
		tl++; L.text = strL[tl]; MC.PlaySFX(uisounds.click1);
		LPrev.SetActive(true); LPrevOff.SetActive(false); LPrev.GetComponent<Image>().color = Color.white;
		LNext.SetActive(tl < 2); LNextOff.SetActive(!LNext.activeSelf);
	}
	public void OnAPrev(BaseEventData d) {
		ta--; A.text = strA[ta]; MC.PlaySFX(uisounds.click1);
		ANext.SetActive(true); ANextOff.SetActive(false); ANext.GetComponent<Image>().color = Color.white;
		APrev.SetActive(ta > 0); APrevOff.SetActive(ta == 0);
	}
	public void OnANext(BaseEventData d) {
		ta++; A.text = strA[ta]; MC.PlaySFX(uisounds.click1);
		APrev.SetActive(true); APrevOff.SetActive(false); APrev.GetComponent<Image>().color = Color.white;
		ANext.SetActive(ta < 2); ANextOff.SetActive(ta == 2);
	}
	public void OnEffPrev(BaseEventData d) {
		te--; Eff.text = strE[te]; MC.PlaySFX(uisounds.click1);
		EffNext.SetActive(true); EffNextOff.SetActive(false); EffNext.GetComponent<Image>().color = Color.white;
		EffPrev.SetActive(te > 0); EffPrevOff.SetActive(te == 0);
	}
	public void OnEffNext(BaseEventData d) {
		te++; Eff.text = strE[te]; MC.PlaySFX(uisounds.click1);
		EffPrev.SetActive(true); EffPrevOff.SetActive(false); EffPrev.GetComponent<Image>().color = Color.white;
		EffNext.SetActive(te < 3); EffNextOff.SetActive(te == 3);
	}

	public void OnMusicVolumeChange(float v) {
		volM = Mathf.Round(v*100f)/100f; MV.text = Mathf.RoundToInt(v*100) + "%";
	}
	public void OnSoundVolumeChange(float v) {
		volS = Mathf.Round(v*100f)/100f; SV.text = Mathf.RoundToInt(v * 100) + "%";
	}
	public void OnOverlayChange(float v) {
		ovlA = 1 - v; OV.text = Mathf.RoundToInt(v * 100) + "%";
	}

	//------------------------------------ HINTS ------------------------------------------------------
	public void OnResDMEnter(BaseEventData d) { C(); if (!isMM) SetHint(descrips[0], true); }
	public void OnVolumesEnter(BaseEventData d) { C(); }
	public void OnVidEnter(BaseEventData d) { C(); SetHint(descrips[4], true); }
	public void OnFSEnter(BaseEventData d) { C(); SetHint(descrips[6], true); }
	public void OnLEnter(BaseEventData d) { C(); SetHint(descrips[5], true); }
	public void OnAEnter(BaseEventData d) { C(); SetHint(descrips[7], true); }
	public void OnEEnter(BaseEventData d) { C(); SetHint(descrips[8], true); }
	public void OnOVEnter(BaseEventData d) { C(); SetHint(descrips[9], true); }
	public void OnLabelExit(BaseEventData d) { DescBlock.gameObject.SetActive(false); }
	void SetHint(string t, bool twoLines) {
		SaveConfirm.gameObject.SetActive(false);
		float scale = 1; if (Screen.height > 768) scale = Screen.height / 768.0F;
		DescBlock.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.RoundToInt((twoLines ? 50 : 30)*scale));
		Desc.text = t; DescBlock.gameObject.SetActive(true);
	}

	//------------------------------------ BUTTONS ------------------------------------------------------
	public void OnSaveEnter(BaseEventData d) { C(); BS.color = blue; }
	public void OnSaveExit(BaseEventData d) { BS.color = Color.white; }
	public void OnResetEnter(BaseEventData d) { C(); BR.color = blue; }
	public void OnResetExit(BaseEventData d) { BR.color = Color.white; }
	public void OnBackEnter(BaseEventData d) { C(); BB.color = blue; }
	public void OnBackExit(BaseEventData d) { BB.color = Color.white; }
	public void OnPSYEnter(BaseEventData d) { CO(); PSY.color = blue; }
	public void OnPSYExit(BaseEventData d) { PSY.color = Color.white; }
	public void OnPSNEnter(BaseEventData d) { CO(); PSN.color = blue; }
	public void OnPSNExit(BaseEventData d) { PSN.color = Color.white; }
	public void OnPRYEnter(BaseEventData d) { CO(); PRY.color = blue; }
	public void OnPRYExit(BaseEventData d) { PRY.color = Color.white; }
	public void OnPRNEnter(BaseEventData d) { CO(); PRN.color = blue; }
	public void OnPRNExit(BaseEventData d) { PRN.color = Color.white; }
	public void OnPBYEnter(BaseEventData d) { CO(); PBY.color = blue; }
	public void OnPBYExit(BaseEventData d) { PBY.color = Color.white; }
	public void OnPBNEnter(BaseEventData d) { CO(); PBN.color = blue; }
	public void OnPBNExit(BaseEventData d) { PBN.color = Color.white; }

	//-----save-----
	public void OnSave(BaseEventData d) {
		var U = COMMON.U; MC.PlaySFX(uisounds.save);
		if (!isMM && U.videosLowRes != vlr) DC.MC.changedVideoRes = true;
		if (!isMM && (U.fontSize != fs || U.textLayout != tl || U.textAnim != ta)) DC.MC.changedUI = true;
		if (Mathf.RoundToInt(U.overlayAlpha*100) != Mathf.RoundToInt(ovlA*100)) DC.MC.changedTransp = true;
		if (U.textEffects != te) DC.MC.changedEffects = true;

		U.volM = volM; U.volS = volS; U.videosLowRes = vlr;
		U.fontSize = fs; U.textLayout = tl; U.textAnim = ta;
		U.textEffects = te; U.overlayAlpha = ovlA;
		U.Save(pth);

		if (Screen.width != resX[ri] || Screen.height != resY[ri]) {
			oldX = Screen.width; oldY = Screen.height;
			Screen.SetResolution(resX[ri], resY[ri], fullscr); MC.ReScale(resX[ri], resY[ri]);
			Overlay.SetActive(true); ResDlg.gameObject.SetActive(true);
			resChange = true; resC = 20; resCD = 20; ResDlg.text = resConfirm.Replace("[X]", resCD.ToString());
		} else {
			if (Screen.fullScreen && !fullscr) Screen.fullScreen = false;
			if (!Screen.fullScreen && fullscr) Screen.fullScreen = true;
			StartCoroutine(SaveConfirmation());
		}

		MC.SFX.volume = volS;
		if (DC.BGM != null) DC.BGM.volume = volM;
		if (DC.Sound != null) DC.Sound.volume = volS;
		if (DC.Sound2 != null) DC.Sound2.volume = volS;
	}
	IEnumerator SaveConfirmation() {
		var c = SaveConfirm.color; c.a = 1; SaveConfirm.color = c; SaveConfirm.gameObject.SetActive(true);
		yield return new WaitForSeconds(1.5F);
		scDis = true;
	}

	//-----resolution-----
	public void OnResolutionConfirm(BaseEventData d) {
		MC.PlaySFX(uisounds.click2);
		Overlay.SetActive(false); ResDlg.gameObject.SetActive(false);
		PSY.color = Color.white; resChange = false;
		StartCoroutine(SaveConfirmation());
	}
	public void OnResolutionCancel(BaseEventData d) {
		MC.PlaySFX(uisounds.back);
		Overlay.SetActive(false); ResDlg.gameObject.SetActive(false);
		PSN.color = Color.white; resChange = false;

		Screen.SetResolution(oldX, oldY, fullscr); MC.ReScale(oldX, oldY);
		ri = 0; while (ri < resX.Length-1 && (resX[ri] != oldX || resY[ri] != oldY)) ri++;
		RES.text = resX[ri] + " x " + resY[ri];
		ResPrev.SetActive(isMM && ri > 0); ResPrevOff.SetActive(!ResPrev.activeSelf);
		ResNext.SetActive(isMM && ri < resX.Length-1); ResNextOff.SetActive(!ResNext.activeSelf);
	}

	//-----reset-----
	public void OnReset(BaseEventData d) {
		BR.color = Color.white;
		MC.PlaySFX(uisounds.click2);
		SaveConfirm.gameObject.SetActive(false);
		Overlay.SetActive(true); PRY.transform.parent.gameObject.SetActive(true);
	}
	public void OnResetCancel(BaseEventData d) {
		MC.PlaySFX(uisounds.back);
		Overlay.SetActive(false); PRY.transform.parent.gameObject.SetActive(false);
		PRN.color = Color.white;
	}
	public void OnResetConfirm(BaseEventData d) {
		MC.PlaySFX(uisounds.delete);
		var lang = COMMON.U.languageID; COMMON.U = new UserSettings(); COMMON.U.languageID = lang;
		UpdateShownValues(); COMMON.U.Save(pth);
		DC.MC.changedUI = !isMM;
		MC.SFX.volume = volS;
		if (DC.BGM != null) DC.BGM.volume = volM;
		if (DC.Sound != null) DC.Sound.volume = volS;
		if (DC.Sound2 != null) DC.Sound2.volume = volS;
		Overlay.SetActive(false); PRY.transform.parent.gameObject.SetActive(false);
		PRY.color = Color.white;
	}

	//-----back-----
	private bool ChangesPending() {
		return (Screen.width != resX[ri] || Screen.height != resY[ri] || Screen.fullScreen != fullscr ||
			COMMON.U.fontSize != fs || COMMON.U.textLayout != tl ||
			COMMON.U.textAnim != ta || COMMON.U.textEffects != te ||
			Mathf.RoundToInt(COMMON.U.volM*100) != Mathf.RoundToInt(volM*100) ||
			Mathf.RoundToInt(COMMON.U.volS*100) != Mathf.RoundToInt(volS*100) ||
			Mathf.RoundToInt(COMMON.U.overlayAlpha*100) != Mathf.RoundToInt(ovlA*100));
	}

	public void OnBack(BaseEventData d) {
		BB.color = Color.white;
		if (!ChangesPending()) { 
			MC.PlaySFX(uisounds.back); Exit();
		}
		else {
			MC.PlaySFX(uisounds.click2);
			SaveConfirm.gameObject.SetActive(false);
			Overlay.SetActive(true); PBY.transform.parent.gameObject.SetActive(true);
			C();
		}
	}
	public void OnBackCancel(BaseEventData d) {
		MC.PlaySFX(uisounds.back);
		Overlay.SetActive(false); PBY.transform.parent.gameObject.SetActive(false);
		PBN.color = Color.white;
	}
	public void OnBackConfirm(BaseEventData d) {
		MC.PlaySFX(uisounds.delete);
		Overlay.SetActive(false); PBY.transform.parent.gameObject.SetActive(false);
		PBY.color = Color.white; Exit();
	}
	void Exit() {
		gameObject.SetActive(false); DC.MC.submenuOpened = false;
		if (!isMM) transform.root.Find("Menu/Pause").gameObject.SetActive(true);
		else transform.root.Find("Menu/Main").gameObject.SetActive(true);
		BB.color = Color.white; PBY.color = Color.white;
	}

	void C() { //clear selection
		if (cur >= 0 && cur <= 9) {
			transform.Find(labels[cur]).GetComponent<Text>().color = Color.white;
			cur = -1; DescBlock.gameObject.SetActive(false); holdLeft = false; holdRight = false;
		}
		else if (cur >= 10) {
			cur = -1; BS.color = Color.white; BR.color = Color.white; BB.color = Color.white;
		}
	}
	void CO() { //clear selection when in overlay
		if (hotYes) { hotYes = false; PBY.color = Color.white; PRY.color = Color.white; PSY.color = Color.white; }
		else if (hotNo) { hotNo = false; PBN.color = Color.white; PRN.color = Color.white; PSN.color = Color.white; }
	}

	void Update() {
		if (scDis && SaveConfirm.color.a > 0) {
			var c = SaveConfirm.color; c.a -= 0.05F * Time.deltaTime * 60; SaveConfirm.color = c;
		}
		else if (scDis && SaveConfirm.color.a <= 0) {
			scDis = false; SaveConfirm.gameObject.SetActive(false);
		}

		if (resChange && resC > 0) {
			resC -= Time.deltaTime;
			if (resC < resCD - 1) { resCD--; ResDlg.text = resConfirm.Replace("[X]", resCD.ToString()); }
		}
		else if (resChange) OnResolutionCancel(null);

		//-------------------- KEYBOARD CONTROL (pop-ups) ---------------------------
		if (Overlay.activeSelf) {
			if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
				hotYes = true; hotNo = false;
				if (PBY.gameObject.activeInHierarchy) { PBY.color = blue; PBN.color = Color.white; }
				else if (PRY.gameObject.activeInHierarchy) { PRY.color = blue; PRN.color = Color.white; }
				else { PSY.color = blue; PSN.color = Color.white; }
			}
			else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
				hotYes = false; hotNo = true;
				if (PBY.gameObject.activeInHierarchy) { PBY.color = Color.white; PBN.color = blue; }
				else if (PRY.gameObject.activeInHierarchy) { PRY.color = Color.white; PRN.color = blue; }
				else { PSY.color = Color.white; PSN.color = blue; }
			}
			else if (Input.GetKeyDown(KeyCode.Escape) || (hotNo && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))) {
				hotYes = false; hotNo = false; cur = -1;
				if (PBY.gameObject.activeInHierarchy) OnBackCancel(null);
				else if (PRY.gameObject.activeInHierarchy) OnResetCancel(null);
				else OnResolutionCancel(null);
			}
			else if (hotYes && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
				hotYes = false; hotNo = false; cur = -1;
				if (PBY.gameObject.activeInHierarchy) OnBackConfirm(null);
				else if (PRY.gameObject.activeInHierarchy) OnResetConfirm(null);
				else OnResolutionConfirm(null);
			}
		}

		//-------------------- KEYBOARD CONTROL (main) ---------------------------
		else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
			if (cur == 9) {
				transform.Find(labels[9]).GetComponent<Text>().color = Color.white; OnLabelExit(null);
				cur = 10; BS.color = blue; BR.color = Color.white; BB.color = Color.white;
			} else {
				if (cur == -1) cur = isMM ? 0 : 2;
				else if (cur > 9) { cur = isMM ? 0 : 2; BS.color = Color.white; BR.color = Color.white; BB.color = Color.white; }
				else { transform.Find(labels[cur]).GetComponent<Text>().color = Color.white; cur++; }
				transform.Find(labels[cur]).GetComponent<Text>().color = lblu;
				if (descrips[cur] == null) DescBlock.gameObject.SetActive(false);
				else SetHint(descrips[cur], descrips[cur].Contains("\n"));
			}
		}
		else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
			if (cur == (isMM ? 0 : 2)) {
				transform.Find(labels[isMM ? 0 : 2]).GetComponent<Text>().color = Color.white; OnLabelExit(null);
				cur = 10; BS.color = blue; BR.color = Color.white; BB.color = Color.white;
			} else {
				if (cur == -1) cur = isMM ? 0 : 2;
				else if (cur > 9) { cur = 9; BS.color = Color.white; BR.color = Color.white; BB.color = Color.white; }
				else { transform.Find(labels[cur]).GetComponent<Text>().color = Color.white; cur--; }
				transform.Find(labels[cur]).GetComponent<Text>().color = lblu;
				if (descrips[cur] == null) DescBlock.gameObject.SetActive(false);
				else SetHint(descrips[cur], descrips[cur].Contains("\n"));
			}
		}
		else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
			holdLeft = true; holdTime = 0; holdCount = 0;
			if (cur == 0 && ResPrev.activeSelf) OnResPrev(null); 
			else if (cur == 1 && DMPrev.activeSelf) OnDMPrev(null);
			else if (cur == 2 && MVS.value > 0) MVS.value -= 0.01F;
			else if (cur == 3 && SVS.value > 0) SVS.value -= 0.01F;
			else if (cur == 4 && VidPrev.activeSelf) OnVidPrev(null);
			else if (cur == 5 && LPrev.activeSelf) OnLPrev(null);
			else if (cur == 6 && FSPrev.activeSelf) OnFSPrev(null);
			else if (cur == 7 && APrev.activeSelf) OnAPrev(null);
			else if (cur == 8 && EffPrev.activeSelf) OnEffPrev(null);
			else if (cur == 9 && OVS.value > 0.05F) OVS.value -= 0.01F;
			else if (cur == 11) { cur = 10; BS.color = blue; BR.color = Color.white; }
			else if (cur == 12) { cur = 11; BR.color = blue; BB.color = Color.white; }
			else if (cur == -1) { cur = 10; BS.color = blue; BR.color = Color.white; BB.color = Color.white; }
		}
		else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
			holdRight = true; holdTime = 0; holdCount = 0;
			if (cur == 0 && ResNext.activeSelf) OnResNext(null); 
			else if (cur == 1 && DMNext.activeSelf) OnDMNext(null);
			else if (cur == 2 && MVS.value < 1) MVS.value += 0.01F;
			else if (cur == 3 && SVS.value < 1) SVS.value += 0.01F;
			else if (cur == 4 && VidNext.activeSelf) OnVidNext(null);
			else if (cur == 5 && LNext.activeSelf) OnLNext(null);
			else if (cur == 6 && FSNext.activeSelf) OnFSNext(null);
			else if (cur == 7 && ANext.activeSelf) OnANext(null);
			else if (cur == 8 && EffNext.activeSelf) OnEffNext(null);
			else if (cur == 9 && OVS.value < 0.75F) OVS.value += 0.01F;
			else if (cur == 10) { cur = 11; BS.color = Color.white; BR.color = blue; }
			else if (cur == 11) { cur = 12; BR.color = Color.white; BB.color = blue; }
			else if (cur == -1) { cur = 10; BS.color = blue; BR.color = Color.white; BB.color = Color.white; }
		}
		else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A)) holdLeft = false;
		else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D)) holdRight = false;
		else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
			if (cur == 10) OnSave(null);
			else if (cur == 11) OnReset(null);
			else if (cur == 12) OnBack(null);
		}
		else if (Input.GetKeyDown(KeyCode.Escape)) OnBack(null);

		if (holdLeft || holdRight) {
			holdTime += Time.deltaTime;
			if (holdTime > 0.5F) holdCount++;
			if (holdCount > 2) {
				holdCount = 0;
				if (holdLeft) {
					if (cur == 2 && MVS.value > 0) MVS.value -= 0.01F;
					else if (cur == 3 && SVS.value > 0) SVS.value -= 0.01F;
					else if (cur == 9 && OVS.value > 0.05F) OVS.value = OVS.value - 0.01F;
				} else {
					if (cur == 2 && MVS.value < 1) MVS.value += 0.01F;
					else if (cur == 3 && SVS.value < 1) SVS.value += 0.01F;
					else if (cur == 9 && OVS.value < 0.75F) OVS.value = OVS.value + 0.01F;
				}
			}
		}
	}
}