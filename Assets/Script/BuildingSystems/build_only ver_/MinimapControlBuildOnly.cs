using UnityEngine;
using System.Collections;

public class MinimapControlBuildOnly : MonoBehaviour {

	public bool canMoveCamera = true;

	GameObject mainCamera;
	Rect minimapRect ;

	void Start(){
		minimapRect = new Rect (0, Screen.height*0.92f, Screen.width * 0.8f, Screen.height*0.08f);
		mainCamera = GameObject.Find("Main Camera");
	}
	public void IsChangingCanMoveCamera(){
		canMoveCamera = !canMoveCamera;
	}
	void Update () {
		if(Input.touchCount > 0){
			for(int i = 0; i < Input.touchCount; i++) {
				if (minimapRect.Contains(Input.GetTouch(0).position) && canMoveCamera == true && Input.GetTouch(0).phase == TouchPhase.Moved ){

					Vector3 targetPos = this.camera.ScreenToWorldPoint(Input.GetTouch(0).position);
					float tarX = targetPos.x;
					mainCamera.transform.position = new Vector3( tarX, 0, -10);

				}
			}
		}
	}
}
