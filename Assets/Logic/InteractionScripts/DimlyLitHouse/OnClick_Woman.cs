using UnityEngine;
using System.Collections;

public class OnClick_Woman : CharacterOnClick {

	DataControlChapter1 DC;
	public Animation A; public GameObject HL;
	Vector3 pos1 = new Vector3(15.95F, 20, 0.7F);
	Vector3 pos2 = new Vector3(14.43F, 20, -0.01F);
	int state = 0;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter1>();
		A["Breathe"].speed = 0.02857F;
		A["CryBreathe"].speed = 0.02857F;
		A["Stand"].speed = 0.02F;
		A["Kneel"].speed = 0.02F;
	}

	public override void RedHL(bool on) {
		HL.GetComponent<Renderer>().material.SetColor("_TintColor", on ? new Color(1, 0.141F, 0.141F) : new Color(0, 0.463F, 1));
		HL.SetActive(on);
	}
	public override void MsEnter() { if (!DC.S.SC1.nothingToTalkWoman) { HL.SetActive(true); DC.activeHL = HL; } }
	public override void MsExit() { HL.SetActive(false); DC.activeHL = null; }
	void OnMouseEnter() { if (Cursor.visible) MsEnter(); }
	void OnMouseExit() { if (Cursor.visible) MsExit(); }
	void OnMouseDown() {
		if (!DC.S.SC1.nothingToTalkWoman)
			DC.UIC.StartDialogue(DC.DialogueWoman, DC.S.SC1.DSWoman, 1, DC.S.SC1.nextDIDWoman, false);
	}

	public void Switch1() { state = 1; A.CrossFade("CryPose"); }
	public void Switch1I() { state = 1; A["CryPose"].normalizedTime = 1; A.Play("CryPose"); } //I for Instant
	public void Switch1B() { state = 0; A["CryPose"].speed = -1; A.Play("CryPose"); }
	public void Switch2() { state = 2; A.transform.localPosition = pos1; A.Play("Stand"); }
	public void Switch3() { state = 3; A.transform.localPosition = pos2; A.Play("Kneel"); A.transform.localRotation = Quaternion.Euler(Vector3.zero); }
	void Update() {
		if (DC.paused) return;
		if (state == 0 && !A.isPlaying) A.Play("Breathe");
		if (state == 1 && !A.isPlaying) A.Play("CryBreathe");
		if (state == 2 && !A.isPlaying) A.Play("Stand");
		if (state == 3 && !A.isPlaying) A.Play("Kneel");
		//if (Input.GetKeyDown(KeyCode.Z)) Switch1();
		//if (Input.GetKeyDown(KeyCode.X)) Switch1B();
		//if (Input.GetKeyDown(KeyCode.C)) Switch2();
		//if (Input.GetKeyDown(KeyCode.V)) Switch3();
	}
}
