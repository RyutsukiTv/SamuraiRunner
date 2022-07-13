using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformGenerator : MonoBehaviour
{
    // Start is called before the first frame update



    public GameObject[] AllPatern;

    public float distanceBetween;

    public Transform Player;
    public Transform startPoint;
    public Transform generatorPoint;


    private float plateformWidth;
    private int RandomPaternId;



    void Start()
    {
        RandomPaternId =Random.Range(0, AllPatern.Length);
        plateformWidth = AllPatern[RandomPaternId].GetComponent<BoxCollider2D>().size.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < generatorPoint.position.x && startPoint.position.x < Player.position.x){
            transform.position = new Vector3(transform.position.x + plateformWidth + distanceBetween, transform.position.y, transform.position.z);
            Instantiate (AllPatern[RandomPaternId], transform.position, transform.rotation);
            RandomPaternId =Random.Range(0, AllPatern.Length);
        }
    }
}
