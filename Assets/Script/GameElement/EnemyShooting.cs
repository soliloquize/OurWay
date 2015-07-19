using UnityEngine;
using System.Collections;

public class EnemyShooting : MonoBehaviour {

	public GameObject cameraMain;
	public GameObject testPlayer;

	public GameObject bulletResource;
	public float shootRate = 0.5f;
	public float bulletSpeed;
	public float restTime = 1f;
	public float bulletNumber = 3;

	public bool startShoot = true;
	private bool canShoot = false;
	private Vector3 posOnScreen;
	private Vector3 originalPosition;
	private float activePosX;
	private bool inCameraView;
	//private float disToCamera;


//	void Start(){
//		testPlayer = GameObject.FindGameObjectWithTag("Player");
//		cameraMain = GameObject.Find ("Main Camera");
//
//		disToCamera = cameraMain.transform.position.x - transform.position.x; 
//		if (Mathf.Abs (disToCamera) > 35) {
//			inCameraView = false;
//			InvokeRepeating("OutOfView",0.1f, 1f);
//		}else {
//			inCameraView = true;
//			InvokeRepeating("NotShooting", 0.1f, 1f);
//			InvokeRepeating("IsInCameraView", 0.1f, 1f);
//		}
//	}
//
//	void Update(){
//		disToCamera = cameraMain.transform.position.x - transform.position.x;
//		posOnScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position);
//	}
//
//	void StartShooting(){
//		StartCoroutine(Shoot ());
//	}
//
//	void NotShooting(){
//		float x = posOnScreen.x;
//		if (inCameraView == true && x <= Screen.width*0.44f) {
//			StartShooting();
//			CancelInvoke("NotShooting");
//
//		} 
//	}
//
//	void IsInCameraView(){
//		if (Mathf.Abs (disToCamera) >= 30) {
//			inCameraView = false;
//			StopCoroutine(Shoot());
//			InvokeRepeating("OutOfView",1f, 1f);
//			CancelInvoke("NotShooting");
//
//		}
//	}
//
//	void OutOfView(){
//		if (Mathf.Abs (disToCamera) < 32) {
//			inCameraView = true;
//			InvokeRepeating("NotShooting",1f, 1f);
//			InvokeRepeating("IsInCameraView", 1f, 1f);
//			CancelInvoke("OutOfView");
//		}
//	}

	//destroy object outside screen

	void Start(){
		cameraMain = GameObject.Find ("Main Camera");

		bulletSpeed = 13;
		bulletNumber = 2;
	}

	void Update(){

		posOnScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position);		
		Vector3 viewPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
		
		float x = posOnScreen.x;
		
		if (canShoot == false){
			//if(disToCamera < 1175) canShoot = true;
			if(  (viewPos.x >= 0f) && (viewPos.x < 1f) ){
				canShoot = true;
//				Debug.Log ("can shoot is true");
			}
		} 
		else {
			if (startShoot == true && x <= Screen.width*0.44f){
				StartCoroutine("Shoot");
				startShoot = false;
			} 
			
			if(  (viewPos.x <= 0f) || (viewPos.x > 1f) ){
				//print (gameObject.name + "'s distance to camera is over 384 and should been destroyed!");
				cameraMain.GetComponent<TrackBackScript>().destroyedEnemyVectors.Add(transform.position);
				Destroy(gameObject);
			}
		}

//		disToCamera = cameraMain.transform.position.x - transform.position.x; 
//		//Debug.Log (disToCamera);
//		posOnScreen = Camera.main.WorldToScreenPoint(gameObject.transform.position);
//		float x = posOnScreen.x;
//		if ( disToCamera > 1175){
//			//print (gameObject.name + "'s distance to camera is over 384 and should been destroyed!");
//			Destroy(gameObject);
//		}
//
//		if (startShoot == true && x <= Screen.width*0.44f){
//			StartCoroutine(Shoot ());
//			startShoot = false;
//		}
	}

	void OnTriggerEnter(Collider c){
		if(c.transform.tag == "bullet"){
			cameraMain.GetComponent<TrackBackScript>().destroyedEnemyVectors.Add(transform.position);
			Destroy(c.gameObject);
			Destroy(gameObject);
			testPlayer.GetComponent<playerControl>().GainScore(4);
		}
	}
	public void BulletSpeed(float a){
		bulletSpeed = a;
	}
	public void BulletNumber(float n){
		bulletNumber = n;
	}
	public void BulletRestTime(float rt){
		restTime = rt;
	}

	IEnumerator Shoot(){
		testPlayer = GameObject.FindGameObjectWithTag("Player");
		Vector3 tarPos =  testPlayer.gameObject.transform.position;
		Vector3 shootDir = tarPos - transform.position; 

		while(true){
			for (int i = 0; i < bulletNumber; i++){
				GameObject bullet = Instantiate (bulletResource, transform.position, bulletResource.transform.rotation) as GameObject;
				bullet.GetComponent<Rigidbody>().velocity = shootDir*bulletSpeed * Time.deltaTime;
				//Destroy(bullet, 10/bulletSpeed);
				yield return new WaitForSeconds (shootRate);
			}
			 tarPos =  testPlayer.gameObject.transform.position;
			 shootDir = tarPos - transform.position; 
			yield return new WaitForSeconds (restTime);
			}
		}


	public void ResetShooting(){
		canShoot = false;
		startShoot = true;
		StopCoroutine("Shoot");
		Debug.Log ("should stop shoot()");

	}

}
