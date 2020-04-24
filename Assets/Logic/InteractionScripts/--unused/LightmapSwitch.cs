using UnityEngine;
using System.Collections;

public class LightmapSwitch : MonoBehaviour {

	public Texture2D[] Far, Near;
	public GameObject[] Lights;
	//LightmapData[] LM;
	//bool lmOn = true;

	void Start() {
		/*LM = new LightmapData[Far.Length];
		for (int i = 0; i < Far.Length; i++) {
			LM[i] = new LightmapData();
			LM[i].lightmapFar = Far[i];
			LM[i].lightmapNear = Near[i];
		}*/
	}

	void Update () {
		/*if (Input.GetKeyDown(KeyCode.L)) {
			if (lmOn) {
				LightmapSettings.lightmaps = null;
				lmOn = false;
			} else {
				LightmapSettings.lightmaps = LM;
				lmOn = true;
			}

		}*/
	}
}
