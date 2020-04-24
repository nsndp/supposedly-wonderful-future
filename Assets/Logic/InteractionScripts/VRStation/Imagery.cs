using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Imagery : MonoBehaviour {

	DataControlChapter3 DC; bool moviePaused = false;
	public MovieTexture[] BlockA, BlockB, BlockC, BlockD, BlockE;
	public MovieTexture[] BlockA_LowRes, BlockB_LowRes, BlockC_LowRes, BlockD_LowRes, BlockE_LowRes;
	MovieTexture[][] M, Mlr; RawImage I; RectTransform R;
	int i = 0, j = -1, b = 0, r, rm; bool fadeIn, fadeOut, fadeOutPrepare, varHue;
	float tc, foTimer, foSec; float h, s, v;
	//aspect ratio: 0 - default (16:9), 1 - more than 1.77F, 2 - less than 1.77F
	//fade: 0 - no need, 1 - only out, 2 - both in and out
	float[][] d = new float[][] { new float[] {4.37F, 10.03F, 23.23F, 14.03F, 14.03F, 5.97F}, new float[] {30.3F, 30.3F, 7.9F, 16.43F}, new float[] {79.3F, 20.57F}, new float[] {26.07F, 15.93F, 23.83F}, new float[] {4f, 4f, 15.93F} };
	int[][] ar = new int[][] { new int[] {0, 1, 1, 1, 1, 2}, new int[] {0,0,0,2}, new int[] {0,0}, new int[] {0,0,0}, new int[] {0,0,0} };
	int[][] fade = new int[][] { new int[] {1, 2, 2, 2, 2, 1}, new int[] {2,2,0,0}, new int[] {2,2}, new int[] {2,1,2}, new int[] {0,0,2} };
	int[][] repsMin = new int[][] { new int[] {5, 5, 3, 4, 4, 5}, new int[] {2,2,5,3}, new int[] {1,3}, new int[] {3,3,3}, new int[] {5,5,3} };
	int[][] repsMax = new int[][] { new int[] {10, 10, 6, 8, 8, 10}, new int[] {4,4,10,6}, new int[] {1,6}, new int[] {6,6,6}, new int[] {10,10,6} };
	int[] repsBlock = new int[] {7, 3, 2, 2, 3};
	public bool changeRes = false;
	bool testMode = false;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter3>();
		M = new MovieTexture[][] { BlockA, BlockB, BlockC, BlockD, BlockE };
		Mlr = new MovieTexture[][] { BlockA_LowRes, BlockB_LowRes, BlockC_LowRes, BlockD_LowRes, BlockE_LowRes };
		I = GetComponent<RawImage>(); R = GetComponent<RectTransform>();
		ChangeMovie();
	}

	void ChangeMovie(bool restartLast = false) {
		if (!restartLast) {
			if (!testMode) {
				b++; if (b > repsBlock[i]) { b = 0; i = Random.Range(0, M.Length * 2); if (i >= M.Length) i = 0; }
				j = Random.Range(0, M[i].Length);
				r = 1; rm = Random.Range(repsMin[i][j], repsMax[i][j] + 1);
			} else {
				//in test mode we iterate through them one-by-one
				j++; if (j == M[i].Length) { j = 0; i++; if (i == M.Length) i = 0; } r = 1; rm = 1;
			}
		}
		if (i > 0) { h = 0; s = 0; varHue = false; }
		else { //Block A - playing with color
			int rr = Random.Range(0, 4);
			if (rr == 0) { h = 0; s = 0; varHue = false; }
			else if (rr == 3) { h = 0; s = 1; varHue = true; }
			else { h = Random.value; s = 1; varHue = false; }
		}
		//Debug.Log(i + " " + j + " - " + b + "/" + repsBlock[i] + " " + r + "/" + rm);
		R.anchoredPosition = Vector2.zero;
		if (ar[i][j] == 0) R.sizeDelta = new Vector2(Mathf.RoundToInt(Screen.height * 1.7777777777F), Screen.height);
		else if (ar[i][j] == 1) R.sizeDelta = new Vector2(Screen.width, Mathf.RoundToInt(1.0F * Screen.width / M[i][j].width * M[i][j].height));
		else {
			R.sizeDelta = new Vector2(Mathf.RoundToInt(1.0F * Screen.height / M[i][j].height * M[i][j].width), Screen.height);
			R.anchoredPosition = new Vector2(Mathf.RoundToInt(Screen.width / 2.0F - R.sizeDelta.x / 2.0F - 100), 0);
		}
		I.texture = !COMMON.U.videosLowRes ? M[i][j] : Mlr[i][j];
		((MovieTexture)I.texture).loop = false; ((MovieTexture)I.texture).Play();
		if (fade[i][j] <= 1) v = 1; else v = 0; I.material.color = Color.HSVToRGB(h, s, v);
		if (fade[i][j] >= 1) { fadeOutPrepare = true; foTimer = 0; foSec = d[i][j] - 1; }
		if (fade[i][j] == 2) { fadeIn = true; tc = 0; }
	}

	void Update() {
		if (DC.paused && moviePaused) return;
		if (DC.paused) { ((MovieTexture)I.texture).Pause(); moviePaused = true; return; }
		if (!DC.paused && moviePaused) {
			if (!changeRes) ((MovieTexture)I.texture).Play(); else ChangeMovie(true);
			moviePaused = false;
		}

		if (!((MovieTexture)I.texture).isPlaying) {
			((MovieTexture)I.texture).Stop();
			r++; if (r > rm) ChangeMovie();
			else {
				((MovieTexture)I.texture).Play();
				if (fade[i][j] <= 1) v = 1; else v = 0; I.material.color = Color.HSVToRGB(h, s, v);
				if (fade[i][j] >= 1) { fadeOutPrepare = true; foTimer = 0; foSec = d[i][j] - 1; }
				if (fade[i][j] == 2) { fadeIn = true; tc = 0; }
			}
		}
		if (fadeOutPrepare) {
			foTimer += Time.deltaTime;
			if (foTimer >= foSec) { fadeOut = true; tc = 0; fadeOutPrepare = false; }
		}
		if (varHue) {
			h += 0.001F * Time.deltaTime * 60; if (h > 1) h = 0;
			I.material.color = Color.HSVToRGB(h, s, v);
		}
		if (fadeIn && tc <= 1) {
			tc += 0.025F * Time.deltaTime * 60; v = Mathf.Lerp(0, 1, tc);
			I.material.color = Color.HSVToRGB(h, s, v);
		}
		else if (fadeIn && tc >= 1) fadeIn = false;
		else if (fadeOut && tc <= 1) {
			tc += 0.025F * Time.deltaTime * 60; v = Mathf.Lerp(1, 0, tc);
			I.material.color = Color.HSVToRGB(h, s, v);
		}
		else if (fadeOut && tc >= 1) fadeOut = false;
	}
}