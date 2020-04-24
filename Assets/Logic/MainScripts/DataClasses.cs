using UnityEngine;
using System.Collections;

using System.Xml;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

/* ----------------------- TEXT, XML (same for everything, just ID and Text) ------------------------- */

public class Line {
	[XmlAttribute("ID")] public int ID;
	[XmlAttribute("Text")] public string Text;
	public Line() {	}
	public Line(int ID, string Text) {
		this.ID = ID;
		this.Text = Text;
	}
}

[XmlRoot("Text")]
public class TXT {
	[XmlArray("List"), XmlArrayItem("Line")] public Line[] Lines;
		
	public static TXT Load(string path) {
		var serializer = new XmlSerializer(typeof(TXT));
		using(var stream = new FileStream(path, FileMode.Open)) {
			return serializer.Deserialize(stream) as TXT;
		}
	}
	//public void Save(string path) { //we don't need save, but let's keep it just in case
	//	var serializer = new XmlSerializer(typeof(TXT));
	//	using(var stream = new FileStream(path, FileMode.Create)) { serializer.Serialize(stream, this); }
	//}

	//make a binary search here, because we have IDs in ascending order, and the order is guaranteed
	//or is it guaranteed?.. I can't google out for sure
	public string GetLine(int ID) { //O(n)
		int i = 0;
		while (i < Lines.Length && Lines[i].ID != ID) i++;
		if (i == Lines.Length) return null;
		return Lines[i].Text;
	}
}

/* ------------------------- NEWS ----------------------------- */

public class NewsPiece {
	[XmlAttribute("Type")] public int Type;
	[XmlAttribute("Title")] public string Title;
	[XmlAttribute("Subtitle")] public string Subtitle;
	[XmlText] public string Text;
}

public class NewsIssue {
	public string Date;
	[XmlArray("News"), XmlArrayItem("Piece")] public NewsPiece[] P;
}

[XmlRoot("Root")]
public class NEWS {
	[XmlArray("List"), XmlArrayItem("Issue")] public NewsIssue[] I;

	public static NEWS Load(string path) {
		var serializer = new XmlSerializer(typeof(NEWS));
		using(var stream = new FileStream(path, FileMode.Open)) {
			return serializer.Deserialize(stream) as NEWS;
		}
	}
}

/* --------------------------------------- UI TRANSLATIONS ------------------------------------------- */

public class Phrase {
	[XmlAttribute("Type")] public int Type;
	[XmlAttribute("Path")] public string Path;
	[XmlAttribute("Text")] public string Text;
	[XmlAttribute("Scale")] public float? Scale;
	[XmlAttribute("Shift")] public float? Shift;
}

public class PhraseGroup {
	[XmlAttribute("Path")] public string Root;
	[XmlAttribute("Width")] public int? Width;
	[XmlElement("Phrase")] public Phrase[] Phrases;
}

[XmlRoot("Text")]
public class Translation {
	[XmlElement("Root")] public PhraseGroup[] PhraseGroups;

	public static Translation Load(string path) {
		if (!File.Exists(path)) return null;
		try {
			var serializer = new XmlSerializer(typeof(Translation));
			using(var stream = new FileStream(path, FileMode.Open)) {
				return serializer.Deserialize(stream) as Translation;
			}
		} catch (System.Exception e) {
			Debug.Log(e.Message);
			return null;
		}
	}
}

/* ------------------------- STRUCTURES, BIN ----------------------------- */
/* ------------------------- 1. Narration -------------------------------- */

[System.Serializable]
public class NarrationStructure {
	public int[] Next;
	public int?[] TriggersEvent;
	public bool[] Finisher;
	public int[] Additive;

	public void Save(string path) {
		var binFormatter = new BinaryFormatter();
		using (var stream = new FileStream(path, FileMode.Create)) {
			binFormatter.Serialize(stream, this);
		}
	}
	public static NarrationStructure Load(string path) {
		if (!File.Exists(path)) { Debug.Log("File \""+ path + "\" not found."); return null; }
		var binFormatter = new BinaryFormatter();
		using (var stream = new FileStream(path, FileMode.Open)) {
			return binFormatter.Deserialize(stream) as NarrationStructure;
		}
	}
}

/* ------------------------- 2. Dialogue -------------------------------- */

[System.Serializable]
public class DialogueStructure {
	public int[][] R; //R for ResponsesID
	public bool[] Locked;
	public bool[] Used;
	public bool[] Finisher;
	public int[][] UnlockingID;
	public int[][] LockingID;
	public int?[] TriggersEvent;

	public DialogueStructure() { }
	public DialogueStructure(DialogueStructure SRC) {
		int sz = SRC.R.Length;
		R = new int[sz][]; LockingID = new int[sz][]; UnlockingID = new int[sz][];
		Locked = new bool[sz]; Used = new bool[sz]; Finisher = new bool[sz]; TriggersEvent = new int?[sz];
		for (int i = 0; i < sz; i++) Locked[i] = SRC.Locked[i];
		for (int i = 0; i < sz; i++) Used[i] = SRC.Used[i];
		for (int i = 0; i < sz; i++) Finisher[i] = SRC.Finisher[i];
		for (int i = 0; i < sz; i++) TriggersEvent[i] = SRC.TriggersEvent[i];
		for (int i = 0; i < sz; i++) if (SRC.R[i] == null) R[i] = null;
		else { int z = SRC.R[i].Length; R[i] = new int[z]; for (int j = 0; j < z; j++) R[i][j] = SRC.R[i][j]; }
		for (int i = 0; i < sz; i++) if (SRC.LockingID[i] == null) LockingID[i] = null;
		else { int z = SRC.LockingID[i].Length; LockingID[i] = new int[z]; for (int j = 0; j < z; j++) LockingID[i][j] = SRC.LockingID[i][j]; }
		for (int i = 0; i < sz; i++) if (SRC.UnlockingID[i] == null) UnlockingID[i] = null;
		else { int z = SRC.UnlockingID[i].Length; UnlockingID[i] = new int[z]; for (int j = 0; j < z; j++) UnlockingID[i][j] = SRC.UnlockingID[i][j]; }
	}

	public void Save(string path) {
		var binFormatter = new BinaryFormatter();
		using (var stream = new FileStream(path, FileMode.Create)) {
			binFormatter.Serialize(stream, this);
		}
	}
	public static DialogueStructure Load(string path) {
		if (!File.Exists(path)) { Debug.Log("Dialogue file " + path + " not found."); return null; }
		var binFormatter = new BinaryFormatter();
		using (var stream = new FileStream(path, FileMode.Open)) {
			return binFormatter.Deserialize(stream) as DialogueStructure;
		}
	}
}