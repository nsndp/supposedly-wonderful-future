using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_Desk : MonoBehaviour {

	public bool isF; public GameObject D; public GameObject otherCol;
	Transform S, SM, SH; GameObject T, P; Color c, cm, ct, cp;
	DataControl DC; int phase = 0; float t = 0; bool switched = false;
	string nm = "FoldingDesk";

	//material's final transparencies of traces for standing (1) and sitting (2) positions
	float trace1max = 0.0588F; //(15 out of 255)
	float trace2max = 0.098F;  //(25 out of 255)

	public void Init() {
		S = D.transform.Find("Screens"); c = S.GetComponent<Renderer>().material.GetColor("_Color");
		SM = D.transform.Find("MainScreen/Background"); cm = SM.GetComponent<Image>().color;
		T = D.transform.Find("Trace").gameObject; ct = T.GetComponent<Renderer>().material.GetColor("_TintColor");
		P = D.transform.Find("Projector").gameObject; cp = P.GetComponent<Renderer>().material.GetColor("_Color");
		SH = GameObject.Find("CollidersB").transform.Find("Screens");
		DC = GameObject.Find("Data").GetComponent<DataControl>();
	}
	public void StartWithU() {
		D.transform.Find("Main").localRotation = Quaternion.Euler(0, 90, -90);
		D.transform.Find("SupportL").localRotation = Quaternion.Euler(270, 90, 0);
		D.transform.Find("SupportR").localRotation = Quaternion.Euler(270, -90, 0);
		S.localPosition = new Vector3(-0.3F, 1.2F, 0);
		SM.parent.localPosition = new Vector3(-0.301F, 1.2F, 0);
		SH.localPosition = new Vector3(5.3F, 1.2F, -1.2F);
		ct.a = trace2max; T.GetComponent<Renderer>().material.SetColor("_TintColor", ct);
		D.transform.Find("HighlightS").localPosition = new Vector3(-0.3F, 1.2F, 0);
		T.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(21, 100);
	}
	public void RevealSuspectF() {
		D.GetComponent<Animation>().Play("RevealSuspectF");
		D.transform.Find("HighlightS").GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 100);
		SH.GetComponents<BoxCollider>()[2].size = new Vector3(0.85F, 0.26F, 0.01F);
	}

	void OnMouseDown() {
		if (phase == 0) {
			Cursor.visible = false; DC.UIC.Col(false); DC.bMenu.SetActive(false);
			if (isF) D.GetComponent<Animation>()[nm].speed = 1.0F;
			else { D.GetComponent<Animation>()[nm].speed = -1.0F; D.GetComponent<Animation>()[nm].time = D.GetComponent<Animation>()[nm].length; }
			D.GetComponent<Animation>().Play(nm);
			phase = 1;
		}
	}

	void Update() {
		if (phase == 1 && !D.GetComponent<Animation>().isPlaying) { t = 0; phase = 2; }
		else if (phase == 2 && t <= 100) {
			if (!switched && t >= 51) {
				switched = true;
				S.localPosition = isF ? new Vector3(-0.3F, 1.2F, 0) : new Vector3(-0.1F, 1.6F, 0);
				SM.parent.localPosition = isF ? new Vector3(-0.301F, 1.2F, 0) : new Vector3(-0.101F, 1.6F, 0);
				SH.localPosition = isF ? new Vector3(5.3F, 1.2F, -1.2F) : new Vector3(5.3F, 1.6F, -1.4F);
				D.transform.Find("HighlightS").localPosition = S.localPosition;
				T.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(21, isF ? 100 : 0);
			}
			//screens
			if (t <= 50) c.a = Mathf.Lerp(1, 0, t / 50); else c.a = Mathf.Lerp(0, 1, (t - 50) / 50);
			cm.a = c.a; S.GetComponent<Renderer>().material.SetColor("_Color", c); SM.GetComponent<Image>().color = cm;
			//projector
			float f; if (t <= 50) f = Mathf.Lerp(1, 0, t / 50); else f = Mathf.Lerp(0, 1, (t - 50) / 50);
			cp.r = f; cp.g = f; cp.b = f;
			var m = P.GetComponent<Renderer>().materials; m[1].SetColor("_Color", cp); P.GetComponent<Renderer>().materials = m;
			//traces
			if (isF && t <= 50) ct.a = Mathf.Lerp(trace1max, 0, t / 50);
			else if (isF) ct.a = Mathf.Lerp(0, trace2max, (t - 50) / 50);
			else if (!isF && t <= 50) ct.a = Mathf.Lerp(trace2max, 0, t / 50);
			else if (!isF) ct.a = Mathf.Lerp(0, trace1max, (t - 50) / 50);
			T.GetComponent<Renderer>().material.SetColor("_TintColor", ct);
			t += Time.deltaTime <= 0.025F ? 1 : 2;
		}
		else if (phase == 2 && t > 100) {
			Cursor.visible = true; DC.UIC.Col(true); DC.bMenu.SetActive(true);
			phase = 0; switched = false;
			GameObject.Find("Data").GetComponent<DataControlChapter2>().S.SC2.deskUnfolded = isF;
			otherCol.SetActive(true); this.gameObject.SetActive(false);
		}
	}
}