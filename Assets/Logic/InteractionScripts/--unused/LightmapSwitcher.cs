using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightmapSwitcher : MonoBehaviour {

	public Texture2D[] DayDir, DayLight, NightDir, NightLight;
	LightmapData[] dayLightMaps, nightLightMaps;
	//bool isDay = true;

	void Start() {
		/*dayLightMaps = new LightmapData[DayDir.Length];
		for (int i = 0; i < DayDir.Length; i++)
		{
			dayLightMaps[i] = new LightmapData();
			dayLightMaps[i].lightmapDir = DayDir[i];
			dayLightMaps[i].lightmapLight = DayLight[i];
		}

		nightLightMaps = new LightmapData[NightDir.Length];
		for (int i = 0; i < NightDir.Length; i++)
		{
			nightLightMaps[i] = new LightmapData();
			nightLightMaps[i].lightmapDir = NightDir[i];
			nightLightMaps[i].lightmapLight = NightLight[i];
		}*/
	}

	void Switch() {
		var JRT = GameObject.Find("JackiePrologueLM-B").transform;
		var JLM = GameObject.Find("JackiePrologue").transform;
		Renderer R1, R2; string[] objs = new string[] { "Jackie_Body", "Jackie_Eyebrows", "Jackie_eyelashes", "Jackie_Hair", "Jackie_HighPolyEyes", "Jackie_Shoes" };
		for (int i = 0; i < objs.Length; i++) {
			R1 = JRT.Find(objs[i]).GetComponent<Renderer>();
			R2 = JLM.Find(objs[i]).GetComponent<Renderer>();
			R2.lightmapIndex = R1.lightmapIndex; R2.lightmapScaleOffset = R1.lightmapScaleOffset;
		}

		//if (isDay) { LightmapSettings.lightmaps = nightLightMaps; isDay = false; }
		//else { LightmapSettings.lightmaps = dayLightMaps; isDay = true; }
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.Y)) Switch();
	}
}
