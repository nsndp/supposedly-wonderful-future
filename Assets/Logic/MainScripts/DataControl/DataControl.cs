using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public abstract class DataControl : MonoBehaviour {
	public SaveGame S;
	public TXT Comments, Narration;
	public NarrationStructure NS;
	public bool paused = false;
	public GameObject currentColliders;
	public UIControl UIC;
	public MenuControl MC;
	public GameObject bMenu, bReturn;
	public Image bS;
	public GameObject activeHL;

	public AudioSource BGM, Sound, Sound2;
	public float loopAt;
	public bool musicFadeOut, musicFadeIn;
	public AudioClip playNext;
	public int resumeNextAt;

	//used everywhere except in Start() to properly start playing a different BGM
	public void MPlay(AudioClip cl, float lp, int resumeAt = 0) {
		BGM.Stop(); BGM.clip = cl; BGM.timeSamples = resumeAt; BGM.Play();
		if (resumeAt == 0) BGM.volume = COMMON.U.volM; else { BGM.volume = 0; musicFadeIn = true; }
		loopAt = lp; MC.prevSample = 0;
		//if (resumeAt != 0) Debug.Log("Resume BGM at " + (1.0F * resumeAt / cl.frequency));
	}
	//used to gradually fade out current BGM
	public void MStop() {
		musicFadeIn = false; musicFadeOut = true; playNext = null;
	}
	//used to gradually fade out current BGM and then start playing a different one immediately
	public void MChange(AudioClip cl, float lp, int resumeAt = 0) {
		musicFadeIn = false; musicFadeOut = true;
		playNext = cl; loopAt = lp; resumeNextAt = resumeAt;
	}
	//resumeAt parts are used only in Chapter 2

	public abstract int[] GetCCID();
	public abstract void UISettingsChanged();
	public abstract void VideoResChanged();
	public abstract void PauseAnimations(bool pause);

	public void CursorLock(bool locked) {
		Cursor.visible = !locked;
		Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
		//if (COMMON.trailerRecordMode > 0) Cursor.visible = false;
	}

	protected string dataFolder;
	public string LOC(string name) {
		var path = dataFolder + name + ".xml";
		if (COMMON.U.languageID == 0) return path;
		string pathL = "";
		if (COMMON.U.languageID == 1) pathL = dataFolder + "RU/" + name + "_RU.xml";
		if (!File.Exists(pathL)) return path;
		return pathL;
	}
}
