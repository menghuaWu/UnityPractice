using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [Header("Player")]
    public GameObject Player;
    public AnimationClip Attack_clip;
    public GameObject fire_pos;
    public GameObject Arrow;
    public GameObject RedArrow;
    public GameObject GreenArrow;
    public int GreenArrow_count = 0;
    public int arrowType = 1;

    private bool CanCreateArrow;
    
    private RaycastHit hit;
    private Vector3 target_pos;
    private Vector3 look_pos;

    public float PlayerMaxHP = 100f;
    public static float PlayerHP = 100f;
    public static float PlayerMP = 100f;
    public float PlayerMaxMP = 100f;
    public Image imageHP;
    public Image imageMP;
    public Image NPC_VALUE;

    [Header("NPC")]
    public GameObject NPC_Start_Pos;
    public GameObject NPC;
    public float NPC_MAX_Num = 15f;
    public float NPC_MAX_COUNT;
    private float NPC_CUR_Num = 0;
    private float NPC_Real_Time = 0;
    private float NPC_Born_Time = 0;

    [Header("Magic")]
    public GameObject Magic_Obj_Ani;
    private bool bCreateMagic = true;
    //private float Magic_Cur_Time = 0;
    //private float Magic_Total_Time = 5;
    private GameObject Magic_Obj;
    private bool bCanDragMagic = true;


    int floorLayer;
    public static bool die = false;

    private void Awake()
    {
        Player.GetComponent<Animation>().clip = Attack_clip;
        Player.GetComponent<Animation>().Stop();
    }

    // Use this for initialization
    void Start () {
        floorLayer = LayerMask.GetMask("Floor");
        
        PlayerMaxHP = 100;
        PlayerHP = 100;
        PlayerMaxMP = 100;
        PlayerMP = 100;

        PlayerHP = PlayerMaxHP;
        PlayerMP = PlayerMaxMP;

        NPC_MAX_COUNT = NPC_MAX_Num;
    }
	
	// Update is called once per frame
	void Update () {
        
        CreatNPC();
        if (Input.GetMouseButton(0))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(camRay,out hit, 1000, floorLayer))
            {

                //target_pos = new Vector3(hit.point.x , Player.transform.position.y, hit.point.z);
                //look_pos = Vector3.Lerp(look_pos, target_pos, 1);
                //Player.transform.LookAt(look_pos);
                
                if (Magic_Obj && bCanDragMagic == true)
                {
                    if (hit.collider.CompareTag("floor"))
                    {
                        Magic_Obj.transform.position = hit.point;
                        
                        
                    }
                    
                }
                else
                {
                    
                    target_pos = new Vector3(hit.point.x, Player.transform.position.y, hit.point.z);
                    look_pos = Vector3.Lerp(look_pos, target_pos, 1);
                    Player.transform.LookAt(look_pos);
                    
                }
            }
            if (!Player.GetComponent<Animation>().isPlaying)
            {
                Player.GetComponent<Animation>().Stop();
                Player.GetComponent<Animation>().Play();
                CanCreateArrow = true;
            }
        }

        if (BtnEvent._press && PlayerMP >= 10)
        {
            if (bCreateMagic == true)
            {
                bCreateMagic = false;
                bCanDragMagic = true;

                Magic_Obj = Instantiate(Magic_Obj_Ani, new Vector3(200, 200, 200), Quaternion.identity) as GameObject;
            }
        }

        if (!BtnEvent._press)
        {
            if (Magic_Obj && bCreateMagic == false)
            {
                bCanDragMagic = false;
                bCreateMagic = true;

                Magic_Obj.BroadcastMessage("PlayMagicAnimation");
                PlayerMP = Mathf.Max(PlayerMP - 10, 0);
                PlayerMaxMP = PlayerMP;
                imageMP.fillAmount = PlayerMaxMP / 100;
            }
        }



        if (Player.GetComponent<Animation>().isPlaying)
        {
            Player.GetComponent<Animation>()["moon_att"].speed = 10f;
            if (Player.GetComponent<Animation>()["moon_att"].normalizedTime >= 0.5 && CanCreateArrow == true)
            {
                CanCreateArrow = false;
                if (Input.GetMouseButton(0) && arrowType == 1)
                {
                    CreatArrow();
                }else if (Input.GetMouseButton(0) && arrowType == 2)
                {
                    CreatRedArrow();
                }
                else if(Input.GetMouseButton(0) && arrowType == 3)
                {
                    CreatGreenArrow();
                }

            }
        }

        if (die)
        {
            NPC_MAX_COUNT--;            
            if (NPC_MAX_COUNT == 0)
            {
                NPC_VALUE.fillAmount = 0;
            }
            else
            {
                NPC_VALUE.fillAmount = NPC_MAX_COUNT / 15;
            }
            die = false;
        }

    }

    private void CreatGreenArrow()
    {
        for (int i=0;i<GreenArrow_count; i++)
        {
            float a = i * 15f;
            GameObject Fire;
            float n = 360 - (15 * GreenArrow_count) / 2;
            Fire = Instantiate(GreenArrow, fire_pos.transform.position, Player.transform.rotation) as GameObject;
            Fire.transform.Rotate(new Vector3(0, n + a, 0));
        }
    }

    void CreatArrow()
    {
        GameObject arrow = Instantiate(Arrow, fire_pos.transform.position, Player.transform.rotation) as GameObject;
        arrow.transform.Rotate(Vector3.up * 360);
    }

    void CreatRedArrow()
    {
        GameObject arrow = Instantiate(RedArrow, fire_pos.transform.position, Player.transform.rotation) as GameObject;
        arrow.transform.Rotate(Vector3.up * 360);
    }

    public void Change_Type(int val)
    {
        arrowType = val;
    }

    void CreatNPC()
    {
        if (NPC_CUR_Num < NPC_MAX_Num)
        {
            if (NPC_Real_Time >= NPC_Born_Time)
            {
                Vector3 Max_Start = NPC_Start_Pos.GetComponent<Collider>().bounds.max;
                Vector3 Min_Start = NPC_Start_Pos.GetComponent<Collider>().bounds.min;

                Vector3 Random_pos = new Vector3(UnityEngine.Random.Range(Min_Start.x, Max_Start.x), 0f, Max_Start.z);
                GameObject NPC_Prefab = null;

                Vector3 angles = transform.rotation.eulerAngles;
                angles.y = 180;

                NPC_Prefab = Instantiate(NPC, Random_pos, Quaternion.Euler(angles)) as GameObject;
                NPC_CUR_Num++;

                NPC_Born_Time = UnityEngine.Random.Range(1.5f, 3f);
                NPC_Real_Time = 0;

                
                

            }
            else
            {
                NPC_Real_Time = NPC_Real_Time + Time.deltaTime;
            }
        }
        
    }

    void CalculateHP(float att)
    {
        PlayerHP = Mathf.Max(PlayerHP - att, 0);
        PlayerMaxHP = PlayerHP;
        imageHP.fillAmount = PlayerMaxHP / 100 ;
        
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
    }

    public void ChangeScene(string scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
}
