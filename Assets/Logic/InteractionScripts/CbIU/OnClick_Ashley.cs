using UnityEngine;
using System.Collections;

public class OnClick_Ashley : CharacterOnClick {
	DataControlChapter2 DC;
	public Animation A; public GameObject HL;
	//Vector3 SrcPos, DstPos = new Vector3(4, 1.1F, 2);
	//Quaternion SrcRot, DstRot = Quaternion.Euler(new Vector3(13, 187, 0));
	public int state = 0; int r, chance; int CH = 20;
	string s; bool inTalk = false;
	float tc; int camPhase = 0;
		
	public void LoadDuringDialogue() {
		A["Turn"].wrapMode = WrapMode.ClampForever; A["Turn"].speed = 1.0F;
		A["Turn"].time = A["Turn"].length; A.Play("Turn");
		inTalk = true;
	}
	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		A["Turn"].AddMixingTransform(A.transform.Find("Ashley/master/root/spine/spine-1/chest/DEF-chest-1/DEF-neck"), true);
		A["Turn"].AddMixingTransform(A.transform.Find("Ashley/master/root/spine/spine-1/chest/chest-1/neck"), false);
		A["Turn"].AddMixingTransform(A.transform.Find("Ashley/master/root/spine/spine-1/chest/chest-1/neck/head"), true);
		A["Turn"].layer = 3;
		A["Breathe"].AddMixingTransform(A.transform.Find("Ashley/master/root/spine/spine-1/chest"), false);
		A["Breathe"].layer = 2; A["Breathe"].speed = 0.02F;
		s = "Breathe"; A.Play(s);
	}

	public override void RedHL(bool on) {
		HL.GetComponent<Renderer>().material.SetColor("_TintColor", on ? new Color(1, 0.141F, 0.141F) : new Color(0, 0.463F, 1));
		HL.SetActive(on);
	}
	public override void MsEnter() { if (!DC.S.SC2.nothingToTalk_Ashley) { HL.SetActive(true); DC.activeHL = HL; } }
	public override void MsExit() { HL.SetActive(false); DC.activeHL = null; }
	void OnMouseEnter() { if (Cursor.visible) MsEnter(); }
	void OnMouseExit() { if (Cursor.visible) MsExit(); }
	void OnMouseDown() {
		if (!DC.S.SC2.nothingToTalk_Ashley) {
			A["Turn"].wrapMode = WrapMode.ClampForever; A["Turn"].speed = 1.0F;
			DC.UIC.Col(false); DC.CursorLock(true); DC.bMenu.SetActive(false);
			if (!DC.S.SC2.phaseTwo) {
				if (!DC.S.SC2.briefed) DC.introT = DC.BGM.timeSamples; else DC.briefedT = DC.BGM.timeSamples;
				DC.MStop();
			}
			tc = 0; camPhase = 1;
		}
	}

	void PlayForward(string s) { A[s].speed = 1.0F; A.Play(s); }
	void PlayBackward(string s) { A[s].speed = -1.0F; A[s].time = A[s].length; A.Play(s); }
	void Update() {
		if (DC.paused) return;

		//CHANGE VIEW
		if (camPhase == 1 && tc <= 1) {
			DC.camB.transform.position = Vector3.Lerp(DC.PosB, DC.PosAsh, Mathf.SmoothStep(0, 1, tc));
			DC.camB.transform.rotation = Quaternion.Lerp(DC.RotB, DC.RotAsh, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01F * Time.deltaTime * 60;
			//if (tc > 0.8F && !A.IsPlaying("Turn")) A.Play("Turn");
		}
		else if (camPhase == 1 && tc > 1) {
			A.Play("Turn"); inTalk = true; DC.CursorLock(false); DC.bMenu.SetActive(true); camPhase = 0;
			DC.UIC.StartDialogue(DC.DialogueAshley, DC.S.SC2.DSAshley, 2, DC.S.SC2.nextDID_Ashley, false);
			if (!DC.S.SC2.phaseTwo) DC.MPlay(DC.ash, 0, DC.ashT);
		}
		else if (camPhase == 2 && tc <= 1) {
			DC.camB.transform.position = Vector3.Lerp(DC.PosAsh, DC.PosB, Mathf.SmoothStep(0, 1, tc));
			DC.camB.transform.rotation = Quaternion.Lerp(DC.RotAsh, DC.RotB, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01F * Time.deltaTime * 60;
		}
		else if (camPhase == 2 && tc > 1) {
			DC.UIC.Col(true); DC.CursorLock(false); DC.bMenu.SetActive(true); camPhase = 0;
			if (!DC.S.SC2.phaseTwo && DC.S.SC2.briefed) DC.MPlay(DC.briefed, 24.631F, DC.briefedT);
			else if (!DC.S.SC2.phaseTwo) DC.MPlay(DC.intro, 30.708F, DC.introT);
		}

		//FINISHER
		if (inTalk && DC.S.inDialogue == -1) {
			A["Turn"].wrapMode = WrapMode.Default; A["Turn"].speed = -1.0F;
			A["Turn"].time = A["Turn"].length; A.Play("Turn");
			DC.UIC.Col(false); DC.CursorLock(true); DC.bMenu.SetActive(false);
			tc = 0; camPhase = 2; inTalk = false;
			if (!DC.S.SC2.phaseTwo) { DC.ashT = DC.BGM.timeSamples; DC.MStop(); }
			if (DC.S.SC2.metAshley && !DC.S.SC2.theyDontExist) {
				DC.currentColliders.transform.Find("Closet").gameObject.SetActive(!DC.S.SC2.discussedCloset);
				DC.currentColliders.transform.Find("Tree").gameObject.SetActive(!DC.S.SC2.discussedTree);
				DC.currentColliders.transform.Find("Guitar").gameObject.SetActive(!DC.S.SC2.discussedGuitar);
			}
		}

		//IDLE
		if (!A.IsPlaying(s)) {
			if (inTalk) { s = "Breathe"; A.Play(s); }
			else if (state == 0) {
				r = Random.Range(0, chance);
				if (r > 1) { s = "Breathe"; A.Play(s); chance--; }
				else if (r == 1) { s = "PoseAtoB"; PlayForward(s); state = 1; chance = CH; }
				else { s = "PoseAtoC"; PlayForward(s); state = 2; chance = CH; }
			}
			else if (state == 1) {
				r = Random.Range(0, chance);
				if (r > 1) { s = "Breathe"; A.Play(s); chance--; }
				else if (r == 1) { s = "PoseBtoC"; PlayForward(s); state = 2; chance = CH; }
				else { s = "PoseAtoB"; PlayBackward(s); state = 0; chance = CH; }
			}
			else if (state == 2) {
				r = Random.Range(0, chance);
				if (r > 1) { s = "Breathe"; A.Play(s); chance--; }
				else if (r == 1) { s = "PoseAtoC"; PlayBackward(s); state = 0; chance = CH; }
				else { s = "PoseBtoC"; PlayBackward(s); state = 1; chance = CH; }
			}
		}
	}
}
