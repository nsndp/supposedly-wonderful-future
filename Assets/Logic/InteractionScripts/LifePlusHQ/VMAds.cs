using UnityEngine;
using System.Collections;

public class VMAds : MonoBehaviour {

	public Material[] M;
	DataControlHub DC; MeshRenderer R; Color full;
	int ind = 0, phase = 0; float t, tc, fullTime = 20.0F;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		R = GetComponent<MeshRenderer>();
		phase = 1; t = 0;
	}

	void Update() {
		if (DC.paused || DC.S == null || DC.S.SH.currentRoom == 1) return;
		if (phase == 1) {
			t += Time.deltaTime;
			if (t >= fullTime) { full = R.materials[1].color; phase = 2; tc = 0; }
		}
		else if (phase == 2 && tc <= 1) {
			tc += 0.02F * Time.deltaTime * 60;
			R.materials[1].color = Color.Lerp(full, Color.black, tc);
		}
		else if (phase == 2 && tc > 1) {
			ind++; if (ind >= M.Length) ind = 0;
			var m = R.materials; m[1] = M[ind]; R.materials = m;
			full = R.materials[1].color; R.materials[1].color = Color.black;
			phase = 3; tc = 0;
		}
		else if (phase == 3 && tc <= 1) {
			tc += 0.02F * Time.deltaTime * 60;
			R.materials[1].color = Color.Lerp(Color.black, full, tc);
		}
		else if (phase == 3 && tc > 1) {
			phase = 1; t = 0;
		}
	}
}
