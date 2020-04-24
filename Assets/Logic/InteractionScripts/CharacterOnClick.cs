using UnityEngine;
using System.Collections;

public abstract class CharacterOnClick : MonoBehaviour {
	public abstract void MsEnter();
	public abstract void MsExit();
	public abstract void RedHL(bool on);
}
