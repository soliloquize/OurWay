using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndScrScript : MonoBehaviour {

	public Image greyScrn;
	public Text story;
	int canChangeScr = 0;

	void Update () {

		if (Input.touchCount > 0) {
			if(Input.GetTouch(0).phase == TouchPhase.Began){
				if ( canChangeScr == 0){
				greyScrn.gameObject.SetActive(true);
					canChangeScr = 2;
				//changeToStory();
				}

				if (canChangeScr == 2){
					story.gameObject.SetActive(true);
				//	changeToStory();
				}
			}
		}
	}

	IEnumerator changeToStory(){
		canChangeScr = 1;
		yield return new WaitForSeconds (3);
		canChangeScr = 2;

	}



}
