using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateformDestroyer : MonoBehaviour
{

    public GameObject PlateFormDestructionPoint;



    // Start is called before the first frame update
    void Start()
    {
        PlateFormDestructionPoint = GameObject.Find(
            "PlateformDestructionPoint"
        );
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < PlateFormDestructionPoint.transform.position.x){
            Destroy (gameObject);
        }
    }
}
