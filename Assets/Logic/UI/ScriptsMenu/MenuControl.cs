using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.IO;

public enum uisounds { click1, click2, back, open, go, save, delete, exit }

public class MenuControl : MonoBehaviour {
	public bool submenuOpened = false;
	DataControl D;
	public GameObject Main; GameObject Pause, PauseBckg, SaveLoad, Settings;
	GameObject Exit; Text ExitYes, ExitNo;
	Color defaultBlue = new Color(0.188F, 0.647F, 1);
	//Color brighterBlue = new Color(0.718F, 0.878F, 1);
	int currentItem = -1; int itemsCount;
	public bool changedUI = false, changedVideoRes = false, changedTransp = false, changedEffects = false;
	Image CreditsBckg, CreditsStop; RectTransform CreditsArea; int crdPhase = 0;
	Image MbS; GameObject LoadMsg; Color c;
	Image QS1; Text QS2; Vector2 qsPos, qsPosShifted;
	public int phase = 0; int qPhase = 0; int sceneID; bool tbc = false; float fadeOutSpd;
	bool isMM = true; bool gotSaves; int firstItem = 0;
	public int prevSample = 0;
	public AudioSource SFX; public AudioClip sfxClick1, sfxClick2, sfxBack, sfxOpen, sfxGo, sfxSave, sfxDelete, sfxExit;
	Transform htp;
	public float scl = 1;
	public SteamControl STEAM;

	//-------------------------------- COMMON ----------------------------------

	void Scaling(Transform area, float scale) {
		Vector2 v;
		var R = area.GetComponentsInChildren<RectTransform>(true);
		var T = area.GetComponentsInChildren<Text>(true);
		for (int i = 0; i < R.Length; i++) {
			v = R[i].offsetMin; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); R[i].offsetMin = v;
			v = R[i].offsetMax; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); R[i].offsetMax = v;
		}
		for (int i = 0; i < T.Length; i++) T[i].fontSize = Mathf.RoundToInt(T[i].fontSize * scale);
	}
	public void ReScale(int W, int H) {
		var old = 1.0F / scl; scl = W / 1280.0F;
		Scaling(transform.Find("Loading"), scl * old);
		Scaling(transform.Find("Main"), scl * old);
		Scaling(CreditsArea.parent, scl * old);
	}

	public void Init() {
		//Debug.Log(SystemInfo.graphicsDeviceType);
		//Cursor.SetCursor(CursorImg, Vector2.zero, CursorMode.Auto); //done by simply setting "Default Cursor" in Player Settings
		isMM = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 0;
		D = GameObject.Find("Data").GetComponent<DataControl>();
		STEAM = GameObject.Find("Steam").GetComponent<SteamControl>();
		Main = transform.Find("Main").gameObject;
		Pause = transform.Find("Pause").gameObject; PauseBckg = transform.Find("PauseBackground").gameObject;
		SaveLoad = transform.Find("SaveLoad").gameObject; Settings = transform.Find("Settings").gameObject;
		MbS = transform.Find("BlackScreen").GetComponent<Image>(); LoadMsg = transform.Find("Loading").gameObject;
		SFX = transform.parent.Find("SFX").GetComponent<AudioSource>();
		htp = !isMM ? null : GameObject.Find("HowToPlay").transform.Find("Panel").transform;
		CreditsBckg = !isMM ? null : GameObject.Find("Credits").transform.Find("Bckg").GetComponent<Image>();
		CreditsArea = !isMM ? null : CreditsBckg.transform.parent.Find("Area").GetComponent<RectTransform>();
		CreditsStop = !isMM ? null : CreditsBckg.transform.parent.Find("Stop").GetComponent<Image>();
		SaveLoad.SetActive(false); Settings.SetActive(false);

		if (Screen.width != 1280) {
			scl = Screen.width / 1280.0F;
			Scaling(transform.Find("Loading"), scl);
			if (isMM) { Scaling(transform.Find("Main"), scl); Scaling(CreditsArea.parent, scl); }
			else if (scl < 1) Scaling(transform.Find("Pause"), scl);
		}

		if (isMM) {
			var info = new DirectoryInfo(COMMON.saveFolder).GetFiles("*Save*.bin");
			gotSaves = info.Length > 0; if (!gotSaves) firstItem = 1;
			Main.transform.Find("Options/Continue").gameObject.SetActive(gotSaves);
			Main.transform.Find("Options").GetComponent<VerticalLayoutGroup>().spacing = gotSaves ? 4 : 5;
		}
		if (isMM && COMMON.demoVersion) {
			var DV = transform.Find("Main/DemoVersion").GetComponent<RectTransform>();
			var GT = transform.Find("Main/GameTitle").GetComponent<Text>();
			var G = new GUIStyle(); G.wordWrap = true; G.richText = true; G.font = GT.font; G.fontSize = GT.fontSize; G.fontStyle = GT.fontStyle;
			var szz = G.CalcSize(new GUIContent(GT.text)); DV.gameObject.SetActive(true);
			DV.anchoredPosition = new Vector2(-Mathf.RoundToInt(Screen.width / 2 - szz.x / 2), -Mathf.RoundToInt(szz.y / 2 + 10 * scl));
		}

		SFX.volume = COMMON.U.volS;
		itemsCount = isMM ? Main.transform.Find("Options").childCount : Pause.transform.Find("Options").childCount;
		Exit = transform.Find(isMM ? "Main/ExitConfirm" : "Pause/ExitConfirm").gameObject; Exit.SetActive(false);
		Main.SetActive(isMM && COMMON.mainMenuSkipLogos ? true : false);
		ExitYes = Exit.transform.Find("Yes").GetComponent<Text>();
		ExitNo = Exit.transform.Find("No").GetComponent<Text>();
		if (!isMM) { //quicksave preparations
			QS1 = transform.parent.Find("UI/Quicksave").GetComponent<Image>();
			QS2 = QS1.transform.Find("Text").GetComponent<Text>();
			float scale = Screen.width / 1280.0F;
			qsPos = new Vector2(Mathf.RoundToInt(-75*scale), Mathf.RoundToInt(-10*scale));
			qsPosShifted = new Vector2(Mathf.RoundToInt(-10*scale), 0);
		}
	}

	public void PlaySFX(uisounds s) {
		SFX.Stop(); SFX.timeSamples = 0;
		switch (s) {
		case uisounds.click1: SFX.clip = sfxClick1; break;
		case uisounds.click2: SFX.clip = sfxClick2; break;
		case uisounds.back: SFX.clip = sfxBack; break;
		case uisounds.open: SFX.clip = sfxOpen; break;
		case uisounds.go: SFX.clip = sfxGo; break;
		case uisounds.save: SFX.clip = sfxSave; break;
		case uisounds.delete: SFX.clip = sfxDelete; break;
		case uisounds.exit: SFX.clip = sfxExit; break;
		default: break;
		}
		SFX.Play();
	}

	public void OnItemEnter(int index) {
		if (isMM) {
			if (currentItem >= 0) Main.transform.Find("Options").GetChild(currentItem).GetComponent<Text>().color = Color.white;
			Main.transform.Find("Options").GetChild(index).GetComponent<Text>().color = defaultBlue;
			currentItem = index;
		} else {
			if (currentItem >= 0) Pause.transform.Find("Options").GetChild(currentItem).GetComponent<Text>().color = Color.white;
			Pause.transform.Find("Options").GetChild(index).GetComponent<Text>().color = defaultBlue;
			currentItem = index;
		}
	}
	public void OnItemExit() {
		if (currentItem != -1) {
			if (isMM) {
				Main.transform.Find("Options").GetChild(currentItem).GetComponent<Text>().color = Color.white;
				currentItem = -1;
			} else {
				Pause.transform.Find("Options").GetChild(currentItem).GetComponent<Text>().color = Color.white;
				currentItem = -1;
			}
		}
	}

	public void OnRecent(BaseEventData d) {
		if (!COMMON.demoVersion) PlaySFX(uisounds.go);
		var info = new DirectoryInfo(COMMON.saveFolder).GetFiles("*Save*.bin");
		System.Array.Sort(info, SaveLoad.GetComponent<SaveLoad>().Comparison);
		COMMON.saveToLoad = info[0].Name;
		if (!COMMON.demoVersion) ((DataControlMainMenu)D).LoadScreen(info[0].Name);
		else {
			var S = SaveGame.Load(COMMON.saveFolder + info[0].Name);
			if (S.SH == null || !S.SH.startedCH2) {
				PlaySFX(uisounds.go); ((DataControlMainMenu)D).LoadScreen(info[0].Name);
			} else PlaySFX(uisounds.back);
		}
	}
	public void OnNew(BaseEventData d) {
		PlaySFX(uisounds.go);
		COMMON.saveToLoad = null;
		((DataControlMainMenu)D).LoadScreen(null);
	}
	public void OnCredits(BaseEventData d) {
		PlaySFX(uisounds.open); D.CursorLock(true);
		CreditsBckg.color = new Color(0, 0, 0, 0); CreditsBckg.gameObject.SetActive(true);
		crdPhase = 1;
		submenuOpened = true; OnItemExit();
	}

	// ----------------------------------- HOW TO PLAY ------------------------------------------
	public void OnHowToPlay(BaseEventData d) {
		PlaySFX(uisounds.open); htp.gameObject.SetActive(true);
		htp.Find("1").gameObject.SetActive(true); htp.Find("Next").gameObject.SetActive(true);
		htp.Find("2").gameObject.SetActive(false); htp.Find("Close").gameObject.SetActive(false);
		submenuOpened = true; Main.SetActive(false); OnItemExit();
	}
	public void OnHTPNextEnter(BaseEventData d) { htp.Find("Next").GetComponent<Text>().color = defaultBlue; }
	public void OnHTPNextExit(BaseEventData d) { htp.Find("Next").GetComponent<Text>().color = Color.white; }
	public void OnHTPCloseEnter(BaseEventData d) { htp.Find("Close").GetComponent<Text>().color = defaultBlue; }
	public void OnHTPCloseExit(BaseEventData d) { htp.Find("Close").GetComponent<Text>().color = Color.white; }
	public void OnHTPNextClick(BaseEventData d) {
		PlaySFX(uisounds.click2); htp.Find("Next").GetComponent<Text>().color = Color.white;
		htp.Find("1").gameObject.SetActive(false); htp.Find("Next").gameObject.SetActive(false);
		htp.Find("2").gameObject.SetActive(true); htp.Find("Close").gameObject.SetActive(true);
	}
	public void OnHTPCloseClick(BaseEventData d) {
		htp.Find("Close").GetComponent<Text>().color = Color.white;
		PlaySFX(uisounds.click2); htp.gameObject.SetActive(false);
		submenuOpened = false; Main.SetActive(true);
	}

	// ----------------------------------- SAVE-LOAD-CONTINUE -----------------------------------
	public void OnLoad(BaseEventData d) {
		PlaySFX(uisounds.open);
		if (isMM) Main.SetActive(false); else Pause.SetActive(false);
		SaveLoad.SetActive(true);
		SaveLoad.GetComponent<SaveLoad>().Show(true);
		submenuOpened = true;
		OnItemExit();
	}
	public void OnSettings(BaseEventData d) {
		PlaySFX(uisounds.open);
		if (isMM) Main.SetActive(false); else Pause.SetActive(false);
		Settings.GetComponent<Settings>().Show();
		Settings.SetActive(true);
		submenuOpened = true;
		OnItemExit();
	}
	public void OnContinue(BaseEventData d) {
		HidePause();
		OnItemExit();
	}
	public void OnSave(BaseEventData d) {
		PlaySFX(uisounds.open);
		Pause.SetActive(false); SaveLoad.SetActive(true);
		SaveLoad.GetComponent<SaveLoad>().Show(false);
		submenuOpened = true;
		OnItemExit();
	}

	// ----------------------------------- EXIT -----------------------------------
	public void OnExitGame(BaseEventData d) {
		PlaySFX(uisounds.click2);
		if (isMM) Main.transform.Find("Options").gameObject.SetActive(false);
		else {
			Pause.transform.Find("Options").gameObject.SetActive(false);
			Exit.transform.Find("Text1").gameObject.SetActive(false);
			Exit.transform.Find("Text2").gameObject.SetActive(true);
		}
		Exit.SetActive(true); ExitYes.color = Color.white; ExitNo.color = Color.white;
		submenuOpened = true; OnItemExit();
	}
	public void OnMainMenu(BaseEventData d) {
		PlaySFX(uisounds.click2);
		Pause.transform.Find("Options").gameObject.SetActive(false);
		Exit.transform.Find("Text1").gameObject.SetActive(true);
		Exit.transform.Find("Text2").gameObject.SetActive(false);
		Exit.SetActive(true); ExitYes.color = Color.white; ExitNo.color = Color.white;
		submenuOpened = true; OnItemExit();
	}
	public void OnExitNoEnter(BaseEventData d) { ExitNo.color = defaultBlue; ExitYes.color = Color.white; }
	public void OnExitNoExit(BaseEventData d) { ExitNo.color = Color.white; }
	public void OnExitYesEnter(BaseEventData d) { ExitYes.color = defaultBlue; ExitNo.color = Color.white; }
	public void OnExitYesExit(BaseEventData d) { ExitYes.color = Color.white; }
	public void OnExitYesClick(BaseEventData d) {
		Exit.SetActive(false);
		if (isMM || Exit.transform.Find("Text2").gameObject.activeSelf) {
			PlaySFX(uisounds.exit); sceneID = -20; fadeOutSpd = 0.025F;
			c = MbS.color; c.a = 0; MbS.color = c; MbS.gameObject.SetActive(true);
			D.CursorLock(true); phase = 1;
		} else {
			PlaySFX(uisounds.go); COMMON.mainMenuSkipLogos = true; LoadLevel(-1);
		}
	}
	public void OnExitNoClick(BaseEventData d) {
		PlaySFX(uisounds.back);
		if (isMM) Main.transform.Find("Options").gameObject.SetActive(true);
		else Pause.transform.Find("Options").gameObject.SetActive(true);
		Exit.SetActive(false); submenuOpened = false; ExitNo.color = Color.white;
	}

	// ----------------------------------- SAVE/LOAD -----------------------------------
	public void LoadLevel(int lid, bool slowFadeOut = false) {
		sceneID = (lid >= 10) ? 7 : ((lid == 0) ? 6 : ((lid == -1) ? 0 : lid));
		if (COMMON.demoVersion && sceneID > 1) sceneID -= 3; //as we remove 3 levels (2-3-4) from the demo
		fadeOutSpd = !slowFadeOut ? 0.025F : 0.005F;
		c = MbS.color; c.a = 0; MbS.color = c; MbS.gameObject.SetActive(true);
		D.CursorLock(true); D.bMenu.SetActive(false); phase = 1;
	}
	IEnumerator Loading() {
		UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneID);
		yield return new WaitForSeconds(1);
		LoadMsg.transform.SetAsLastSibling(); LoadMsg.SetActive(true);
	}
	public void FadeInLevel(bool turnBackColliders) {
		c = MbS.color; c.a = 1; MbS.color = c; MbS.gameObject.SetActive(true);
		tbc = turnBackColliders; if (tbc) D.UIC.Col(false);
		phase = 2;
	}
	IEnumerator Quicksave() {
		D.S.Save(COMMON.saveFolder + "Quicksave.bin");
		SaveLoad.GetComponent<SaveLoad>().TakeScreenshot("Quicksave");
		bool shifted = COMMON.U.textLayout == 1 && (D.S.inNarration || D.S.inDialogue >= 0);
		QS1.GetComponent<RectTransform>().anchorMin = (!shifted) ? Vector2.one : new Vector2(1, 0.75F);
		QS1.GetComponent<RectTransform>().anchorMax = (!shifted) ? Vector2.one : new Vector2(1, 0.75F);
		QS1.GetComponent<RectTransform>().anchoredPosition = (!shifted) ? qsPos : qsPosShifted;
		c = QS1.color; c.a = 0.627F; QS1.color = c; QS1.gameObject.SetActive(true);
		c = QS2.color; c.a = 1; QS2.color = c;
		yield return new WaitForSeconds(1.5F); qPhase = 1;
	}

	public void ShowPause() {
		if (D.S.inDialogue == -1 && !D.S.inNarration) D.UIC.Col(false);
		PauseBckg.SetActive(true); Pause.SetActive(true);
		D.paused = true; D.PauseAnimations(true); if (D.BGM.clip != null) D.BGM.Pause();
		D.UIC.ClearSelection();
		D.bMenu.SetActive(false); PlaySFX(uisounds.back);
	}
	void HidePause() {
		if (D.S.inDialogue == -1 && !D.S.inNarration) D.UIC.Col(true);
		PauseBckg.SetActive(false); Pause.SetActive(false);
		D.paused = false; D.PauseAnimations(false); if (D.BGM.clip != null) D.BGM.UnPause();
		if (changedUI) { changedUI = false; D.UIC.InitSettings(true); }
		if (changedTransp) { changedTransp = false; D.UIC.SetTransparency(); }
		if (changedEffects) { changedEffects = false; D.UIC.SetEffects(); }
		if (changedVideoRes) { changedVideoRes = false; D.VideoResChanged(); }
		D.bMenu.SetActive(true);
	}

	//void Awake() { QualitySettings.vSyncCount = 0; Application.targetFrameRate = 30; }

	void Update() {
		if (Application.isEditor) {
			if (Input.GetKeyDown(KeyCode.O)) {
				var s = new string[] {"Dialogue", "Responses", "Narration", "NarrationAlt", "DialogueAlt", "ButtonMenu", "ButtonArrow"};
				foreach (var ss in s) D.UIC.transform.Find(ss).gameObject.SetActive(false);
				//GameObject.Find("Camera").GetComponent<maxCamera>().enabled = true;
			}
			if (Input.GetKeyDown(KeyCode.P)) {
				string fn = System.DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss");
				Application.CaptureScreenshot("C:/Users/SNDP/Desktop/SWF/Environment/Screenshots/" + fn + ".png");
			}
			if (Input.GetKeyDown(KeyCode.B) && D.BGM != null && !D.paused)
				D.BGM.timeSamples = D.BGM.clip.samples - 441000;
		}

		//ESC
		if (!isMM) {
			if (Input.GetKeyDown(KeyCode.Escape) && !D.paused && D.bMenu.activeSelf) ShowPause();
			else if (Input.GetKeyDown(KeyCode.Escape) && D.paused && !submenuOpened) { HidePause(); OnItemExit(); }
		}

		//SAVE/LOAD
		if (!isMM && Input.GetKeyDown(KeyCode.F5) && phase == 0 && D.bMenu.activeSelf) StartCoroutine(Quicksave());
		else if (Input.GetKeyDown(KeyCode.F8) && phase == 0 && (!isMM || Cursor.visible)) {
			COMMON.saveToLoad = "Quicksave.bin";
			var p = COMMON.saveFolder + COMMON.saveToLoad;
			if (File.Exists(p)) {
				//COMMON.trailerRecordMode = -1;
				var SNEW = SaveGame.Load(p);
				if (!COMMON.demoVersion) LoadLevel(SNEW.levelID);
				else if (SNEW.SH == null || !SNEW.SH.startedCH2) LoadLevel(SNEW.levelID);
			}
		}
		if (phase == 1 && MbS.color.a < 1) {
			c = MbS.color; c.a += fadeOutSpd * Time.deltaTime * 60; MbS.color = c;
			D.BGM.volume = (1-c.a)*COMMON.U.volM;
			if (D.Sound != null && D.Sound.isPlaying) D.Sound.volume = (1-c.a)*COMMON.U.volS;
			if (D.Sound2 != null && D.Sound2.isPlaying) D.Sound2.volume = (1-c.a)*COMMON.U.volS;
		}
		else if (phase == 1 && MbS.color.a >= 1) { phase = 0; D.BGM.Stop(); if (sceneID > -20) StartCoroutine(Loading()); else Application.Quit(); }
		else if (phase == 2 && MbS.color.a > 0) { c = MbS.color; c.a -= 0.05F * Time.deltaTime * 60; MbS.color = c; }
		else if (phase == 2 && MbS.color.a <= 0) { phase = 0; if (tbc) D.UIC.Col(true); MbS.gameObject.SetActive(false); }
		if (qPhase == 1 && QS2.color.a > 0) {
			c = QS1.color; c.a -= 0.031F * Time.deltaTime * 60; QS1.color = c;
			c = QS2.color; c.a -= 0.05F * Time.deltaTime * 60; QS2.color = c;
		}
		else if (qPhase == 1 && QS2.color.a <= 0) { qPhase = 0; QS1.gameObject.SetActive(false); }

		//MUSIC
		if (!D.paused) {
			if (D.BGM.clip != null && D.BGM.isPlaying && D.loopAt > 0.05F) { //for loopAt = 0, having "Loop" in AudioSource settings is already enough
				if (D.BGM.timeSamples < prevSample) {
					D.BGM.timeSamples = Mathf.RoundToInt(D.loopAt * D.BGM.clip.frequency);
					if (Application.isEditor) Debug.Log("loop BGM at " + D.loopAt);
				}
				prevSample = D.BGM.timeSamples;
			}
			if (D.musicFadeOut && D.BGM.volume > 0) {
				var decr = ((D.S == null || D.S.levelID < 10) ? 0.02F : 0.01F) * COMMON.U.volM;
				D.BGM.volume -= decr * Time.deltaTime * 60;
			}
			else if (D.musicFadeOut) {
				D.musicFadeOut = false;
				if (D.playNext == null) { D.BGM.Stop(); D.BGM.clip = null; }
				else D.MPlay(D.playNext, D.loopAt, D.resumeNextAt);
			}
			if (D.musicFadeIn && D.BGM.volume < COMMON.U.volM) D.BGM.volume += 0.02F * Time.deltaTime * 60 * COMMON.U.volM;
			else if (D.musicFadeIn) { D.BGM.volume = COMMON.U.volM; D.musicFadeIn = false; }
		}

		if (isMM || D.paused) {
			if (!submenuOpened) {
				if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) {
					if (currentItem == -1) OnItemEnter(firstItem);
					else if (currentItem < itemsCount - 1) OnItemEnter(currentItem + 1);
				}
				else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) {
					if (currentItem == -1) OnItemEnter(firstItem);
					else if (currentItem > firstItem) OnItemEnter(currentItem - 1);
				}
				else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
					if (isMM) {
						if (currentItem == 0) OnRecent(null);
						else if (currentItem == 1) OnNew(null);
						else if (currentItem == 2) OnLoad(null);
						else if (currentItem == 3) OnHowToPlay(null);
						else if (currentItem == 4) OnSettings(null);
						else if (currentItem == 5) OnCredits(null);
						else if (currentItem == 6) OnExitGame(null);
					} else {
						if (currentItem == 0) OnContinue(null);
						else if (currentItem == 1) OnSave(null);
						else if (currentItem == 2) OnLoad(null);
						else if (currentItem == 3) OnSettings(null);
						else if (currentItem == 4) OnMainMenu(null);
						else if (currentItem == 5) OnExitGame(null);
					}
				}
			}
			else if (submenuOpened && htp != null && htp.gameObject.activeSelf) {
				if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
					if (htp.Find("1").gameObject.activeSelf) OnHTPNextClick(null); else OnHTPCloseClick(null);
				}
			}
			else if (submenuOpened && Exit.activeSelf) {
				if (Input.GetKeyDown(KeyCode.Escape)) OnExitNoClick(null);
				else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
					ExitYes.color = Color.white; ExitNo.color = defaultBlue;
				}
				else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) {
					ExitYes.color = defaultBlue; ExitNo.color = Color.white;
				}
				else if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) {
					if (ExitNo.color == defaultBlue) OnExitNoClick(null);
					else if (ExitYes.color == defaultBlue) OnExitYesClick(null);
				}
			}
		}

		//credits
		if (isMM) {
			if (crdPhase == 1 && CreditsBckg.color.a < 1)
				CreditsBckg.color = new Color(0, 0, 0, CreditsBckg.color.a + 0.02F * Time.deltaTime * 60);
			else if (crdPhase == 1) {
				CreditsArea.anchoredPosition = new Vector2(0, -Screen.height);
				CreditsArea.gameObject.SetActive(true); crdPhase = 2;
			}
			else if (crdPhase == 2 && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))) {
				CreditsStop.color = new Color(0, 0, 0, 0);
				CreditsStop.gameObject.SetActive(true);
				crdPhase = 3;
			}
			else if (crdPhase == 2 && CreditsArea.anchoredPosition.y < CreditsArea.sizeDelta.y) {
				var step = Screen.width >= 1920 ? 1 : 0.5F; if (Time.deltaTime > 0.025F) step *= 2;
				CreditsArea.anchoredPosition += new Vector2(0, step);
			}
			else if (crdPhase == 2) {
				CreditsArea.gameObject.SetActive(false); crdPhase = 4;
			}
			//if interrupted, include an additional step of fading out credits by fading in another black screen
			else if (crdPhase == 3 && CreditsStop.color.a < 1) {
				var step = Screen.width >= 1920 ? 1 : 0.5F; if (Time.deltaTime > 0.025F) step *= 2;
				CreditsArea.anchoredPosition += new Vector2(0, step);
				CreditsStop.color = new Color(0, 0, 0, CreditsStop.color.a + 0.02F * Time.deltaTime * 60);
			}
			else if (crdPhase == 3) {
				CreditsStop.gameObject.SetActive(false);
				CreditsArea.gameObject.SetActive(false); crdPhase = 4;
			}
			else if (crdPhase == 4 && CreditsBckg.color.a > 0)
				CreditsBckg.color = new Color(0, 0, 0, CreditsBckg.color.a - 0.02F * Time.deltaTime * 60);
			else if (crdPhase == 4) {
				CreditsBckg.gameObject.SetActive(false);
				crdPhase = 0; D.CursorLock(false); submenuOpened = false;
			}
		}
	}
}