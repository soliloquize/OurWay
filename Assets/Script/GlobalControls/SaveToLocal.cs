using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SaveToLocal : MonoBehaviour {

	public List<mapClass> mapToSave;
	public GameObject playCamera;
	public Text demonName;
	public Text builderName;
	//public string lvID;

	public void SaveNewLevel(){

		if(PlayerPrefs.GetInt("loadPreLevel") == 1){
			int newID = Random.Range(00001, 99999);
			string levelID = newID.ToString("00000");
			PlayerPrefs.SetString("currentLv", levelID);
		}
		string levelId = PlayerPrefs.GetString("currentLv");
		SaveToLevelList(levelId);
		SaveLevel(levelId);

	}

	public void OverWriteLevel(){
		string id = PlayerPrefs.GetString("currentLv");
		SaveLevel(id);
	}

	void SaveToLevelList(string newID){

		Item newLevel = new Item();
		newLevel.levelName = demonName.text + "_" + builderName.text;
		//newLevel.diff = 0;
		//newLevel.fun = 0;
		newLevel.levelID = newID;
		string levelVer = PlayerPrefs.GetInt("currentVersion").ToString();

		string levelInfoToAdd =   newID + "_" + demonName.text + "_" + builderName.text + "_" + levelVer /* + "," + newLevel.diff.ToString() + ","
			+ newLevel.fun.ToString() */ + "|";
		string filename = "LevelList.txt";
		string fileNameIos = Application.persistentDataPath + "/" + filename;



		if (File.Exists(fileNameIos)){
			//Debug.Log ("file exists");
			StreamReader sr = File.OpenText (fileNameIos);
			string preLine = sr.ReadLine();
			sr.Close();
			//Debug.Log ("read previous line in the file as : " + preLine);
			string lineToWrite = preLine + levelInfoToAdd;
			//Debug.Log ("file lines to write in is " + lineToWrite);

			StreamWriter sw = File.CreateText (fileNameIos);
			sw.WriteLine(lineToWrite);
			sw.Close();

		} else {
			//Debug.Log ( "file not exist");
			StreamWriter sw = File.CreateText (fileNameIos);
			sw.WriteLine(levelInfoToAdd);
			sw.Close();
		}

//		if (!Directory.Exists(fileNameIos)){
//			StreamWriter sw = File.CreateText (fileNameIos);
//			sw.WriteLine(levelInfoToAdd);
//			sw.Close();
//		} else {
//			StreamWriter sw = new StreamWriter(fileNameIos,true);
//			sw.WriteLine(levelInfoToAdd);
//			sw.Close();
//		}


		//StreamReader sr = File.OpenText (fileNameIos);
		//string preLine =  sr.ReadLine();
		//StartCoroutine(GetComponent<SaveFileToServer>().LevelListInfo(lists));

	}


	void SaveLevel(string id){ //save the list to a local file
		print (PlayerPrefs.GetInt("currentVersion"));
		if (PlayerPrefs.GetInt("currentVersion") == 2){
		mapToSave = GetComponent<editorControl> ().currentMap;
		} else if (PlayerPrefs.GetInt("currentVersion") == 1 || PlayerPrefs.GetInt("currentVersion") == 3 ) {
		mapToSave = GetComponent<editorControlBuildOnly> ().currentMap;
			//print ("current map count is " + mapToSave.Count);
		}
		string writeToFileString = "";

		foreach(mapClass m in mapToSave){

			string x = m.objLoc.x.ToString();
			string y = m.objLoc.y.ToString();
			//string z = m.objLoc.z.ToString();
			writeToFileString +=  m.objName + "_"  + m.index.ToString() + "_"  + x + "," + y + /*"," + z + */ "/";
		}
		//print (writeToFileString);


		string fileName = id + ".txt";

		string fileNameIos = Application.persistentDataPath + "/" + fileName;
		Debug.Log ("the filename in savetolocal is " + fileNameIos);
		//string path = System.Environment.GetFolderPath( System.Environment.SpecialFolder.Personal );
		#if UNITY_IOS
		StreamWriter sw = File.CreateText (fileNameIos);
		#else
		StreamWriter sw = File.CreateText (fileName);
		#endif
		sw.WriteLine(writeToFileString);
		sw.Close ();

		//Debug.Log ("the id is " + id + " the write to file string is " + writeToFileString);
		StartCoroutine(GetComponent<SaveFileToServer>().UploadLevelInfo(id, writeToFileString));
		//return writeToFileString;
		//StartCoroutine(GetComponent<SaveFileToServer>().UploadLevelInfo(id, writeToFileString));
	}





}
