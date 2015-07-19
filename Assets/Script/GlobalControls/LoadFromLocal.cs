using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LoadFromLocal : MonoBehaviour {

	public List<mapClass> mapLoaded = new List<mapClass>();
	public string levelID;
	public Transform terrainBlock;
	public int loadVersion;

	void Start(){
		levelID = PlayerPrefs.GetString("currentLv");
		Debug.Log ("the level id in load from local start() is " + levelID);
		loadVersion = PlayerPrefs.GetInt("currentVersion");
		LoadList (levelID); 
		}

	void LoadList(string v){

		string fileName = levelID + ".txt";
		string fileNameIos = Application.persistentDataPath + "/" + fileName;
		StreamReader sr = File.OpenText (fileNameIos);
		string s = sr.ReadLine ();

		string[] itemSet = s.Split('/');
		
		foreach(string i in itemSet){			
			if(i.Length<=0) {
				switch (loadVersion){
				case 0:
					AddToMapShootOnly (mapLoaded);
					return;
				case 1:
					AddToMapBuildOnly (mapLoaded);
					return;
				case 2:
					AddToMapShootBuild (mapLoaded);
					return;
				case 3: 
					AddToMapBuildOnly (mapLoaded);
					return;
				case 4:
					AddToMapShootBuild (mapLoaded);
					return;
				}
			}
			
			string[] subItemSet = i.Split('_');
			string objName = subItemSet[0];
			string index = subItemSet[1];
			string locString = subItemSet[2];

			string[] xy = locString.Split(',');
			float x = float.Parse(xy[0]);
			float y = float.Parse(xy[1]);
			Vector3 loc = new Vector3(x,y,0f);
			GameObject chosenObj = Resources.Load(objName) as GameObject;

			if (chosenObj.name == "PhaseChangeLine"){
				loc.z = 0f;
				GameObject newGameObj = Instantiate (chosenObj, loc, chosenObj.transform.rotation) as GameObject;
				if (index == "0"){
					if (PlayerPrefs.GetInt("currentVersion") == 0 ){
						newGameObj.transform.tag = "Ending";
					
					}else {
						int temInd = itemSet.Length + 2;
						index = temInd.ToString();
					}
				}
				newGameObj.name = objName + index;
				newGameObj.transform.parent = terrainBlock;

			} else {

				GameObject newGameObj = Instantiate (chosenObj, loc, chosenObj.transform.rotation) as GameObject;
				newGameObj.name = objName + index;
				newGameObj.transform.parent = terrainBlock;

				if (chosenObj.name == "GreenJellyPre" && ( PlayerPrefs.GetInt("currentVersion") == 1 || PlayerPrefs.GetInt("currentVersion") == 3)){
					Destroy(newGameObj.GetComponent<EnemyShooting>());
				}
			}


			int objIndex = int.Parse(index);
			mapLoaded.Add (new mapClass(objName, objIndex ,loc));
			//Debug.Log(mapLoaded.Count);
		}

	}

	void AddToMapBuildOnly(List<mapClass> m){
		gameObject.GetComponent<editorControlBuildOnly>().AddLoadList(m);
	}

	void AddToMapShootOnly(List<mapClass> m){
		gameObject.GetComponent<editorControl>().AddLoadList(m);
	}

	void AddToMapShootBuild(List<mapClass> m){
		gameObject.GetComponent<editorControl>().AddLoadList(m);
	}

}
