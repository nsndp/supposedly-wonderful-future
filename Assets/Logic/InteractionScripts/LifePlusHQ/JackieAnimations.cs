using UnityEngine;
using System.Collections;

public class JackieAnimations : CharacterOnClick {

	public Texture2D[] tex; public GameObject HL, HL2;
	Color[] trc = new Color[] { //13 45 68, 3 16 62, 27 58 39, 19 43 66 x 2, 11 44 60, 0 21 69
		new Color(0.051F, 0.176F, 0.267F), new Color(0.012F, 0.063F, 0.243F), new Color(0.106F, 0.227F, 0.153F),
		new Color(0.075F, 0.169F, 0.259F), new Color(0.075F, 0.169F, 0.259F),
		new Color(0.043F, 0.172F, 0.235F), new Color(0, 0.082F, 0.27F)
	};
	DataControlHub DC;
	SkinnedMeshRenderer scr; Material[] m; Color c;
	float tc, tt; public int r, chance, phase, camPhase = 0;
	bool trigger = false, zoomEvent;

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		DC.JA["BreatheTalk"].AddMixingTransform(DC.JA.transform.Find("Jackie/master/root/spine/spine-1/chest"), false);
		DC.JA["StandBreathe"].AddMixingTransform(DC.JA.transform.Find("Jackie/master/root/spine/spine-1/chest"), false);
		DC.JA["BreatheTalk"].layer = 2; DC.JA["BreatheTalk"].wrapMode = WrapMode.Loop;
		DC.JA["StandBreathe"].layer = 2; DC.JA["StandBreathe"].wrapMode = WrapMode.Loop;
		DC.JA["BreatheTalk"].speed = 0.02F;
		DC.JA["StandBreathe"].speed = 0.02F;

		DC.JA["Sit"].speed = 0.02F;
		scr = DC.JA.transform.Find("Screens").GetComponent<SkinnedMeshRenderer>(); m = scr.materials;
		c = m[1].GetColor("_Color"); c.a = 0; m[1].SetColor("_Color", c);
		c = m[2].GetColor("_TintColor"); c.a = 0; m[2].SetColor("_TintColor", c);
		scr.materials = m;
	}
	public void ChangeColliders() {
		HL = HL2;
		transform.localPosition = new Vector3(-5, 0.01F, 1.5F); transform.localScale = Vector3.one;
		gameObject.AddComponent<BoxCollider>(); gameObject.AddComponent<BoxCollider>();
		var cld = this.GetComponents<BoxCollider>();
		cld[0].center = new Vector3(0.006F, 1.085F, -0.014F); cld[0].size = new Vector3(0.327F, 0.74F, 0.219F);
		cld[1].center = new Vector3(0.02F, 1.61F, 0.033F); cld[1].size = new Vector3(0.14F, 0.31F, 0.17F);
		cld[2].center = new Vector3(-0.11F, 0.58F, 0.023F); cld[2].size = new Vector3(0.11F, 0.28F, 0.11F);
		cld[3].center = new Vector3(-0.18F, 0.217F, 0.017F); cld[3].size = new Vector3(0.12F, 0.45F, 0.073F);
		cld[4].center = new Vector3(0.17F, 0.537F, 0.02F); cld[4].size = new Vector3(0.138F, 0.377F, 0.094F);
		cld[5].center = new Vector3(0.194F, 0.183F, 0.007F); cld[5].size = new Vector3(0.086F, 0.38F, 0.1F);
		cld[6].center = new Vector3(-0.185F, 0.024F, 0.134F); cld[6].size = new Vector3(0.114F, 0.067F, 0.168F);
		cld[7].center = new Vector3(0.19F, 0.029F, 0.135F); cld[7].size = new Vector3(0.093F, 0.078F, 0.166F);

	}

	public override void RedHL(bool on) {
		HL.GetComponent<Renderer>().material.SetColor("_TintColor", on ? new Color(1, 0.141F, 0.141F) : new Color(0, 0.463F, 1));
		HL.SetActive(on);
	}
	public override void MsEnter() {
		if (!DC.S.SH.finishedInteraction && camPhase != 3 && !DC.JA.IsPlaying("Stand2Back")) {
			//Stand2Back is for day 4 when turning back takes longer than zooming out and activating colliders
			//camPhase != 3 is also for day 4 when you zoomed in with letsStare
			HL.SetActive(true); DC.activeHL = HL;
		}
	}
	public override void MsExit() { HL.SetActive(false); DC.activeHL = null; }
	void OnMouseEnter() { if (Cursor.visible) MsEnter(); }
	void OnMouseExit() { if (Cursor.visible) MsExit(); }
	void OnMouseDown() {
		if (!DC.S.SH.finishedInteraction && camPhase != 3 && !DC.JA.IsPlaying("Stand2Back")) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			zoomEvent = false; tc = 0; camPhase = 1;
		}
	}

	void Update() {
		//for trailer
		//if (Input.GetKeyDown(KeyCode.Q)) { DC.camL.localPosition = DC.posB; DC.camL.localRotation = DC.rotB; }
		//if (Input.GetKeyDown(KeyCode.W)) trigger = true;

		if (DC.paused) return;
		if (camPhase == 1 && tc <= 1) {
			DC.camL.localPosition = Vector3.Lerp(DC.pos, DC.S.levelID < 14 ? DC.posB : DC.posA, Mathf.SmoothStep(0, 1, tc));
			DC.camL.localRotation = Quaternion.Lerp(DC.rot, DC.S.levelID < 14 ? DC.rotB : DC.rotA, Mathf.SmoothStep(0, 1, tc));
			tc += (DC.S.levelID == 14 || COMMON.U.textLayout < 2 ? 0.008333333F : 0.00769230F) * Time.deltaTime * 60;
			if (!zoomEvent && tc > 0.4F) {
				if (DC.S.levelID < 14) trigger = true;
				else if (!DC.S.SH.letsStare) {
					DC.JA["Stand2"].time = 0; DC.JA["Stand2"].speed = 1; DC.JA.Play("Stand2");
					//if we had TurnedB = 2 once already, then always make a full turn; turning head is just for introduction
					if (DC.S.SH.JackieTurnedB != 2) { phase = 2; DC.S.SH.JackieTurnedB = 1; }
				}
				zoomEvent = true;
			}
		}
		else if (camPhase == 1 && tc > 1 && DC.S.SH.letsStare) {
			DC.CursorLock(false); DC.bReturn.SetActive(true);
			camPhase = 3; if (DC.S.levelID == 14) trigger = false;
		}
		else if (camPhase == 1 && tc > 1) {
			DC.UIC.StartDialogue(DC.DialogueChats, DC.S.SH.DSJChats, 0, DC.S.SH.nextDIDJChats, DC.S.levelID == 14);
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			camPhase = 2; if (DC.S.levelID == 14) trigger = false;
		}
		else if (camPhase == 2 && DC.S.inDialogue == -1) {
			if (DC.S.levelID == 14 && DC.S.SH.JackieTurnedB == 2) DC.JA.Play("Stand2Back");
			else if (DC.S.levelID == 14) { DC.JA["Stand2"].speed = -1; DC.JA.Play("Stand2"); }
			else { DC.JA.CrossFade("SitFromTalk", 0.3F, PlayMode.StopAll); phase = 15; }
			if (!DC.S.SH.letsStare) {
				DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
				tc = 0; camPhase = 4;
			} else {
				DC.UIC.Col(false); DC.bMenu.SetActive(false); DC.bReturn.SetActive(true);
				camPhase = 3;
			}
		}
		else if (camPhase == 3 && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bReturn.SetActive(false);
			tc = 0; camPhase = 4;
		}
		else if (camPhase == 4 && tc <= 1) {
			DC.camL.localPosition = Vector3.Lerp(DC.S.levelID < 14 ? DC.posB : DC.posA, DC.pos, Mathf.SmoothStep(0, 1, tc));
			DC.camL.localRotation = Quaternion.Lerp(DC.S.levelID < 14 ? DC.rotB : DC.rotA, DC.rot, Mathf.SmoothStep(0, 1, tc));
			tc += 0.008333333F * Time.deltaTime * 60;
		}
		else if (camPhase == 4 && tc > 1) {
			DC.CursorLock(false); DC.UIC.Col(true); DC.bMenu.SetActive(true);
			camPhase = 0;
		}

		//if (COMMON.trailerRecordMode == 3 && Input.GetKeyDown(KeyCode.F)) { phase = 1; DC.JA["Stand1"].speed = 0.75F; }
		//TURNS
		if (phase == 1 && DC.JA["Stand1"].normalizedTime >= 0.25F) {
			DC.JA["Stand1"].speed = 0; phase = 0;
		}
		if (phase == 2 && DC.JA["Stand2"].normalizedTime >= 0.25F) {
			DC.JA["Stand2"].speed = 0; phase = 0;
		}

		//SITTING
		if (phase == 10 && !DC.JA.isPlaying) {
			r = Random.Range(0, chance); //Debug.Log(r);
			if (r <= 1) { DC.JA.Play("SitIdle2"); chance = 10; phase = 11; }
			else if (r <= 7) { DC.JA.Play("SitIdle1"); chance = 10; }
			else { DC.JA.Play("Sit"); chance--; }
		}
		//switch screens
		else if (phase == 11 && DC.JA["SitIdle2"].time >= 0.25F) {
			int newI = Random.Range(1, tex.Length);
			var v = tex[0]; tex[0] = tex[newI]; tex[newI] = v;
			var u = trc[0]; trc[0] = trc[newI]; trc[newI] = u;
			m = scr.materials;
			if (m[1].color.a < 1) {
				m[1].mainTexture = tex[0]; m[1].SetTexture("_EmissionMap", tex[0]);
				trc[0].a = 0; m[2].SetColor("_TintColor", trc[0]);
			} else {
				m[0].mainTexture = tex[0]; m[0].SetTexture("_EmissionMap", tex[0]);
				trc[0].a = 0; m[3].SetColor("_TintColor", trc[0]);
			}
			scr.materials = m;
			phase = 12; tt = 0;
		}
		else if (phase == 12 && tt <= 1) {
			scr.SetBlendShapeWeight(0, tt * 100);
			m = scr.materials;
			c = m[0].color; c.a = 0.78F - (0.78F * tt); m[0].SetColor("_Color", c);
			c = m[1].color; c.a = 0.78F * tt; m[1].SetColor("_Color", c);
			c = m[3].GetColor("_TintColor"); c.a = 0.059F - (0.059F * tt); m[3].SetColor("_TintColor", c);
			c = m[2].GetColor("_TintColor"); c.a = 0.059F * tt; m[2].SetColor("_TintColor", c);
			scr.materials = m;
			tt += 0.02F * Time.deltaTime * 60;
		}
		else if (phase == 12 && tt > 1) {
			scr.SetBlendShapeWeight(0, 0);
			m = scr.materials;
			m[0] = scr.materials[1]; m[1] = scr.materials[0];
			m[2] = scr.materials[3]; m[3] = scr.materials[2];
			scr.materials = m;
			phase = 10;
		}
		//to talk
		else if (trigger && !DC.JA.IsPlaying("SitIdle2")) {
			DC.JA.CrossFade("SitToTalk");
			trigger = false; phase = 13;
		}
		else if (phase == 13 && DC.JA["SitToTalk"].time >= 0.25F) { tt = 0; phase = 14; }
		else if (phase == 14 && tt <= 1) {
			scr.SetBlendShapeWeight(1, tt * 100);
			var m = scr.materials;
			c = m[0].color; c.a = 0.78F - (0.78F * tt); m[0].SetColor("_Color", c);
			c = m[3].GetColor("_TintColor"); c.a = 0.059F - (0.059F * tt); m[3].SetColor("_TintColor", c);
			scr.materials = m;
			tt += 0.02F * Time.deltaTime * 60;
		}
		else if (phase == 14 && tt > 1 && !DC.JA.isPlaying) DC.JA.Play("BreatheTalk");
		//from talk
		else if (phase == 15 && DC.JA["SitFromTalk"].time >= 0.9F) { tt = 0; phase = 16; }
		else if (phase == 16 && tt <= 1) {
			scr.SetBlendShapeWeight(1, 100 - tt * 100);
			var m = scr.materials;
			c = m[0].color; c.a = 0.78F * tt; m[0].SetColor("_Color", c);
			c = m[3].GetColor("_TintColor"); c.a = 0.059F * tt; m[3].SetColor("_TintColor", c);
			scr.materials = m;
			tt += 0.02F * Time.deltaTime * 60;
		}
		else if (phase == 16 && tt > 1 && !DC.JA.isPlaying) phase = 10;
	}
}
