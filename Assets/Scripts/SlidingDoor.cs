using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Open Speed")]
    [Range(0, 10)] public float speed;

    [Header("Open Position")]
    public Transform targetPosition;

    private float distance;





    private void Update()
    {
        distance = Vector2.Distance(targetPosition.position, transform.position);

        if (GameManager.Instance.GetScore() == 4)
        {
            if (distance > 1)
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
        }




    }
}
