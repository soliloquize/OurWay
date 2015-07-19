using UnityEngine;
using System.Collections;

public class SaveFileToServer : MonoBehaviour {

	string uploadLevelBlockUrl= "http://www.jun610.com/ourway/levelRecordings/levelBlockUpload.php"; //be sure to add a ? to your url
	//string uploadLevelListUrl = "http://www.jun610.com/ourway/levelLists/levelListUpload.php";
	public string uploadBuildingActUrl = "http://www.jun610.com/ourway/metricRecordings/buildingUpload.php";


	public IEnumerator UploadLevelInfo(string levelID, string mapBlocks) {
		
		//CancelInvoke("DoRecord");

		int lvVer = PlayerPrefs.GetInt("currentVersion");
		string levelInfo = levelID + "_" + lvVer.ToString();
		WWWForm form = new WWWForm();

		form.AddField("action","saveLevel");
		form.AddField("lvID", levelInfo);
//		form.AddField("content",mapBlocks);

		string s = mapBlocks;
		string[] itemSet = s.Split('/');

		string blocks = "";

		foreach(string i in itemSet){

			if (i.Length <= 0 ){
				break;
			}

			blocks += "|" + i;
		}

		form.AddField("blockID", blocks);
//		string objName = " ";
//		string index = " ";
//		string locString = " ";
//
//		foreach(string i in itemSet){
//			if (i.Length <= 0){
//				break;
//			}
//			string[] subItemSet = i.Split('_');
//			objName += subItemSet[0];
//			index += subItemSet[1];
//			string locxy = subItemSet[2];
//			string[] xy = locxy.Split(',');
//			locString += xy[0] + "-" + xy[1];
//
//		}
//
//		form.AddField("blockPos", locString);
//		form.AddField("blockType",objName);
//		form.AddField("blockID",index);
		
		//form.AddField("metrics", inMetrics.ToString() ); //this could be from Player Settings
		//form.AddField ("name", inID);


		
		WWW w = new WWW(uploadLevelBlockUrl,form);
		yield return w;
		if (w.error != null) {
			Debug.Log(w.error);
		} else {
			Debug.Log ("Successful Upload!");
			Debug.Log (w.text + "\n\n" + form.ToString());
		}  

	}


//	public IEnumerator LevelListInfo(string levelList) {
//		
//		WWWForm form = new WWWForm();
//
//		form.AddField("action","saveLevelList");
//		form.AddField("fileName", "list");
//		form.AddField("lvList", levelList);
//		WWW w = new WWW(uploadLevelListUrl,form);
//		yield return w;
//		if (w.error != null) {
//			Debug.Log(w.error);
//		} else {
//			Debug.Log ("Successful Upload list!");
//			Debug.Log (w.text + "\n\n" + form.ToString());
//		} 
//	}

}
