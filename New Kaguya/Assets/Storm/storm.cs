using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storm : MonoBehaviour
{
    public float StormSpeed;
    public Transform Player;
    public Transform StartPos;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.position.x >  StartPos.position.x){
            transform.position = Vector3.right * StormSpeed*Time.deltaTime + transform.position;
        }
    }
}
