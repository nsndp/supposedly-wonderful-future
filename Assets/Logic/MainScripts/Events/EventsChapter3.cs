using UnityEngine;
using System.Collections;

public enum events3 {
	startupVR, startDialogue, elevatorIsImportant, finish,
	countQuestions, askedQ2, askedQ3, askedQ4, askedQ7, askedQ15, askedQ52, returnToQ
}

public static class EventsC3 {
	public static void UpdateDID(SaveChapter3 S, int did) {
		S.nextDID = did;
	}

	public static void CountQ(SaveChapter3 S) {
		S.questionsAsked = 0; for (int i = 125; i <= 134; i++) if (S.DStruct.Used[i]) S.questionsAsked++;
		if (S.questionsAsked >= 5) S.DStruct.Locked[405] = false;
		if (S.questionsAsked >= 10) { S.DStruct.Locked[404] = false; S.DStruct.Locked[402] = true; }
	}

	public static void Trigger(SaveChapter3 S, int eventID) {
		var DC = GameObject.Find("Data").GetComponent<DataControlChapter3>();
		switch ((events3)eventID) {
		case events3.startupVR: S.inStartup = true; DC.Startup.SetActive(true); break;
		case events3.startDialogue:
			DC.Video.SetActive(true); DC.MPlay(DC.theme, 1.164F);
			DC.UIC.StartDialogue(DC.Dialogue, S.DStruct, 0, S.nextDID, false);
			break;
		case events3.elevatorIsImportant: if (S.DStruct.Used[72] && S.DStruct.Used[82]) S.DStruct.Locked[84] = false; break;
		case events3.countQuestions: CountQ(S); break;
		case events3.askedQ2: S.talkedAboutCreation = true; CountQ(S); break;
		case events3.askedQ3: S.DStruct.R[128] = new int[] {221}; CountQ(S); break;
		case events3.askedQ4: S.DStruct.R[153] = new int[] {157}; CountQ(S); break;
		case events3.askedQ15: S.DStruct.R[225] = new int[] {218}; break;
		case events3.askedQ52: if (DC.S.SH.creatorIsPicky) S.DStruct.Locked[263] = false; break;
		case events3.askedQ7: S.talkedAboutBeliefs = true; CountQ(S); break;
		case events3.returnToQ:
			S.DStruct.Used[402] = false; S.DStruct.Used[135] = false;
			S.DStruct.R[135] = new int[] {401};
			break;
		case events3.finish:
			DC.S.levelID = 13;
			DC.S.Save(COMMON.saveFolder + "Autosave.bin");
			COMMON.saveToLoad = "Autosave.bin";
			DC.MC.LoadLevel(10, true);
			break;
		default: break;
		}
	}
}