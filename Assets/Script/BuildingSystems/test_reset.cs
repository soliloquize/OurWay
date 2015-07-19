using UnityEngine;
using System.Collections;

public class test_reset : MonoBehaviour {

	public GameObject map;

public void resetMap(){

		map = GameObject.Find ("terrain");
		foreach (Transform child in map.transform) {
			Destroy(child.gameObject);
				}

	}
}
