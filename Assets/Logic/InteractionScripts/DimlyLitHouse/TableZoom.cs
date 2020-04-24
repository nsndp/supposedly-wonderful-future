using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TableZoom : MonoBehaviour {

	public Camera cam;
	public Transform Booklet;
	Vector3 DstPos = new Vector3(1.6F, 0.72F, -1.2F);
	Vector3 DstRotEuler = new Vector3(90, 0, 0);
	Vector3 SrcPos; Quaternion SrcRot, DstRot;
	GameObject T, B, P;
	DataControlChapter1 DC;
	public int phase = 0; public float tc;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		T = GameObject.Find("Livingroom/Colliders").transform.Find("Table").gameObject;
		B = GameObject.Find("Livingroom/Colliders").transform.Find("Booklet").gameObject;
		P = GameObject.Find("Livingroom/Colliders").transform.Find("Pill").gameObject;
		SrcPos = cam.transform.localPosition;
		SrcRot = cam.transform.rotation;
		DstRot = Quaternion.Euler(DstRotEuler);
	}

	void Update() {
		//camera zoom in
		if (phase == 1 && tc <= 1) {
			tc += 0.008F * Time.deltaTime * 60;
			cam.transform.localPosition = Interpolate(SrcPos, DstPos, tc, 1); //Vector3.Lerp(SrcPos, DstPos, tc);
			cam.transform.rotation = Quaternion.Lerp(SrcRot, DstRot, tc);
		}
		else if (phase == 1 && tc > 1) {
			DC.CursorLock(false); DC.bReturn.SetActive(true);
			T.SetActive(false); B.SetActive(true); DC.UIC.Col(true);
			phase = 2;
		}
		//camera zoom out
		else if (phase == 2 && DC.bReturn.activeSelf && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.CursorLock(true); DC.UIC.Col(false);
			DC.bReturn.SetActive(false); DC.bReturn.GetComponent<ButtonArrow>().clicked = false;
			phase = 3; tc = 0;
		}
		else if (phase == 3 && tc <= 1) {
			tc += 0.008F * Time.deltaTime * 60;
			cam.transform.localPosition = Interpolate(DstPos, SrcPos, tc, 1); //Vector3.Lerp(DstPos, SrcPos, tc);
			cam.transform.rotation = Quaternion.Lerp(DstRot, SrcRot, tc);
		}
		else if (phase == 3 && tc > 1) {
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			T.SetActive(true); B.SetActive(false); DC.UIC.Col(true);
			phase = 0;
		}
		//booklet open
		else if (phase == 4) {
			Cursor.visible = false; DC.bReturn.SetActive(false); DC.UIC.Col(false);
			phase = 5; tc = 0;
		}
		else if (phase == 5 && tc <= 1) {
			tc += 0.01666666F * Time.deltaTime * 60;
			Booklet.localRotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(180, 0, tc), 0));
			if (!DC.S.SC1.pickedPill && tc >= 0.2F) Booklet.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, (1 - 1.25F*(tc-0.2F)) * 100);
			Booklet.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, 4*tc * 100);
			if (tc >= 0.75F) Booklet.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(1, (1 - 4*(tc-0.75F)) * 100);
		}
		else if (phase == 5 && tc > 1) {
			Cursor.visible = true; DC.bReturn.SetActive(true);
			B.SetActive(false);
			P.SetActive(!DC.S.SC1.DSWoman.Used[122] || DC.S.SC1.choseEuthanasia && !DC.S.SC1.pickedPill);
			DC.UIC.Col(true); phase = 6;
		}
		//booklet closing and camera zoom out
		else if (phase == 6 && DC.bReturn.activeSelf && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.CursorLock(true); DC.UIC.Col(false);
			DC.bReturn.SetActive(false); DC.bReturn.GetComponent<ButtonArrow>().clicked = false;
			phase = 8; tc = 0;
		}
		else if (phase == 7 && tc <= 1) {
			tc += 0.01666666F * Time.deltaTime * 60;
			Booklet.localRotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(0, 180, tc), 0));
			if (!DC.S.SC1.pickedPill) Booklet.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 1.25F*tc * 100);
		}
		else if (phase == 7 && tc > 1) {
			phase = 8; tc = 0;
		}
		else if (phase == 8 && tc <= 1) {
			tc += 0.008F * Time.deltaTime * 60;
			cam.transform.localPosition = Interpolate(DstPos, SrcPos, tc, 1); //Vector3.Lerp(DstPos, SrcPos, tc);
			cam.transform.rotation = Quaternion.Lerp(DstRot, SrcRot, tc);
			Booklet.localRotation = Quaternion.Euler(new Vector3(0, Mathf.Lerp(0, 180, 2*tc), 0));
			if (!DC.S.SC1.pickedPill && tc >= 0.2F) Booklet.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0, 1.25F*(tc-0.2F) * 100);
		}
		else if (phase == 8 && tc > 1) {
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			T.SetActive(true); B.SetActive(false); P.SetActive(false); DC.UIC.Col(true);
			phase = 0;
		}
		//afterstory comment
		else if (phase == 9) {
			DC.UIC.Col(false);
			DC.UIC.DisplayComment(DC.Comments.GetLine(81));
			Cursor.visible = false; DC.bMenu.SetActive(false); phase = 10;
		}
		else if (phase == 10 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.UIC.HideComment();
			Cursor.visible = true; DC.bMenu.SetActive(true); DC.UIC.Col(true);
			phase = -1; T.SetActive(false);
			DC.S.SC1.postCheckedTable = true;
		}
	}

	//insert the function you need instead of SQRT
	//smoothing is good for quadratic interpolation to slow it down on the edges
	Vector3 Interpolate(Vector3 src, Vector3 dst, float t, int numberOfSmooths) {
		var v = Vector3.zero;
		for (int i = 0; i < numberOfSmooths; i++) t = Mathf.SmoothStep(0, 1, t);
		v.x = (Mathf.Lerp(src.x, dst.x, t) - (src.x < dst.x ? src.x : dst.x)) / Mathf.Abs(dst.x - src.x);
		v.y = (Mathf.Lerp(src.y, dst.y, t) - (src.y < dst.y ? src.y : dst.y)) / Mathf.Abs(dst.y - src.y);
		v.z = (Mathf.Lerp(src.z, dst.z, t) - (src.z < dst.z ? src.z : dst.z)) / Mathf.Abs(dst.z - src.z);
		v.x = Mathf.Sqrt(v.x)*Mathf.Abs(dst.x - src.x) + (src.x < dst.x ? src.x : dst.x);
		v.y = Mathf.Sqrt(v.y)*Mathf.Abs(dst.y - src.y) + (src.y < dst.y ? src.y : dst.y);
		v.z = Mathf.Sqrt(v.z)*Mathf.Abs(dst.z - src.z) + (src.z < dst.z ? src.z : dst.z);
		return v;
	}
}
