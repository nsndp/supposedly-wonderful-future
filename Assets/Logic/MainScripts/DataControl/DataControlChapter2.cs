using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;

public class DataControlChapter2 : DataControl {

	public TXT DialogueKettley, DialogueShawn, DialogueAshley, DialogueScreens, DialogueOther;
	public TXT DialogueSuspectA, DialogueSuspectB, DialogueSuspectC, DialogueSuspectD, DialogueSuspectE, DialogueSuspectF;
	public InvestigationDataCBIU DataScreens;
	public Transform ppKettley, ppShawn, ppAshley, ppScreens, ppArchwayL, ppArchwayB;
	public AudioClip intro, briefed, phaseTwo, fakes, post, logs, ash, sA, sB, sC, sD, sE, sF;
	public int introT, ashT, briefedT, logsT, sAT, sBT, sCT, sDT, sET, fakesT;
	public bool noZoomOut = false; //used for Shawn and Kettley to stop zoom out for black screen flicks
	public Transform camL, camB;
	public Vector3 PosA, PosB, PosShawn, PosShawnAlt, PosKet, PosAsh, PosS0, PosS16, PosS16Classic, PosSAlt, PosSShift;
	public Quaternion RotA, RotB, RotShawn, RotShawnAlt, RotKet, RotKetAlt, RotAsh, RotS;
	[System.NonSerialized] public string[] pageText = new string[] { "PAGE", "OF" };

	public override int[] GetCCID() { return S.SC2.CCID; }
	public override void VideoResChanged() { }

	void AssignVectors() {
		var a = (Screen.width * 1.0F / Screen.height - 1.7777777777F) / (1.25F - 1.7777777777F);
		PosA = new Vector3(Mathf.Lerp(-2.51F, -2.8F, a), Mathf.Lerp(1.76F, 1.87F, a), Mathf.Lerp(-2.17F, -2.3F, a));
		RotA = Quaternion.Euler(new Vector3(14.947F, Mathf.Lerp(53.76F, 48, a), 0));
		PosB = new Vector3(Mathf.Lerp(3.43F, 3.5F, a), Mathf.Lerp(1.885F, 1.8F, a), Mathf.Lerp(3.67F, 4.2F, a));
		RotB = Quaternion.Euler(new Vector3(Mathf.Lerp(12.2F, 11, a), Mathf.Lerp(163, 169, a), 0));
		PosShawn = new Vector3(Mathf.Lerp(-0.5F, -0.75F, a), 1.79F, Mathf.Lerp(-1.3F, -1.45F, a));
		RotShawn = Quaternion.Euler(new Vector3(16, 60, 0));
		PosShawnAlt = new Vector3(-0.5F, 1.65F, -1.3F);
		RotShawnAlt = Quaternion.Euler(new Vector3(18, 75, 0));
		PosKet = new Vector3(-1.67F, 1.8F, 0.6F);
		RotKet = Quaternion.Euler(new Vector3(22, 30, 0));
		RotKetAlt = Quaternion.Euler(new Vector3(27, Mathf.Lerp(35, 30, a), 0));
		PosAsh = new Vector3(3.1F, 1.15F, 2.3F);
		RotAsh = Quaternion.Euler(new Vector3(9, 144, 0));
		PosS0 = new Vector3(5.3F, 1.558F, -1.12F);
		PosS16 = new Vector3(5.3F, 1.6F, -1.07F);
		PosS16Classic = new Vector3(5.3F, 1.54F, -1.07F);
		PosSAlt = new Vector3(5.42F, 1.6F, Mathf.Lerp(-1.15F, -1.05F, a));
		PosSShift = new Vector3(0, -0.4F, 0.2F);
		RotS = Quaternion.Euler(new Vector3(0, 180, 0));
	}

	void Start() {
		if (!GetComponent<BinGeneration_CbIU_1>().enabled) Init();
	}
	public void Init() {
		COMMON.LoadUserSettings(); if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, 2);
		dataFolder = COMMON.dataFolder + "CbIU/";
		DialogueKettley = TXT.Load(LOC("Kettley"));
		DialogueShawn = TXT.Load(LOC("Shawn"));
		DialogueAshley = TXT.Load(LOC("Ashley"));
		DialogueScreens = TXT.Load(LOC("Screens"));
		DialogueOther = TXT.Load(LOC("Other"));
		DialogueSuspectA = TXT.Load(LOC("SuspectA"));
		DialogueSuspectB = TXT.Load(LOC("SuspectB"));
		DialogueSuspectC = TXT.Load(LOC("SuspectC"));
		DialogueSuspectD = TXT.Load(LOC("SuspectD"));
		DialogueSuspectE = TXT.Load(LOC("SuspectE"));
		DataScreens = InvestigationDataCBIU.Load(LOC("ScreensData"));
		Comments = TXT.Load(LOC("Comments"));
		Narration = TXT.Load(LOC("Narration"));
		NS = NarrationStructure.Load(dataFolder + "Narration.bin");

		if (COMMON.saveToLoad == null) {
			Debug.Log("TEST MODE"); new GameObject("Steam", typeof(SteamControl));
			S = new SaveGame(); S.levelID = 2;
		}
		else {
			S = SaveGame.Load(COMMON.saveFolder + COMMON.saveToLoad);
			if (Application.isEditor) Debug.Log("Game loaded: " + COMMON.saveToLoad);
		}
		if (S.SC2 == null) {
			S.SC2 = new SaveChapter2();
			S.SC2.DSKettley = DialogueStructure.Load(dataFolder + "Kettley.bin");
			S.SC2.DSShawn = DialogueStructure.Load(dataFolder + "Shawn.bin");
			S.SC2.DSAshley = DialogueStructure.Load(dataFolder + "Ashley.bin");
			S.SC2.DSScreens = DialogueStructure.Load(dataFolder + "Screens.bin");
			S.SC2.DSOther = DialogueStructure.Load(dataFolder + "Other.bin");
			S.SC2.DSA = DialogueStructure.Load(dataFolder + "SuspectA.bin");
			S.SC2.DSB = DialogueStructure.Load(dataFolder + "SuspectB.bin");
			S.SC2.DSC = DialogueStructure.Load(dataFolder + "SuspectC.bin");
			S.SC2.DSD = DialogueStructure.Load(dataFolder + "SuspectD.bin");
			S.SC2.DSE = DialogueStructure.Load(dataFolder + "SuspectE.bin");
		}

		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		bMenu = UIC.transform.Find("ButtonMenu").gameObject;
		bReturn = UIC.transform.Find("ButtonArrow").gameObject;
		bS = UIC.transform.Find("BlackScreen").GetComponent<UnityEngine.UI.Image>();
		MC.Init(); UIC.Init(); AssignVectors(); SetStates();

		//UIC.GetLongestLine("Kettley", DialogueKettley); UIC.GetLongestLine("Shawn", DialogueShawn); UIC.GetLongestLine("Ashley", DialogueAshley); UIC.GetLongestLine("Screens", DialogueScreens); UIC.GetLongestLine("Other", DialogueOther);
		//UIC.GetLongestLine("SuspectA", DialogueSuspectA); UIC.GetLongestLine("SuspectB", DialogueSuspectB); UIC.GetLongestLine("SuspectC", DialogueSuspectC); UIC.GetLongestLine("SuspectD", DialogueSuspectD); UIC.GetLongestLine("SuspectE", DialogueSuspectE);
		//UIC.GetLongestLine("Comments", Comments); UIC.GetLongestLine("Narration", Narration);
	}

	void SetStates() {
		camL = GameObject.Find("AlwaysRender").transform.Find("CameraA");
		camB = GameObject.Find("AlwaysRender").transform.Find("CameraB");
		var colL = GameObject.Find("CollidersL").transform;
		var colB = GameObject.Find("CollidersB").transform;

		bool r = S.SC2.currentRoom == 0; currentColliders = r ? colL.gameObject : colB.gameObject;
		GameObject.Find("LivingRoomRender").SetActive(r); GameObject.Find("BedroomRender").SetActive(!r);
		if (!r) foreach (Collider cd in colL.GetComponentsInChildren<Collider>()) cd.enabled = false;
		else foreach (Collider cd in colB.GetComponentsInChildren<Collider>()) cd.enabled = false;
		camL.gameObject.SetActive(r); camB.gameObject.SetActive(!r);
		camL.localPosition = PosA; camL.localRotation = RotA;
		camB.localPosition = PosB; camB.localRotation = RotB;
		if (S.SC2.screenZoom >= 0) {
			camB.localPosition = COMMON.U.textLayout == 2 ? PosSAlt : (S.SC2.screenZoom == 0 ? PosS0 : (COMMON.U.textLayout == 1 ? PosS16 : PosS16Classic));
			if (S.SC2.deskUnfolded) camB.localPosition += PosSShift; camB.localRotation = RotS;
		}
		else if (S.inDialogue == 2) { camB.localPosition = PosAsh; camB.localRotation = RotAsh; }
		else if (S.inDialogue == 0) { camL.localPosition = PosKet; camL.localRotation = COMMON.U.textLayout < 2 ? RotKet : RotKetAlt; }
		else if (S.inDialogue == 1 || S.SC2.ShawnSearch) {
			camL.localPosition = COMMON.U.textLayout < 2 ? PosShawn : PosShawnAlt;
			camL.localRotation = COMMON.U.textLayout < 2 ? RotShawn : RotShawnAlt;
		}

		//objects
		if (S.SC2.currentRoom == 0 && (!S.SC2.metKettley || !S.SC2.metShawn)) colL.transform.Find("Archway").gameObject.SetActive(false);
		if (!S.SC2.metAshley || S.SC2.discussedCloset || S.SC2.theyDontExist) colB.transform.Find("Closet").gameObject.SetActive(false); else colB.transform.Find("Closet").gameObject.SetActive(true);
		if (!S.SC2.metAshley || S.SC2.discussedTree || S.SC2.theyDontExist) colB.transform.Find("Tree").gameObject.SetActive(false); else colB.transform.Find("Tree").gameObject.SetActive(true);
		if (!S.SC2.metAshley || S.SC2.discussedGuitar || S.SC2.theyDontExist) colB.transform.Find("Guitar").gameObject.SetActive(false); else colB.transform.Find("Guitar").gameObject.SetActive(true);
		colB.Find("DeskF").gameObject.SetActive(!S.SC2.deskUnfolded);
		colB.Find("DeskU").gameObject.SetActive(S.SC2.deskUnfolded);
		colB.Find("DeskF").GetComponent<OnClick_Desk>().Init();
		colB.Find("DeskU").GetComponent<OnClick_Desk>().Init();
		if (S.SC2.deskUnfolded) colB.Find("DeskU").GetComponent<OnClick_Desk>().StartWithU();
		colB.Find("Screens").GetComponent<OnClick_Screens>().Init();
		if (S.SC2.screenZoom >= 0 && COMMON.U.textLayout == 0) UIC.ClassicShiftCh2SCR(true, S.SC2.screenZoom == 0);

		//plot points
		ppKettley = colL.Find("Kettley"); ppShawn = colL.Find("Shawn");
		ppAshley = colB.Find("Ashley"); ppScreens = colB.Find("Screens");
		ppArchwayL = colL.Find("Archway"); ppArchwayB = colB.Find("Archway");
		if (!S.SC2.metKettley) S.SC2.npp = 0; else if (!S.SC2.metShawn) S.SC2.npp = 1;
		else if (!S.SC2.metAshley) S.SC2.npp = 2; else if (!S.SC2.briefed) S.SC2.npp = 1;
		else if (!S.SC2.learnedAllOnScreens) S.SC2.npp = 3; else if (!S.SC2.learnedAshleyView) S.SC2.npp = 2;
		else if (!S.SC2.phaseTwo) S.SC2.npp = 1; else if (!S.SC2.spottedStrangeThing) S.SC2.npp = 3;
		else if (!S.SC2.theyDontExist) S.SC2.npp = 1; else if (!S.SC2.toldTheGoodNews) S.SC2.npp = 2;
		else if (!S.SC2.foundF) S.SC2.npp = 1; else if (!S.SC2.metF) S.SC2.npp = 3;
		else if (!S.SC2.trackedF) S.SC2.npp = 1; else if (!S.SC2.realCulpritArrested) S.SC2.npp = 0;
		else S.SC2.npp = 1;
		UPP();

		if (S.SC2.ShawnSearch) noZoomOut = true;
		ppKettley.GetComponent<OnClick_Kettley>().Init(); if (S.inDialogue == 0) ppKettley.GetComponent<OnClick_Kettley>().LoadDuringDialogue();
		ppShawn.GetComponent<OnClick_Shawn>().Init(); if (S.inDialogue == 1 || S.SC2.ShawnSearch) ppShawn.GetComponent<OnClick_Shawn>().LoadDuringDialogue();
		ppAshley.GetComponent<OnClick_Ashley>().Init(); if (S.inDialogue == 2) ppAshley.GetComponent<OnClick_Ashley>().LoadDuringDialogue();

		BGM = transform.Find("Music").GetComponent<AudioSource>(); BGM.volume = COMMON.U.volM;
		//TextSound = transform.Find("Text").GetComponent<AudioSource>(); TextSound.volume = COMMON.U.volS;
		if (S.SC2.realCulpritArrested) { BGM.clip = post; loopAt = 0; }
		else if (S.inDialogue == 4 && S.SC2.screenZoom == 6) { BGM.clip = sF; loopAt = 0; }
		else if (S.SC2.theyDontExist) { BGM.clip = fakes; loopAt = 0; }
		else if (S.SC2.ShawnSearch) BGM.clip = null;
		else if (S.SC2.phaseTwo) { BGM.clip = phaseTwo; loopAt = 0; }
		else if (S.inDialogue == 2) { BGM.clip = ash; loopAt = 0; }
		else if (S.inDialogue == 3) { BGM.clip = logs; loopAt = 0; }
		else if (S.inDialogue == 11) { BGM.clip = sA; loopAt = 0; }
		else if (S.inDialogue == 12) { BGM.clip = sB; loopAt = 6.672F; }
		else if (S.inDialogue == 13) { BGM.clip = sC; loopAt = 10.353F; }
		else if (S.inDialogue == 14) { BGM.clip = sD; loopAt = 5.359F; }
		else if (S.inDialogue == 15) { BGM.clip = sE; loopAt = 0; }
		else if (S.SC2.briefed) { BGM.clip = briefed; loopAt = 24.631F; }
		else if (S.SC2.seenIntro) { BGM.clip = intro; loopAt = 30.708F; }
		else BGM.clip = null; if (BGM.clip != null) BGM.Play();

		CursorLock(false); bS.gameObject.SetActive(!S.SC2.roomAppeared || S.SC2.ShawnSearch && S.inNarration);
		if (!S.SC2.roomAppeared) UIC.StartNarration(0);
		if (S.inNarration) UIC.StartNarration(S.NID);
		else if (S.inDialogue == 0) UIC.StartDialogue(DialogueKettley, S.SC2.DSKettley, 0, S.SC2.nextDID_Kettley, true);
		else if (S.inDialogue == 1) UIC.StartDialogue(DialogueShawn, S.SC2.DSShawn,1, S.SC2.nextDID_Shawn, true);
		else if (S.inDialogue == 2) UIC.StartDialogue(DialogueAshley, S.SC2.DSAshley, 2, S.SC2.nextDID_Ashley, false);
		else if (S.inDialogue == 3) UIC.StartDialogue(DialogueScreens, S.SC2.DSScreens, 3, S.SC2.nextDID_Screens, false, true, S.SC2.screenZoom == 0);
		else if (S.inDialogue == 4) UIC.StartDialogue(DialogueOther, S.SC2.DSOther, 4, S.SC2.nextDID_Other, false);
		else if (S.inDialogue == 11) UIC.StartDialogue(DialogueSuspectA, S.SC2.DSA, 11, S.SC2.nextDID_A, false);
		else if (S.inDialogue == 12) UIC.StartDialogue(DialogueSuspectB, S.SC2.DSB, 12, S.SC2.nextDID_B, false);
		else if (S.inDialogue == 13) UIC.StartDialogue(DialogueSuspectC, S.SC2.DSC, 13, S.SC2.nextDID_C, false);
		else if (S.inDialogue == 14) UIC.StartDialogue(DialogueSuspectD, S.SC2.DSD, 14, S.SC2.nextDID_D, false);
		else if (S.inDialogue == 15) UIC.StartDialogue(DialogueSuspectE, S.SC2.DSE, 15, S.SC2.nextDID_E, false);
		MC.FadeInLevel(!S.inNarration && S.inDialogue == -1);

		//for testing phase 2 quickly
		//S.SC2.DSScreens.Locked[31] = false; S.SC2.DSShawn.Locked[250] = false;
	}

	public void UPP() {
		var hh = GetComponent<HighlightHints>();
		if (S.SC2.currentRoom == 0) {
			if (S.SC2.npp == 0) hh.NPS[0] = ppKettley;
			else if (S.SC2.npp == 1) hh.NPS[0] = ppShawn;
			else hh.NPS[0] = ppArchwayL;
		} else {
			if (S.SC2.npp == 2) hh.NPS[0] = ppAshley;
			else if (S.SC2.npp == 3) hh.NPS[0] = ppScreens;
			else hh.NPS[0] = ppArchwayB;
		}
	}

	public override void UISettingsChanged() {
		GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().UIChangeRuntime();
		if (S.inDialogue == 0) {
			camL.localRotation = COMMON.U.textLayout < 2 ? RotKet : RotKetAlt;
		}
		else if (S.inDialogue == 1) {
			camL.localPosition = COMMON.U.textLayout < 2 ? PosShawn : PosShawnAlt;
			camL.localRotation = COMMON.U.textLayout < 2 ? RotShawn : RotShawnAlt;
		}
	}

	public override void PauseAnimations(bool pause) {
		if (S.SC2.currentRoom == 0) {
			var scrK = ppKettley.GetComponent<OnClick_Kettley>();
			scrK.A["BreatheA"].speed = pause ? 0 : 0.01666F;
			scrK.A["BreatheB"].speed = pause ? 0 : 0.01666F;
			scrK.A["Turn"].speed = pause ? 0 : 1;
			scrK.A["AtoB"].speed = pause ? 0 : (scrK.state == 0 ? -1 : 1);
			scrK.HL["K"].speed = scrK.A["AtoB"].speed;
			var scrS = ppShawn.GetComponent<OnClick_Shawn>();
			scrS.A["BreatheBase"].speed = pause ? 0 : 0.02F;
			scrS.A["BreatheTurned"].speed = pause ? 0 : 0.02F;
			scrS.A["BreatheThink"].speed = pause ? 0 : 0.02F;
			scrS.A["Base"].speed = pause ? 0 : 1;
			scrS.A["TurnA"].speed = pause ? 0 : -1; //the only time when you can pause Shawn's turning is when it after talking
			scrS.A["HandR"].speed = pause ? 0 : 1; scrS.A["HandL"].speed = pause ? 0 : 1;
			scrS.A["HandU"].speed = pause ? 0 : 1; scrS.A["HandD"].speed = pause ? 0 : 1;
			scrS.A["HandRU"].speed = pause ? 0 : 1; scrS.A["HandRD"].speed = pause ? 0 : 1;
			scrS.A["HandLU"].speed = pause ? 0 : 1; scrS.A["HandLD"].speed = pause ? 0 : 1;
			scrS.A["Think"].speed = pause ? 0 : (scrS.state == 21 ? -1 : 1); scrS.HL["S1"].speed = scrS.A["Think"].speed;
			scrS.A["ThinkScratch"].speed = pause ? 0 : 1; scrS.HL["S11"].speed = scrS.A["ThinkScratch"].speed;
		} else {
			var scrA = ppAshley.GetComponent<OnClick_Ashley>();
			scrA.A["Breathe"].speed = pause ? 0 : 0.02F;
			scrA.A["PoseAtoB"].speed = pause ? 0 : (scrA.state == 1 ? 1 : -1);
			scrA.A["PoseAtoC"].speed = pause ? 0 : (scrA.state == 2 ? 1 : -1);
			scrA.A["PoseBtoC"].speed = pause ? 0 : (scrA.state == 2 ? 1 : -1);
			scrA.A["Turn"].speed = pause ? 0 : 1;
		}
	}
}

[System.Serializable]
public class SaveChapter2 {
	public DialogueStructure DSKettley, DSShawn, DSAshley, DSScreens, DSOther, DSA, DSB, DSC, DSD, DSE;
	public int nextDID_Kettley, nextDID_Shawn, nextDID_Ashley, nextDID_Screens, nextDID_Other;
	public int nextDID_A, nextDID_B, nextDID_C, nextDID_D, nextDID_E;
	public bool nothingToTalk_Kettley, nothingToTalk_Shawn, nothingToTalk_Ashley;

	public int currentRoom = 0; //0 - living room, 1 - bedroom
	public int npp = 0; //0 - Kettley, 1 - Shawn, 2 - Ashley, 3 - screens
	public int[] CCID = new int[] {1};
	public int screenZoom = -1; //0 - main screen, 1-6 - suspects A-E
	public int mScrInd1 = -1; //0 - profiles, 1 - blogs, 2 - commenters, 3 - meet-ups
	public int mScrInd2 = -1; //1: 0-6, 2: 0-3, 2: 0-19, 3: 0-5
	public int mScrInd3 = -1; //page number

	public bool roomAppeared = false;
	public bool seenIntro = false;
	public bool metKettley = false;
	public bool metShawn = false;
	public bool metAshley = false;
	public bool briefedToSet = false;
	public bool briefed = false;
	public bool checkedScreens = false;
	public bool accusedSomebody = false;
	public bool arrestedAorC = false;
	public bool arrestedBorD = false;
	public bool[] learnedSuspect = new bool[4];
	public bool[] sawMeetup = new bool[6];
	public bool learnedAllOnScreens = false;
	public bool learnedAshleyView = false;
	public bool phaseTwoToSet = false;
	public bool phaseTwo = false;
	public bool[] checkedActivity = new bool[20];
	public int checkedReals = 0;
	public int checkedFakes = 0;
	public int warrantsUsed = 0;
	public bool spottedStrangeThing = false;
	public bool ShawnSearch = false;
	public bool theyDontExist = false;
	public bool toldTheGoodNews = false;
	public bool foundF = false;
	public bool metF = false;
	public bool trackedF = false;
	public bool realCulpritArrested = false;
	public bool queueTheReveal = false;
	public bool hackerRevealed = false;
	public bool deskUnfolded = false;
	public bool discussedCloset = false;
	public bool discussedTree = false;
	public bool discussedGuitar = false;
	public bool prisonsTalkKettley = false;
}

[XmlRoot("Root")]
public class InvestigationDataCBIU {
	[XmlArray("ProfileList"), XmlArrayItem("Profile")] public string[] Profiles;
	[XmlArray("BlogList"), XmlArrayItem("Blog")] public string[] Blogs;
	public string Discussion;
	public string ErrorMessage;
	[XmlArray("CommenterList"), XmlArrayItem("Commenter")] public CommenterCBIU[] Commenters;
	[XmlArray("MeetupList"), XmlArrayItem("Meetup")] public string[] Meetups;

	public static InvestigationDataCBIU Load(string path) {
		var serializer = new XmlSerializer(typeof(InvestigationDataCBIU));
		using(var stream = new FileStream(path, FileMode.Open)) {
			return serializer.Deserialize(stream) as InvestigationDataCBIU;
		}
	}
}
public class CommenterCBIU {
	public string Profile;
	public string History;
}