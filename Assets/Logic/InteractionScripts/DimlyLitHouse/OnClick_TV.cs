using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClick_TV : MonoBehaviour {

	public MeshRenderer TV;
	public GameObject TVLight;
	public Material TVScreen, TVWhiteNoise;
	DataControlChapter1 DC;
	int phase = 0, c = 0;
	bool fadeInNoise = false;

	public void TurnOn(bool fromLoad = false) {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		var m = TV.materials; m[1] = TVWhiteNoise; TV.materials = m;
		TVLight.SetActive(true); DC.S.SC1.tvIsOn = true;
		//a special case when we reload in living room with TV on; all other times fade in happens with RoomChange
		if (fromLoad && DC.S.SC1.currentRoom == 0) fadeInNoise = true;
	}
	public void TurnOff() {
		var m = TV.materials; m[1] = TVScreen; TV.materials = m;
		TVLight.SetActive(false); DC.S.SC1.tvIsOn = false; fadeInNoise = false;
		DC.Sound2.Stop();
	}

	void OnMouseDown() { phase = 1; }
	
	void Update() {
		if (DC.paused) return;
		//just like CommentOnClick but with TV being turned on at the third comment
		if (phase == 1) {
			var i = DC.S.SC1.CCID[10];
			DC.UIC.DisplayComment(DC.Comments.GetLine(i));
			if (i == 90) { DC.S.SC1.DSWoman.Locked[75] = false; DC.S.SC1.nothingToTalkWoman = false; }
			if (i == 92) TurnOff();
			DC.S.SC1.CCID[10]++;
			Cursor.visible = false; DC.UIC.Col(false); DC.bMenu.SetActive(false);
			phase = 2;
		}
		else if (phase == 2 && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.UIC.HideComment();
			Cursor.visible = true; DC.UIC.Col(true); DC.bMenu.SetActive(true);
			phase = 0;
			if (DC.S.SC1.CCID[10] == 93) gameObject.SetActive(false);
		}

		if (DC.S.SC1.tvIsOn) {
			c++;
			if (c % (Time.deltaTime < 0.025F ? 3 : 2) == 0) TV.materials[1].mainTextureOffset = new Vector2(Random.value, Random.value);
			if (c >= 32000) c = 0;
		}
		if (fadeInNoise && !DC.Sound2.isPlaying) { DC.Sound2.volume = 0; DC.Sound2.Play(); }
		else if (fadeInNoise && DC.Sound2.volume < 0.8F*COMMON.U.volS) DC.Sound2.volume += 0.02F * COMMON.U.volS * Time.deltaTime * 60;
		else if (fadeInNoise && DC.Sound2.volume >= 0.8F*COMMON.U.volS) fadeInNoise = false;

		//if (DC.MC.phase == 1 && DC.S.SC1.tvIsOn && DC.S.SC1.currentRoom == 0) DC.Sound2.volume -= 0.02F;
	}
}
