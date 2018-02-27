using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public GameObject _player; //target object
    private Vector3 offset; // distance

	// Use this for initialization
	void Start () {
        offset = transform.position - _player.transform.position; //Distance = this transform.posion - target.transform.position
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = _player.transform.position + offset; // this transform.position = target transform.position + distance in every frame
	}
}
