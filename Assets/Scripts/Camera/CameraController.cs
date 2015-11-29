using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	bool isPanning;
	int panDirection;
	float degreesPanned;

	enum Direction {
		RIGHT = 1,
		LEFT = -1
	};

	// Use this for initialization
	void Start () {
		isPanning = false;
	}
	
	// Update is called once per frame
	void Update () {
		switch(isPanning){
		case true:
			PanCamera(panDirection);
			break;
		case false:
			if(Input.GetButton("Camera Pan L")) {
				panDirection = (int) Direction.LEFT;
				PanCamera(panDirection);
			}
			if(Input.GetButton("Camera Pan R")) {
				panDirection = (int) Direction.RIGHT;
				PanCamera(panDirection);
			}
			break;
		default:
			break;
		}	
	}

	void PanCamera(int direction) {
		float degreesToPan = 20 * Time.deltaTime;

		if ((degreesPanned + degreesToPan) < 45f) {
			transform.RotateAround (Vector3.zero, Vector3.up, degreesToPan * direction);
			degreesPanned += degreesToPan;
			isPanning = true;
		} else {
			transform.RotateAround (Vector3.zero, Vector3.up, (45f - degreesPanned) * direction);
			degreesPanned = 0;
			isPanning = false;
		}
	}
}
