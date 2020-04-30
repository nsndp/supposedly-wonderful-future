using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

[System.Serializable]
public class SaveGame {
	public SavePrologue SP;
	public SaveChapter1 SC1;
	public SaveChapter2 SC2;
	public SaveChapter3 SC3;
	public SaveChapter4 SC4;
	public SaveChapter5 SC5;
	public SaveHub SH;
	public SaveHub SHcp11end, SHcp12end, SHcp13end;

	public string Name;
	public int levelID;
	public int inDialogue = -1;
	public bool inNarration = false;
	public int NID = 0;

	public SaveGame() {}

	public void Save(string path) {
		var binFormatter = new BinaryFormatter();
		using (var stream = new FileStream(path, FileMode.Create)) {
			binFormatter.Serialize(stream, this);
		}
	}
	public static SaveGame Load(string path) {
		if (!File.Exists(path)) { Debug.Log("File \""+ path + "\" not found."); return null; }
		var binFormatter = new BinaryFormatter();
		using (var stream = new FileStream(path, FileMode.Open)) {
			return binFormatter.Deserialize(stream) as SaveGame;
		}
	}
}

public static class COMMON { //common data
	public static string saveToLoad = null;
	public static UserSettings U = null;
	public static bool mainMenuSkipLogos = false;
	public static string saveFolder = Application.persistentDataPath + "/";
	public static string dataFolder = Path.Combine(Application.dataPath, "DATA/");
	public static bool demoVersion = false;
	public static int trailerRecordMode = -1;

	public static bool LoadUserSettings() {
		string pth = saveFolder + "UserSettings.bin";
		if (!File.Exists(pth)) {
			U = new UserSettings(); U.Save(pth);
			Debug.Log("NO USER SETTINGS FOUND: default file is generated."); //this should only happen in MainMenu
			return true;
		}
		U = UserSettings.Load(pth);
		if (U == null) {
			U = new UserSettings(); U.Save(pth);
			Debug.Log("PROBLEM LOADING USER SETTINGS: default file is generated.");
			return true;
		}
		return false;
	}
}

[System.Serializable]
public class UserSettings {
	public float volM = 1;
	public float volS = 1;
	public bool videosLowRes = false;

	public int fontSize = 4; //0-7
	public int textLayout = 0; //0 - classic, 1 - cinematic, 2 - sidelong
	public int textAnim = 2; //0 - off, 1 - responses only, 2 - on
	public int textEffects = 0; //0 - off, 1 - shadow, 2 - outline, 3 - shadow & outline
	public float overlayAlpha = 0.6F;

	public int languageID = 0;

	public void Save(string path) {
		var binFormatter = new BinaryFormatter();
		using (var stream = new FileStream(path, FileMode.Create)) {
			binFormatter.Serialize(stream, this);
		}
	}
	public static UserSettings Load(string path) {
		try {
			var binFormatter = new BinaryFormatter();
			using (var stream = new FileStream(path, FileMode.Open)) {
				return binFormatter.Deserialize(stream) as UserSettings;
			}
		} catch (System.Exception e) {
			Debug.Log(e.Message);
			return null;
		}
	}
}

// save names xml file for speeding up Save/Load menu work
[XmlRoot("Text")]
public class SaveNames {
	[XmlArray("List"), XmlArrayItem("Save")] public List<Namae> list;
	public SaveNames() { list = new List<Namae>(); }

	public static SaveNames Load(string path) {
		if (!File.Exists(path)) {
			Debug.Log("NO SAVE NAMES FILE FOUND: will make a new one.");
			return null;
		}
		try {
			var serializer = new XmlSerializer(typeof(SaveNames));
			using(var stream = new FileStream(path, FileMode.Open)) {
				return serializer.Deserialize(stream) as SaveNames;
			}
		} catch (System.Exception e) {
			Debug.Log(e.Message);
			Debug.Log("PROBLEM LOADING SAVE NAMES FILE: will make a new one.");
			return null;
		}
	}
	public void Save(string path) {
		var serializer = new XmlSerializer(typeof(SaveNames));
		using(var stream = new FileStream(path, FileMode.Create)) { serializer.Serialize(stream, this); }
	}

	public string GetName(int id) {
		int i = 0;
		while (i < list.Count && list[i].id != id) i++;
		if (i == list.Count) return null;
		return list[i].nm;
	}
	public Namae Get(int id) {
		int i = 0;
		while (i < list.Count && list[i].id != id) i++;
		if (i == list.Count) return null;
		return list[i];
	}
}

public class Namae {
	[XmlAttribute("Number")] public int id;
	[XmlAttribute("Name")] public string nm;
	public Namae() { }
	public Namae(int id, string nm) {
		this.id = id;
		this.nm = nm;
	}
}