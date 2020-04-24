using UnityEngine;
using System.Collections;

public class OnClick_Kettley : CharacterOnClick {
	UIControl UIC; DataControlChapter2 DC;
	public Animation A, HL;

	int CH = 10;
	//int CH1 = 5, CH2 = 5, CH3 = 5;
	public int state = 0; int r, chance; string s; float tc;
	int camPhase = 0; bool inTalk = false;

	public void LoadDuringDialogue() {
		A["Turn"].wrapMode = WrapMode.ClampForever; A["Turn"].speed = 1.0F;
		A["Turn"].time = A["Turn"].length; A.Play("Turn");
		inTalk = true;
	}
	public void Init() {
		UIC = GameObject.Find("Interface/UI").GetComponent<UIControl>();
		DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		A["Turn"].AddMixingTransform(A.transform.Find("Kettley/master/root/spine/spine-1/chest/DEF-chest-1/DEF-neck"), true);
		A["Turn"].AddMixingTransform(A.transform.Find("Kettley/master/root/spine/spine-1/chest/chest-1/neck"), false);
		A["Turn"].AddMixingTransform(A.transform.Find("Kettley/master/root/spine/spine-1/chest/chest-1/neck/head"), true);
		A["Turn"].layer = 2;
		A["BreatheA"].speed = 0.01666F; A["BreatheB"].speed = 0.01666F;
		s = "BreatheA"; chance = CH; A.Play(s);
	}

	void HLV() {
		HL.Stop();
		if (A.IsPlaying("AtoB")) { HL["K"].speed = A["AtoB"].speed; HL["K"].time = A["AtoB"].time; }
		else { HL["K"].speed = 0; HL["K"].normalizedTime = state == 1 ? 1 : 0; }
		HL.Play("K");
	}
	public override void RedHL(bool on) {
		HL.GetComponent<Renderer>().material.SetColor("_TintColor", on ? new Color(1, 0.141F, 0.141F) : new Color(0, 0.463F, 1));
		HL.gameObject.SetActive(on); if (on) HLV();
	}
	public override void MsEnter() {
		if (!DC.S.SC2.nothingToTalk_Kettley) {
			HL.gameObject.SetActive(true); DC.activeHL = HL.gameObject; HLV();
		}
	}
	public override void MsExit() { HL.gameObject.SetActive(false); DC.activeHL = null; }
	void OnMouseEnter() { if (Cursor.visible) MsEnter(); }
	void OnMouseExit() { if (Cursor.visible) MsExit(); }
	void OnMouseDown() {
		if (!DC.S.SC2.nothingToTalk_Kettley) {
			A["Turn"].wrapMode = WrapMode.ClampForever; A["Turn"].speed = 1.0F;
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			tc = 0; camPhase = 1;
		}
	}

	void Update() {
		if (DC.paused) return;
		//CHANGE VIEW
		if (camPhase == 1 && tc <= 1) {
			DC.camL.transform.position = Vector3.Lerp(DC.PosA, DC.PosKet, Mathf.SmoothStep(0, 1, tc));
			DC.camL.transform.rotation = Quaternion.Lerp(DC.RotA, COMMON.U.textLayout < 2 ? DC.RotKet : DC.RotKetAlt, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01F * Time.deltaTime * 60;
			//if (tc > 0.8F && !A.IsPlaying("Turn")) A.Play("Turn");
		}
		else if (camPhase == 1 && tc > 1) {
			A.Play("Turn"); inTalk = true; DC.CursorLock(false); DC.bMenu.SetActive(true); camPhase = 0;
			UIC.StartDialogue(DC.DialogueKettley, DC.S.SC2.DSKettley, 0, DC.S.SC2.nextDID_Kettley, true);
		}
		else if (camPhase == 2 && tc <= 1) {
			DC.camL.transform.position = Vector3.Lerp(DC.PosKet, DC.PosA, Mathf.SmoothStep(0, 1, tc));
			DC.camL.transform.rotation = Quaternion.Lerp(COMMON.U.textLayout < 2 ? DC.RotKet : DC.RotKetAlt, DC.RotA, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01F * Time.deltaTime * 60;
		}
		else if (camPhase == 2 && tc > 1) {
			DC.CursorLock(false); DC.UIC.Col(true); DC.bMenu.SetActive(true); camPhase = 0;
		}
		
		//FINISHER
		if (inTalk && DC.S.inDialogue == -1 && !DC.noZoomOut) {
			A["Turn"].wrapMode = WrapMode.Default; A["Turn"].speed = -1.0F;
			A["Turn"].time = A["Turn"].length; A.Play("Turn");
			DC.CursorLock(true); DC.bMenu.SetActive(false); tc = 0; camPhase = 2; inTalk = false;
			HL.gameObject.SetActive(false); DC.activeHL = null; //will it fix it for laptop?
		}
		
		//IDLE
		if (!A.IsPlaying(s)) {
			if (inTalk && state == 0) { s = "BreatheA"; A.Play(s); }
			else if (inTalk && state == 1) { s = "BreatheB"; A.Play(s); }
			else if (state == 0) {
				r = Random.Range(0, chance);
				if (r > 1) { s = "BreatheA"; A.Play(s); chance--; }
				else if (r == 1) {
					s = "AtoB"; A[s].speed = 1.0F; A.Play(s);
					HL.Stop(); HL["K"].speed = 1; HL["K"].time = 0; HL.Play("K");
					state = 1; chance = CH;
				}
			}
			else if (state == 1) {
				r = Random.Range(0, chance);
				if (r > 1) { s = "BreatheB"; A.Play(s); chance--; }
				else if (r == 1) {
					s = "AtoB"; A[s].speed = -1.0F; A[s].time = A[s].length; A.Play(s);
					HL.Stop(); HL["K"].speed = -1; HL["K"].time = HL["K"].length; HL.Play("K");
					state = 0; chance = CH;
				}
			}
		}
	}

	//----------FULL VERSION----------
	/*void OnMouseDown() {
		GetComponent<MeshRenderer>().enabled = false;
		if (!DC.S.SC2.nothingToTalk_Kettley) {
			T.animation["Turn"].wrapMode = WrapMode.ClampForever; T.animation["Turn"].speed = 1.0F;
			prevStateIsZero = state == 1 ? false : true;
			if (state == 2 && T.animation.IsPlaying("DeviceTakeOut")) T.animation["DeviceTakeOut"].speed = -1.0F;
			else if (state == 2) { T.animation["DeviceTakeOut"].speed = -1.0F; T.animation["DeviceTakeOut"].time = T.animation["DeviceTakeOut"].length; T.animation.CrossFade("DeviceTakeOut"); }
			foreach (Collider cl in DC.currentColliders.GetComponentsInChildren<Collider>()) cl.enabled = false;
			if (changeView) { tc = 0; state = 5; Screen.lockCursor = true; }
			else if (!T.animation.IsPlaying("BreatheA") && !T.animation.IsPlaying("BreatheB")) {
				state = 50; Screen.lockCursor = true;
			} else {
				T.animation.Play("Turn"); state = prevStateIsZero ? 60 : 61;
				UICtrl.StartDialogue(DC.DialogueKettley, DC.S.SC2.DSKettley, DC.S, 0, DC.S.SC2.nextDID_Kettley, true);
			}
		}
	}

	void Update () {
		//no change view
		if (state == 50 && !T.animation.isPlaying) {
			T.animation.Play("Turn"); state = 51;
		}
		else if (state == 51 && T.animation["Turn"].time >= T.animation["Turn"].length) {
			Screen.lockCursor = false; state = prevStateIsZero ? 60 : 61;
			UICtrl.StartDialogue(DC.DialogueKettley, DC.S.SC2.DSKettley, DC.S, 0, DC.S.SC2.nextDID_Kettley, true);
		}
		//changing view
		else if (state == 5 && tc <= 1) {
			cam.transform.position = Vector3.Lerp(SrcPos, DstPos, Mathf.SmoothStep(0, 1, tc));
			cam.transform.rotation = Quaternion.Lerp(SrcRot, DstRot, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01F;
			if (tc >= 0.6F && !T.animation.IsPlaying("Turn") && (!T.animation.isPlaying || T.animation.IsPlaying("BreatheA") || T.animation.IsPlaying("BreatheB"))) {
				T.animation.Play("Turn");
			}
		}
		else if (state == 5 && tc > 1 && (T.animation.IsPlaying("Turn") || !T.animation.isPlaying)) {
			if (!T.animation.IsPlaying("Turn")) T.animation.Play("Turn");
			Screen.lockCursor = false; state = prevStateIsZero ? 60 : 61;
			UICtrl.StartDialogue(DC.DialogueKettley, DC.S.SC2.DSKettley, DC.S, 0, DC.S.SC2.nextDID_Kettley, true);
		}
		//common
		else if (state == 60 && !T.animation.IsPlaying("BreatheA")) T.animation.Play("BreatheA");
		else if (state == 61 && !T.animation.IsPlaying("BreatheB")) T.animation.Play("BreatheB");
		else if (DC.S.inDialogue == -1 && (state == 60 || state == 61)) {
			T.animation["Turn"].wrapMode = WrapMode.Default; T.animation["Turn"].speed = -1.0F;
			T.animation["Turn"].time = T.animation["Turn"].length;
			if (!changeView) { T.animation.Play("Turn"); state = prevStateIsZero ? 0 : 1; }
			else {
				turn = false; Screen.lockCursor = true; tc = 0; state = 7;
				foreach (Collider cl in DC.currentColliders.GetComponentsInChildren<Collider>()) cl.enabled = false;
			}
		}
		//changing view
		else if (state == 7 && tc <= 1) {
			cam.transform.position = Vector3.Lerp(DstPos, SrcPos, Mathf.SmoothStep(0, 1, tc));
			cam.transform.rotation = Quaternion.Lerp(DstRot, SrcRot, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01F; if (tc >= 0.2F && !turn) { T.animation.Play("Turn"); turn = true; }
		}
		else if (state == 7 && tc > 1) {
			foreach (Collider cl in DC.currentColliders.GetComponentsInChildren<Collider>()) cl.enabled = true;
			Screen.lockCursor = false; state = prevStateIsZero ? 0 : 1;
		}

		//IDLE
		else if (state == 0 && !T.animation.isPlaying) {
			r = Random.Range(0, chance);
			if (r > 1) { T.animation.Play("BreatheA"); chance--; }
			else if (r == 0) { T.animation["AtoB"].speed = 1.0F; T.animation.Play("AtoB"); state = 1; chance = CH2; }
			else { T.animation["DeviceTakeOut"].speed = 1.0F; T.animation.Play("DeviceTakeOut"); state = 2; chance = CH3; }
		}
		else if (state == 1 && !T.animation.isPlaying) {
			r = Random.Range(0, chance);
			if (r > 1) { T.animation.Play("BreatheB"); chance--; }
			else if (r == 1) { T.animation["Facepalm"].speed = 1.0F; T.animation.Play("Facepalm"); chance = CH2; }
			else {
				T.animation["AtoB"].speed = -1.0F; T.animation["AtoB"].time = T.animation["AtoB"].length;
				T.animation.Play("AtoB"); state = 0; chance = CH1;
			}
		}
		else if (state == 2 && !T.animation.isPlaying) {
			r = Random.Range(0, chance);
			if (r > 0) { T.animation.Play("DevicePlay"); chance--; }
			else {
				T.animation["DeviceTakeOut"].speed = -1.0F; T.animation["DeviceTakeOut"].time = T.animation["DeviceTakeOut"].length;
				T.animation.Play("DeviceTakeOut"); state = 0; chance = CH1;
			}
		}
		
		s = "DeviceTakeOut";
		if (T.animation.IsPlaying(s)) {
			if (T.animation[s].speed > 0 && T.animation[s].time > 1.8F && !D.activeSelf && !DF.activeSelf) D.SetActive(true);
			else if (T.animation[s].speed > 0 && T.animation[s].time > 4.79F && !DF.activeSelf) { DF.SetActive(true); D.SetActive(false); }
			else if (T.animation[s].speed < 0 && T.animation[s].time < 4.8F && DF.activeSelf) { DF.SetActive(false); D.SetActive(true); }
			else if (T.animation[s].speed < 0 && T.animation[s].time < 1.8F && D.activeSelf) D.SetActive(false);
		}
	}*/
}
