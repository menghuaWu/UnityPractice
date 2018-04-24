using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour {

    public float att = 30f;
    public float moveSpeed = 15f;
    public GameObject fire_part;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        //transform.Translate(Vector3.forward*moveSpeed*Time.deltaTime);
        transform.Translate(transform.forward*moveSpeed*Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("NPC"))
        {
            other.gameObject.SendMessage("NPC_Hurt",att);

            Destroy(gameObject);
            Instantiate(fire_part, transform.position, Quaternion.identity);
        }
    }
}
