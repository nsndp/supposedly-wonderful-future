using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailerRecord : MonoBehaviour {

	int activator = 0, phase = 0, k, j;
	string line = ""; float t0; bool twice = true;
	//news -->
	bool go = false; float tm; float[] starts; int[] inds; string[] strs;
	RectTransform[] N1; Text[] N2; GUIStyle GN; Image Bs;
	int[] js; bool[] twices;
	//<-- news
	Image BG; Text TX; Color cl;
	UIControl UIC; DataControl DC;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControl>();
		UIC = GameObject.Find("Interface").transform.Find("UI").GetComponent<UIControl>();
		BG = transform.Find("Background").GetComponent<Image>();
		TX = BG.transform.Find("Text").GetComponent<Text>();
	}

	void PrepareSize(/*string overrideW = ""*/) {
		var G = new GUIStyle(); G.font = TX.font; G.fontSize = TX.fontSize;
		G.fontStyle = TX.fontStyle; G.wordWrap = true; G.richText = true;
		var str = line.Replace("{", "").Replace("[", "").Replace("@", "").Split('\n');
		var w = G.CalcSize(new GUIContent(str[0])).x;
		for (int i = 1; i < str.Length; i++) { var w2 = G.CalcSize(new GUIContent(str[i])).x; if (w2 > w) w = w2; }
		//if (overrideW != "") w = G.CalcSize(new GUIContent(overrideW)).x;
		var indent = TX.GetComponent<RectTransform>().offsetMin.x * 2;
		var off = Mathf.FloorToInt((Screen.width - w - indent) / 2);
		BG.GetComponent<RectTransform>().offsetMin = new Vector2(off, 0);
		BG.GetComponent<RectTransform>().offsetMax = new Vector2(-off, 0);
		var h = G.lineHeight * 3;
		h += TX.GetComponent<RectTransform>().offsetMin.y - TX.GetComponent<RectTransform>().offsetMax.y;
		BG.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, h);
	}

	void PreparationA() {
		UIC.transform.Find("ButtonMenu").GetComponent<Image>().color = new Color(1, 1, 1, 0);
		PrepareSize(); COMMON.trailerRecordMode = 1;
	}
	void PreparationB(bool goDark = false) {
		UIC.transform.Find("Dialogue").gameObject.SetActive(false);
		UIC.transform.Find("DialogueAlt").gameObject.SetActive(false);
		UIC.transform.Find("Responses").gameObject.SetActive(false);
		UIC.transform.Find("Narration").gameObject.SetActive(false);
		UIC.transform.Find("NarrationAlt").gameObject.SetActive(false);
		UIC.transform.parent.Find("Anim").gameObject.SetActive(false);
		if (goDark) {
			UIC.transform.Find("BlackScreen").GetComponent<Image>().color = new Color(0, 0, 0, 1);
			UIC.transform.Find("BlackScreen").gameObject.SetActive(true);
		}
		UIC.transform.Find("ButtonMenu").GetComponent<Image>().color = new Color(1, 1, 1, 0);
		Cursor.visible = false;
		PrepareSize(); activator = 4;
	}

	void PreparationNews(int[] x, int[] y, Color[] c, string[] s) {
		N1 = new RectTransform[x.Length];
		N2 = new Text[x.Length];
		strs = s;
		for (int i = 0; i < x.Length; i++) {
			var B = Object.Instantiate(BG.gameObject, this.transform);
			var BR = B.GetComponent<RectTransform>();
			var OR = B.transform.GetChild(0).GetComponent<RectTransform>();
			var OT = B.transform.GetChild(0).GetComponent<Text>();
			BR.anchorMin = new Vector2(0, 1); BR.anchorMax = new Vector2(0, 1); BR.pivot = new Vector2(0, 1);
			BR.anchoredPosition = new Vector2(x[i] - 60 - 20, -y[i]); BR.sizeDelta = new Vector2(2000, 69);
			OR.pivot = new Vector2(0, 1); OR.offsetMin = new Vector2(20, 0); OR.offsetMax = new Vector2(0, 0);
			OT.color = c[i]; OT.text = s[i]; OT.fontStyle = FontStyle.Italic;
			OT.horizontalOverflow = HorizontalWrapMode.Overflow;
			Destroy(OT.GetComponent<Shadow>()); Destroy(OT.GetComponent<Outline>());
			N1[i] = BR; N2[i] = OT; N1[i].gameObject.SetActive(false);
		};
		GN = new GUIStyle(); GN.font = TX.font; GN.fontSize = TX.fontSize;
		GN.fontStyle = FontStyle.Italic; GN.wordWrap = true; GN.richText = true;
		starts = new float[] {0, 0.8F, 1.4F, 1.8F, 2F, 2.2F, 2.4F, 2.6F, 2.8F, 2.95F, 3.1F, 3.25F};
		inds = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		js = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
		twices = new bool[] {true, true, true, true, true, true, true, true, true, true, true, true};
		activator = 5;
		Bs = UIC.transform.Find("BlackScreen").GetComponent<Image>(); Bs.color = new Color(0, 0, 0, 0);
		Bs.gameObject.SetActive(true);
	}

	public void Go() {
		BG.color = new Color(0, 0, 0, 0); BG.gameObject.SetActive(true);
		TX.text = ""; k = 0; j = 0; phase = 1;
		DC.S.inDialogue = 111;
		DC.CursorLock(true); UIC.Col(false);
	}
	public void GoDelayed() {
		Go(); StartCoroutine(Waiting(2, 1));
	}

	IEnumerator Waiting(float sec, int ph) { phase = 0; yield return new WaitForSeconds(sec); phase = ph; }

	void Update() {
		//main
		if (phase == 1) {
			cl = BG.color; cl.a += 0.025F * Time.deltaTime * 60; BG.color = cl;
			if (cl.a >= 0.825F) { cl.a = 0.825F; BG.color = cl; phase = 2; t0 = Time.time; }
		}
		else if (phase == 2) {
			j++; if (j == 2 || Time.deltaTime > 0.025F) {
				j = 0;
				if (line[k] == '[') { k++; StartCoroutine(Waiting(0.5F, 2)); }
				else if (line[k] == '{') { k++; StartCoroutine(Waiting(0.8F, 2)); }
				else if (line[k] == '@') { k++; TX.text = ""; }
				else {
					TX.text += line[k]; k++;
					if (twice && k < line.Length && line[k] != '[' && line[k] != '{') { TX.text += line[k]; k++; }
					twice = !twice;
					if (k == line.Length) {	k = 0; phase = 0; Debug.Log(Time.time - t0); }
				}
			}
		}

		//news
		if (go) {
			tm += Time.deltaTime;
			for (int i = 0; i < N1.Length; i++) {
				if (!N1[i].gameObject.activeSelf && tm > starts[i]) {
					N2[i].text = "";
					N1[i].GetComponent<Image>().color = new Color(1, 1, 1, 0);
					N1[i].gameObject.SetActive(true);
					//N1[i].GetComponent<Image>().enabled = false;
				}
				else if (N1[i].gameObject.activeSelf && inds[i] < strs[i].Length) {
					js[i]++; if (js[i] == 2) {
						inds[i]++; N2[i].text = strs[i].Substring(0, inds[i]);
						if (twices[i] && inds[i] < strs[i].Length) { inds[i]++; N2[i].text = strs[i].Substring(0, inds[i]); }
						twices[i] = !twices[i]; js[i] = 0;
						if (N1[i].GetComponent<Image>().color.a < 0.75F) { cl = N1[i].GetComponent<Image>().color; cl.a += 0.103F * Time.deltaTime * 60; N1[i].GetComponent<Image>().color = cl; }
					}
				}
			}
			//if (tm >= 1.8F && Bs.color.a <= 1) { cl = Bs.color; cl.a += 0.025F; Bs.color = cl; }
			if (inds[11] == strs[11].Length) { Debug.Log(tm); go = false; }
		}


		if (Input.GetKeyDown(KeyCode.G) && activator == 4) Go(); //main
		if (Input.GetKeyDown(KeyCode.G) && activator == 5) go = true; //news

		if (Input.GetKey(KeyCode.LeftShift)) {
			if (Input.GetKeyDown(KeyCode.T) && activator == 0) activator = 1;
			else if (Input.GetKeyDown(KeyCode.R) && activator == 1) activator = 2;
			else if (Input.GetKeyDown(KeyCode.L) && activator == 2) activator = 3;
			else if (activator == 3) {
				if (DC.S.levelID == 0 && Input.GetKeyDown(KeyCode.Alpha0)) {
					line = "Have you ever imagined…{ a day like this would come?";
					Debug.Log("TRAILER INTRO: press G when ready");
					PreparationB(true);
				}
				else if (DC.S.levelID == 0 && Input.GetKeyDown(KeyCode.Alpha1)) {
					line = "Good morning, Mr. Morton.[\nI represent a company called LIFE+.[\nWe would like to offer you a job.";
					Debug.Log("TRAILER 0-1: now open the door");
					PreparationA();
				}
				else if (DC.S.levelID == 0 && Input.GetKeyDown(KeyCode.Alpha2)) {
					line = "You'll have to visit 2048 and solve a series of problems.{\nAfter that, you can come back…[ or you can stay.[ Where\nscience is stronger and you can live a longer, happier life.";
					//line = "You'll have to visit 2046\nand solve a series of problems.\n{@After that, you can come back…[\nor you can stay in the future.\n{@Where science is stronger and you\ncan live a longer, happier life.";

					//line = "I felt that I really found my calling.[\nI wanted to entertain people…[\nonly people had different things in mind.";
					//line = "God? Is that what she called me again?[\nMan, it’s such a misnomer.";
					Debug.Log("TRAILER 0-2: press G when ready");
					PreparationB();
				}
				else if (DC.S.levelID == 0 && Input.GetKeyDown(KeyCode.Alpha3)) {
					line = "Where you can live forever, really.";
					Debug.Log("TRAILER 0-3: press G when ready");
					PreparationB(true);
				}
				else if (DC.S.levelID == 1 && Input.GetKeyDown(KeyCode.Alpha1)) {
					line = "It can’t be the future.[\nHow can it be the future when time is still?";
					Debug.Log("TRAILER 1-0: now talk to the woman");
					PreparationA();
				}
				else if (DC.S.levelID == 1 && Input.GetKeyDown(KeyCode.Alpha2)) {
					line = "Mommy wouldn’t play with me.[\nShe just covers her face and starts crying.";
					Debug.Log("TRAILER 1-2: now talk to Mitty");
					PreparationA();
				}
				else if (DC.S.levelID == 1 && Input.GetKeyDown(KeyCode.Alpha3)) {
					line = "I thought I was coming to a utopia,\nnot a horror freak show!";
					Debug.Log("TRAILER 1-3: now click on the door");
					PreparationA();
				}
				else if (DC.S.levelID == 2 && Input.GetKeyDown(KeyCode.Alpha1)) {
					line = "There are troubles, all right, just the stupid kind.[\nPeople are so into illusions, they forget what\na heavy stone can do to your head.";
					Debug.Log("TRAILER 2-1: now talk to Kettley");
					PreparationA();
				}
				else if (DC.S.levelID == 2 && Input.GetKeyDown(KeyCode.Alpha2)) {
					line = "Online, you do stuff because it matters.[\nIt’s like a purer form of existence.";
					Debug.Log("TRAILER 2-2: now talk to Shawn");
					PreparationA();
				}
				else if (DC.S.levelID == 2 && Input.GetKeyDown(KeyCode.Alpha3)) {
					line = "He shot himself in the head.[\nNo, I mean…[ he moved to Japan.[\nI think it was a Dostoevsky reference.";
					Debug.Log("TRAILER 2-3: now talk to Ashley");
					PreparationA();
				}
				else if (DC.S.levelID == 4 && Input.GetKeyDown(KeyCode.Alpha1)) {
					line = "These 10 million, they were people, every single one![\nSo many feelings, never felt, so many hopes, lost.[\nSo many dreams…";
					Debug.Log("TRAILER 4-1: now click on the table");
					PreparationA();
				}
				else if (DC.S.levelID == 4 && Input.GetKeyDown(KeyCode.Alpha2)) {
					line = "What is “real,” anyway?[\nThis whole thing could be a computer simulation,\nand we wouldn’t know any better.";
					Debug.Log("TRAILER 4-2: press G when ready");
					PreparationA();
				}
				else if (DC.S.levelID == 5 && Input.GetKeyDown(KeyCode.Alpha1)) {
					line = "How do you deal with deaths on the news?[\n“Thank God it wasn’t me.”[\nAnd then you carry on with your day.";
					//line = "I’m just a sad and tired old man.[\nA lonely failure at the top of the world.";
					Debug.Log("TRAILER 5-1: press F when ready");
					PreparationB(); COMMON.trailerRecordMode = 3;
				}
				else if (DC.S.levelID >= 10 && Input.GetKeyDown(KeyCode.Alpha1)) {
					line = "Do you know why he kept asking all those questions?";
					Debug.Log("TRAILER HUB-1: now talk to Jackie");
					PreparationA(); COMMON.trailerRecordMode = 2;
					var v = TX.GetComponent<RectTransform>().offsetMin; v.y = - TX.GetComponent<RectTransform>().offsetMax.y; TX.GetComponent<RectTransform>().offsetMin = v;
					//float scale = 1; if (Screen.width != 1280.0F) scale = Screen.width / 1280.0F;
					v = BG.GetComponent<RectTransform>().anchoredPosition; v.y = -130; BG.GetComponent<RectTransform>().anchoredPosition = v;
					v = BG.GetComponent<RectTransform>().sizeDelta; v.y = 100; BG.GetComponent<RectTransform>().sizeDelta = v;
					var DLGRT = UIC.transform.Find("DialogueAlt").GetComponent<RectTransform>();
					v = DLGRT.offsetMin; v.y = 172; DLGRT.offsetMin = v;
				}
				else if (DC.S.levelID >= 10 && Input.GetKeyDown(KeyCode.Alpha2)) {
					line = "I wouldn’t trade it for anything else.{\nOur beautiful, complicated mess.";
					Debug.Log("TRAILER HUB-2: press F to turn Jackie, G to show text");
					PreparationB(); COMMON.trailerRecordMode = 3;
				}

				else if (Input.GetKeyDown(KeyCode.N)) {
					//PreparationB(true);
					UIC.transform.Find("Dialogue").gameObject.SetActive(false);
					UIC.transform.Find("DialogueAlt").gameObject.SetActive(false);
					UIC.transform.Find("Responses").gameObject.SetActive(false);
					UIC.transform.Find("ButtonMenu").GetComponent<Image>().color = new Color(1, 1, 1, 0);
					UIC.transform.Find("ButtonArrow").GetComponent<Image>().color = new Color(1, 1, 1, 0);
					COMMON.trailerRecordMode = 1;
					PreparationNews(
						new int[] {100, 180, 300, 220, 220, 200, 130, 160, 200, 110, 250, 170},
						new int[] {110, 180, 360, 600, 230, 10, 450, 300, 500, 650, 55, 550},
						new Color[] {
							Color.white, new Color(0.733F, 0.878F, 1), new Color(0.271F, 0.667F, 1),
							new Color(0.478F, 0.761F, 1), new Color(0.478F, 0.761F, 1), new Color(0.11F, 0.596F, 1),
							new Color(0.737F, 0.878F, 1), Color.white, Color.white,
							Color.white, new Color(0.58F, 0.808F, 1), new Color(0.11F, 0.596F, 1)
						},
						new string[] {
							"As the days of summer heat draw to a close across the cities of Europe, so do the heated debates of the UN World Summit in Berlin",
							"End of July is a good time to be a technology enthusiast, as Tokyo Big Sight opens its doors for the annual Global Information Week",
							"On a rainy day in November, 2033, twenty people walked into a medical facility in Singapore",
							"The first manned mission to Mars won’t launch until at least 2053, says the UN Office for Outer Space Affairs",
							"A team of marine explorers has managed to drill 5 kilometers into the Earth’s mantle and retrieve multiple valuable rock samples, says the latest report from the Scientific Drilling Project.",
							"Time travel could be possible within the next few years, claims Denny Taslim, a person who may have built the world’s first working prototype of a time machine.",
							"Cells and tissue renewal process, as it came to be known, looks pretty mundane on the outside:",
							"The rise of corporate power has been a fruitful topic for decades now, but it’s only recently that a new realization started to sink in:",
							"Once (and if) we are able to explain consciousness in purely scientific terms",
							"Child mortality dropped by 66% between 1990 and 2025 and by 63% more between 2025 and 2045",
							"For those who worry about the danger of hacking, LIFE+ cybersecurity division is present as usual to provide peace of mind and show their state-of-the-art software.",
							"According to Nikki Hardingham, communication officer at the World Health Organization, reduction of prejudice did more for world peace"
						}
					);
					Debug.Log("TRAILER NEWS: press G when ready");
				}
			}
		}
	}
}
