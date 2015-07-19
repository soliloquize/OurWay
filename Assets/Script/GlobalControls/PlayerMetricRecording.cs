using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
public class PlayerMetricRecording : MonoBehaviour {
    
    List<Vector2> playerPositions = new List<Vector2>();
    List<Vector2> playerBuilderScore = new List<Vector2>();
    
    public Transform player, builder;
    
    public string uploadScoreUrl="http://www.jun610.com/ourway/metricUpload.php"; //be sure to add a ? to your url
    
    
    void Start(){
        InvokeRepeating("DoRecord", 0f, 1f);
		InvokeRepeating("DoSave", 0f, 10f);
    }

    void Update(){
        if(Input.GetKeyDown (KeyCode.Space)){
            DoSave();
        }
    }
    
    void DoRecord(){
        //happens every 1 seconds
        Vector2 currentPlayerPosition = new Vector2(player.position.x, player.position.y);
        
        /*Vector2 currentPlayerBuilderScore = new Vector2(
            playe.score,
            builderScore.score
            );
        */
 /*       playerPositions.Add(currentPlayerPosition);
        //playerBuilderScore.Add(currentPlayerBuilderScore);
    }
    
    void DoSave(){
        //save the data
        string mysaveData = GetVector2AsString(playerPositions);
        StartCoroutine(UploadMetrics(mysaveData,"aab2"));

    }
    
    string GetVector2AsString(List<Vector2> inVector2List){
        string returnString = "POSITIONS: \n";
        foreach(Vector2 v in inVector2List){
            returnString = returnString + "x * y" + ",";
            
        }
        Debug.Log (returnString);
        return returnString;
    }
    
    
    IEnumerator UploadMetrics(string inMetrics, string inID) {

		//CancelInvoke("DoRecord");
        WWWForm form = new WWWForm();
        form.AddField("action", "telemetry");
        
        form.AddField("metrics", inMetrics.ToString() ); //this could be from Player Settings
        form.AddField ("name", inID);

        WWW w = new WWW(uploadScoreUrl,form);
        yield return w;
        if (w.error != null) {
            Debug.Log(w.error);
        } else {
            Debug.Log ("Successful Upload!");
            Debug.Log (w.text + "\n\n" + form.ToString());
        }  

		playerPositions.Clear();
		//InvokeRepeating("DoRecord", 0f, 1f);
    }

}
*/
