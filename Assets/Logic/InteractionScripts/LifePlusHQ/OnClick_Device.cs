using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClick_Device : MonoBehaviour {

	DataControlHub DC; int phase = 0;
	public SkinnedMeshRenderer W; MeshRenderer D;
	public Material[] M;
	public Texture T1, T2;

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		D = W.transform.parent.GetComponent<MeshRenderer>();
		W.material = M[DC.S.levelID -11];
		if (DC.S.SH.widgetIsOn) {
			W.gameObject.SetActive(true); W.SetBlendShapeWeight(0, 0);
			var m = D.materials; m[0].SetTexture("_EmissionMap", T2); D.materials = m;
		}
	}

	void OnMouseDown() {
		DC.Sound.clip = DC.widgetSet; DC.Sound.Play();
		Cursor.visible = false; DC.bMenu.SetActive(false); DC.UIC.Col(false);
		if (!DC.S.SH.widgetIsOn) phase = 1;
		else {
			W.GetComponent<Animation>()["Widget"].normalizedTime = 1;
			W.GetComponent<Animation>()["Widget"].speed = -1;
			W.GetComponent<Animation>().Play();
			phase = 4;
		}
	}
	
	void Update() {
		if (phase == 1) {
			var m = D.materials;
			var c = m[0].GetColor("_EmissionColor");
			var t = 0.025F * Time.deltaTime * 60;
			c = new Color(c.r - t, c.g - t, c.b - t);
			m[0].SetColor("_EmissionColor", c);
			if (c.r <= 0) {
				m[0].SetTexture("_EmissionMap", T2);
				W.gameObject.SetActive(true);
				W.SetBlendShapeWeight(0, 100);
				W.GetComponent<Animation>()["Widget"].normalizedTime = 0;
				W.GetComponent<Animation>()["Widget"].speed = 1;
				W.GetComponent<Animation>().Play();
				phase = 2;
			}
			D.materials = m;
		}
		else if (phase == 2) {
			var m = D.materials;
			var c = m[0].GetColor("_EmissionColor");
			var t = 0.02F * Time.deltaTime * 60;
			c = new Color(c.r + t, c.g + t, c.b + t);
			m[0].SetColor("_EmissionColor", c);
			if (c.r >= 1) phase = 3;
			D.materials = m;
		}
		else if (phase == 3 && !W.GetComponent<Animation>().isPlaying) {
			DC.S.SH.widgetIsOn = true;
			Cursor.visible = true; DC.bMenu.SetActive(true); DC.UIC.Col(true);
			phase = 0;
		}
		else if (phase == 4) {
			var m = D.materials;
			var c = m[0].GetColor("_EmissionColor");
			var t = 0.02F * Time.deltaTime * 60;
			c = new Color(c.r - t, c.g - t, c.b - t);
			m[0].SetColor("_EmissionColor", c);
			if (c.r <= 0) { m[0].SetTexture("_EmissionMap", T1); phase = 5; }
			D.materials = m;
		}
		else if (phase == 5 && !W.GetComponent<Animation>().isPlaying) {
			var m = D.materials;
			var c = m[0].GetColor("_EmissionColor");
			var t = 0.025F * Time.deltaTime * 60;
			c = new Color(c.r + t, c.g + t, c.b + t);
			m[0].SetColor("_EmissionColor", c);
			if (c.r >= 1) {
				DC.S.SH.widgetIsOn = false;
				Cursor.visible = true; DC.bMenu.SetActive(true); DC.UIC.Col(true);
				phase = 0;
			}
			D.materials = m;
		}
	}
}