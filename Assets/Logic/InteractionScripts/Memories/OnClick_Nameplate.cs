using UnityEngine;
using System.Collections;

public class OnClick_Nameplate : MonoBehaviour {

	DataControlChapter4 DC;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter4>();
	}

	void OnMouseDown() {
		DC.S.SC4.nextDID = 35;
		DC.UIC.StartDialogue(DC.Dialogue, DC.S.SC4.DStruct, 0, 35, false);
	}
}
