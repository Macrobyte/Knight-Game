using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public new GameObject camera;
    public float parallaxEffect;
    private float length;
    private float startPos;

    public float variable;

    void Start()
    {    
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float dis = (camera.transform.position.x * parallaxEffect);
        float disy = (camera.transform.position.y * parallaxEffect + variable);

        transform.position = new Vector3(startPos + dis, startPos + disy, transform.position.z);
    }
}
