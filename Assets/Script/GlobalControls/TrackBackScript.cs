using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TrackBackScript: MonoBehaviour {

	List<Vector3> mapBackup = new List<Vector3> ();
	public GameObject mainCam;
	public List<mapClass> currentMapList = new List<mapClass>();
	//List<Vector3> currentVectorList = new List<Vector3>();
	public List<Vector3> destroyedEnemyVectors = new List<Vector3>();
	public GameObject testPlayer;
	Vector3 checkPointLoc;
	float curCameraX;
	float trackBackX;
	int saveHealthPoint;
	Transform terrainBlock;

	void Start(){
		terrainBlock = GameObject.Find ("BlockContainer").transform;
		trackBackX = transform.position.x;
		SaveCheckPoint ();
	}
	//backup destroied blocks
	void Update(){
		curCameraX = transform.position.x;
	}

//	public void LoadLevelLists(){
//		List<mapClass> wholeMapList = new List<mapClass>();
//		wholeMapList = mainCam.GetComponent<editorControl>().currentMap;
//		print ( wholeMapList.Count);
//		foreach(mapClass block in wholeMapList){
//			print (block.objName + block.index + block.objLoc);
//			mapClass copiedWholeMapBlock = new mapClass(block.objName, block.index, block.objLoc); //Creating a new mapclass object so we're not directly referencing the old one
//			currentMapList.Add(copiedWholeMapBlock);
//			/*JPATT EDIT END */
//		}
////		foreach(Vector3 v in mapVec){
////			Vector3 p = new Vector3 (v.x , v.y, v.z);
////			currentVectorList.Add (p);
////		}
//		BackTrack();
//	}

	public void BackupCurrentMap(float dX, float dY){
		mapBackup.Add (new Vector3 (dX, dY ,0));
	}
	
	public void SaveCheckPoint(){
		float pX = testPlayer.transform.position.x;
		float pY = testPlayer.transform.position.y;
		checkPointLoc = new Vector3 (pX, pY, 0);
		//checkPointLine.transform.position = checkPointLoc;
		trackBackX = curCameraX;
		saveHealthPoint = testPlayer.GetComponent<playerControl> ().healthPoint;
		mapBackup.Clear();
		destroyedEnemyVectors.Clear();
	}
	
	public float restartPointX;
	public void BackTrack(){

		//Vector3 p = new Vector3 (checkPointLoc.x, checkPointLoc.y, testPlayer.transform.position.z);

		//recover items in mapBackup
		List<mapClass> wholeMapList = new List<mapClass>();
		wholeMapList = mainCam.GetComponent<editorControl>().currentMap;

		foreach (Vector3 b in mapBackup) {
			mapClass m = wholeMapList.Find ( currentMapItem => currentMapItem.objLoc == b);

			string deletedObjectName = m.objName + m.index;
			string backupBlockName = m.objName;
			GameObject backupBlock = (GameObject)Resources.Load(backupBlockName, typeof(GameObject));
			GameObject newItem = Instantiate (backupBlock, m.objLoc,backupBlock.transform.rotation) as GameObject;
			newItem.name = deletedObjectName;
			newItem.transform.parent = terrainBlock;
		}
		mapBackup.Clear ();

		//reset shooting enemies
		/* FIND ALL ENEMIES */
		GameObject[] dieObjects = GameObject.FindGameObjectsWithTag("CauseDie");
		foreach(GameObject die in dieObjects){
			if(die.GetComponent<EnemyShooting>()){
				die.GetComponent<EnemyShooting>().ResetShooting();
				Debug.Log ("the " + die.gameObject.name + "should reset shooting");
			}
		}
		/* FIND ALL BULLETS */
		GameObject[] bullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
		foreach(GameObject b in bullets){
			Destroy(b);
		}

		//recover died enemies
		/* RESPAWN KILLED ENEMIES */
		if (destroyedEnemyVectors.Count > 0){
			Debug.Log("the count in the destoryed vectors is " + destroyedEnemyVectors.Count);
			foreach(Vector3 enemyPos in destroyedEnemyVectors){
				Debug.Log("not crashing");
				//Debug.Log ("the length of vector list is " + currentVectorList.Count);
				Debug.Log ("the checking v3 is " + enemyPos);
				//if (currentVectorList.Contains (enemyPos)) {
				mapClass m = wholeMapList.Find ( currentMapItem => currentMapItem.objLoc == enemyPos);
				Debug.Log("find this v3 in current mapclass list is " + m.objLoc);
				string deletedObjectName = m.objName + m.index;
				string backupBlockName = m.objName;
				GameObject backupBlock = (GameObject)Resources.Load(backupBlockName, typeof(GameObject));
				GameObject newItem = Instantiate (backupBlock, m.objLoc,backupBlock.transform.rotation) as GameObject;
				newItem.name = deletedObjectName;
				newItem.transform.parent = terrainBlock;
				//}
			}
		}
		destroyedEnemyVectors.Clear();
		//StartCoroutine (WaitDelay(2f));
		restartPointX = curCameraX;
		transform.position= new Vector3 (trackBackX, transform.position.y, transform.position.z);
		testPlayer.transform.position = checkPointLoc;
		testPlayer.GetComponent<playerControl> ().healthPoint = saveHealthPoint;


	}

	IEnumerator WaitDelay(float waittime){
		GetComponent<editorControl>().CancelInvoke("Move");
		yield return new WaitForSeconds(waittime);
		GetComponent<editorControl>().RunGame();
	}

}
