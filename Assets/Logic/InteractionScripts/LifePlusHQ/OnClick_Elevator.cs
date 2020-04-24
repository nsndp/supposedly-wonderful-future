using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_Elevator : MonoBehaviour {

	public GameObject E; public Material ArrowActive, Arrows;
	DataControlHub DC; int phase = 0; float tc = 0;
	//float tt = 0; //for trailer

	void Start() { DC = GameObject.Find("Data").GetComponent<DataControlHub>(); }
	IEnumerator Waiting(float sec, int ph) { phase = -1; yield return new WaitForSeconds(sec); phase = ph; }

	void OnMouseDown() {
		DC.UIC.Col(false); DC.CursorLock(true); DC.bMenu.SetActive(false);
		//var m = E.GetComponent<Renderer>().materials; m[3] = ArrowActive; E.GetComponent<Renderer>().materials = m;
		var c = DC.bS.color; c.a = 0; DC.bS.color = c; DC.bS.gameObject.SetActive(true);
		tc = 0; phase = 1;
	}

	void Update() {
		if (phase == 1 && tc <= 1) {
			DC.camL.localPosition = Vector3.Lerp(DC.pos, DC.posElv, Mathf.SmoothStep(0, 1, tc));
			DC.camL.localRotation = Quaternion.Lerp(DC.rot, DC.rotElv, Mathf.SmoothStep(0, 1, tc));
			tc += 0.008333333F * Time.deltaTime * 60;
			if (DC.BGM.volume > 0.4F * COMMON.U.volM) DC.BGM.volume -= 0.01F * COMMON.U.volM * Time.deltaTime * 60;
			if (tc >= 0.55F && !DC.Sound.isPlaying) { DC.Sound.clip = DC.elevatorDoors; DC.Sound.Play(); }
			if (tc >= 0.6F && !E.GetComponent<Animation>().isPlaying) {
				var m = E.GetComponent<Renderer>().materials; m[3] = ArrowActive; E.GetComponent<Renderer>().materials = m;
				E.GetComponent<Animation>().Play();
			}
			//tt += Time.deltaTime; //for trailer
		}
		else if (phase == 1 && tc > 1) phase = 2;

		//for trailer -->
		/*else if (phase == 2 && tt < 1.8F) tt += Time.deltaTime;
		else if (phase == 2 && tt >= 1.8F) {
			var pp = DC.camL.GetComponent<PostProcessing>();
			pp.Multiply = Mathf.Lerp(1, 2, (tt - 1.8F) / 2); pp.Multiply = 1 + Mathf.Pow(pp.Multiply - 1, 2);
			pp.Add = Mathf.Lerp(0, 1, (tt - 1.8F) / 2); pp.Add = Mathf.Pow(pp.Add, 2);
			tt += Time.deltaTime; if (tt >= 4) phase = 22;
		}*/
		//<-- for trailer
		else if (phase == 2 && !E.GetComponent<Animation>().isPlaying && !DC.Sound.isPlaying) {
			DC.Sound.clip = DC.transition2; DC.Sound.Play(); phase = 3;
		}

		else if (phase == 3 && DC.bS.color.a < 1) {
			var c = DC.bS.color; c.a += 0.02F * Time.deltaTime * 60; DC.bS.color = c;
		}
		else if (phase == 3 && DC.bS.color.a >= 1 && !DC.Sound.isPlaying) {
			DC.camL.gameObject.SetActive(false);
			DC.camR.gameObject.SetActive(true);
			DC.currentColliders = GameObject.Find("Room/Colliders"); DC.UIC.Col(false);
			DC.GetComponent<HighlightHints>().NPS[0] = DC.currentColliders.transform.Find("Bed");
			phase = 4;
		}
		else if (phase == 4 && DC.bS.color.a > 0) {
			var c = DC.bS.color; c.a -= 0.02F * Time.deltaTime * 60; DC.bS.color = c;
			DC.BGM.volume += 0.012F * COMMON.U.volM * Time.deltaTime * 60;
		}
		else if (phase == 4 && DC.bS.color.a <= 0) {
			DC.bS.gameObject.SetActive(false);
			var m = E.GetComponent<Renderer>().materials; m[3] = Arrows; E.GetComponent<Renderer>().materials = m;
			E.transform.Find("DoorL").localPosition = Vector3.zero;
			E.transform.Find("DoorR").localPosition = Vector3.zero;
			DC.UIC.Col(true); DC.bMenu.SetActive(true);
			DC.S.SH.currentRoom = 1;
			DC.CursorLock(false); phase = 0;
		}
	}
}