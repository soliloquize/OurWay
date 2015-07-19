using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

	void Update(){
		Vector3 viewPos = Camera.main.WorldToViewportPoint(gameObject.transform.position);
		if(  (viewPos.x <= 0f) || (viewPos.x > 0.44f) ){

			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision c){
		if (c.transform.tag == "CauseDie"){

			Destroy(gameObject);
		}
	}


}
