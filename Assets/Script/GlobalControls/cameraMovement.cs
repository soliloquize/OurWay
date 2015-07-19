using UnityEngine;
using System.Collections;

public class cameraMovement : MonoBehaviour {

	public int gridSize = 1;
	public float startTime =1;
	public float speed = 1f;

	public void Move () {
		Vector3 scroll = new Vector3 (transform.position.x + gridSize, 1, -10);
		transform.position = scroll;
	}

	public void Start () {
		InvokeRepeating ("Move", startTime, speed);
	}

	public void SpeedUp(){
		if (speed > 0.4) {
					speed = speed - 0.2f;
				}
		}

	public void SpeedDown(){
		if (speed < 1.6f) {
					speed = speed + 0.2f;
				}
	}

}