using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoomChange : MonoBehaviour {

	DataControlChapter1 DC; int from, to;
	int phase = 0; Color c; bool doorsounds = false;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
	}

	public void DoIt(int dst) {
		c = DC.bS.color; c.a = 0; DC.bS.color = c; DC.bS.gameObject.SetActive(true);
		from = DC.S.SC1.currentRoom; to = dst;
		doorsounds = from == 1 || from == 2 || to == 1 || to == 2;
		DC.UIC.Col(false); DC.currentColliders = DC.room[to].Find("Colliders").gameObject; DC.UIC.Col(false);
		if (doorsounds) { DC.BGM.volume = 0.4F * COMMON.U.volM; DC.Sound.clip = DC.doorOpen; DC.Sound.Play(); }
		if (to == 0 && DC.S.SC1.tvIsOn) { DC.Sound2.volume = 0; DC.Sound2.Play(); }
		DC.CursorLock(true); DC.bMenu.SetActive(false); phase = 1;
	}

	void Update() {
		if (phase == 1 && DC.bS.color.a < 1) {
			var c = DC.bS.color; c.a += 0.02F * Time.deltaTime * 60; DC.bS.color = c;
			if (!doorsounds) DC.BGM.volume -= 0.012F * COMMON.U.volM * Time.deltaTime * 60;
			if (from == 0 && DC.S.SC1.tvIsOn && DC.Sound2.volume > 0) DC.Sound2.volume -= 0.02F * COMMON.U.volS * Time.deltaTime * 60;
		}
		else if (phase == 1 && DC.bS.color.a >= 1) { 
			DC.cam[from].SetActive(false); DC.cam[from].GetComponent<Camera>().enabled = false; DC.cam[from].GetComponent<AudioListener>().enabled = false;
			DC.cam[to].SetActive(true); DC.cam[to].GetComponent<Camera>().enabled = true; DC.cam[to].GetComponent<AudioListener>().enabled = true;
			DC.Sound.clip = DC.footsteps; DC.Sound.Play();
			if (from == 0 && DC.S.SC1.tvIsOn) DC.Sound2.Stop();
			phase = 2;
		}
		else if (phase == 2 && !DC.Sound.isPlaying) {
			if (doorsounds) { DC.Sound.clip = DC.doorClose; DC.Sound.Play(); }
			phase = 3;
		}
		else if (phase == 3 && DC.bS.color.a > 0) {
			var c = DC.bS.color; c.a -= 0.02F * Time.deltaTime * 60; DC.bS.color = c;
			if (!doorsounds) DC.BGM.volume += 0.012F * COMMON.U.volM * Time.deltaTime * 60;
			else if (c.a <= 0.6F) DC.BGM.volume += 0.02F * COMMON.U.volM * Time.deltaTime * 60;
			if (to == 0 && DC.S.SC1.tvIsOn && DC.Sound2.volume < 0.8F*COMMON.U.volS) DC.Sound2.volume += 0.02F * COMMON.U.volS * Time.deltaTime * 60;
		}
		else if (phase == 3 && DC.bS.color.a <= 0) {
			if (DC.S.SC1.womanGotUp && !DC.S.SC1.storyFinished && to == 1) Events.Trigger(DC.S, (int)events1.startPretense);
			if (!DC.S.SC1.visitedEntryway && to == 3) DC.S.SC1.visitedEntryway = true;
			DC.S.SC1.currentRoom = to; DC.UPP();
			DC.bS.gameObject.SetActive(false);
			DC.UIC.Col(true); DC.CursorLock(false); DC.bMenu.SetActive(true); phase = 0;
		}
	}
}
