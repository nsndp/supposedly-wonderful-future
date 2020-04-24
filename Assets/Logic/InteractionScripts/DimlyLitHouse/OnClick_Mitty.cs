using UnityEngine;
using System.Collections;

public class OnClick_Mitty : CharacterOnClick {
	DataControlChapter1 DC; public Animation A, HL;
	bool inTalk = false; int CH = 3600; int r, state = 0, chance, repeat;

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		A["Turn"].AddMixingTransform(A.transform.Find("Mitty/master/root/spine/spine-1/chest/DEF-chest-1/DEF-neck"), true);
		A["Turn"].AddMixingTransform(A.transform.Find("Mitty/master/root/spine/spine-1/chest/chest-1/neck"), false);
		A["Turn"].AddMixingTransform(A.transform.Find("Mitty/master/root/spine/spine-1/chest/chest-1/neck/head"), true);
		A["TurnMommy"].AddMixingTransform(A.transform.Find("Mitty/master/root/spine/spine-1/chest/DEF-chest-1/DEF-neck"), true);
		A["TurnMommy"].AddMixingTransform(A.transform.Find("Mitty/master/root/spine/spine-1/chest/chest-1/neck"), false);
		A["TurnMommy"].AddMixingTransform(A.transform.Find("Mitty/master/root/spine/spine-1/chest/chest-1/neck/head"), true);
		A["Breathe"].AddMixingTransform(A.transform.Find("Mitty/master/root/spine/spine-1/chest"), false);

		A["Turn"].layer = 2; A["TurnMommy"].layer = 2; A["Breathe"].layer = 1;
		//A["Idle"].layer = 0; A["Cough"].layer = 0;
		A["Breathe"].wrapMode = WrapMode.Loop; A["Breathe"].speed = 0.02F;
		A["Idle"].wrapMode = WrapMode.ClampForever;
		A["Cough"].wrapMode = WrapMode.Once;
		A.Play("Idle"); A["Idle"].speed = 0; A.Play("Breathe"); chance = CH;
		//A["LegsToPosition"].AddMixingTransform(A.transform.Find("Mitty/master/root/leg_hinge_L/leg_socket_L/thigh_L"), true);
		//A["LegsToPosition"].AddMixingTransform(A.transform.Find("Mitty/master/root/leg_hinge_R/leg_socket_R/thigh_R"), true);
		//A["LegsCycle"].AddMixingTransform(A.transform.Find("Mitty/master/root/leg_hinge_L/leg_socket_L/thigh_L/DEF-shin_01_L"), true);
		//A["LegsCycle"].AddMixingTransform(A.transform.Find("Mitty/master/root/leg_hinge_L/leg_socket_L/thigh_L/shin_L"), true);
		//A["LegsCycle"].AddMixingTransform(A.transform.Find("Mitty/master/root/leg_hinge_R/leg_socket_R/thigh_R/DEF-shin_01_R"), true);
		//A["LegsCycle"].AddMixingTransform(A.transform.Find("Mitty/master/root/leg_hinge_R/leg_socket_R/thigh_R/shin_R"), true);
	}

	public void LoadDuringDialogue() {
		A["Turn"].wrapMode = WrapMode.ClampForever; A["Turn"].speed = 1.0F;
		A["Turn"].time = A["Turn"].length; A.Play("Turn"); inTalk = true;
	}

	public override void RedHL(bool on) {
		HL.GetComponent<Renderer>().material.SetColor("_TintColor", on ? new Color(1, 0.141F, 0.141F) : new Color(0, 0.463F, 1));
		HL.gameObject.SetActive(on);
		if (on) { HL.Stop(); HL["M"].speed = A["Idle"].speed; HL["M"].time = A["Idle"].time; HL.Play("M"); }
	}
	public override void MsEnter() {
		if (!DC.S.SC1.nothingToTalkMitty) {
			HL.gameObject.SetActive(true); DC.activeHL = HL.gameObject;
			HL.Stop(); HL["M"].speed = A["Idle"].speed; HL["M"].time = A["Idle"].time; HL.Play("M");
		}
	}
	public override void MsExit() {
		HL.gameObject.SetActive(false); DC.activeHL = null;
	}
	void OnMouseEnter() { if (Cursor.visible) MsEnter(); }
	void OnMouseExit() { if (Cursor.visible) MsExit(); }
	void OnMouseDown() {
		if (!DC.S.SC1.nothingToTalkMitty) {
			DC.UIC.StartDialogue(DC.DialogueMitty, DC.S.SC1.DSMitty, 0, DC.S.SC1.nextDIDMitty, true);
			A["Turn"].wrapMode = WrapMode.ClampForever; A["Turn"].speed = 1.0F;
			A.Play("Turn"); inTalk = true;
		}
	}

	void Update() {
		if (DC.paused) return;
		if (inTalk && DC.S.inDialogue == -1) {
			A["Turn"].wrapMode = WrapMode.Default; A["Turn"].speed = -1.0F;
			A["Turn"].time = A["Turn"].length; A.Play("Turn");
			inTalk = false;
		}
		if (state == 0) {
			r = Random.Range(0, chance);
			if (r > 0) chance -= Time.deltaTime > 0.025F ? 2 : 1;
			else {
				state = 1; A["Idle"].speed = 1; repeat = Random.Range(4, 8);
				HL.Stop(); HL["M"].speed = 1; HL["M"].time = 0; HL.Play("M");
			}
			//else { state = 3; A.Play("Cough"); }
		}
		else if (state == 1 && A["Idle"].time >= 1.5F) {
			if (repeat == 0) state = 2;
			else { A["Idle"].time = 0.5F; HL["M"].time = 0.5F; repeat--; }
		}
		else if (state == 2 && A["Idle"].time >= 2) {
			A["Idle"].time = 0; A["Idle"].speed = 0;
			state = 0; chance = CH;
		}
		else if (state == 3 && !A.IsPlaying("Cough")) {
			state = 0; chance = CH;
		}
		//if (Input.GetKeyDown(KeyCode.Z)) Switch1();
		//if (Input.GetKeyDown(KeyCode.X)) Switch2();
		//if (Input.GetKeyDown(KeyCode.C)) Switch3();
	}

	public void Switch1() {
		A["Breathe"].time = 0; A.Stop("Breathe");
		A.Play("LieDown"); state = -1;
	}
	public void Switch2() {
		A["TurnMommy"].wrapMode = WrapMode.ClampForever; A.Play("TurnMommy"); state = -1;
	}
	public void Switch3() {
		A["TurnMommy"].time = 0; A.Stop("TurnMommy");
		A["PatMommy"].wrapMode = WrapMode.Loop; A.Play("PatMommy"); state = -1;
	}
}
