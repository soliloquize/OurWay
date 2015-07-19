using UnityEngine;
using System.Collections;

public class StartShootOnly : MonoBehaviour {

	public GameObject mainCamera;
	public GameObject minimapCam;

	public void StartGame(){
		mainCamera.GetComponent<Camera>().rect = new Rect(0,0,1,0.92f);
		mainCamera.GetComponent<editorControl>().RunGame();
		minimapCam.SetActive(true);
		Destroy(gameObject);
	}

}
