using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster_Controller : MonoBehaviour {

   
    public GameObject NPC_player;
    public float speed = 5;
    private float HP = 100;
    public float MaxHP = 100;
    private bool bHurt = false;
    private bool Attack = false;
    public AnimationClip[] Ani = new AnimationClip[3];
    public GameObject NPC_Soul;

    

    public Image NPC_HP;
    
    public AudioClip hitVoice;
    public AudioClip hitWall;
    private int NPC_ATT;
    public int NPC_ATT_MAX = 0;
    public int NPC_ATT_MIN = 0;
    private GameObject Player;

    // Use this for initialization
    void Start () {
        Player = GameObject.Find("GameManager");
        HP = MaxHP;
        NPC_player.GetComponent<Animation>().clip = Ani[0];
        NPC_player.GetComponent<Animation>().Play();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Translate(Vector3.forward * Time.deltaTime * speed,Space.Self);

        if(NPC_player.GetComponent<Animation>()[Ani[1].name.ToString()].normalizedTime >= 0.3)
        {
            if (Attack == false)
            {
                GetComponent<AudioSource>().clip = hitWall;
                GetComponent<AudioSource>().Play();
                Attack = true;
                NPC_ATT = Random.Range(NPC_ATT_MAX, NPC_ATT_MIN);
                Player.BroadcastMessage("CalculateHP", NPC_ATT);
                Invoke("Att", NPC_player.GetComponent<Animation>()[Ani[1].name.ToString()].length);
            }
        }

	}

    void Att()
    {
        Attack = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Defence_wall"))
        {
            speed = 0f;
            NPC_player.GetComponent<Animation>().Stop();
            NPC_player.GetComponent<Animation>().clip = Ani[1];
            NPC_player.GetComponent<Animation>().Play();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Defence_wall"))
        {
            if (!NPC_player.GetComponent<Animation>().isPlaying)
            {
            
                NPC_player.GetComponent<Animation>().Stop();
                NPC_player.GetComponent<Animation>().clip = Ani[1];
                NPC_player.GetComponent<Animation>().Play();

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Defence_wall"))
        {
            speed = 5f;
            NPC_player.GetComponent<Animation>().Stop();
            NPC_player.GetComponent<Animation>().clip = Ani[0];
            NPC_player.GetComponent<Animation>().Play();
        }
    }

    void NPC_Hurt(float att)
    {
        if (bHurt == false)
        {
            bHurt = true;
            GetComponent<AudioSource>().clip = hitVoice;
            GetComponent<AudioSource>().Play();
            transform.Translate(Vector3.back * Time.deltaTime * 40f);
            HP -= att;
            MaxHP = HP;
            NPC_HP.fillAmount = MaxHP / 100;
            if (HP <=0)
            {
                GetComponent<Collider>().enabled = false;
                speed = 0;

                NPC_player.GetComponent<Animation>().Stop();
                NPC_player.GetComponent<Animation>().clip = Ani[2];
                NPC_player.GetComponent<Animation>().Play();
                Vector3 soul = new Vector3(transform.position.x, transform.position.y+2, transform.position.z-2);
                Instantiate(NPC_Soul, soul, Quaternion.identity);
                Destroy(gameObject, 2);
                

            }
        }
        bHurt = false;
    }

    private void OnDestroy()
    {
        GameManager.die = true;
        
    }
}
