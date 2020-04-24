using UnityEngine;
using System.Collections;

public class Projection : MonoBehaviour {

	DataControlChapter5 DC;
	float tc = 0; int k = 0;
	Renderer Ef, Em, Pr;
	int transPhase = 0; float h1, h2, h3, s1, s2, s3, v1, v2, v3, t;

	Color[] Emitters = new Color[] {
		new Color(0.839F, 0.792F, 0.537F), new Color(0.5725F, 0.839F, 0.537F), new Color(0.537F, 0.588F, 0.839F),
		new Color(0.792F, 0.663F, 0.522F), new Color(0.643F, 0.788F, 0.412F)
	};
	Color[] Effects = new Color[] {
		new Color(0.176F, 0.137F, 0.0745F), new Color(0.063F, 0.141F, 0.082F), new Color(0.082F, 0.09F, 0.141F),
		new Color(0.141F, 0.114F, 0.082F), new Color(0.063F, 0.094F, 0.012F)
	};
	Color[] Emissions = new Color[] {
		new Color(0.067F, 0.047F, 0.02F), new Color(0.031F, 0.067F, 0.039F), new Color(0.012F, 0.035F, 0.059F),
		new Color(0.059F, 0.047F, 0.035F), new Color(0.027F, 0.043F, 0.004F)
	};
	public Texture[] Tex;
	//unsplash 44		214 202 137 / 45 35 19 / 17 12 5
	//michigan trees 	146 214 137 / 16 36 21 / 8 17 10
	//unsplash 75 		137 150 214 / 21 23 36 / 3 9 15
	//tuscany 			202 169 133 / 36 29 21 / 15 12 9
	//trees				164 201 105 / 16 24 33 / 7 11 1

	void Start() {
		DC = GameObject.Find("Data").GetComponent<DataControlChapter5>();

		Ef = transform.Find("Effect").GetComponent<Renderer>();
		Em = transform.Find("Emission").GetComponent<Renderer>();
		Pr = transform.Find("Projection").GetComponent<Renderer>();

		GetComponent<Renderer>().materials[1].SetColor("_Color", Emitters[k]);
		Ef.material.SetColor("_TintColor", Effects[k]); 
		Em.material.SetColor("_TintColor", Emissions[k]);
		Pr.material.mainTexture = Tex[k]; Pr.material.SetTexture("_EmissionMap", Tex[k]);

		Ef.transform.localRotation = Quaternion.Euler(Vector3.zero);
		Em.transform.localRotation = Quaternion.Euler(Vector3.zero);
		Pr.transform.localRotation = Quaternion.Euler(Vector3.zero);
	}

	void Update() {
		if (DC.paused) return;
		var rot = Quaternion.Euler(Vector3.Lerp(Vector3.zero, new Vector3(0, 0, 360), tc / 60)); //10 sec for testing
		Ef.transform.localRotation = rot;
		Em.transform.localRotation = rot;
		Pr.transform.localRotation = rot;
		tc += Time.deltaTime;
		if (tc > 60) { //and here change sec for testing too
			tc = 0;
			Color.RGBToHSV(Emitters[k], out h1, out s1, out v1);
			Color.RGBToHSV(Effects[k], out h2, out s2, out v2);
			Color.RGBToHSV(Emissions[k], out h3, out s3, out v3);
			transPhase = 1; t = 0;
		}

		if (transPhase == 1 && t <= 1) {
			GetComponent<Renderer>().materials[1].SetColor("_Color", Color.HSVToRGB(h1, s1, Mathf.Lerp(v1, 0, t)));
			Ef.material.SetColor("_TintColor", Color.HSVToRGB(h2, s2, Mathf.Lerp(v2, 0, t))); 
			Em.material.SetColor("_TintColor", Color.HSVToRGB(h3, s3, Mathf.Lerp(v3, 0, t)));
			var c = Pr.material.color; c.a = Mathf.Lerp(1, 0, t); Pr.material.SetColor("_Color", c);
			t += 0.02F * Time.deltaTime * 60;
		}
		else if (transPhase == 1 && t > 1) {
			k++; if (k == Tex.Length) k = 0;
			Pr.material.mainTexture = Tex[k]; Pr.material.SetTexture("_EmissionMap", Tex[k]);
			Color.RGBToHSV(Emitters[k], out h1, out s1, out v1);
			Color.RGBToHSV(Effects[k], out h2, out s2, out v2);
			Color.RGBToHSV(Emissions[k], out h3, out s3, out v3);
			transPhase = 2; t = 0;
		}
		else if (transPhase == 2 && t <= 1) {
			GetComponent<Renderer>().materials[1].SetColor("_Color", Color.HSVToRGB(h1, s1, Mathf.Lerp(0, v1, t)));
			Ef.material.SetColor("_TintColor", Color.HSVToRGB(h2, s2, Mathf.Lerp(0, v2, t))); 
			Em.material.SetColor("_TintColor", Color.HSVToRGB(h3, s3, Mathf.Lerp(0, v3, t)));
			var c = Pr.material.color; c.a = Mathf.Lerp(0, 1, t); Pr.material.SetColor("_Color", c);
			t += 0.02F * Time.deltaTime * 60;
		}
		else if (transPhase == 2 && t > 1) transPhase = 0;
	}
}
