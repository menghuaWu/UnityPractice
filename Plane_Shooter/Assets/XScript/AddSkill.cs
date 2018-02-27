using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSkill : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            if (GameObject.Find("ApplicationGameMaker").GetComponent<GameManager>().useSkill == false)
            {
                GameObject.Find("ApplicationGameMaker").GetComponent<GameManager>().useSkill = true;
                Destroy(gameObject);
            }
            else
            {
                return;
            }
            
        }
        else
        {
            return;
        }
    }
}
