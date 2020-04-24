using UnityEngine;
using System.Collections;

public class OnClick_Jackie : CharacterOnClick {

	DataControlPrologue DC; public GameObject HL;
	float tc; public int phase = 0;

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
		DC.A["Turn"].AddMixingTransform(DC.A.transform.Find("Jackie/master/root/spine/spine-1/chest/DEF-chest-1/DEF-neck"), true);
		DC.A["Turn"].AddMixingTransform(DC.A.transform.Find("Jackie/master/root/spine/spine-1/chest/chest-1/neck"), false);
		DC.A["Turn"].AddMixingTransform(DC.A.transform.Find("Jackie/master/root/spine/spine-1/chest/chest-1/neck/head"), true);
		DC.A["Turn"].layer = 2;
		DC.A["Stand"].wrapMode = WrapMode.Loop; DC.A["Sit"].wrapMode = WrapMode.Loop;
		DC.A["Stand"].speed = 0.02857F; DC.A["Sit"].speed = 0.02F;
	}

	public override void RedHL(bool on) {
		HL.GetComponent<Renderer>().material.SetColor("_TintColor", on ? new Color(1, 0.141F, 0.141F) : new Color(0, 0.463F, 1));
		HL.SetActive(on);
	}
	public override void MsEnter() { if (DC.S.SP.invitedJackieIn) HL.SetActive(true); }
	public override void MsExit() { HL.SetActive(false); }
	void OnMouseEnter() { if (Cursor.visible) MsEnter(); }
	void OnMouseExit() { if (Cursor.visible) MsExit(); }
	void OnMouseDown() {
		if (DC.S.SP.invitedJackieIn) { HL.SetActive(false); phase = 5; }
	}

	void Update() {
		//zoom in to the door
		if (phase == 1) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			DC.Door.Play(); phase = 2; tc = 0;
			DC.S.SP.knockKnock = false; DC.Sound2.Stop();
			DC.Sound.clip = DC.doorOpen; DC.Sound.Play();
			DC.currentColliders.transform.Find("Door").gameObject.SetActive(false);
		}
		else if (phase == 2 && tc <= 1) {
			tc += 0.008333F * Time.deltaTime * 60;
			DC.cam.transform.position = Vector3.Lerp(DC.camPosM, DC.camPosA, Mathf.SmoothStep(0, 1, tc));
			DC.cam.transform.rotation = Quaternion.Lerp(DC.camRotM, DC.camRotA, Mathf.SmoothStep(0, 1, tc));
		}
		else if (phase == 2 && tc > 1) {
			DC.UIC.StartDialogue(DC.DialogueJackie, DC.S.SP.DSJackie, 0, DC.S.SP.nextDIDJackie, false);
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			DC.MPlay(!DC.isAltMusic ? DC.dialogue : DC.dialogueAlt, 0); phase = 0;
		}
		//zoom out of the couch
		else if (phase == 3 && DC.S.inDialogue == -1) {
			DC.A["Turn"].speed = 1.0F; DC.A["Turn"].time = 0;
			DC.A["Turn"].wrapMode = WrapMode.ClampForever; DC.A.Play("Turn");
			DC.UIC.Col(false); DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.MStop(); phase = 4; tc = 0;
		}
		else if (phase == 4 && tc <= 1) {
			tc += 0.01F * Time.deltaTime * 60;
			DC.cam.transform.position = Vector3.Lerp(DC.camPosB, DC.camPosM, Mathf.SmoothStep(0, 1, tc));
			DC.cam.transform.rotation = Quaternion.Lerp(DC.camRotB, DC.camRotM, Mathf.SmoothStep(0, 1, tc));
		}
		else if (phase == 4 && tc > 1) {
			DC.UIC.Col(true); DC.CursorLock(false); DC.bMenu.SetActive(true);
			if (!DC.isAltMusic) DC.MPlay(DC.main, 0); else DC.MPlay(DC.mainAlt, 6.845F);
			phase = 0;
		}
		//zoom back to the couch
		else if (phase == 5) {
			DC.UIC.Col(false); DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.MStop(); phase = 6; tc = 0;
		}
		else if (phase == 6 && tc <= 1) {
			tc += 0.01F * Time.deltaTime * 60;
			DC.cam.transform.position = Vector3.Lerp(DC.camPosM, DC.camPosB, Mathf.SmoothStep(0, 1, tc));
			DC.cam.transform.rotation = Quaternion.Lerp(DC.camRotM, DC.camRotB, Mathf.SmoothStep(0, 1, tc));
		}
		else if (phase == 6 && tc > 1) {
		//if (phase == 6 && tc > 0.8F && !ft) {
			DC.A["Turn"].speed = -1.0F; DC.A["Turn"].time = DC.A["Turn"].length;
			DC.A["Turn"].wrapMode = WrapMode.Once; DC.A.Play("Turn");
			DC.UIC.StartDialogue(DC.DialogueJackie, DC.S.SP.DSJackie, 0, DC.S.SP.nextDIDJackie, false);
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			DC.MPlay(!DC.isAltMusic ? DC.dialogue : DC.dialogueAlt, 0); phase = 0;
		}
	}
}
