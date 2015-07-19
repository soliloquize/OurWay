using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReadyToStart : MonoBehaviour {
	
	public bool BPIsReady = false;
	public bool TPIsReady = false;

	public void BPReady(){
		BPIsReady = true;
	}

	public void TPReady(){
		TPIsReady = true;
	}


	public GameObject CountDown;
	public GameObject readyPanel;

	void Start(){
		Time.timeScale = 0;
	}

	void Update(){
		if (BPIsReady && TPIsReady){
			//CountDown.SetActive(true);
			//readyPanel.SetActive(true);
			IsReadyToStart();
			BPIsReady = false;
			TPIsReady = false;
		}
	}
	
	public GameObject player;
	public GameObject bgmid;
	public GameObject pauseButton;
	public GameObject editorCanvas;
	public GameObject areaControlText;
	public GameObject controlHandle;
	public GameObject mainCamera;
	public GameObject playerStatus;
	public GameObject minimapCam;

	void IsReadyToStart(){
		Time.timeScale = 1;

		GameObject.Find ("Setting_tp").SetActive(false);
		GameObject.Find ("Setting_bp").SetActive(false);

		mainCamera.GetComponent<Camera>().rect = new Rect(0,0,1,0.92f);
		minimapCam.SetActive(true);

		pauseButton.SetActive(true);
		editorCanvas.SetActive(true);
		player.SetActive(true);
		bgmid.SetActive(true);
		areaControlText.SetActive(false);
		controlHandle.SetActive(false);
		mainCamera.GetComponent<editorControl>().enabled = true;
		mainCamera.GetComponent<editorControl>().RunGame();
		//CountDown.SetActive(false);
		readyPanel.SetActive(false);
		playerStatus.SetActive(true);


		
	}



}
