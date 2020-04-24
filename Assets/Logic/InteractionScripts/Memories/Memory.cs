using UnityEngine;
using System.Collections;

public class Memory : MonoBehaviour {

	public Transform OBJ; public int MI; public Texture2D[] pics;
	public Texture2D colorPic; public MovieTexture mov, movLowRes; //if you want to use only colorPic and not movie, just leave mov as null (but colorPic is needed either way)
	DataControlChapter4 DC; MeshRenderer imgA, imgB;
	public Vector3 dstPos; public Quaternion dstRot;
	int ind, phase = 0, camPhase = 0; float tc, tcc; Color c;
	public bool animate = false; int? sender;
	//cracks-related
	GameObject[] cr = new GameObject[10]; int cc = 0;
	int[] unused = new int[] {0, 1, 2, 3, 4, 5, 6, 7}; int l = 8;
	float greyAST = 0.882F;
	float[] greys = new float[] {0.737F, 0.741F, 0.753F, 0.714F, 0.71F, 0.698F, 0.741F, 0.718F}; //188, 189, 192, 182, 181, 178, 189, 183
	Vector3[] pos = new Vector3[] {
		new Vector3(0.012F, -0.01F, 0.1F), new Vector3(0.012F, 0.02F, 0.06F), new Vector3(0.012F, -0.09F, 0.06F),
		new Vector3(0.012F, 0.09F, 0.13F), new Vector3(0.012F, 0.065F, 0.08F), new Vector3(0.012F, 0.035F, 0.12F),
		new Vector3(0.012F, -0.06F, 0.135F), new Vector3(0.012F, -0.053F, 0.085F),
		new Vector3(0.012F, -0.094F, 0.11F), new Vector3(0.012F, 0.1F, 0.05F)
	};

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter4>();
		//pics = Resources.LoadAll<Texture2D>(folders[MI]);
		imgA = OBJ.transform.Find("ImageA").GetComponent<MeshRenderer>();
		imgB = OBJ.transform.Find("ImageB").GetComponent<MeshRenderer>();

		var c = imgA.transform.position;
		var v = imgA.GetComponent<MeshFilter>().mesh.vertices;
		var h = (v[0].z - v[1].z) * imgA.transform.parent.localScale.z;
		var shift = Vector3.Cross(imgA.transform.TransformPoint(v[1])-c,imgA.transform.TransformPoint(v[2])-c).normalized;
		var d = (h/2)/Mathf.Tan(DC.cam.GetComponent<Camera>().fieldOfView/2*Mathf.PI/180);
		dstPos = c + shift * d;
		dstRot = Quaternion.Euler(imgA.transform.rotation.eulerAngles - new Vector3(270, 0, 90));

		if (DC.S.SC4.shatterMe) { imgB.gameObject.SetActive(false); imgA.material.mainTexture = colorPic; }
		else if (DC.S.SC4.sawReveal) { Untrasparent(); ind = 0; phase = 1; Update(); }
		else if (DC.S.SC4.memoryActive[MI]) { animate = DC.S.SC4.curM == MI; ind = 0; phase = 1; Update(); }
		else {
			imgA.gameObject.SetActive(false); imgB.gameObject.SetActive(false);
			var m = OBJ.GetComponent<Renderer>().materials; m[0].color = new Color(0.04F, 0.04F, 0.04F); OBJ.GetComponent<Renderer>().materials = m;
			this.gameObject.SetActive(false);
		}
	}

	void OnMouseDown() {
		DC.GetComponent<AreaZoom>().phase = 0; DC.bReturn.SetActive(false); //for the colorful part
		DC.UIC.Col(false); DC.bMenu.SetActive(false); DC.CursorLock(true);
		DC.S.SC4.curM = MI; camPhase = 1; tcc = 0;
	}
	IEnumerator WaitingC(float sec, int ph) { camPhase = 0; yield return new WaitForSeconds(sec); camPhase = ph; }
	IEnumerator Waiting(float sec, int ph) {
		phase = 0; yield return new WaitForSeconds(sec); phase = ph;
		if (ph == 2 && !animate) phase = 0; //in case we're already zooming out at this point
	}

	//FRAME ACTIVATION AFTER ZOOMING OUT
	public void FinDlg() { camPhase = 4; DC.UIC.Col(false); DC.bMenu.SetActive(false); DC.bReturn.SetActive(true); }
	public void ActivationFinished() { camPhase = 7; }
	public void Activate(int? snd) {
		c = imgA.material.color; c.a = 0; imgA.material.color = c; imgA.gameObject.SetActive(true);
		c = imgB.material.color; c.a = 0; imgB.material.color = c; imgB.gameObject.SetActive(true);
		imgA.material.mainTexture = pics[0];
		sender = snd; tc = 0; phase = 3;
	}

	//CRACKS
	public void Untrasparent() {
		//cracks will disappear under certain camera angles because of 2+ transparent objects sorting issue,
		//so while they are around (after reveal), we need to change imgA material
		imgB.gameObject.SetActive(false);
		imgA.material.shader = Shader.Find("Self-Illumin/Diffuse");
		imgA.material.color = new Color(greys[MI], greys[MI], greys[MI]);
	}
	public void Colorize() {
		for (int a = 0; a < cc; a++) cr[a].SetActive(false);
		imgA.material.mainTexture = colorPic;
	}
	public void MovieFirstTime() {
		imgA.material.mainTexture = (COMMON.U.videosLowRes && movLowRes != null) ? movLowRes : mov;
		((MovieTexture)imgA.material.mainTexture).loop = true;
		((MovieTexture)imgA.material.mainTexture).Stop();
		((MovieTexture)imgA.material.mainTexture).Play();
	}
	public void ResetMovie() { //used only if video res changed in settings
		imgA.material.mainTexture = colorPic;
	}
	public void PauseMovie(bool pause) { //used only for pausing
		if (pause) ((MovieTexture)imgA.material.mainTexture).Pause();
		else ((MovieTexture)imgA.material.mainTexture).Play();
	}
	IEnumerator Crack(int ind, bool anim, float delaySec) {
		cr[ind] = (GameObject)Object.Instantiate(OBJ.Find("ImageCrack").gameObject); cr[ind].transform.parent = OBJ;
		int RES = 0; if (ind == 8 || ind == 9) RES = ind; //first 8 cracks' order is randomized, but last 2 are fixed
		else { int a = Random.Range(0, l); RES = unused[a]; unused[a] = unused[l-1]; l--; }
		cr[ind].transform.localPosition = pos[RES];
		cr[cc].transform.localRotation = Quaternion.Euler(Random.Range(0, 359), 0, 0);
		cr[cc].transform.localScale = Vector3.one;
		int type = Random.Range(0, 4); cr[ind].GetComponent<Renderer>().material.mainTexture = DC.NB.cracksImg[type];
		if (delaySec != 0) yield return new WaitForSeconds(delaySec);
		cr[ind].gameObject.SetActive(true); DC.Sound.Play();
		if (anim) yield return new WaitForSeconds(0.2F);
		cr[ind].GetComponent<Renderer>().material.mainTexture = DC.NB.cracksImg[type+4];
	}
	public void ReloadCracks(int howMany) {
		for (int a = 0; a < howMany; a++) { StartCoroutine(Crack(cc, false, 0)); cc++; }
	}
	public void MakeCracks(int howMany, bool whileZooming) {
		float t = 0; for (int a = 0; a < howMany; a++) {
			StartCoroutine(Crack(cc, true, t)); cc++;
			if (howMany < 3 || howMany == 3 && a != 1 || howMany == 4 && a != 0 && a != 2) t += 0.6F; //2 cracks: 1+1, 3: 1+2, 4: 2+2
		}
		if (whileZooming) StartCoroutine(WaitingC(t, 2));
	}
	public void FadeCrack() { if (phase == 4) cr[cc].SetActive(false); cc--; tc = 0; phase = 4; }
	public void FadeLastCracks() { if (phase == 4) cr[cc].SetActive(false); tc = 0; phase = 5; }
	public void ShatterMe() {
		float t = 0; for (int a = 0; a < 4; a++) { StartCoroutine(Crack(cc, true, t)); cc++; t += 0.4F; }
		c = imgB.material.color; c.a = 0; imgB.material.color = c; imgB.material.mainTexture = null;
		imgB.transform.localPosition = new Vector3(0.02F, 0, 0.095F); imgB.gameObject.SetActive(true);
		tc = 0; StartCoroutine(Waiting(1.0F, 6));
	}

	void Update() {
		//for trailer
		//if (MI == 1 && Input.GetKeyDown(KeyCode.Q)) { DC.cam.position = dstPos; DC.cam.rotation = dstRot; }
		//if (MI == 1 && Input.GetKeyDown(KeyCode.W)) MakeCracks(4, false);
		//if (MI == 1 && Input.GetKeyDown(KeyCode.Q)) DC.NB.ShowScreen();

		if (DC.paused) return;
		if (camPhase == 1 && tcc <= 1) {
			DC.cam.position = Vector3.Lerp(DC.camPos, dstPos, Mathf.SmoothStep(0, 1, tcc));
			DC.cam.rotation = Quaternion.Lerp(DC.camRot, dstRot, Mathf.SmoothStep(0, 1, tcc));
			tcc += 0.008333333F * Time.deltaTime * 60;
		}
		else if (camPhase == 1 && tcc > 1) {
			if (DC.S.SC4.shatterMe && mov != null) {
				if (!(imgA.material.mainTexture is MovieTexture)) MovieFirstTime();
				else ((MovieTexture)imgA.material.mainTexture).Play();
			}
			else if (DC.S.SC4.firstGlimpse && !DC.S.SC4.sawReveal) { animate = true; phase = 2; }

			if (DC.S.SC4.memoryDID[MI] == -1) { camPhase = 4; DC.CursorLock(false); DC.bReturn.SetActive(true); }
			else if (DC.S.SC4.sawReveal && !DC.S.SC4.sawFinishP1) MakeCracks(2, true);
			else if (DC.S.SC4.sawFinishP1 && !DC.S.SC4.sawFinishP2) MakeCracks(Mathf.Min(3-cc), true);
			else if (DC.S.SC4.sawFinishP2) MakeCracks(4-cc, true);
			else if (!DC.S.SC4.memoryDelay[MI]) camPhase = 2;
			else StartCoroutine(WaitingC(5.2F, 2));
		}
		else if (camPhase == 2) {
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			DC.S.SC4.nextDID = DC.S.SC4.memoryDID[MI];
			DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, DC.S.SC4.nextDID, false);
			DC.S.SC4.memoryDelay[MI] = false;
			camPhase = 3;
		}
		else if (camPhase == 4 && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.bReturn.SetActive(false); DC.CursorLock(true);
			animate = false; camPhase = 5; tcc = 0;
			if (DC.S.SC4.shatterMe && mov != null) ((MovieTexture)imgA.material.mainTexture).Pause();
		}
		else if (camPhase == 5 && tcc <= 1) {
			DC.cam.position = Vector3.Lerp(dstPos, DC.camPos, Mathf.SmoothStep(0, 1, tcc));
			DC.cam.rotation = Quaternion.Lerp(dstRot, DC.camRot, Mathf.SmoothStep(0, 1, tcc));
			tcc += 0.008333333F * Time.deltaTime * 60;
		}
		else if (camPhase == 5 && tcc > 1) {
			//if new frames are being activated
			if (!DC.S.SC4.memoryActive[3] && DC.S.SC4.memorySeen[1] && DC.S.SC4.memorySeen[2]) {
				DC.S.SC4.memoryDID[3] = 220; DC.S.SC4.memoryDID[4] = 250;
				DC.M[3].gameObject.SetActive(true); DC.M[4].gameObject.SetActive(true);
				DC.M[3].Activate(MI); DC.M[4].Activate(null); camPhase = 6; //"null" so only one of them triggers ActivationFinished
				DC.MChange(DC.mFrames, 17.993F);
			}
			else if (!DC.S.SC4.memoryActive[5] && (DC.S.SC4.memorySeen[3] || DC.S.SC4.memorySeen[4])) {
				DC.S.SC4.memoryDID[5] = 280; DC.S.SC4.memoryDID[6] = 320;
				DC.M[5].gameObject.SetActive(true); DC.M[6].gameObject.SetActive(true);
				DC.M[5].Activate(MI); DC.M[6].Activate(null); camPhase = 6;
			}
			else if (!DC.S.SC4.memoryActive[7] && (DC.S.SC4.memorySeen[5] || DC.S.SC4.memorySeen[6])) {
				DC.S.SC4.memoryDID[7] = 355; DC.M[7].gameObject.SetActive(true);
				DC.M[7].Activate(MI); camPhase = 6;
			}
			else if (DC.S.SC4.memorySeen[7] && !DC.S.SC4.sawReveal && !DC.S.SC4.activatedNotebook) {
				DC.NB.ShowScreen(); camPhase = 6;
			}
			//if in post-finish colorful mode
			else if (DC.S.SC4.shatterMe) {
				DC.bReturn.SetActive(true); DC.UIC.Col(true); DC.CursorLock(false);
				DC.GetComponent<AreaZoom>().phase = 3;
				DC.S.SC4.curM = -1; camPhase = 0;
			}
			//otherwise
			else {
				DC.bMenu.SetActive(true); DC.CursorLock(false); DC.UIC.Col(true);
				DC.S.SC4.curM = -1; camPhase = 0;
				if (!DC.S.SC4.firstGlimpse) {
					DC.S.SC4.firstGlimpse = true; DC.NmPlt.SetActive(true);
					DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, DC.S.SC4.nextDID, false);
					DC.MPlay(DC.mStart, 0);
				}
			}
		}
		else if (camPhase == 7) {
			DC.bMenu.SetActive(true); DC.CursorLock(false); DC.UIC.Col(true);
			DC.S.SC4.curM = -1; camPhase = 0;
			if (!DC.S.SC4.memoryActive[5]) { DC.S.SC4.nextDID = 90; DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, 90, false); }
		}

		//MAIN EMBOSS IMAGES LOOP
		if (phase == 1) {
			imgA.material.mainTexture = pics[ind];
			ind++; if (ind >= pics.Length) ind = 0;
			imgB.material.mainTexture = pics[ind];
			c = imgA.material.color; c.a = 1; imgA.material.color = c;
			c = imgB.material.color; c.a = 0; imgB.material.color = c;
			tc = 0; if (!animate) phase = 0; else StartCoroutine(Waiting(1.0F, 2));
		}
		else if (phase == 2 && tc <= 1) {
			tc += 0.02F * Time.deltaTime * 60; //tc += 0.01666666666F;
			c = imgA.material.color; c.a = 1-tc; imgA.material.color = c;
			c = imgB.material.color; c.a = tc; imgB.material.color = c;
		}
		else if (phase == 2 && tc > 1) phase = 1;

		//ACTIVATION
		else if (phase == 3 && tc <= 1) {
			tc += 0.01666666666F * Time.deltaTime * 60; c = imgA.material.color; c.a = tc; imgA.material.color = c;
		}
		else if (phase == 3 && tc > 1) {
			var m = OBJ.GetComponent<Renderer>().materials; m[0].color = new Color(greyAST, greyAST, greyAST); OBJ.GetComponent<Renderer>().materials = m;
			DC.S.SC4.memoryActive[MI] = true;
			if (sender != null) DC.M[(int)sender].ActivationFinished();
			phase = 0;
		}

		//CRACKS FADE
		else if (phase == 4 && tc <= 1) {
			tc += 0.008F * Time.deltaTime * 60;
			c = cr[cc].GetComponent<Renderer>().material.color; c.a = 1-tc; cr[cc].GetComponent<Renderer>().material.color = c;
		}
		else if (phase == 4 && tc > 1) {
			cr[cc].SetActive(false); phase = 0;
		}
		else if (phase == 5 && tc <= 1) {
			tc += 0.008F * Time.deltaTime * 60;
			c = cr[0].GetComponent<Renderer>().material.color; c.a = 1-tc; cr[0].GetComponent<Renderer>().material.color = c;
			c = cr[1].GetComponent<Renderer>().material.color; c.a = 1-tc; cr[1].GetComponent<Renderer>().material.color = c;
		}
		else if (phase == 5 && tc > 1) {
			cr[0].SetActive(false); cr[1].SetActive(false);
			imgA.gameObject.SetActive(true); imgB.gameObject.SetActive(true);
			imgA.material.shader = Shader.Find("AlphaSelfIllum");
			imgA.material.color = new Color(greyAST, greyAST, greyAST);
			animate = true; tc = 0; StartCoroutine(Waiting(1.0F, 2));
			DC.UIC.DelayedTrigger(8.0F, (int)events4.chapterEnd);
		}

		//SHATTER ME
		else if (phase == 6 && tc <= 1) {
			tc += 0.02F * Time.deltaTime * 60; c = imgB.material.color; c.a = tc; imgB.material.color = c;
		}
		else if (phase == 6 && tc > 1) {
			for (int i = 0; i < 8; i++) DC.M[i].Colorize();
			StartCoroutine(Waiting(2.0F, 7));
		}
		else if (phase == 7) {
			if (mov == null) imgA.material.mainTexture = colorPic; else MovieFirstTime();
			phase = 8; tc = 0;
		}
		else if (phase == 8 && tc <= 1) {
			tc += 0.01F * Time.deltaTime * 60; c = imgB.material.color; c.a = 1-tc; imgB.material.color = c;
		}
		else if (phase == 8 && tc > 1) {
			imgB.gameObject.SetActive(false);
			DC.NB.OBJ.Find("ImgA").gameObject.SetActive(false);
			DC.NB.OBJ.Find("ImgB").gameObject.SetActive(false);
			DC.S.SC4.activatedNotebook = false;
			DC.S.SC4.shatterMe = true; DC.S.SC4.memoryDelay = new bool[8];
			DC.S.SC4.memoryDID = new int[] {-1, -1, -1, -1, -1, -1, -1, -1};
			StartCoroutine(Waiting(2.0F, 9));
		}
		else if (phase == 9) {
			DC.bMenu.SetActive(true); DC.CursorLock(false);
			DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, DC.S.SC4.nextDID, false);
			phase = 0;
		}
	}
}