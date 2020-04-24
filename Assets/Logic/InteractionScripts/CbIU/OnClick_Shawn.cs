using UnityEngine;
using System.Collections;

public class OnClick_Shawn : CharacterOnClick {
	UIControl UIC; DataControlChapter2 DC;
	public Animation A, HL; public SkinnedMeshRenderer Screens, Trace;
		
	int CH1 = 20, CH2 = 20;
	public int state; int r, prevr, chance;
	string s; bool click = false; float tc;
	int stateScr; float f, t, tStart; int bs1, bs2, bs3, bs4;

	public void LoadDuringDialogue() {
		A["TurnA"].time = A["TurnA"].length; A.Play("TurnA");
		A.Play("BreatheTurned"); state = 31;
	}
	public void Init() {
		UIC = GameObject.Find("Interface/UI").GetComponent<UIControl>();
		DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		A["BreatheBase"].AddMixingTransform(A.transform.Find("Shawn/master/root/spine/spine-1/chest"), false);
		A["BreatheTurned"].AddMixingTransform(A.transform.Find("Shawn/master/root/spine/spine-1/chest"), false);
		A["BreatheBase"].speed = 0.02F; A["BreatheTurned"].speed = 0.02F; A["BreatheThink"].speed = 0.02F;
		A["BreatheBase"].layer = 2; A["BreatheBase"].wrapMode = WrapMode.Loop;
		A["BreatheTurned"].layer = 2; A["BreatheTurned"].wrapMode = WrapMode.Loop;
		//T.animation["Base"].layer = 1;
		A.Play("Base"); A.Play("BreatheBase");
		s = "Base"; state = 0; chance = CH1; stateScr = 0; prevr = 0;
	}

	void HLV() {
		if (A.IsPlaying("Think")) {
			HL.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 0);
			HL.Stop("S1"); HL["S1"].speed = A["Think"].speed; HL["S1"].time = A["Think"].time; HL.Play("S1");
		}
		else if (A.IsPlaying("ThinkScratch")) {
			HL.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
			HL.Stop("S11"); HL["S11"].speed = A["ThinkScratch"].speed; HL["S11"].time = A["ThinkScratch"].time; HL.Play("S11");
		} else {
			HL.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, state == 2 ? 100 : 0);
			HL.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 0);
		}
	}
	public override void RedHL(bool on) {
		HL.GetComponent<Renderer>().material.SetColor("_TintColor", on ? new Color(1, 0.141F, 0.141F) : new Color(0, 0.463F, 1));
		HL.gameObject.SetActive(on); if (on) HLV();
	}
	public override void MsEnter() { 
		if (!DC.S.SC2.nothingToTalk_Shawn && state != 33) { //this state is for when he still keeps turning
			HL.gameObject.SetActive(true); DC.activeHL = HL.gameObject; HLV();
		}
	}
	public override void MsExit() { HL.gameObject.SetActive(false); DC.activeHL = null; }
	void OnMouseEnter() { if (Cursor.visible) MsEnter(); }
	void OnMouseExit() { if (Cursor.visible) MsExit(); }
	void OnMouseDown() {
		if (!DC.S.SC2.nothingToTalk_Shawn && state != 33) {
			Cursor.visible = false; Cursor.lockState = CursorLockMode.Locked;
			DC.UIC.Col(false); DC.bMenu.SetActive(false); click = true;
		}
	}

	void Update() {
		if (DC.paused) return;
		//handling conversation start depending on what Shawn was doing at the time of clicking
		//is it's state = 1 (hand waving), we wait until the animation is over and only then respond
		if (click && state == 0) {
			s = "TurnA"; A[s].speed = 1.2F; A.CrossFade(s, 0.3F, PlayMode.StopAll); //also stops "BreatheBase"
			click = false; state = 3; tc = 0;
		}
		else if (click && state == 2) {
			s = "TurnB"; A[s].speed = 1.2F; A.CrossFade(s);
			click = false; state = 3; tc = 0;
		}
		else if (state == 3) {
			if (tc <= 1) {
				DC.camL.transform.position = Vector3.Lerp(DC.PosA, COMMON.U.textLayout < 2 ? DC.PosShawn : DC.PosShawnAlt, Mathf.SmoothStep(0, 1, tc));
				DC.camL.transform.rotation = Quaternion.Lerp(DC.RotA, COMMON.U.textLayout < 2 ? DC.RotShawn : DC.RotShawnAlt, Mathf.SmoothStep(0, 1, tc));
				tc += 0.01F * Time.deltaTime * 60;
			}
			if (!A.isPlaying) {
				A.Play("BreatheTurned"); state = 31; DC.CursorLock(false); DC.bMenu.SetActive(true);
				UIC.StartDialogue(DC.DialogueShawn, DC.S.SC2.DSShawn, 1, DC.S.SC2.nextDID_Shawn, true);
			}
		}
		else if (state == 31) {
			if (!A.isPlaying) A.Play("BreatheTurned");
			if (DC.S.inDialogue == -1 && !DC.noZoomOut) { //FINISHER
				s = "TurnA"; A[s].speed = -1.0F; A[s].time = A[s].length;
				A.CrossFade(s, 0.3F, PlayMode.StopAll); //also stops "BreatheTurned"
				click = false; state = 32; tc = 0;
				DC.CursorLock(true); DC.bMenu.SetActive(false); DC.UIC.Col(false);
				if (DC.S.SC2.briefedToSet || DC.S.SC2.phaseTwoToSet) DC.MStop();
			}
		}
		else if (state == 32 && tc <= 1) {
			DC.camL.transform.position = Vector3.Lerp(COMMON.U.textLayout < 2 ? DC.PosShawn : DC.PosShawnAlt, DC.PosA, Mathf.SmoothStep(0, 1, tc));
			DC.camL.transform.rotation = Quaternion.Lerp(COMMON.U.textLayout < 2 ? DC.RotShawn : DC.RotShawnAlt, DC.RotA, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01F * Time.deltaTime * 60;
		}
		else if (state == 32 && tc > 1) {
			state = 33; DC.CursorLock(false); DC.bMenu.SetActive(true); DC.UIC.Col(true);
			if (DC.S.SC2.briefedToSet) {
				DC.S.SC2.briefed = true; DC.S.SC2.briefedToSet = false;
				DC.MPlay(DC.briefed, 24.631F);
			}
			else if (DC.S.SC2.phaseTwoToSet) {
				DC.S.SC2.phaseTwo = true; DC.S.SC2.phaseTwoToSet = false;
				DC.MPlay(DC.phaseTwo, 0);
			}
		}
		else if (state == 33 && !A.isPlaying) {
			state = 0; A.Play("Base"); A.Play("BreatheBase"); chance = CH1;
		}
		
		//idle routine
		else if (state == 0 && !A.IsPlaying("Base")) { //where Shawn is typing, and we decide for how long
			r = Random.Range(0, chance);
			if (r != 0) { A.Play("Base"); chance--; }
			else { state = 1; s = null; }
		}
		else if (state == 1) {
			//where Shawn stops typing and can either move screens or go into the thinking state
			//with the former, one of the 8 surrounding screens will change places with the central one, hence 8 options
			//at first we choose at random, then we choose the reversal of the last one (because we need to return
			//screens back to the original position), then we choose at random again, and so on
			//each move include hand waving and screen moving, the latter is animated manually below with stateScr
			//that stateScr block also need to wait for hand waving to stop in case it finishes first,
			//so we give it the current animation name using "s" variable
			r = Random.Range(0, 11);
			//if (r == 0) { state = 0; chance = CH1; }
			if (r == 0 || r > 8) {
				state = 2; chance = CH2;
				s = "Think"; A[s].speed = 1.0F; A.CrossFade(s, 0.3F, PlayMode.StopAll);
				HL.Stop("S1"); HL["S1"].speed = 1; HL["S1"].time = 0; HL.Play("S1");
			}
			else if (prevr == 0) {
				switch (r) {
				case 1: s = "HandR"; A.Play(s); stateScr = 1; tStart = 0.75F; bs1 = 11; bs2 = 14; break;
				case 2: s = "HandL"; A.Play(s); stateScr = 1; tStart = 0.75F; bs1 = 7; bs2 = 10; break;
				case 3: s = "HandU"; A.Play(s); stateScr = 1; tStart = 0.66F; bs1 = 4; bs2 = 12; break;
				case 4: s = "HandD"; A.Play(s); stateScr = 1; tStart = 0.875F; bs1 = 13; bs2 = 21; break;
				case 5: s = "HandRU"; A.Play(s); tStart = 0.75F; stateScr = 3;
					if (Random.Range(0, 2) == 0) { bs1 = 17; bs2 = 19; bs3 = 18; bs4 = 13; }
					else { bs1 = 18; bs2 = 9; bs3 = 17; bs4 = 10; }
					break;
				case 6: s = "HandLU"; A.Play(s); tStart = 0.75F; stateScr = 3;
					if (Random.Range(0, 2) == 0) { bs1 = 22; bs2 = 20; bs3 = 23; bs4 = 13; }
					else { bs1 = 23; bs2 = 16; bs3 = 22; bs4 = 11; }
					break;
				case 7: s = "HandRD"; A.Play(s); stateScr = 3; tStart = 0.875F;
					if (Random.Range(0, 2) == 0) { bs1 = 0; bs2 = 2; bs3 = 1; bs4 = 12; }
					else { bs1 = 1; bs2 = 8; bs3 = 0; bs4 = 10; }
					break;
				case 8: s = "HandLD"; A.Play(s); stateScr = 3; tStart = 0.875F;
					if (Random.Range(0, 2) == 0) { bs1 = 5; bs2 = 3; bs3 = 6; bs4 = 12; }
					else { bs1 = 6; bs2 = 15; bs3 = 5; bs4 = 11; }
					break;
				}
				prevr = r; state = -1; t = 0;
			} else {
				switch (prevr) {
				case 1: s = "HandL"; A.Play(s); stateScr = 2; tStart = 0.75F; break;
				case 2: s = "HandR"; A.Play(s); stateScr = 2; tStart = 0.75F; break;
				case 3: s = "HandD"; A.Play(s); stateScr = 2; tStart = 0.875F; break;
				case 4: s = "HandU"; A.Play(s); stateScr = 2; tStart = 0.66F; break;
				case 5: s = "HandLD"; A.Play(s); stateScr = 4; tStart = 0.875F; break;
				case 6: s = "HandRD"; A.Play(s); stateScr = 4; tStart = 0.875F; break;
				case 7: s = "HandLU"; A.Play(s); stateScr = 4; tStart = 0.75F; break;
				case 8: s = "HandRU"; A.Play(s); stateScr = 4; tStart = 0.75F; break;
				}
				if (prevr > 4) { var v = bs1; bs1 = bs3; bs3 = v; v = bs2; bs2 = bs4; bs4 = v; }
				prevr = 0; state = -1; t = 0;
			}
		}
		else if (state == 2 && !A.IsPlaying(s)) {
			//where Shawn stops to ponder about stuff
			//here we have different breathing animation, which is on the main layer (because we also move hands)
			//after every full breath, he either scratches his head, breathes again,
			//or goes back to typing (which utilizes state 21 on its way there)
			r = Random.Range(0, chance);
			if (r > 3) { s = "BreatheThink"; A.Play(s); chance--; }
			else if (r == 0) { s = "ThinkScratch"; A.Play(s); HL.Stop("S11"); HL.Play("S11"); chance = CH2; }
			else {
				s = "Think"; A[s].speed = -1.0F; A[s].time = A[s].length; A.Play(s); state = 21;
				HL.Stop("S1"); HL["S1"].speed = -1; HL["S1"].time = HL["S1"].length; HL.Play("S1");
			}
		}
		else if (state == 21 && !A.IsPlaying(s)) {
			state = 0; A.Play("Base"); A.Play("BreatheBase"); chance = CH1;
		}

		if (stateScr > 0) t += Time.deltaTime;
		if (stateScr > 0 && t >= tStart) {
			if (stateScr == 1 || stateScr == 2) {
				if (t > tStart + 0.8F && !A.IsPlaying(s)) { f = stateScr == 1 ? 100 : 0; state = 0; chance = CH1; stateScr = 0; }
				else { f = Mathf.Lerp(0, 100, (t - tStart) / 0.8F); if (stateScr == 2) f = 100 - f; }
				Screens.SetBlendShapeWeight(bs1, f); Trace.SetBlendShapeWeight(bs1, f);
				Screens.SetBlendShapeWeight(bs2, f); Trace.SetBlendShapeWeight(bs2, f);
			}
			if (stateScr == 3 || stateScr == 4) {
				if (t > tStart + 0.5F) { f = stateScr == 3 ? 100 : 0; stateScr += 2; }
				else { f = Mathf.Lerp(0, 100, (t - tStart) / 0.5F); if (stateScr == 4) f = 100 - f; }
				Screens.SetBlendShapeWeight(bs1, f); Trace.SetBlendShapeWeight(bs1, f);
				Screens.SetBlendShapeWeight(bs2, f); Trace.SetBlendShapeWeight(bs2, f);
			}
			else if (stateScr == 5 || stateScr == 6) {
				if (t > tStart + 1 && !A.IsPlaying(s)) { f = stateScr == 5 ? 100 : 0; state = 0; chance = CH1; stateScr = 0; }
				else { f = Mathf.Lerp(0, 100, (t - tStart - 0.5F) / 0.5F); if (stateScr == 6) f = 100 - f; }
				Screens.SetBlendShapeWeight(bs3, f); Trace.SetBlendShapeWeight(bs3, f);
				Screens.SetBlendShapeWeight(bs4, f); Trace.SetBlendShapeWeight(bs4, f);
			}
		}
	}
}