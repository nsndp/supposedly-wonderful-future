using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class DataControlChapter5 : DataControl {

	public TXT Dialogue;
	public Transform cam, Cred; public Animation OM;
	public Vector3 PosFarA, RotFarA, PosFarB, RotFarB;
	public Vector3 PosCloseA, RotCloseA, PosCloseB, RotCloseB;
	public AudioClip main, intro, credits, gun, shot;

	public override int[] GetCCID() { return null; }
	public override void VideoResChanged() { }

	void AssignVectors() {
		var a = (Screen.width * 1.0F / Screen.height - 1.7777777777F) / (1.25F - 1.7777777777F);
		PosFarA = Vector3.Lerp(new Vector3(-0.24F, 2.2F, -4.36F), new Vector3(-0.2F, 2, -4.8F), a);
		PosFarB = Vector3.Lerp(new Vector3(-1.2F, 2.13F, -4.3F), new Vector3(-1.4F, 1.8F, -4.3F), a);
		PosCloseA = Vector3.Lerp(new Vector3(-0.52F, 1.48F, -2.8F), new Vector3(-0.4F, 1.58F, -3.1F), a);
		PosCloseB = Vector3.Lerp(new Vector3(-1.4F, 1.35F, -2.88F), new Vector3(-1.3F, 1.5F, -3.4F), a);
		RotFarA = new Vector3(14, -0.8F, 0); RotFarB = new Vector3(12, 3.2F, 0);
		RotCloseA = new Vector3(20, 0, 0); RotCloseB = new Vector3(16, 6, 0);
		//PosBR = new Vector3(-0.3F, 1.26F, -2.9F); RotBR = new Vector3(16, 8, 0);
	}

	void Start() {
		if (!GetComponent<BinGeneration_OfficeCEO>().enabled) Init();
	}

	public void Init() {
		COMMON.LoadUserSettings(); if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, 5);
		dataFolder = COMMON.dataFolder + "OfficeCEO/";
		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		bMenu = UIC.transform.Find("ButtonMenu").gameObject;
		bReturn = UIC.transform.Find("ButtonArrow").gameObject;
		bS = UIC.transform.Find("BlackScreen").GetComponent<UnityEngine.UI.Image>();
		MC.Init(); UIC.Init(); AssignVectors();
		BGM = transform.Find("Music").GetComponent<AudioSource>(); BGM.volume = COMMON.U.volM;
		Sound = transform.Find("Sound").GetComponent<AudioSource>(); Sound.volume = COMMON.U.volS;

		//if (COMMON.saveToLoad == null) { //TEST
		//  new GameObject("Steam", typeof(SteamControl));
		//	S = new SaveGame(); S.levelID = 5; S.SH = new SaveHub(); S.SH.asked[3] = true;
		//	S.Save(COMMON.saveFolder + "Autosave.bin"); COMMON.saveToLoad = "Autosave.bin";
		//}

		if (COMMON.saveToLoad == null) {
			Dialogue = TXT.Load(LOC("Intro"));
			SetStatesIntro();
			GetComponent<Intro>().LaunchIntro();
		} else {
			Dialogue = TXT.Load(LOC("OldMan"));
			Narration = TXT.Load(LOC("Narration"));
			NS = NarrationStructure.Load(dataFolder + "Narration.bin");
			S = SaveGame.Load(COMMON.saveFolder + COMMON.saveToLoad);
			if (Application.isEditor) Debug.Log("Game loaded: " + COMMON.saveToLoad);
			if (S.SC5 == null) {
				S.SC5 = new SaveChapter5();
				S.SC5.DS = DialogueStructure.Load(dataFolder + "OldMan.bin");
			}
			SetStates();
			//UIC.GetLongestLine("OldMan", Dialogue); UIC.GetLongestLine("Narration", Narration);
		}
	}

	void SetStatesIntro() {
		currentColliders = new GameObject();
		bS.gameObject.SetActive(true);
		//camera
		cam = GameObject.Find("Camera").transform;
		if (COMMON.U.textLayout < 2) { cam.localPosition = PosCloseA; cam.localRotation = Quaternion.Euler(RotCloseA); }
		else { cam.localPosition = PosCloseB; cam.localRotation = Quaternion.Euler(RotCloseB); }
		//character animations
		OM = GameObject.Find("Desk/OldMan").GetComponent<Animation>();
		OM.GetComponent<OldManAnimations>().Init();
		OM.Play("Main"); OM["LowerHead"].time = OM["LowerHead"].length; OM.Play("LowerHead");
		//music
		BGM.clip = intro; loopAt = 4.892F;
	}

	void SetStates() {
		currentColliders = new GameObject();
		bS.gameObject.SetActive(S.inDialogue == -1);
		//camera
		cam = GameObject.Find("Camera").transform;
		if (!S.SC5.closeUp && COMMON.U.textLayout < 2) { cam.localPosition = PosFarA; cam.localRotation = Quaternion.Euler(RotFarA); }
		else if (S.SC5.closeUp && COMMON.U.textLayout < 2) { cam.localPosition = PosCloseA; cam.localRotation = Quaternion.Euler(RotCloseA); }
		else if (!S.SC5.closeUp && COMMON.U.textLayout == 2) { cam.localPosition = PosFarB; cam.localRotation = Quaternion.Euler(RotFarB); }
		else { cam.localPosition = PosCloseB; cam.localRotation = Quaternion.Euler(RotCloseB); }

		if (S.SH.asked[3]) S.SC5.DS.Locked[27] = false;

		//character animations
		OM = GameObject.Find("Desk/OldMan").GetComponent<Animation>();
		OM.GetComponent<OldManAnimations>().Init();
		if (S.SC5.agreed) {
			OM.transform.Find("Handgun").gameObject.SetActive(false);
			OM["Relax"].time = OM["Relax"].length; OM.Play("Relax");
			OM["RelaxHead"].time = OM["RelaxHead"].length; OM.Play("RelaxHead");
		}
		else if (S.SC5.refused) {
			OM["TakeGun"].time = OM["TakeGun"].length; OM.Play("TakeGun");
			OM["TakeGunHead"].time = 0.5F; OM["TakeGunHead"].speed = 0; OM.Play("TakeGunHead");
		}
		else if (S.SC5.heardEnough) {
			OM.Play("Main");
			OM["LowerHead"].time = OM["LowerHead"].length; OM.Play("LowerHead");
		}
		else {
			OM.Play("Main");
			if (COMMON.U.textLayout == 2) OM.Play("Turned");
		}

		//prepare credits
		Cred.gameObject.SetActive(false);
		if (Screen.width != 1280) {
			float scale = Screen.width / 1280.0F; Vector2 v;
			var RT = Cred.GetComponentsInChildren<RectTransform>(true);
			var TX = Cred.GetComponentsInChildren<Text>(true);
			for (int i = 1; i < RT.Length; i++) {
				v = RT[i].offsetMin; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); RT[i].offsetMin = v;
				v = RT[i].offsetMax; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); RT[i].offsetMax = v;
			}
			for (int i = 0; i < TX.Length; i++) TX[i].fontSize = Mathf.RoundToInt(TX[i].fontSize * scale);
		}

		if (S.SC5.heardEnough) { BGM.clip = intro; BGM.Play(); loopAt = 4.892F; }
		else if (S.SC5.closeUp) { BGM.clip = main; BGM.Play(); loopAt = 22.571F; }
		else BGM.clip = null;

		CursorLock(false);
		if (S.inDialogue == -1) UIC.StartNarration(0);
		else UIC.StartDialogue(Dialogue, S.SC5.DS, 0, S.SC5.nextDID, false);
		MC.FadeInLevel(false);
	}

	public override void UISettingsChanged() {
		if (!S.SC5.closeUp && COMMON.U.textLayout < 2) { cam.localPosition = PosFarA; cam.localRotation = Quaternion.Euler(RotFarA); }
		else if (S.SC5.closeUp && COMMON.U.textLayout < 2) { cam.localPosition = PosCloseA; cam.localRotation = Quaternion.Euler(RotCloseA); }
		else if (!S.SC5.closeUp && COMMON.U.textLayout == 2) { cam.localPosition = PosFarB; cam.localRotation = Quaternion.Euler(RotFarB); }
		else { cam.localPosition = PosCloseB; cam.localRotation = Quaternion.Euler(RotCloseB); }

		OM.GetComponent<OldManAnimations>().SetCamVars();
		if (!S.SC5.agreed && !S.SC5.refused && !S.SC5.heardEnough)
			if (COMMON.U.textLayout == 2) OM.Play("Turned"); else OM.Stop("Turned");
	}
	public override void PauseAnimations(bool pause) {
		OM["Main"].speed = pause ? 0 : 0.01F;
		OM["LowerHead"].speed = pause ? 0 : 0.25F;
		OM["LowerHeadTurned"].speed = pause ? 0 : 0.25F;
		OM["Relax"].speed = pause ? 0 : 0.5F;
		OM["RelaxHead"].speed = pause ? 0 : 0.5F;
		OM["TakeGun"].speed = pause ? 0 : 1;
		OM["TakeGunHead"].speed = pause ? 0 : (OM.GetComponent<OldManAnimations>().phase == 5 ? -0.6F : 1);
	}
}

[System.Serializable]
public class SaveChapter5 {
	public DialogueStructure DS = null;
	public int nextDID = 0;
	public bool closeUp = false;
	public bool heardEnough = false;
	public bool agreed = false;
	public bool refused = false;
}