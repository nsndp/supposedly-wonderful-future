using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIControl : MonoBehaviour {

	Color lightBlue = new Color(0.718F, 0.878F, 1);
	Color brightBlue = new Color(0.188F, 0.647F, 1);
	public Color cMain, cActive;
	DataControl DC;
	Image B, NRT, NRT1, NRT2, RSP, DLG, DLG1, DLG2;
	RectTransform RM, RM1, RM2; RectTransform CNT, CommentBlock; RectTransform QSBlock; Text QS;
	Text F, fieldD, fieldD1, fieldD2, Comment; Text[] N, N1, N2; int ni = 0;
	public Text[] R; Text[] R1, R2; public GameObject ArrowU, ArrowD; GameObject ArrowU1, ArrowU2, ArrowD1, ArrowD2;
	float scale; int an; //char[] symbols = new char[] {'➥','▼','↓','→','➔','➜','➞','➥','↳','↷','⇘','»'};
	TXT D; DialogueStructure DS; public int id; bool altLayoutRight, ignoreUsed, rspOnly; int finEv = -1;
	public int phase = 0; string[] line; int k, i, j;
	int[] pauses = new int[10]; int pi, pmax;
	int[] breaks = new int[15]; int bi, bmax;
	int Rtop, Rbottom, Rleft, Rright, Rspacing;
	public int rtop, rcount, viscount, rcur; public bool inHover = false;
	public float wmax; public GUIStyle G;
	public int bPhase = 0; float speed; int evnt = -10; float evntDelay = -1;
	int tPhase = 0; Text TT1, TT2;
	Color cl; Vector2 v;
	public Sprite OverlayU, OverlayAll;

	[System.NonSerialized] public string[] TitleSTR1 = new string[] {
		"PROLOGUE", "CHAPTER 1", "CHAPTER 2", "CHAPTER 3", "CHAPTER 4", "CHAPTER 5"
	};
	[System.NonSerialized] public string[] TitleSTR2 = new string[] {
		"An Unlikely Visitor", "Dimly Lit House", "Cyberbullying Investigation Unit",
		"Elevator Experiment", "1440 minutes", "Black Hole"
	};

	float[] L0W = new float[] {0.3F, 0.26F, 0.23F, 0.2F, 0.17F, 0.1F, 0, 0};
	float[] L0H = new float[] {-0.12F, -0.08F, -0.05F, 0, 0, 0, 0, 0.1F};
	float[] L1W = new float[] {620, 540, 460, 380, 300, 190, 70, 70};
	float[] L2C = new float[] {0.18F, 0.1F, 0.06F, 0, 0, 0, 0, 0};
	float[] L2U = new float[] {0.3F, 0.25F, 0.2F, 0.1F, 0.1F, 0.1F, 0.1F, 0.1F};
	float[] ComInd = new float[] {400, 450, 520, 580, 650, 720, 800, 900};

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControl>();
		B = transform.Find("BlackScreen").GetComponent<Image>();
		TT1 = transform.Find("TitleText1").GetComponent<Text>(); TT2 = transform.Find("TitleText2").GetComponent<Text>();
		CNT = transform.parent.Find("Anim/Continue").GetComponent<RectTransform>(); //CNT.GetComponent<Text>().text = symbols[0].ToString();
		CommentBlock = transform.Find("Comment").GetComponent<RectTransform>(); Comment = CommentBlock.transform.Find("Text").GetComponent<Text>();
		QSBlock = transform.Find("Quicksave").GetComponent<RectTransform>(); QS = QSBlock.transform.Find("Text").GetComponent<Text>();
		NRT1 = transform.Find("Narration").GetComponent<Image>(); NRT2 = transform.Find("NarrationAlt").GetComponent<Image>();
		RSP = transform.Find("Responses").GetComponent<Image>();
		DLG1 = transform.Find("Dialogue").GetComponent<Image>(); DLG2 = transform.Find("DialogueAlt").GetComponent<Image>();
		RM1 = transform.Find("Responses/Main").GetComponent<RectTransform>(); RM2 = transform.Find("DialogueAlt/Responses").GetComponent<RectTransform>();
		ArrowU1 = transform.Find("Responses/ArrowUp").gameObject; ArrowU2 = transform.Find("DialogueAlt/ArrowUp").gameObject;
		ArrowD1 = transform.Find("Responses/ArrowDown").gameObject; ArrowD2 = transform.Find("DialogueAlt/ArrowDown").gameObject;
		fieldD1 = transform.Find("Dialogue/Text").GetComponent<Text>(); fieldD2 = transform.Find("DialogueAlt/Text").GetComponent<Text>();
		R1 = new Text[12]; for (int a = 1; a <= 12; a++) R1[a-1] = transform.Find("Responses/Main/"+a).GetComponent<Text>();
		R2 = new Text[12]; for (int a = 1; a <= 12; a++) R2[a-1] = transform.Find("DialogueAlt/Responses/"+a).GetComponent<Text>();
		N2 = new Text[10]; for (int a = 1; a <= 10; a++) N2[a-1] = transform.Find("NarrationAlt/Text"+a).GetComponent<Text>();
		N1 = new Text[1]; N1[0] = transform.Find("Narration/Text").GetComponent<Text>();

		if (Screen.width == 1280.0F) scale = 1;
		else { //SCALING
			scale = Screen.width / 1280.0F;
			var RT = transform.GetComponentsInChildren<RectTransform>(true);
			var TX = transform.GetComponentsInChildren<Text>(true);
			for (int i = 1; i < RT.Length; i++) {
				v = RT[i].offsetMin; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); RT[i].offsetMin = v;
				v = RT[i].offsetMax; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); RT[i].offsetMax = v;
			}
			for (int i = 0; i < TX.Length; i++) TX[i].fontSize = Mathf.RoundToInt(TX[i].fontSize * scale);
		}

		InitSettings(false); SetTransparency(); SetEffects();
		if (Application.isEditor) Debug.Log(Screen.width + " " + Screen.height);
	}

	private Vector2 GetSidelongAnchors(bool isMin, bool isRight) {
		var f = COMMON.U.fontSize; var L2E = 0.04F;
		if (isMin) return new Vector2(!isRight ? L2E : 0.5F + L2C[f], L2U[f]);
		return new Vector2(!isRight ? 0.5F - L2C[f] : 1 - L2E, 0.9F);
	}

	//test method for finding lines that are most likely to go off-screen when using big font setting
	public void GetLongestLine(string name, TXT file) {
		var max = 0; var id = 0; var segment = 0;
		foreach (var l in file.Lines) {
			var p = l.Text.Replace("[pause]", "").Split(new string[] {"[clear]"}, System.StringSplitOptions.None);
			for (int i = 0; i < p.Length; i++) if (p[i].Length > max) { max = p[i].Length; id = l.ID; segment = i; }
		}
		Debug.Log(name + ": " + max + ", ID = " + id + (segment == 0 ? "" : " (p = " + segment + ")"));
	}

	public void ClassicShiftCh2SCR(bool setOn, bool SZ0 = false) {
		var f = COMMON.U.fontSize;
		var shift = !setOn && f < 6 ? Mathf.RoundToInt(20 * scale) : 0;
		var anchor = !setOn ? 0.4F + L0H[f] : (SZ0 ? 0.27F : 0.35F);
		DLG2.GetComponent<RectTransform>().offsetMin = new Vector2(0, shift);
		DLG2.GetComponent<RectTransform>().offsetMax = new Vector2(0, shift);
		DLG2.GetComponent<RectTransform>().anchorMax = new Vector2(1 - L0W[f], anchor);
	}

	public void InitSettings(bool fromChangedSettings) {
		var U = COMMON.U;
		NRT1.gameObject.SetActive(false); NRT2.gameObject.SetActive(false);
		DLG1.gameObject.SetActive(false); DLG2.gameObject.SetActive(false);
		RSP.gameObject.SetActive(false); CNT.gameObject.SetActive(false);
		if (U.textLayout == 1) { NRT = NRT1; DLG = DLG1; RM = RM1; fieldD = fieldD1; N = N1; R = R1; ArrowU = ArrowU1; ArrowD = ArrowD1; }
		else { NRT = NRT2; DLG = DLG2; RM = RM2; fieldD = fieldD2; N = N2; R = R2; ArrowU = ArrowU2; ArrowD = ArrowD2; }
		if (U.textLayout == 1) { cMain = Color.white; cActive = lightBlue; }
		else { cMain = lightBlue; cActive = brightBlue; }

		//font size
		//int fs = 14 + U.fontSize * 2 + (U.textLayout == 0 ? 2 : 0);
		int fs = 8 + U.fontSize * 2; //8-10-12-14-16-18-20-22
		fs = Mathf.RoundToInt(fs * scale);
		N1[0].fontSize = fs; for (int a = 0; a < N2.Length; a++) N2[a].fontSize = fs;
		fieldD1.fontSize = fs; for (int a = 0; a < R1.Length; a++) R1[a].fontSize = fs;
		fieldD2.fontSize = fs; for (int a = 0; a < R2.Length; a++) R2[a].fontSize = fs;
		Comment.GetComponent<Text>().fontSize = fs;

		var arrowScl = U.fontSize > 5 ? 1 : (U.fontSize > 3 ? 0.85F : (U.fontSize > 1 ? 0.7F : 0.55F));
		var arrowSize = new Vector2(Mathf.RoundToInt(19 * arrowScl * scale), Mathf.RoundToInt(10 * arrowScl * scale));
		ArrowU.GetComponent<RectTransform>().sizeDelta = arrowSize;
		ArrowD.GetComponent<RectTransform>().sizeDelta = arrowSize;

		//to account for when switching happens after pausing during responses appearing one-by-one
		for (int i = 0; i < 12; i++) { cl = R[i].color; cl.a = 1; R[i].color = cl; }
		cl = ArrowD.GetComponent<Image>().color; cl.a = 1; ArrowD.GetComponent<Image>().color = cl;
		cl = ArrowU.GetComponent<Image>().color; cl.a = 1; ArrowU.GetComponent<Image>().color = cl;

		Rtop = U.textLayout == 1 ? 20 : (U.fontSize > 3 ? 15 : 10); Rbottom = 15;
		Rleft = 30; if (U.textLayout == 1 || U.textLayout == 0 && U.fontSize > 5) Rleft = 50; Rright = Rleft;
		Rspacing = U.fontSize > 3 ? 8 : 6;
		if (scale < 1) {
			Rtop = Mathf.RoundToInt(Rtop * scale); Rbottom = Mathf.RoundToInt(Rbottom * scale);
			Rleft = Mathf.RoundToInt(Rleft * scale); Rright = Mathf.RoundToInt(Rright * scale);
		}
		Rspacing = Mathf.RoundToInt(Rspacing * scale);

		if (U.textLayout == 1) {
			//largest font: changing things so it can fit, resetting to default otherwise
			if (U.fontSize == 7) { fieldD.lineSpacing = 0.95F; N1[0].lineSpacing = 0.95F; } 
			else { fieldD.lineSpacing = 1; N1[0].lineSpacing = 1; }
			v = fieldD1.GetComponent<RectTransform>().offsetMin;
			if (U.fontSize == 7) v.y = Mathf.RoundToInt(20 * scale); else v.y = Mathf.RoundToInt(25 * scale);
			fieldD1.GetComponent<RectTransform>().offsetMin = v;
			N1[0].GetComponent<RectTransform>().offsetMin = v;
			//smaller fonts: shortening the lines
			int indent = Mathf.RoundToInt(L1W[U.fontSize] * scale);	Rright = indent;
			v = fieldD1.GetComponent<RectTransform>().offsetMax; v.x = -indent;
			fieldD1.GetComponent<RectTransform>().offsetMax = v;
			N1[0].GetComponent<RectTransform>().offsetMax = v;
		} else {
			Vector2 oMin, oMax, aMin, aMax, oMinN, oMaxN, aMinN, aMaxN;
			if (U.textLayout == 0) {
				int fft = Mathf.RoundToInt(20*scale);
				oMin = new Vector2(0, U.fontSize < 6 ? fft : 0); oMax = oMin;
				aMin = new Vector2(0 + L0W[U.fontSize], 0);
				aMax = new Vector2(1 - L0W[U.fontSize], 0.4F + L0H[U.fontSize]);
				oMinN = oMin; oMaxN = oMax; aMinN = aMin; aMaxN = new Vector2(aMax.x, aMax.y - 0.1F);
			} else {
				oMin = Vector2.zero; oMax = oMin; oMinN = oMin; oMaxN = oMax;
				aMin = GetSidelongAnchors(true, altLayoutRight); aMax = GetSidelongAnchors(false, altLayoutRight);
				aMinN = GetSidelongAnchors(true, false); aMaxN = GetSidelongAnchors(false, false);
			}
			var DRect = DLG2.GetComponent<RectTransform>(); var NRect = NRT2.GetComponent<RectTransform>();
			DRect.offsetMin = oMin; DRect.offsetMax = oMax; DRect.anchorMin = aMin; DRect.anchorMax = aMax;
			NRect.offsetMin = oMinN; NRect.offsetMax = oMaxN; NRect.anchorMin = aMinN; NRect.anchorMax = aMaxN;
			v = fieldD2.GetComponent<RectTransform>().offsetMin; v.x = Rleft; fieldD2.GetComponent<RectTransform>().offsetMin = v;
			v = fieldD2.GetComponent<RectTransform>().offsetMax; v.x = -Rright; fieldD2.GetComponent<RectTransform>().offsetMax = v;
			v = N2[0].GetComponent<RectTransform>().offsetMin; v.x = Rleft; N2[0].GetComponent<RectTransform>().offsetMin = v;
			v = N2[0].GetComponent<RectTransform>().offsetMax; v.x = -Rright; N2[0].GetComponent<RectTransform>().offsetMax = v;
			DLG2.sprite = U.textLayout == 0 && U.fontSize > 5 ? OverlayU : OverlayAll; NRT2.sprite = DLG2.sprite;
		}

		wmax = RM.rect.width - Rleft - Rright;
		G = new GUIStyle(); G.font = R[0].font; G.fontSize = R[0].fontSize;
		G.fontStyle = R[0].fontStyle; G.wordWrap = true; G.richText = true;
		CNT.sizeDelta = new Vector2(G.lineHeight, G.lineHeight);

		//quicksave message size should be like hint size, not dependant on settings, only on resolution
		G.fontSize = QS.fontSize;
		v = G.CalcSize(new GUIContent(QS.text));
		QSBlock.sizeDelta = new Vector2(v.x + Mathf.RoundToInt(40 * scale), v.y + Mathf.RoundToInt(20 * scale));
		G.fontSize = R[0].fontSize;

		if (fromChangedSettings) {
			DC.UISettingsChanged();
			if (DC.S.inDialogue >= 0) StartDialogue(D, DS, DC.S.inDialogue, id, altLayoutRight, ignoreUsed, rspOnly);
			if (DC.S.inNarration) StartNarration(DC.S.NID);
		}
	}

	public void SetEffects() {
		bool shadowFlag = COMMON.U.textEffects == 1 || COMMON.U.textEffects == 3;
		bool outlineFlag = COMMON.U.textEffects == 2 || COMMON.U.textEffects == 3;
		Color shadowClr = COMMON.U.textEffects == 1 ? new Color(0,0,0,1) : new Color(0,0,0,0.5F);
		Color outlineClr = COMMON.U.textEffects == 2 ? new Color(0,0,0,0.75F) : new Color(0,0,0,0.5F);
		fieldD1.GetComponent<Shadow>().effectColor = shadowClr; fieldD1.GetComponent<Shadow>().enabled = shadowFlag;
		fieldD1.GetComponent<Outline>().effectColor = outlineClr; fieldD1.GetComponent<Outline>().enabled = outlineFlag;
		fieldD2.GetComponent<Shadow>().effectColor = shadowClr; fieldD2.GetComponent<Shadow>().enabled = shadowFlag;
		fieldD2.GetComponent<Outline>().effectColor = outlineClr; fieldD2.GetComponent<Outline>().enabled = outlineFlag;
		N1[0].GetComponent<Shadow>().effectColor = shadowClr; N1[0].GetComponent<Shadow>().enabled = shadowFlag;
		N1[0].GetComponent<Outline>().effectColor = outlineClr; N1[0].GetComponent<Outline>().enabled = outlineFlag;
		Comment.GetComponent<Shadow>().effectColor = shadowClr; Comment.GetComponent<Shadow>().enabled = shadowFlag;
		Comment.GetComponent<Outline>().effectColor = outlineClr; Comment.GetComponent<Outline>().enabled = outlineFlag;
		for (int i = 0; i < R1.Length; i++) {
			R1[i].GetComponent<Shadow>().effectColor = shadowClr; R1[i].GetComponent<Shadow>().enabled = shadowFlag;
			R1[i].GetComponent<Outline>().effectColor = outlineClr; R1[i].GetComponent<Outline>().enabled = outlineFlag;
			R2[i].GetComponent<Shadow>().effectColor = shadowClr; R2[i].GetComponent<Shadow>().enabled = shadowFlag;
			R2[i].GetComponent<Outline>().effectColor = outlineClr; R2[i].GetComponent<Outline>().enabled = outlineFlag;
		}
		for (int i = 0; i < N2.Length; i++) {
			N2[i].GetComponent<Shadow>().effectColor = shadowClr; N2[i].GetComponent<Shadow>().enabled = shadowFlag;
			N2[i].GetComponent<Outline>().effectColor = outlineClr; N2[i].GetComponent<Outline>().enabled = outlineFlag;
		}
	}
	public void SetTransparency() {
		if (phase != 1) {
			NRT1.color = new Color(0, 0, 0, COMMON.U.overlayAlpha);
			NRT2.color = NRT1.color; DLG1.color = NRT1.color; DLG2.color = NRT1.color;
			RM1.parent.GetComponent<Image>().color = NRT1.color;
			CommentBlock.GetComponent<Image>().color = NRT1.color;
		}
	}

	IEnumerator Waiting(float sec, int ph) { yield return new WaitForSeconds(sec); phase = ph; }
	IEnumerator WaitingT(float sec, int ph) { yield return new WaitForSeconds(sec); tPhase = ph; }
	IEnumerator WaitingB(float sec, int ph) { bPhase = -1; yield return new WaitForSeconds(sec); bPhase = ph; }
	IEnumerator Delayed(float sec, int eventID) { yield return new WaitForSeconds(sec); Events.Trigger(DC.S, eventID); }
	public void DelayedTrigger(float sec, int eventID) { StartCoroutine(Delayed(sec, eventID)); } 
	public void Col(bool en) {
		foreach (Collider c in DC.currentColliders.GetComponentsInChildren<Collider>()) c.enabled = en;
		if (DC.activeHL != null) { DC.activeHL.SetActive(false); DC.activeHL = null; }
	}

	public void HintOff() {
		var H = transform.Find("Hint");
		H.transform.Find("TextM").gameObject.SetActive(false);
		H.transform.Find("TextR").gameObject.SetActive(false);
		H.gameObject.SetActive(false);
	}
	public void HintOn(int i) {
		var H = transform.Find("Hint");
		var T = H.transform.Find(i == 1 ? "TextM" : "TextR").GetComponent<Text>();
		var S = new GUIStyle(); S.wordWrap = true; S.richText = true;
		S.font = T.font; S.fontSize = T.fontSize; S.fontStyle = T.fontStyle;
		var v = S.CalcSize(new GUIContent(T.text));
		H.GetComponent<RectTransform>().sizeDelta = new Vector2(v.x + Mathf.RoundToInt(10 * scale), v.y + Mathf.RoundToInt(10 * scale));
		T.gameObject.SetActive(true);
		H.gameObject.SetActive(true);
	}

	public void HideComment() { CommentBlock.gameObject.SetActive(false); }
	public void DisplayComment(string s) {
		var wm = ComInd[COMMON.U.fontSize]*scale;
		var w = G.CalcSize(new GUIContent(s)).x;
		var h = w <= wm ? G.lineHeight : G.CalcHeight(new GUIContent(s), wm);
		if (w > wm) w = wm;
		var v1 = Comment.GetComponent<RectTransform>().offsetMin;
		var v2 = Comment.GetComponent<RectTransform>().offsetMax;
		CommentBlock.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.CeilToInt(w + v1.x - v2.x + 5*scale)); //last one is a safety margin so we don't cut it pixel-to-pixel
		CommentBlock.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Mathf.CeilToInt(h + v1.y - v2.y));
		Comment.text = s;
		CommentBlock.gameObject.SetActive(true);
	}

	public void StartDialogue(TXT DialogueText, DialogueStructure DialogueStruct, int npcID, int DID, bool altLayoutRight1, bool ignoreUsed1 = false, bool rspOnly1 = false) {
		//if (COMMON.trailerRecordMode == 1) { transform.Find("ForTrailer").GetComponent<TrailerRecord>().Go(); return; }
		//else if (COMMON.trailerRecordMode == 2) transform.Find("ForTrailer").GetComponent<TrailerRecord>().GoDelayed(); 
		D = DialogueText; DS = DialogueStruct; DC.S.inDialogue = npcID; id = DID; altLayoutRight = altLayoutRight1; ignoreUsed = ignoreUsed1; rspOnly = rspOnly1;
		cl = DLG.color; cl.a = 0; DLG.color = cl; DLG.gameObject.SetActive(!rspOnly || COMMON.U.textLayout != 1);
		cl = RSP.color; cl.a = 0; RSP.color = cl; RSP.gameObject.SetActive(COMMON.U.textLayout == 1);
		for (int a = 0; a < R.Length; a++) R[a].gameObject.SetActive(false);
		if (COMMON.U.textLayout == 2) {
			var RT = DLG.GetComponent<RectTransform>();
			RT.anchorMin = GetSidelongAnchors(true, altLayoutRight);
			RT.anchorMax = GetSidelongAnchors(false, altLayoutRight);
		}
		ArrowU.SetActive(false); ArrowD.SetActive(false);
		Col(false); F = fieldD;
		//logic
		line = D.GetLine(DID).Split(new string[] {"[clear]"}, System.StringSplitOptions.None);
		k = 0; PrepareLine(); phase = 1;
	}
	public void StartNarration(int NID) {
		DC.S.NID = NID; id = NID;
		NRT.gameObject.SetActive(true); cl = NRT.color; cl.a = 0; NRT.color = cl;
		Col(false); DC.S.inNarration = true;
		for (int a = 1; a <= ni; a++) N2[a].gameObject.SetActive(false);
		F = N[0]; ni = 0;
		//logic

		//for narrations [clear] is only necessary under these specific conditions
		var l = DC.Narration.GetLine(id); if (COMMON.U.languageID == 0 || COMMON.U.textLayout != 1 || COMMON.U.fontSize <= 5) l = l.Replace("[clear]", "[pause] ");
		line = l.Split(new string[] {"[clear]"}, System.StringSplitOptions.None);

		k = 0; PrepareLine(); phase = 1; //StartCoroutine(Waiting(0.5F, 1));
	}

	void PrepareLine() {
		//preparing pauses
		if (COMMON.U.textAnim < 2) line[k] = line[k].Replace("[pause]", ""); //no pauses if no animation
		pmax = 0; pi = line[k].IndexOf("[pause]");
		while (pi != -1) { pauses[pmax] = pi; pmax++; line[k] = line[k].Remove(pi, 7); pi = line[k].IndexOf("[pause]"); }
		//preparing line breaks (semi-manual word-wrapping, sunce the automatic one can't tell when the next word won't fit until it already tried)
		F.text = line[k]; Canvas.ForceUpdateCanvases(); bmax = F.cachedTextGenerator.lineCount - 1;
		for (int a = 1; a <= bmax; a++) breaks[a-1] = F.cachedTextGenerator.lines[a].startCharIdx;
		//initializing
		F.text = ""; j = 0; i = 0; pi = 0; bi = 0;
		//Debug.Log(line[k]);
		//string s1 = ""; for (int kk = 0; kk < pmax; kk++) s1 += pauses[kk] + ", "; Debug.Log(s1);
		//string s2 = ""; for (int kk = 0; kk < bmax; kk++) s2 += breaks[kk] + ", "; Debug.Log(s2);
	}
	public void SetResponsesVisibility() {
		var hmax = RM.rect.height - Rtop - Rbottom;
		int H = 0; viscount = 0;
		for (int a = 0; a < rcount; a++) {
			if (a < rtop) { R[a].gameObject.SetActive(false); R[a].GetComponent<Text>().color = cMain; }
			else {
				//responses are still set to vertical overflow because lineHeight returns 1 pixel less
				//for some unknown reason, and also just because it's safer
				if (a != rtop) H += Rspacing; //start with the spacing from the previous line (unless it's the top one)
				var l = Mathf.RoundToInt(G.CalcHeight(new GUIContent(R[a].text), wmax) / G.lineHeight);
				//Debug.Log(l + " " + G.lineHeight + " " + R[0].lineSpacing);
				int fl = Mathf.RoundToInt(G.lineHeight); //first line height (which will never have fractions, as far as I can tell)
				var x = fl*R[0].lineSpacing; //all the following line heights (round, not ceil, but ceil if ends in 0.5 [the standard behavior is not, it's bankers' rounding])
				int nl = Mathf.Approximately(x - Mathf.Floor(x), 0.5F) ? Mathf.CeilToInt(x) : Mathf.RoundToInt(x);
				int h = fl + (l-1)*nl; //line spacing affect line heights from 2nd onward, so the first one will be a bit larger
				var w = l > 1 ? wmax : G.CalcSize(new GUIContent(R[a].text)).x;
				R[a].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
				R[a].GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
				R[a].GetComponent<RectTransform>().anchoredPosition = new Vector2(Rleft, -Rtop-H);
				H += h;
				if (H <= hmax) { R[a].gameObject.SetActive(true); viscount++; }
				else { R[a].gameObject.SetActive(false); R[a].GetComponent<Text>().color = cMain; }
			}
		}
		ArrowU.SetActive(rtop > 0); if (rtop == 0) ArrowU.GetComponent<Image>().color = cMain;
		ArrowD.SetActive(H > hmax); if (H <= hmax) ArrowD.GetComponent<Image>().color = cMain;
	}
	public void ClearSelection() {
		if (rcur != -1) { R[rcur].color = cMain; rcur = -1; }
	}

	void Update() {
		if (DC.paused) return;

		//testing layouts
		/*if (Input.GetKey(KeyCode.LeftShift)) {
			if (Input.GetKeyDown(KeyCode.Q)) {
				COMMON.U.textLayout = COMMON.U.textLayout + 1; if (COMMON.U.textLayout > 2) COMMON.U.textLayout = 0;
				Debug.Log("Layout: " + COMMON.U.textLayout + ", Font size: " + COMMON.U.fontSize);
				InitSettings(true); COMMON.U.Save(COMMON.saveFolder + "UserSettings.bin");
			}
			if (Input.GetKeyDown(KeyCode.W)) {
				COMMON.U.fontSize = COMMON.U.fontSize - 1; if (COMMON.U.fontSize < 0) COMMON.U.fontSize = 7;
				Debug.Log("Layout: " + COMMON.U.textLayout + ", Font size: " + COMMON.U.fontSize);
				InitSettings(true); COMMON.U.Save(COMMON.saveFolder + "UserSettings.bin");
			}
			if (Input.GetKeyDown(KeyCode.E)) {
				COMMON.U.fontSize = COMMON.U.fontSize + 1; if (COMMON.U.fontSize > 7) COMMON.U.fontSize = 0;
				Debug.Log("Layout: " + COMMON.U.textLayout + ", Font size: " + COMMON.U.fontSize);
				InitSettings(true); COMMON.U.Save(COMMON.saveFolder + "UserSettings.bin");
			}
		}*/

		//if (DC.TextSound != null && DC.BGM.clip == null) {
		//	if (phase == 2 && !DC.TextSound.isPlaying) DC.TextSound.Play();
		//	if (phase != 2 && DC.TextSound.isPlaying) DC.TextSound.Stop();
		//}

		//1. FADE IN
		if (phase == 1 && DC.S.inNarration) {
			cl = NRT.color; cl.a += 0.025F * Time.deltaTime * 60; NRT.color = cl;
			if (cl.a >= COMMON.U.overlayAlpha) { cl.a = COMMON.U.overlayAlpha; NRT.color = cl; phase = 2; }
		}
		else if (phase == 1) {
			cl = DLG.color; cl.a += 0.025F * Time.deltaTime * 60; DLG.color = cl; RSP.color = cl;
			if (cl.a >= COMMON.U.overlayAlpha) {
				cl.a = COMMON.U.overlayAlpha; DLG.color = cl; RSP.color = cl;
				if (line[k].Length > 0 && (!rspOnly || COMMON.U.textLayout != 1)) phase = 2; else phase = 6; //if line is empty, jump straight to responses
			}
		}
		//2. MAIN PHRASE
		else if (phase == 2 && (COMMON.U.textAnim < 2 || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
			if (pi < pmax) { i = pauses[pi]; F.text = line[k].Substring(0, i); pi++; phase = 3; }
			else { i = line[k].Length - 1; F.text = line[k]; k++; phase = 4; }
			bi = 0; while (bi < bmax && i >= breaks[bi]) { F.text = F.text.Insert(breaks[bi] + bi, "\n"); bi++; }
		}
		else if (phase == 2 && COMMON.U.textAnim == 2) {
			j++; if (j == 2 || Time.deltaTime > 0.025F) { //write another letter every 2 frames, but if fps drops below 40 (or is locked at 30 from the start), switch to every frame [the transition is actually pretty smooth]
				if (pi < pmax && i == pauses[pi]) { phase = 3; pi++; j = 0; }
				else {
					F.text += line[k][i];
					j = 0; i++;
					if (bi < bmax && i == breaks[bi]) { F.text += "\n"; bi++; }
					if (i == line[k].Length) { phase = 4; k++; }
				}
			}
		}

		//3-4. CONTINUE
		else if (phase == 3 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
			CNT.gameObject.SetActive(false); phase = 2;
		}
		else if (phase == 4 && k == line.Length && F == fieldD && !DS.Finisher[id])
			phase = 6;
		else if (phase == 4 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
			CNT.gameObject.SetActive(false);
			if (F != fieldD && k == line.Length) phase = 5;
			else if (F == fieldD && DS.Finisher[id] && k == line.Length) phase = 9;
			else { PrepareLine(); phase = 2; }
		}
		else if ((phase == 3 || phase == 4) && !CNT.gameObject.activeSelf) {
			int strInd = 0; if (bi > 0) strInd = breaks[bi-1];
			string str; if (phase == 4) str = line[k-1].Substring(strInd); else str = line[k].Substring(strInd, i - strInd);
			//width
			var w = G.CalcSize(new GUIContent(str)).x + F.transform.parent.GetComponent<RectTransform>().position.x + F.GetComponent<RectTransform>().anchoredPosition.x + 10*scale;
			v = CNT.anchoredPosition; v.x = w; CNT.anchoredPosition = v;
			//height
			var h = F.GetComponent<RectTransform>().position.y;
			if (COMMON.U.textLayout == 1) h -= Mathf.Round(2 * scale);
			else h -= G.lineHeight + G.lineHeight + G.lineHeight * F.lineSpacing * (bi-1); //one height for the arrow itself, the rest is for lines
			CNT.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, h, CNT.rect.height);
			CNT.gameObject.SetActive(true); an = 0;
		}
		else if (phase == 3 || phase == 4) {
			v = CNT.anchoredPosition;
			var incr = 0.04F * Time.deltaTime * 60;
			if (an < 60) v.x += incr * scale; else v.x -= incr * scale;
			CNT.anchoredPosition = v;
			an += Time.deltaTime <= 0.025F ? 1 : 2; if (an >= 120) an = 0;
		}

		//5. NEXT NARRATION
		else if (phase == 5) {
			if (!DC.NS.Finisher[id]) {
				if (DC.NS.TriggersEvent[id] != null) Events.Trigger(DC.S, (int)DC.NS.TriggersEvent[id]);
				id = DC.NS.Next[id]; if (DC.NS.Additive[id] == 0) DC.S.NID = id;

				//line = DC.Narration.GetLine(id).Split(new string[] {"[clear]"}, System.StringSplitOptions.None);
				var l = DC.Narration.GetLine(id); if (COMMON.U.languageID == 0 || COMMON.U.textLayout != 1 || COMMON.U.fontSize <= 5) l = l.Replace("[clear]", "[pause] ");
				line = l.Split(new string[] {"[clear]"}, System.StringSplitOptions.None);

				var fontAdditiveThreshold = COMMON.U.languageID == 0 ? 6 : 5;
				if (COMMON.U.textLayout == 2 && COMMON.U.fontSize < fontAdditiveThreshold && DC.NS.Additive[id] > 0) { //additiveness: 1 for usual paragraphs, 2 for dialogue narrations (C1 and C4) (differ only in offset)
					var offset = F.GetComponent<RectTransform>().offsetMax.y;
					offset -= Mathf.CeilToInt(G.lineHeight + G.lineHeight * F.lineSpacing * (F.cachedTextGenerator.lineCount-1));
					offset -= Mathf.RoundToInt(DC.NS.Additive[id] == 1 ? G.lineHeight : G.lineHeight / 2);
					ni++; F = N[ni];
					v = F.GetComponent<RectTransform>().offsetMax; v.y = Mathf.RoundToInt(offset); F.GetComponent<RectTransform>().offsetMax = v;
					F.gameObject.SetActive(true);
				} else {
					for (int a = 1; a <= ni; a++) N[a].gameObject.SetActive(false);
					ni = 0; F = N[0];
				}
				k = 0; PrepareLine(); phase = 2;
			} else {
				if (DC.NS.TriggersEvent[id] != null) finEv = (int)DC.NS.TriggersEvent[id];
				for (int a = 0; a <= ni; a++) N[a].text = "";
				for (int a = 1; a <= ni; a++) N[a].gameObject.SetActive(false);
				phase = 10;
			}
		}

		//6-7. GET AND DISPLAY RESPONSES
		else if (phase == 6) {
			if (COMMON.U.textLayout != 1) {
				var offset = F.GetComponent<RectTransform>().offsetMax.y;
				if (F.text == "") offset /= 2; //dividing by two just because it looks better
				else offset -= Mathf.CeilToInt(bmax * F.lineSpacing * G.lineHeight + G.lineHeight);
				v = RM.offsetMax; v.y = offset; RM.offsetMax = v;
				var RT = ArrowU.GetComponent<RectTransform>();
				v = RT.anchoredPosition; v.y = offset - RT.rect.height - Rtop; RT.anchoredPosition = v;
			}
			int b = 0; for (int a = 0; a < DS.R[id].Length; a++) {
				int rid = DS.R[id][a];
				if (!DS.Locked[rid] && !DS.Used[rid]) {
					R[b].text = (b + 1) + ". " + D.GetLine(rid);
					R[b].GetComponent<Responses>().chosenID = rid;
					b++;
				}
			}
			Canvas.ForceUpdateCanvases();
			rtop = 0; rcount = b; rcur = -1; SetResponsesVisibility();
			if (COMMON.U.textAnim == 0) phase = 8; else {
				phase = 7;
				for (int a = 0; a < viscount; a++) { cl = R[a].color; cl.a = 0; R[a].color = cl; }
				if (ArrowD.activeSelf) { cl = ArrowD.GetComponent<Image>().color; cl.a = 0; ArrowD.GetComponent<Image>().color = cl; }
			}
		}
		else if (phase == 7) {
			for (int a = 0; a < viscount; a++)
				if (R[a].color.a < 1 && (a == 0 || R[a-1].color.a >= 0.5F)) {
					cl = R[a].color; cl.a += 0.05F * Time.deltaTime * 60; R[a].color = cl;
				}
			if (!ArrowD.activeSelf && R[viscount-1].color.a >= 1) phase = 8; 
			else if (ArrowD.activeSelf && R[viscount-1].color.a >= 1) {
				cl = ArrowD.GetComponent<Image>().color;
				cl.a += 0.05F * Time.deltaTime * 60;
				ArrowD.GetComponent<Image>().color = cl;
				if (cl.a >= 1) phase = 8;
			}
		}

		//8. RESPONSES SCROLLING AND CONTROL
		else if (phase == 8 && Input.GetAxis("Mouse ScrollWheel") > 0 && rtop > 0) {
			rtop--; SetResponsesVisibility(); ClearSelection();
		}
		else if (phase == 8 && Input.GetAxis("Mouse ScrollWheel") < 0 && !R[rcount-1].IsActive()) {
			rtop++; SetResponsesVisibility(); ClearSelection();
		}
		else if (phase == 8 && !inHover && rcur == -1 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))) {
			rcur = 0; R[0].color = cActive;
			rtop = 0; SetResponsesVisibility();
		}
		else if (phase == 8 && !inHover && rcur < rcount-1 && (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))) {
			R[rcur].color = cMain; rcur++; R[rcur].color = cActive;
			if (!R[rcur].IsActive()) { rtop++; SetResponsesVisibility(); }
		}
		else if (phase == 8 && !inHover && rcur > 0 && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))) {
			R[rcur].color = cMain; rcur--; R[rcur].color = cActive;
			if (!R[rcur].IsActive()) { rtop--; SetResponsesVisibility(); }
		}
		else if (phase == 8 && !inHover && rcur != -1 && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))) {
			R[rcur].color = cMain; id = R[rcur].GetComponent<Responses>().chosenID; phase = 9;
		}
		else if (phase == 8 && !inHover) {
			if (Input.GetKeyDown(KeyCode.Alpha1) && R[0].gameObject.activeSelf) { id = R[0].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha2) && R[1].gameObject.activeSelf) { id = R[1].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha3) && R[2].gameObject.activeSelf) { id = R[2].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha4) && R[3].gameObject.activeSelf) { id = R[3].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha5) && R[4].gameObject.activeSelf) { id = R[4].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha6) && R[5].gameObject.activeSelf) { id = R[5].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha7) && R[6].gameObject.activeSelf) { id = R[6].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha8) && R[7].gameObject.activeSelf) { id = R[7].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }
			else if (Input.GetKeyDown(KeyCode.Alpha9) && R[8].gameObject.activeSelf) { id = R[8].GetComponent<Responses>().chosenID; ClearSelection(); phase = 9; }	
		}

		//9. NEXT DIALOGUE (and closing)
		else if (phase == 9) {
			if (!DS.Finisher[id] && !ignoreUsed) DS.Used[id] = true;
			if (DS.UnlockingID[id] != null) for (int i = 0; i < DS.UnlockingID[id].Length; i++) DS.Locked[DS.UnlockingID[id][i]] = false;
			if (DS.LockingID[id] != null) for (int i = 0; i < DS.LockingID[id].Length; i++) DS.Locked[DS.LockingID[id][i]] = true;
			var ev = DS.TriggersEvent[id];
			if (ev != null) if (!DS.Finisher[id]) Events.Trigger(DC.S, (int)ev); else finEv = (int)ev;
			if (!DS.Finisher[id]) {
				id = DS.R[id][0]; Events.UpdateDID(DC.S, id);
				for (int a = 0; a < R.Length; a++) R[a].gameObject.SetActive(false);
				ArrowU.SetActive(false); ArrowD.SetActive(false);
				line = D.GetLine(id).Split(new string[] {"[clear]"}, System.StringSplitOptions.None);
				k = 0; PrepareLine(); if (line[k].Length > 0 && (!rspOnly || COMMON.U.textLayout != 1)) phase = 2; else phase = 6;
				if (COMMON.U.textAnim == 0) Update(); //execute Update() the second time so there's no "flashing" (1 frame of empty text) in no-animate mode
			} else {
				//preparing for the next chat and setting nothing to talk about (or not)
				if (DS.R[id] == null) Events.SetNothingToTalkAbout(DC.S);
				else {
					id = DS.R[id][0]; Events.UpdateDID(DC.S, id);
					var b = 0; for (int a = 0; a < DS.R[id].Length; a++) {
						int rid = DS.R[id][a]; if (!DS.Locked[rid] && !DS.Used[rid] && !DS.Finisher[rid]) b++;
					}
					//a hack specifically for Shawn's CbIU finisher, because the level complete trigger is also a finisher and the above returns 0
					if (DC.S.levelID == 2 && DC.S.inDialogue == 1 && (id == 379 || id == 404)) b = 1;
					if (b == 0) Events.SetNothingToTalkAbout(DC.S);
				}
				//closing
				for (int a = 0; a < R.Length; a++) R[a].gameObject.SetActive(false);
				ArrowU.SetActive(false); ArrowD.SetActive(false);
				F.text = ""; phase = 10;
			}
		}
		//10. FADE OUT
		else if (phase == 10) {
			if (DC.S.inNarration) {
				cl = NRT.color; cl.a -= 0.025F * Time.deltaTime * 60; NRT.color = cl;
			} else {
				cl = DLG.color; cl.a -= 0.025F * Time.deltaTime * 60; DLG.color = cl;
				if (COMMON.U.textLayout == 1) RSP.color = cl;
			}
			if (cl.a <= 0) {
				DC.S.inDialogue = -1; DC.S.inNarration = false;
				DLG.gameObject.SetActive(false); RSP.gameObject.SetActive(false); NRT.gameObject.SetActive(false);
				if (bPhase == 0) Col(true); phase = 0;
				if (finEv != -1) { Events.Trigger(DC.S, finEv); finEv = -1; }
			}
		}

		//BLACK SCREEN CONTROL
		if (bPhase == 1) {
			var c = B.color; c.a += speed * Time.deltaTime * 60; B.color = c;
			if (c.a >= 1) bPhase = 0;
			if (c.a >= 1 && evnt > -10)
				if (evntDelay < 0) Events.Trigger(DC.S, evnt);
				else DelayedTrigger(evntDelay, evnt); //not currently used anywhere
		}
		else if (bPhase == 2) {
			var c = B.color; c.a -= speed * Time.deltaTime * 60; B.color = c;
			if (c.a <= 0) { B.gameObject.SetActive(false); bPhase = 0; if (phase == 0) Col(true); }
			if (c.a <= 0 && evnt > -10) Events.Trigger(DC.S, evnt);
		}

		//TITLE CONTROL
		if (tPhase == 1) {
			cl = TT1.color; cl.a += 0.01F * Time.deltaTime * 60; TT1.color = cl;
			cl = TT2.color; cl.a += 0.01F * Time.deltaTime * 60; TT2.color = cl;
			if (cl.a >= 1) tPhase = 2;
		}
		else if (tPhase == 2) { tPhase = -1; StartCoroutine(WaitingT(2, 3)); }
		else if (tPhase == 3) {
			cl = TT1.color; cl.a -= 0.01F * Time.deltaTime * 60; TT1.color = cl;
			cl = TT2.color; cl.a -= 0.01F * Time.deltaTime * 60; TT2.color = cl;
			if (cl.a <= 0) tPhase = 4;
		}
		else if (tPhase == 4) { tPhase = -1; StartCoroutine(WaitingT(0.5F, 5)); }
		else if (tPhase == 5) {
			TT1.gameObject.SetActive(false); TT2.gameObject.SetActive(false);
			DC.CursorLock(false); Col(true); DC.bMenu.SetActive(true);
			tPhase = 0;	if (evnt > -10) Events.Trigger(DC.S, evnt);
		}
	}

	public void showTitle(int cid, int e = -10) {
		TT1.gameObject.SetActive(true); TT2.gameObject.SetActive(true);
		TT1.text = TitleSTR1[cid]; cl = TT1.color; cl.a = 0; TT1.color = cl;
		TT2.text = TitleSTR2[cid]; cl = TT2.color; cl.a = 0; TT2.color = cl;
		DC.CursorLock(true); Col(false); DC.bMenu.SetActive(false);
		evnt = e; tPhase = 1;
	}

	public void blackScreenAppear(float spd = 0.01F, int e = -10, float eDelay = -1) {
		B.gameObject.SetActive(true); var c = B.color; c.a = 0; B.color = c;
		speed = spd; evnt = e; evntDelay = eDelay; bPhase = 1; if (phase == 0) Col(false);
	}
	public void blackScreenDisappear(float spd = 0.01F, int e = -10) {
		speed = spd; evnt = e; bPhase = 2;
	}
}