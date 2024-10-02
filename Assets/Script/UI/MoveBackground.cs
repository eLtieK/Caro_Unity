using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    public float moveSpeed;
    public void Reset()
    {
        if (transform.position.x <= -100)
        {
            transform.position = new Vector3(1060*2, transform.position.y);
        }
    }
    void Update()
    {
        transform.position = transform.position + (Vector3.left * moveSpeed) * Time.deltaTime;
        Reset();
    }
}
