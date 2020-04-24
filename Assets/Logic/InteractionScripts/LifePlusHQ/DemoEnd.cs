using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoEnd : MonoBehaviour {

	DataControlHub DC;
	Text T1, T2, T3;
	int phase = 0; Color c;

	void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		T1 = transform.Find("1").GetComponent<Text>();
		T2 = transform.Find("2").GetComponent<Text>();
		T3 = transform.Find("3").GetComponent<Text>();
		if (Screen.width != 1280.0F) {
			var scale = Screen.width / 1280.0F;
			T1.fontSize = Mathf.RoundToInt(T1.fontSize * scale);
			T2.fontSize = Mathf.RoundToInt(T2.fontSize * scale);
			T3.fontSize = Mathf.RoundToInt(T3.fontSize * scale);
			var R = T1.GetComponent<RectTransform>();
			var v = R.anchoredPosition; v.y = Mathf.RoundToInt(v.y * scale); R.anchoredPosition = v;
			R = T2.GetComponent<RectTransform>();
			v = R.anchoredPosition; v.y = Mathf.RoundToInt(v.y * scale); R.anchoredPosition = v;
			R = T3.GetComponent<RectTransform>();
			v = R.anchoredPosition; v.y = Mathf.RoundToInt(v.y * scale); R.anchoredPosition = v;
		}
	}

	public void Show() {
		Init(); gameObject.SetActive(true);
		c = T1.color; c.a = 0; T1.color = c; T2.color = c;
		c = T3.color; c.a = 0; T3.color = c;
		phase = 1;
	}

	IEnumerator Waiting(float sec, int ph) { yield return new WaitForSeconds(sec); phase = ph; }

	void Update() {
		if (phase == 1 && T1.color.a < 1) {
			c = T1.color; c.a += 0.01F * Time.deltaTime * 60; T1.color = c; T2.color = c;
		}
		else if (phase == 1 && T1.color.a >= 1) StartCoroutine(Waiting(1.0F, 2));
		else if (phase == 2 && T3.color.a < 1) {
			c = T3.color; c.a += 0.01F * Time.deltaTime * 60; T3.color = c;
		}
		else if (phase == 2 && T3.color.a >= 1) {
			phase = 0; DC.CursorLock(false); DC.bMenu.SetActive(true);
		}

		//DC.S.Name = "DEMO END"; int k = 1; string pth = COMMON.saveFolder + "Save01.bin";
		//while (k < 99 && System.IO.File.Exists(pth)) { k++; pth = COMMON.saveFolder + "Save" + (k < 10 ? "0" : "") + k + ".bin"; }
		//DC.S.Save(pth); //not updating "FileNames.xml" here, since SaveLoad should take care of that during the next display anyway
		//DC.MC.transform.Find("SaveLoad").GetComponent<SaveLoad>().TakeScreenshot("Save" + (k < 10 ? "0" : "") + k);
	}
}
