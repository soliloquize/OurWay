using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class editorControlBuildOnly : MonoBehaviour {
	public static int currTouch = 0;

	//adding gameobject
	public GameObject chosenBlock = null;
	public bool isAdding = false;
	public bool isDeleting = false;

	private Rect editorRect ;
	//public float edRecYL;
	public float cell_size;
	private float x, y; 
	private Vector3 currentLoc;

	//list/dictionary
	public List<mapClass> currentMap = new List<mapClass>();
	public List<Vector3> currentVector = new List<Vector3>();

	//difficulty line list
	public List<Vector2> difLineVector = new List<Vector2>();
	List<float> checkPosList = new List<float>();
	int checkPointCount = 0;
	//bool canDrawNewLine = true;
	
	public string objName;
	public int objIndex;

	public Transform terrainBlock;

	//private GameObject checkPointLine;

	//point system!
	int pointCost;
	int pointCostTotal;

	public int gridSize = 1;
	public GameObject spaPart;

	int builderScore = 0;
	int score;
	float speed;

	public int lvVer;

	void Start () {
		x = 0f;
		y = 0f;
		objIndex = 1;
		PlayerPrefs.SetInt("currentVersion", lvVer);
		int isLoadingPreLv = PlayerPrefs.GetInt("loadPreLevel");
		if (isLoadingPreLv == 1){

			gameObject.AddComponent<LoadFromLocal>();
			gameObject.GetComponent<LoadFromLocal>().terrainBlock = terrainBlock;
		} else {
			//if (PlayerPrefs.GetInt("currentVersion") == 1 || PlayerPrefs.GetInt("currentVersion") == 3 ){
			int newID = Random.Range(00001, 99999);
			string levelID = newID.ToString("00000");
			PlayerPrefs.SetString("currentLv", levelID);
			//}
		}


		if (PlayerPrefs.GetInt("currentVersion") == 3){
			speed = 1f;
			InvokeRepeating ("Move", 0.1f, speed * 0.1f);
			checkPosList.Add (transform.position.x + 1f);
			Debug.Log("suppose to move ");
		}


		editorRect = new Rect (Screen.width * 0.5f, Screen.height*0.1f, Screen.width * 0.5f, Screen.height*0.7f);
	}

	public void Move () {
		Vector3 scroll = new Vector3 (transform.position.x + gridSize * 0.1f, 2f, -10f);
		transform.position = scroll;
	}

	public void SpeedUp(){
		if (speed > 0.4f) {
			speed = speed - 0.2f;
		}
	}
	
	public void SpeedDown(){
		if (speed < 1.6f) {
			speed = speed + 0.2f;
		}
	}
	public void RunGame(){
		Time.timeScale = 1;
	}

	public void PauseGame(){
		Time.timeScale = 0;
	}

	public void Move (float dis) {
		Vector3 scroll = new Vector3 (transform.position.x + dis, 2f, -10f);
		transform.position = scroll;
	}

	public void MoveToPreCheckPoint(){
		Vector3 prePos = new Vector3 (checkPosList[checkPointCount - 1] - 20f, 2f, -10f);
		transform.position = prePos;
		checkPointCount --;
	}


	void GenerateBlock(){
		//generate chosen block, add to lists, add index, culculate score and point
		GameObject newItem = Instantiate (chosenBlock, new Vector3 (x, y, 0), chosenBlock.transform.rotation) as GameObject;
		newItem.name = objName+objIndex;
		if(chosenBlock.name == "GreenJellyPre"){
			Destroy(newItem.GetComponent<EnemyShooting>());
		}else if (chosenBlock.name == "PhaseChangeLine"){

			Vector3 lineLoc = new Vector3 (x, 120f, 0);
			difLineVector.Add(new Vector2 (x, 120f) );
			newItem.transform.position = lineLoc;
			currentLoc = lineLoc;
			//Debug.Log("the currentLoc in add phase line vector statement with giving a lineLoc is " + currentLoc.x + "," + currentLoc.y);
			pointCost = 0;
			score = 0;
			//WaitToRespawn(1f);
		}
		if(chosenBlock.name == "CheckPointPre" || chosenBlock.name == "PhaseChangeLine"){
			checkPosList.Add (currentLoc.x - 1f);

		}

		//Debug.Log("the currentLoc is adding to the 2 main lists is " + currentLoc.x + "," + currentLoc.y);

		currentMap.Add ( new mapClass(objName, objIndex,currentLoc));
		currentVector.Add (currentLoc);
		objIndex = objIndex+1;
		newItem.transform.parent = terrainBlock;

		builderScore = builderScore + score;
		pointCostTotal = pointCostTotal + pointCost;
		GameObject spawnParticle = Instantiate (spaPart, new Vector3 (x,y,0), spaPart.transform.rotation) as GameObject;
		spawnParticle.name = "par" + objIndex;
		spawnParticle.transform.parent = terrainBlock;
		Destroy(spawnParticle, 2f);
	}

	void Update () {

		if (Input.touchCount > 0) {
						
		for(int i = 0; i < Input.touchCount; i++) {
		
			currTouch = i;

				if (editorRect.Contains (Input.GetTouch (i).position)){

					Vector3 touchPos = new Vector3 ();
					touchPos = Camera.main.ScreenToWorldPoint (Input.GetTouch (i).position);
					touchPos.z = 0;
					
					x = Mathf.Round (touchPos.x / cell_size) * cell_size;
					y = Mathf.Round (touchPos.y / cell_size) * cell_size;
					
					currentLoc = new Vector3 (x,y,0);

					if(isAdding){
							objName = chosenBlock.name;
					
							if (currentVector.Count != 0){

								if(currentVector.Contains(currentLoc)){

								} else {

									if(chosenBlock.name == "PhaseChangeLine") {
										if (difLineVector.Count >= 5){
											isAdding = false;
											chosenBlock = null;
										}else if (CheckLinePosition(currentLoc.x) == false){
										GenerateBlock();
										}
									} else {
										GenerateBlock();
									}
									
								}
							} else {

							GenerateBlock();
						}

					} else if(isDeleting){
						if(currentVector.Contains (currentLoc)){
							mapClass m = currentMap.Find ( currentMapItem => currentMapItem.objLoc == currentLoc);
							string deleteObjectName = m.objName + m.index;	
							GameObject deleteObject = (GameObject) GameObject.Find(deleteObjectName);
							currentMap.Remove(m);
							currentVector.Remove(currentLoc);

							//Debug.Log(deleteObject.name);
							if (deleteObject.name == "PhaseChangeLine" + m.index) {
								Vector2 linePos= new Vector2(deleteObject.transform.position.x, deleteObject.transform.position.y);
								difLineVector.Remove(linePos);
							}
							if (deleteObject.name == "CheckPointPre" || deleteObject.name == "PhaseChangeLine") {
								float curX = x;
								checkPosList.Remove(curX);
							} else {
							int recoverPoint = deleteObject.gameObject.GetComponent<BlockAttribute> ().buildCost;
							int recoverScore = deleteObject.gameObject.GetComponent<BlockAttribute>().score;

							pointCostTotal = pointCostTotal - recoverPoint;
							builderScore = builderScore - recoverScore;
							}
							Destroy(deleteObject);
						}
					} 
				}
			}
		}	

		if (checkPosList.Count > checkPointCount ){
			if (transform.position.x >= checkPosList[checkPointCount]){
				checkPointCount ++;
			}
		}
	}

	bool CheckLinePosition(float curX){
		foreach(Vector2 linePos in difLineVector){
			if (linePos.x == curX) {
				return true;
			}
		}
		return false;
	}

	public void ChooseBlock(){
		chosenBlock = GetComponent<BlockChoosing>().chosenBlock;
		pointCost = chosenBlock.gameObject.GetComponent<BlockAttribute> ().buildCost;
		score = chosenBlock.gameObject.GetComponent<BlockAttribute>().score;

	}



	public void IsAdding(bool a){
		isAdding = a;
		if (a){
		ChooseBlock();
		}
		}


	public void IsDeleting(bool d){
		isDeleting = d;
		}
	


	public void PasteCopyList(List<mapClass> pasteMap){
		foreach(mapClass block in pasteMap){
			if( !currentVector.Contains(block.objLoc)){
				chosenBlock = (GameObject)Resources.Load(block.objName, typeof (GameObject));
				if (chosenBlock.name == "PhaseChangeLine" && 
				    ( difLineVector.Count >= 5 || CheckLinePosition(chosenBlock.transform.position.x) == true ) ) {

				} else {
					GameObject newItem = Instantiate (chosenBlock, block.objLoc,chosenBlock.transform.rotation) as GameObject;
					newItem.name = block.objName + objIndex;
					if(chosenBlock.name == "GreenJellyPre"){
						Destroy(newItem.GetComponent<EnemyShooting>());
					}
					if (chosenBlock.name == "PhaseChangeLine"){
						difLineVector.Add ( new Vector2 (block.objLoc.x, block.objLoc.y));
					}
					pointCostTotal = pointCostTotal + chosenBlock.GetComponent<BlockAttribute>().buildCost;
					mapClass newMapObject = new mapClass(block.objName, objIndex, block.objLoc);
					currentMap.Add (newMapObject);
					newItem.transform.parent = terrainBlock;
					currentVector.Add (block.objLoc);
					objIndex = objIndex+1;
				}
			}else{
			}
		}
	}
		
	public void AddLoadList(List<mapClass> mapLoaded){
		foreach (mapClass m in mapLoaded){
			mapClass newMapObject = new mapClass(m.objName, m.index, m.objLoc);
			currentMap.Add (newMapObject);
			currentVector.Add (m.objLoc);
			if (newMapObject.objName  == "PhaseChangeLine"){
				difLineVector.Add ( new Vector2 (newMapObject.objLoc.x, newMapObject.objLoc.y));
			}
		}
	}

//	IEnumerator WaitToRespawn(float a){
//		canDrawNewLine = false;
//		yield return new WaitForSeconds (a);
//		canDrawNewLine = true;
//
//
//	}

	public void DrawEndLine(){
		float maxX = 1f;
		float maxY = 0f;
		foreach (Vector2 loc in difLineVector){
			if (loc.x > maxX){
				maxX = loc.x;
				maxY = loc.y;
			} else {
			}
		}
		if (difLineVector.Count == 0){
			return;
		}
		Vector3 endLineLoc = new Vector3 (maxX, maxY, 0f);
		Debug.Log ("the line we find for changing to the end line is on " + maxX + "," + maxY);
		mapClass m = currentMap.Find ( currentMapItem => currentMapItem.objLoc == endLineLoc);
		string lineName = m.objName + m.index;	
		GameObject lineObject = (GameObject) GameObject.Find(lineName);
		lineObject.transform.tag = "Ending";
		currentMap.Remove (m);

		string name = "PhaseChangeLine";
		int i = 0;
		Vector3 endLoc = endLineLoc;
		mapClass endLine = new mapClass(name, i, endLoc);
		currentMap.Add(endLine);
		Debug.Log (endLine.index);

	}


	 
}
