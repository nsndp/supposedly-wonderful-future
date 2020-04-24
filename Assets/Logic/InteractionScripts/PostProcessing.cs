using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PostProcessing : MonoBehaviour {

	[Range(0, 2)] public float Saturation = 1;
	[Range(0, 2)] public float Multiply = 1;
	[Range(0, 1)] public float Add = 0;
	[Range(0, 1)] public float Contrast = 0;
	public Shader pps;
	Material mat;

	void Start() {
		if (!SystemInfo.supportsImageEffects) { enabled = false; return; }
	}

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		if (!enabled) return;

		var M = new Matrix4x4();
		var axisX = new Vector3(1, 0, 0);
		var axisY = new Vector3(0, 1, 0);
		var axisZ = new Vector3(0, 0, 1);
		var offset = Vector3.zero;

		if (Saturation != 1) {
			float satshift = 1.0f - Saturation;
			axisX = (axisX * Saturation) + new Vector3(.35f * satshift, .35f * satshift, .35f * satshift);
			axisY = (axisY * Saturation) + new Vector3(.5f * satshift, .5f * satshift, .5f * satshift);
			axisZ = (axisZ * Saturation) + new Vector3(.15f * satshift, .15f * satshift, .15f * satshift);
		}

		float cca = Add;
		float ccm = Multiply;
		cca -= Contrast * .5f;
		ccm += Contrast * .5f;

		axisX *= ccm;
		axisY *= ccm;
		axisZ *= ccm;

		offset.x += cca;
		offset.y += cca;
		offset.z += cca;

		M.SetColumn(0, axisX);
		M.SetColumn(1, axisY);
		M.SetColumn(2, axisZ);
		M.SetColumn(3, offset);

		mat = new Material(pps);
		mat.SetMatrix("_Matrix", M);
		Graphics.Blit(src, dst, mat);
	}

	void Update() {
	}
}
