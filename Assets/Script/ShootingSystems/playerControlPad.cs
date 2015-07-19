using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class playerControlPad : MonoBehaviour {

	//basic var
	public static int currTouch = 0;
	public GameObject cameraMain;
	public GameObject testPlayer;

	//player control rect
	//public float playerRectYR;
	public int[] prTouchCount;
	private int fingerCount;
	private Rect playerRect;

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

	private Vector3 playerLoc = new Vector3();
	private Vector3 currentPos = new Vector3();
	public Rigidbody r;

	private float x, y;
	
	public float trackBackLength = 0;

	//player status
	public int healthPoint = 5;
	public Text healthText;
	public Image healthImg;
	public float playerSpeed;


	void Start () {
		//x = 0f;
		//y = 0f;

		//PlayerRectChange(13f);
		playerRect = new Rect (Screen.width*0.05f, Screen.height* 0.05f, Screen.width * 0.4f, Screen.height* 0.8f);
		//Debug.Log ( playerRect);
		prTouchCount = new int[3];
		playerSpeed = 0.0001f;
		bulletCount = 12;
		InvokeRepeating ( "BulletGainWithTime", bulletSpawnTime , bulletSpawnTime);
		//StartCoroutine(getBullet(2f));
		//r = gameObject.GetComponent<Rigidbody>();
	}
	public void PauseGame(){
		CancelInvoke ("BulletGainWithTime");
	}

	public void StartGame(){
		InvokeRepeating ( "BulletGainWithTime", bulletSpawnTime , bulletSpawnTime);
	}
	
	void Update () {
		
		//move in cases
		//shootSpawnTime = spawnTime;
		r.velocity = Vector3.zero;
		if (Input.touchCount>0){	

			prTouchCount = new int[3];

			for(int i = 0; i < Input.touchCount ; i++) {
				if (playerRect.Contains(Input.GetTouch(i).position)){

				// count array
					prTouchCount[i] = 1;
				}else{
					prTouchCount[i] = 0;
				}

				fingerCount = prTouchCount[0] + prTouchCount[1] + prTouchCount[2];
				Debug.Log (fingerCount);
				if(playerRect.Contains(Input.GetTouch(i).position) && fingerCount >= 1) {	
					
					playerLoc = testPlayer.transform.position;
					currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(i).position);	
					if(Input.GetTouch(i).phase == TouchPhase.Began && Vector3.Distance(playerLoc, currentPos)<11f){
						InvokeRepeating("MoveByDrag",0f,0.01f);
					}else if (Input.GetTouch(i).phase == TouchPhase.Ended){
						CancelInvoke("MoveByDrag");
					}
				}

			}

		}
		shootSpawnTime = shootSpawnTime - 1f*Time.deltaTime;
		if (isShooting == true && shootSpawnTime <= 0 && bulletCount >= 1){
			Rigidbody instantiateBullet = Instantiate (bullet, transform.position, transform.rotation) as Rigidbody;
			instantiateBullet.velocity = transform.InverseTransformDirection (new Vector3(bulletSpeed,0,0));
			//0.7.4shootAudio.Play();
			bulletCount --;
			BulletCount(bulletCount);
			shootSpawnTime = spawnTime;
		} 

	}

	public void IsShooting(bool a){
		isShooting = a;
	}

	void BulletCount(float b){
		float bulletX = 390 - b*32f ;
		showBullet.rectTransform.offsetMin = new Vector2 (bulletX, 18.5f);
	}
	
	void MoveByDrag(){
		for (int n = 0; n< Input.touchCount; n++){
			if(playerRect.Contains(Input.GetTouch(n).position)){	
				playerLoc = testPlayer.transform.position;
				currentPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(n).position);
				x = Mathf.Round (currentPos.x / cell_size) * cell_size;
				y = Mathf.Round (currentPos.y / cell_size) * cell_size;
				Vector3 translate = new Vector3 (x,y,0) - playerLoc;
				r.AddForce(translate*playerSpeed*Time.deltaTime*10f);
				break;
			}
		}

	}


	void OnCollisionEnter(Collision c){
		if (c.transform.tag == "CauseDie") {
			//if(Math.Abs ( c.transform.position.y - transform.position.y) <= cell_size){
				int dam = c.gameObject.GetComponent<BlockAttribute>().takeDamagePoint;
				healthPoint = healthPoint - dam;
				//Debug.Log ("health point is " + healthPoint);
				if (healthPoint == 0 ){
				cameraMain.GetComponent <TrackBackScript>().BackTrack();
				//healthPoint = cameraMain.GetComponent <editorControl>().loadHealthPoint;
				//backPoint = cameraMain.transform.position.x - trackBackLength;
				//cameraMain.transform.position= new Vector3(backPoint,1,cameraMain.transform.localPosition.y -10);
				}
			//}
		}

		}

	void OnTriggerEnter(Collider c){
		if (c.transform.tag == "SlowDown") {
				playerSpeed = 0.00002f;
		}
		if (c.transform.tag == "SaveCheckPoint"){
			cameraMain.GetComponent<TrackBackScript>().SaveCheckPoint();
		}
		if (c.GetComponent<BlockAttribute>().CanCollect == true){
			c.GetComponent<AudioSource>().Play ();
			Destroy(c.gameObject);
		}

	}

	void OnTriggerExit(Collider c){
		if (c.transform.tag == "SlowDown") {
				playerSpeed = 0.0001f;
		}
	}

//	IEnumerator getBullet(float waittime){
//		Debug.Log ("the IEnumerator getBullet is loaded");
//		if (bulletCount <= 11){
//			bulletCount ++;
//			BulletCount(bulletCount);
//			print ("it should gain the bullet count to" + bulletCount);
//		}
//		yield return new WaitForSeconds(2);
//		Debug.Log ("after the yield, the bulletcount is " + bulletCount);
//	}

	void BulletGainWithTime (){
		if (bulletCount <= 11){
			bulletCount ++;
			BulletCount(bulletCount);
			print ("it should gain the bullet count to" + bulletCount);
		}
	}
}