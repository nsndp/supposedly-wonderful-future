using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Credits : MonoBehaviour {

	DataControlChapter5 DC;
	Text Title, Author1, Author2, mHeader, mLicense, m1L, m1R, m2L, m2R, m3L, m3R, m4L, m4R;
	Text vidHeader, vid1src, vid1L, vid1R, vid2src, vid21, vid22;
	Text imgHeader, img1src, img11, img12, img13, img14, img2src, img2, img3src, img31, img32, img4L, img4R;
	Text modHeader, mod1src, mod1, mod2src, mod2, texHeader, tex;
	Text sfxHeader, sfxSource, sfx1, sfx2, sfx3, sfx4, fntHeader, fntL, fntR, end;
	GameObject skip; Text skipYes, skipNo;
	Text[][] toShow, toFade; float[] timings;
	int phase = -1, k = 0, ph = -1; Color c; float t = -1;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter5>();
		Title = transform.Find("Title").GetComponent<Text>();
		Author1 = transform.Find("Author1").GetComponent<Text>();
		Author2 = transform.Find("Author2").GetComponent<Text>();
		mHeader = transform.Find("Music/mHeader").GetComponent<Text>();
		mLicense = transform.Find("Music/mLicense").GetComponent<Text>();
		m1L = transform.Find("Music/m1L").GetComponent<Text>(); m1R = transform.Find("Music/m1R").GetComponent<Text>();
		m2L = transform.Find("Music/m2L").GetComponent<Text>(); m2R = transform.Find("Music/m2R").GetComponent<Text>();
		m3L = transform.Find("Music/m3L").GetComponent<Text>(); m3R = transform.Find("Music/m3R").GetComponent<Text>();
		m4L = transform.Find("Music/m4L").GetComponent<Text>(); m4R = transform.Find("Music/m4R").GetComponent<Text>();
		vidHeader = transform.Find("Videos/vidHeader").GetComponent<Text>();
		vid1src = transform.Find("Videos/vid1src").GetComponent<Text>(); vid2src = transform.Find("Videos/vid2src").GetComponent<Text>();
		vid1L = transform.Find("Videos/vid1L").GetComponent<Text>(); vid1R = transform.Find("Videos/vid1R").GetComponent<Text>();
		vid21 = transform.Find("Videos/vid21").GetComponent<Text>(); vid22 = transform.Find("Videos/vid22").GetComponent<Text>();
		imgHeader = transform.Find("Images/imgHeader").GetComponent<Text>();
		img1src = transform.Find("Images/img1src").GetComponent<Text>();
		img2src = transform.Find("Images/img2src").GetComponent<Text>();
		img3src = transform.Find("Images/img3src").GetComponent<Text>();
		img11 = transform.Find("Images/img11").GetComponent<Text>(); img12 = transform.Find("Images/img12").GetComponent<Text>();
		img13 = transform.Find("Images/img13").GetComponent<Text>(); img14 = transform.Find("Images/img14").GetComponent<Text>();
		img31 = transform.Find("Images/img31").GetComponent<Text>(); img32 = transform.Find("Images/img32").GetComponent<Text>();
		img4L = transform.Find("Images/img4L").GetComponent<Text>(); img4R = transform.Find("Images/img4R").GetComponent<Text>();
		img2 = transform.Find("Images/img2").GetComponent<Text>();
		modHeader = transform.Find("Models/modHeader").GetComponent<Text>();
		mod1src = transform.Find("Models/mod1src").GetComponent<Text>(); mod1 = transform.Find("Models/mod1").GetComponent<Text>();
		mod2src = transform.Find("Models/mod2src").GetComponent<Text>(); mod2 = transform.Find("Models/mod2").GetComponent<Text>();
		texHeader = transform.Find("Textures/texHeader").GetComponent<Text>();
		tex = transform.Find("Textures/tex").GetComponent<Text>();
		sfxHeader = transform.Find("Sounds/sfxHeader").GetComponent<Text>();
		sfxSource = transform.Find("Sounds/sfxSource").GetComponent<Text>();
		sfx1 = transform.Find("Sounds/sfx1").GetComponent<Text>(); sfx2 = transform.Find("Sounds/sfx2").GetComponent<Text>();
		sfx3 = transform.Find("Sounds/sfx3").GetComponent<Text>(); sfx4 = transform.Find("Sounds/sfx4").GetComponent<Text>();
		fntHeader = transform.Find("Fonts/fntHeader").GetComponent<Text>();
		fntL = transform.Find("Fonts/fntL").GetComponent<Text>(); fntR = transform.Find("Fonts/fntR").GetComponent<Text>();
		end = transform.Find("End").GetComponent<Text>();
		skip = transform.Find("Skip").gameObject;
		skipYes = transform.Find("Skip/Panel/Yes").GetComponent<Text>();
		skipNo = transform.Find("Skip/Panel/No").GetComponent<Text>();

		toShow = new Text[][] {
			new Text[] {Title}, new Text[] {Author1, Author2}, new Text[] {mHeader, mLicense, m1L, m1R},
			new Text[] {m2L, m2R}, new Text[] {m3L, m3R}, new Text[] {m4L, m4R},
			new Text[] {vidHeader, vid1src, vid1L, vid1R}, new Text[] {vid2src, vid21, vid22},
			new Text[] {imgHeader, img1src, img11, img12}, new Text[] {img13, img14},
			new Text[] {img2src, img2}, new Text[] {img3src, img31, img32}, new Text[] {img4L, img4R},
			new Text[] {modHeader, mod1src, mod1}, new Text[] {mod2src, mod2},
			new Text[] {texHeader, tex}, new Text[] {sfxHeader, sfxSource, sfx1, sfx2},
			new Text[] {sfx3, sfx4}, new Text[] {fntHeader, fntL, fntR}, new Text[] {end}, null
		};
		toFade = new Text[][] {
			null, new Text[] {Title}, new Text[] {Author1, Author2}, new Text[] {m1L, m1R},
			new Text[] {m2L, m2R}, new Text[] {m3L, m3R}, new Text[] {mHeader, mLicense, m4L, m4R},
			new Text[] {vid1src, vid1L, vid1R}, new Text[] {vidHeader, vid2src, vid21, vid22},
			new Text[] {img11, img12}, new Text[] {img1src, img13, img14}, new Text[] {img2src, img2},
			new Text[] {img3src, img31, img32}, new Text[] {imgHeader, img4L, img4R},
			new Text[] {mod1src, mod1}, new Text[] {modHeader, mod2src, mod2}, new Text[] {texHeader, tex},
			new Text[] {sfx1, sfx2}, new Text[] {sfxHeader, sfxSource, sfx3, sfx4},
			new Text[] {fntHeader, fntL, fntR}, new Text[] {end}
		};
		timings = new float[toFade.Length];
		timings[0] = 0; timings[1] = 10.724F - 0.833F; timings[2] = 21.372F - 0.833F;
		timings[timings.Length-2] = 212.055F - 0.833F; timings[timings.Length-1] = 220;
		float step = (212.055F - 21.372F) / (timings.Length - 4);
		for (int i = 3; i < timings.Length - 2; i++) timings[i] = 21.372F + step * (i-2);
		//for (int i = 0; i < timings.Length; i++) Debug.Log(timings[i]);
	}

	public void OnSkipNoEnter(BaseEventData d) { skipNo.color = new Color(0.188F, 0.647F, 1); }
	public void OnSkipNoExit(BaseEventData d) { skipNo.color = Color.white; }
	public void OnSkipYesEnter(BaseEventData d) { skipYes.color = new Color(0.188F, 0.647F, 1); }
	public void OnSkipYesExit(BaseEventData d) { skipYes.color = Color.white; }
	public void OnSkipNoClick(BaseEventData d) {
		DC.CursorLock(true); skipNo.color = Color.white;
		DC.BGM.UnPause(); skip.SetActive(false); phase = ph;
	}
	public void OnSkipYesClick(BaseEventData d) {
		DC.MC.STEAM.Achievement("ACH_C5");
		DC.CursorLock(true);
		skip.transform.Find("Panel").gameObject.SetActive(false);
		COMMON.mainMenuSkipLogos = true; DC.MC.LoadLevel(-1);
	}

	public void Go() {
		DC.BGM.loop = false; t = 0; phase = 1;
		//k = 18; t = timings[k]; DC.BGM.time = t; //to test the end
	}

	void Update() {
		if (t > 0 && Input.GetKeyDown(KeyCode.Escape)) {
			if (skip.activeSelf) OnSkipNoClick(null);
			else { DC.CursorLock(false); DC.BGM.Pause(); skip.SetActive(true); ph = phase; phase = -1; }
		}
		if (Input.GetKeyDown(KeyCode.Return) && skip.activeSelf) OnSkipYesClick(null);

		if (phase >= 0) t += Time.deltaTime;
		if (phase == 0 && t >= timings[k]) phase = 1;

		else if (phase == 1 && toFade[k] == null) phase = 2;
		else if (phase == 1 && toFade[k][0].color.a > 0) {
			for (int i = 0; i < toFade[k].Length; i++) { c = toFade[k][i].color; c.a -= 0.02F * Time.deltaTime * 60; toFade[k][i].color = c; }
		}
		else if (phase == 1 && toFade[k][0].color.a <= 0) {
			for (int i = 0; i < toFade[k].Length; i++) toFade[k][i].gameObject.SetActive(false); phase = 2;
		}
		else if (phase == 2) {
			if (toShow[k] != null) for (int i = 0; i < toShow[k].Length; i++) { c = toShow[k][i].color; c.a = 0; toShow[k][i].color = c; toShow[k][i].gameObject.SetActive(true); }
			if (toShow[k] != null) phase = 3; else phase = 5;
		}
		else if (phase == 3 && toShow[k][0].color.a < 1) {
			for (int i = 0; i < toShow[k].Length; i++) { c = toShow[k][i].color; c.a += 0.02F * Time.deltaTime * 60; toShow[k][i].color = c; }
		}
		else if (phase == 3 && toShow[k][0].color.a >= 1) { k++; phase = 0; }
		else if (phase == 5) {
			DC.MC.STEAM.Achievement("ACH_C5");
			COMMON.mainMenuSkipLogos = true; DC.MC.LoadLevel(-1); phase = -1;
		}
	}
}