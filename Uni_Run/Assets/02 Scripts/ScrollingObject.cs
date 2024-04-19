using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    public float speed = 10.0f;


    void Start()
    {
        
    }

    void Update()
    {
        if(!GameManager.instance.isGameOver)
        {
            transform.Translate(Vector3.left *  speed * Time.deltaTime);
        }
    }
}
