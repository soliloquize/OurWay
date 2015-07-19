using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cameraMovementText : MonoBehaviour {

	public Text cameraSpeedText;
	public Camera mainCamera;
	void Start () {
		cameraSpeedText = GetComponent<Text> ();

	}
	
	void Update(){
		float speedText = 2f - mainCamera.GetComponent<editorControl> ().speed;
		cameraSpeedText.text = /*"Current Level Speed : " + */speedText.ToString("f2");
	}

}
