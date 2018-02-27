using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    public float m_StartingHealth = 100f;           //一開始的血量
    public Slider m_Slider;                         //Slider 物件
    public Image m_FillImage;                       //Fill 物件
    public Color m_FullHealthColor = Color.green;   //滿寫 顏色
    public Color m_ZeroHealthColor = Color.red;     //0寫 顏色
    public GameObject m_ExplosionPrefab;            //爆炸特效
                                                    
                                                    
    private AudioSource m_ExplosionAudio;           //爆炸音效
    private ParticleSystem m_ExplosionParticles;    //爆炸的粒子系統
    private float m_CurrentHealth;                  //當前的血量
    private bool m_Dead;                            //是否死亡


    private void Awake()
    {
        m_ExplosionParticles = Instantiate(m_ExplosionPrefab).GetComponent<ParticleSystem>();//粒子系統指向複製出來的爆炸特效並取得<ParticalSysrem> (因為爆炸為Prefab)
        m_ExplosionAudio = m_ExplosionParticles.GetComponent<AudioSource>();//取得爆炸音效

        m_ExplosionParticles.gameObject.SetActive(false);//複製出來的爆炸先隱藏
    }


    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;//當前寫量為滿血狀態
        m_Dead = false;//死亡狀態為否

        SetHealthUI();//設定血量UI
    }
    

    public void TakeDamage(float amount)//損血，傳入要扣掉的血量值
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount;//當前血量扣掉傳入的值
        SetHealthUI();//設定血量UI
        if (m_CurrentHealth <= 0 && !m_Dead) //如果當前血量小於0，且目前不是死亡狀態
        {
            //去死吧
            OnDeath();
        }

    }


    private void SetHealthUI()
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = m_CurrentHealth;
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth/m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;
        m_ExplosionParticles.transform.position = transform.position;
        m_ExplosionParticles.gameObject.SetActive(true);
        m_ExplosionParticles.Play();
        m_ExplosionAudio.Play();
        gameObject.SetActive(false);
    }
}