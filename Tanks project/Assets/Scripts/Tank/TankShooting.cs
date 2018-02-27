using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;             //玩家號碼
    public Rigidbody m_Shell;                  //子彈Prefab
    public Transform m_FireTransform;          //產生子彈的位置
    public Slider m_AimSlider;                 //顯示AimSlider
    public AudioSource m_ShootingAudio;        //播放音效(可想像為撥放器)
    public AudioClip m_ChargingClip;           //集氣音效
    public AudioClip m_FireClip;               //發射音效
    public float m_MinLaunchForce = 15f;       //最一開始(沒有集氣)的開火威力
    public float m_MaxLaunchForce = 30f;       //最大開火威力
    public float m_MaxChargeTime = 0.75f;      //從最小至最大威力的間隔時間

    
    private string m_FireButton;         
    private float m_CurrentLaunchForce;  
    private float m_ChargeSpeed;         
    private bool m_Fired;                


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce; //初始化指示條和當前作用力
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;//不同玩家，不同開火按鍵

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;//旭麗速度為20
    }
    

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce; //每個Frame一開始設定蓄力為最小值
        if (m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired) //如果當前蓄力值 >= 最大蓄力值，並且處於未發射狀態
        {
            //at max charge，not yet fired
            m_CurrentLaunchForce = m_MaxLaunchForce; //當前蓄力值 = 最大蓄力值
            Fire(); //開火
        }
        else if(Input.GetButtonDown(m_FireButton)) //當按下開火按鍵時(按下的瞬間) //可視為發射狀態的初始化
        {
            //have we pressed fire for the first time?
            m_Fired = false; //開火狀態設定為否定
            m_CurrentLaunchForce = m_MinLaunchForce; //當前蓄力 = 最小蓄力

            m_ShootingAudio.clip = m_ChargingClip; //發射撥放器設定為蓄力音效
            m_ShootingAudio.Play();//撥放音效
        }
        else if (Input.GetButton(m_FireButton) && !m_Fired) //長按按鍵時，並且未開火狀態(蓄力狀態)
        {
            //Holding the fire buttom，not yet fire
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime; //當前蓄力 = 速度 * smooth時間
            m_AimSlider.value = m_CurrentLaunchForce; //指示條長度 = 當前蓄力值
        }
        else if (Input.GetButtonUp(m_FireButton) && !m_Fired) //放開按鍵時，並且未開火狀態
        {
            //we released the button，having not fire yet 
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        Rigidbody shellInstance = Instantiate(m_Shell, m_FireTransform.position, m_FireTransform.rotation) as Rigidbody;
            //在開火位置生成子彈，並取得子彈的rigidbody(因為前面變數引用rigidbody，所以不用Getconponent，改用as)
        shellInstance.velocity = m_FireTransform.forward * m_CurrentLaunchForce; //開火威力

        m_ShootingAudio.clip = m_FireClip;
        m_ShootingAudio.Play();

        m_CurrentLaunchForce = m_MinLaunchForce;
    }
}