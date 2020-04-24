using UnityEngine;
using System.Collections;

public class OnClick_Notebook : MonoBehaviour {

	public Transform OBJ; public Texture2D[] cracksImg; DataControlChapter4 DC;
	MeshRenderer imgA, imgB; Color c; int phase = 0; float tc;
	float newGrey = 0.753F;
	Vector3[] pos = new Vector3[] {
		new Vector3(-0.009F, -0.095F, 0.018F), new Vector3(0.004F, -0.075F, 0.014F),
		new Vector3(-0.015F, -0.055F, 0.015F), new Vector3(0, -0.047F, 0.017F)
	};

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter4>();
		imgA = OBJ.transform.Find("ImgA").GetComponent<MeshRenderer>();
		imgB = OBJ.transform.Find("ImgB").GetComponent<MeshRenderer>();
	}

	public void ShowScreen() {
		this.gameObject.SetActive(true);
		imgA.gameObject.SetActive(true);
		c = imgA.material.color; c.a = 0; imgA.material.color = c;
		tc = 0; phase = 1;
	}
	void OnMouseDown() {
		DC.UIC.Col(false); DC.bMenu.SetActive(false); DC.CursorLock(true);
		tc = 0; phase = 2; DC.MStop();
	}
	public void ChangeImages() {
		DC.UIC.Col(false); DC.bMenu.SetActive(false);
		OBJ.Find("Img0").gameObject.SetActive(true); imgB.gameObject.SetActive(true);
		c = imgB.material.color; c.a = 0; imgB.material.color = c;
		tc = 0; StartCoroutine(Waiting(1.0F, 4));
	}
	public void Crack(int number, bool delayed) {
		var cr = (GameObject)Object.Instantiate(OBJ.Find("ImgCrack").gameObject); cr.transform.parent = OBJ;
		cr.transform.localPosition = pos[number]; cr.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 359));
		cr.GetComponent<Renderer>().material.mainTexture = cracksImg[number];
		StartCoroutine(CrackCont(cr, number, delayed));
	}
	IEnumerator CrackCont(GameObject cr, int number, bool delayed) {
		if (delayed) yield return new WaitForSeconds(0.6F);
		cr.SetActive(true); DC.Sound.Play();
		yield return new WaitForSeconds(0.2F);
		cr.GetComponent<Renderer>().material.mainTexture = cracksImg[number+4];
	}
	public void ZoomOut() {
		for (int a = 0; a < 8; a++) DC.M[a].Untrasparent();
		DC.UIC.Col(false); DC.bMenu.SetActive(false);
		DC.CursorLock(true); tc = 0; phase = 6;
	}
	public void Reload(int stage) {
		imgB.gameObject.SetActive(true);
		imgB.material.shader = Shader.Find("Self-Illumin/Diffuse");
		imgB.material.color = new Color(newGrey, newGrey, newGrey);
		if (stage >= 2) DC.NB.Crack(0, false); if (stage >= 3) DC.NB.Crack(1, false);
		if (stage == 4) { DC.NB.Crack(2, false); DC.NB.Crack(3, false); }
	}

	IEnumerator Waiting(float sec, int ph) { phase = 0; yield return new WaitForSeconds(sec); phase = ph; }

	void Update() {
		//1. SURPRISE
		if (phase == 1 && tc <= 1) {
			tc += 0.01666666666F * Time.deltaTime * 60;
			c = imgA.material.color; c.a = tc; imgA.material.color = c;
		}
		else if (phase == 1 && tc > 1) {
			DC.M[7].ActivationFinished(); DC.NB.gameObject.SetActive(true);
			DC.S.SC4.activatedNotebook = true; phase = 0;
		}
		//2. ZOOM IN
		if (phase == 2 && tc <= 1) {
			DC.cam.position = Vector3.Lerp(DC.camPos, DC.camPosNB, Mathf.SmoothStep(0, 1, tc));
			DC.cam.rotation = Quaternion.Lerp(DC.camRot, DC.camRotNB, Mathf.SmoothStep(0, 1, tc));
			tc += 0.00666666666F * Time.deltaTime * 60;
		}
		else if (phase == 2 && tc > 1) StartCoroutine(Waiting(0.5F, 3));
		else if (phase == 3) {
			DC.bMenu.SetActive(true); DC.CursorLock(false);
			if (DC.S.SC4.sawReveal) { DC.bReturn.SetActive(true); phase = 7; }
			else {
				DC.S.SC4.revealStage = 0; DC.S.SC4.nextDID = 440; phase = 0;
				DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, 440, false);
			}
		}
		//3. IMAGE CHANGE
		else if (phase == 4 && tc <= 1) {
			tc += 0.01666666666F * Time.deltaTime * 60; //tc += 0.02F;
			c = imgA.material.color; c.a = 1-tc; imgA.material.color = c;
			c = imgB.material.color; c.a = tc; imgB.material.color = c;
		}
		else if (phase == 4 && tc > 1) {
			OBJ.Find("Img0").gameObject.SetActive(false); imgA.gameObject.SetActive(false);
			imgB.material.shader = Shader.Find("Self-Illumin/Diffuse");
			imgB.material.color = new Color(newGrey, newGrey, newGrey);
			StartCoroutine(Waiting(1.0F, 5));
		}
		else if (phase == 5) {
			DC.S.SC4.revealStage = 1;
			DC.bMenu.SetActive(true); DC.UIC.StartNarration(35); phase = 0;
		}
		//5. ZOOM OUT
		else if (phase == 6 && tc <= 1) {
			DC.cam.position = Vector3.Lerp(DC.camPosNB, DC.camPos, Mathf.SmoothStep(0, 1, tc));
			DC.cam.rotation = Quaternion.Lerp(DC.camRotNB, DC.camRot, Mathf.SmoothStep(0, 1, tc));
			tc += 0.00666666666F * Time.deltaTime * 60;
		}
		else if (phase == 6 && tc > 1) {
			DC.NB.gameObject.SetActive(false); DC.UIC.Col(true); DC.bMenu.SetActive(true);
			DC.CursorLock(false); phase = 0;
		}
	}
}
