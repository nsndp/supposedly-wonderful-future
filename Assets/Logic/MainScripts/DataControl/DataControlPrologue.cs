using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class DataControlPrologue : DataControl {

	public TXT DialogueJackie, DialogueOther;
	public Animation A; public Transform cam; public Animation Door;
	public Vector3 JackiePosA, JackiePosB, camPosA, camPosB, camPosM;
	public Quaternion camRotA, camRotB, camRotM;
	public AudioClip dart, dartWall, board, blinds1, blinds2, blinds3, phone, doorOpen;
	public AudioClip main, dialogue, mainAlt, dialogueAlt; public bool isAltMusic;
	float a;

	public override int[] GetCCID() { return S.SP.CCID; }
	public override void VideoResChanged() { }
	public override void UISettingsChanged() {
		if (S.inDialogue == 0) {
			AssignVectors1(); cam.localPosition = !S.SP.invitedJackieIn ? camPosA : camPosB;
		}
	}

	void AssignVectors() {
		a = (Screen.width * 1.0F / Screen.height - 1.7777777777F) / (1.25F - 1.7777777777F);
		camPosM = new Vector3(Mathf.Lerp(-4, -4.5F, a), Mathf.Lerp(2, 2.15F, a), Mathf.Lerp(0, 0.1F, a));
		camRotM = Quaternion.Euler(new Vector3(15, 100, 0));
		camRotA = Quaternion.Euler(new Vector3(15, 100, 0));
		camRotB = Quaternion.Euler(new Vector3(14, 114, 0));
		JackiePosA = new Vector3(2.3F, 0, -2.08F);
		JackiePosB = new Vector3(-0.975F, 0, -2.21F);
		AssignVectors1();
	}
	void AssignVectors1() {
		camPosA = new Vector3(Mathf.Lerp(-0.19F, -0.6F, a), COMMON.U.textLayout < 2 ? Mathf.Lerp(1.70F, 1.75F, a) : Mathf.Lerp(1.66F, 1.75F, a), Mathf.Lerp(-0.98F, -0.945F, a));
		camPosB = new Vector3(Mathf.Lerp(-2.19F, -2.32F, a), COMMON.U.textLayout < 2 ? Mathf.Lerp(1.19F, 1.2F, a) : Mathf.Lerp(1.12F, 1.2F, a), Mathf.Lerp(-0.95F, -0.89F, a));
	}

	void Start() {
		if (!GetComponent<BinGeneration_Office2016>().enabled) Init();
	}
	public void Init() {
		COMMON.LoadUserSettings(); if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, 0);
		dataFolder = COMMON.dataFolder + "Office2018/";
		DialogueJackie = TXT.Load(LOC("Jackie"));
		DialogueOther = TXT.Load(LOC("Other"));
		Comments = TXT.Load(LOC("Comments"));
		Narration = TXT.Load(LOC("Narration"));
		NS = NarrationStructure.Load(dataFolder + "Narration.bin");

		if (COMMON.saveToLoad == null) { 
			Debug.Log("TEST MODE"); new GameObject("Steam", typeof(SteamControl));
			S = new SaveGame();
		}
		else {
			S = SaveGame.Load(COMMON.saveFolder + COMMON.saveToLoad);
			if (Application.isEditor) Debug.Log("Game loaded: " + COMMON.saveToLoad);
		}
		if (S.SP == null) {
			S.SP = new SavePrologue();
			S.SP.DSJackie = DialogueStructure.Load(dataFolder + "Jackie.bin");
			S.SP.DSOther = DialogueStructure.Load(dataFolder + "Other.bin");
		}

		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		bMenu = UIC.transform.Find("ButtonMenu").gameObject;
		bReturn = UIC.transform.Find("ButtonArrow").gameObject;
		bS = UIC.transform.Find("BlackScreen").GetComponent<UnityEngine.UI.Image>();
		MC.Init(); UIC.Init(); AssignVectors(); SetStates();

		//UIC.GetLongestLine("Jackie", DialogueJackie); UIC.GetLongestLine("Other", DialogueOther); UIC.GetLongestLine("Comments", Comments); UIC.GetLongestLine("Narration", Narration);
	}

	void SetStates() {
		currentColliders = GameObject.Find("Colliders").gameObject;
		A = GameObject.Find("JackiePrologue").GetComponent<Animation>();
		Door = GameObject.Find("EntranceArea/Door").GetComponent<Animation>();
		cam = GameObject.Find("Camera").transform;
		cam.localPosition = camPosM; cam.localRotation = camRotM;

		currentColliders.transform.Find("Door").gameObject.SetActive(S.SP.knockKnock);
		currentColliders.transform.Find("Phone").gameObject.SetActive(S.SP.heardMessages < 3);
		currentColliders.transform.Find("Jackie").GetComponent<OnClick_Jackie>().Init();
		currentColliders.transform.Find("Dartboard").GetComponent<OnClick_Darts>().Init();
		GetComponent<WhiteboardZoom>().Init();
		UPP();

		GameObject.Find("Walls/Walls/Corridor").GetComponent<Transform>().localPosition = Vector3.zero;
		GameObject.Find("FrontArea/Darts").GetComponent<Transform>().localPosition = new Vector3(2, 0, -0.5F);
		GameObject.Find("FrontArea/clock").GetComponent<Clock>().minutes = S.SP.invitedJackieIn ? 45 : 15;
		GameObject.Find("Lights").transform.Find("CharacterLight").gameObject.SetActive(S.SP.invitedJackieIn);
		if (!S.SP.invitedJackieIn) {
			A.transform.localPosition = JackiePosA; A.transform.localRotation = Quaternion.Euler(new Vector3(0, -90, 0)); A.Play("Stand");
			currentColliders.transform.Find("Jackie").localPosition -= new Vector3(10, 0, 0);
			if (S.inDialogue == 0) {
				cam.localPosition = camPosA; cam.localRotation = camRotA;
				Door["DoorAnim"].time = Door["DoorAnim"].length; Door.Play();
				currentColliders.transform.Find("Door").gameObject.SetActive(false);
			}
		} else {
			currentColliders.transform.Find("Door").gameObject.SetActive(false);
			A.transform.localPosition = JackiePosB; A.transform.localRotation = Quaternion.Euler(Vector3.zero); A.Play("Sit");
			A.transform.Find("Anchor_Body").localPosition = new Vector3(0, 0.3F, 0);
			A.transform.Find("Anchor_Head").localPosition = new Vector3(0, 1.1F, -0.1F);
			if (S.inDialogue == 0) { cam.localPosition = camPosB; cam.localRotation = camRotB; }
			else {
				A["Turn"].speed = 1.0F; A["Turn"].time = A["Turn"].length;
				A["Turn"].wrapMode = WrapMode.ClampForever; A.Play("Turn");
			}
		}

		BGM = transform.Find("Music").GetComponent<AudioSource>(); BGM.volume = COMMON.U.volM;
		Sound = transform.Find("Sound").GetComponent<AudioSource>(); Sound.volume = COMMON.U.volS;
		Sound2 = transform.Find("Sound2").GetComponent<AudioSource>(); Sound2.volume = COMMON.U.volS;
		if (S.SP.knockKnock) Sound2.Play();
		if (!S.SP.seenTitle || S.inNarration || S.SP.knockKnock) BGM.clip = null;
		else if (S.inDialogue == 0) { BGM.clip = !isAltMusic ? dialogue : dialogueAlt; BGM.Play(); loopAt = 0; }
		else { BGM.clip = !isAltMusic ? main : mainAlt; BGM.Play(); loopAt = !isAltMusic ? 0 : 6.845F; }

		CursorLock(false); bS.gameObject.SetActive(S.SP.blackScreen);
		if (!S.SP.seenTitle) UIC.showTitle(0, (int)events0.seenTitle);
		else if (S.inNarration) UIC.StartNarration(S.NID);
		else if (S.inDialogue == 0) UIC.StartDialogue(DialogueJackie, S.SP.DSJackie, 0, S.SP.nextDIDJackie, false);
		else if (S.inDialogue == 1) UIC.StartDialogue(DialogueOther, S.SP.DSOther, 1, S.SP.nextDIDOther, true);
		MC.FadeInLevel(S.SP.seenTitle && !S.inNarration && S.inDialogue == -1);
	}

	public void UPP() {
		var hh = GetComponent<HighlightHints>();
		if (S.SP.npp == 0) hh.NPS[0] = currentColliders.transform.Find("Phone");
		else if (S.SP.npp == 1) hh.NPS[0] = currentColliders.transform.Find("Door");
		else hh.NPS[0] = currentColliders.transform.Find("Jackie");
	}

	public override void PauseAnimations(bool pause) {
		if (!S.SP.invitedJackieIn) A["Stand"].speed = pause ? 0 : 0.02857F;
		else A["Sit"].speed = pause ? 0 : 0.02F;

		if (pause && Sound2.isPlaying) Sound2.Pause();
		else if (!pause && S.SP.knockKnock) Sound2.UnPause();

		//GameObject.Find("EntranceArea").transform.Find("AirConditioner").GetComponent<Animation>()["ACWorking"].speed = pause ? 0 : 1;
		var cc = GameObject.Find("Walls/Window").transform.Find("Car1").GetComponent<CarAnimationControl>();
		cc.anim1["Car1Moving"].speed = pause ? 0 : 1;
		cc.anim2["Car2Moving"].speed = pause ? 0 : 1;
		cc.anim4["Car4Moving"].speed = pause ? 0 : 1;
	}
}

[System.Serializable]
public class SavePrologue {
	public DialogueStructure DSJackie = null;
	public DialogueStructure DSOther = null;
	public int nextDIDJackie = 0;
	public int nextDIDOther = 0;

	public bool blackScreen = true;
	public bool seenTitle = false;
	public bool checkedPhone = false;
	public int heardMessages = 0;
	public bool knockKnock = false;
	public bool invitedJackieIn = false;

	public bool kindOfAnAss = false;
	public bool longTermProject = false;
	public bool preCalculations = false;

	public int npp = 0; //0 - phone, 1 - door, 2 - Jackie
	public int di = 0;
	public bool wallPierced = false;
	public bool noHolesCountReset = false;
	public Vector3[] DartsPositions;
	public Quaternion[] DartsRotations;
	public int blindsState = 0; //0 - open, 1 - closed, 2 - drawn

	public int[] CCID;

	public SavePrologue() {
		CCID = new int[] {1, 20, 40}; //0 - screw, 1 - darts, 2 - books
	}
}

//16:9 = 1.7777777	(1366 x 768, 1920 x 1080, 1280 x 720)
//3:2  = 1.5
//4:3  = 1.3333333	(1024 x 768, 1280 x 960)
//5:4  = 1.25		(1280 x 1024)
//Debug.Log(Screen.width + " x " + Screen.height + " = " + Screen.width * 1.0F / Screen.height);