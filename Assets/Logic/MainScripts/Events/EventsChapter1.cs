using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum events1 {
	blackScreenAppear, blackScreenDisappear,
	seenTitle, seenIntro, metMitty, metWoman,
	gotFirstGift, gaveFirstGift, gotSecondGift, gaveSecondGift,
	sawMittyProperly, gotExplanation, foundPill, beenForceful, suckItUp,
	holdThatThoughtA, holdThatThoughtB, choicePointOfNoReturn,
	chosePretense, womanGettingUp, womanGotUp, startPretense, inPretense1, inPretense2, inPretense3,
	choseEuthanasia, pickedPill, didEuthanasia1, didEuthanasia2, didEuthanasia3, storyFinised,
	checkedFrontDoor, triedAllFrontDoor, askedAboutDoor, askedTimeIsStill, checkedPhotoCollage,
	finishLevel
}

public static class EventsC1 {
	public static void UpdateDID(SaveChapter1 S, int npc, int did) {
		if (npc == 0) S.nextDIDMitty = did;
		else if (npc == 1) S.nextDIDWoman = did;
		else if (npc == 2) S.nextDIDFrontDoor = did;
	}

	public static void SetNothingToTalkAbout(SaveChapter1 S, int npc) {
		if (npc == 0) S.nothingToTalkMitty = true;
		else if (npc == 1) S.nothingToTalkWoman = true;
	}

	public static void Trigger(SaveChapter1 S, int eventID) {
		var DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		switch ((events1)eventID) {
		case events1.blackScreenAppear:
			DC.UIC.blackScreenAppear();
		break;
		case events1.blackScreenDisappear:
			DC.UIC.blackScreenDisappear(); S.blackScreen = false;
		break;
		case events1.seenTitle:
			S.seenTitle = true; DC.UIC.StartNarration(0);
		break;
		case events1.seenIntro:
			S.seenIntro = true; DC.MPlay(DC.main, 31.065F);
		break;
		case events1.checkedFrontDoor:
			S.checkedFrontDoor = true;
			S.DSMitty.Locked[30] = false; S.DSMitty.Locked[29] = true;
			S.DSWoman.Locked[5] = true; S.DSWoman.Locked[6] = false;
			S.nothingToTalkMitty = false;
			S.nothingToTalkWoman = false;
		break;
		case events1.triedAllFrontDoor:
			if (S.DSOther.Used[3] && S.DSOther.Used[4] && S.DSOther.Used[5]) S.triedAllFrontDoor = true;
		break;
		case events1.askedAboutDoor:
			S.askedAboutDoor = true;
			S.nextDIDFrontDoor = 15;
		break;
		case events1.askedTimeIsStill:
			S.askedTimeIsStill = true;
			if (S.metMitty && S.checkedPhotoCollage) DC.room[3].Find("Colliders/Photo1").gameObject.SetActive(true);
		break;
		case events1.checkedPhotoCollage:
			S.checkedPhotoCollage = true;
			DC.room[3].Find("Colliders/Photo2").gameObject.SetActive(true);
			DC.room[3].Find("Colliders/Photo3").gameObject.SetActive(true);
			if (S.askedTimeIsStill) DC.room[3].Find("Colliders/Photo1").gameObject.SetActive(true);
		break;
		case events1.metMitty:
			S.metMitty = true;
			DC.room[0].Find("Colliders/Pinboard").gameObject.SetActive(true);
			DC.room[3].Find("Colliders/PhotoCollage").gameObject.SetActive(true);
		break;
		case events1.metWoman:
			S.metWoman = true;
			S.DSMitty.Locked[31] = false; S.nothingToTalkMitty = false;
			S.npp = 1; DC.UPP();
		break;
		case events1.gotFirstGift:
			S.gotFirstGift = true;
			S.DSWoman.Locked[35] = false; S.DSWoman.Locked[36] = false;
			S.nothingToTalkWoman = false;
			DC.room[0].Find("Colliders/TV").gameObject.SetActive(true);
			DC.room[0].Find("Colliders/TV").GetComponent<OnClick_TV>().TurnOn();
			S.npp = 0; DC.UPP();
		break;
		case events1.gaveFirstGift:
			S.gaveFirstGift = true;
			S.DSMitty.Locked[75] = false; S.DSMitty.Locked[76] = false; S.nothingToTalkMitty = false;
			S.npp = 1; DC.UPP();
		break;
		case events1.gotSecondGift:
			S.gotSecondGift = true;
			S.DSWoman.Locked[57] = false; S.nothingToTalkWoman = false;
			S.npp = 0; DC.UPP();
		break;
		case events1.gaveSecondGift:
			S.gaveSecondGift = true;
			S.DSMitty.Locked[108] = false; S.nothingToTalkMitty = false;
			if (S.tvIsOn) DC.room[0].Find("Colliders/TV").GetComponent<OnClick_TV>().TurnOff();
			S.npp = 1; DC.UPP();
			DC.MChange(DC.prereveal, 0.977F);
			DC.room[0].Find("Colliders/Pinboard").gameObject.SetActive(false);
			DC.room[3].Find("Colliders/Umbrella").gameObject.SetActive(false);
			DC.room[3].Find("Colliders/Shoes").gameObject.SetActive(false);
			DC.room[3].Find("Colliders/PhotoCollage").gameObject.SetActive(false);
			DC.room[3].Find("Colliders/Photo2").gameObject.SetActive(false);
		break;
		case events1.sawMittyProperly:
			DC.LightsOnWorkaround(true);
			DC.Sound.clip = DC.lightSwitch; DC.Sound.Play();
			DC.MChange(DC.reveal, 0);
			S.sawMittyProperly = true;
			S.DSWoman.Locked[91] = false; S.DSWoman.Locked[92] = false; S.nothingToTalkWoman = false;
			S.npp = 0; DC.UPP();
		break;
		case events1.gotExplanation:
			S.gotExplanation = true;
			S.DSMitty.Locked[120] = false; S.DSMitty.Locked[121] = false; S.DSMitty.Locked[122] = false;
			S.nothingToTalkMitty = false;
			if (!S.foundPill) { S.npp = 2; DC.UPP(); }
			DC.MChange(DC.explanation, 7.376F);
		break;
		case events1.foundPill:
			S.foundPill = true;
			S.DSWoman.Locked[122] = false;
			S.nothingToTalkWoman = false;
			S.npp = 0; DC.UPP();
		break;
		case events1.beenForceful: S.beenForceful = true; break;
		case events1.suckItUp: S.DSWoman.R[149] = new int[] {151}; S.DSWoman.R[153] = new int[] {155}; break;
		case events1.holdThatThoughtA: S.DSWoman.Used[135] = false; S.DSWoman.Used[145] = false; break;
		case events1.holdThatThoughtB: S.DSWoman.Used[134] = false; S.DSWoman.Used[159] = false; break;
		case events1.choicePointOfNoReturn:
			S.choicePNR = true;
			DC.MChange(DC.finale, 0);
		break;
		case events1.chosePretense:
			Cursor.visible = false; DC.bMenu.SetActive(false);
			S.nothingToTalkMitty = true;
			S.chosePretense = true; S.npp = 1; DC.UPP();
			S.blackScreen = true; DC.UIC.blackScreenAppear(0.01F, (int)events1.womanGettingUp);
		break;
		case events1.womanGettingUp:
			Cursor.visible = true; DC.bMenu.SetActive(true);
			DC.UIC.StartNarration(6);
		break;
		case events1.womanGotUp:
			S.womanGotUp = true; DC.WomanSCR.Switch2();
			S.blackScreen = false; DC.UIC.blackScreenDisappear();
		break;
		case events1.startPretense:
			S.sawEachOther = true; DC.MittySCR.Switch2();
			DC.UIC.StartNarration(15);
		break;
		case events1.inPretense1:
			Cursor.visible = false; DC.bMenu.SetActive(false);
			S.womanCollapsed = true; DC.MStop();
			S.blackScreen = true; DC.UIC.blackScreenAppear(0.01F, (int)events1.inPretense2);
		break;
		case events1.inPretense2:
			Cursor.visible = true; DC.bMenu.SetActive(true);
			DC.UIC.StartNarration(26);
		break;
		case events1.inPretense3:
			DC.WomanSCR.Switch3(); DC.MittySCR.Switch3();
			S.blackScreen = false; DC.UIC.blackScreenDisappear();
			S.inPretense = true; DC.MPlay(DC.pretense, 7.16F);
			DC.cam[1].transform.localPosition = DC.camPos[4] + new Vector3(0, 20, 0);
			DC.cam[1].transform.localRotation = DC.camRot[4];
		break;
		case events1.choseEuthanasia:
			S.choseEuthanasia = true; DC.WomanSCR.Switch1();
			S.npp = 2; DC.UPP();
		break;
		case events1.pickedPill:
			S.pickedPill = true;
			DC.Sound.clip = DC.pillPickup; DC.Sound.Play();
			S.DSMitty.Locked[120] = true; S.DSMitty.Locked[121] = true; S.DSMitty.Locked[122] = true;
			S.DSMitty.Locked[133] = false; S.DSMitty.Locked[134] = false; S.nothingToTalkMitty = false;
			S.npp = 1; DC.UPP();
		break;
		case events1.didEuthanasia1:
			Cursor.visible = false; DC.bMenu.SetActive(false);
			DC.MStop(); S.gavePill = true; S.blackScreen = true;
			DC.UIC.blackScreenAppear(0.01F, (int)events1.didEuthanasia2);
		break;
		case events1.didEuthanasia2:
			Cursor.visible = true; DC.bMenu.SetActive(true);
			DC.LightsOnWorkaround(false);
			DC.MittySCR.Switch1(); DC.WomanSCR.Switch1B();
			DC.UIC.StartNarration(8);
		break;
		case events1.didEuthanasia3:
			S.DSWoman.Locked[181] = false; S.DSWoman.Locked[182] = false; S.nothingToTalkWoman = false;
			DC.UIC.blackScreenDisappear(); S.blackScreen = false;
			DC.MPlay(DC.euthanasia, 0); S.didEuthanasia = true;
			S.npp = 0; DC.UPP();
		break;
		case events1.storyFinised:
			S.storyFinished = true;
			S.npp = 3; DC.UPP();
			//same as in SetStates()
			DC.room[0].Find("Colliders/Pinboard").gameObject.SetActive(true);
			var scr = DC.room[3].Find("Colliders/Shoes").GetComponent<CommentOnClick>();
			scr.StartingCommentID = 82; scr.EndingCommentID = 82; S.CCID[scr.ObjectIndex] = 82;
			scr.gameObject.SetActive(true);
			scr = DC.room[3].Find("Colliders/Photo1").GetComponent<CommentOnClick>();
			scr.StartingCommentID = 83; scr.EndingCommentID = 83; S.CCID[scr.ObjectIndex] = 83;
			scr.gameObject.SetActive(true);
			scr = DC.room[3].Find("Colliders/Photo3").GetComponent<CommentOnClick>();
			scr.StartingCommentID = 84; scr.EndingCommentID = 84; S.CCID[scr.ObjectIndex] = 84;
			scr.gameObject.SetActive(true);
			//opening the door
			var mesh = DC.room[3].Find("FrontDoor/Lock").GetComponent<MeshFilter>().mesh;
			float shift = 0.5002F; var uvs = new Vector2[4];
			for (int i = 0; i < 4; i++) uvs[i] = new Vector2(mesh.uv[i].x + shift, mesh.uv[i].y);
			mesh.uv = uvs;
			S.nextDIDFrontDoor = 21;
			DC.Sound.clip = DC.eLock; DC.Sound.Play();
			//narration
			int ind = !S.visitedEntryway ? 12 : (!S.triedAllFrontDoor ? 11 : 10);
			DC.UIC.StartNarration(ind);
		break;
		case events1.finishLevel:
			DC.S.levelID = 11;
			DC.S.Save(COMMON.saveFolder + "Autosave.bin");
			COMMON.saveToLoad = "Autosave.bin";
			DC.MC.LoadLevel(DC.S.levelID, true);
		break;
		default: break;
		}
	}
}