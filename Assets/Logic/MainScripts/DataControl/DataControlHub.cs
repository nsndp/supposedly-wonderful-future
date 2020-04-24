using UnityEngine;
using System.Collections;
using System.IO;

public class DataControlHub : DataControl {

	public TXT DialogueP1, DialogueP2, DialogueP3, DialogueP4, DialogueP5, DialogueChats, DialogueOther;
	public Transform lounge, room, camL, camR; public Animation JA; public JackieAnimations JAScr;
	public Vector3 pos, posA, posB, JposA, JposB, posElv, posRoom;
	public Quaternion rot, rotA, rotB, JrotA, JrotB, rotElv, rotRoom;
	public AudioClip night1, night2, night3, night4, closure1, closure2, closure3, closure4, intro23, intro5;
	public AudioClip transition1, transition2, elevatorDoors, fruitEating, widgetSet;
	public DemoEnd demoEnd; float ar;
	public Texture SkinPaintedC, SkinPaintedS;

	public override int[] GetCCID() { return S.SH.CCID; }
	public override void VideoResChanged() { }

	void AssignVectors() {
		//pos = new Vector3(Mathf.Lerp(-0.01F, 0.5F, a), 2.4F, Mathf.Lerp(-2.4F, -2.85F, a));//0.47F, 2.44F, -2.3F [nl] rot = Quaternion.Euler(new Vector3(13, 274, 0));
		ar = (Screen.width * 1.0F / Screen.height - 1.7777777777F) / (1.25F - 1.7777777777F);
		pos = Vector3.Lerp(new Vector3(0.31F, 2.46F, -2.42F), new Vector3(1.23F, 2.68F, -2.48F), ar);
		rot = Quaternion.Euler(new Vector3(13, -86, 0));
		GameObject.Find("Lounge").transform.Find("Elevator/Highlight").GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, ar*100);
		posRoom = Vector3.Lerp(new Vector3(0.1F, 2.16F, -3.2F), new Vector3(-0.2F, 2.16F, -4.4F), ar);
		rotRoom = Quaternion.Euler(new Vector3(15, 10.9F, 0));
		posB = new Vector3(-3, 1.23F, -1.76F);
		AssignVectors1();
		JposA = new Vector3(-5, 0.01F, 1.5F); JrotA = Quaternion.Euler(Vector3.zero);
		JposB = new Vector3(-4.56F, 0.01F, -2); JrotB = Quaternion.Euler(new Vector3(0, 90, 0));
		posElv = new Vector3(-2.2F, 1.8F, -2.4F); rotElv = Quaternion.Euler(new Vector3(9, 208, 0));
	}
	void AssignVectors1() { //layout-dependant parts
		if (COMMON.U.textLayout < 2) {
			posA = Vector3.Lerp(new Vector3(-4.4F, 1.86F, -1.3F), new Vector3(-4.2F, 1.7F, -1.4F), ar);
			rotA = Quaternion.Euler(new Vector3(Mathf.Lerp(10, 12, ar), 358, 0));
			rotB = Quaternion.Euler(new Vector3(12.5F, 255, 0));
		} else {
			posA = Vector3.Lerp(new Vector3(-4, 1.8F, -1.3F), new Vector3(-4.2F, 1.7F, -1.3F), ar);
			rotA = Quaternion.Euler(Vector3.Lerp(new Vector3(10, 4, 0), new Vector3(12, 6.5F, 0), ar));
			rotB = Quaternion.Euler(new Vector3(18, 240, 0));
		}
	}

	public override void UISettingsChanged() {
		AssignVectors1();
		if (!S.SH.isNight) {
			camL.localPosition = posA + new Vector3(0, 20, 0); camL.localRotation = rotA;
		}
		else if (S.SH.isNight && S.inDialogue == 0) {
			camL.localPosition = S.levelID < 14 ? posB : posA;
			camL.localRotation = S.levelID < 14 ? rotB : rotA;
		}
	}

	void Start() {
		if (!GetComponent<BinGeneration_LifePlusHQ_1>().enabled) Init();
	}
	public void Init() {
		COMMON.LoadUserSettings(); if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, 6);
		dataFolder = COMMON.dataFolder + "LifePlusHQ/";

		if (COMMON.saveToLoad == null) { //TEST
			Debug.Log("TEST MODE"); var o = new GameObject("Steam", typeof(SteamControl)); S = new SaveGame();
			S.levelID = 11; S.SC1 = new SaveChapter1(); S.SC1.chosePretense = true; S.SP = new SavePrologue();
			//S.levelID = 12; S.SC2 = new SaveChapter2(); S.SC2.realCulpritArrested = true; S.SC2.prisonsTalkKettley = true;
			//S.levelID = 13; S.SC3 = new SaveChapter3(); S.SC2 = new SaveChapter2(); S.SC2.realCulpritArrested = true;
			//S.levelID = 14; S.SC4 = new SaveChapter4(); S.SH = new SaveHub(); S.SC1 = new SaveChapter1(); S.SC2 = new SaveChapter2();
			//S.SC4.theChoice = 1; S.SH.visitedJackie = true; for (int i = 0; i < 8; i++) S.SH.asked[i] = true;
		}
		else {
			S = SaveGame.Load(COMMON.saveFolder + COMMON.saveToLoad);
			if (Application.isEditor) Debug.Log("Game loaded: " + COMMON.saveToLoad);
		}
		if (S.SH == null) S.SH = new SaveHub();
		if (S.SH.DSJP1 == null) S.SH.DSJP1 = DialogueStructure.Load(dataFolder + "JackieProblem1.bin");
		if (S.SH.DSJChats == null) S.SH.DSJChats = DialogueStructure.Load(dataFolder + "JackieChats.bin");
		if (S.SH.DSOther == null) S.SH.DSOther = DialogueStructure.Load(dataFolder + "Other.bin");
		if (!COMMON.demoVersion && S.SH.DSJP2 == null) S.SH.DSJP2 = DialogueStructure.Load(dataFolder + "JackieProblem2.bin");
		//extra check for R == null is because there was this strange bug when DSJP3 was initiated before level 12 but all its arrays were null, and I couldn't find any other reason than some random corruption
		if (S.levelID >= 12 && (S.SH.DSJP3 == null || S.SH.DSJP3.R == null)) S.SH.DSJP3 = DialogueStructure.Load(dataFolder + "JackieProblem3.bin");
		if (S.levelID == 14 && (S.SH.DSJP4 == null || S.SH.DSJP4.R == null)) S.SH.DSJP4 = DialogueStructure.Load(dataFolder + "JackieProblem4.bin");
		if (S.levelID == 14 && (S.SH.DSJP5 == null || S.SH.DSJP5.R == null)) S.SH.DSJP5 = DialogueStructure.Load(dataFolder + "JackieProblem5.bin");

		if (S.levelID == 11) DialogueP1 = TXT.Load(LOC("JackieProblem1"));
		if (!COMMON.demoVersion && S.levelID == 11 || S.levelID == 12) DialogueP2 = TXT.Load(LOC("JackieProblem2"));
		if (S.levelID == 12 || S.levelID == 13) DialogueP3 = TXT.Load(LOC("JackieProblem3"));
		if (S.levelID == 14) {
			DialogueP4 = TXT.Load(LOC("JackieProblem4"));
			DialogueP5 = TXT.Load(LOC("JackieProblem5"));
		}
		DialogueChats = TXT.Load(LOC("JackieChats"));
		DialogueOther = TXT.Load(LOC("Other"));
		Narration = TXT.Load(LOC("Narration"));
		Comments = TXT.Load(LOC("Comments"));
		NS = NarrationStructure.Load(dataFolder + "Narration.bin");

		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		bMenu = UIC.transform.Find("ButtonMenu").gameObject;
		bReturn = UIC.transform.Find("ButtonArrow").gameObject;
		bS = UIC.transform.Find("BlackScreen").GetComponent<UnityEngine.UI.Image>();
		MC.Init(); UIC.Init(); AssignVectors(); SetStates();

		//UIC.GetLongestLine("JackieChats", DialogueChats); UIC.GetLongestLine("Other", DialogueOther); UIC.GetLongestLine("Narration", Narration); UIC.GetLongestLine("Comments", Comments);
		//if (DialogueP1 != null) UIC.GetLongestLine("JackieProblem1", DialogueP1);
		//if (DialogueP2 != null) UIC.GetLongestLine("JackieProblem2", DialogueP2);
		//if (DialogueP3 != null) UIC.GetLongestLine("JackieProblem3", DialogueP3);
		//if (DialogueP4 != null) UIC.GetLongestLine("JackieProblem4", DialogueP4);
		//if (DialogueP5 != null) UIC.GetLongestLine("JackieProblem5", DialogueP5);
	}

	public void SetDayOrNight(bool setNight) {
		camL.localPosition = setNight ? pos : posA + new Vector3(0, 20, 0);
		camL.localRotation = setNight ? rot : rotA;
		camL.GetComponent<PostProcessing>().Multiply = setNight ? 1 : 1.05F;
		camL.GetComponent<PostProcessing>().Contrast = setNight ? 0.12F : 0.1F;
		var r = JA.transform.Find("Jackie_Body").GetComponent<SkinnedMeshRenderer>(); var m = r.materials;
		m[0].mainTexture = setNight ? SkinPaintedS : SkinPaintedC;
		m[2].color = setNight ? new Color(0.863F, 0.725F, 0.604F) : new Color(0.886F, 0.78F, 0.678F); //DCB99A - night; E2C7AD - day
		r.materials = m;
		if (!setNight || S.levelID == 14) {
			JA.transform.localPosition = JposA; JA.transform.localRotation = JrotA;
			JA.transform.Find("Projector").gameObject.SetActive(false);
			JA.transform.Find("Screens").gameObject.SetActive(false);
			JA.Stop(); JA.Play("StandBreathe");
			if (!setNight || S.levelID < 14) {
				JA.Play("Stand1"); JA["Stand1"].speed = 0;
				JA.transform.localPosition += new Vector3(0, 20, 0);
				JA["Stand1"].normalizedTime = S.levelID == 11 && !S.SH.startedCH2 ? 0 : 1;
			} else {
				JA.Play("Stand2"); JA["Stand2"].speed = 0;
				lounge.Find("Lights/Character1").gameObject.SetActive(false);
				lounge.Find("Lights/Character2(D4)").gameObject.SetActive(true);
				JAScr.ChangeColliders();
			}
		} else {
			JA.transform.localPosition = JposB; JA.transform.localRotation = JrotB;
			JA.transform.Find("Projector").gameObject.SetActive(true);
			JA.transform.Find("Screens").gameObject.SetActive(true);
			JA.Stop(); JA.Play("Sit"); JAScr.phase = 10; JAScr.chance = 10;
			//anchors for proper light probing
			JA.transform.Find("Anchor_Body").localPosition = new Vector3(0, 0.6F, -0.3F);
			JA.transform.Find("Anchor_Hair").localPosition = new Vector3(0, 1.1F, -0.6F);
		}
	}
	void SetStates() {
		lounge = GameObject.Find("Lounge").transform; room = GameObject.Find("Room").transform;
		currentColliders = S.SH.currentRoom == 0 ? lounge.Find("Colliders").gameObject : room.Find("Colliders").gameObject;
		camR = room.Find("Camera").transform; camR.gameObject.SetActive(S.SH.currentRoom == 1);
		camL = lounge.Find("Camera").transform; camL.gameObject.SetActive(S.SH.currentRoom == 0);
		camR.localPosition = posRoom; camR.localRotation = rotRoom;
		JA = GameObject.Find("JackieMain").GetComponent<Animation>();
		JAScr = lounge.Find("Colliders/JackieCol").GetComponent<JackieAnimations>(); JAScr.Init();
		SetDayOrNight(S.SH.isNight);
		room.Find("Screens").GetComponent<NewsControl>().Init();
		room.Find("Colliders/AccessPoint").GetComponent<AccessPoint>().Init();
		GetComponent<HighlightHints>().NPS[0] = S.SH.currentRoom == 0 ? lounge.Find("Colliders/Elevator") : room.Find("Colliders/Bed");
		if (S.levelID >= 13) {
			room.Find("Colliders/Picture1").gameObject.SetActive(false);
			room.Find("Colliders/Picture2").gameObject.SetActive(false);
		}
		if (S.levelID == 11 || S.SH.fruitsCounter == 0) room.Find("Colliders/Fruits").gameObject.SetActive(false);
		if (S.levelID >= 12 && S.SH.fruitsCounter < 3) room.Find("Fruits/Banana").gameObject.SetActive(false);
		if (S.levelID >= 12 && S.SH.fruitsCounter < 2) room.Find("Fruits/Apple").gameObject.SetActive(false);
		if (S.levelID >= 12 && S.SH.fruitsCounter < 1) room.Find("Fruits/Orange").gameObject.SetActive(false);
		room.Find("Colliders/Device").GetComponent<OnClick_Device>().Init();

		if (S.levelID == 11 && !S.SH.isNight) {
			if (S.SH.JackieTurnedA == 1) JA["Stand1"].normalizedTime = 0.25F;
			else if (S.SH.JackieTurnedA == 2) JA["Stand1"].normalizedTime = 1;
		}
		if (S.inDialogue == 0 && S.levelID < 14) {
			camL.localPosition = posB; camL.localRotation = rotB;
			JA["SitToTalk"].normalizedTime = 1; JA.Play("SitToTalk");
			JA.Play("BreatheTalk"); JAScr.camPhase = 2;
			var smr = JA.transform.Find("Screens").GetComponent<SkinnedMeshRenderer>();
			smr.SetBlendShapeWeight(1, 100);
			var m = smr.materials;
			var c = m[0].color; c.a = 0; m[0].SetColor("_Color", c);
			c = m[3].GetColor("_TintColor"); c.a = 0; m[3].SetColor("_TintColor", c);
			smr.materials = m;
		}
		else if (S.inDialogue == 0 && S.levelID == 14) {
			camL.localPosition = posA; camL.localRotation = rotA;
			if (S.SH.JackieTurnedB == 1) JA["Stand2"].normalizedTime = 0.25F;
			else if (S.SH.JackieTurnedB == 2) JA["Stand2"].normalizedTime = 1;
			JAScr.camPhase = 2;
		}

		//STARTS LOGIC
		if (S.levelID == 11 && !S.SH.started1) {
			S.SH.blackScreen = true; S.SH.started1 = true;
			if (S.SC1.choseEuthanasia) { NS.Next[10] = 12; S.SH.DSJP1.Locked[47] = false; S.SH.DSJP1.Locked[48] = true; }
			if (S.SP.kindOfAnAss) S.SH.DSJChats.Locked[172] = false;
			if (S.SP.longTermProject) {
				S.SH.DSJChats.R[256] = new int[] {257}; S.SH.DSJChats.R[279] = new int[] {280};
				S.SH.DSJChats.R[270] = new int[] {272}; S.SH.DSJChats.R[271] = new int[] {272};
			}
			UIC.StartNarration(10);
		}
		if (S.levelID == 12 && !S.SH.started2) {
			S.SH.blackScreen = false; S.SH.started2 = true;
			if (S.SC2.prisonsTalkKettley) S.SH.DSJP2.Locked[120] = false;
			if (S.SC2.realCulpritArrested) {
				S.SH.nextDIDJP2 = 67;
				S.SH.DSJP2.Locked[70] = false; S.SH.DSJP2.Locked[71] = false; S.SH.DSJP2.Locked[74] = false; S.SH.DSJP2.Locked[77] = false;
				S.SH.DSJP2.R[133] = new int[] {S.SH.likeISaid ? 138 : 139};
			} else {
				S.SH.nextDIDJP2 = 60;
				S.SH.DSJP2.Locked[73] = false; S.SH.DSJP2.Locked[76] = false;
				S.SH.DSJP2.R[133] = new int[] {S.SH.likeISaid ? 140 : 141};
				if (S.SC2.arrestedBorD) S.SH.DSJP2.R[73] = new int[] {99};
			}
			UIC.StartDialogue(DialogueP2, S.SH.DSJP2, 2, S.SH.nextDIDJP2, true);
			S.SH.visitedLobby = true; //don't use this intro text on day 2, even if haven't seen it on day 1
			S.SH.nextDIDBed = 20;
			if (!S.SH.visitedJackie) { S.SH.nextDIDJChats = 0; S.SH.DSJChats.Locked[4] = false; S.SH.DSJChats.Locked[3] = true; }
			else if (!S.SH.startedQuestions) S.SH.nextDIDJChats = 23;
			else S.SH.nextDIDJChats = 24;
			S.SH.DSJChats.Locked[150] = false; S.SH.DSJChats.Locked[170] = false; S.SH.DSJChats.Locked[244] = false;
		}
		if (S.levelID == 13 && !S.SH.started3) {
			S.SH.blackScreen = false; S.SH.started3 = true;
			if (S.SC3.questionsAsked < 5) S.SH.DSJP3.R[136] = new int[] {145};
			if (S.SC3.talkedAboutCreation) { S.SH.DSJP3.Locked[339] = false; S.SH.DSJP3.Locked[415] = false; }
			if (S.SC3.talkedAboutBeliefs) { S.SH.DSJP3.Locked[341] = false; S.SH.DSJP3.Locked[417] = false; }
			if (S.SH.oneLiner1 && S.SH.oneLiner2) { S.SH.DSJP3.R[660] = new int[] {665}; S.SH.DSJP3.R[641] = new int[] {645}; }
			if (S.SC2.realCulpritArrested) {
				S.SH.DSJP3.R[526] = new int[] {528}; S.SH.DSJP3.R[527] = new int[] {528};
				S.SH.DSJP3.R[545] = new int[] {548}; S.SH.DSJP3.R[546] = new int[] {548}; S.SH.DSJP3.R[547] = new int[] {548};
			}
			S.SH.nextDIDJP3 = 120; UIC.StartDialogue(DialogueP3, S.SH.DSJP3, 3, S.SH.nextDIDJP3, true);
			S.SH.visitedLobby = true; S.SH.nextDIDBed = 40;
			S.SH.nextDIDJChats = S.SH.visitedJackie ? 30 : 32;
			S.SH.DSJChats.Locked[150] = false; S.SH.DSJChats.Locked[170] = false; S.SH.DSJChats.Locked[244] = false; //duplicated for testing purporses
			S.SH.DSJChats.R[7] = new int[] {37};
			S.SH.DSJChats.Locked[350] = false; S.SH.DSJChats.Locked[520] = false;
			S.SH.fruitsCounter = 3; room.Find("Colliders/Fruits").gameObject.SetActive(true);
		}
		if (S.levelID == 14 && !S.SH.started4) {
			S.SH.blackScreen = false; S.SH.started4 = true;
			if (S.SC4.memorySeen[6]) S.SH.DSJP4.Locked[47] = false;
			S.SH.DSJP4.Locked[48 + S.SC4.theChoice] = false;
			if (S.SC4.theChoice != 1) S.SH.DSJP4.R[130] = new int[] {133};
			if (S.SH.asked[6]) { S.SH.DSJP4.R[137] = new int[] {139}; S.SH.DSJP4.R[138] = new int[] {139}; } 
			if (S.SC1.beenForceful && S.SC2.accusedSomebody) S.SH.DSJP4.R[51] = new int[] {78};
			UIC.StartDialogue(DialogueP4, S.SH.DSJP4, 4, S.SH.nextDIDJP4, true);
			S.SH.visitedLobby = true; S.SH.nextDIDBed = 60;
			S.SH.nextDIDJChats = S.SH.visitedJackie ? 40 : 32;
			bool askedAll = true; for (int i = 0; i < 8; i++) if (!S.SH.asked[i]) askedAll = false;
			if (askedAll) { S.SH.DSJChats.R[43] = new int[] {45}; S.SH.DSJChats.R[44] = new int[] {45}; }
			S.SH.DSJChats.Locked[150] = false; S.SH.DSJChats.Locked[170] = false; S.SH.DSJChats.Locked[244] = false; S.SH.DSJChats.Locked[350] = false; S.SH.DSJChats.Locked[520] = false; S.SH.DSJChats.R[7] = new int[] {37}; //duplicated for testing purporses
			S.SH.fruitsCounter = 3; room.Find("Colliders/Fruits").gameObject.SetActive(true);
		}

		BGM = transform.Find("Music").GetComponent<AudioSource>(); BGM.volume = COMMON.U.volM;
		Sound = transform.Find("Sound").GetComponent<AudioSource>(); Sound.volume = COMMON.U.volS;
		//TextSound = transform.Find("Text").GetComponent<AudioSource>(); TextSound.volume = COMMON.U.volS;
		if (S.levelID == 11) {
			if (S.SH.startedCH2) { BGM.clip = intro23; loopAt = 0; }
			else if (!S.SH.isNight) { BGM.clip = closure1; loopAt = 0; }
			else if (!S.SH.firstNightIntro) { BGM.clip = night1; loopAt = 8.161F; }
			else BGM.clip = null;
		}
		else if (S.levelID == 12) {
			if (S.SH.startedCH3) { BGM.clip = intro23; loopAt = 0; }
			else if (!S.SH.isNight) { BGM.clip = closure2; loopAt = 16.518F; }
			else { BGM.clip = night2; loopAt = 19.854F; }
		}
		else if (S.levelID == 13) {
			if (!S.SH.isNight) { BGM.clip = closure3; loopAt = 21.217F; }
			else { BGM.clip = night3; loopAt = 2.813F; }
		}
		else if (S.levelID == 14) {
			if (S.SH.startedCH5) { BGM.clip = intro5; loopAt = 0; }
			else if (!S.SH.isNight) { BGM.clip = closure4; loopAt = 2.615F; }
			else { BGM.clip = night4; loopAt = 0; }
		}
		if (BGM.clip != null) BGM.Play();

		CursorLock(false); bS.gameObject.SetActive(S.SH.blackScreen);
		if (S.inNarration) UIC.StartNarration(S.NID);
		else if (S.inDialogue == 0) UIC.StartDialogue(DialogueChats, S.SH.DSJChats, 0, S.SH.nextDIDJChats, S.levelID == 14);
		else if (S.inDialogue == 1) UIC.StartDialogue(DialogueP1, S.SH.DSJP1, 1, S.SH.nextDIDJP1, true);
		else if (S.inDialogue == 2) UIC.StartDialogue(DialogueP2, S.SH.DSJP2, 2, S.SH.nextDIDJP2, true);
		else if (S.inDialogue == 3) UIC.StartDialogue(DialogueP3, S.SH.DSJP3, 3, S.SH.nextDIDJP3, true);
		else if (S.inDialogue == 4) UIC.StartDialogue(DialogueP4, S.SH.DSJP4, 4, S.SH.nextDIDJP4, true);
		else if (S.inDialogue == 5) UIC.StartDialogue(DialogueP5, S.SH.DSJP5, 5, S.SH.nextDIDJP5, true);
		else if (S.inDialogue == 10) UIC.StartDialogue(DialogueOther, S.SH.DSOther, 10, S.SH.nextDIDBed, true);
		else if (S.inDialogue == 11) UIC.StartDialogue(DialogueOther, S.SH.DSOther, 11, 0, false);
		MC.FadeInLevel(!S.inNarration && S.inDialogue == -1);
	}

	float pauseSPD = 0;
	public override void PauseAnimations(bool pause) {
		if (S.SH.currentRoom == 0) {
			if (!S.SH.isNight) {
				JA["StandBreathe"].speed = pause ? 0 : 0.02F;
				if (pause) { pauseSPD = JA["Stand1"].speed; JA["Stand1"].speed = 0; } else JA["Stand1"].speed = pauseSPD;
			}
			else if (S.levelID == 14) {
				JA["StandBreathe"].speed = pause ? 0 : 0.02F;
				JA["Stand2Back"].speed = pause ? 0 : 1;
				if (pause) { pauseSPD = JA["Stand2"].speed; JA["Stand2"].speed = 0; } else JA["Stand2"].speed = pauseSPD;
			}
			else {
				JA["Sit"].speed = pause ? 0 : 0.02F; JA["BreatheTalk"].speed = pause ? 0 : 0.02F;
				JA["SitIdle1"].speed = pause ? 0 : 1; JA["SitIdle2"].speed = pause ? 0 : 1;
				JA["SitToTalk"].speed = pause ? 0 : 1; JA["SitFromTalk"].speed = pause ? 0 : 1;
			}
		}
	}
}

[System.Serializable]
public class SaveHub {
	public DialogueStructure DSJP1, DSJP2, DSJP3, DSJP4, DSJP5, DSJChats, DSOther;
	public int nextDIDJP1, nextDIDJP2, nextDIDJP3, nextDIDJP4, nextDIDJP5, nextDIDJChats;
	public int nextDIDBed = 10;
	public bool isNight = false;
	public int currentRoom = 0; //0 - lounge, 1 - room

	public bool blackScreen = false;
	public bool started1 = false;
	public bool started2 = false;
	public bool started3 = false;
	public bool started4 = false;
	public bool firstNightIntro = false;
	public bool visitedLobby = false;
	public bool triedBrowsing = false;
	public bool[][] readNews;
	public int newsFontSize = 2;
	public bool newsDarkColor = false;
	public bool widgetIsOn = false;
	public int fruitsCounter = 3;

	public int JackieTurnedA = 0; //for P1 closure
	public int JackieTurnedB = 0; //for D4 night chats
	public bool likeISaid = false;
	public bool creatorIsPicky = false;
	public bool oneLiner1 = false;
	public bool oneLiner2 = false;

	public bool visitedJackie = false;
	public bool startedQuestions = false;
	public bool[] asked = new bool[8]; //1A, 1B, 2A, 2B, 3A, 3B, 1E, 2E
	public bool[] askedPersonal = new bool[5];
	public bool[] askedCorporate = new bool[6];
	public bool letsStare = false;
	public bool finishedInteraction = false;

	//used on Save/Load screen for LevelID=11/12/14 to tell whether the next chapter already started or not
	public bool startedCH2 = false;
	public bool startedCH3 = false;
	public bool startedCH5 = false;

	public int[] CCID;

	public SaveHub() {
		CCID = new int[] {1, 10}; //1 - coffee picture, 2 - salad picture
		readNews = new bool[4][] { new bool[4], new bool[4], new bool[4], new bool[4] };
	}

	public SaveHub(SaveHub SRC) {
		nextDIDJP1 = SRC.nextDIDJP1; nextDIDJP2 = SRC.nextDIDJP2; nextDIDJP3 = SRC.nextDIDJP3;
		nextDIDJP4 = SRC.nextDIDJP4; nextDIDJP5 = SRC.nextDIDJP5; nextDIDJChats = SRC.nextDIDJChats;
		nextDIDBed = SRC.nextDIDBed; isNight = SRC.isNight; currentRoom = SRC.currentRoom;
		blackScreen = SRC.blackScreen; visitedLobby = SRC.visitedLobby; triedBrowsing = SRC.triedBrowsing;
		started1 = SRC.started1; started2 = SRC.started2; started3 = SRC.started3; started4 = SRC.started4;
		readNews = new bool[4][] { new bool[4], new bool[4], new bool[4], new bool[4] };
		for (int i = 0; i < 4; i++) for (int j = 0; j < 4; j++) readNews[i][j] = SRC.readNews[i][j];
		JackieTurnedA = SRC.JackieTurnedA; JackieTurnedB = SRC.JackieTurnedB;
		likeISaid = SRC.likeISaid; creatorIsPicky = SRC.creatorIsPicky;
		oneLiner1 = SRC.oneLiner1; oneLiner2 = SRC.oneLiner2;
		visitedJackie = SRC.visitedJackie; startedQuestions = SRC.startedQuestions;
		for (int i = 0; i < 8; i++) asked[i] = SRC.asked[i];
		for (int i = 0; i < 5; i++) askedPersonal[i] = SRC.askedPersonal[i];
		for (int i = 0; i < 6; i++) askedCorporate[i] = SRC.askedCorporate[i];
		letsStare = SRC.letsStare; finishedInteraction = SRC.finishedInteraction;
		startedCH2 = SRC.startedCH2; startedCH3 = SRC.startedCH3; startedCH5 = SRC.startedCH5;
		CCID = new int[] {SRC.CCID[0], SRC.CCID[1]};
		if (SRC.DSJP1 == null) DSJP1 = null; else DSJP1 = new DialogueStructure(SRC.DSJP1);
		if (SRC.DSJP2 == null) DSJP2 = null; else DSJP2 = new DialogueStructure(SRC.DSJP2);
		if (SRC.DSJP3 == null) DSJP3 = null; else DSJP3 = new DialogueStructure(SRC.DSJP3);
		if (SRC.DSJP4 == null) DSJP4 = null; else DSJP4 = new DialogueStructure(SRC.DSJP4);
		if (SRC.DSJP5 == null) DSJP5 = null; else DSJP5 = new DialogueStructure(SRC.DSJP5);
		if (SRC.DSJChats == null) DSJChats = null; else DSJChats = new DialogueStructure(SRC.DSJChats);
		if (SRC.DSOther == null) DSOther = null; else DSOther = new DialogueStructure(SRC.DSOther);
	}
}
