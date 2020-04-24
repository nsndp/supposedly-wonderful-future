using UnityEngine;
using System.Collections;

public enum events5 {
	blackScreenOff, startDialogue,
	lookCloser, heardEnough,
	agreed, agreedAnim, refused, refusedAnim, gunShot, rollCredits
}

public static class EventsC5 {
	public static void UpdateDID(SaveChapter5 S, int did) {
		S.nextDID = did;
	}

	public static void Trigger(int eventID) {
		var DC = GameObject.Find("Data").GetComponent<DataControlChapter5>();
		switch ((events5)eventID) {
		case events5.blackScreenOff: DC.UIC.blackScreenDisappear(0.008F); break;
		case events5.startDialogue: DC.OM.GetComponent<OldManAnimations>().phase = 3; break;
		case events5.lookCloser:
			DC.OM.GetComponent<OldManAnimations>().phase = 1; DC.S.SC5.closeUp = true;
			DC.BGM.clip = DC.main; DC.BGM.PlayDelayed(0.2F); DC.loopAt = 22.571F;
			break;
		case events5.heardEnough:
			DC.S.SC5.heardEnough = true;
			DC.OM.Play(COMMON.U.textLayout < 2 ? "LowerHead" : "LowerHeadTurned");
			DC.musicFadeOut = true; DC.playNext = DC.intro; DC.loopAt = 4.892F;
			break;
		case events5.agreed:
			DC.S.SC5.agreed = true; DC.OM.transform.Find("Handgun").gameObject.SetActive(false);
			DC.Sound.clip = DC.gun; DC.Sound.Play();
			DC.UIC.DelayedTrigger(1, (int)events5.agreedAnim);
			break;
		case events5.refused:
			DC.S.SC5.refused = true;
			DC.UIC.DelayedTrigger(1, (int)events5.refusedAnim);
			break;
		case events5.agreedAnim:
			DC.OM.CrossFade("Relax", 1); //only on this layer; head movements are separate on layer 2
			DC.OM.GetComponent<OldManAnimations>().phase = 4;
			break;
		case events5.refusedAnim:
			DC.OM.Stop("LowerHead"); DC.OM.Stop("LowerHeadTurned");
			DC.OM.Play("TakeGunHead"); DC.OM.CrossFade("TakeGun", 1);
			DC.OM.GetComponent<OldManAnimations>().phase = 5;
			break;
		case events5.gunShot:
			DC.CursorLock(true); DC.bMenu.SetActive(false);
			DC.Cred.gameObject.SetActive(true);
			DC.BGM.Stop(); DC.Sound.clip = DC.shot; DC.Sound.Play();
			DC.UIC.DelayedTrigger(5, (int)events5.rollCredits);
			break;
		case events5.rollCredits:
			DC.MPlay(DC.credits, 0);
			DC.Cred.GetComponent<Credits>().Go();
			break;
		default: break;
		}
	}
}