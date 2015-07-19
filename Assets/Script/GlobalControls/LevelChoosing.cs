using UnityEngine;
using System.Collections;

public class LevelChoosing : MonoBehaviour {
	

	public void ChangeToScene(string levelname)
	{
		Application.LoadLevel(levelname);

	}

	public void SetLevelVersion(int v){
		PlayerPrefs.SetInt("currentVersion", v);
	}

}
