using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{

    public Transform target;
    public float yOffset  = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Set the position of the health bar slightly above the player
            Vector3 targetPosition = target.position + Vector3.up * yOffset;
            transform.position = Camera.main.WorldToScreenPoint(targetPosition);
        }
    }
}
