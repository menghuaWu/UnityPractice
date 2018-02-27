using System;
using UnityEngine;

[Serializable]//資料序列化，為了在Inspector顯示公共變數
public class TankManager
{
    //因為受到GameManager控制所以不用繼承Mono，也不用拖曳到物件上
    /*
     在不同的階段中，受控於GameManager，並去控制Tank
         */
    public Color m_PlayerColor; //Tank顏色            
    public Transform m_SpawnPoint; //Tank生成點 
    [HideInInspector] public int m_PlayerNumber;//指定幾號玩家             
    [HideInInspector] public string m_ColoredPlayerText;//字串，代表符合該顏色的坦克
    [HideInInspector] public GameObject m_Instance;    //代表GameManager生成的Tank     
    [HideInInspector] public int m_Wins;//玩家贏的次數                  


    private TankMovement m_Movement;  //開啟movent腳本(Enable or Disable)     
    private TankShooting m_Shooting;  //開啟涉及腳本(Enable or Disable)
    private GameObject m_CanvasGameObject;//用於某個階段中是否要顯示Canvas


    public void Setup()
    {
        m_Movement = m_Instance.GetComponent<TankMovement>();//取得腳本
        m_Shooting = m_Instance.GetComponent<TankShooting>();//取得腳本
        m_CanvasGameObject = m_Instance.GetComponentInChildren<Canvas>().gameObject;

        m_Movement.m_PlayerNumber = m_PlayerNumber;
        m_Shooting.m_PlayerNumber = m_PlayerNumber;

        m_ColoredPlayerText = "<color=#" + ColorUtility.ToHtmlStringRGB(m_PlayerColor) + ">PLAYER " + m_PlayerNumber + "</color>";

        MeshRenderer[] renderers = m_Instance.GetComponentsInChildren<MeshRenderer>();

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].material.color = m_PlayerColor;
        }
    }

    //沒有控制Tank的時候
    public void DisableControl()
    {
        m_Movement.enabled = false;//腳本關閉
        m_Shooting.enabled = false;//腳本關閉

        m_CanvasGameObject.SetActive(false);//Canvas關閉
    }

    //開始控制Tank的時候
    public void EnableControl()
    {
        m_Movement.enabled = true;//腳本打開
        m_Shooting.enabled = true;

        m_CanvasGameObject.SetActive(true);//Canvas打開
    }

    //遊戲一開始的默認狀態
    public void Reset()
    {
        m_Instance.transform.position = m_SpawnPoint.position;
        m_Instance.transform.rotation = m_SpawnPoint.rotation;

        m_Instance.SetActive(false);
        m_Instance.SetActive(true);
    }
}
