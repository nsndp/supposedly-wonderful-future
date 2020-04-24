using UnityEngine;
using System.Collections;

public class OldManAnimations : MonoBehaviour {

	DataControlChapter5 DC;
	Vector3 SrcPos, DstPos; Quaternion SrcRot, DstRot;
	float tc; public int phase = 0;

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter5>();
		SetCamVars();
		var headanims = new string[] {"Turned", "LowerHead", "LowerHeadTurned", "RelaxHead", "TakeGunHead"};
		for (int i = 0; i < headanims.Length; i++) {
			DC.OM[headanims[i]].AddMixingTransform(transform.Find("OldMan/master/root/spine/spine-1/chest/DEF-chest-1/DEF-neck"), true);
			DC.OM[headanims[i]].AddMixingTransform(transform.Find("OldMan/master/root/spine/spine-1/chest/chest-1/neck"), false);
			DC.OM[headanims[i]].AddMixingTransform(transform.Find("OldMan/master/root/spine/spine-1/chest/chest-1/neck/head"), true);
			DC.OM[headanims[i]].wrapMode = WrapMode.ClampForever;
			DC.OM[headanims[i]].layer = 2;
		}
		DC.OM["Main"].wrapMode = WrapMode.Loop; DC.OM["Main"].speed = 0.01F;
		DC.OM["LowerHead"].speed = 0.25F; DC.OM["LowerHeadTurned"].speed = 0.25F;
		DC.OM["Relax"].wrapMode = WrapMode.ClampForever; DC.OM["Relax"].speed = 0.5F;
		DC.OM["TakeGun"].wrapMode = WrapMode.ClampForever;
		DC.OM["RelaxHead"].speed = 0.5F;
	}

	public void SetCamVars() {
		SrcPos = COMMON.U.textLayout < 2 ? DC.PosFarA : DC.PosFarB;
		SrcRot = COMMON.U.textLayout < 2 ? Quaternion.Euler(DC.RotFarA) : Quaternion.Euler(DC.RotFarB);
		DstPos = COMMON.U.textLayout < 2 ? DC.PosCloseA : DC.PosCloseB;
		DstRot = COMMON.U.textLayout < 2 ? Quaternion.Euler(DC.RotCloseA) : Quaternion.Euler(DC.RotCloseB);
	}
	
	void Update() {
		//for trailer
		//if (Input.GetKeyDown(KeyCode.Q)) DC.OM.Play("LowerHead");
		//if (COMMON.trailerRecordMode == 3 && Input.GetKeyDown(KeyCode.F)) { Events.Trigger(DC.S, (int)events5.lookCloser); DC.OM.Play("LowerHead"); }
		//if (COMMON.trailerRecordMode == 3 && phase == 2 && tc > 1) DC.UIC.transform.Find("ForTrailer").GetComponent<TrailerRecord>().Go();

		if (DC.paused) return;
		if (phase == 1) {
			//DC.bMenu.SetActive(false);
			phase = 2; tc = 0;
		}
		else if (phase == 2 && tc <= 1) {
			DC.cam.transform.position = Vector3.Lerp(SrcPos, DstPos, Mathf.SmoothStep(0, 1, tc));
			DC.cam.transform.rotation = Quaternion.Lerp(SrcRot, DstRot, Mathf.SmoothStep(0, 1, tc));
			tc += 0.008333333333333333F * Time.deltaTime * 60;
		}
		else if (phase == 2 && tc > 1) {
			//DC.bMenu.SetActive(true);
			phase = 0;
		}

		else if (phase == 3 && !DC.S.inNarration) {
			DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC5.DS, 0, DC.S.SC5.nextDID, false);
			phase = 0;
		}
		else if (phase == 4 && DC.OM["Relax"].time > 0.4F) { //add a head movement to final animations
			//Debug.Log("ATAMA!");
			DC.OM.Stop("LowerHead"); DC.OM.Stop("LowerHeadTurned");
			DC.OM.Play("RelaxHead");
			phase = 0;
		}
		else if (phase == 5 && DC.OM["TakeGun"].time > 1.15F) { //add a head movement to final animations
			DC.OM["TakeGunHead"].speed = -0.6F;
			phase = 6;
		}
		else if (phase == 6 && DC.OM["TakeGunHead"].time < 0.5F) {
			DC.OM["TakeGunHead"].speed = 0;
			phase = 0;
		}
	}
}
