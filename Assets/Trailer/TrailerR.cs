using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrailerR : MonoBehaviour {

	MeshRenderer S1, S2, S3, R, A, B, C;
	Color c; int phase = 0; float t = 0, tc = 0;
	Text Title; Image bS; int row = 2;
	AudioSource AS;
	Transform cam; Vector3 srcPos; Quaternion srcRot, dstRot;
	Vector3[] dstPos = new Vector3[] {
		new Vector3(0, 0, -0.59F), new Vector3(0, 0.203F, -0.56F),
		new Vector3(0, 0.382F, -0.455F), new Vector3(0, 0.515F, -0.298F)
	};

	void Start() {
		S1 = transform.Find("1").GetComponent<MeshRenderer>();
		S2 = transform.Find("2").GetComponent<MeshRenderer>();
		S3 = transform.Find("3").GetComponent<MeshRenderer>();
		R = transform.Find("4").GetComponent<MeshRenderer>();
		A = transform.Find("A").GetComponent<MeshRenderer>();
		B = transform.Find("B").GetComponent<MeshRenderer>();
		C = transform.Find("C").GetComponent<MeshRenderer>();
		AS = this.GetComponent<AudioSource>();
		MaterialReset(S1); MaterialReset(S2); MaterialReset(S3);
		MaterialReset(R); MaterialReset(A); MaterialReset(B); MaterialReset(C);
		Title = GameObject.Find("Canvas").transform.Find("Title").GetComponent<Text>();
		bS = GameObject.Find("Canvas").transform.Find("BlackScreen").GetComponent<Image>();
		c = Title.color; c.a = 0; Title.color = c;
		cam = GameObject.Find("Camera").transform;
		srcPos = cam.localPosition; srcRot = cam.localRotation;
		dstRot = Quaternion.Euler(new Vector3(20*row, 0, 0));
		Debug.Log(Screen.width + " " + Screen.height);
	}

	void MaterialReset(MeshRenderer M) {
		var m = M.materials;
		for (int i = 0; i < m.Length; i++) {
			c = m[i].GetColor("_Color"); c.a = 0;
			m[i].SetColor("_Color", c);
			m[i].SetColor("_EmissionColor", c);
		}
		M.materials = m;
	}

	void MaterialUp(MeshRenderer M) {
		var m = M.materials;
		for (int i = 0; i < m.Length; i++) {
			c = m[i].GetColor("_Color"); c.a += 0.02F;
			m[i].SetColor("_Color", c);
			m[i].SetColor("_EmissionColor", c);
		}
		M.materials = m;
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.R)) {
			cam.localPosition = srcPos; cam.localRotation = srcRot;
			MaterialReset(S1); MaterialReset(S2); MaterialReset(S3);
			MaterialReset(R); MaterialReset(A); MaterialReset(B); MaterialReset(C);
			c = bS.color; c.a = 0; bS.color = c; bS.gameObject.SetActive(false);
			AS.Stop();
		}

		if (Input.GetKeyDown(KeyCode.S)) { phase = 1; t = 0; AS.Play(); }
		if (phase == 1) {
			if (S1.materials[0].color.a < 1) MaterialUp(S1);
			t += Time.deltaTime;
			if (t > 2) phase = 2;
		}
		else if (phase == 2) {
			if (S2.materials[0].color.a < 1) MaterialUp(S2);
			t += Time.deltaTime;
			if (t > 4) phase = 3;
		}
		else if (phase == 3) {
			if (S3.materials[0].color.a < 1) MaterialUp(S3);
			t += Time.deltaTime;
			if (t > 6) phase = 4;
		}
		else if (phase == 4) {
			if (R.materials[0].color.r < 1) MaterialUp(R);
			t += Time.deltaTime;
			if (t > 8) phase = 5;
		}
		else if (phase == 5) {
			if (A.materials[0].color.r < 1) MaterialUp(A);
			t += Time.deltaTime;
			if (t > 10) phase = 6;
		}
		else if (phase == 6) {
			if (B.materials[0].color.r < 1) MaterialUp(B);
			t += Time.deltaTime;
			if (t > 12) phase = 7;
		}
		else if (phase == 7) {
			if (C.materials[0].color.r < 1) MaterialUp(C);
			t += Time.deltaTime;
			if (t > 14) { phase = 8; tc = 0; }
		}
		else if (phase == 8 && tc <= 1) {
			cam.localPosition = Interpolate(srcPos, dstPos[row], tc);
			cam.localRotation = Quaternion.Lerp(srcRot, dstRot, Mathf.SmoothStep(0, 1, tc));
			if (Title.color.a > 0) {
				c = Title.color; c.a -= 0.05F; Title.color = c;
			}
			if (tc >= 0.55F) {
				if (!bS.gameObject.activeSelf) bS.gameObject.SetActive(true);
				c = bS.color; c.a += 0.025F; bS.color = c;
			}
			tc += 0.01F * Time.deltaTime * 60;
		}
		else if (phase == 8 && tc > 1) phase = 0;
	}

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
