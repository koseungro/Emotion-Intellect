﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sphere_rotate : MonoBehaviour
{
    public float Rotate_speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, Rotate_speed*Time.deltaTime, 0);
    }
}
