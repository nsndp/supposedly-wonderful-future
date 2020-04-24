using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facepunch.Steamworks;

public class SteamControl : MonoBehaviour {

	int a = 0;

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
	}
}
