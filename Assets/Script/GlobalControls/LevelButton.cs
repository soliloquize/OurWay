using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LevelButton : MonoBehaviour {
	
	public Text levelID;
	public Text levelName;
	//public Text diffLv;
	//public Text funLv;

	public void LoadAsShootOnly () {
		string lvID = gameObject.GetComponentInParent<LevelButton>().levelID.text;

		PlayerPrefs.SetString("currentLv",lvID);
		PlayerPrefs.SetInt("currentVersion", 0);
		Debug.Log("the level id when pressing load as shoot only, is " + lvID);
		Application.LoadLevel("Load_ShootOnly");
	}
	
	public void LoadAsShootBuild () {
		string lvID = gameObject.GetComponentInParent<LevelButton>().levelID.text;
		PlayerPrefs.SetString("currentLv", lvID );
		PlayerPrefs.SetInt("currentVersion", 4);
		Application.LoadLevel("Load_ShootBuild");
	}
	
	public void LoadAsBuildOnly (int needLoadLv) {
		string lvID = gameObject.GetComponentInParent<LevelButton>().levelID.text;
		PlayerPrefs.SetString("currentLv",lvID);
		PlayerPrefs.SetInt("currentVersion", 1);
		PlayerPrefs.SetInt("loadPreLevel", needLoadLv);
		Debug.Log("the level id when pressing load as shoot only, is " + lvID);

		Application.LoadLevel("Load_BuildOnly");
	}
	
	public void LoadAsBuildOnly2 (int needLoadLv) {
		string lvID = gameObject.GetComponentInParent<LevelButton>().levelID.text;
		PlayerPrefs.SetString("currentLv",lvID);
		PlayerPrefs.SetInt("currentVersion", 3);
		PlayerPrefs.SetInt("loadPreLevel", needLoadLv);
		Debug.Log("the level id when pressing load as shoot only, is " + lvID);

		Application.LoadLevel("Load_BuildOnly_2");
	}

	public void BuildOnlyNewLv(){
		PlayerPrefs.SetInt("loadPreLevel", 0);
	}

}
