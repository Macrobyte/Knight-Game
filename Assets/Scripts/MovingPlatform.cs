using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Platform Speed")]
    [Range(0,10)]public float platformSpeed;

    [Header("Target Positions")]
    public Transform pos1;
    public Transform pos2;
    public Transform startPos;

    private Vector3 nextPosition;

    private void Start()
    {
        nextPosition = startPos.position;
        
    }

    private void Update()
    {

        if (transform.position == pos1.position)
        {
            nextPosition = pos2.position;
        }

        if(transform.position == pos2.position)
        {
            nextPosition = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, nextPosition, platformSpeed * Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }

}
