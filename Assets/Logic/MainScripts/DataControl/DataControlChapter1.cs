using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class DataControlChapter1 : DataControl {

	public TXT DialogueMitty, DialogueWoman, DialogueOther;

	public GameObject[] cam = new GameObject[4];
	public Transform[] room = new Transform[4];
	public GameObject ReturnLeft, ReturnRight; bool rActive = false;
	public Vector3[] camPos; public Quaternion[] camRot; int roomChangersWidth;
	public OnClick_Mitty MittySCR;
	public OnClick_Woman WomanSCR; 
	public AudioClip main, prereveal, reveal, explanation, finale, euthanasia, pretense;
	public AudioClip doorOpen, doorClose, footsteps, lightSwitch, pillPickup, eLock;

	public override int[] GetCCID() { return S.SC1.CCID; }
	public override void UISettingsChanged() { }
	public override void VideoResChanged() { }

	void AssignVectors() {
		var a = (Screen.width * 1.0F / Screen.height - 1.7777777777F) / (1.25F - 1.7777777777F);
		camPos = new Vector3[5]; camRot = new Quaternion[5];
		camPos[0] = Vector3.Lerp(new Vector3(-0.388F, 1.9F, -4.157F), new Vector3(-0.35F, 1.85F, -4.8F), a);
		camRot[0] = Quaternion.Euler(new Vector3(10.944F, 3.302F, 0));
		camPos[1] = Vector3.Lerp(new Vector3(0.36F, 1.5F, -1), new Vector3(0.45F, 1.628F, -1.47F), a);
		camRot[1] = Quaternion.Euler(new Vector3(14.947F, -9.746F, 0));
		camPos[2] = Vector3.Lerp(new Vector3(-1.101F, 1, -1.848F), new Vector3(-1.36F, 1.02F, -2.18F), a);
		camRot[2] = Quaternion.Euler(new Vector3(4.958F, Mathf.Lerp(24.83F, 27.5F, a), 0));
		camPos[3] = Vector3.Lerp(new Vector3(0.215F, 1.8F, -1.972F), new Vector3(0.4F, 1.85F, -2.35F), a);
		camRot[3] = Quaternion.Euler(new Vector3(12.444F, -22.774F, 0));
		//this one is another angle for bedroom when in pretense
		camPos[4] = Vector3.Lerp(new Vector3(0.86F, 1.37F, -0.76F), new Vector3(0.9F, 1.625F, -1.3F), a);
		camRot[4] = Quaternion.Euler(new Vector3(24, -21, 0));
	}

	void Start () {
		if (!GetComponent<BinGeneration_DimlyLitHouse>().enabled) Init();
	}
	public void Init() {
		COMMON.LoadUserSettings(); if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, 1);
		dataFolder = COMMON.dataFolder + "DimlyLitHouse/";
		DialogueMitty = TXT.Load(LOC("Mitty"));
		DialogueWoman = TXT.Load(LOC("Woman"));
		DialogueOther = TXT.Load(LOC("Other"));
		Narration = TXT.Load(LOC("Narration"));
		Comments = TXT.Load(LOC("Comments"));
		NS = NarrationStructure.Load(dataFolder + "Narration.bin");

		if (COMMON.saveToLoad == null) {
			Debug.Log("TEST MODE");	new GameObject("Steam", typeof(SteamControl));
			S = new SaveGame(); S.levelID = 1; S.SP = new SavePrologue();
		}
		else {
			S = SaveGame.Load(COMMON.saveFolder + COMMON.saveToLoad);
			if (Application.isEditor) Debug.Log("Game loaded: " + COMMON.saveToLoad);
		}
		if (S.SC1 == null) {
			S.SC1 = new SaveChapter1();
			S.SC1.DSMitty = DialogueStructure.Load(dataFolder + "Mitty.bin");
			S.SC1.DSWoman = DialogueStructure.Load(dataFolder + "Woman.bin");
			S.SC1.DSOther = DialogueStructure.Load(dataFolder + "Other.bin");
		}

		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		bMenu = UIC.transform.Find("ButtonMenu").gameObject;
		bReturn = UIC.transform.Find("ButtonArrow").gameObject;
		bS = UIC.transform.Find("BlackScreen").GetComponent<UnityEngine.UI.Image>();
		MC.Init(); UIC.Init(); SetStates();

		//UIC.GetLongestLine("Mitty", DialogueMitty); UIC.GetLongestLine("Woman", DialogueWoman); UIC.GetLongestLine("Other", DialogueOther); UIC.GetLongestLine("Narration", Narration); UIC.GetLongestLine("Comments", Comments);
	}

	void SetStates() {
		//1. Room change-related stuff
		AssignVectors(); roomChangersWidth = Mathf.RoundToInt(100f * Screen.width / 1280.0F);
		room[0] = GameObject.Find("Livingroom").transform; room[1] = GameObject.Find("Bedroom").transform;
		room[2] = GameObject.Find("Kitchen").transform; room[3] = GameObject.Find("Entryway").transform;
		for (int i = 0; i < 4; i++) {
			cam[i] = room[i].Find("Camera").gameObject;
			cam[i].SetActive(S.SC1.currentRoom == i);
			cam[i].GetComponent<Camera>().enabled = S.SC1.currentRoom == i;
			cam[i].GetComponent<AudioListener>().enabled = S.SC1.currentRoom == i;
			cam[i].transform.localPosition = camPos[i];
			cam[i].transform.localRotation = camRot[i];
		}
		ReturnLeft = UIC.transform.Find("ReturnLeft").gameObject;
		ReturnRight = UIC.transform.Find("ReturnRight").gameObject;
		currentColliders = room[S.SC1.currentRoom].Find("Colliders").gameObject;

		//2. Plot points
		if (!S.SC1.metWoman) S.SC1.npp = 0;
		else if (!S.SC1.gotFirstGift) S.SC1.npp = 1; else if (!S.SC1.gaveFirstGift) S.SC1.npp = 0;
		else if (!S.SC1.gotSecondGift) S.SC1.npp = 1; else if (!S.SC1.gaveSecondGift) S.SC1.npp = 0;
		else if (!S.SC1.sawMittyProperly) S.SC1.npp = 1; else if (!S.SC1.gotExplanation) S.SC1.npp = 0;
		else if (!S.SC1.foundPill) S.SC1.npp = 2;
		else if (!S.SC1.chosePretense && !S.SC1.choseEuthanasia) S.SC1.npp = 0;
		else if (S.SC1.chosePretense && !S.SC1.storyFinished) S.SC1.npp = 1;
		else if (S.SC1.choseEuthanasia && !S.SC1.pickedPill) S.SC1.npp = 2;
		else if (S.SC1.choseEuthanasia && !S.SC1.didEuthanasia) S.SC1.npp = 1;
		else if (!S.SC1.storyFinished) S.SC1.npp = 0; else S.SC1.npp = 3;
		UPP();

		//3. Other
		if (S.SP.preCalculations) NS.Next[1] = 3;
		if (S.SC1.pickedPill) {
			GetComponent<TableZoom>().Booklet.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 0);
			GetComponent<TableZoom>().Booklet.parent.Find("Box").gameObject.SetActive(false);
			GetComponent<TableZoom>().Booklet.parent.Find("Pill").gameObject.SetActive(false);
		}
		if (S.SC1.inPretense) {
			cam[1].transform.localPosition = camPos[4];
			cam[1].transform.localRotation = camRot[4];
		}
		if (S.SC1.storyFinished) {
			var scr = room[3].Find("Colliders/Shoes").GetComponent<CommentOnClick>();
			scr.StartingCommentID = 82; scr.EndingCommentID = 82;
			scr = room[3].Find("Colliders/Photo1").GetComponent<CommentOnClick>();
			scr.StartingCommentID = 83; scr.EndingCommentID = 83;
			scr = room[3].Find("Colliders/Photo3").GetComponent<CommentOnClick>();
			scr.StartingCommentID = 84; scr.EndingCommentID = 84;
			//opening the door
			var mesh = room[3].Find("FrontDoor/Lock").GetComponent<MeshFilter>().mesh;
			float shift = 0.5002F; var uvs = new Vector2[4];
			for (int i = 0; i < 4; i++) uvs[i] = new Vector2(mesh.uv[i].x + shift, mesh.uv[i].y);
			mesh.uv = uvs;
			S.SC1.nextDIDFrontDoor = 21;
		}

		//4. Colliders state based on progress
		GameObject o; CommentOnClick scrc;
		o = room[3].Find("Colliders/PhotoCollage").gameObject; scrc = o.GetComponent<CommentOnClick>();
		o.SetActive(S.SC1.metMitty && !S.SC1.gaveSecondGift && S.SC1.CCID[scrc.ObjectIndex] <= scrc.EndingCommentID);
		o = room[3].Find("Colliders/Photo1").gameObject; scrc = o.GetComponent<CommentOnClick>();
		o.SetActive((S.SC1.checkedPhotoCollage && S.SC1.askedTimeIsStill || S.SC1.storyFinished) && S.SC1.CCID[scrc.ObjectIndex] <= scrc.EndingCommentID);
		o = room[3].Find("Colliders/Photo2").gameObject; scrc = o.GetComponent<CommentOnClick>();
		o.SetActive(S.SC1.checkedPhotoCollage && !S.SC1.gaveSecondGift && S.SC1.CCID[scrc.ObjectIndex] <= scrc.EndingCommentID);
		o = room[3].Find("Colliders/Photo3").gameObject; scrc = o.GetComponent<CommentOnClick>();
		o.SetActive((S.SC1.checkedPhotoCollage || S.SC1.storyFinished) && S.SC1.CCID[scrc.ObjectIndex] <= scrc.EndingCommentID);
		o = room[3].Find("Colliders/Umbrella").gameObject; scrc = o.GetComponent<CommentOnClick>();
		o.SetActive(!S.SC1.gaveSecondGift && S.SC1.CCID[scrc.ObjectIndex] <= scrc.EndingCommentID);
		o = room[3].Find("Colliders/Shoes").gameObject; scrc = o.GetComponent<CommentOnClick>();
		o.SetActive((!S.SC1.gaveSecondGift || S.SC1.storyFinished) && S.SC1.CCID[scrc.ObjectIndex] <= scrc.EndingCommentID);
		o = room[0].Find("Colliders/Pinboard").gameObject;
		o.SetActive(S.SC1.metMitty && !S.SC1.gaveSecondGift || S.SC1.storyFinished && !S.SC1.postCheckedPinboard);
		o = room[0].Find("Colliders/Table").gameObject;
		o.SetActive(!S.SC1.storyFinished || !S.SC1.postCheckedTable);
		o = room[0].Find("Colliders/TV").gameObject;
		if (!S.SC1.tvIsOn) o.SetActive(false); else { o.SetActive(true); o.GetComponent<OnClick_TV>().TurnOn(true); }
		//this is, like, invariable, and is here (and not in editor) only because I'm not sure it's possible to drag a enum value there
		room[3].Find("Colliders/PhotoCollage").GetComponent<CommentOnClick>().triggersEvent = (int)events1.checkedPhotoCollage;

		//5. Poses
		MittySCR = room[1].Find("Colliders/Mitty").GetComponent<OnClick_Mitty>();
		WomanSCR = room[2].Find("Colliders/Woman").GetComponent<OnClick_Woman>();
		MittySCR.Init();
		if (S.SC1.choseEuthanasia && !S.SC1.gavePill) WomanSCR.Switch1I();
		else if (S.SC1.gavePill) MittySCR.Switch1();
		else if (S.SC1.womanCollapsed) { WomanSCR.Switch3(); MittySCR.Switch3(); }
		else if (S.SC1.sawEachOther) { WomanSCR.Switch2(); MittySCR.Switch2(); }
		else if (S.SC1.womanGotUp) WomanSCR.Switch2();
		else if (S.inDialogue == 0) MittySCR.LoadDuringDialogue();

		if (S.SC1.sawMittyProperly && !S.SC1.gavePill) LightsOnWorkaround(true);

		//6. Music and sound effects
		BGM = transform.Find("Music").GetComponent<AudioSource>(); BGM.volume = COMMON.U.volM;
		Sound = transform.Find("Sound").GetComponent<AudioSource>(); Sound.volume = COMMON.U.volS;
		Sound2 = transform.Find("Sound2").GetComponent<AudioSource>(); Sound2.volume = COMMON.U.volS;
		if (S.SC1.inPretense) { BGM.clip = pretense; loopAt = 7.16F; }
		else if (S.SC1.didEuthanasia) { BGM.clip = euthanasia; loopAt = 0; }
		else if (S.SC1.womanCollapsed) BGM.clip = null;
		else if (S.SC1.gavePill) BGM.clip = null;
		else if (S.SC1.choicePNR) { BGM.clip = finale; loopAt = 0; }
		else if (S.SC1.gotExplanation) { BGM.clip = explanation; loopAt = 7.376F; }
		else if (S.SC1.sawMittyProperly) { BGM.clip = reveal; loopAt = 0; }
		else if (S.SC1.gaveSecondGift) { BGM.clip = prereveal; loopAt = 0.977F; }
		else if (S.SC1.seenIntro) { BGM.clip = main; loopAt = 31.065F; }
		if (BGM.clip != null) BGM.Play();

		CursorLock(false); bS.gameObject.SetActive(S.SC1.blackScreen);
		if (!S.SC1.seenTitle) UIC.showTitle(1, (int)events1.seenTitle);
		else if (S.inNarration) UIC.StartNarration(S.NID);
		else if (S.inDialogue == 0) UIC.StartDialogue(DialogueMitty, S.SC1.DSMitty, 0, S.SC1.nextDIDMitty, true);
		else if (S.inDialogue == 1) UIC.StartDialogue(DialogueWoman, S.SC1.DSWoman, 1, S.SC1.nextDIDWoman, false);
		else if (S.inDialogue == 2) UIC.StartDialogue(DialogueOther, S.SC1.DSOther, 2, S.SC1.nextDIDFrontDoor, true);
		MC.FadeInLevel(S.SC1.seenTitle && !S.inNarration && S.inDialogue == -1);
	}

	public void UPP() {
		var hh = GetComponent<HighlightHints>();
		if (S.SC1.currentRoom == 0) {
			if (S.SC1.npp == 0) hh.NPS[0] = room[0].Find("Colliders/DoorToKitchen");
			else if (S.SC1.npp == 1) hh.NPS[0] = room[0].Find("Colliders/DoorToBedroom");
			else if (S.SC1.npp == 2) hh.NPS[0] = room[0].Find("Colliders/Table");
			else hh.NPS[0] = room[0].Find("Colliders/DoorToEntryway");
		}
		else if (S.SC1.currentRoom == 1) {
			if (S.SC1.npp == 1) hh.NPS[0] = room[1].Find("Colliders/Mitty"); else hh.NPS[0] = ReturnRight.transform;
		}
		else if (S.SC1.currentRoom == 2) {
			if (S.SC1.npp == 0) hh.NPS[0] = room[2].Find("Colliders/Woman"); else hh.NPS[0] = ReturnLeft.transform;
		} else {
			if (S.SC1.npp == 3) hh.NPS[0] = room[3].Find("Colliders/FrontDoor"); else hh.NPS[0] = ReturnRight.transform;
		}
	}

	public override void PauseAnimations(bool pause) {
		MittySCR.A["Breathe"].speed = pause ? 0 : 0.02F;
		MittySCR.A["Idle"].speed = pause ? 0 : 1; MittySCR.HL["M"].speed = pause ? 0 : 1;
		MittySCR.A["Turn"].speed = pause ? 0 : 1; MittySCR.A["TurnMommy"].speed = pause ? 0 : 1;
		MittySCR.A["PatMommy"].speed = pause ? 0 : 1;
		WomanSCR.A["Breathe"].speed = pause ? 0 : 0.02857F; WomanSCR.A["CryBreathe"].speed = pause ? 0 : 0.02857F;
		WomanSCR.A["Stand"].speed = pause ? 0 : 0.02F; WomanSCR.A["Kneel"].speed = pause ? 0 : 0.02F;
		WomanSCR.A["CryPose"].speed = pause ? 0 : 1;
		if (pause && Sound2.isPlaying) Sound2.Pause();
		else if (!pause && S.SC1.currentRoom == 0 && S.SC1.tvIsOn) Sound2.UnPause();
	}

	public void LightsOnWorkaround(bool on) {
		var v = new Vector3(0, on ? 20 : -20, 0);
		cam[1].GetComponent<Transform>().localPosition += v;
		MittySCR.A.GetComponent<Transform>().localPosition += v;
		MittySCR.HL.GetComponent<Transform>().localPosition += v;
		room[1].Find("Colliders").GetComponent<Transform>().localPosition += v;
		cam[1].GetComponent<PostProcessing>().Contrast = on ? 0.25F : 0.2F;
	}
	/*public void SetLM(int ind) {
		var lm = new LightmapData[LMBase.Length];
		for (int i = 0; i < LMBase.Length; i++) {
			lm[i] = new LightmapData();
			lm[i].lightmapLight = ind == 1 ? LMBase[i] : (ind == 2 ? LMLight[i] : (ind == 3 ? LMGotUp[i] : LMPretense[i]));
			lm[i].lightmapDir = ind == 1 ? LMBaseD[i] : (ind == 2 ? LMLightD[i] : (ind == 3 ? LMGotUpD[i] : LMPretenseD[i]));
		}
		LightmapSettings.lightmaps = lm;
	}*/

	void Update() {
		if (bMenu.activeSelf && S.inDialogue == -1 && !S.inNarration && UIC.bPhase == 0) {
			if (S.SC1.currentRoom == 2 && !rActive && Input.mousePosition.x <= roomChangersWidth) { rActive = true; ReturnLeft.SetActive(true); }
			else if (S.SC1.currentRoom == 2 && rActive && Input.mousePosition.x > roomChangersWidth) { rActive = false; ReturnLeft.SetActive(false); }
			else if ((S.SC1.currentRoom == 1 || S.SC1.currentRoom == 3) && !rActive && Input.mousePosition.x >= Screen.width - roomChangersWidth) { rActive = true; ReturnRight.SetActive(true); }
			else if ((S.SC1.currentRoom == 1 || S.SC1.currentRoom == 3) && rActive && Input.mousePosition.x < Screen.width - roomChangersWidth) { rActive = false; ReturnRight.SetActive(false); }
			else if (S.SC1.currentRoom == 3 && rActive && activeHL != null) { activeHL.SetActive(false); activeHL = null; } //used for 5:4 aspect ratio because overlaps with photo collage colliders
			else if (Input.GetMouseButtonDown(0) && rActive) {
				rActive = false; ReturnLeft.SetActive(false); ReturnRight.SetActive(false);
				GetComponent<RoomChange>().DoIt(0);
			}
		}
	}
}

[System.Serializable]
public class SaveChapter1 {
	public DialogueStructure DSMitty = null;
	public DialogueStructure DSWoman = null;
	public DialogueStructure DSOther = null;
	public int nextDIDMitty = 0;
	public int nextDIDWoman = 0;
	public int nextDIDFrontDoor = 0;
	public bool nothingToTalkMitty = false;
	public bool nothingToTalkWoman = false;

	public int currentRoom = 0; //0 - living room, 1 - bedroom, 2 - kitchen, 3 - entryway
	public int npp; //next plot point: 0 - woman, 1 - Mitty, 2 - table, 3 - front door

	public bool blackScreen = true;
	public bool seenTitle = false;
	public bool seenIntro = false;
	public bool visitedEntryway = false;
	public bool metWoman = false;
	public bool metMitty = false;
	public bool checkedPinboard = false;
	public bool checkedFrontDoor = false;
	public bool triedAllFrontDoor = false;
	public bool askedAboutDoor = false;
	public bool askedTimeIsStill = false;
	public bool checkedPhotoCollage = false;
	public bool tvIsOn = false;
	public bool gotFirstGift = false;
	public bool gotSecondGift = false;
	public bool gaveFirstGift = false;
	public bool gaveSecondGift = false;
	public bool sawMittyProperly = false;
	public bool gotExplanation = false;
	public bool foundPill = false;
	public bool beenForceful = false;
	public bool choicePNR = false;
	public bool chosePretense = false;
	public bool womanGotUp = false;
	public bool sawEachOther = false;
	public bool womanCollapsed = false;
	public bool inPretense = false;
	public bool choseEuthanasia = false;
	public bool pickedPill = false;
	public bool gavePill = false;
	public bool didEuthanasia = false;
	public bool storyFinished = false;
	public bool postCheckedPinboard = false;
	public bool postCheckedTable = false;

	public int[] CCID;

	public SaveChapter1() {
		//0 - pinboard, 1 - windows drawing, 2 - owl drawing, 3 - butterfly drawing
		//4 - umbrella, 5 - shoes, 6 - photo collage, 7 - teddy photo, 8 - watch photo,
		//9 - stones photo, 10 - TV, 11 - pill
		CCID = new int[] {1, 10, 20, 30, 50, 60, 70, 73, 74, 75, 90, 100};
	}
}