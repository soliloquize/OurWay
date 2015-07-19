using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public class editorControl : MonoBehaviour {
	public static int currTouch = 0;
	//find the player
	//private GameObject testPlayer;
	//adding gameobject
	public GameObject chosenBlock = null;
	public bool isAdding = false;
	public bool isDeleting = false;
	public AudioSource addBlockAudio;

	private Rect editorRect ;
	public float edRecYL;
	public float cell_size = 1f;
	private float x, y; 
	private Vector3 currentLoc;

	//list/dictionary
	public List<mapClass> currentMap = new List<mapClass>();
	//public List<mapClass> edgeMap = new List<mapClass>();
	public List<Vector3> currentVector = new List<Vector3>();
	//public List<Vector3> destroyedEnemyVectors = new List<Vector3>();
	
	public string objName;
	public int objIndex;
	
	//private Rect blockChooserRect;
	//private Rect blockChooserRectFlip;	
	public Transform terrainBlock;

	//set checkpoint
	//public float checkPointTime = 20f;
	//private GameObject checkPointLine;
	//private float trackBackX = 0;
	private float curCameraX;
	//public Vector3 checkPointLoc;
	public int saveHealthPoint;
	public int loadHealthPoint;

	//point system!
	public int curPoint;
	public int pointGainRate;
	public Text pointText;
	private int pointCost;

	public GameObject edgeBlock;
	//private float edgeX;

	public int gridSize = 1;
	//public float startTime =1;
	public float speed = 1f;
	public GameObject spaPart;
	public Text spaPartText;
	//public ToggleGroup editorToggles;

	private int builderScore = 0;
	private int score;

	public bool canDrawCheckpoint;
	public int lvVer;
	void Start () {

		x = 0f;
		y = 0f;
		objIndex = 1;
		//testPlayer = GameObject.Find ("PlanePlayer");
		//editor rect
		//EditorRectChange(13f);
		//trackBackX = transform.position.x;
		canDrawCheckpoint = true;
		StartCoroutine(WaitToDrawFlag(5f));
		//checkPointLine = GameObject.Find ("checkPointLine");
		editorRect = new Rect (Screen.width * 0.5f, Screen.height*0.1f, Screen.width * 0.5f, Screen.height*0.7f);
		//Debug.Log ( "editor rect is " + editorRect);
		//SaveCheckPoint ();
		//GeneratePreEdge ();
		//edgeX = 4;
		speed = 1f;
		PlayerPrefs.SetInt("currentVersion", lvVer);
		if (PlayerPrefs.GetInt("currentVersion") == 2 ){
			int newID = Random.Range(00001, 99999);
			string levelID = newID.ToString("00000");
			PlayerPrefs.SetString("currentLv", levelID);
		}
	}


	public void RunGame(){
		InvokeRepeating ("PointGain", 1f, 1f);
		Time.timeScale = 1;
		//InvokeRepeating ("GenerateEdge", 1f, speed*cell_size/1.2f);
		InvokeRepeating ("Move", 0.1f, speed*0.1f);		
	}

	public void PauseGame(){
		//CancelInvoke("PointGain");
		Time.timeScale = 0;
		//CancelInvoke("GenerateEdge");
		//CancelInvoke("Move");
	}

	public void IsCheating(bool cheating){
		if (cheating == true){
			Time.timeScale = 0;
			isAdding = false;
		}else{
			Time.timeScale = 1;
		}
	}

	public void Move () {
		Vector3 scroll = new Vector3 (transform.position.x + gridSize * 0.1f, 2f, -10f);
		transform.position = scroll;
	}

	void PointGain(){
		curPoint = curPoint + pointGainRate;
	}

	public IEnumerator WaitToGainPoints(float t){
		yield return new WaitForSeconds(t);
		curPoint = curPoint + pointGainRate;
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

//	public void EditorRectChange(float eY){
//		edRecYL = eY/60 + 0.25f + 0.05f;
//		editorRect = new Rect (Screen.width * edRecYL, Screen.height*0.15f, Screen.width * (1f - edRecYL), Screen.height*0.65f);
//		//print ("editor rect is " + editorRect);
//	}

	//generate chosen block, add to lists, add index, culculate score and point
	void GenerateBlock(){

		GameObject newItem = Instantiate (chosenBlock, new Vector3 (x, y, 0), chosenBlock.transform.rotation) as GameObject;
		newItem.name = objName+objIndex;
		currentMap.Add ( new mapClass(objName, objIndex,currentLoc));
		currentVector.Add (currentLoc);
		objIndex = objIndex+1;
		newItem.transform.parent = terrainBlock;
		if (pointCost < 0 && curPoint >392){}else{curPoint = curPoint - pointCost;}
		builderScore = builderScore + score;

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
						if (curPoint - pointCost>0) {

							objName = chosenBlock.name;
					
							if (currentVector.Count != 0){

								if(currentVector.Contains(currentLoc)){
								}
								else {
									if (curPoint >= 392 && pointCost <0){
									} 
									else if ( chosenBlock.name != "CheckPointPre" &&  curPoint - pointCost>0){ 
										GenerateBlock();
									} else if (canDrawCheckpoint == true ){
										GenerateBlock();
										StartCoroutine(WaitToDrawFlag(20f));
									} else {
										isAdding = false;
										chosenBlock = null;
									}
								}
							} else {
								GenerateBlock();
							}					
						} else if (pointCost < 0){
							GenerateBlock();
						}

					} else if(isDeleting ){
							if(currentVector.Contains (currentLoc)){
							mapClass m = currentMap.Find ( currentMapItem => currentMapItem.objLoc == currentLoc);
							string deleteObjectName = m.objName + m.index;	
							GameObject deleteObject = (GameObject) GameObject.Find(deleteObjectName);
							currentMap.Remove(m);
							currentVector.Remove(currentLoc);
							Destroy(deleteObject);
							int recoverPoint = deleteObject.gameObject.GetComponent<BlockAttribute> ().buildCost;
							int recoverScore = deleteObject.gameObject.GetComponent<BlockAttribute>().score;
							curPoint = curPoint + recoverPoint;
							builderScore = builderScore - recoverScore;
						}
						else { }
					}
				}
			}	

		}

		if (PlayerPrefs.GetInt("currentVersion") == 0 ){
		}else {
			pointText.text = "Editor Point: " + curPoint.ToString();
		}
		int standardScore = gameObject.GetComponent<DifChange>().builderSS;
		if (builderScore >= standardScore){
			gameObject.GetComponent<DifChange>().bSS = true;
		}

	}
//	public void SendListsToBackTrack(){
//		GetComponent<TrackBackScript>().LoadLevelLists();
//		//GetComponent <TrackBackScript>().BackTrack();
//	}
	public IEnumerator WaitToDrawFlag(float waittime){
		canDrawCheckpoint = false;
		yield return new WaitForSeconds(waittime);
		canDrawCheckpoint = true;
	}

	public void ChooseBlock(/*string blockName*/){
//		isDeleting = false;
//		string chosenBlockName = blockName +"Pre";
//		chosenBlock = (GameObject)Resources.Load(chosenBlockName, typeof(GameObject));

		chosenBlock = GetComponent<BlockChoosing>().chosenBlock;
		//print (chosenBlock.name);
		pointCost = chosenBlock.gameObject.GetComponent<BlockAttribute> ().buildCost;
		score = chosenBlock.gameObject.GetComponent<BlockAttribute>().score;
		addBlockAudio = chosenBlock.gameObject.GetComponent<BlockAttribute>().blockSoundEffect;
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
				if ( block.objName == "PhaseChangeLine"){
				} else {
				chosenBlock = (GameObject)Resources.Load(block.objName, typeof (GameObject));
				pointCost = chosenBlock.GetComponent<BlockAttribute>().buildCost;

					if(curPoint >= 392 && pointCost < 0){
					} else {
						GameObject newItem = Instantiate (chosenBlock, block.objLoc,chosenBlock.transform.rotation) as GameObject;
						newItem.name = block.objName + objIndex;
						mapClass newMapObject = new mapClass(block.objName, objIndex, block.objLoc);
						currentMap.Add (newMapObject);
						newItem.transform.parent = terrainBlock;
						currentVector.Add (block.objLoc);
						objIndex = objIndex+1;

						curPoint = curPoint - pointCost;
					}
				}
			}
		}
	}
		
	public void AddLoadList(List<mapClass> mapLoaded){
		Debug.Log ("heard from loading!");
		foreach (mapClass m in mapLoaded){
			mapClass newMapObject = new mapClass(m.objName, m.index, m.objLoc);
			currentMap.Add (newMapObject);
			currentVector.Add (m.objLoc);
		}


	}

	public void AddDifLine(int phase){
		GameObject line = (GameObject)Resources.Load("PhaseChangeLine", typeof(GameObject));
		Vector3 lineLoc = new Vector3 ( gameObject.transform.position.x, 192f, 1f);
		GameObject newItem = Instantiate (line, lineLoc, Quaternion.identity) as GameObject;
		newItem.name = "PhaseChangeLine" + objIndex;
		currentMap.Add ( new mapClass("PhaseChangeLine", objIndex,lineLoc));
		objIndex = objIndex+1;
		newItem.transform.parent = terrainBlock;

		if (phase == 4) {
			Vector3 endlineLoc = new Vector3 ( gameObject.transform.position.x + 3500f, 192f, 1f);
			GameObject newEndLine = Instantiate (line, endlineLoc, Quaternion.identity) as GameObject;
			newEndLine.name = "EndLine";
			currentMap.Add ( new mapClass("PhaseChangeLine", 0 ,lineLoc));
			newEndLine.transform.parent = terrainBlock;
			newEndLine.transform.tag = "Ending";
		}
	}
	 
}
