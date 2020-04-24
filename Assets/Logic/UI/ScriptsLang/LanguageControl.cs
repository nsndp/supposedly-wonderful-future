using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public static class LanguageControl {

	static Translation DefaultC, DefaultM;
	static Sprite DefaultS;

	private static Translation SaveENG(Translation T) {
		var BU = new Translation();
		BU.PhraseGroups = new PhraseGroup[T.PhraseGroups.Length];
		for (int i = 0; i < T.PhraseGroups.Length; i++) {

			if (T.PhraseGroups[i].Phrases == null) continue;
			BU.PhraseGroups[i] = new PhraseGroup() { Root = T.PhraseGroups[i].Root, Phrases = new Phrase[T.PhraseGroups[i].Phrases.Length] };
			var root = GameObject.Find(T.PhraseGroups[i].Root);
			if (root && T.PhraseGroups[i].Width != null) BU.PhraseGroups[i].Width = Mathf.RoundToInt(root.GetComponent<RectTransform>().sizeDelta.x);

			for (int j = 0; j < T.PhraseGroups[i].Phrases.Length; j++) {
				var p = T.PhraseGroups[i].Phrases[j];
				var b = new Phrase() { Type = p.Type, Path = p.Path };
				BU.PhraseGroups[i].Phrases[j] = b;
				if (root) {
					if (p.Type == 0 || p.Type == 2) {
						var obj = root.transform.Find(p.Path);
						if (obj != null) {
							var t = obj.GetComponent<Text>();
							if (t != null) b.Text = t.text;
							if (p.Scale != null) b.Scale = 1.0F / p.Scale;
							if (p.Shift != null) b.Shift = -p.Shift;
						}
					}
					else if (p.Type == 1) b.Text = TextInScript(root.transform, p.Path, "", false);
				}
			}
		}
		return BU;
	}

	private static string TextInScript(Transform obj, string data, string transVal, bool change) {
		var s = data.Split('.'); if (s.Length < 2) return null;
		var script = obj.GetComponent(s[0]); if (script == null) return null;
		var f = script.GetType().GetField(s[1]); if (f == null || f.FieldType != typeof(string) && f.FieldType != typeof(string[])) return null;
		if (f.FieldType == typeof(string[])) { 
			var a = (string[])f.GetValue(script);
			int i; var res = System.Int32.TryParse(s[2], out i); if (!res) return null;
			if (!change) return a[i];
			a[i] = transVal; f.SetValue(script, a); return null;
		} else {
			var a = (string)f.GetValue(script);
			if (!change) return a;
			a = transVal; f.SetValue(script, a); return null;
		}
	}

	private static void DoOne(Translation T) {
		foreach (var g in T.PhraseGroups) {
			var root = GameObject.Find(g.Root);
			if (root) {
				if (g.Width != null) {
					var v = root.GetComponent<RectTransform>().sizeDelta; v.x = (int)g.Width;
					root.GetComponent<RectTransform>().sizeDelta = v;
				}
				if (g.Phrases == null) continue;
				foreach (var p in g.Phrases) {
					if (p.Type == 0 || p.Type == 2) {
						var obj = root.transform.Find(p.Path);
						if (obj != null) {
							var t = obj.GetComponent<Text>();
							if (t != null) t.text = p.Text;
							if (t != null && p.Scale != null) t.fontSize = Mathf.RoundToInt((float)p.Scale * t.fontSize);
							if (p.Shift != null) obj.GetComponent<RectTransform>().anchoredPosition += new Vector2((float)p.Shift, 0);
						}
					}
					else if (p.Type == 1) TextInScript(root.transform, p.Path, p.Text, true);

					else if (p.Type == 3 || p.Type == 31) {
						var obj = root.transform.Find(p.Path);
						var tm = obj.GetComponent<TextMesh>();
						tm.text = p.Text;
						if (p.Type == 31) tm.anchor = TextAnchor.MiddleCenter;
						if (p.Scale != null) tm.fontSize = Mathf.RoundToInt((float)p.Scale * tm.fontSize);
						if (p.Shift != null) obj.transform.localPosition += new Vector3(0, (float)p.Shift, 0);
					}
				}
			}
		}
	}

	public static string CultureCode(int langID) {
		if (langID == 0) return "en-US";
		if (langID == 1) return "ru-RU";
		return "en-US";
	}

	public static void RestoreENG() {
		if (DefaultC != null) DoOne(DefaultC);
		if (DefaultM != null) DoOne(DefaultM);
		if (DefaultS != null) GameObject.Find("HowToPlay").transform.Find("Panel/1/Img").GetComponent<Image>().sprite = DefaultS;
	}

	private static void LoadImage(string code, string file, int i, string root, string path,
								  bool emits = false, bool normal = false, bool crisp = false) {
		var p = COMMON.dataFolder + "Images/" + code + "/" + file;
		if (File.Exists(p)) {
			var t = new Texture2D(2, 2, TextureFormat.RGBA32, !crisp);
			t.LoadImage(File.ReadAllBytes(p)); if (crisp) t.anisoLevel = 16;
			var o = GameObject.Find(root).transform; if (path != null) o = o.Find(path);
			var r = o.GetComponent<Renderer>();	var m = r.materials;
			if (!normal) m[i].mainTexture = t; else m[i].SetTexture("_BumpMap", t);
			if (emits) m[i].SetTexture("_EmissionMap", t);
			r.materials = m;
		}
	}

	public static void Translate(int langID, int sceneID) {
		var code = ""; if (langID == 1) code = "RU"; else if (langID == 2) code = "ZH";
		if (sceneID == -1) {
			//-------------- MAIN MENU ---------------------
			var TC = Translation.Load(COMMON.dataFolder + "Interface/" + code + "/Common_" + code + ".xml");
			var TM = Translation.Load(COMMON.dataFolder + "Interface/" + code + "/MainMenu_" + code + ".xml");
			//backuping english first - yes, every time, because other languages might have different set of translated fields
			if (TC != null) { DefaultC = SaveENG(TC); DoOne(TC); }
			if (TM != null) { DefaultM = SaveENG(TM); DoOne(TM); }
			var f = COMMON.dataFolder + "Images/" + code + "/HowToPlay.png";
			if (File.Exists(f)) {
				var o = GameObject.Find("HowToPlay").transform.Find("Panel/1/Img").GetComponent<Image>();
				var t = new Texture2D(2, 2); t.LoadImage(File.ReadAllBytes(f));
				if (DefaultS == null) DefaultS = o.sprite;
				o.sprite = Sprite.Create(t, new Rect(0, 0, t.width, t.height), Vector2.zero, 100.0f);
			}
		} else {			
			//---------------- COMMON --------------------
			var TC = Translation.Load(COMMON.dataFolder + "Interface/" + code + "/Common_" + code + ".xml");
			if (TC != null) DoOne(TC);
			//------------- SCENE-SPECIFIC ---------------
			if (sceneID == 6) {
				var TH = Translation.Load(COMMON.dataFolder + "Interface/" + code + "/Hub_" + code + ".xml");
				if (TH != null) DoOne(TH);
				LoadWidget(code);
				LoadAccessPoints(code);
			}
			else if (sceneID == 0) {
				LoadImage(code, "Whiteboard.png", 1, "FrontArea", "Whiteboard/Whiteboard", false, false, true);
				LoadImage(code, "OfficeMouse.png", 2, "FrontArea", "Bookshelf");
				//LoadImage(code, "OfficePhone.png", 0, "WorkArea", "Desks/Phone");
			}
			else if (sceneID == 1) {
				LoadImage(code, "Booklet.jpg", 0, "Livingroom", "Table/Booklet1");
				LoadImage(code, "Booklet.jpg", 0, "Livingroom", "Table/Booklet2");
				LoadImage(code, "TornPaper.png", 0, "Livingroom", "PaperTable");
				LoadImage(code, "Sertraline.png", 0, "Livingroom", "AntidepressantBoxes");
				LoadImage(code, "Motivation.png", 0, "Kitchen", "Words");
				LoadImage(code, "MotivationNM.png", 0, "Kitchen", "Words", false, true);
				LoadImage(code, "DeliveryBox.png", 0, "Kitchen", "Boxes1");
				LoadImage(code, "Calendar.jpg", 0, "BedroomLit", "Calendar");
			}
			else if (sceneID == 2) {
				var T2 = Translation.Load(COMMON.dataFolder + "Interface/" + code + "/Chapter2_" + code + ".xml");
				if (T2 != null) DoOne(T2);
			}
			else if (sceneID == 3) {
				var T3 = Translation.Load(COMMON.dataFolder + "Interface/" + code + "/Chapter3_" + code + ".xml");
				if (T3 != null) DoOne(T3);
			}
			else if (sceneID == 4) {
				LoadImage(code, "Nameplate.png", 0, "Objects", "Table/Nameplate");
				LoadImage(code, "Notebook.png", 0, "Objects", "Notebook");
			}
			else if (sceneID == 5) {
				var T5 = Translation.Load(COMMON.dataFolder + "Interface/" + code + "/Chapter5_" + code + ".xml");
				if (T5 != null) {
					//GameObject.Find() is not finding an inactive object after all
					var cred = GameObject.Find("Data").GetComponent<DataControlChapter5>().Cred.gameObject;
					cred.SetActive(true); DoOne(T5); cred.SetActive(false);
				}
				LoadImage(code, "Sertraline.png", 0, "Antidepressant", null);
				LoadImage(code, "NoConnection.png", 0, "DeskSecond", "Screens", true);
			}
		}
	}

	//------------- HUB-SPECIFIC METHODS ---------------
	private static void LoadWidget(string code) {
		var scr = GameObject.Find("Room").transform.Find("Colliders/Device").GetComponent<OnClick_Device>();
		var m = scr.M;
		for (int i = 1; i <= 4; i++) {
			var p = COMMON.dataFolder + "Images/" + code + "/Widget" + i + ".png";
			if (File.Exists(p)) {
				var t = new Texture2D(2, 2); t.LoadImage(File.ReadAllBytes(p)); t.anisoLevel = 12;
				//IMPORTANT: need to create another instance because modifying materials
				//linked through scripts changes the actual asset (like sharedMaterial behavior)
				var mm = new Material(m[i-1]);
				mm.mainTexture = t; mm.SetTexture("_EmissionMap", t); m[i-1] = mm;
			}
		}
		scr.M = m;
	}
	private static void LoadAccessPoints(string code) {
		var scr = GameObject.Find("Room").transform.Find("Colliders/AccessPoint").GetComponent<AccessPoint>();
		var p1 = COMMON.dataFolder + "Images/" + code + "/AccessPoint1.png";
		var p2 = COMMON.dataFolder + "Images/" + code + "/AccessPoint2.png";
		var p1m = COMMON.dataFolder + "Images/" + code + "/AccessPoint1IM.png";
		var p2m = COMMON.dataFolder + "Images/" + code + "/AccessPoint2IM.png";
		if (File.Exists(p1) && File.Exists(p1m)) {
			var t1 = new Texture2D(2, 2); t1.LoadImage(File.ReadAllBytes(p1)); t1.anisoLevel = 16;
			var t2 = new Texture2D(2, 2); t2.LoadImage(File.ReadAllBytes(p1m));
			//making a new instance (see above) and assigining it to script
			var mm = new Material(scr.ap1);
			mm.mainTexture = t1; mm.SetTexture("_Illum", t2);
			scr.ap1 = mm;
			//also changing the instance that's already assigned to the object at the moment
			var r = GameObject.Find("Room").transform.Find("AccessPoint").GetComponent<Renderer>();
			var rm = r.materials; rm[1] = mm; r.materials = rm;

			var A = new Transform[] {
				GameObject.Find("Lounge").transform.Find("AccessArea"),
				GameObject.Find("LoungeDay").transform.Find("AccessArea")
			};
			foreach (var a in A)
				for (int i = 0; i < a.childCount; i++)
					if (a.GetChild(i).name == "AccessPoint") {
						var m = a.GetChild(i).GetComponent<Renderer>().materials;
						m[1].mainTexture = t1; m[1].SetTexture("_Illum", t2);
						a.GetChild(i).GetComponent<Renderer>().materials = m;
					}
		}
		if (File.Exists(p2) && File.Exists(p2m)) {
			var t1 = new Texture2D(2, 2); t1.LoadImage(File.ReadAllBytes(p2)); t1.anisoLevel = 16;
			var t2 = new Texture2D(2, 2); t2.LoadImage(File.ReadAllBytes(p2m));
			var mm = new Material(scr.ap2);
			mm.mainTexture = t1; mm.SetTexture("_Illum", t2);
			scr.ap2 = mm;
		}
	}
}
