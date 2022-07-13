using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stormApproching : MonoBehaviour
{
    public float StormSpeed;
    public Transform Player;
    public Transform startPoint;

    public PlayerController PlayerController;
    // Update is called once per frame
    void Update()
    {
        if(Player.position.x > startPoint.position.x){
            transform.position += new Vector3(StormSpeed* Time.deltaTime,0,0);
        }
        if(PlayerController.movementSpeed == 0){
            StormSpeed = 0;
        }
    }
}
