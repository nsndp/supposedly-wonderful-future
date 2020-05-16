using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataControlMainMenu : DataControl {

	public override int[] GetCCID() { return null; }
	public override void UISettingsChanged() { }
	public override void VideoResChanged() { }
	public override void PauseAnimations(bool pause) { }

	Transform[] R; Animation SR; Transform cam;
	Transform L1, L2; RectTransform[] Letter, Rc; RectTransform PlusLine; Text Slogan; Image LbS, SS, Glow;
	float src, dst; int row, nmb; int phase = 0; float tc, scale, targA, targB; Color c; Vector2 v;
	bool loading = false, skip = false;
	Vector3 srcPos; Quaternion srcRot, dstRot;
	int[] lDst = new int[] {-374, -218, -46, 165, 375};
	int[] lSrc = new int[] {51, 0, 52, 53, -4};
	int lSrcY = 0; int lDstY = 85;
	Vector3[] dstPos = new Vector3[] {
		new Vector3(0, 0, -0.59F), new Vector3(0, 0.203F, -0.56F),
		new Vector3(0, 0.382F, -0.455F), new Vector3(0, 0.515F, -0.298F)
	};

	void Start() {
		var isNew = COMMON.LoadUserSettings();
		if (isNew && SteamManager.Initialized && Steamworks.SteamUtils.GetSteamUILanguage() == "russian") COMMON.U.languageID = 1; //for first launch on russian Steam set language to Russian

		MC = GameObject.Find("Interface").transform.Find("Menu").GetComponent<MenuControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		MC.Init(); UIC.enabled = false;
		bMenu = UIC.transform.Find("ButtonMenu").gameObject; bMenu.SetActive(false);
		BGM = transform.Find("Music").GetComponent<AudioSource>(); loopAt = 5.91F;
		BGM.volume = COMMON.U.volM;

		MC.transform.Find("Main/Language").GetComponent<LanguageSelect>().SetOnLoad(COMMON.U.languageID);
		if (COMMON.U.languageID > 0) LanguageControl.Translate(COMMON.U.languageID, -1); //need to happen after MM.Init() because "Main" scaling

		var L = GameObject.Find("Logos"); L1 = L.transform.Find("Tools"); L2 = L.transform.Find("LifePlus");
		LbS = L.transform.Find("BlackScreen").GetComponent<Image>();
		SS = L.transform.Find("BlackScreenSkip").GetComponent<Image>();
		Letter = new RectTransform[] {L2.Find("L").GetComponent<RectTransform>(), L2.Find("I").GetComponent<RectTransform>(), L2.Find("F").GetComponent<RectTransform>(), L2.Find("E").GetComponent<RectTransform>(), L2.Find("+").GetComponent<RectTransform>()};
		Rc = new RectTransform[] {L2.Find("1").GetComponent<RectTransform>(), L2.Find("2").GetComponent<RectTransform>(), L2.Find("3").GetComponent<RectTransform>(), L2.Find("4").GetComponent<RectTransform>(), L2.Find("5").GetComponent<RectTransform>()};
		PlusLine = Letter[4].transform.Find("1").GetComponent<RectTransform>();
		Slogan = L2.Find("Slogan").GetComponent<Text>();
		Glow = L2.Find("Glow").GetComponent<Image>();

		cam = GameObject.Find("Camera").transform; srcPos = cam.localPosition; srcRot = cam.localRotation;
		var Sp = GameObject.Find("Sphere").transform; SR = Sp.GetComponent<Animation>();
		R = new Transform[] {Sp.Find("RowZ"), Sp.Find("RowU1"), Sp.Find("RowU2"), Sp.Find("RowU3") };
		SR["SphereRotation"].normalizedTime = Random.value; SR.Play("SphereRotation");

		if (COMMON.mainMenuSkipLogos) { CursorLock(false); MC.FadeInLevel(false); }
		else {
			if (Screen.width == 1280.0F) scale = 1;
			else {
				scale = Screen.width / 1280.0F;
				var RT = L.transform.GetComponentsInChildren<RectTransform>(true);
				var TX = L.transform.GetComponentsInChildren<Text>(true);
				for (int i = 1; i < RT.Length; i++) {
					//no rounding to int here
					v = RT[i].offsetMin; v.x = v.x * scale; v.y = v.y * scale; RT[i].offsetMin = v;
					v = RT[i].offsetMax; v.x = v.x * scale; v.y = v.y * scale; RT[i].offsetMax = v;
				}
				for (int i = 0; i < TX.Length; i++) TX[i].fontSize = Mathf.RoundToInt(TX[i].fontSize * scale);
			}

			//c = LbS.color; c.a = 1; LbS.color = c; LbS.gameObject.SetActive(true);
			//L2.gameObject.SetActive(false); L1.gameObject.SetActive(true);
			//CursorLock(true); tc = 0; phase = 1;
			L1.gameObject.SetActive(false); L2.gameObject.SetActive(true);
			CursorLock(true); StartCoroutine(Waiting(0.25F, 3)); //phase = 3;
		}
		BGM.Play();

		if (Application.isEditor) Debug.Log(Screen.width + " " + Screen.height);
		//if (Application.isEditor) AnalyzeDialogueLines();
	}

	//go through all dialogue xmls and find two longest lines for each one
	//used to check if it will fit (for font size = 8 maximum chars is ~550)
	private void AnalyzeDialogueLines() {
		var fl = System.IO.Directory.GetFiles(COMMON.dataFolder, "*.xml", System.IO.SearchOption.AllDirectories);
		var ss = new string[fl.Length];
		for (int k = 0; k < fl.Length; k++) {
			ss[k] = fl[k]; TXT F; try { F = TXT.Load(fl[k]); } catch { continue; } if (F.Lines == null) continue;
			int m1 = 0, m2 = 0, i1 = 0, i2 = 0; int? j1 = null, j2 = null;
			for (int i = 0; i < F.Lines.Length; i++) {
				var s = F.Lines[i].Text.Replace("[pause]", "").Split(new string[] {"[clear]"}, System.StringSplitOptions.None);
				for (int j = 0; j < s.Length; j++) {
					if (s[j].Length > m1) {
						m2 = m1; i2 = i1; j2 = j1;
						m1 = s[j].Length; i1 = F.Lines[i].ID; j1 = s.Length == 1 ? null : (int?)j;
					}
					else if (s[j].Length > m2) { m2 = s[j].Length; i2 = F.Lines[i].ID; j2 = s.Length == 1 ? null : (int?)j; }
				}
			}
			var res1 = m1 + " at ID=" + i1 + (j1 == null ? "" : "(" + j1 + ")");
			var res2 = m2 + " at ID=" + i2 + (j2 == null ? "" : "(" + j2 + ")");
			ss[k] = res1 + ", " + res2 + " [" + fl[k] + "]";
		}
		System.Array.Sort(ss);
		for (int u = ss.Length - 1; u > 0; u--) Debug.Log(ss[u]);
	}

	public void LoadScreen(string sn) {
		if (sn == null) S = null; else S = SaveGame.Load(COMMON.saveFolder + sn);
		if (S == null) { row = 1; nmb = 8; } //counting clockwise from the front from 0
		else if (S.levelID == 0) { row = 0; nmb = 2; }
		else if (S.levelID == 1) { row = 2; nmb = 7; }
		else if (S.levelID == 2) { row = 2; nmb = (S.SC2 == null || S.SC2.currentRoom == 0) ? 5 : 9; }
		else if (S.levelID == 3) { row = 0; nmb = 0; } //add chapter 3 screen here
		else if (S.levelID == 4) { row = 0; nmb = 8; }
		else if (S.levelID == 5) { row = 3; nmb = 0; }
		else if (S.SH == null || S.SH.currentRoom == 0 && !S.SH.isNight) { row = 1; nmb = 4; }
		else if (S.SH.currentRoom == 1) { row = 1; nmb = 2; }
		else if (S.levelID == 14) { row = 0; nmb = 6; }
		else { row = 2; nmb = 0; }

		if (row == 0 || row == 2) {
			src = R[row].localRotation.eulerAngles.z - 360;
			dst = -nmb*(row == 0 ? 30 : 36);
			if (src < dst) dst -= 360;
		} else {
			src = R[row].localRotation.eulerAngles.z;
			dst = 360 - nmb*(row == 1 ? 30 : 60);
			if (src > dst) src -= 360;
		}
		SR.Stop(); MC.transform.Find("Main").gameObject.SetActive(false);
		tc = 0; phase = 20; CursorLock(true);
	}

	IEnumerator Waiting(float sec, int ph) {
		phase = 12; yield return new WaitForSeconds(sec); if (phase == 12) phase = ph; //could be not 12 anymore because user pressed skip
	}

	void Update() {
		//SKIP
		if (!skip && phase >= 1 && phase <= 12 && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
			SS.gameObject.SetActive(true); skip = true;
		}
		else if (skip && SS.color.a < 1) { var cc = SS.color; cc.a += 0.04F * Time.deltaTime * 60; SS.color = cc; }
		else if (skip && SS.color.a >= 1) {
			MC.Main.SetActive(true); LbS.gameObject.SetActive(false);
			L1.gameObject.SetActive(false); L2.gameObject.SetActive(false);
			skip = false; phase = 15;
		}
		else if (phase == 15 && SS.color.a > 0) { var cc = SS.color; cc.a -= 0.02F * Time.deltaTime * 60; SS.color = cc; }
		else if (phase == 15 && SS.color.a <= 0) { SS.gameObject.SetActive(false); CursorLock(false); phase = 0; }

		//TOOLS
		if (phase == 1 && c.a > 0) { c.a -= 0.02F * Time.deltaTime * 60; LbS.color = c; }
		else if (phase == 1 && c.a <= 0) StartCoroutine(Waiting(2, 2));
		else if (phase == 2 && c.a < 1) { c.a += 0.02F * Time.deltaTime * 60; LbS.color = c; }

		//LIFE PLUS
		else if (phase == 2 && c.a >= 1) {
			L1.gameObject.SetActive(false); L2.gameObject.SetActive(true); LbS.gameObject.SetActive(false);
			StartCoroutine(Waiting(1, 3));
		}
		else if (phase == 3) {
			targA = Rc[4].anchoredPosition.x - Rc[0].anchoredPosition.x;
			v = Rc[0].sizeDelta; v.x = 0; Rc[0].sizeDelta = v; Rc[0].gameObject.SetActive(true);
			phase = 4; tc = 0;
		}
		else if (phase == 4 && tc <= 1) {
			v = Rc[0].sizeDelta; v.x = Mathf.Lerp(0, targA, tc); Rc[0].sizeDelta = v;
			tc += 0.02F * Time.deltaTime * 60;
		}
		else if (phase == 4 && tc > 1) {
			targA = Rc[1].sizeDelta.y; targB = Rc[2].sizeDelta.y;
			v = Rc[1].sizeDelta; v.y = 0; Rc[1].sizeDelta = v; Rc[1].gameObject.SetActive(true);
			v = Rc[2].sizeDelta; v.y = 0; Rc[2].sizeDelta = v; Rc[2].gameObject.SetActive(true);
			tc = 0; phase = 5;
		}
		else if (phase == 5 && tc <= 1) {
			v = Rc[1].sizeDelta; v.y = Mathf.Lerp(0, targA, tc); Rc[1].sizeDelta = v;
			v = Rc[2].sizeDelta; v.y = Mathf.Lerp(0, targB, tc); Rc[2].sizeDelta = v;
			tc += 0.02F * Time.deltaTime * 60;
		}
		else if (phase == 5 && tc > 1) {
			targA = Rc[4].sizeDelta.x + Rc[4].anchoredPosition.x - Rc[0].anchoredPosition.x;
			targB = Rc[4].sizeDelta.x;
			v = Rc[3].sizeDelta; v.x = 0; Rc[3].sizeDelta = v; Rc[3].gameObject.SetActive(true);
			v = Rc[4].sizeDelta; v.x = 0; Rc[4].sizeDelta = v; Rc[4].gameObject.SetActive(true);
			tc = 0; phase = 6;
		}
		else if (phase == 6 && tc <= 1) {
			v = Rc[0].sizeDelta; v.x = Mathf.Lerp(targA - targB, targA, tc); Rc[0].sizeDelta = v;
			v = Rc[3].sizeDelta; v.x = Mathf.Lerp(0, targB, tc); Rc[3].sizeDelta = v;
			v = Rc[4].sizeDelta; v.x = Mathf.Lerp(0, targB, tc); Rc[4].sizeDelta = v;
			tc += 0.02F * Time.deltaTime * 60;
		}
		else if (phase == 6 && tc > 1) {
			for (int i = 0; i < 5; i++) Rc[i].gameObject.SetActive(false);
			for (int i = 0; i < 5; i++) Letter[i].gameObject.SetActive(true);
			c = Glow.color; c.a = 0; Glow.color = c; Glow.gameObject.SetActive(true);
			tc = 0; phase = 7;
		}
		else if (phase == 7 && tc <= 1) {
			for (int i = 0; i < 5; i++)
				Letter[i].anchoredPosition = new Vector2(
					Mathf.Lerp(lSrc[i]*scale, lDst[i]*scale, Mathf.SmoothStep(0, 1, tc)),
					Mathf.Lerp(lSrcY*scale, lDstY*scale, Mathf.SmoothStep(0, 1, tc))
				);
			PlusLine.anchoredPosition = new Vector2(Mathf.Lerp(8*scale, 0, tc*2), -103*scale);
			PlusLine.sizeDelta = new Vector2(Mathf.Lerp(256*scale, 180*scale, tc*2), 50*scale);
			tc += 0.01F * Time.deltaTime * 60;
			if (tc >= 0.75F) { c.a += 0.04F * Time.deltaTime * 60; Glow.color = c; }
		}
		else if (phase == 7 && tc > 1) StartCoroutine(Waiting(0.2F, 8));
		else if (phase == 8 && c.a > 0) { c.a -= 0.025F * Time.deltaTime * 60; Glow.color = c; }
		else if (phase == 8 && c.a <= 0) {
			c = Slogan.color; c.a = 0; Slogan.color = c; Slogan.gameObject.SetActive(true);
			tc = 0; phase = 9;
		}
		else if (phase == 9 && c.a < 1) { c.a += 0.02F * Time.deltaTime * 60; Slogan.color = c; }
		else if (phase == 9 && c.a >= 1) StartCoroutine(Waiting(0.8F, 10));
		else if (phase == 10) { c = LbS.color; c.a = 0; LbS.color = c; LbS.gameObject.SetActive(true); phase = 11; }
		else if (phase == 11 && c.a < 1) { c.a += 0.02F * Time.deltaTime * 60; LbS.color = c; }
		else if (phase == 11 && c.a >= 1) {
			L2.gameObject.SetActive(false);
			StartCoroutine(Waiting(0.5F, 13));
		}
		else if (phase == 13 && c.a > 0) { c.a -= 0.02F * Time.deltaTime * 60; LbS.color = c; }
		else if (phase == 13 && c.a <= 0) {
			MC.Main.SetActive(true);
			LbS.gameObject.SetActive(false); CursorLock(false); phase = 0;
		}

		//MAIN
		if (phase == 20 && tc <= 1) {
			R[row].localRotation = Quaternion.Euler(new Vector3(0, 0, Mathf.SmoothStep(src, dst, tc)));
			tc += 0.02F * Time.deltaTime * 60;
		}
		else if (phase == 20 && tc > 1) {
			dstRot = Quaternion.Euler(new Vector3(20*row, 0, 0));
			phase = 21; tc = 0;
		}
		else if (phase == 21 && tc <= 1) {
			cam.localPosition = Interpolate(srcPos, dstPos[row], tc); //Vector3.Lerp(srcPos, dstPos[row], Mathf.SmoothStep(0, 1, tc));
			cam.localRotation = Quaternion.Lerp(srcRot, dstRot, Mathf.SmoothStep(0, 1, tc));
			if (!loading && tc >= 0.5F) { MC.LoadLevel(S == null ? 5 : S.levelID); loading = true; }
			tc += 0.01F * Time.deltaTime * 60;
		}
	}

	//same as in TableZoom for DLH, only without x since it's always 0 here
	//for row 3 it's SQRT, for row 0 it's power of 2 (curve in a different direction), for others it's linear
	Vector3 Interpolate(Vector3 src, Vector3 dst, float t) {
		var v = Vector3.zero;
		t = Mathf.SmoothStep(0, 1, t);
		v.y = (Mathf.Lerp(src.y, dst.y, t) - (src.y < dst.y ? src.y : dst.y)) / Mathf.Abs(dst.y - src.y);
		v.z = (Mathf.Lerp(src.z, dst.z, t) - (src.z < dst.z ? src.z : dst.z)) / Mathf.Abs(dst.z - src.z);
		if (row == 0) { v.y = v.y * v.y; v.z = v.z * v.z; }
		else if (row == 3) { v.y = Mathf.Sqrt(v.y); Mathf.Sqrt(v.z); }
		v.y = v.y * Mathf.Abs(dst.y - src.y) + (src.y < dst.y ? src.y : dst.y);
		v.z = v.z * Mathf.Abs(dst.z - src.z) + (src.z < dst.z ? src.z : dst.z);
		return v;
	}
}
