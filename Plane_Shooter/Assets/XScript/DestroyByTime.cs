﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

    public float _time = 1f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, _time);	
	}
}
