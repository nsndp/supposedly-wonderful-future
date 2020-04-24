using UnityEngine;
using System.Collections;
using System.IO;

public class DataControlChapter3 : DataControl {

	public TXT Dialogue;
	public AudioClip theme;
	public GameObject Video, Startup;

	public override int[] GetCCID() { return null; }
	public override void UISettingsChanged() { }
	public override void VideoResChanged() { Video.GetComponent<Imagery>().changeRes = true; }
	public override void PauseAnimations(bool pause) { }

	void Start() {
		if (!GetComponent<BinGeneration_VRStation>().enabled) Init();
	}
	public void Init() {
		COMMON.LoadUserSettings(); if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, 3);
		dataFolder = COMMON.dataFolder + "VRStation/";
		Dialogue = TXT.Load(LOC("Dialogue"));
		Narration = TXT.Load(LOC("Narration"));
		NS = NarrationStructure.Load(dataFolder + "Narration.bin");

		if (COMMON.saveToLoad == null) {
			Debug.Log("TEST MODE"); new GameObject("Steam", typeof(SteamControl));
			S = new SaveGame(); S.levelID = 3; S.SH = new SaveHub();
		}
		else {
			S = SaveGame.Load(COMMON.saveFolder + COMMON.saveToLoad);
			if (Application.isEditor) Debug.Log("Game loaded: " + COMMON.saveToLoad);
		}
		if (S.SC3 == null) {
			S.SC3 = new SaveChapter3();
			S.SC3.DStruct = DialogueStructure.Load(dataFolder + "Dialogue.bin");
		}

		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		bMenu = UIC.transform.Find("ButtonMenu").gameObject;
		bReturn = UIC.transform.Find("ButtonArrow").gameObject;
		bS = UIC.transform.Find("BlackScreen").GetComponent<UnityEngine.UI.Image>();
		MC.Init(); UIC.Init();

		Startup = GameObject.Find("VR").transform.Find("Startup").gameObject;
		Video = GameObject.Find("VR").transform.Find("Video").gameObject;
		Startup.SetActive(S.SC3.inStartup);
		Video.SetActive(S.inDialogue >= 0);

		BGM = transform.Find("Music").GetComponent<AudioSource>(); BGM.volume = COMMON.U.volM;
		Sound = transform.Find("Sound").GetComponent<AudioSource>(); Sound.volume = COMMON.U.volS;
		if (S.inDialogue >= 0) { BGM.clip = theme; BGM.Play(); loopAt = 1.164F; }

		CursorLock(false); currentColliders = new GameObject();
		if (!S.SC3.started) { S.SC3.started = true; UIC.StartNarration(0); }
		else if (S.inNarration) UIC.StartNarration(S.NID);
		else if (S.inDialogue >= 0) UIC.StartDialogue(Dialogue, S.SC3.DStruct, 0, S.SC3.nextDID, false);
		MC.FadeInLevel(!S.inNarration && S.inDialogue == -1);

		//UIC.GetLongestLine("Dialogue", Dialogue); UIC.GetLongestLine("Narration", Narration);
	}
}

[System.Serializable]
public class SaveChapter3 {
	public DialogueStructure DStruct;
	public int nextDID = 0;
	public bool started = false;
	public bool inStartup = false;
	public bool talkedAboutCreation = false;
	public bool talkedAboutBeliefs = false;
	public int questionsAsked = 0;
}