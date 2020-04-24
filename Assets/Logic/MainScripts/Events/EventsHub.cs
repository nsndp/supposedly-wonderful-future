using UnityEngine;
using System.Collections;

public enum eventsH {
	blackScreenOff, startDialogue1, JackieTurns, firstNightStarted, firstTransitionToLobby, JackieMaybeTurnsB,
	wantToChangeScenery, likeISaid, introDefinesOutroA, introDefinesOutroB,
	creatorIsPicky, countermeasures, noMindlessDestruction, oneLiner1, oneLiner2,
	visitedJackieAtNight, startedQuestions, chatFinisherIntro, chatFinisher, //chats - general
	annoyingSkirts, dontBeModest, explainedSimulations, letsStare, finishedInteraction, //chats - misc
	asked1A, asked1B, asked2A, asked2B, asked3A, asked3B, asked1E, asked2E, //chats - questions
	askedP1, askedP2, askedP3, askedP4, askedP5,
	askedC1, askedC2, askedC3, askedC4, askedC5, askedC6,
	chillTime, chillTimeB, chillTimeC, chillTimeD,
	oyasumi, oyasumiA, oyasumiB, oyasumiC, oyasumiD, oyasumiE,
	goToCbIU, goToMetaphors, goToMemories, goToFinale, loadLevel
}

public static class EventsHub {
	public static void UpdateDID(SaveHub S, int npc, int did) {
		if (npc == 0) S.nextDIDJChats = did;
		else if (npc == 1) S.nextDIDJP1 = did;
		else if (npc == 2) S.nextDIDJP2 = did;
		else if (npc == 3) S.nextDIDJP3 = did;
		else if (npc == 4) S.nextDIDJP4 = did;
		else if (npc == 5) S.nextDIDJP5 = did;
		else if (npc == 10) S.nextDIDBed = did;
	}

	static void LoadLevel(DataControlHub DC, int id) {
		DC.S.levelID = id;
		DC.S.Save(COMMON.saveFolder + "Autosave.bin");
		COMMON.saveToLoad = "Autosave.bin";
		DC.MC.LoadLevel(DC.S.levelID, true);
	}

	public static void Trigger(SaveHub S, int eventID) {
		var DC = GameObject.Find("Data").GetComponent<DataControlHub>();
		switch ((eventsH)eventID) {
		case eventsH.blackScreenOff: DC.UIC.blackScreenDisappear(); S.blackScreen = false; break;

		case eventsH.startDialogue1:
			DC.UIC.StartDialogue(DC.DialogueP1, S.DSJP1, 1, S.nextDIDJP1, true);
			DC.JA["Stand1"].speed = 0.75F; DC.JAScr.phase = 1; S.JackieTurnedA = 1;
			break;
		case eventsH.JackieTurns: DC.JA["Stand1"].speed = 1; S.JackieTurnedA = 2; break;
		case eventsH.JackieMaybeTurnsB:
			if (DC.S.SH.DSJChats.R[43][0] != 45) { DC.JA["Stand2"].speed = 1; S.JackieTurnedB = 2; }
			break;
		case eventsH.firstTransitionToLobby:
			DC.currentColliders.transform.Find("Corridor").GetComponent<OnClick_Corridor>().DoTransition();
			S.visitedLobby = true;
			break;
		case eventsH.wantToChangeScenery: S.nextDIDJChats = 15; break;
		case eventsH.likeISaid: S.likeISaid = true; break;
		case eventsH.introDefinesOutroA: for (int i = 700; i <= 702; i++) S.DSJP3.R[i] = new int[] {703}; break;
		case eventsH.introDefinesOutroB: for (int i = 700; i <= 702; i++) S.DSJP3.R[i] = new int[] {707}; break;
		case eventsH.creatorIsPicky: S.creatorIsPicky = true; break;
		case eventsH.countermeasures:
			if (S.DSJP3.Used[310] && S.DSJP3.Used[311] && S.DSJP3.Used[312] && S.DSJP3.Used[313] && S.DSJP3.Used[314]) {
				S.DSJP3.Locked[316] = false; S.DSJP3.Locked[315] = true;
			}
			break;
		case eventsH.noMindlessDestruction: S.DSJP3.R[360] = new int[] {367}; break;
		case eventsH.oneLiner1: S.oneLiner1 = true; break;
		case eventsH.oneLiner2: S.oneLiner2 = true; break;

		case eventsH.visitedJackieAtNight: S.visitedJackie = true; break;
		case eventsH.startedQuestions: S.startedQuestions = true; if (DC.S.levelID == 14) { DC.JA["Stand2"].speed = 1; S.JackieTurnedB = 2; } break;
		case eventsH.chatFinisherIntro: S.DSJChats.Used[13] = false; break;
		case eventsH.chatFinisher: S.DSJChats.Used[50] = false; S.DSJChats.Used[51] = false; break;
		case eventsH.letsStare: DC.MC.STEAM.Achievement("ACH_CHATS"); S.letsStare = true; break;
		case eventsH.finishedInteraction: S.finishedInteraction = true; break;
		case eventsH.annoyingSkirts: S.DSJChats.Locked[497] = false; break;
		case eventsH.dontBeModest: S.DSJChats.R[224] = new int[] {227}; break;
		case eventsH.explainedSimulations: S.DSJChats.R[306] = new int[] {308}; break;
		case eventsH.asked1A: S.asked[0] = true; break;
		case eventsH.asked1B: S.asked[1] = true; S.DSJChats.Locked[212] = false; break;
		case eventsH.asked2A: S.asked[2] = true; break;
		case eventsH.asked2B: S.asked[3] = true; break;
		case eventsH.asked1E: S.asked[6] = true; break;
		case eventsH.asked2E: S.asked[7] = true; break;
		case eventsH.askedP1: S.askedPersonal[0] = true; break;
		case eventsH.askedP2: S.askedPersonal[1] = true; break;
		case eventsH.askedP3: S.askedPersonal[2] = true; break;
		case eventsH.askedP4: S.askedPersonal[3] = true; break;
		case eventsH.askedP5: S.askedPersonal[4] = true; break;
		case eventsH.askedC1: S.askedCorporate[0] = true; break;
		case eventsH.askedC2: S.askedCorporate[1] = true; break;
		case eventsH.askedC3: S.askedCorporate[2] = true; break;
		case eventsH.askedC4: S.askedCorporate[3] = true; break;
		case eventsH.askedC5: S.askedCorporate[4] = true; break;
		case eventsH.askedC6: S.askedCorporate[5] = true; break;
		case eventsH.asked3A:
			bool askedAllP = true; for (int i = 0; i < 5; i++) if (!S.askedPersonal[i]) askedAllP = false;
			if (!askedAllP) { S.DSJChats.Used[365] = false; S.DSJChats.Used[351] = false; }
			else { S.asked[4] = true; S.DSJChats.Locked[351] = true; } //locking is only for when asking all in one go, when "tell me more" is not used yet
			break;
		case eventsH.asked3B:
			bool askedAllC = true; for (int i = 0; i < 6; i++) if (!S.askedCorporate[i]) askedAllC = false;
			if (!askedAllC) { S.DSJChats.Used[531] = false; S.DSJChats.Used[521] = false; }
			else { S.asked[5] = true; S.DSJChats.Locked[521] = true; }
			break;

		case eventsH.chillTime:
			DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.UIC.blackScreenAppear(0.01F, (int)eventsH.chillTimeB);
			DC.MStop();
			break;
		case eventsH.chillTimeB:
			S.currentRoom = 1; S.isNight = true; DC.SetDayOrNight(true);
			DC.camL.gameObject.SetActive(false); DC.camR.gameObject.SetActive(true);
			DC.currentColliders = GameObject.Find("Room/Colliders"); DC.UIC.Col(false);
			DC.GetComponent<HighlightHints>().NPS[0] = DC.currentColliders.transform.Find("Bed");
			DC.UIC.DelayedTrigger(2, (int)eventsH.chillTimeC);
			break;
		case eventsH.chillTimeC:
			if (DC.S.levelID != 11) {
				DC.UIC.blackScreenDisappear(0.01F, (int)eventsH.chillTimeD);
				if (DC.S.levelID == 12) DC.MPlay(DC.night2, 19.854F);
				else if (DC.S.levelID == 13) DC.MPlay(DC.night3, 2.813F);
				else DC.MPlay(DC.night4, 0);
			}
			else {
				DC.CursorLock(false); DC.bMenu.SetActive(true);
				DC.UIC.StartNarration(20); S.firstNightIntro = true; S.blackScreen = true;
			} 
			break;
		case eventsH.chillTimeD:
			DC.CursorLock(false); DC.bMenu.SetActive(true); DC.UIC.Col(true);
			break;
		case eventsH.firstNightStarted:
			S.firstNightIntro = false; DC.MPlay(DC.night1, 8.161F);
			break;

		case eventsH.oyasumi: Events.Trigger(DC.S, (int)eventsH.oyasumiA); break;
		case eventsH.oyasumiA:
			if (DC.S.levelID == 11) DC.MC.STEAM.Achievement("ACH_C1");
			else if (DC.S.levelID == 12) DC.MC.STEAM.Achievement("ACH_C2");
			else DC.MC.STEAM.Achievement("ACH_C4");
			DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.UIC.blackScreenAppear(0.01F, (int)eventsH.oyasumiB);
			DC.MStop();
			break;
		case eventsH.oyasumiB:
			DC.UIC.DelayedTrigger(2, (int)eventsH.oyasumiC);
			break;
		case eventsH.oyasumiC:
			if (COMMON.demoVersion) { DC.demoEnd.Show(); DC.bS.gameObject.SetActive(false); }
			else if (DC.S.levelID == 11) { S.startedCH2 = true; DC.UIC.showTitle(2, (int)eventsH.oyasumiD); }
			else if (DC.S.levelID == 12) { S.startedCH3 = true; DC.UIC.showTitle(3, (int)eventsH.oyasumiD); }
			else { S.startedCH5 = true; DC.UIC.showTitle(5, (int)eventsH.oyasumiD); }
			break;
		case eventsH.oyasumiD:
			DC.CursorLock(true); DC.bMenu.SetActive(false);
			S.currentRoom = 0; S.isNight = false; DC.SetDayOrNight(false);
			DC.currentColliders = GameObject.Find("Lounge/Colliders");
			DC.camL.gameObject.SetActive(true); DC.camR.gameObject.SetActive(false);
			DC.UIC.blackScreenDisappear(0.01F, (int)eventsH.oyasumiE);
			if (DC.S.levelID < 14) DC.MPlay(DC.intro23, 0); else DC.MPlay(DC.intro5, 0);
			break;
		case eventsH.oyasumiE:
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			if (DC.S.levelID == 11) DC.UIC.StartDialogue(DC.DialogueP2, S.DSJP2, 2, S.nextDIDJP2, true);
			else if (DC.S.levelID == 12) DC.UIC.StartDialogue(DC.DialogueP3, S.DSJP3, 3, S.nextDIDJP3, true);
			else DC.UIC.StartDialogue(DC.DialogueP5, S.DSJP5, 5, S.nextDIDJP5, true);
			break;

		case eventsH.goToCbIU: DC.S.SHcp11end = new SaveHub(DC.S.SH); LoadLevel(DC, 2); break;
		case eventsH.goToMetaphors: DC.S.SHcp12end = new SaveHub(DC.S.SH); LoadLevel(DC, 3); break;
		case eventsH.goToFinale: LoadLevel(DC, 5); break;
		case eventsH.goToMemories:
			DC.MC.STEAM.Achievement("ACH_C3");
			S.isNight = false; S.currentRoom = 0; //prepare for the return
			DC.S.SHcp13end = new SaveHub(DC.S.SH); LoadLevel(DC, 4);
			break;
	}
	}
}