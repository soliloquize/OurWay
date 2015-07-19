using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class Item  {

		public string levelName;
		public string levelID;
		//public int diff;
		//public int fun;
		//public Button loadAsShooterOnly;
		//public Button loadAsShooterBuilder;
}

	
public class LevelListMenu : MonoBehaviour {
		
	public GameObject sampleButton;
	public List<Item> itemList;		
	public Transform contentPanel;
	
	void Start () {

		LoadLevelList ();
	}

	void LoadLevelList(){
		string fileName = "LevelList.txt";

		string fileNameIos = Application.persistentDataPath + "/" + fileName;
		StreamReader sr = File.OpenText (fileNameIos);
		string s = sr.ReadLine ();
		//print ("the loading level list is " + s);
		string[] itemSet = s.Split('|');
		foreach(string i in itemSet){
			if (i.Length <= 0 ){
				PopulateList();
				return;
			}

			string[] subItemSet = i.Split('_');
			string lvID = subItemSet[0];
			string demonName = subItemSet[1];
			string builderName = subItemSet[2];
			//string levelVer = subItemSet[3];

			string lvName = demonName + "_" + builderName;

			Item newitem = new Item();
			newitem.levelID = lvID;
			newitem.levelName = lvName;
			itemList.Add(newitem);

		}


	}


	void PopulateList () {
		foreach (var item in itemList) {
			GameObject newButton = Instantiate (sampleButton) as GameObject;
			LevelButton button = newButton.GetComponent <LevelButton> ();
			button.levelName.text = item.levelName;
			button.levelID.text = item.levelID;
			//button.diffLv.text = item.diff.ToString();
			//button.funLv.text = item.fun.ToString();

			//button.shooterOnly = item.loadAsShooterOnly;
			//button.shooterBuilder = item.loadAsShooterBuilder;
			newButton.transform.SetParent (contentPanel);
		}
	}
	

	
}
