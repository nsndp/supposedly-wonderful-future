using UnityEngine;
using System.Collections;
using System.IO;

public class DataControlChapter4 : DataControl {

	public TXT Dialogue; public Memory[] M;
	public Transform cam; public OnClick_Notebook NB; public GameObject NmPlt, Area;
	public Vector3 camPos, camPosOut, camPosNB; public Quaternion camRot, camRotOut, camRotNB;
	public AudioClip mStart, mFrames, mNotebook, mTruth, mStay, mColor;

	public override int[] GetCCID() { return null; }
	public override void UISettingsChanged() { }

	void Start() {
		if (!GetComponent<BinGeneration_Memories>().enabled) Init();
	}
	public void Init() {
		COMMON.LoadUserSettings(); if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, 4);
		dataFolder = COMMON.dataFolder + "Memories/";
		Dialogue = TXT.Load(LOC("Dialogue"));
		Narration = TXT.Load(LOC("Narration"));
		NS = NarrationStructure.Load(dataFolder + "Narration.bin");

		if (COMMON.saveToLoad == null) {
			Debug.Log("TEST MODE"); new GameObject("Steam", typeof(SteamControl));
			S = new SaveGame(); S.levelID = 4;
		}
		else {
			S = SaveGame.Load(COMMON.saveFolder + COMMON.saveToLoad);
			if (Application.isEditor) Debug.Log("Game loaded: " + COMMON.saveToLoad);
		}
		if (S.SC4 == null) {
			S.SC4 = new SaveChapter4();
			S.SC4.DStruct = DialogueStructure.Load(dataFolder + "Dialogue.bin");
		}

		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		bMenu = UIC.transform.Find("ButtonMenu").gameObject;
		bReturn = UIC.transform.Find("ButtonArrow").gameObject;
		bS = UIC.transform.Find("BlackScreen").GetComponent<UnityEngine.UI.Image>();
		MC.Init(); UIC.Init(); AssignVectors(); SetStates();

		//UIC.GetLongestLine("Dialogue", Dialogue); UIC.GetLongestLine("Narration", Narration);
	}

	void AssignVectors() {
		var a = (Screen.width * 1.0F / Screen.height - 1.7777777777F) / (1.25F - 1.7777777777F);
		camPosOut = new Vector3(-5.6F, 1.5F, -3);
		camRotOut = Quaternion.Euler(new Vector3(14, 10, 0));
		camPos = Vector3.Lerp(new Vector3(-4.84F, 1.03F, -0.99F), new Vector3(-4.84F, 1.1F, -1.1F), a);
		camRot = Quaternion.Euler(new Vector3(Mathf.Lerp(21, 19, a), 351, 0));
		camPosNB = new Vector3(-4.831F, 0.662F, -0.391F); //NB coord = 0.1F, 0.601F, -0.4F
		camRotNB = Quaternion.Euler(new Vector3(90, 0, 0));
	}

	void SetStates() {		
		currentColliders = GameObject.Find("Colliders");
		cam = GameObject.Find("Camera").transform;
		NB = currentColliders.transform.Find("Notebook").GetComponent<OnClick_Notebook>();
		NmPlt = currentColliders.transform.Find("Nameplate").gameObject;
		Area = currentColliders.transform.Find("Area").gameObject;
		M = currentColliders.GetComponentsInChildren<Memory>(); for (int i = 0; i < 8; i++) M[i].Init();

		BGM = transform.Find("Music").GetComponent<AudioSource>(); BGM.volume = COMMON.U.volM;
		Sound = transform.Find("Sound").GetComponent<AudioSource>(); Sound.volume = COMMON.U.volS;
		if (S.SC4.theChoice == 1) { BGM.clip = mColor; loopAt = 0; }
		else if (S.SC4.theChoice == 2) { BGM.clip = mStay; loopAt = 0; }
		else if (S.SC4.sawReveal || S.SC4.revealStage > 1) { BGM.clip = mTruth; loopAt = 0; }
		else if (S.SC4.revealStage == 0 || S.SC4.revealStage == 1) { BGM.clip = S.inNarration ? mNotebook : null; loopAt = 0; }
		else if (S.SC4.memorySeen[0] && S.SC4.memorySeen[1] && S.SC4.memorySeen[2]) { BGM.clip = mFrames; loopAt = 17.993F; }
		else if (S.SC4.firstGlimpse) { BGM.clip = mStart; loopAt = 0; }
		else BGM.clip = null;
		if (BGM.clip != null) BGM.Play();

		if (S.SC4.curM >= 0) { cam.localPosition = M[S.SC4.curM].dstPos; cam.localRotation = M[S.SC4.curM].dstRot; }
		else if (S.SC4.revealStage >= 0) { cam.localPosition = camPosNB; cam.localRotation = camRotNB; }
		else if (!S.SC4.startingZoom || S.SC4.shatterMe) { cam.localPosition = camPosOut; cam.localRotation = camRotOut; }
		else { cam.localPosition = camPos; cam.localRotation = camRot; }

		if (S.SC4.firstGlimpse && !S.SC4.memorySeen[0] && !S.SC4.sawName) NmPlt.SetActive(true); else NmPlt.SetActive(false);
		if (S.SC4.memorySeen[7] && !S.SC4.sawReveal) NB.gameObject.SetActive(true); else NB.gameObject.SetActive(false);
		if (S.SC4.activatedNotebook) NB.OBJ.Find("ImgA").gameObject.SetActive(true);
		if (S.SC4.revealStage > 0) NB.Reload(S.SC4.revealStage);
		if (S.SC4.curM >= 0) {
			if (S.SC4.shatterMe && M[S.SC4.curM].mov != null) M[S.SC4.curM].MovieFirstTime();
			else if (!S.SC4.sawReveal) M[S.SC4.curM].animate = true;
			else if (!S.SC4.sawFinishP1) M[S.SC4.curM].ReloadCracks(2);
			else if (!S.SC4.sawFinishP2) M[S.SC4.curM].ReloadCracks(3);
			else M[S.SC4.curM].ReloadCracks(4 + S.SC4.addCracks);
		}
		if (!S.SC4.startingZoom || S.SC4.shatterMe && S.SC4.curM == -1)
			for (int i = 0; i < M.Length; i++) M[i].gameObject.SetActive(false);
		else Area.SetActive(false);

		if (!S.SC4.startingZoom) UPP(0);
		else if (!S.SC4.readyToRemember) UPP(1);
		else if (!S.SC4.memorySeen[0]) UPP(2);
		else if (!S.SC4.memorySeen[1] && !S.SC4.memorySeen[2]) UPP(3);
		else if (!S.SC4.memorySeen[1]) UPP(4);
		else if (!S.SC4.memorySeen[2]) UPP(5);
		else if (!S.SC4.memorySeen[3] && !S.SC4.memorySeen[4]) UPP(6);
		else if (!S.SC4.memorySeen[5] && !S.SC4.memorySeen[6]) UPP(7);
		else if (!S.SC4.memorySeen[7]) UPP(8);
		else if (!S.SC4.sawReveal) UPP(9);
		else if (!S.SC4.shatterMe) UPP(10);
		else UPP(-1);

		CursorLock(false); bS.gameObject.SetActive(!S.SC4.seenIntro);
		if (!S.SC4.seenIntro) UIC.showTitle(4, (int)events4.finishIntro);
		else if (S.inNarration) UIC.StartNarration(S.NID);
		else if (S.inDialogue >= 0) UIC.StartDialogue(Dialogue, S.SC4.DStruct, 0, S.SC4.nextDID, false);
		MC.FadeInLevel(S.SC4.seenIntro && !S.inNarration && S.inDialogue == -1);
	}

	public void UPP(int vl) {
		S.SC4.npp = vl; var hh = GetComponent<HighlightHints>();
		switch (vl) {
		case 0: hh.NPS = new Transform[] {Area.transform}; break;
		case 1: hh.NPS = new Transform[] {M[0].transform, M[1].transform, M[2].transform}; break;
		case 2: hh.NPS = new Transform[] {M[0].transform}; break;
		case 3: hh.NPS = new Transform[] {M[1].transform, M[2].transform}; break;
		case 4: hh.NPS = new Transform[] {M[1].transform}; break;
		case 5: hh.NPS = new Transform[] {M[2].transform}; break;
		case 6: hh.NPS = new Transform[] {M[3].transform, M[4].transform}; break;
		case 7: hh.NPS = new Transform[] {M[5].transform, M[6].transform}; break;
		case 8: hh.NPS = new Transform[] {M[7].transform}; break;
		case 9: hh.NPS = new Transform[] {NB.transform}; break;
		case 10: hh.NPS = new Transform[] {
				M[0].transform, M[1].transform, M[2].transform, M[3].transform,
				M[4].transform, M[5].transform, M[6].transform, M[7].transform
			};
			break;
		default: hh.NPS = null; break;
		}
	}

	public override void VideoResChanged() {
		if (S.SC4.shatterMe) {
			if (S.SC4.curM == -1) for (int i = 0; i < 8; i++) M[i].ResetMovie();
			else if (M[S.SC4.curM].mov != null) M[S.SC4.curM].MovieFirstTime();
		}
	}
	public override void PauseAnimations(bool pause) {
		if (S.SC4.shatterMe && S.SC4.curM >= 0) M[S.SC4.curM].PauseMovie(pause);
	}
}

[System.Serializable]
public class SaveChapter4 {
	public DialogueStructure DStruct;
	public int nextDID = 0;
	public int curM = -1;
	public int npp; //next plot point: 0 - area, 1 - f123, 2 - f1, 3 - f23, 4 - f2, 5 - f3, 6 - f45, 7 - f67, 8 - f8, 9 - notebook, 10 - any frame
	public bool seenIntro = false;
	public bool startingZoom = false;
	public bool firstGlimpse = false;
	public bool sawName = false;
	public bool readyToRemember = false;
	public bool noticedThing1 = false;
	public bool noticedThing2 = false;
	public bool activatedNotebook = false;
	public int revealStage = -1; //0 - start, 1 - imageB, 2 - one crack, 3 - two cracks, 4 - four cracks
	public bool sawReveal = false;
	public bool sawFinishP1 = false;
	public bool sawFinishP2 = false;
	public int addCracks = 0;
	public bool shatterMe = false;
	public int theChoice = 0; //1 - convinced, 2 - allowed to stay, 3 - failed
	public bool[] memoryActive = new bool[] {true, true, true, false, false, false, false, false};
	public bool[] memoryDelay = new bool[] {true, true, true, true, true, true, true, true};
	public int[] memoryDID = new int[] {-1, -1, -1, -1, -1, -1, -1, -1};
	public bool[] memorySeen = new bool[8];
}
