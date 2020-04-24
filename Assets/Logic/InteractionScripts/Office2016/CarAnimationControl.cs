using UnityEngine;
using System.Collections;

public class CarAnimationControl : MonoBehaviour {

	DataControlPrologue DC;
	public GameObject car2, car3, car4;
	public Animation anim1, anim2, anim4; //Animation anim3;

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
		anim1 = this.GetComponent<Animation>();
		anim2 = car2.GetComponent<Animation>();
		//anim3 = car3.GetComponent<Animation>();
		anim4 = car4.GetComponent<Animation>();
	}

	void Update() {
		if (DC.paused) return;
		if (!anim1.isPlaying && Random.Range(0, 500) == 0) {
			anim1["Car1Moving"].speed = Random.Range(0.5F, 1.25F);
			anim1.Play();
		}
		if (!anim2.isPlaying && Random.Range(0, 500) == 0) {
			anim2["Car2Moving"].speed = Random.Range(0.5F, 1.25F);
			anim2.Play();
		}
		/*if (!anim3.isPlaying && Random.Range(0, 500) == 0) {
			anim3["Car3Moving"].speed = Random.Range(0.5F, 1.25F);
			anim3.Play();
		}*/
		if (!anim4.isPlaying && Random.Range(0, 500) == 0) {
			anim4["Car4Moving"].speed = Random.Range(0.5F, 1.25F);
			anim4.Play();
		}
	}
}
