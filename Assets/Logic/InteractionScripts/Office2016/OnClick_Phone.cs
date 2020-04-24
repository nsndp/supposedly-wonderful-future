using UnityEngine;
using System.Collections;

public class OnClick_Phone : MonoBehaviour {

	public MeshRenderer PhoneLight;
	DataControlPrologue DC;
	int phase = 0; float tc = 0;
	Color ca = new Color(0.0745F, 1, 0, 1);
	Color ci = new Color(0.15F, 0.15F, 0.15F, 1);

	void Start () {
		DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
	}

	void OnMouseDown() {
		if (!DC.S.SP.checkedPhone) DC.S.SP.checkedPhone = true;
		DC.UIC.StartDialogue(DC.DialogueOther, DC.S.SP.DSOther, 1, DC.S.SP.nextDIDOther, true);
	}

	void Update () {
		if (!DC.S.SP.checkedPhone || phase > 0) {
			tc += Time.deltaTime;
			if (phase == 0 && tc > 5) { PhoneLight.material.SetColor("_Color", ca); phase = 1; tc = 0; }
			else if (phase == 1 && tc > 0.4F) { PhoneLight.material.SetColor("_Color", ci); phase = 2; tc = 0; }
			else if (phase == 2 && tc > 0.4F) { PhoneLight.material.SetColor("_Color", ca); phase = 3; tc = 0; }
			else if (phase == 3 && tc > 0.4F) { PhoneLight.material.SetColor("_Color", ci); phase = 4; tc = 0; }
			else if (phase == 4 && tc > 0.4F) { PhoneLight.material.SetColor("_Color", ca); phase = 5; tc = 0; }
			else if (phase == 5 && tc > 0.4F) { PhoneLight.material.SetColor("_Color", ci); phase = 0; tc = 0; }
		}
	}
}
