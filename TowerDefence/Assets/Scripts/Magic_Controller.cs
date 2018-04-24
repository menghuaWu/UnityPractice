using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic_Controller : MonoBehaviour {

    public int att;
    public bool Play_Water_SFX = false;
    public AudioClip magic_sound;
	// Use this for initialization
	void Start () {
        GetComponent<Animation>().Stop();
	}

    void PlayMagicAnimation()
    {
        GetComponent<Animation>().Play("Dragon");
        GetComponent<AudioSource>().clip = magic_sound;
        GetComponent<AudioSource>().Play();
        Destroy(gameObject.transform.parent.gameObject, 2.5f);
    }
	
	// Update is called once per frame
	void Update () {
        if (GetComponent<Animation>().IsPlaying("Dragon"))
        {
            float step = GetComponent<Animation>()["Dragon"].normalizedTime;
            if (step >= 0.45)
            {
                if (Play_Water_SFX == false)
                {
                    Play_Water_SFX = true;

                    gameObject.transform.parent.gameObject.BroadcastMessage("PlayWaterPartical");
                }
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPC"))
        {
            other.BroadcastMessage("NPC_Hurt", att);
        }
    }
}
