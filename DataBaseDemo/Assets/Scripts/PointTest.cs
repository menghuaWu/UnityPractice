using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointTest : MonoBehaviour {

    public int score;
	// Use this for initialization
	void Start () {
        LeaderBoard._instance.NewScore(score);
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
