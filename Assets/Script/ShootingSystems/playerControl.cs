using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class playerControl : MonoBehaviour {

	//basic var
	public static int currTouch = 0;
	public GameObject cameraMain;
	public GameObject testPlayer;

	//player control rect
	//public float playerRectYR;
	public int[] prTouchCount;
	private int fingerCount;
	//private Rect playerRect;

	private ControllerColliderHit playerHit;

	//bullet and shooting function
	public Rigidbody bullet;

	public Image showBullet;
	public int bulletCount;
	public float bulletSpeed = 20;
	public bool isShooting;
	public float spawnTime;
	public float shootSpawnTime;
	public float bulletSpawnTime;
	public AudioSource shootAudio;

	public float cell_size = 3;

	//character movement by dragging
	//private Vector3 playerLoc = new Vector3();
	//private Vector3 currentPos = new Vector3();

	public Rigidbody r;

	private float x, y;
	private int moveDir;
	private int shotDir;
	private bool isMoving;
	
	public float trackBackLength = 0;

	//player status
	public int healthPoint = 5;
	public Text healthText;
	public Text scoreText;
	public Image showHealth;
	public float playerSpeed;
	public int playerScore = 0;
	//private Vector3 bulletDir;

	private Animator playerAnim;
	private float reHurtTime = 0.5f;
	private bool canHurt = true;
	bool canSaveCheckPoint = true;

	void Start () {
		//x = 0f;
		//y = 0f;
	
		//PlayerRectChange(13f);
		//playerRect = new Rect (Screen.width*0.05f, Screen.height* 0.05f, Screen.width * 0.4f, Screen.height* 0.8f);
		//Debug.Log ( playerRect);
		prTouchCount = new int[3];
		playerAnim = GetComponentInChildren<Animator>();
		//playerSpeed = 0.0001f;
		bulletCount = 12;
		InvokeRepeating ( "BulletGainWithTime", bulletSpawnTime , bulletSpawnTime);
		//StartCoroutine(getBullet(2f));
		//r = gameObject.GetComponent<Rigidbody>();
		//bulletDir = new Vector3 (0, bulletSpeed, 0);
	}
	public void PauseGame(){
		CancelInvoke ("BulletGainWithTime");
	}

	public void StartGame(){
		InvokeRepeating ( "BulletGainWithTime", bulletSpawnTime , bulletSpawnTime);
	}

	void Update () {

		Vector3 currentSpeed= r.velocity;
		Vector3 addToVelocity = new Vector3();
		Vector3 targetSpeed = new Vector3();

		if (isMoving == true){



			switch(moveDir)
			{
				case 1 :
					addToVelocity = new Vector3(0,-playerSpeed,0) * Time.deltaTime;
					targetSpeed = addToVelocity + currentSpeed;
					
					targetSpeed = new Vector3(
					Mathf.Clamp(targetSpeed.x, -playerSpeed, playerSpeed),
					Mathf.Clamp(targetSpeed.y, -playerSpeed, playerSpeed),
						0
						);		
					r.velocity = targetSpeed;
				break;

				case 2 :
				addToVelocity = new Vector3(0,playerSpeed,0) * Time.deltaTime;
				targetSpeed = addToVelocity + currentSpeed;
				targetSpeed = new Vector3(
					Mathf.Clamp(targetSpeed.x, -playerSpeed, playerSpeed),
					Mathf.Clamp(targetSpeed.y, -playerSpeed, playerSpeed),
					0
					);				
				r.velocity = targetSpeed;
				break;

				case 3 :
				addToVelocity = new Vector3(playerSpeed,0,0) * Time.deltaTime;
				targetSpeed = addToVelocity + currentSpeed;
				targetSpeed = new Vector3(
					Mathf.Clamp(targetSpeed.x, -playerSpeed, playerSpeed),
					Mathf.Clamp(targetSpeed.y, -playerSpeed, playerSpeed),
					0
					);				
				r.velocity = targetSpeed;
				break;

				case 4 :
				addToVelocity = new Vector3(-playerSpeed,0,0) * Time.deltaTime;
				targetSpeed = addToVelocity + currentSpeed;
				targetSpeed = new Vector3(
					Mathf.Clamp(targetSpeed.x, -playerSpeed, playerSpeed),
					Mathf.Clamp(targetSpeed.y, -playerSpeed, playerSpeed),
					0
					);				
				r.velocity = targetSpeed;
				break;

				case 0:
				r.velocity = Vector3.zero;
				isMoving = false;
				break;
			}

		}

		shootSpawnTime = shootSpawnTime - 1f*Time.deltaTime;

		if (isShooting == true && shootSpawnTime <= 0 && bulletCount >= 1){
			Rigidbody instantiateBullet = Instantiate (bullet, transform.position, transform.rotation) as Rigidbody;
			//instantiateBullet.velocity = transform.InverseTransformDirection (new Vector3(bulletSpeed,0,0));

			switch(shotDir)
			{
			case 1 :
				instantiateBullet.velocity = transform.InverseTransformDirection(new Vector3(0,0,bulletSpeed) * Time.deltaTime);
				playerAnim.SetTrigger("ShootingRt");
				//playerAnim.SetInteger("shotDirAnim",1);
				break;
			case 2 :
				instantiateBullet.velocity = transform.InverseTransformDirection(new Vector3(0,0,-bulletSpeed) * Time.deltaTime);
				playerAnim.SetTrigger("ShootingLf");
				//playerAnim.SetInteger("shotDirAnim",2);
				break;
			case 3 :
				instantiateBullet.velocity = transform.InverseTransformDirection(new Vector3(bulletSpeed,0,0) * Time.deltaTime);
				playerAnim.SetTrigger("ShootingUp");
				//playerAnim.SetInteger("shotDirAnim",3);
				break;
			case 4 :
				instantiateBullet.velocity = transform.InverseTransformDirection(new Vector3(-bulletSpeed,0,0) * Time.deltaTime);
				playerAnim.SetTrigger("ShootingDn");
				//playerAnim.SetInteger("shotDirAnim",4);
				break;
			}
		
			shootAudio.Play();
			bulletCount --;
			BulletCount(bulletCount);
			shootSpawnTime = spawnTime;

		} 

		healthText.text = "shooter health :" + healthPoint.ToString();
		scoreText.text = "score" + ":   " + playerScore.ToString();

		int sSS = cameraMain.GetComponent<DifChange>().ShooterSS;
		if (playerScore >= sSS){
			cameraMain.GetComponent<DifChange>().sSS = true;
		}
	}
	
	public void IsShooting(bool a){
		isShooting = a;
	}



	public void IsMovingTo (int d){
		if (d != 0) {
			isMoving = true;
			r.velocity = Vector3.zero; 
			playerAnim.SetInteger("moveDirAnim", d);
		}
		moveDir = d;
	}

	public void IsShootingTo (int sd){
		isShooting = true;
		shotDir = sd;

	}

	public void ShootSpeed(float sp){
		bulletSpeed = sp*10000;
	}

	
	//show bullet and health count on image
	void BulletCount(float b){
		float bulletX = 1 - b*32f ;
		showBullet.rectTransform.offsetMin = new Vector2 (bulletX, -33f);
	}
	void HealthCount(float h){
		float healthX = 1250 - (10f-healthPoint) * 127;
		showHealth.rectTransform.offsetMax = new Vector2 (healthX, 122);
	}


	void OnCollisionEnter(Collision c){
		if (c.transform.tag == "CauseDie") {
			if(canHurt == true){
				int dam = 1 ;
				healthPoint = healthPoint - dam;

				playerAnim.SetTrigger("getHitAnim");
				StartCoroutine(getHurt(reHurtTime));
				//Debug.Log ("health point is " + healthPoint);
				if (healthPoint == 0 ){
				cameraMain.GetComponent <TrackBackScript>().BackTrack();
				//healthPoint = cameraMain.GetComponent <editorControl>().loadHealthPoint;
				//backPoint = cameraMain.transform.position.x - trackBackLength;
				//cameraMain.transform.position= new Vector3(backPoint,1,cameraMain.transform.localPosition.y -10);
				}
				HealthCount(healthPoint);
			}
			//}
		}

		}

	void OnTriggerEnter(Collider c){
		if (c.transform.tag == "SaveCheckPoint" || c.transform.tag == "ChangePhase"){
			if (canSaveCheckPoint == true){
				cameraMain.GetComponent<TrackBackScript>().SaveCheckPoint();
				StartCoroutine(CheckPointSaved());
				c.gameObject.renderer.material.color = Color.grey;
			}
			if (c.transform.tag == "ChangePhase" && PlayerPrefs.GetInt("currentVersion") == 0){
				cameraMain.GetComponent<DifChange>().ShooterReady();
				cameraMain.GetComponent<DifChange>().BuilderReady();
			}
			
		}
		if (c.transform.tag == "EnemyBullet") {
			if (canHurt == true){
				int dam = 1 ;
				healthPoint = healthPoint - dam;
				HealthCount(healthPoint);
				StartCoroutine(getHurt(0.1f));
				playerAnim.SetTrigger("getHitAnim");
				if (bulletCount <= 11){
					bulletCount ++;
					BulletCount(bulletCount);
				}
				if (healthPoint == 0 ){
					cameraMain.GetComponent <TrackBackScript>().BackTrack();

					Debug.Log("the health after track back is " + healthPoint);
					HealthCount(healthPoint);
				}
			}
//			if (healthPoint == 0 ){
//				cameraMain.GetComponent <editorControl>().BackTrack();
//			}
		}

		if (c.transform.tag == "gainHealth"){
			int h = 1;
			if (healthPoint < 10){
				healthPoint = healthPoint + h;
				HealthCount(healthPoint);
			} else {
				playerScore = playerScore + 2;
			}
			Destroy(c.transform.gameObject);
			cameraMain.GetComponent<TrackBackScript>().destroyedEnemyVectors.Add(c.transform.position);
		}

		if (c.transform.tag == "Ending"){
			Time.timeScale = 0;
			//if(PlayerPrefs.GetInt("currentVersion") == 2){
				cameraMain.GetComponent<DifChange>().BuildPlayToTheEnd();
			//	return;
			//}else if(PlayerPrefs.GetInt("currentVersion") == 0) {
			//	cameraMain.GetComponent<DifChange>().ShootOnlyToTheEnd();
			//	return;
			//}
		}

	}

	public void GainScore(int scr){
		playerScore = playerScore + scr;
	}
	public GameObject endingScr;

	void OnTriggerExit(Collider c){
		if (c.transform.tag == "SlowDown") {
				playerSpeed = 0.0001f;
		}
	}

	IEnumerator getHurt(float f){
		canHurt = false;
		yield return new WaitForSeconds(reHurtTime);
		canHurt = true;
	}


	IEnumerator CheckPointSaved(){
		canSaveCheckPoint = false;
		yield return new WaitForSeconds(5f);
		canSaveCheckPoint = true;
	}


	void BulletGainWithTime (){
		if (bulletCount <= 11){
			bulletCount ++;
			BulletCount(bulletCount);
			//print ("it should gain the bullet count to" + bulletCount);
		}
	}
}