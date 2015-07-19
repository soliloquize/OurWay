using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletScript : MonoBehaviour {
	//private ControllerColliderHit hit;
	float destroyThreshold = .4f;
	Camera main;

	//destory List
	public List<Vector3> deletedMap = new List<Vector3>();

	
	void Start(){
		main = GameObject.Find ("Main Camera").GetComponent<Camera>();
	}

	void Update(){
		Vector3 pos = main.WorldToViewportPoint (transform.position);
		if (pos.x >= destroyThreshold) Destroy (gameObject);
	}

	public void bulletRect(float x){
		destroyThreshold = x;
		}

	void OnTriggerEnter (Collider hit) {
		if (hit.transform.tag == "CauseDie") {
			float enemyHealth = hit.gameObject.GetComponent<BlockAttribute>().healthPoint;
			if (enemyHealth == 0){
				main.GetComponent<TrackBackScript>().BackupCurrentMap(hit.transform.position.x, hit.transform.position.y);
			Destroy (hit.gameObject);
			} else {
				hit.gameObject.GetComponent<BlockAttribute>().healthPoint --;

				if (hit.gameObject.GetComponent<BlockAttribute>().blockTypeId == 2){
					hit.gameObject.GetComponent<BlockAttribute>().BlockBChangeColor();
				}
			}

			Destroy (gameObject);
			
		}
	
	}
}
