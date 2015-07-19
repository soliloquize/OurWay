using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CopyPaste : MonoBehaviour {

	public Camera miniMapCam;
	public GameObject mainCamera;

	//lists of map objects
	//public List<mapClass> wholeMapList;
	public List<mapClass> copyMap = new List<mapClass>();
	//x values of first tap(copy from this position) and second tap(paste to the position) on the minimap
	public float copyFromPosX;
	public float pasteTarPosX;
	public float areaWide; //how big is the copy area
	//public Button copyPasteButtonA;
	//public Button copyPasteButtonB;
	public Image butnImg;
	public GameObject framePre;
	//public Texture tempImg;

	public float disToCopy;
	public float disToPaste;

	private GameObject frame;
	private bool isDraggingToCopy;
	private bool isDraggingToPaste;
	//private Texture2D scrShot;

	void Start () {
		//wholeMapList = mainCamera.GetComponent<editorControl>().currentMap;
		framePre = (GameObject)Resources.Load("copypaste_framePre", typeof(GameObject));
		isDraggingToCopy = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0 && isDraggingToCopy == true) {
			for(int i = 0; i < Input.touchCount; i++) {
				if(miniMapCam.rect.Contains (new Vector2 (Input.GetTouch(i).position.x / Screen.width , Input.GetTouch(i).position.y / Screen.height))){
					if (Input.GetTouch(i).phase == TouchPhase.Moved){
						float movingX =miniMapCam.ScreenToWorldPoint ( Input.GetTouch(i).position).x;
						frame.transform.position = new Vector3 (movingX,0 , 0);
						//Debug.Log (frame.transform.position.x);
						
					}
					if (Input.GetTouch(i).phase == TouchPhase.Ended){
						copyFromPosX = miniMapCam.ScreenToWorldPoint ( Input.GetTouch(i).position).x;
						disToCopy = 700 - copyFromPosX;
						//copyPasteButtonA.enabled = false;
						//copyPasteButtonB.enabled = true;
						SavePosition ();
						float onScrPosX = Input.GetTouch(i).position.x;
						StartCoroutine (CaptureScreen(onScrPosX));
						Destroy(frame);
						isDraggingToCopy = false;


					}
					
				}
			}	
		}

		if (Input.touchCount > 0 && isDraggingToPaste== true) {
			for(int i = 0; i < Input.touchCount; i++) {
				if(miniMapCam.rect.Contains (new Vector2 (Input.GetTouch(i).position.x / Screen.width , Input.GetTouch(i).position.y / Screen.height))){
					if (Input.GetTouch(i).phase == TouchPhase.Moved){
						float movingX =miniMapCam.ScreenToWorldPoint ( Input.GetTouch(i).position).x;

						frame.transform.position = new Vector3 (movingX,0 , 0);
						//Debug.Log (frame.transform.position.x);
					}

					if (Input.GetTouch(i).phase == TouchPhase.Ended){
						copyFromPosX = miniMapCam.ScreenToWorldPoint ( Input.GetTouch(i).position).x;
						disToPaste = 700 - frame.transform.position.x;
						//copyPasteButtonA.enabled = true;
						//copyPasteButtonB.enabled = false;
						PasteAndClear(); 
						Destroy(frame);
						isDraggingToPaste = false;
					}					
				}
			}	
		}
	}

	public void StartToCopy(){
		frame = Instantiate (framePre, new Vector3 (0, 10000, 0), Quaternion.identity) as GameObject;
		frame.name = "copyFrame";
		//frame = copyAreaFrame;
	}

	public void DragToCopy(bool c){
		isDraggingToCopy = c;
	}

	public void SavePosition(){
		float X = frame.transform.position.x;
		float minX = X - 600;
		float maxX = X + 600;
		List<mapClass> wholeMapList = new List<mapClass>();
		int levelVersion = PlayerPrefs.GetInt("currentVersion");
		if (levelVersion == 2 || levelVersion == 0){
		wholeMapList = mainCamera.GetComponent<editorControl>().currentMap;
		}else if (levelVersion == 1 || levelVersion == 3){
			wholeMapList = mainCamera.GetComponent<editorControlBuildOnly>().currentMap;
		}
		//Debug.Log (wholeMapList.Count);
		foreach(mapClass block in wholeMapList){
			//Debug.Log(block.objLoc.x + "vs" + minX + "and" + maxX);
			if (block.objLoc.x >= minX && block.objLoc.x <= maxX){

				/*JPATT EDIT START */
				mapClass copiedWholeMapBlock = new mapClass(block.objName, block.index, block.objLoc); //Creating a new mapclass object so we're not directly referencing the old one
				copyMap.Add(copiedWholeMapBlock);
				/*JPATT EDIT END */

				//Debug.Log ("save position of blocks counts: " + copyMap.Count);
			}
		}
		//Debug.Log (copyMap.Count);

	}

	public void StartToPaste(){
		frame = Instantiate (framePre, new Vector3 (0, 10000, 0), Quaternion.identity) as GameObject;
		frame.name = "copyFrame";
	}

	public void DrageToPaste(bool p){
		isDraggingToPaste = p;
	}

	public void PasteAndClear(){

		float offsetX = disToCopy - disToPaste;
		for(int i = 0 ; i < copyMap.Count; i++ ) {

			//float cell_size = mainCamera.GetComponent<editorControl>().cell_size;
			float cell_size = 120f;
			float objX = Mathf.Round ((copyMap[i].objLoc.x + offsetX )  / cell_size) * cell_size;
			copyMap[i].objLoc.x = objX;
		}
		int levelVersion = PlayerPrefs.GetInt("currentVersion");
		if (levelVersion == 1 || levelVersion == 3){
			mainCamera.GetComponent<editorControlBuildOnly>().PasteCopyList(copyMap);
		}else if (levelVersion == 2){
			mainCamera.GetComponent<editorControl>().PasteCopyList(copyMap);
		}
		copyMap.Clear();
	}

	//Rect r;
	IEnumerator CaptureScreen( float copyX/*Bounds bound*/) {
		//isCapturing = true;
		yield return new WaitForEndOfFrame();
		//frame.GetComponent<SpriteRenderer>().bounds;
		//Debug.Log (frame.GetComponent<SpriteRenderer>().bounds.min.x + "," + frame.GetComponent<SpriteRenderer>().bounds.max.x);
		//Vector3 frameOrigin = miniMapCam.WorldToScreenPoint(new Vector3 (bound.min.x, bound.max.y, 0f));
		//Vector3 frameExtent = miniMapCam.WorldToScreenPoint(new Vector3 (bound.max.x, bound.min.y, 0f));
		//Debug.Log ("the origin x and y are " + frameOrigin.x + " , " + frameOrigin.y + " and the extent x and y are " + frameExtent.x + " , " + frameExtent.y);
		//Rect scrShotRect = new Rect(frameOrigin.x , Screen.height - frameOrigin.y , frameExtent.x - frameOrigin.x , frameOrigin.y - frameExtent.y);
		//Rect scrShotRect = new Rect(bound.min.x, bound.min.y + Screen.height * 0.92f, (bound.max.x - bound.min.x) * 0.08f, (bound.max.y - bound.min.y) * 0.08f);
		//Rect scrShotRect = new Rect ( bound.min.x, -Screen.height * 0.5f, temwidth, Screen.height);
		//Debug.Log (copyX);
		Rect scrShotRect = new Rect ( copyX - 20f, Screen.height * 0.92f, 895f, 53f);
		//Rect scrShotRect = new Rect ( copyX, 0f, Screen.height * 0.08f, Screen.height * 0.08f);  //this.camera.pixelRect;
		//Debug.Log (this.camera.pixelRect); // THIS ONE IS PERFECT!!!! .. WELL, NOT REALLY

		//r = scrShotRect;
		int imgSize = (int) Mathf.Round(this.camera.pixelHeight) ;
		Texture2D scrShot = new Texture2D( imgSize, imgSize , TextureFormat.RGB24, true);
			//this.camera.pixelWidth, this.camera.pixelHeight, TextureFormat.RGB24, true);   //Screen.width,Screen.height, TextureFormat.RGB24, true);
		scrShot.ReadPixels (scrShotRect, 0, 0);
		//scrShot.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
		scrShot.Apply();
		Sprite temSprite = Sprite.Create(scrShot, new Rect(0, 0, scrShot.width, scrShot.height), new Vector2(0.5f, 0.5f));
		butnImg.sprite = temSprite;
		//isCapturing = false;
	}
//	public float temleft, temtop, temwidth, temheight = 0f;
//	void OnDrawGizmosSelected(){
//		Gizmos.color = Color.red;
//		//Debug.Log ("draw gizmos");
//		Gizmos.DrawGUITexture(r, tempImg);
//		//Gizmos.DrawGUITexture(new Rect(temleft, temtop, temwidth, temheight), tempImg);
//	}

}
