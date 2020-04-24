using UnityEngine;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AccessPoint : MonoBehaviour {
	public Material ap1, ap2, ap3;
	DataControlHub DC; Transform Screens;
	public bool newsInteraction = false;
	float tc; public int phase = -1, phaseScr = 0; float[] ts = new float[4];
	Vector3 SrcPos, DstPos1, DstPos2, DstPos3; Quaternion SrcRot, DstRot; Vector3[] DstShift = new Vector3[4];
	TextMesh t0, t1, t2, t3; StringBuilder s;
	Color c; Renderer ap, p; Light lt; bool once;
	float ppContrastMax;

	float w = 0.4F; Vector3 sclV;
	int currentIND, newIND; Transform SCR; Vector3[] canvasData = new Vector3[5];
	bool seenProjections = false; //this one is not in save data intentionally,
	//so we see the full version every new evening and also after reloading

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		ppContrastMax = DC.camR.GetComponent<PostProcessing>().Contrast;
		Screens = DC.room.Find("Screens"); ap = DC.room.Find("AccessPoint").GetComponent<Renderer>();
		p = ap.transform.Find("Projector").GetComponent<Renderer>(); p.gameObject.SetActive(false);
		lt = ap.transform.Find("Light").GetComponent<Light>(); lt.gameObject.SetActive(false);
		t0 = ap.transform.Find("Text0").GetComponent<TextMesh>(); t0.gameObject.SetActive(false);
		t1 = ap.transform.Find("Text1").GetComponent<TextMesh>(); t1.gameObject.SetActive(false);
		t2 = ap.transform.Find("Text2").GetComponent<TextMesh>(); t2.gameObject.SetActive(false);
		t3 = ap.transform.Find("Text3").GetComponent<TextMesh>(); t3.gameObject.SetActive(false);
		SrcPos = DC.camR.position; SrcRot = DC.camR.rotation; DstRot = Quaternion.Euler(Vector3.zero);
		DstPos1 = new Vector3(21.2F, 1.1F, 1.6F); //this one for a close-up of the projector
		DstPos2 = new Vector3(21.2F, 1.15F, 0.6F); //this one to zoom out and see all 4 screens for the first time
		DstPos3 = new Vector3(21.2F, 1.05F, 1.2F); //and this one like DstPos2 but closer, used when we switch between screens

		var ratio = Screen.width * 1.0F / Screen.height;
		var scl = w / Screen.width; sclV = new Vector3(scl, scl, 1);
		//calculating camera positions to see each screen full-screen
		for (int i = 0; i < 4; i++) {
			var ctr = Screens.GetChild(i).position;
			var cnr = new Vector3[4]; Screens.GetChild(i).GetComponent<RectTransform>().GetWorldCorners(cnr);
			DstShift[i] = Vector3.Cross(cnr[2]-ctr,cnr[3]-ctr).normalized;
			float d = (w/ratio/2)/Mathf.Tan(DC.camR.GetComponent<Camera>().fieldOfView/2*Mathf.PI/180);
			DstShift[i] = DstShift[i] * d;
		}
		//screens are going to hide behind the wall, because if we disable them
		//or make too small to be rendered, they won't render again straight away for some reason
		Screens.localPosition += new Vector3(0, 0, 1);
	}

	void OnMouseDown() {
		if (phase == -1) if (!seenProjections) phase = 0; else phase = 12;
	}

	public void ScreenSwitch(int newScr) {
		if (phase == 10) {
			ChangeCanvasMode(false);
			newIND = newScr; DC.CursorLock(true); DC.bReturn.SetActive(false);
			tc = 0; phase = 8; once = true;
		}
	}

	Color arrowC = new Color(0.7843F, 0.902F, 1);
	Color arrowCA = new Color(0.6275F, 0.88F, 1);
	public void OnSwitchPrevEnter(BaseEventData d) { if (phase == 10) SCR.Find("Header/Date/Prev").GetComponent<Image>().color = arrowCA; }
	public void OnSwitchPrevExit(BaseEventData d) { if (phase == 10) SCR.Find("Header/Date/Prev").GetComponent<Image>().color = arrowC; }
	public void OnSwitchNextEnter(BaseEventData d) { if (phase == 10) SCR.Find("Header/Date/Next").GetComponent<Image>().color = arrowCA; }
	public void OnSwitchNextExit(BaseEventData d) { if (phase == 10) SCR.Find("Header/Date/Next").GetComponent<Image>().color = arrowC; }
	public void OnSwitchPrevClick(BaseEventData d) { if (phase == 10) { ScreenSwitch(currentIND - 1); SCR.Find("Header/Date/Prev").GetComponent<Image>().color = arrowC; } }
	public void OnSwitchNextClick(BaseEventData d) { if (phase == 10) { ScreenSwitch(currentIND + 1); SCR.Find("Header/Date/Next").GetComponent<Image>().color = arrowC; } }

	void ChangeCanvasMode(bool toOverlay) {
		if (toOverlay) {
			SCR = Screens.GetChild(currentIND);
			canvasData[0] = SCR.position;
			canvasData[1] = SCR.localRotation.eulerAngles;
			canvasData[2] = SCR.localScale;
			SCR.SetParent(null); //overlay canvas gets shifted if it has parents with non-zero positions,
			//so we need to change it while it's in this mode
			SCR.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			SCR.GetComponent<Canvas>().pixelPerfect = true;
			SCR.GetComponent<Canvas>().sortingOrder = 0;
		} else {
			SCR.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
			SCR.SetParent(Screens); SCR.SetSiblingIndex(currentIND);
			SCR.position = canvasData[0];
			SCR.localRotation = Quaternion.Euler(canvasData[1]);
			SCR.localScale = canvasData[2];
		}
	}

	void Update() {
		//1. CAMERA ZOOM IN
		if (phase == 0) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			tc = 0; phase = 1;
			//switching from texture to mesh text (no mipmaps visible in zooming, but no text "jumps")
			//t0.gameObject.SetActive(true); var m = ap.materials; m[1] = ap2; ap.materials = m;
		}
		else if (phase == 1 && tc <= 1) {
			tc += 0.00588F * Time.deltaTime * 60; //0.005F
			DC.camR.transform.position = Vector3.Lerp(SrcPos, DstPos1, Mathf.SmoothStep(0, 1, tc));
			DC.camR.transform.rotation = Quaternion.Lerp(SrcRot, DstRot, Mathf.SmoothStep(0, 1, tc));
		}
		//2. LOADING MESSAGES
		else if (phase == 1 && tc > 1) {
			tc = 0; phase = 2;
			//alternative for what's in phase = 0
			t0.gameObject.SetActive(true); var m = ap.materials; m[1] = ap2; ap.materials = m;
		}
		else if (phase == 2 && tc <= 0.34F) tc += Time.deltaTime;
		else if (phase == 2 && tc > 0.34F) { t0.color = Color.white; tc = 0; phase = 21; }
		else if (phase == 21 && tc <= 0.67F) tc += Time.deltaTime;
		else if (phase == 21 && tc > 0.67F) { t0.gameObject.SetActive(false); tc = 0; phase = 22; }
		else if (phase == 22 && tc <= 0.34F) tc += Time.deltaTime;
		else if (phase == 22 && tc > 0.34F) {
			tc = 0; phase = 3;
			s = new StringBuilder(t1.text); t1.text = ""; t1.gameObject.SetActive(true);
		}
		else if (phase == 3 && s.Length > 0) {
			tc += Time.deltaTime; if (tc > 0.033F) { t1.text = t1.text + s[0]; s.Remove(0, 1); tc = 0; }
		}
		else if (phase == 3 && s.Length == 0 && tc <= 2) tc += Time.deltaTime;
		else if (phase == 3 && s.Length == 0 && tc > 2) {
			tc = 0; phase = 4; t1.gameObject.SetActive(false);
			s = new StringBuilder(t2.text); t2.text = ""; t2.gameObject.SetActive(true);
		}
		else if (phase == 4 && s.Length > 0) {
			tc += Time.deltaTime; if (tc > 0.033F) { t2.text = t2.text + s[0]; s.Remove(0, 1); tc = 0; }
		}
		else if (phase == 4 && s.Length == 0 && tc <= 2) tc += Time.deltaTime;
		else if (phase == 4 && s.Length == 0 && tc > 2) {
			tc = 0; phase = 5; t2.gameObject.SetActive(false);
			var m = ap.materials; m[1] = ap3; ap.materials = m;
			c = t3.color; c.a = 1; t3.color = c;
			s = new StringBuilder(t3.text); t3.text = ""; t3.gameObject.SetActive(true);
		}
		else if (phase == 5 && s.Length > 0) {
			tc += Time.deltaTime; if (tc > 0.1F) { t3.text = t3.text + s[0]; s.Remove(0, 1); tc = 0; }
		}
		else if (phase == 5 && s.Length == 0 && tc <= 1) tc += Time.deltaTime;
		//3. PROJECTOR AND CAMERA
		else if (phase == 5 && s.Length == 0 && tc > 1) {
			tc = 0; phase = 6; once = true;
			p.gameObject.SetActive(true); p.material.color = Color.black;
			lt.gameObject.SetActive(true); lt.intensity = 0;
		}
		else if (phase == 6 && tc <= 1) {
			if (p.material.color.r < 0.8F) {
				var clrInc = 0.016F * Time.deltaTime * 60; p.material.color += new Color(clrInc, clrInc, clrInc, 1);
				c = t3.color; c.a -= 0.02F * Time.deltaTime * 60; t3.color = c;
				lt.intensity += 0.04F * Time.deltaTime * 60;
			}
			if (p.material.color.r > 0.32F) {
				DC.camR.transform.position = Vector3.Lerp(DstPos1, DstPos2, Mathf.SmoothStep(0, 1, tc));
				tc += 0.01F * Time.deltaTime * 60;
			}
			if (p.material.color.r >= 0.8F && once) {
				once = false; t3.gameObject.SetActive(false);
				phaseScr = 1; Screens.localPosition -= new Vector3(0, 0, 1); for (int i = 0; i < 4; i++) ts[i] = 0;
			}
		}
		else if (phase == 6 && tc > 1) { currentIND = DC.S.levelID - 11; tc = 0; phase = 7; }
		else if (phase == 7 && tc <= 1) {
			var scr = Screens.GetChild(currentIND);
			DC.camR.transform.position = Vector3.Lerp(DstPos2, scr.position + DstShift[currentIND], Mathf.SmoothStep(0, 1, tc));
			DC.camR.transform.localRotation = Quaternion.Lerp(DstRot, scr.localRotation, Mathf.SmoothStep(0, 1, tc));
			if (tc >= 0.8F) DC.camR.GetComponent<PostProcessing>().Contrast = Mathf.Lerp(ppContrastMax, 0, (tc - 0.8F) * 5);
			tc += 0.01F * Time.deltaTime * 60;
		}
		else if (phase == 7 && tc > 1) {
			ChangeCanvasMode(true);
			phase = 10; DC.CursorLock(false); DC.bReturn.SetActive(true);
		}

		//----------SHORT VERSION----------
		else if (phase == 12) {
			var m = ap.materials; m[1] = ap3; ap.materials = m;
			p.gameObject.SetActive(true); p.material.color = Color.black;
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			tc = 0; phase = 13;
		}
		else if (phase == 13 && tc < 0.67F) tc += Time.deltaTime;
		else if (phase == 13 && p.material.color.r < 0.8F) {
			var clrInc = 0.016F * Time.deltaTime * 60;
			p.material.color += new Color(clrInc, clrInc, clrInc, 1);
		}
		else if (phase == 13 && p.material.color.r >= 0.8F) {
			phaseScr = 1; Screens.localPosition -= new Vector3(0, 0, 1); for (int i = 0; i < 4; i++) ts[i] = 0;
			currentIND = DC.S.levelID - 11; phase = 14; tc = 0;
		}
		else if (phase == 14 && tc <= 1) {
			DC.camR.transform.position = Vector3.Lerp(SrcPos, Screens.GetChild(currentIND).position + DstShift[currentIND], Mathf.SmoothStep(0, 1, tc));
			DC.camR.transform.localRotation = Quaternion.Lerp(SrcRot, Screens.GetChild(currentIND).localRotation, Mathf.SmoothStep(0, 1, tc));
			if (tc >= 0.8F) DC.camR.GetComponent<PostProcessing>().Contrast = Mathf.Lerp(ppContrastMax, 0, (tc - 0.8F) * 5);
			tc += 0.00588F * Time.deltaTime * 60; //0.005F;
		}
		else if (phase == 14 && tc > 1) {
			ChangeCanvasMode(true);
			phase = 10; DC.CursorLock(false); DC.bReturn.SetActive(true);
		}

		//----------ZOOM OUT----------
		else if (phase == 10 && !newsInteraction && DC.bReturn.activeSelf && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			ChangeCanvasMode(false);
			DC.CursorLock(true); DC.bReturn.SetActive(false);
			DC.bReturn.GetComponent<ButtonArrow>().clicked = false;
			tc = 0; once = true; phase = 11;
		}
		else if (phase == 11 && tc <= 1) {
			DC.camR.transform.position = Vector3.Lerp(Screens.GetChild(currentIND).position + DstShift[currentIND], SrcPos, Mathf.SmoothStep(0, 1, tc));
			DC.camR.transform.localRotation = Quaternion.Lerp(Screens.GetChild(currentIND).localRotation, SrcRot, Mathf.SmoothStep(0, 1, tc));
			if (tc <= 0.2F) DC.camR.GetComponent<PostProcessing>().Contrast = Mathf.Lerp(0, ppContrastMax, tc * 5);
			if (tc >= 0.3F/*0.2F*/ && once) { once = false; phaseScr = 1; for (int i = 0; i < 4; i++) ts[i] = 0; }
			if (tc >= 0.3F/*0.2F*/ && p.material.color.r > 0) p.material.color -= new Color(0.016F, 0.016F, 0.016F, 1);
			tc += 0.005F/*0.006F*/ * Time.deltaTime * 60;
			//also revert seenProjections at the beginning
		}
		else if (phase == 11 && tc > 1) {
		p.gameObject.SetActive(false);
			seenProjections = true; phase = -1;
			DC.CursorLock(false); DC.UIC.Col(true); DC.bMenu.SetActive(true);
		}

		//----------SCREEN SWITCH----------
		else if (phase == 8 && tc <= 1) {
			DC.camR.transform.position = Vector3.Lerp(Screens.GetChild(currentIND).position + DstShift[currentIND], DstPos3, Mathf.SmoothStep(0, 1, tc));
			DC.camR.transform.localRotation = Quaternion.Lerp(Screens.GetChild(currentIND).localRotation, DstRot, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01333333F * Time.deltaTime * 60;
		}
		else if (phase == 8 && tc > 1) { 
			tc = 0; phase = 9;
		}
		else if (phase == 9 && tc <= 1) {
			DC.camR.transform.position = Vector3.Lerp(DstPos3, Screens.GetChild(newIND).position + DstShift[newIND], Mathf.SmoothStep(0, 1, tc));
			DC.camR.transform.localRotation = Quaternion.Lerp(DstRot, Screens.GetChild(newIND).localRotation, Mathf.SmoothStep(0, 1, tc));
			tc += 0.01333333F * Time.deltaTime * 60;
		}
		else if (phase == 9 && tc > 1) {
			currentIND = newIND;
			ChangeCanvasMode(true);
			phase = 10; DC.CursorLock(false); DC.bReturn.SetActive(true);
		}
		//testing purposes
		//if (Input.GetKeyDown(KeyCode.Alpha1)) ScreenSwitch(0);
		//if (Input.GetKeyDown(KeyCode.Alpha2)) ScreenSwitch(1);
		//if (Input.GetKeyDown(KeyCode.Alpha3)) ScreenSwitch(2);
		//if (Input.GetKeyDown(KeyCode.Alpha4)) ScreenSwitch(3);

		//----------SCREENS-----------
		if (phaseScr == 1 && ts[3] <= 1) {
			Screens.GetChild(0).GetComponent<RectTransform>().localScale = Vector3.Lerp(Vector3.zero, sclV, phase != 11 ? ts[0] : 1 - ts[0]);
			Screens.GetChild(1).GetComponent<RectTransform>().localScale = Vector3.Lerp(Vector3.zero, sclV, phase != 11 ? ts[1] : 1 - ts[1]);
			Screens.GetChild(2).GetComponent<RectTransform>().localScale = Vector3.Lerp(Vector3.zero, sclV, phase != 11 ? ts[2] : 1 - ts[2]);
			Screens.GetChild(3).GetComponent<RectTransform>().localScale = Vector3.Lerp(Vector3.zero, sclV, phase != 11 ? ts[3] : 1 - ts[3]);
			var speedo = 0.02F * Time.deltaTime * 60;
			ts[0] += speedo;
			if (ts[0] >= 0.5F) ts[1] += speedo;
			if (ts[1] >= 0.5F) ts[2] += speedo;
			if (ts[2] >= 0.5F) ts[3] += speedo;
		}
		else if (phaseScr == 1 && ts[3] > 1) {
			phaseScr = 0;
			if (phase == 11) {
				Screens.localPosition += new Vector3(0, 0, 1);
				for (int i = 0; i < 4; i++) Screens.GetChild(i).GetComponent<RectTransform>().localScale = sclV;
				var m = ap.materials; m[1] = ap1; ap.materials = m;
			}
		}
	}
}
