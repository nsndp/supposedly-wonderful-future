using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_Corridor : MonoBehaviour {

	DataControlHub DC; int phase = 0;

	void Start () {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
	}

	void OnMouseDown() {
		if (!DC.S.SH.visitedLobby) DC.UIC.StartDialogue(DC.DialogueOther, DC.S.SH.DSOther, 11, 0, false);
		else DoTransition();
	}

	public void DoTransition() {
		var c = DC.bS.color; c.a = 0; DC.bS.color = c; DC.bS.gameObject.SetActive(true);
		DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
		DC.BGM.volume = 0.4F * COMMON.U.volM; DC.Sound.clip = DC.transition1; DC.Sound.Play();
		phase = 1;
	}

	void Update() {
		if (phase == 1 && DC.bS.color.a < 1) {
			var c = DC.bS.color; c.a += 0.02F * Time.deltaTime * 60; DC.bS.color = c;
		}
		else if (phase == 1 && DC.bS.color.a >= 1 && !DC.Sound.isPlaying) {
			DC.Sound.clip = DC.elevatorDoors; DC.Sound.Play(); phase = 2;
		}
		else if (phase == 2 && !DC.Sound.isPlaying) {
			DC.camR.gameObject.SetActive(false);
			DC.camL.gameObject.SetActive(true);
			DC.camL.localPosition = DC.pos; DC.camL.localRotation = DC.rot;
			DC.currentColliders = GameObject.Find("Lounge/Colliders"); DC.UIC.Col(false);
			DC.GetComponent<HighlightHints>().NPS[0] = DC.currentColliders.transform.Find("Elevator");
			phase = 3;
		}
		else if (phase == 3 && DC.bS.color.a > 0) {
			var c = DC.bS.color; c.a -= 0.02F * Time.deltaTime * 60; DC.bS.color = c;
			DC.BGM.volume += 0.012F * COMMON.U.volM * Time.deltaTime * 60;
		}
		else if (phase == 3 && DC.bS.color.a <= 0) {
			DC.bS.gameObject.SetActive(false);
			DC.UIC.Col(true); DC.bMenu.SetActive(true);
			DC.S.SH.currentRoom = 0; DC.CursorLock(false); phase = 0;
		}
	}
}
