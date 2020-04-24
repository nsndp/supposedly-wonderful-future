using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum events2 {
	roomAppear, finishIntro, storyFinished,
	discussCloset, discussTree, discussGuitar,
	//logs
	profileKarl, profileVivienne, profileDaniel, profileAnne, profileCloe, profileAshley, profileBoyd,
	blogA, blogB, blogC, blogComments, zoomOut, pageDown, pageUp, zoomOutSuspect,
	comm1, comm2, comm3, comm4, comm5, comm6, comm7, comm8, comm9, comm10, comm11, comm12, comm13, comm14,
	comm15, comm16, comm17, comm18, comm19, comm20, commExpand, commHide, commNext, commPrev,
	meetup1, meetup2, meetup3, meetup4, meetup5, meetup6,
	returnFromProfiles, returnFromLogs, returnFromRecordings, returnFromSuspects, exitScreens,
	//suspects
	callA, callB, callC, callD, callE, callF, callAgainA, callAgainB, callAgainC, callAgainD, callAgainE,
	askQuestionSuspectA, holdThatThoughtSuspectA, accusedSuspectA, SuspectA78, SuspectA2ndTime, SuspectA3rdTime,
	askQuestionSuspectB, holdThatThoughtSuspectB, accusedSuspectB, SuspectB35, SuspectB121, SuspectB2ndTime, SuspectB3rdTime,
	askQuestionSuspectC, holdThatThoughtSuspectC, accusedSuspectC,
	askQuestionSuspectD, holdThatThoughtSuspectD, accusedSuspectD, SuspectD27,
	seenCloeAtMeetUp, infiniteCloe,
	//apartment people
	pardner, KettleyInitial, KettleyBanterQuestion, KettleyBanterFinish, KettleyPrisons, KettleyQ56, KettleyQ57, KettleyReturns,
	KettleyWaitWithSearch, KettleySearches, WaitForKet1, WaitForKet2, searchA, searchB, searchC, searchD, searchD2,
	KettleyWaitWithArrest, KettleyArrestGo, arrestA, arrestB, arrestC, arrestD, KettleyDoesIt, didArrestHim,
	briefed, enoughBriefing, enoughBriefing2, wtfIsMeetup1, wtfIsMeetup2, banterShawn, loverBoy, tightSchedule, //Shawn
	notSoSimple, ShawnSearchA, ShawnSearchB, ShawnSearchB2, ShawnSearchC, ShawnSearchD, theyDontExist, queueTheDramaticReveal, goArrestHim, stickAround, //Shawn, Phase 2
	metKettley, metShawn, metAshley, notReallySuicidal, learnedAshleyView, AshleyFriendQuestion, enoughAboutFriends, //Ashley
	toldTheGoodNews //Ashley, Phase 2
}

public static class EventsC2 {
	static int[] realComm = {1, 3, 4, 7, 10, 12, 14, 15, 16, 17, 18, 20};
	static bool ArrayContains(int[] a, int v) {
		int i = 0; while (i < a.Length && a[i] != v) i++;
		if (i == a.Length) return false;
		return true;
	}

	public static void UpdateDID(SaveChapter2 S, int npc, int did) {
		if (npc == 0) S.nextDID_Kettley = did;
		else if (npc == 1) S.nextDID_Shawn = did;
		else if (npc == 2) S.nextDID_Ashley = did;
		else if (npc == 3) S.nextDID_Screens = did;
		else if (npc == 4) S.nextDID_Other = did;
		else if (npc == 11) S.nextDID_A = did;
		else if (npc == 12) S.nextDID_B = did;
		else if (npc == 13) S.nextDID_C = did;
		else if (npc == 14) S.nextDID_D = did;
		else if (npc == 15) S.nextDID_E = did;
	}
	
	public static void SetNothingToTalkAbout(SaveChapter2 S, int npc) {
		if (npc == 0) S.nothingToTalk_Kettley = true;
		else if (npc == 1) S.nothingToTalk_Shawn = true;
		else if (npc == 2) S.nothingToTalk_Ashley = true;
	}

	static void PH2(SaveChapter2 S) {
		if (S.learnedSuspect[0] && S.learnedSuspect[1] && S.learnedSuspect[2] && S.learnedSuspect[3] &&
		    S.sawMeetup[0] && S.sawMeetup[1] && S.sawMeetup[2] &&
		    S.sawMeetup[3] && S.sawMeetup[4] && S.sawMeetup[5]) {
			S.learnedAllOnScreens = true;
			var DD = GameObject.Find("Data").GetComponent<DataControlChapter2>();
			S.npp = 2; DD.UPP();
		}
		if (S.learnedAllOnScreens && S.learnedAshleyView) {
			S.nothingToTalk_Shawn = false;
			S.DSShawn.Locked[250] = false; for (int i = 95; i <= 104; i++) S.DSShawn.Locked[i] = true;
			var DD = GameObject.Find("Data").GetComponent<DataControlChapter2>();
			S.npp = 1; DD.UPP();
		}
	}

	static void Pagination(SaveChapter2 S, string txt, int k) {
		var DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		var log = GameObject.Find("MainScreen").transform.Find("Log").GetComponent<Text>();
		var logPages = GameObject.Find("MainScreen").transform.Find("Page").GetComponent<Text>();
		var G = new GUIStyle(); G.wordWrap = true; G.richText = true;
		G.font = log.font; G.fontSize = log.fontSize; G.fontStyle = log.fontStyle;
		var H = G.CalcHeight(new GUIContent(txt), log.GetComponent<RectTransform>().rect.width);
		var pageCount = Mathf.CeilToInt(H / log.GetComponent<RectTransform>().rect.height);
		if (k == -1) k = 0; else if (k == pageCount) k = pageCount - 1; S.mScrInd3 = k;
		logPages.text = DC.pageText[0] + " " + (k+1) + " " + DC.pageText[1] + " " + pageCount;
		if (pageCount == 1) {
			S.DSScreens.Locked[25] = true; S.DSScreens.Locked[26] = true;
			log.text = txt;
		} else {
			S.DSScreens.Locked[25] = false; S.DSScreens.Locked[26] = false;
			log.gameObject.SetActive(true);
			for (int z = 0; z <= k; z++) {
				log.GetComponent<Text>().text = txt;
				if (z < pageCount - 1) {
					Canvas.ForceUpdateCanvases();
					var m = log.cachedTextGenerator.characterCountVisible;
					if (txt[m] != '\n' && txt[m] != ' ') while (txt[m] != '\n' && txt[m] != ' ') m--;
					log.text = txt.Substring(0, m);
					txt = txt.Substring(m+1); //txt[m], which is either ' ' or '\n' now, is omitted
				}
			}
			log.gameObject.SetActive(false);
		}
	}
	public static void Screen(int i, int j, int k = 0) {
		var DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		DC.S.SC2.mScrInd1 = i; DC.S.SC2.mScrInd2 = j; DC.S.SC2.mScrInd3 = k;
		if (i == -1) { //zoom out and clear screen
			for (int z = 7; z <= 13; z++) DC.S.SC2.DSScreens.Finisher[z] = true;
			for (int z = 16; z <= 19; z++) DC.S.SC2.DSScreens.Finisher[z] = true;
			DC.S.SC2.DSScreens.Finisher[14] = false; DC.S.SC2.DSScreens.Finisher[20] = false;
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomOut();
			SetScreenText(DC);
		}
		else if (DC.S.SC2.screenZoom == -1) { //update screen but only after zooming in (see OnClick_Screens)
			if (i == 0) for (int z = 7; z <= 13; z++) DC.S.SC2.DSScreens.Finisher[z] = false;
			else if (i == 1) for (int z = 16; z <= 19; z++) DC.S.SC2.DSScreens.Finisher[z] = false;
			if (i == 0) DC.S.SC2.DSScreens.Finisher[14] = true;
			else if (i == 1) DC.S.SC2.DSScreens.Finisher[20] = true;
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(0);
		}
		else SetScreenText(DC); //update screen immediately
	}
	public static void SetScreenText(DataControlChapter2 DC) {
		var i = DC.S.SC2.mScrInd1; var j = DC.S.SC2.mScrInd2; var k = DC.S.SC2.mScrInd3;
		var Scr = GameObject.Find("MainScreen").transform;
		var o1 = Scr.Find("Profile").gameObject; o1.SetActive(false);
		var o2 = Scr.Find("Log").gameObject; o2.SetActive(false);
		var o2b = Scr.Find("Page").gameObject; o2b.SetActive(false);
		var o3 = Scr.Find("CommenterProfile").gameObject; o3.SetActive(false);
		var o4 = Scr.Find("CommenterHistory").gameObject; o4.SetActive(false);
		if (i == 0) {
			o1.GetComponent<Text>().text = DC.DataScreens.Profiles[j];
			o1.SetActive(true);
		}
		else if (i == 1) {
			Pagination(DC.S.SC2, j < 3 ? DC.DataScreens.Blogs[j] : DC.DataScreens.Discussion, k);
			int[] r = null; if (j == 0) r = new int[] {21}; else if (j == 1) r = new int[] {22};
			else if (j == 2) r = new int[] {24}; else if (j == 3) r = new int[] {23};
			DC.S.SC2.DSScreens.R[25] = r; DC.S.SC2.DSScreens.R[26] = r;
			o2.SetActive(true); o2b.SetActive(true);
		}
		else if (i == 2) {
			o3.GetComponent<Text>().text = DC.DataScreens.Commenters[j].Profile;
			o3.SetActive(true);
			if (k == 1) {
				o4.SetActive(true);
				if (!ArrayContains(realComm, j+1)) o4.GetComponent<Text>().text = DC.DataScreens.ErrorMessage;
				else o4.GetComponent<Text>().text = DC.DataScreens.Commenters[j].History;
			}
			if (j == 0) DC.S.SC2.DSScreens.Locked[67] = true; else DC.S.SC2.DSScreens.Locked[67] = false;
			if (j == 19) DC.S.SC2.DSScreens.Locked[68] = true; else DC.S.SC2.DSScreens.Locked[68] = false;
		}
		else if (i == 3) {
			Pagination(DC.S.SC2, DC.DataScreens.Meetups[j], k);
			DC.S.SC2.DSScreens.R[25] = new int[] {98 + j};
			DC.S.SC2.DSScreens.R[26] = new int[] {98 + j};
			o2.SetActive(true); o2b.SetActive(true);
		}
	}

	public static void Trigger(SaveChapter2 S, int eventID) {
		var DC = GameObject.Find("Data").GetComponent<DataControlChapter2>();
		switch ((events2)eventID) {
		case events2.roomAppear:
			DC.UIC.blackScreenDisappear(); S.roomAppeared = true;
			break;
		case events2.finishIntro:
			S.seenIntro = true; DC.MPlay(DC.intro, 30.708F);
			break;
		case events2.storyFinished:
			DC.noZoomOut = true;
			DC.S.levelID = 12;
			DC.S.Save(COMMON.saveFolder + "Autosave.bin");
			COMMON.saveToLoad = "Autosave.bin";
			DC.MC.LoadLevel(DC.S.levelID, true);
			break;

		//-----------------------------------------SCREENS-----------------------------------------
		case events2.profileKarl: Screen(0, 0); break;
		case events2.profileVivienne: Screen(0, 1); break;
		case events2.profileDaniel:	Screen(0, 2); break;
		case events2.profileAnne: Screen(0, 3); break;
		case events2.profileCloe: Screen(0, 4); break;
		case events2.profileAshley: Screen(0, 5); break;
		case events2.profileBoyd: Screen(0, 6); break;
		case events2.blogA: Screen(1, 0); break;
		case events2.blogB: Screen(1, 1); break;
		case events2.blogC: Screen(1, 2); break;
		case events2.blogComments: Screen(1, 3); break;
		case events2.pageDown: Screen(S.mScrInd1, S.mScrInd2, S.mScrInd3 + 1); break;
		case events2.pageUp: Screen(S.mScrInd1, S.mScrInd2, S.mScrInd3 - 1); break;
		case events2.comm1: Screen(2, 0); break;
		case events2.comm2: Screen(2, 1); break;
		case events2.comm3: Screen(2, 2); break;
		case events2.comm4: Screen(2, 3); break;
		case events2.comm5: Screen(2, 4); break;
		case events2.comm6: Screen(2, 5); break;
		case events2.comm7: Screen(2, 6); break;
		case events2.comm8: Screen(2, 7); break;
		case events2.comm9: Screen(2, 8); break;
		case events2.comm10: Screen(2, 9); break;
		case events2.comm11: Screen(2, 10); break;
		case events2.comm12: Screen(2, 11); break;
		case events2.comm13: Screen(2, 12); break;
		case events2.comm14: Screen(2, 13); break;
		case events2.comm15: Screen(2, 14); break;
		case events2.comm16: Screen(2, 15); break;
		case events2.comm17: Screen(2, 16); break;
		case events2.comm18: Screen(2, 17); break;
		case events2.comm19: Screen(2, 18); break;
		case events2.comm20: Screen(2, 19); break;
		case events2.commExpand:
			Screen(2, S.mScrInd2, 1);
			if (!S.checkedActivity[S.mScrInd2]) {
				S.checkedActivity[S.mScrInd2] = true;
				if (!ArrayContains(realComm, S.mScrInd2+1)) S.checkedFakes++; else S.checkedReals++;
				if (S.checkedFakes == 2) {
					S.spottedStrangeThing = true; S.DSShawn.Locked[271] = false; S.nothingToTalk_Shawn = false;
					S.npp = 1; DC.UPP();
				}
				else if (S.checkedFakes == 8 && S.checkedReals == 12) { S.DSShawn.R[273] = new int[] {276}; S.nothingToTalk_Shawn = false; }
				else if (S.checkedFakes >= 4 && S.checkedReals >=5 && S.DSShawn.R[273][0] == 274) { S.DSShawn.R[273] = new int[] {275}; S.nothingToTalk_Shawn = false; }
			}
			break;
		case events2.commHide:
			GameObject.Find("MainScreen").transform.Find("CommenterHistory").gameObject.SetActive(false);
			S.mScrInd3 = 0;
			break;
		case events2.commNext: Screen(2, S.mScrInd2 + 1); break;
		case events2.commPrev: Screen(2, S.mScrInd2 - 1); break;
		case events2.zoomOut: if (S.mScrInd1 > -1) Screen(-1, -1); break;
		case events2.meetup1: Screen(3, 0); if (!S.sawMeetup[0]) { S.sawMeetup[0] = true; PH2(S); } break;
		case events2.meetup2: Screen(3, 1); if (!S.sawMeetup[1]) { S.sawMeetup[1] = true; PH2(S); } break;
		case events2.meetup3: Screen(3, 2); if (!S.sawMeetup[2]) { S.sawMeetup[2] = true; PH2(S); } break;
		case events2.meetup4: Screen(3, 3); if (!S.sawMeetup[3]) { S.sawMeetup[3] = true; PH2(S); } break;
		case events2.meetup5: Screen(3, 4); if (!S.sawMeetup[4]) { S.sawMeetup[4] = true; PH2(S); } break;
		case events2.meetup6: Screen(3, 5); if (!S.sawMeetup[5]) { S.sawMeetup[5] = true; PH2(S); } break;
		case events2.returnFromProfiles: S.DSScreens.R[1] = new int[] {27}; if (S.mScrInd1 > -1) Screen(-1, -1); break;
		case events2.returnFromLogs: S.DSScreens.R[2] = new int[] {28}; if (S.mScrInd1 > -1) Screen(-1, -1); break;
		case events2.returnFromRecordings: S.DSScreens.R[3] = new int[] {74}; break;
		case events2.returnFromSuspects: if (S.DSScreens.R[4][0] == 110) S.DSScreens.R[4] = new int[] {109}; break;
		case events2.exitScreens:
			if (!S.phaseTwo) {
				DC.logsT = DC.BGM.timeSamples;
				if (S.briefed) DC.MChange(DC.briefed, 24.631F, DC.briefedT);
				else DC.MChange(DC.intro, 30.708F, DC.introT);
			}
			break;

		//-----------------------------------------SUSPECTS-----------------------------------------
		case events2.callA:
			S.DSKettley.Locked[172] = false;
			if (S.warrantsUsed == 0) S.DSKettley.Locked[37] = false; else if (S.warrantsUsed == 1) S.DSKettley.Locked[38] = false;
			S.DSScreens.TriggersEvent[111] = (int)events2.callAgainA;
			S.DSScreens.Finisher[111] = true; S.DSScreens.R[111] = new int[] {0};
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(1);
			break;
		case events2.callB:
			S.DSKettley.Locked[173] = false;
			if (S.warrantsUsed == 0) S.DSKettley.Locked[37] = false; else if (S.warrantsUsed == 1) S.DSKettley.Locked[38] = false;
			S.DSScreens.TriggersEvent[112] = (int)events2.callAgainB;
			S.DSScreens.Finisher[112] = true; S.DSScreens.R[112] = new int[] {0};
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(2);
			break;
		case events2.callC:
			S.DSKettley.Locked[174] = false;
			S.DSB.Locked[93] = false; S.DSD.Locked[80] = false; S.DSD.Locked[81] = false;
			if (S.warrantsUsed == 0) S.DSKettley.Locked[37] = false; else if (S.warrantsUsed == 1) S.DSKettley.Locked[38] = false;
			S.DSScreens.TriggersEvent[113] = (int)events2.callAgainC;
			S.DSScreens.Finisher[113] = true; S.DSScreens.R[113] = new int[] {0};
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(3);
			break;
		case events2.callD:
			S.DSKettley.Locked[175] = false;
			if (S.warrantsUsed == 0) S.DSKettley.Locked[37] = false; else if (S.warrantsUsed == 1) S.DSKettley.Locked[38] = false;
			S.DSScreens.TriggersEvent[114] = (int)events2.callAgainD;
			S.DSScreens.Finisher[114] = true; S.DSScreens.R[114] = new int[] {0};
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(4);
			break;
		case events2.callE:
			S.DSKettley.Locked[35] = false; S.nothingToTalk_Kettley = false;
			S.DSShawn.Locked[103] = false; S.nothingToTalk_Shawn = false;
			S.DSAshley.Locked[95] = false; S.nothingToTalk_Ashley = false; if (S.DSAshley.Used[48]) { S.DSAshley.Used[48] = false; S.DSAshley.Used[96] = false; }
			S.DSScreens.TriggersEvent[115] = (int)events2.callAgainE;
			S.DSScreens.Finisher[115] = true; S.DSScreens.R[115] = new int[] {0};
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(5);
			break;
		case events2.callF:
			S.DSScreens.R[4] = new int[] {130}; S.metF = true;
			for (int i = 336; i <= 338; i++) S.DSShawn.Locked[i] = false; S.nothingToTalk_Shawn = false;
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(6);
			S.npp = 1; DC.UPP();
			break;
		case events2.callAgainA: GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(1); break;
		case events2.callAgainB: GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(2); break;
		case events2.callAgainC: GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(3); break;
		case events2.callAgainD: GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(4); break;
		case events2.callAgainE: GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomIn(5); break;
		case events2.zoomOutSuspect: GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomOut(); break;
		
		case events2.askQuestionSuspectA:
			if (S.DSA.Used[12] && S.DSA.Used[13] && S.DSA.Used[14] && S.DSA.Used[15] && S.DSA.Used[16]) {
				S.DSA.Locked[17] = false; S.learnedSuspect[0] = true; PH2(S);
			}
			break;
		case events2.askQuestionSuspectB:
			if (S.DSB.Used[17] && S.DSB.Used[18] && S.DSB.Used[19] && S.DSB.Used[20] && S.DSB.Used[21]) {
				S.DSB.Locked[22] = false; S.learnedSuspect[1] = true; PH2(S);
			}
			break;
		case events2.askQuestionSuspectC: 
			if (S.DSC.Used[30] && S.DSC.Used[31] && S.DSC.Used[32] && S.DSC.Used[33]) {
				S.DSC.Locked[34] = false; S.learnedSuspect[2] = true; PH2(S);
			}
			if ((S.DSC.Used[30] || S.DSC.Used[31]) && S.DSAshley.Locked[93]) {
				S.DSAshley.Locked[93] = false; S.nothingToTalk_Ashley = false;
				if (S.DSAshley.Used[48]) { S.DSAshley.Used[48] = false; S.DSAshley.Used[96] = false; }
			}
			break;
		case events2.askQuestionSuspectD:
			if (S.DSD.Used[14] && S.DSD.Used[15] && S.DSD.Used[16] && S.DSD.Used[17]) {
				S.DSD.Locked[18] = false; S.learnedSuspect[3] = true; PH2(S);
			}
			break;
		case events2.holdThatThoughtSuspectA: S.DSA.Used[17] = false; S.DSA.Used[79] = false; break;
		case events2.holdThatThoughtSuspectB: S.DSB.Used[22] = false; S.DSB.Used[103] = false; break;
		case events2.holdThatThoughtSuspectC: S.DSC.Used[34] = false; S.DSC.Used[153] = false; break;
		case events2.holdThatThoughtSuspectD: S.DSD.Used[18] = false; S.DSD.Used[93] = false; break;
		case events2.accusedSuspectA:
			S.DSScreens.Finisher[111] = false; S.DSScreens.R[111] = new int[] {127}; S.DSScreens.TriggersEvent[111] = null;
			S.accusedSomebody = true;
			break;
		case events2.accusedSuspectB:
			S.DSScreens.Finisher[112] = false; S.DSScreens.R[112] = new int[] {127}; S.DSScreens.TriggersEvent[112] = null;
			S.accusedSomebody = true;
			break;
		case events2.accusedSuspectC:
			S.DSScreens.Finisher[113] = false; S.DSScreens.R[113] = new int[] {127}; S.DSScreens.TriggersEvent[113] = null;
			S.accusedSomebody = true;
			break;
		case events2.accusedSuspectD:
			S.DSScreens.Finisher[114] = false; S.DSScreens.R[114] = new int[] {127}; S.DSScreens.TriggersEvent[114] = null;
			if (S.DSD.Used[91]) S.DSD.Locked[99] = true; else S.DSD.Locked[98] = true;
			S.accusedSomebody = true;
		break;
		case events2.SuspectA78: S.DSA.R[84] = new int[] {87}; S.DSA.R[84] = new int[] {87}; break;
		case events2.SuspectA2ndTime: S.DSA.R[18] = new int[] {104}; break;
		case events2.SuspectA3rdTime: S.DSA.R[18] = new int[] {10}; break;
		case events2.SuspectB35: S.DSB.R[79] = new int[] {82}; break;
		case events2.SuspectB121:
			if (!S.DSB.Used[116] || !S.DSB.Used[117] || !S.DSB.Used[118] || !S.DSB.Used[119] || !S.DSB.Used[120]) {
				S.DSB.Used[23] = false; S.DSB.Used[121] = false;
			}
		break;
		case events2.SuspectB2ndTime: S.DSB.R[24] = new int[] {164}; break;
		case events2.SuspectB3rdTime: S.DSB.R[24] = new int[] {15}; break;
		case events2.SuspectD27: S.DSD.R[47] = new int[] {48}; break;
		case events2.seenCloeAtMeetUp: S.DSE.Locked[46] = false; break;
		case events2.infiniteCloe:
			S.DSE.Used[121] = false; S.DSE.Used[123] = false;
			GameObject.Find("CollidersB/Screens").GetComponent<OnClick_Screens>().ZoomOut();
		break;

		//-----------------------------------------KETTLEY-----------------------------------------
		case events2.metKettley:
			S.metKettley = true;
			if (!S.metShawn) { S.npp = 1; DC.UPP(); }
			if (S.metShawn) { DC.ppArchwayL.gameObject.SetActive(true); S.npp = 2; DC.UPP(); }
			break;
		case events2.pardner: S.DSKettley.Locked[40] = false; break;
		case events2.KettleyInitial: if (S.DSKettley.Used[31] && S.DSKettley.Used[32]) S.DSKettley.Locked[41] = false; break;
		case events2.KettleyQ56: if (!S.DSKettley.Used[57]) S.DSKettley.R[58] = new int[] {57}; break;
		case events2.KettleyQ57: if (!S.DSKettley.Used[56]) S.DSKettley.R[59] = new int[] {56}; break;
		case events2.KettleyPrisons: S.prisonsTalkKettley = true; break;
		case events2.KettleyBanterQuestion: //if asked three banter questions, unlock "grumpy cat" question
			if (S.DSKettley.Used[72] && S.DSKettley.Used[73] && S.DSKettley.Used[74]) S.DSKettley.Locked[76] = false;
		break;
		case events2.KettleyBanterFinish:
			if (!S.DSKettley.Used[75] || !S.DSKettley.Used[76]) {
				S.DSKettley.Used[36] = false; S.DSKettley.Used[77] = false;
				S.DSKettley.R[36] = new int[] {71};
			}
		break;
		case events2.KettleyWaitWithSearch:
			S.DSKettley.Used[176] = false;
			if (S.warrantsUsed == 0) S.DSKettley.Used[37] = false;
			else if (S.warrantsUsed == 1) S.DSKettley.Used[38] = false;
			break;
		case events2.searchA: S.DSKettley.R[180] = new int[] {190}; S.DSKettley.R[181] = new int[] {190}; S.nextDID_A = 115; break;
		case events2.searchB: S.DSKettley.R[180] = new int[] {215}; S.DSKettley.R[181] = new int[] {215}; S.nextDID_B = 171; break;
		case events2.searchC: S.DSKettley.R[180] = new int[] {232}; S.DSKettley.R[181] = new int[] {232}; S.nextDID_C = 165; break;
		case events2.searchD: S.DSKettley.R[180] = new int[] {257}; S.DSKettley.R[181] = new int[] {257}; S.DSShawn.Locked[102] = false; S.nothingToTalk_Shawn = false; break;
		case events2.KettleySearches:
			S.warrantsUsed++; if (S.warrantsUsed == 1) {
				S.DSKettley.Locked[38] = false;
				S.DSKettley.R[172] = new int[] {181}; S.DSKettley.R[173] = new int[] {181};
				S.DSKettley.R[174] = new int[] {181}; S.DSKettley.R[175] = new int[] {181};
			} else if (S.warrantsUsed == 2) {
				S.DSKettley.Used[38] = false; S.DSKettley.R[38] = new int[] {182};
			}
			DC.noZoomOut = true; DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.UIC.blackScreenAppear(0.01F, (int)events2.WaitForKet1);
			break;
		case events2.WaitForKet1: DC.UIC.DelayedTrigger(2, (int)events2.WaitForKet2); break;
		case events2.WaitForKet2:
			if (DC.S.SC2.trackedF) DC.UIC.blackScreenDisappear(0.01F, (int)events2.didArrestHim);
			else DC.UIC.blackScreenDisappear(0.01F, (int)events2.KettleyReturns);
			break;
		case events2.KettleyReturns:
			DC.noZoomOut = false; DC.CursorLock(false); DC.bMenu.SetActive(true);
			DC.UIC.StartDialogue(DC.DialogueKettley, DC.S.SC2.DSKettley, 0, DC.S.SC2.nextDID_Kettley, true);
			break;
		case events2.KettleyWaitWithArrest: S.DSKettley.Used[39] = false; S.DSKettley.Used[295] = false; break;
		case events2.KettleyArrestGo:
			DC.noZoomOut = true; DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.UIC.blackScreenAppear(0.01F, (int)events2.WaitForKet1);
			break;
		case events2.arrestA:
			S.DSKettley.R[300] = new int[] {301}; S.DSAshley.Locked[235] = false;
			for (int i = 256; i <= 259; i++) S.DSAshley.R[i] = new int[] {198};
			S.nextDID_Shawn = 390; S.nextDID_Ashley = 230; S.nothingToTalk_Shawn = false; S.nothingToTalk_Ashley = false;
			S.DSScreens.R[4] = new int[] {130};
			S.arrestedAorC = true; S.npp = 1; DC.UPP();
			break;
		case events2.arrestB:
			S.DSKettley.R[300] = new int[] {306}; S.DSAshley.Locked[236] = false;
			for (int i = 256; i <= 259; i++) S.DSAshley.R[i] = new int[] {198};
			S.nextDID_Shawn = 390; S.nextDID_Ashley = 230; S.nothingToTalk_Shawn = false; S.nothingToTalk_Ashley = false;
			S.DSScreens.R[4] = new int[] {130};
			S.arrestedBorD = true; S.npp = 1; DC.UPP();
			break;
		case events2.arrestC:
			S.DSKettley.R[300] = new int[] {310}; S.DSAshley.Locked[237] = false;
			S.nextDID_Shawn = 390; S.nextDID_Ashley = 230; S.nothingToTalk_Shawn = false; S.nothingToTalk_Ashley = false;
			S.DSScreens.R[4] = new int[] {130};
			S.arrestedAorC = true; S.npp = 1; DC.UPP();
			break;
		case events2.arrestD:
			S.DSKettley.R[300] = new int[] {316}; S.DSAshley.Locked[238] = false;
			for (int i = 256; i <= 259; i++) S.DSAshley.R[i] = new int[] {198};
			S.nextDID_Shawn = 390; S.nextDID_Ashley = 230; S.nothingToTalk_Shawn = false; S.nothingToTalk_Ashley = false;
			S.DSScreens.R[4] = new int[] {130};
			S.arrestedBorD = true; S.npp = 1; DC.UPP();
			break;
		case events2.KettleyDoesIt:
			DC.noZoomOut = true; DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.UIC.blackScreenAppear(0.01F, (int)events2.WaitForKet1);
			DC.MStop();
			break;
		case events2.didArrestHim:
			DC.noZoomOut = false; DC.CursorLock(false); DC.bMenu.SetActive(true);
			DC.MPlay(DC.post, 0);
			DC.UIC.StartDialogue(DC.DialogueKettley, DC.S.SC2.DSKettley, 0, DC.S.SC2.nextDID_Kettley, true);
			S.DSShawn.Locked[361] = false; S.nothingToTalk_Shawn = false;
			S.DSAshley.Locked[181] = false; S.DSAshley.Locked[182] = false; S.DSAshley.Locked[209] = false;
			S.nothingToTalk_Ashley = false;
			S.realCulpritArrested = true;
			S.npp = 1; DC.UPP();
			break;
		
		//-----------------------------------------SHAWN-----------------------------------------
		case events2.metShawn:
			S.metShawn = true; S.DSKettley.Locked[33] = true;
			if (!S.metKettley) { S.npp = 0; DC.UPP(); }
			else { DC.ppArchwayL.gameObject.SetActive(true); S.npp = 2; DC.UPP(); }
			break;
		case events2.briefed:
			S.briefedToSet = true; S.DSScreens.R[4] = new int[] {110};
			S.nextDID_Kettley = 145; S.DSKettley.Locked[39] = false; S.nothingToTalk_Kettley = false;
			S.DSAshley.Locked[47] = false;
			S.npp = 3; DC.UPP();
			break;
		case events2.enoughBriefing:
			if (S.DSShawn.Locked[95]) {
				bool askedEverything = true; for (int i = 38; i <= 49; i++) if (!S.DSShawn.Used[i]) askedEverything = false;
				if (!askedEverything) S.DSShawn.Locked[95] = false;
			}
			break;
		case events2.enoughBriefing2:
			bool askedAll = true; for (int i = 38; i <= 49; i++) if (!S.DSShawn.Used[i]) askedAll = false;
			if (!askedAll) { S.DSShawn.Used[85] = false; S.DSShawn.Used[95] = false; }
			break;
		case events2.loverBoy:
			S.DSAshley.Locked[93] = false; S.nothingToTalk_Ashley = false;
			if (S.DSAshley.Used[48]) { S.DSAshley.Used[48] = false; S.DSAshley.Used[96] = false; }
			break;
		case events2.banterShawn:
			if (!S.DSShawn.Used[201] || !S.DSShawn.Used[202] || !S.DSShawn.Used[203]) { 
				S.DSShawn.Used[104] = false; S.DSShawn.Used[204] = false;
			}
			break;
		case events2.tightSchedule:
			S.DSShawn.Locked[376] = false; S.DSShawn.Locked[375] = true;
			S.DSShawn.Locked[398] = false; S.DSShawn.Locked[397] = true;
			break;
		case events2.wtfIsMeetup1: S.DSShawn.Locked[121] = true; break;
		case events2.wtfIsMeetup2: S.DSShawn.R[116] = new int[] {95, 96, 98, 99, 100, 101, 102, 103, 104, 105}; break;
		case events2.searchD2: S.nextDID_D = 105; S.DSD.Locked[106] = false; break;
		case events2.notSoSimple:
			S.phaseTwoToSet = true; S.DSScreens.Locked[31] = false;
			S.npp = 3; DC.UPP();
			break;
		case events2.ShawnSearchA:
			DC.CursorLock(true); DC.bMenu.SetActive(false);
			S.ShawnSearch = true; DC.MStop();
			DC.noZoomOut = true; DC.UIC.blackScreenAppear(0.01F, (int)events2.ShawnSearchB);
			break;
		case events2.ShawnSearchB:
			DC.UIC.DelayedTrigger(1, (int)events2.ShawnSearchB2);
			break;
		case events2.ShawnSearchB2:
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			DC.UIC.StartNarration(4);
			break;
		case events2.ShawnSearchC:
			DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.UIC.blackScreenDisappear(0.01F, (int)events2.ShawnSearchD);
			break;
		case events2.ShawnSearchD:
			DC.CursorLock(false); DC.bMenu.SetActive(true);
			DC.noZoomOut = false;
			DC.UIC.StartDialogue(DC.DialogueShawn, DC.S.SC2.DSShawn, 1, DC.S.SC2.nextDID_Shawn, true);
			break;
		case events2.theyDontExist:
			S.ShawnSearch = false; S.theyDontExist = true; S.nothingToTalk_Ashley = false;
			S.DSAshley.Locked[150] = false; for (int i = 45; i <= 48; i++) S.DSAshley.Locked[i] = true;
			S.DSKettley.Locked[37] = true; S.DSKettley.Locked[38] = true; S.DSKettley.Locked[39] = true;
			S.DSScreens.R[4] = new int[] {130};
			var colB = GameObject.Find("CollidersB").transform;
			colB.Find("Closet").gameObject.SetActive(false);
			colB.Find("Tree").gameObject.SetActive(false);
			colB.Find("Guitar").gameObject.SetActive(false);
			DC.MPlay(DC.fakes, 0);
			S.npp = 2; DC.UPP();
			break;
		case events2.queueTheDramaticReveal:
			S.queueTheReveal = true; S.foundF = true;
			S.npp = 3; DC.UPP();
			break;
		case events2.goArrestHim:
			S.trackedF = true; S.nextDID_Kettley = 330; S.nothingToTalk_Kettley = false;
			S.npp = 0; DC.UPP();
			break;
		case events2.stickAround: S.DSAshley.Locked[196] = false; S.DSAshley.Locked[195] = true; break;
		
		//-----------------------------------------ASHLEY-----------------------------------------
		case events2.metAshley:
			S.metAshley = true;
			S.DSShawn.Locked[26] = false; S.nothingToTalk_Shawn = false;
			S.DSKettley.Locked[36] = false; S.nothingToTalk_Kettley = false;
			S.npp = 1; DC.UPP();
			break;
		case events2.notReallySuicidal: S.DSKettley.Locked[34] = false; S.DSShawn.Locked[100] = false; break;
		case events2.learnedAshleyView: S.learnedAshleyView = true; PH2(S); break;
		case events2.AshleyFriendQuestion: if (S.DSAshley.Used[87] && S.DSAshley.Used[88] && S.DSAshley.Used[89] && S.DSAshley.Used[90] && S.DSAshley.Used[91]) S.DSAshley.Locked[92] = false; break;
		case events2.enoughAboutFriends:
			bool askedAllAsh = true; for (int i = 87; i <= 95; i++) if (!S.DSAshley.Locked[i] && !S.DSAshley.Used[i]) askedAllAsh = false;
			if (!askedAllAsh) { S.DSAshley.Used[48] = false; S.DSAshley.Used[96] = false; }
			else if (askedAllAsh && !S.DSAshley.Used[48]) S.DSAshley.Used[48] = true;
			break;
		case events2.toldTheGoodNews:
			S.toldTheGoodNews = true; S.DSShawn.Locked[312] = false; S.nothingToTalk_Shawn = false;
			S.npp = 1; DC.UPP();
			break;
		
		//-----------------------------------------ROOM-----------------------------------------
		case events2.discussCloset:
			S.discussedCloset = true; DC.currentColliders.transform.Find("Closet").gameObject.SetActive(false);
			if (S.discussedTree && S.discussedGuitar) { S.DSOther.Locked[57] = false; S.DSOther.Locked[58] = false; }
			break;
		case events2.discussTree:
			S.discussedTree = true; DC.currentColliders.transform.Find("Tree").gameObject.SetActive(false);
			if (S.discussedCloset && S.discussedGuitar) { S.DSOther.Locked[57] = false; S.DSOther.Locked[58] = false; }
			break;
		case events2.discussGuitar:
			S.discussedGuitar = true; DC.currentColliders.transform.Find("Guitar").gameObject.SetActive(false);
			if (S.discussedCloset && S.discussedTree) { S.DSOther.Locked[57] = false; S.DSOther.Locked[58] = false; }
			break;
		default: break;
		}
	}
}
