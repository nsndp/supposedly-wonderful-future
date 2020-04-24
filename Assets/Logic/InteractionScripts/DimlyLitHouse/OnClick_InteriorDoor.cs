using UnityEngine;
using System.Collections;

public class OnClick_InteriorDoor : MonoBehaviour {

	public int to;
	RoomChange RC;

	void Start() {
		RC = GameObject.Find("Data").GetComponent<RoomChange>();
	}

	void OnMouseDown() {
		RC.DoIt(to);
	}
}
