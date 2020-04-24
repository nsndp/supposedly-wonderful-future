using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_Screens : MonoBehaviour {
	DataControlChapter2 DC;
	public SkinnedMeshRenderer Screens; SkinnedMeshRenderer Trace;
	RectTransform Canv; Vector3 CanvFullScale;
	string discussionOriginal; float hOriginal;
	int SZ; int uvI; float uvL, uvR;
	float tc; int phase = 0;
	Vector3 DstPos;

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		Trace = Screens.transform.parent.Find("Trace").GetComponent<SkinnedMeshRenderer>();
		Canv = Screens.transform.parent.Find("MainScreen").GetComponent<RectTransform>();
		CanvFullScale = Canv.localScale;
		discussionOriginal = DC.DataScreens.Discussion;
		hOriginal = Canv.Find("Log").GetComponent<RectTransform>().rect.height;

		if (DC.S.SC2.hackerRevealed) {
			Screens.SetBlendShapeWeight(0, 100); Trace.SetBlendShapeWeight(0, 100);
			Screens.SetBlendShapeWeight(1, 100); Trace.SetBlendShapeWeight(1, 100);
			Screens.transform.parent.Find("HighlightS").GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
			this.GetComponents<BoxCollider>()[2].size = new Vector3(0.85F, 0.26F, 0.01F);
		}

		//screens control
		SZ = DC.S.SC2.screenZoom;
		if (SZ >= 0) {
			if (SZ != 0) { Screens.SetBlendShapeWeight(2, 100); Trace.SetBlendShapeWeight(2, 100); Canv.localScale = Vector3.zero; }
			if (SZ != 1) { Screens.SetBlendShapeWeight(3, 100); Trace.SetBlendShapeWeight(3, 100); }
			if (SZ != 2) { Screens.SetBlendShapeWeight(4, 100); Trace.SetBlendShapeWeight(4, 100); }
			if (SZ != 3) { Screens.SetBlendShapeWeight(5, 100); Trace.SetBlendShapeWeight(5, 100); }
			if (SZ != 4) { Screens.SetBlendShapeWeight(6, 100); Trace.SetBlendShapeWeight(6, 100); }
			if (SZ != 5) { Screens.SetBlendShapeWeight(7, 100); Trace.SetBlendShapeWeight(7, 100); }
			if (SZ != 6 && DC.S.SC2.hackerRevealed) { Screens.SetBlendShapeWeight(1, 0); Trace.SetBlendShapeWeight(1, 0); }
			if (SZ > 0) { Screens.SetBlendShapeWeight(7+SZ, 100); Trace.SetBlendShapeWeight(7+SZ, 100); }
			if (SZ > 0 && COMMON.U.textLayout == 2) { Screens.SetBlendShapeWeight(13+SZ, 100); Trace.SetBlendShapeWeight(13+SZ, 100); }
			if (SZ == 0 && COMMON.U.textLayout == 2) { Screens.SetBlendShapeWeight(20, 100); Trace.SetBlendShapeWeight(20, 100); }
			if (SZ == 0 && COMMON.U.textLayout == 2) Canv.sizeDelta = new Vector2(720, 720);
		}
		//manual control for suspects uv mapping because we resize them to squares for alt layout
		var uvs = Screens.sharedMesh.uv;
		uvs[4].x = 0.501F; uvs[5].x = 0.999F; uvs[6].x = 0.999F; uvs[7].x = 0.501F;
		uvs[8].x = 0.001F; uvs[9].x = 0.499F; uvs[10].x = 0.499F; uvs[11].x = 0.001F;
		uvs[12].x = 0.001F; uvs[13].x = 0.499F; uvs[14].x = 0.499F; uvs[15].x = 0.001F;
		uvs[16].x = 0.501F; uvs[17].x = 0.999F; uvs[18].x = 0.999F; uvs[19].x = 0.501F;
		uvs[20].x = 0.001F; uvs[21].x = 0.499F; uvs[22].x = 0.499F; uvs[23].x = 0.001F;
		uvs[24].x = 0.501F; uvs[25].x = 0.999F; uvs[26].x = 0.999F; uvs[27].x = 0.501F;
		if (SZ > 0) {
			uvI = SZ == 1 ? 20 : (SZ == 2 ? 16 : (SZ == 3 ? 8 : (SZ == 4 ? 4 : (SZ == 5 ? 12 : 24))));
			uvL = (SZ % 2 == 1) ? 0.001F : 0.501F; uvR = (SZ % 2 == 1) ? 0.499F : 0.999F;
			if (COMMON.U.textLayout == 2) { uvs[uvI].x += 0.1094F; uvs[uvI+1].x -= 0.1094F; uvs[uvI+2].x -= 0.1094F; uvs[uvI+3].x += 0.1094F; }
		}
		Screens.sharedMesh.uv = uvs;
		
		TextUpdate();
		if (SZ == 0) {
			if (DC.S.SC2.mScrInd1 != 2) DC.S.SC2.mScrInd3 = 0; //reset page number, because they are different for layouts, but keep expand/hide for profiles
			EventsC2.SetScreenText(DC);
		}
	}

	void TextUpdate() {
		//font size and LR indent depending on layout
		int fs = COMMON.U.fontSize < 6 ? 20 : 24;
		Canv.Find("Profile").GetComponent<Text>().fontSize = fs + 2;
		Canv.Find("Log").GetComponent<Text>().fontSize = fs + 2;
		Canv.Find("Page").GetComponent<Text>().fontSize = fs + 2;
		Canv.Find("CommenterProfile").GetComponent<Text>().fontSize = fs;
		Canv.Find("CommenterHistory").GetComponent<Text>().fontSize = fs;
		var rct = new RectTransform[] {Canv.Find("Profile").GetComponent<RectTransform>(), Canv.Find("Log").GetComponent<RectTransform>(), Canv.Find("Page").GetComponent<RectTransform>(), Canv.Find("CommenterProfile").GetComponent<RectTransform>(), Canv.Find("CommenterHistory").GetComponent<RectTransform>()};
		Vector2 v; for (int i = 0; i < rct.Length; i++) {
			v = rct[i].offsetMin; v.x = COMMON.U.textLayout < 2 ? 50 : 30; rct[i].offsetMin = v;
			v = rct[i].offsetMax; v.x = COMMON.U.textLayout < 2 ? -50 : -30; rct[i].offsetMax = v;
		}
		if (COMMON.U.textLayout == 2) { v = rct[4].offsetMax; v.x = -20; rct[4].offsetMax = v; }

		//adjustments for logs section
		var Tx = Canv.Find("Log").GetComponent<Text>();
		var G = new GUIStyle(); G.font = Tx.font; G.fontSize = Tx.fontSize;
		G.fontStyle = Tx.fontStyle; G.wordWrap = true; G.richText = true;
		var w = COMMON.U.textLayout < 2 ? 1180 : 660; //Tx.GetComponent<RectTransform>().rect.width;
		PageSizeAdjustment(Tx, G);
		CommentSectionAdjustment(G, w);
	}

	public void UIChangeRuntime() {
		SZ = DC.S.SC2.screenZoom;
		if (SZ >= 0) {
			SetDST(); DC.camB.localPosition = DstPos;
			if (COMMON.U.textLayout == 0) DC.UIC.ClassicShiftCh2SCR(true, SZ == 0);
		}
		if (SZ == 0) {
			Screens.SetBlendShapeWeight(20, COMMON.U.textLayout == 2 ? 100 : 0);
			Trace.SetBlendShapeWeight(20, COMMON.U.textLayout == 2 ? 100 : 0);
		}
		else if (SZ > 0) {
			Screens.SetBlendShapeWeight(13+SZ, COMMON.U.textLayout == 2 ? 100 : 0);
			Trace.SetBlendShapeWeight(13+SZ, COMMON.U.textLayout == 2 ? 100 : 0);
			uvI = SZ == 1 ? 20 : (SZ == 2 ? 16 : (SZ == 3 ? 8 : (SZ == 4 ? 4 : (SZ == 5 ? 12 : 24))));
			uvL = (SZ % 2 == 1) ? 0.001F : 0.501F; uvR = (SZ % 2 == 1) ? 0.499F : 0.999F;
			var uvs = Screens.sharedMesh.uv;
			uvs[uvI].x = COMMON.U.textLayout < 2 ? uvL : uvL + 0.1094F;
			uvs[uvI+1].x = COMMON.U.textLayout < 2 ? uvR : uvR - 0.1094F;
			uvs[uvI+2].x = COMMON.U.textLayout < 2 ? uvR : uvR - 0.1094F;
			uvs[uvI+3].x = COMMON.U.textLayout < 2 ? uvL : uvL + 0.1094F;
			Screens.sharedMesh.uv = uvs;
		}
		Canv.sizeDelta = new Vector2(SZ == 0 && COMMON.U.textLayout == 2 ? 720 : 1280, 720);
		TextUpdate();
		if (SZ == 0) {
			if (DC.S.SC2.mScrInd1 != 2) DC.S.SC2.mScrInd3 = 0;
			EventsC2.SetScreenText(DC);
		}
	}

	void OnMouseDown() {
		if (!DC.S.SC2.checkedScreens) {
			DC.S.SC2.checkedScreens = true;
			DC.S.SC2.DSAshley.Locked[94] = false; DC.S.SC2.nothingToTalk_Ashley = false;
			if (DC.S.SC2.DSAshley.Used[48]) { DC.S.SC2.DSAshley.Used[48] = false; DC.S.SC2.DSAshley.Used[96] = false; }
		}
		DC.UIC.StartDialogue(DC.DialogueScreens, DC.S.SC2.DSScreens, 3, DC.S.SC2.nextDID_Screens, false, true);
		if (!DC.S.SC2.phaseTwo) {
			if (DC.S.SC2.briefed) DC.briefedT = DC.BGM.timeSamples; else DC.introT = DC.BGM.timeSamples;
			DC.MChange(DC.logs, 0, DC.logsT);
		}
	}

	//-------------------------------------- ZOOM IN AND OUT --------------------------------------
	void SetDST() {
		DstPos = COMMON.U.textLayout == 2 ? DC.PosSAlt : (SZ == 0 ? DC.PosS0 : (COMMON.U.textLayout == 1 ? DC.PosS16 : DC.PosS16Classic));
		if (DC.S.SC2.deskUnfolded) DstPos += DC.PosSShift;
	}
	public void ZoomIn(int i, GameObject Tt = null, GameObject T2t = null) {
		SZ = i; DC.S.SC2.screenZoom = i; SetDST();
		uvI = SZ == 1 ? 20 : (SZ == 2 ? 16 : (SZ == 3 ? 8 : (SZ == 4 ? 4 : (SZ == 5 ? 12 : 24))));
		uvL = (SZ % 2 == 1) ? 0.001F : 0.501F; uvR = (SZ % 2 == 1) ? 0.499F : 0.999F;
		DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false); tc = 0; phase = 1;
		if (SZ != 0 && !DC.S.SC2.phaseTwo) { DC.logsT = DC.BGM.timeSamples; DC.MStop(); }
		else if (SZ == 6) { DC.fakesT = DC.BGM.timeSamples; DC.MStop(); }
		if (COMMON.U.textLayout == 0) DC.UIC.ClassicShiftCh2SCR(true, SZ == 0);
	}
	public void ZoomOut() {
		SZ = DC.S.SC2.screenZoom; SetDST();
		DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false); tc = 0; phase = 2;
		if (SZ != 0 && !DC.S.SC2.phaseTwo || SZ == 6) {
			if (SZ == 1) DC.sAT = DC.BGM.timeSamples; else if (SZ == 2) DC.sBT = DC.BGM.timeSamples;
			else if (SZ == 3) DC.sCT = DC.BGM.timeSamples; else if (SZ == 4) DC.sDT = DC.BGM.timeSamples;
			else if (SZ == 5) DC.sET = DC.BGM.timeSamples;
			DC.MStop();
		}
		if (COMMON.U.textLayout == 0) DC.UIC.ClassicShiftCh2SCR(false);
	}

	void Update() {
		if (phase == 1 && tc <= 1) {
			tc += 0.0066666666F * (Time.deltaTime * 60);
			DC.camB.position = Vector3.Lerp(DC.PosB, DstPos, Mathf.SmoothStep(0, 1, tc));
			DC.camB.rotation = Quaternion.Lerp(DC.RotB, DC.RotS, Mathf.SmoothStep(0, 1, tc));
			if (tc >= 0.1F && tc < 0.54F) { //minimizing all screens except the active one
				var f = (tc-0.1F)*250;
				if (SZ != 0) Canv.localScale = Vector3.Lerp(CanvFullScale, Vector3.zero, f/100);
				if (SZ != 0) { Screens.SetBlendShapeWeight(2, f); Trace.SetBlendShapeWeight(2, f); }
				if (SZ != 1) { Screens.SetBlendShapeWeight(3, f); Trace.SetBlendShapeWeight(3, f); }
				if (SZ != 2) { Screens.SetBlendShapeWeight(4, f); Trace.SetBlendShapeWeight(4, f); }
				if (SZ != 3) { Screens.SetBlendShapeWeight(5, f); Trace.SetBlendShapeWeight(5, f); }
				if (SZ != 4) { Screens.SetBlendShapeWeight(6, f); Trace.SetBlendShapeWeight(6, f); }
				if (SZ != 5) { Screens.SetBlendShapeWeight(7, f); Trace.SetBlendShapeWeight(7, f); }
				if (SZ != 6 && DC.S.SC2.hackerRevealed) { Screens.SetBlendShapeWeight(1, 100-f); Trace.SetBlendShapeWeight(1, 100-f); }
			}
			if (SZ == 0 && COMMON.U.textLayout == 2 && tc >= 0.4F && tc < 0.74F) { //ideally it's < 0.7F but another 0.04F is to make sure we don't stop at 98.9 or something regardless of deltaTime
				var f = (tc-0.4F)*3.34F;
				Screens.SetBlendShapeWeight(20, f*100); Trace.SetBlendShapeWeight(20, f*100);
				var v = Canv.sizeDelta; v.x = Mathf.Lerp(1280, 720, f); Canv.sizeDelta = v;
			}
			if (SZ > 0 && tc >= 0.4F && tc < 0.74F) { //if a suspect, moving their screen to the center
				var f = (tc-0.4F)*334;
				Screens.SetBlendShapeWeight(7+SZ, f); Trace.SetBlendShapeWeight(7+SZ, f);
				if (COMMON.U.textLayout == 2) { //resizing to a square and adjusting uv maps accordingly
					Screens.SetBlendShapeWeight(13+SZ, f); Trace.SetBlendShapeWeight(13+SZ, f);
					var uvs = Screens.sharedMesh.uv; //print these out to see which one is which number
					uvs[uvI].x = Mathf.Lerp(uvL, uvL + 0.1094F, f/100);
					uvs[uvI+1].x = Mathf.Lerp(uvR, uvR - 0.1094F, f/100);
					uvs[uvI+2].x = Mathf.Lerp(uvR, uvR - 0.1094F, f/100);
					uvs[uvI+3].x = Mathf.Lerp(uvL, uvL + 0.1094F, f/100);
					Screens.sharedMesh.uv = uvs;
				}
			}
		}
		else if (phase == 1 && tc > 1) {
		//if (phase == 1 && tc > 0.8F && !ft) { //for trailer
			DC.CursorLock(false); DC.bMenu.SetActive(true); phase = 0;
			if (SZ == 0) { EventsC2.SetScreenText(DC); DC.UIC.StartDialogue(DC.DialogueScreens, DC.S.SC2.DSScreens, 3, DC.S.SC2.nextDID_Screens, false, true, true); }
			else if (SZ == 1) DC.UIC.StartDialogue(DC.DialogueSuspectA, DC.S.SC2.DSA, 11, DC.S.SC2.nextDID_A, false);
			else if (SZ == 2) DC.UIC.StartDialogue(DC.DialogueSuspectB, DC.S.SC2.DSB, 12, DC.S.SC2.nextDID_B, false);
			else if (SZ == 3) DC.UIC.StartDialogue(DC.DialogueSuspectC, DC.S.SC2.DSC, 13, DC.S.SC2.nextDID_C, false);
			else if (SZ == 4) DC.UIC.StartDialogue(DC.DialogueSuspectD, DC.S.SC2.DSD, 14, DC.S.SC2.nextDID_D, false);
			else if (SZ == 5) DC.UIC.StartDialogue(DC.DialogueSuspectE, DC.S.SC2.DSE, 15, DC.S.SC2.nextDID_E, false);
			else if (SZ == 6) { DC.S.SC2.nextDID_Other = 0; DC.UIC.StartDialogue(DC.DialogueOther, DC.S.SC2.DSOther, 4, 0, false); }
			//music
			if (!DC.S.SC2.phaseTwo) {
				if (SZ == 1) DC.MPlay(DC.sA, 0, DC.sAT);
				else if (SZ == 2) DC.MPlay(DC.sB, 6.672F, DC.sBT);
				else if (SZ == 3) DC.MPlay(DC.sC, 10.353F, DC.sCT);
				else if (SZ == 4) DC.MPlay(DC.sD, 5.359F, DC.sDT);
				else if (SZ == 5) DC.MPlay(DC.sE, 0, DC.sET);
			}
			else if (SZ == 6) DC.MPlay(DC.sF, 0);
		}
		else if (phase == 2 && tc <= 1) {
			tc += 0.0066666666F * (Time.deltaTime * 60);
			DC.camB.position = Vector3.Lerp(DstPos, DC.PosB, Mathf.SmoothStep(0, 1, tc));
			DC.camB.rotation = Quaternion.Lerp(DC.RotS, DC.RotB, Mathf.SmoothStep(0, 1, tc));
			if (SZ == 0 && COMMON.U.textLayout == 2 && tc >= 0.3F && tc < 0.64F) {
				float f = (tc-0.3F)*3.34F;
				Screens.SetBlendShapeWeight(20, (1-f)*100); Trace.SetBlendShapeWeight(20, (1-f)*100);
				var v = Canv.sizeDelta; v.x = Mathf.Lerp(720, 1280, f); Canv.sizeDelta = v;
			}
			if (SZ > 0 && tc >= 0.3F && tc < 0.64F) {
				float f = (0.3F-(tc-0.3F))*334;
				Screens.SetBlendShapeWeight(7+SZ, f); Trace.SetBlendShapeWeight(7+SZ, f);
				if (COMMON.U.textLayout == 2) {
					Screens.SetBlendShapeWeight(13+SZ, f); Trace.SetBlendShapeWeight(13+SZ, f);
					var uvs = Screens.sharedMesh.uv;
					uvs[uvI].x = Mathf.Lerp(uvL, uvL + 0.1094F, f/100);
					uvs[uvI+1].x = Mathf.Lerp(uvR, uvR - 0.1094F, f/100);
					uvs[uvI+2].x = Mathf.Lerp(uvR, uvR - 0.1094F, f/100);
					uvs[uvI+3].x = Mathf.Lerp(uvL, uvL + 0.1094F, f/100);
					Screens.sharedMesh.uv = uvs;
				}
			}
			if (tc >= 0.5F && tc < 0.94F) {
				float f = (0.4F-(tc-0.5F))*250;
				if (SZ != 0) Canv.localScale = Vector3.Lerp(Vector3.zero, CanvFullScale, (100-f)/100);
				if (SZ != 0) { Screens.SetBlendShapeWeight(2, f); Trace.SetBlendShapeWeight(2, f); }
				if (SZ != 1) { Screens.SetBlendShapeWeight(3, f); Trace.SetBlendShapeWeight(3, f); }
				if (SZ != 2) { Screens.SetBlendShapeWeight(4, f); Trace.SetBlendShapeWeight(4, f); }
				if (SZ != 3) { Screens.SetBlendShapeWeight(5, f); Trace.SetBlendShapeWeight(5, f); }
				if (SZ != 4) { Screens.SetBlendShapeWeight(6, f); Trace.SetBlendShapeWeight(6, f); }
				if (SZ != 5) { Screens.SetBlendShapeWeight(7, f); Trace.SetBlendShapeWeight(7, f); }
				if (SZ != 6 && DC.S.SC2.hackerRevealed) { Screens.SetBlendShapeWeight(1, 100-f); Trace.SetBlendShapeWeight(1, 100-f); }
			}
		}
		else if (phase == 2 && tc > 1) {
			if (SZ == 0) DC.UIC.StartDialogue(DC.DialogueScreens, DC.S.SC2.DSScreens, 3, DC.S.SC2.nextDID_Screens, false, true);
			else DC.UIC.Col(true);
			DC.CursorLock(false); DC.bMenu.SetActive(true); phase = 0;
			if (!DC.S.SC2.phaseTwo && SZ != 0) DC.MPlay(DC.briefed, 24.631F, DC.briefedT);
			else if (SZ == 6) DC.MPlay(DC.fakes, 0, DC.fakesT);
			SZ = -1; DC.S.SC2.screenZoom = -1;
		}
	}

	//-------------------------------------- LOGS SUPPORT FUNCTIONS --------------------------------------
	//making logs height the multiple of line length (to precisely calculate the number of pages where we divide by it)
	public void PageSizeAdjustment(Text Tx, GUIStyle G) {
		var lh = G.CalcHeight(new GUIContent("Test"), 1000);
		var h = Mathf.Floor(hOriginal / lh) * lh;
		Tx.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
	}
	//making indent for logs - comment section
	public void CommentSectionAdjustment(GUIStyle G, float w) {
		var s = discussionOriginal.Split('\n');
		var sNew = new System.Text.StringBuilder(discussionOriginal.Length);
		bool flag = false;
		for (int i = 0; i < s.Length; i++) {
			if (s[i] == "") flag = false;
			else if (s[i] != "" && !flag) flag = true;
			else if (s[i] != "" && flag) { //lines that needs to have indentation (responses)
				var sb = new System.Text.StringBuilder(s[i].Length); sb.Append("       ");
				var ss = s[i].Split(' '); var h = G.CalcHeight(new GUIContent(sb.ToString()), w);
				for (int j = 0; j < ss.Length; j++) {
					if (G.CalcHeight(new GUIContent(sb.ToString() + " " + ss[j]), w) > h) {
						sb.Append('\n'); sb.Append("       "); h += h;
					}
					sb.Append(" " + ss[j]);
				}
				s[i] = sb.ToString();
			}
			sNew.Append(s[i]); if (i < s.Length - 1) sNew.Append('\n');
		}
		DC.DataScreens.Discussion = sNew.ToString();
	}
}