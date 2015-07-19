using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameGlobalUIScript : MonoBehaviour {

	public Image BPArea;
	public Image GRYArea;

	public void GrayAreaPos(float PosValue){
		float minX;
		float maxX;
		minX = 2048 * ( PosValue / 60f);
		maxX = 2048 * ( PosValue / 60f);
		GRYArea.rectTransform.offsetMin = new Vector2 (minX, 0);
		GRYArea.rectTransform.offsetMax = new Vector2 (maxX, 1);


	}

	public void BPAreaValue (float SizeValue){

		float minX;
		minX = 2048 * (SizeValue / 60f + 0.3f);
		BPArea.rectTransform.offsetMin = new Vector2 (minX , 0);
		//print ("min X is " + minX);
		}

	public void ExitToMenu(string levelname)
	{
		Application.LoadLevel(levelname);
	}

}
