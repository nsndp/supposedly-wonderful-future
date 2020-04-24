using UnityEngine;
using System.Collections;

public enum events0 {
	seenTitle, introFinished, blackScreenAppear, blackScreenDisappear,
	playMessage, phoneStepAway,
	invitingJackie, invitingJackie2, invitingJackie3,
	kindOfAnAss, longTermProject,
	askingQ1, askingQ3, askingMandatoryPartsInQ2,
	timeMachineQ, darkAlleyQ, finalConvincing,
	zoomOutJackie, finishPrologue
}

public static class EventsP {
	public static void UpdateDID(SavePrologue S, int npc, int did) {
		if (npc == 0) S.nextDIDJackie = did;
		else S.nextDIDOther = did;
	}

	public static void Trigger(SavePrologue S, int eventID) {
		var DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
		switch ((events0)eventID) {
		case events0.seenTitle: S.seenTitle = true; DC.UIC.StartNarration(0); break;
		case events0.introFinished: if (!DC.isAltMusic) DC.MPlay(DC.main, 0); else DC.MPlay(DC.mainAlt, 6.845F); break;
		case events0.blackScreenAppear: DC.UIC.blackScreenAppear(); break;
		case events0.blackScreenDisappear: DC.UIC.blackScreenDisappear(); S.blackScreen = false; break;

		case events0.playMessage: DC.Sound.clip = DC.phone; DC.Sound.Play(); S.heardMessages++; break;
		case events0.phoneStepAway:
			if (S.heardMessages == 1) S.nextDIDOther = 1;
			else if (S.heardMessages == 2) S.nextDIDOther = 2;
			else if (S.heardMessages == 3) DC.currentColliders.transform.Find("Phone").gameObject.SetActive(false);
			if (!S.invitedJackieIn) {
				S.knockKnock = true; DC.MStop(); DC.Sound2.Play();
				DC.currentColliders.transform.Find("Door").gameObject.SetActive(true);
				S.npp = 1; DC.UPP();
			}
			break;

		case events0.invitingJackie:
			S.invitedJackieIn = true;
			Cursor.visible = false; DC.bMenu.SetActive(false);
			DC.UIC.blackScreenAppear(0.01F, (int)events0.invitingJackie2);
			break;
		case events0.invitingJackie2:
			DC.A.transform.localPosition = DC.JackiePosB; DC.A.transform.localRotation = Quaternion.Euler(Vector3.zero); DC.A.Play("Sit");
			DC.currentColliders.transform.Find("Jackie").localPosition += new Vector3(10, 0, 0);
			DC.Door.GetComponent<Animation>()["DoorAnim"].speed = -1.0F; DC.Door.GetComponent<Animation>().Play();
			DC.cam.localPosition = DC.camPosB; DC.cam.localRotation = DC.camRotB;
			GameObject.Find("Lights").transform.Find("CharacterLight").gameObject.SetActive(true);
			DC.A.transform.Find("Anchor_Body").localPosition = new Vector3(0, 0.3F, 0);
			DC.A.transform.Find("Anchor_Head").localPosition = new Vector3(0, 1.1F, -0.1F);
			DC.UIC.blackScreenDisappear(0.01F, (int)events0.invitingJackie3);
			//for trailer -->
			//DC.cam.localPosition = DC.camPosM; DC.cam.localRotation = DC.camRotM;
			//DC.A["Turn"].speed = 1.0F; DC.A["Turn"].time = DC.A["Turn"].length;
			//DC.A["Turn"].wrapMode = WrapMode.ClampForever; DC.A.Play("Turn");
			//DC.S.SP.nextDIDJackie = 76;
			//<-- for trailer
			break;
		case events0.invitingJackie3:
			Cursor.visible = true; DC.bMenu.SetActive(true);
			DC.UIC.StartDialogue(DC.DialogueJackie, DC.S.SP.DSJackie, 0, DC.S.SP.nextDIDJackie, false);
			S.npp = 3; DC.UPP();
			break;
		case events0.zoomOutJackie:
			DC.currentColliders.transform.Find("Jackie").GetComponent<OnClick_Jackie>().phase = 3;
			break;

		case events0.kindOfAnAss: S.kindOfAnAss = true; break;
		case events0.longTermProject: S.longTermProject = true; break;
		case events0.finalConvincing: S.DSJackie.R[280] = new int[] {282}; break;
		case events0.darkAlleyQ:
			S.DSJackie.R[209] = new int[] {242, 225, 240};
			S.preCalculations = true;
			break;
		case events0.timeMachineQ:
			S.DSJackie.R[217] = new int[] {242, 225, 240};
			S.DSJackie.R[221] = new int[] {242, 225, 240};
			S.preCalculations = true;
			break;
		case events0.askingQ1:
			if (S.DSJackie.Used[87]) { S.DSJackie.Locked[88] = false; S.DSJackie.Locked[89] = false; }
			break;
		case events0.askingQ3:
			if (S.DSJackie.Used[84] || S.DSJackie.Used[85]) { S.DSJackie.Locked[88] = false; S.DSJackie.Locked[89] = false; }
			break;
		case events0.askingMandatoryPartsInQ2:
			if (S.DSJackie.Used[128] && S.DSJackie.Used[129]) S.DSJackie.Locked[133] = false;
			break;
		case events0.finishPrologue:
			DC.MC.STEAM.Achievement("ACH_C0");
			DC.S.levelID = 1;
			DC.S.Save(COMMON.saveFolder + "Autosave.bin");
			COMMON.saveToLoad = "Autosave.bin";
			DC.MC.LoadLevel(DC.S.levelID, true);
			break;
		default: break;
		}
	}
}