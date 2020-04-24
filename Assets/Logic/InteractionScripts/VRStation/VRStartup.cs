using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VRStartup : MonoBehaviour {
	DataControlChapter3 DC;
	Text[] L; Text T; string s;
	int phase = 0, k, ind; float t;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter3>();
		L = new Text[5]; for (int i = 1; i <= 5; i++) {
			L[i-1] = transform.Find(i.ToString()).GetComponent<Text>();
			L[i-1].gameObject.SetActive(false);
		}
		if (Screen.width != 1280) {
			float scale = Screen.width / 1280.0F; Vector2 v;
			for (int i = 0; i < 5; i++) {
				L[i].fontSize = Mathf.RoundToInt(L[i].fontSize * scale);
				v = L[i].GetComponent<RectTransform>().offsetMin; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); L[i].GetComponent<RectTransform>().offsetMin = v;
				v = L[i].GetComponent<RectTransform>().offsetMax; v.x = Mathf.RoundToInt(v.x * scale); v.y = Mathf.RoundToInt(v.y * scale); L[i].GetComponent<RectTransform>().offsetMax = v;
			}
		}
		T = L[0]; T.gameObject.SetActive(true); s = T.text; T.text = "";
		t = 0; k = 0; ind = 0; phase = 1;
	}

	IEnumerator Waiting(float sec, int ph) {
		phase = 0; yield return new WaitForSeconds(sec); phase = ph;
	}

	void Update() {
		if (DC.paused) { if (DC.Sound.loop) DC.Sound.loop = false; return; }
		if (phase == 1) {
			t += Time.deltaTime;
			if (t >= 0.5F) { k++; t = 0; T.text = k % 2 == 1 ? "_" : ""; if (k % 2 == 1) DC.Sound.Play(); }
			if (k == 8) { t = 0; k = 0; phase = 2; }
		}
		else if (phase == 2) {
			t += Time.deltaTime; if (!DC.Sound.loop) { DC.Sound.loop = true; DC.Sound.Play(); }
			if (t >= 0.03F) {
				t = 0; k++; T.text = s.Substring(0, k);
				if (k == s.Length) {
					DC.Sound.loop = false; ind++; if (ind == L.Length) {
						ind = 0; StartCoroutine(Waiting(5, 3));
					} else {
						T = L[ind]; T.gameObject.SetActive(true); s = T.text; T.text = "";
						t = 0; k = 0; StartCoroutine(Waiting(1, 2));
					}
				}
			}
		}
		else if (phase == 3) {
			DC.Sound.Play(); L[ind].gameObject.SetActive(false); ind++;
			if (ind < L.Length) StartCoroutine(Waiting(0.5F, 3));
			else StartCoroutine(Waiting(1, 4));
		}
		else if (phase == 4) {
			DC.S.SC3.inStartup = false; DC.UIC.StartNarration(4);
			phase = 0; gameObject.SetActive(false);
		}
	}
}