using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using Facepunch.Steamworks;
using Steamworks;

public class SteamControl : MonoBehaviour {

	public void Achievement(string apiName) {
		if (!SteamManager.Initialized) return;
		try {
			SteamUserStats.SetAchievement(apiName);
			SteamUserStats.StoreStats();
		}
		catch (Exception e) { Debug.Log(e.Message); }
	}

	int a = 0;

	void Update() {
		//achievement reset tests - one with less keys + notification, one with more keys + silent
		/*if (Input.GetKeyDown(KeyCode.R) && Input.GetKeyDown(KeyCode.T)) {
			var qs = GameObject.Find("Interface").transform.Find("UI/Quicksave").GetComponent<Image>();
			var tt = qs.transform.Find("Text").GetComponent<Text>();
			qs.gameObject.SetActive(true); var c = qs.color; c.a = 1; qs.color = c;
			tt.text = "ACH RESET"; c = tt.color; c.a = 1; tt.color = c;
			ResetAchievements();
		}*/
		/*if (Input.GetKeyDown(KeyCode.LeftShift)) a = 0;
		if (Input.GetKey(KeyCode.LeftShift)) {
			if (Input.GetKeyDown(KeyCode.R) && a == 0) a = 1;
			else if (Input.GetKeyDown(KeyCode.E) && a == 1) a = 2;
			else if (Input.GetKeyDown(KeyCode.S) && a == 2) a = 3;
			else if (Input.GetKeyDown(KeyCode.E) && a == 3) a = 4;
			else if (Input.GetKeyDown(KeyCode.T) && a == 4) { ResetAchievements(); a = 0; }
		}*/
	}

	private void ResetAchievements() {
		if (!SteamManager.Initialized) return;
		try {
			var names = new string[] { "ACH_C0", "ACH_C1", "ACH_C2", "ACH_C3", "ACH_C4", "ACH_C5", "ACH_CHATS", "ACH_NEWS" };
			foreach (var n in names) {
				bool unlocked;
				SteamUserStats.GetAchievement(n, out unlocked);
				if (unlocked) SteamUserStats.ClearAchievement(n);
			}
			SteamUserStats.StoreStats();
			Debug.Log("RESET ACHIEVEMENTS");
		}
		catch (Exception e) { Debug.Log(e.Message); }
	}

	#region FACEPUNCH - not working on Linux

	/*int a = 0;

	void Start() {
		DontDestroyOnLoad(gameObject);
		Facepunch.Steamworks.Config.ForUnity(Application.platform.ToString());
		new Facepunch.Steamworks.Client(719210);
	}

	void OnDestroy() {
		if (Client.Instance != null) Client.Instance.Dispose();
	}

	public void Achievement(string apiName) {
		if (Client.Instance != null) Client.Instance.Achievements.Trigger(apiName, true);
	}

	public void ResetAchievements() {
		Client.Instance.Achievements.Reset("ACH_C0");
		Client.Instance.Achievements.Reset("ACH_C1");
		Client.Instance.Achievements.Reset("ACH_C2");
		Client.Instance.Achievements.Reset("ACH_C3");
		Client.Instance.Achievements.Reset("ACH_C4");
		Client.Instance.Achievements.Reset("ACH_C5");
		Client.Instance.Achievements.Reset("ACH_CHATS");
		Client.Instance.Achievements.Reset("ACH_NEWS");
	}

	void Update() {

		if (Client.Instance != null)
			Client.Instance.Update();

		if (Input.GetKeyDown(KeyCode.LeftShift)) a = 0;
		if (Input.GetKey(KeyCode.LeftShift)) {
			if (Input.GetKeyDown(KeyCode.R) && a == 0) a = 1;
			else if (Input.GetKeyDown(KeyCode.E) && a == 1) a = 2;
			else if (Input.GetKeyDown(KeyCode.S) && a == 2) a = 3;
			else if (Input.GetKeyDown(KeyCode.E) && a == 3) a = 4;
			else if (Input.GetKeyDown(KeyCode.T) && a == 4 && Client.Instance != null) {
				ResetAchievements(); a = 0;
				Debug.Log("RESET ACHIEVEMENTS");
			}
		}
	}*/

	#endregion
}
