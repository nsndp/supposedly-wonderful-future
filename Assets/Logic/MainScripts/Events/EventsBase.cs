using UnityEngine;
using System.Collections;

public static class Events {
	public static void UpdateDID(SaveGame S, int did) {
		switch (S.levelID) {
		case 0: EventsP.UpdateDID(S.SP, S.inDialogue, did); break;
		case 1: EventsC1.UpdateDID(S.SC1, S.inDialogue, did); break;
		case 2: EventsC2.UpdateDID(S.SC2, S.inDialogue, did); break;
		case 3: EventsC3.UpdateDID(S.SC3, did); break;
		case 4: EventsC4.UpdateDID(S.SC4, did); break;
		case 5: EventsC5.UpdateDID(S.SC5, did); break;
		default: EventsHub.UpdateDID(S.SH, S.inDialogue, did); break;
		}
	}

	public static void SetNothingToTalkAbout(SaveGame S) {
		switch (S.levelID) {
		case 1: EventsC1.SetNothingToTalkAbout(S.SC1, S.inDialogue); break;
		case 2: EventsC2.SetNothingToTalkAbout(S.SC2, S.inDialogue); break;
		default: break;
		}
	}

	public static void Trigger(SaveGame S, int eventID) {
		switch (S.levelID) {
		case 0: EventsP.Trigger(S.SP, eventID); if (Application.isEditor) Debug.Log((events0)eventID); break;
		case 1: EventsC1.Trigger(S.SC1, eventID); if (Application.isEditor) Debug.Log((events1)eventID); break;
		case 2: EventsC2.Trigger(S.SC2, eventID); if (Application.isEditor) Debug.Log((events2)eventID); break;
		case 3: EventsC3.Trigger(S.SC3, eventID); if (Application.isEditor) Debug.Log((events3)eventID); break;
		case 4: EventsC4.Trigger(S.SC4, eventID); if (Application.isEditor) Debug.Log((events4)eventID); break;
		case 5: EventsC5.Trigger(eventID); if (Application.isEditor) Debug.Log((events5)eventID); break;
		default: EventsHub.Trigger(S.SH, eventID); if (Application.isEditor) Debug.Log((eventsH)eventID); break;
		}
	}
}