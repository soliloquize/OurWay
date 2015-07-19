using UnityEngine;
using System.Collections;

public class DifChange : MonoBehaviour {
	//private int score = 0;
	private GameObject stageLine;

	public int phaseChange = 0;
	public int currentphase = 0;

	//public GameObject mainCam;
	//public bool changePhase = false;
	public GameObject shooterReadyButton;
	public GameObject builderReadyButton;

	private bool shooterIsReady = false;
	private bool builderIsReady = false;

	public int builderSS = 10;
	public int ShooterSS = 20;

	public bool bSS = false;
	public bool sSS = false;
	private bool bothReady;

	public void gainScore(int scr){
		//score = score + scr;
	}

	public void ShooterReady(){
		shooterIsReady = true;
	}

	public void BuilderReady(){
		builderIsReady = true;
	}


	public void CallReadyButtons(){
		shooterReadyButton.SetActive(true);
		builderReadyButton.SetActive(true);

	}

	void Update(){
		if (bSS && sSS){
			CallReadyButtons();
			//Coroutine (CanChangePhase(10));
			bSS = false;
			sSS = false;
		}

		if (shooterIsReady == true && builderIsReady == true){
			phaseChange ++;

			switch (phaseChange)
			{
			case 1 :
				ChangeToPhase2();
				//phaseChange = 0;
				break;
				
			case 2:
				ChangeToPhase3();
				//phaseChange = 0;
				break;
				
			case 3 :
				ChangeToPhase4();
				//phaseChange = 0;
				break;
				
			case 4:
				LevelEnding();
				//phaseChange = 0;
				break;
				
				
			}


			Debug.Log (PlayerPrefs.GetInt("currentVersion"));
			if (PlayerPrefs.GetInt("currentVersion") == 2){
				GetComponent<editorControl>().AddDifLine(phaseChange);
			}
			shooterIsReady = false;
			builderIsReady = false;

			if(PlayerPrefs.GetInt("currentVersion") == 2) {
			shooterReadyButton.SetActive(false);
				builderReadyButton.SetActive(false);
			}

		}



	}

	public IEnumerator CanChangePhase(float waittime){
		CallReadyButtons();
		yield return new WaitForSeconds(waittime);
	}

	void ChangeToPhase2(){
		GameObject shootPad;
		shootPad = GameObject.Find("Canvas_PlayerStatus/ShootControl");
		shootPad.GetComponent<RectTransform>().anchoredPosition = new  Vector3 (82f,82f,0);

		GameObject bulletCount;
		bulletCount = GameObject.Find("Canvas_PlayerStatus/Shoot_L");
		bulletCount.GetComponent<RectTransform>().anchoredPosition = new  Vector3 (339f,47f, 0);

		if(PlayerPrefs.GetInt("currentVersion") == 2) {
		GameObject spike;
		spike = GameObject.Find("Canvas_EditorControl/ToggleGroup/Choose_BlockB_toggle");
		spike.GetComponent<RectTransform>().anchoredPosition = new Vector3 (-214f, 45f, 0);
		}

		builderSS = 120;
		ShooterSS = 50;
	}

	void ChangeToPhase3(){

		if(PlayerPrefs.GetInt("currentVersion") == 2) {
		GameObject addGhostToggle;
		addGhostToggle = GameObject.Find("Canvas_EditorControl/ToggleGroup/Choose_BlockC_toggle");
		addGhostToggle.GetComponent<RectTransform>().anchoredPosition = new Vector3 (-300f, 45f, 0);
		}

		builderSS = 250;
		ShooterSS = 100;

	}

	void ChangeToPhase4(){
		GameObject demon;
		demon = GameObject.FindWithTag("Player");
		demon.GetComponent<playerControl>().playerSpeed = 650f;

		if(PlayerPrefs.GetInt("currentVersion") == 2) {
		GameObject mainCamera;
		mainCamera = GameObject.Find ("Main Camera");
		mainCamera.GetComponent<editorControl>().pointGainRate = 2;
		mainCamera.GetComponent<editorControl>().WaitToGainPoints(1f);

		}

		builderSS = 380;
		ShooterSS = 140;
	}

	void LevelEnding(){

		GameObject demon;
		demon = GameObject.FindWithTag("Player");
		demon.GetComponent<playerControl>().playerSpeed = 950f;

		if(PlayerPrefs.GetInt("currentVersion") == 2) {
		GameObject mainCamera;
		mainCamera = GameObject.Find ("Main Camera");
		mainCamera.GetComponent<editorControl>().pointGainRate = 4;
		mainCamera.GetComponent<editorControl>().WaitToGainPoints(1f);
		}

		builderSS = 9999;
		ShooterSS = 9999;
		
//		stageLine = (GameObject)Resources.Load("PhaseChangeLine", typeof(GameObject));
//		GameObject line = Instantiate(stageLine, new Vector3(this.transform.position.x + 3500f, -38f,0), Quaternion.identity) as GameObject;
//		line.name = "EndLine";
//		line.tag = "Ending";
	}

	public GameObject endingScr;
	public GameObject editorC;
	public void BuildPlayToTheEnd(){
		GameObject editorControlC, playerC, pauseC, mapCam, mainCam;
		//editorControlC = GameObject.Find("Canvas_EditorControl");
		playerC = GameObject.Find ("Canvas_PlayerStatus");
		pauseC = GameObject.Find ("Canvas_Pause");
		mapCam = GameObject.Find ("MapCamera");


		GetComponent<editorControl>().enabled = false;
		playerC.SetActive(false);
		pauseC.SetActive (false);
		mapCam.GetComponent<Camera>().rect = new Rect();
		GetComponent<Camera>().rect = new Rect(0,0,1,1);

		endingScr.SetActive(true);
	}

	public void ShootOnlyToTheEnd(){

	}
}
