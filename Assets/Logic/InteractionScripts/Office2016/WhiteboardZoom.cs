using UnityEngine;
using System.Collections;

public class WhiteboardZoom : MonoBehaviour {

	public MeshFilter NoHolesCount;
	public GameObject EraserAndRedMarker;
	DataControlPrologue DC; GameObject colBoard, colScrew;
	Vector3 DstPos = new Vector3(0.69F, 1.336F, 1.366F); Vector3 DstRotEuler = new Vector3(0, 60, 0);
	Vector3 SrcPos; Quaternion SrcRot, DstRot;
	float tc; public int phase = -1;
	double secondCounter = 0; int frameCounter = 1;

	public void Init() {
		DC = GameObject.Find("Data").GetComponent<DataControlPrologue>();
		colBoard = DC.currentColliders.transform.Find("Whiteboard").gameObject;
		colScrew = DC.currentColliders.transform.Find("ScrewGuide").gameObject;
		SrcPos = DC.camPosM; SrcRot = DC.camRotM;
		DstRot = Quaternion.Euler(DstRotEuler);
		var uvs = new Vector2[4];
		if (!DC.S.SP.noHolesCountReset) {
			uvs[0] = new Vector2(0, 2/3F);
			uvs[1] = new Vector2(1/7F, 1);
			uvs[2] = new Vector2(1/7F, 2/3F);
			uvs[3] = new Vector2(0, 1);
		} else {
			uvs[0] = new Vector2(6/7F, 0);
			uvs[1] = new Vector2(1, 1/3F);
			uvs[2] = new Vector2(1, 0);
			uvs[3] = new Vector2(6/7F, 1/3F);
		}
		NoHolesCount.mesh.uv = uvs;
	}
	
	void Update() {
		if (phase == 0) {
			DC.CursorLock(true); DC.UIC.Col(false); DC.bMenu.SetActive(false);
			tc = 0; phase = 1;
		}
		else if (phase == 1 && tc <= 1) {
			tc += 0.006666F * Time.deltaTime * 60;
			DC.cam.transform.position = Vector3.Lerp(SrcPos, DstPos, Mathf.SmoothStep(0, 1, tc));
			DC.cam.transform.rotation = Quaternion.Lerp(SrcRot, DstRot, Mathf.SmoothStep(0, 1, tc));
		}
		else if (phase == 1 && tc > 1) {
			if (DC.S.SP.wallPierced && !DC.S.SP.noHolesCountReset) {
				phase = 2; EraserAndRedMarker.SetActive(false);
				DC.Sound.clip = DC.board; DC.Sound.Play();
			} else {
				DC.CursorLock(false); DC.UIC.Col(true); DC.bReturn.SetActive(true);
				colBoard.SetActive(false); colScrew.SetActive(true);
				phase = 3;
			}
		}
		else if (phase == 2) { //no holes count animation
			secondCounter += Time.deltaTime;
			if (secondCounter >= 0.1) { //1 second has passed
				var mesh = NoHolesCount.mesh;
				var uvs = new Vector2[4];
				if (frameCounter != 7 && frameCounter != 14) {
					uvs[0] = new Vector2(mesh.uv[0].x + 1/7F, mesh.uv[0].y);
					uvs[1] = new Vector2(mesh.uv[1].x + 1/7F, mesh.uv[1].y);
					uvs[2] = new Vector2(mesh.uv[2].x + 1/7F, mesh.uv[2].y);
					uvs[3] = new Vector2(mesh.uv[3].x + 1/7F, mesh.uv[3].y);
				} else {
					uvs[0] = new Vector2(0, mesh.uv[0].y - 1/3F);
					uvs[1] = new Vector2(1/7F, mesh.uv[1].y - 1/3F);
					uvs[2] = new Vector2(1/7F, mesh.uv[2].y - 1/3F);
					uvs[3] = new Vector2(0, mesh.uv[3].y - 1/3F);
				}
				mesh.uv = uvs;
				frameCounter++;
				if (frameCounter == 21) {
					DC.S.SP.noHolesCountReset = true; EraserAndRedMarker.SetActive(true);
					DC.CursorLock(false); DC.UIC.Col(true); DC.bReturn.SetActive(true);
					colBoard.SetActive(false); colScrew.SetActive(true);
					phase = 3;
				}
				secondCounter = 0;
			}
		}
		else if (phase == 3 && DC.bReturn.activeSelf && (DC.bReturn.GetComponent<ButtonArrow>().clicked || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))) {
			DC.CursorLock(true); DC.UIC.Col(false);
			DC.bReturn.SetActive(false); DC.bReturn.GetComponent<ButtonArrow>().clicked = false;
			tc = 0; phase = 4;
		}
		else if (phase == 4 && tc <= 1) {
			tc += 0.006666F * Time.deltaTime * 60;
			DC.cam.transform.position = Vector3.Lerp(DstPos, SrcPos, Mathf.SmoothStep(0, 1, tc));
			DC.cam.transform.rotation = Quaternion.Lerp(DstRot, SrcRot, Mathf.SmoothStep(0, 1, tc));
		}
		else if (phase == 4 && tc > 1) {
			DC.CursorLock(false); DC.UIC.Col(true); DC.bMenu.SetActive(true);
			colBoard.SetActive(true); colScrew.SetActive(false);
			phase = -1;
		}
	}
}
