using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

	DataControlChapter5 DC;
	Image DLG, RSP, BS; RectTransform VLG; Text fieldD, fieldR; Color c, ca, cm;
	int phase = 0; string line; int i,j;
	int[] breaks = new int[10]; int bi, bmax;
	Color lightBlue = new Color(0.718F, 0.878F, 1);
	Color brightBlue = new Color(0.188F, 0.647F, 1);

	void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter5>();
		BS = DC.UIC.transform.Find("BlackScreen").GetComponent<Image>();
		RSP = DC.UIC.transform.Find("Responses").GetComponent<Image>();
		VLG = DC.UIC.transform.Find("DialogueAlt/Responses").GetComponent<RectTransform>();
		DLG = DC.UIC.transform.Find(COMMON.U.textLayout != 1 ? "DialogueAlt" : "Dialogue").GetComponent<Image>();
		fieldD = DC.UIC.transform.Find(COMMON.U.textLayout != 1 ? "DialogueAlt/Text" : "Dialogue/Text").GetComponent<Text>();
		fieldR = DC.UIC.transform.Find(COMMON.U.textLayout != 1 ? "DialogueAlt/Responses/1" : "Responses/Main/1").GetComponent<Text>();
		for (int a = 1; a <= 9; a++) DC.UIC.transform.Find(COMMON.U.textLayout != 1 ? "DialogueAlt/Responses/"+a : "Responses/Main/"+a).gameObject.SetActive(false);
		Destroy(fieldR.GetComponent<Responses>());
		cm = COMMON.U.textLayout == 1 ? Color.white : lightBlue;
		ca = COMMON.U.textLayout == 1 ? lightBlue : brightBlue;
	}

	IEnumerator Waiting(float sec, int ph) { yield return new WaitForSeconds(sec); phase = ph; }

	public void LaunchIntro() {
		Init();
		DC.bMenu.SetActive(false); DC.CursorLock(true);
		BS.gameObject.SetActive(true); DLG.gameObject.SetActive(true);
		line = DC.Dialogue.GetLine(0); PrepareBreaks(); fieldD.text = ""; i = 0; j = 0;
		StartCoroutine(Waiting(1, 1));
	}

	void PrepareBreaks() {
		fieldD.text = line; Canvas.ForceUpdateCanvases(); bmax = fieldD.cachedTextGenerator.lineCount - 1;
		for (int a = 1; a <= bmax; a++) breaks[a-1] = fieldD.cachedTextGenerator.lines[a].startCharIdx;
		bi = 0;
	}
	void ResponseSize() {
		var l = Mathf.RoundToInt(DC.UIC.G.CalcHeight(new GUIContent(fieldR.text), DC.UIC.wmax) / DC.UIC.G.lineHeight);
		var fl = Mathf.RoundToInt(DC.UIC.G.lineHeight);
		var h = l < 2 ? fl : fl + fl*fieldR.lineSpacing;
		var w = l > 1 ? DC.UIC.wmax : DC.UIC.G.CalcSize(new GUIContent(fieldR.text)).x;
		fieldR.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, w);
		fieldR.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
	}

	void Click() {
		fieldR.color = cm; fieldR.gameObject.SetActive(false);
		line = DC.Dialogue.GetLine(phase == 5 ? 2 : 4); PrepareBreaks();
		fieldD.text = ""; i = 0; j = 0; int nextPhase = phase + 1;
		phase = -1; StartCoroutine(Waiting(0.2F, nextPhase));
	}

	void Update() {
		if (phase == 1 && i < line.Length) {
			j++; if (j == 2 || Time.deltaTime > 0.025F) {
				j = 0; fieldD.text += line[i]; i++;
				if (bi < bmax && i == breaks[bi]) { fieldD.text += "\n"; bi++; }
				if (line[i-1] == '…') { phase = -1; StartCoroutine(Waiting(1, 1)); }
			}
		}
		else if (phase == 1 && i >= line.Length) {
			phase = -1; StartCoroutine(Waiting(1, 2));
		}
		else if (phase == 2) {
			c = BS.color; c.a -= 0.005F * Time.deltaTime * 60; BS.color = c;
			if (!DC.BGM.isPlaying) DC.BGM.Play();
			if (c.a <= 0) {
				BS.gameObject.SetActive(false);
				if (COMMON.U.textLayout != 1) { var v = VLG.offsetMax; v.y = -fieldD.cachedTextGenerator.lineCount * fieldD.cachedTextGenerator.lines[0].height + fieldD.GetComponent<RectTransform>().offsetMax.y; VLG.offsetMax = v; }
				RSP.gameObject.SetActive(COMMON.U.textLayout == 1);
				c = RSP.color; c.a = COMMON.U.textLayout != 1 ? 0.5F : 0; RSP.color = c;
				phase = -1; StartCoroutine(Waiting(0.5F, 3));
			}
		}
		else if (phase == 3) {
			c = RSP.color; c.a += 0.0125F * Time.deltaTime * 60; RSP.color = c;
			if (c.a >= 0.5F) {
				fieldR.text = "1. " + DC.Dialogue.GetLine(1);
				ResponseSize();
				c = fieldR.color; c.a = 0; fieldR.color = c; fieldR.gameObject.SetActive(true);
				phase = 4;
			}
		}
		else if (phase == 4) {
			c = fieldR.color; c.a += 0.025F * Time.deltaTime * 60; fieldR.color = c;
			if (c.a >= 1) { DC.CursorLock(false); phase = 5; }
		}
		else if ((phase == 5 || phase == 8) && 
			(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Alpha1))) {
			Click();
		}
		else if ((phase == 5 || phase == 8) &&
         	Input.mousePosition.x > fieldR.transform.position.x &&
		    Input.mousePosition.x < fieldR.transform.position.x + fieldR.GetComponent<RectTransform>().rect.width &&
		    Input.mousePosition.y > fieldR.transform.position.y - fieldR.GetComponent<RectTransform>().rect.height &&
		  	Input.mousePosition.y < fieldR.transform.position.y) {
			if (fieldR.color != ca) fieldR.color = ca;
			if (Input.GetMouseButton(0)) Click();
		}
		else if ((phase == 5 || phase == 8) && fieldR.color == ca) {
			fieldR.color = cm;
		}
		else if (phase == 6 && i < line.Length) {
			j++; if (j == 2 || Time.deltaTime > 0.025F) {
				j = 0; fieldD.text += line[i]; i++;
				if (bi < bmax && i == breaks[bi]) { fieldD.text += "\n"; bi++; }
				if (line[i-1] == '…') { phase = -1; StartCoroutine(Waiting(1, 6)); }
			}
		}
		else if (phase == 6 && i >= line.Length) {
			if (COMMON.U.textLayout != 1) { var v = VLG.offsetMax; v.y = -fieldD.cachedTextGenerator.lineCount * fieldD.cachedTextGenerator.lines[0].height + fieldD.GetComponent<RectTransform>().offsetMax.y; VLG.offsetMax = v; }
			fieldR.text = "1. " + DC.Dialogue.GetLine(3);
			ResponseSize();
			c = fieldR.color; c.a = 0; fieldR.color = c; fieldR.gameObject.SetActive(true);
			phase = -1; StartCoroutine(Waiting(1, 7));
		}
		else if (phase == 7) {
			c = fieldR.color; c.a += 0.025F * Time.deltaTime * 60; fieldR.color = c;
			if (c.a >= 1) phase = 8;
		}
		else if (phase == 9 && i < line.Length) {
			j++; if (j == 2 || Time.deltaTime > 0.025F) {
				j = 0; fieldD.text += line[i]; i++;
			}
		}
		else if (phase == 9 && i >= line.Length) {
			phase = -1; StartCoroutine(Waiting(1.5F, 10));
		}
		else if (phase == 10) {
			c = fieldD.color; c.a -= 0.025F * Time.deltaTime * 60; fieldD.color = c;
			c = DLG.color; c.a -= 0.0125F * Time.deltaTime * 60; DLG.color = c;
			if (COMMON.U.textLayout == 1) { c = RSP.color; c.a -= 0.0125F * Time.deltaTime * 60; RSP.color = c; }
			if (c.a <= 0) {
				phase = 0;
				DC.S = new SaveGame(); DC.S.levelID = 0;
				DC.S.Save(COMMON.saveFolder + "Autosave.bin");
				COMMON.saveToLoad = "Autosave.bin";
				DC.MC.LoadLevel(0, true);
			}
		}
	}
}
