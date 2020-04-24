using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NewsControl : MonoBehaviour {
	public NEWS N; DataControlHub D; Transform[] SCR; public Sprite[] storyImages;
	Text Tx, Tx2; string TXT; int pageCount; int[] pageBreaks;
	TextGenerator jSensor; float jW; AccessPoint AP;
	[System.NonSerialized] public string[] storyType = new string[] { "STORY OF THE DAY", "OPINION PIECE" };

	public void Init() {
		D = GameObject.Find("Data").GetComponent<DataControlHub>();
		N = NEWS.Load(D.LOC("News")); //N = NEWS.Load(COMMON.dataFolder + "LifePlusHQ/News.xml");
		int day = D.S.levelID-10;
		//make other screens
		SCR = new Transform[] {transform.GetChild(0), null, null, null};
		SCR[1] = Object.Instantiate(SCR[0]); SCR[1].SetParent(SCR[0].parent, false); SCR[1].name = "News2"; SCR[1].SetSiblingIndex(1);
		SCR[2] = Object.Instantiate(SCR[0]); SCR[2].SetParent(SCR[0].parent, false); SCR[2].name = "News3"; SCR[2].SetSiblingIndex(2);
		SCR[3] = Object.Instantiate(SCR[0]); SCR[3].SetParent(SCR[0].parent, false); SCR[3].name = "News4"; SCR[3].SetSiblingIndex(3);
		SCR[0].localPosition = new Vector3(-0.232F, 0.1475F, -0.1437F); SCR[0].localRotation = Quaternion.Euler(new Vector3(0, -10, 0));
		SCR[1].localPosition = new Vector3(0.232F, 0.1475F, -0.1437F); SCR[1].localRotation = Quaternion.Euler(new Vector3(0, 10, 0));
		SCR[2].localPosition = new Vector3(-0.2222F, -0.1324F, -0.19F); SCR[2].localRotation = Quaternion.Euler(new Vector3(30, -10, 0));
		SCR[3].localPosition = new Vector3(0.2222F, -0.1324F, -0.19F); SCR[3].localRotation = Quaternion.Euler(new Vector3(30, 10, 0));
		for (int i = 0; i < 4; i++) {
			SCR[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
			SCR[i].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
			SCR[i].GetComponent<RectTransform>().localScale = new Vector3(0.4F / Screen.width, 0.4F / Screen.width, 1);
		}
		var ratio = Screen.width * 1.0F / Screen.height;
		var indent = 0.4F / ratio - 0.4F / 1.77777777777777F;
		if (indent > 0.01F) { //adjusting for non-16:9 resolutions
			SCR[0].localPosition += new Vector3(-indent / 8, indent, 0);
			SCR[1].localPosition += new Vector3(indent / 8, indent, 0);
			SCR[2].localPosition += new Vector3(-indent / 8, indent / 2, -indent / 4);
			SCR[3].localPosition += new Vector3(indent / 8, indent / 2, -indent / 4);
		}
		//arrows
		SCR[0].Find("Header/Date/Next").gameObject.SetActive(D.S.levelID > 11);
		SCR[1].Find("Header/Date/Next").gameObject.SetActive(D.S.levelID > 12);
		SCR[2].Find("Header/Date/Next").gameObject.SetActive(D.S.levelID > 13);
		SCR[3].Find("Header/Date/Next").gameObject.SetActive(false);
		SCR[0].Find("Header/Date/Prev").gameObject.SetActive(false);
		SCR[1].Find("Header/Date/Prev").gameObject.SetActive(true);
		SCR[2].Find("Header/Date/Prev").gameObject.SetActive(true);
		SCR[3].Find("Header/Date/Prev").gameObject.SetActive(true);
		//screens that are not unlocked yet
		for (int k = day; k < 4; k++) {
			SCR[k].Find("Body").gameObject.SetActive(false);
			SCR[k].Find("Header").gameObject.SetActive(false);
			SCR[k].Find("Address").gameObject.SetActive(false);
			SCR[k].Find("NotYet").gameObject.SetActive(true);
		}
		//loading data
		for (int k = 0; k < day; k++) {
			SCR[k].Find("Header/Date/Text").GetComponent<Text>().text = N.I[k].Date;
			SCR[k].Find("Body/Panel1/Title").GetComponent<Text>().text = N.I[k].P[0].Title;
			SCR[k].Find("Body/Panel1/Subtitle").GetComponent<Text>().text = N.I[k].P[0].Subtitle;
			SCR[k].Find("Body/Panel1/Text").GetComponent<Text>().text = N.I[k].P[0].Text.Replace("\n\n", "\n\t").Insert(0, "\t");
			SCR[k].Find("Body/Panel2/Title").GetComponent<Text>().text = N.I[k].P[1].Title.Replace("[nl]", "\n");
			SCR[k].Find("Body/Panel2/Subtitle").GetComponent<Text>().text = N.I[k].P[1].Subtitle;
			SCR[k].Find("Body/Panel2/Text").GetComponent<Text>().text = N.I[k].P[1].Text.Replace("\n\n", "\n\t").Insert(0, "\t");
			SCR[k].Find("Body/Panel3/Text").GetComponent<Text>().text = N.I[k].P[2].Text.Replace("\n\n", "\n\t").Insert(0, "\t");
			SCR[k].Find("Body/Panel4/Text").GetComponent<Text>().text = N.I[k].P[3].Text.Replace("\n\n", "\n\t").Insert(0, "\t");
			SCR[k].Find("Body/Panel1/Image").GetComponent<Image>().sprite = storyImages[k];
			if (N.I[k].P[1].Title.Contains("[nl]")) {
				SCR[k].Find("Body/Panel2/Subtitle").GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -65);
				SCR[k].Find("Body/Panel2/Text").GetComponent<RectTransform>().offsetMax = new Vector2(-12, -93);
			}
		}
		//scaling
		if (Screen.width != 1280) for (int k = 0; k < day; k++) {
			var scale = Screen.width / 1280.0F; Vector2 v;
			var R = SCR[k].GetComponentsInChildren<RectTransform>(true);
			var T = SCR[k].GetComponentsInChildren<Text>(true);
			for (int i = 1; i < R.Length; i++) {
				v = R[i].offsetMin; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); R[i].offsetMin = v;
				v = R[i].offsetMax; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); R[i].offsetMax = v;
			}
			for (int i = 0; i < T.Length; i++) T[i].fontSize = Mathf.RoundToInt(T[i].fontSize * scale);
		}
		//initializing expanders, adding ... to the end, justifying
		Canvas.ForceUpdateCanvases();
		for (int k = 0; k < day; k++) for (int i = 1; i <= 4; i++) {
			SCR[k].Find("Body/Panel"+i).GetComponent<NewsExpand>().Init();
			var tx = SCR[k].Find("Body/Panel"+i+"/Text").GetComponent<Text>();
			int j = tx.cachedTextGenerator.characterCountVisible;
			//Debug.Log(k + " " + i + " : " + tx.text.Substring(0, j).Remove(0, j - 30));
			if (j == tx.text.Length) tx.text += " "; //text is already finished - need to have space at the end for proper last line justification
			else if (tx.text[j] == '\n') tx.text = tx.text.Substring(0, j+1); //paragraph end
			else if (tx.text[j-1] != ' ') {
				j -= 3; while (tx.text[j-1] != ' ') j--; j--; //now we at the last symbol before space
				char ch = tx.text[j-1]; if (ch == ',' || ch == ';' || ch == '–') j--;
				tx.text = tx.text.Substring(0, j) + " ... ";
			}
		}
		Canvas.ForceUpdateCanvases();
		for (int k = 0; k < day; k++) for (int i = 1; i <= 4; i++) Justify(SCR[k].Find("Body/Panel"+i+"/Text"));
		jSensor = SCR[0].Find("Body/Panel2/Text").GetComponent<Text>().cachedTextGenerator;
		jW = SCR[0].Find("Body/Panel2/Text").GetComponent<RectTransform>().rect.width - 2;
		AP = D.room.Find("Colliders/AccessPoint").GetComponent<AccessPoint>();
	}

	public void LoadStory(int day, int story) {
		var P = GameObject.Find("News"+(day+1)).transform.Find("Body/PanelExpanded").GetComponent<RectTransform>();
		var FA = P.Find("FullArea").GetComponent<RectTransform>();
		var T = P.Find("FullArea/Text").GetComponent<RectTransform>(); Tx = T.GetComponent<Text>();
		var TypeRect = P.Find("FullArea/Type").GetComponent<RectTransform>();
		var TypeName = P.Find("FullArea/Type/Name").GetComponent<Text>();
		var tt = "";
		if (N.I[day].P[story].Type == 0) tt = "<color=#30A5FF>" + N.I[day].P[story].Title + "</color>\n<color=#0B6BB4>" + N.I[day].P[story].Subtitle + "</color>\n\n";
		else if (N.I[day].P[story].Type == 1) tt = "<color=#30A5FF>" + N.I[day].P[story].Title.Replace("[nl]", " ") + "</color>\n<i>" + N.I[day].P[story].Subtitle + "</i>\n\n";
		TXT = tt + N.I[day].P[story].Text;
		var scl = Screen.width / 1280.0F;
		int f = D.S.SH.newsFontSize; //int f = COMMON.U.fontSize; if (COMMON.U.altLayout && f > 0) f--;
		int fs = f == 3 ? 22 : (f == 2 ? 20 : (f == 1 ? 18 : 16)); fs = Mathf.RoundToInt(fs * scl);
		int indent = f == 3 ? 200 : (f == 2 ? 250 : (f == 1 ? 290 : 320)); indent = Mathf.RoundToInt(indent * scl); 

		if (N.I[day].P[story].Type == 2) TypeRect.gameObject.SetActive(false);
		else {
			TypeName.fontSize = fs; TypeName.text = N.I[day].P[story].Type == 0 ? storyType[0] : storyType[1];
			TypeRect.anchoredPosition = new Vector2(-indent + Mathf.RoundToInt(20*scl), Mathf.RoundToInt(f > 1 ? -18*scl : -14*scl));
			TypeRect.sizeDelta = new Vector2(Mathf.RoundToInt(f == 3 ? 250*scl : (f == 2 ? 210*scl : (f == 1 ? 185*scl : 166*scl))), Mathf.RoundToInt(f > 1 ? 38*scl : 34*scl));
			TypeRect.gameObject.SetActive(true);
		}
		P.gameObject.SetActive(true);
		T.offsetMin = new Vector2(indent, 111); T.offsetMax = new Vector2(-indent, -Mathf.RoundToInt(10*scl));
		P.Find("FullArea/GreyL").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, indent - Mathf.RoundToInt(20*scl));
		P.Find("FullArea/GreyR").GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, indent - Mathf.RoundToInt(20*scl));
		Tx.fontSize = fs; Tx.text = TXT; Canvas.ForceUpdateCanvases();
		FA.offsetMax = new Vector2(0, 0);
		FA.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, T.rect.height - 2 * T.anchoredPosition.y + Mathf.RoundToInt(10*scl));
		P.gameObject.SetActive(false);
	}

	public void Justify(Transform o) {
		var g = o.GetComponent<Text>().cachedTextGenerator;
		var t = o.GetComponent<Text>().text;
		var v = g.verts;
		var w = Mathf.RoundToInt(o.GetComponent<RectTransform>().rect.width - 2);

		for (int i = 0; i < g.lineCount; i++) {
			var k = (i < g.lineCount - 1) ? g.lines[i+1].startCharIdx - 1 : t.Length - 1;
			if (t[k] != '\n') { //unless it's the end of the paragraph
				var ww = w - g.verts[4*k].position.x; //+g.verts[0].position.x; //how many pixels to add for the whole line
				var s = t.Substring(g.lines[i].startCharIdx, k - g.lines[i].startCharIdx).Split(' ');
				var d = ww / (s.Length - 1); //how much distance in pixels to add to every space in the line
				//so, every character before the first space stays in place
				//every character after the first space gets an additional indent of d
				//every character after the second space - an indent of 2*d, and so on
				int l = 0;
				for (int j = g.lines[i].startCharIdx; j <= k; j++) {
					for (int u = 0; u < 4; u++) {
						var vv = v[4*j + u];
						//vv.position = new Vector2(Mathf.Round(vv.position.x + d * l), vv.position.y);
						vv.position = vv.position + new Vector3(Mathf.Round(d * l), 0);
						v[4*j + u] = vv;
					}
					if (t[j] == ' ') l++;
				}
			}
		}

		o.GetComponent<CanvasRenderer>().SetVertices(v as System.Collections.Generic.List<UIVertex>);
	}

	void Update() {
		if (D != null && D.S.SH.currentRoom == 1 && AP.phase > -1) {
			if (jSensor.verts[4*jSensor.lines[1].startCharIdx-1].position.x < jW - 10) { //10 is like a safe margin
				for (int k = 0; k < D.S.levelID-10; k++) for (int i = 1; i <= 4; i++) Justify(SCR[k].Find("Body/Panel"+i+"/Text"));
				if (Application.isEditor) Debug.Log("justify again");
			}
		}
	}

	/*IEnumerator OnApplicationFocus(bool hasFocus) {
		if (D!= null && D.S.SH.currentRoom == 1 && hasFocus) {
			yield return new WaitForEndOfFrame();
			for (int k = 0; k < D.S.levelID-10; k++)
				for (int i = 1; i <= 4; i++)
					Justify(SCR[k].Find("Body/Panel"+i+"/Text"));
		}
	}*/
}