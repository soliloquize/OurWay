using UnityEngine;
using System.Collections;

public class BlockAttribute : MonoBehaviour {

	public int healthPoint;
	public bool CanBroke;
	public bool CausedDie;
	public int takeDamagePoint;
	public int buildCost;
	//public bool CanBlock;
	public AudioSource blockSoundEffect;
	public bool CanCollect;
	public int blockTypeId;
	public int score;

	public void TakeDamage(){
		healthPoint -= 1;
		}
	public void BlockBChangeColor(){

		if (healthPoint == 3) {
			gameObject.renderer.material.color = new Color(1f, 0.6f, 0f, 1f);
		}else if (healthPoint == 2) {
			gameObject.renderer.material.color = new Color(0.95f, 1f, 0.2f, 1f);
		}else if (healthPoint == 1) {
			gameObject.renderer.material.color = new Color(1f, 0.45f, 0.15f, 1f);
		}else if (healthPoint == 0) {
			gameObject.renderer.material.color = new Color(1f, 0.15f, 0.15f, 1f);
		}
	}
		
	void OnCollisionEnter(Collision c){
		if(c.transform.tag == "EnemyBullet"){
			Destroy(c.gameObject);
		}
	}

}
