using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sphere : MonoBehaviour {

	GameObject[] quadsZero; GameObject[][] quadsUp; GameObject[][] quadsDown;
	Texture2D[] T; Texture2D[] texZ; Texture2D[][] texU; Texture2D[][] texD;
	bool animate = false; bool oneWay = false; float speed = 5;

	void Start() {
		//cool test: for (int i = 0; i < 360; i += 30) for (int j = 30; j <= 150; j += 15) MakeQuad(0.5F, i, j, 0.1F, 0.1F, new Color(1, 0, 0), new Color(0.25F, 0, 0), true, null);
		T = Resources.LoadAll<Texture2D>("MM");
		Do();
	}

	void Do() {
		for (int i = 0; i < transform.childCount; i++) Destroy(transform.GetChild(i).gameObject);
		var l = new int[] {12, 12, 10, 6, 2}; var w = 0.2F; //72 quads
		//var l = new int[] {10, 10, 8, 6}; var w = 0.2F; //58 quads
		//var l = new int[] {9, 9, 8, 5, 1}; var w = 0.25F;
		InitMaterials(l);
		MakeSphere(0.5F, 20, w, l);
	}

	void InitMaterials(int[] l) {
		texZ = new Texture2D[l[0]]; texU = new Texture2D[l.Length-1][]; texD = new Texture2D[l.Length-1][];
		for (int i = 0; i < l.Length - 1; i++) {
			texU[i] = new Texture2D[l[i+1]];
			texD[i] = new Texture2D[l[i+1]];
		}
		//make a random order
		var ind = new int[T.Length];
		for (int i = 0; i < T.Length; i++) ind[i] = i;
		/*for (int i = 0; i < T.Length; i++) {
			var k = Random.Range(i, T.Length);
			var v = ind[i]; ind[i] = ind[k]; ind[k] = v;
		}*/
		//string s = ""; for (int i = 0; i < T.Length; i++) s += ind[i] + ", "; Debug.Log(s);
		//assign
		int z = 0;
		for (int i = 0; i < texZ.Length; i++) { texZ[i] = T[ind[z]]; z++; if (z >= ind.Length) z = 0; }
		for (int i = 0; i < texU.Length; i++) for (int j = 0; j < texU[i].Length; j++) { texU[i][j] = T[ind[z]]; z++; if (z >= ind.Length) z = 0; }
		for (int i = 0; i < texD.Length; i++) for (int j = 0; j < texD[i].Length; j++) { texD[i][j] = T[ind[z]]; z++; if (z >= ind.Length) z = 0; }
	}

	void MakeSphere(float r, float phiStep, float scaleX, int[] lines) {
		float scaleY = scaleX / 1.77777777777777777777777777777F;
		//Color c1 = new Color(1, 0, 0); Color c2 = new Color(0.25F, 0, 0);
		//Color c1 = new Color(0, 0.7F, 1); Color c2 = new Color(0, 0.175F, 0.25F);
		Color c1 = Color.white; Color c2 = Color.black; //new Color(0.2F, 0.2F, 0.2F);

		//lines[0] says how many quads should be in the central ring,
		//lines[1] - in the rings above and below the central one,
		//lines[2] - in the 2nd ring above and 2nd ring below, and so on
		quadsUp = new GameObject[lines.Length - 1][];
		quadsDown = new GameObject[lines.Length - 1][];

		for (int i = 0; i < lines.Length; i++) {
			float thetaStep = 360 / lines[i];
			if (i == 0) {
				quadsZero = new GameObject[lines[i]];
				for (int j = 0; j < lines[i]; j++) quadsZero[j] = MakeQuad(r, j*thetaStep, 90, scaleX, scaleY, c1, c2, false, null/*texZ[j]*/);
			} else {
				quadsUp[i-1] = new GameObject[lines[i]];
				quadsDown[i-1] = new GameObject[lines[i]];
				for (int j = 0; j < lines[i]; j++) {
					quadsUp[i-1][j] = MakeQuad(r, j*thetaStep, 90-i*phiStep, scaleX, scaleY, c1, c2, false, null/*texU[i-1][j]*/);
					quadsDown[i-1][j] = MakeQuad(r, j*thetaStep, 90+i*phiStep, scaleX, scaleY, c1, c2, false, null/*texD[i-1][j]*/);
				}
			}
		}
	}

	GameObject MakeQuad(float r, float theta, float phi, float scaleX, float scaleY, Color c1, Color c2, bool drawLine, Texture2D tex) {
		float x = r * Mathf.Cos(theta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
		float y = r * Mathf.Sin(theta * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad);
		float z = r * Mathf.Cos(phi * Mathf.Deg2Rad);

		var o = GameObject.CreatePrimitive(PrimitiveType.Quad);
		o.transform.SetParent(this.transform);
		o.GetComponent<Renderer>().material = new Material(Shader.Find("Self-Illumin/Diffuse"));
		if (tex == null) o.GetComponent<Renderer>().material.color = c1;
		else {
			o.GetComponent<Renderer>().material.mainTexture = tex; //Resources.Load("MainMenu/" + tex) as Texture2D;
			o.GetComponent<Renderer>().material.color = new Color(0.8F, 0.8F, 0.8F);
		}

		var b = GameObject.CreatePrimitive(PrimitiveType.Quad); b.name = "Back";
		b.transform.SetParent(o.transform);
		b.transform.localRotation = Quaternion.Euler(0, 180, 0);
		b.GetComponent<Renderer>().material = new Material(Shader.Find("Self-Illumin/Diffuse"));
		if (tex == null) b.GetComponent<Renderer>().material.color = c2;
		else {
			b.GetComponent<Renderer>().material.mainTexture = tex; //Resources.Load("MainMenu/" + tex) as Texture2D;
			b.GetComponent<Renderer>().material.mainTextureScale = new Vector2(-1, 1);
			b.GetComponent<Renderer>().material.color = new Color(0.2F, 0.2F, 0.2F);
		}

		o.transform.localPosition = new Vector3(x, y, -z);
		o.transform.localScale = new Vector3(scaleX, scaleY, 1);

		o.transform.Rotate(new Vector3(0, -theta, 0));
		o.transform.Rotate(new Vector3(90 - phi, 0, 0));

		if (drawLine) {
			var l = new GameObject("Line");
			l.transform.SetParent(this.transform);
			l.transform.localRotation = Quaternion.Euler(Vector3.zero);
			l.AddComponent<LineRenderer>(); var lr = l.GetComponent<LineRenderer>();
			lr.material = new Material(Shader.Find("Self-Illumin/Diffuse"));
			lr.useWorldSpace = false;
			lr.SetPosition(0, Vector3.zero);
			lr.SetPosition(1, new Vector3(x, y, -z));
			lr.SetWidth(0.005F, 0.005F);
		}

		return o;
	}

	void Update() {
		if (animate) {
			for (int i = 0; i < quadsZero.Length; i++)
				quadsZero[i].transform.RotateAround(Vector3.zero, -Vector3.up, speed * Time.deltaTime);
			for (int i = 0; i < quadsUp.Length; i++)
				for (int j = 0; j < quadsUp[i].Length; j++) {
					quadsUp[i][j].transform.RotateAround(Vector3.zero, -Vector3.up, (!oneWay && i % 2 == 0) ? -speed * Time.deltaTime : speed * Time.deltaTime);
					quadsDown[i][j].transform.RotateAround(Vector3.zero, -Vector3.up, (!oneWay && i % 2 == 0) ? -speed * Time.deltaTime : speed * Time.deltaTime);
				}
		}

		//rotating by 10 degrees at a time to make screenshots for loading icon
		if (Input.GetKeyDown(KeyCode.RightBracket)) {
			for (int i = 0; i < quadsZero.Length; i++)
				quadsZero[i].transform.RotateAround(Vector3.zero, -Vector3.up, 10);
			for (int i = 0; i < quadsUp.Length; i++)
				for (int j = 0; j < quadsUp[i].Length; j++) {
					quadsUp[i][j].transform.RotateAround(Vector3.zero, -Vector3.up, (i % 2 == 0) ? -10 : 10);
					quadsDown[i][j].transform.RotateAround(Vector3.zero, -Vector3.up, (i % 2 == 0) ? -10 : 10);
				}
		}
	}
}
