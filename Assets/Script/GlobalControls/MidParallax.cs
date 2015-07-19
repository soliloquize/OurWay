using UnityEngine;
using System.Collections;

public class MidParallax : MonoBehaviour {

	public float paraSpeed = 0;

	public void Update () {

		renderer.material.mainTextureOffset = new Vector2 (Time.time * paraSpeed, 0f);
		
	}


	public void SpeedUp(){
		if (paraSpeed<0.036f){
		paraSpeed = paraSpeed + 0.004f;
		}
	}
	
	public void SpeedDown(){
		if (paraSpeed>0.007f){
		paraSpeed = paraSpeed - 0.004f;
		}
	}

}
