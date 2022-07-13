using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class distance : MonoBehaviour
{

    public GameObject startPos;
    public Text scoretext;
    public Text scoretextEnd;
    public GameObject scoreTextObj;

    public Text BestScore;



    private float distanceCalculate;
    private static float distanceRecord = 0;

    public PlayerController PlayerController;


    // Start is called before the first frame update
    void Start()
    {
        scoretext = scoreTextObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x>startPos.transform.position.x && PlayerController.canMove){
            distanceCalculate = (startPos.transform.position.x + this.transform.position.x);
            scoretext.text = distanceCalculate.ToString("F1")+"m";
            scoretextEnd.text = "score: "+distanceCalculate.ToString("F1")+"m";

            distanceRecord = PlayerController.movementSpeed;
        }
        //if(distanceCalculate>distanceRecord){
        //        distanceRecord = distanceCalculate;
        //        BestScore.text = "best score: "+ distanceCalculate.ToString("F1")+"m";
        //}
    }
}
