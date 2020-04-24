using UnityEngine;
using System.Collections;

public enum events4 {
	start0, start1, start2, start3A, start3B, sawName,
	memory1, memory2, memory3, memory4, memory5, memory6, memory7, memory8,
	ghettoIndeed, mentioningAdoption, contradiction1, contradiction2,
	revealA, revealA2, revealB, revealC, revealC2, revealC3, revealD, revealE, revealF,
	finish1, trustHisSymptoms, finish2, thinkSomeMore, choiceA, choiceB, choiceC,
	cracksFade1, cracksFade2, finish3B, finish3B2, finish3C,
	cracksIncrease1, cracksIncrease2, shatterMe, finish3A, littleLonger, chapterEnd,
	finishIntro
}

public static class EventsC4 {
	public static void UpdateDID(SaveChapter4 S, int did) {
		S.nextDID = did;
	}

	public static void Trigger(SaveChapter4 S, int eventID) {
		var DC = GameObject.Find("Data").GetComponent<DataControlChapter4>();
		switch ((events4)eventID) {
		case events4.finishIntro: DC.UIC.blackScreenDisappear(); S.seenIntro = true; break;
		case events4.start0: S.memoryDID[0] = 40; S.memoryDID[1] = 40; S.memoryDID[2] = 40; break;
		case events4.start1: S.memoryDID[0] = 60; S.memoryDID[1] = 61; S.memoryDID[2] = 62; DC.M[S.curM].FinDlg(); break;
		case events4.start2:
			S.memoryDID[0] = 100;
			S.memoryDID[1] = DC.S.SC4.curM == 1 ? -1 : 83; S.memoryDID[2] = DC.S.SC4.curM == 2 ? -1 : 86;
			DC.M[S.curM].FinDlg();
			S.readyToRemember = true; DC.UPP(2);
			break;
		case events4.start3A: S.memoryDID[1] = -1; DC.M[S.curM].FinDlg(); break;
		case events4.start3B: S.memoryDID[2] = -1; DC.M[S.curM].FinDlg(); break;
		case events4.sawName: S.sawName = true; S.DStruct.R[48] = new int[] {50}; DC.NmPlt.SetActive(false); break;
		
		case events4.memory1:
			S.memoryDID[0] = -1; S.memorySeen[0] = true; DC.NmPlt.SetActive(false);
			S.memoryDID[1] = 130; S.memoryDID[2] = 160; DC.M[0].FinDlg();
			DC.UPP(3);
			break;
		case events4.memory2:
			S.memoryDID[1] = -1; S.memorySeen[1] = true; DC.M[1].FinDlg();
			DC.UPP(S.memorySeen[2] ? 6 : 5);
			break;
		case events4.memory3:
			S.memoryDID[2] = -1; S.memorySeen[2] = true; DC.M[2].FinDlg();
			DC.UPP(S.memorySeen[1] ? 6 : 4);
			break;
		case events4.memory4:
			S.DStruct.Locked[396] = true; S.DStruct.Locked[397] = true;
			S.memoryDID[3] = -1; S.memorySeen[3] = true; DC.M[3].FinDlg(); DC.UPP(7);
			break;
		case events4.memory5:
			S.DStruct.Locked[396] = true; S.DStruct.Locked[397] = true;
			S.memoryDID[4] = -1; S.memorySeen[4] = true; DC.M[4].FinDlg(); DC.UPP(7);
			break;
		case events4.memory6:
			S.DStruct.Locked[396] = false; S.DStruct.Locked[397] = true;
			S.memoryDID[5] = -1; S.memorySeen[5] = true; DC.M[5].FinDlg(); DC.UPP(8);
			break;
		case events4.memory7:
			S.DStruct.Locked[396] = true; S.DStruct.Locked[397] = false;
			if (S.noticedThing1 && S.noticedThing2) S.DStruct.Locked[421] = false;
			S.memoryDID[6] = -1; S.memorySeen[6] = true; DC.M[6].FinDlg(); DC.UPP(8);
			break;
		case events4.memory8: S.memoryDID[7] = -1; S.memorySeen[7] = true; DC.M[7].FinDlg(); DC.UPP(9); break;
		case events4.ghettoIndeed: S.DStruct.R[195] = new int[] {201}; break;
		case events4.mentioningAdoption: S.DStruct.Locked[312] = false; break;
		case events4.contradiction1: S.noticedThing1 = true; if (S.noticedThing2 && S.memorySeen[6]) S.DStruct.Locked[421] = false; break;
		case events4.contradiction2: S.noticedThing2 = true; if (S.noticedThing1 && S.memorySeen[6]) S.DStruct.Locked[421] = false; break;

		case events4.revealA:
			DC.UIC.Col(false); DC.bMenu.SetActive(false); DC.UIC.DelayedTrigger(0.5F, (int)events4.revealA2);
			break;
		case events4.revealA2:
			DC.bMenu.SetActive(true); DC.UIC.StartNarration(0);
			DC.MPlay(DC.mNotebook, 0);
			break;
		case events4.revealB: DC.NB.ChangeImages(); break;
		case events4.revealC:
			DC.UIC.Col(false); DC.bMenu.SetActive(false);
			DC.UIC.DelayedTrigger(0.5F, (int)events4.revealC2);
			DC.MStop();
			break;
		case events4.revealC2:
			DC.NB.Crack(0, false); S.revealStage = 2; DC.UIC.DelayedTrigger(0.5F, (int)events4.revealC3);
			break;
		case events4.revealC3:
			DC.bMenu.SetActive(true); DC.UIC.StartDialogue(DC.Dialogue, S.DStruct, 0, S.nextDID, false);
			DC.MPlay(DC.mTruth, 0);
			break;
		case events4.revealD: DC.NB.Crack(1, false); S.revealStage = 3; break;
		case events4.revealE: DC.NB.Crack(2, false); DC.NB.Crack(3, true); S.revealStage = 4; break;
		case events4.revealF:
			S.memoryDID = new int[] {470, 470, 470, 470, 470, 470, 470, 470};
			S.revealStage = -1; S.sawReveal = true; DC.NB.ZoomOut(); DC.UPP(10);
			break;
		
		case events4.finish1:
			S.sawFinishP1 = true; S.memoryDID = new int[] {505, 505, 505, 505, 505, 505, 505, 505};
			//preparing dialogue in finish 2
			if (S.memorySeen[4] && S.memorySeen[5]) { S.DStruct.R[510] = new int[] {514}; S.DStruct.R[517] = new int[] {521}; }
			else if (S.memorySeen[5]) { S.DStruct.R[510] = new int[] {512}; S.DStruct.R[517] = new int[] {519}; }
			else if (S.memorySeen[4]) { S.DStruct.R[510] = new int[] {513}; S.DStruct.R[517] = new int[] {520}; }
			DC.M[S.curM].FinDlg();
			break;
		case events4.finish2:
			S.sawFinishP2 = true; S.memoryDID = new int[] {565, 565, 565, 565, 565, 565, 565, 565};
			DC.M[S.curM].FinDlg();
			break;
		case events4.thinkSomeMore: S.DStruct.Used[568] = false; DC.M[S.curM].FinDlg(); break;
		case events4.trustHisSymptoms: S.DStruct.R[556] = new int[] {559}; break;
		case events4.choiceA: S.theChoice = 1; DC.MChange(DC.mColor, 0); break;
		case events4.choiceB: S.theChoice = 2; DC.MChange(DC.mStay, 0); break;
		case events4.choiceC: S.theChoice = 3; break;
		case events4.cracksFade1: S.addCracks = -1; DC.M[S.curM].FadeCrack(); break;
		case events4.cracksFade2: S.addCracks = -2; DC.M[S.curM].FadeCrack(); break;
		case events4.cracksIncrease1: S.addCracks = 1; DC.M[S.curM].MakeCracks(1, false); break;
		case events4.cracksIncrease2: S.addCracks = 2; DC.M[S.curM].MakeCracks(1, false); break;
		case events4.finish3B:
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			DC.UIC.DelayedTrigger(0.5F, (int)events4.finish3B2);
			break;
		case events4.finish3B2: DC.M[S.curM].FadeLastCracks(); break;
		case events4.finish3C:
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			DC.UIC.DelayedTrigger(0.5F, (int)events4.chapterEnd);
			break;
		case events4.shatterMe:
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			DC.M[S.curM].ShatterMe(); DC.UPP(-1);
			break;
		case events4.finish3A: DC.M[S.curM].FinDlg(); break;

		case events4.littleLonger:
			//DC.UIC.Col(false);
			break;
		case events4.chapterEnd:
			DC.S.levelID = 14; DC.S.Save(COMMON.saveFolder + "Autosave.bin");
			COMMON.saveToLoad = "Autosave.bin";
			DC.MC.LoadLevel(DC.S.levelID, true);
			break;
		default: break;
		}
	}
}