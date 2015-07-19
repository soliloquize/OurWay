using UnityEngine;
using System.Collections;

public class BlockChoosing : MonoBehaviour {

	public GameObject chosenBlock;

	public void isChoosingBlockA(bool a){
		chosenBlock = (GameObject)Resources.Load("BlockAPre", typeof(GameObject));
	}

	public void isChoosingBlockB(bool b) {
		chosenBlock = (GameObject)Resources.Load("BlockBPre", typeof(GameObject));
	}

	public void isChoosingBlockC(bool c) {
		chosenBlock = (GameObject)Resources.Load("BlockCPre", typeof(GameObject));
	}

	public void isChoosingCheckPoint (bool save) {
		chosenBlock = (GameObject)Resources.Load("CheckPointPre", typeof(GameObject));
	}

	public void isChoosingGreenJelly (bool d) {
		chosenBlock = (GameObject)Resources.Load("GreenJellyPre", typeof(GameObject));
	}

	public void isChoosingDifChangeLine (bool l) {
		chosenBlock = (GameObject)Resources.Load("PhaseChangeLine", typeof(GameObject));
	}

}
