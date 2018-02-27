using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;              
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);//在某範圍內的某個Layer中，所有物體都會被碰撞
                                                    //中心點            //半徑             //Layer

        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();

            if (!targetRigidbody)
                continue;

            targetRigidbody.AddExplosionForce(m_ExplosionForce, transform.position, m_ExplosionRadius);
                                            //威力(隨著距離減少)//爆炸位置          //爆炸半徑

            TankHealth targetHealth = targetRigidbody.GetComponent<TankHealth>();//取得目標物的<TankHealth>Script元件

            if (!targetHealth)
                continue;

            float damage = CalculateDamage(targetRigidbody.position);//藉由距離計算傷害

            targetHealth.TakeDamage(damage);//傳入計算完的損血值到<TankHeaith>腳本的TakeDamage方法
        }

        m_ExplosionParticles.transform.parent = null;

        m_ExplosionParticles.Play();

        m_ExplosionAudio.Play();

        Destroy(m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

        Destroy(gameObject);


    }


    private float CalculateDamage(Vector3 targetPosition)
    {
        // Calculate the amount of damage a target should take based on it's position.
        Vector3 explosionToTarget = targetPosition - transform.position;//算出兩點的向量距離(偵測目標位置 - 本地位置)

        float explositionDistance = explosionToTarget.magnitude;//返回向量的長度(sqrMagnitude CPU較快)

        float relativeDistance = (m_ExplosionRadius - explositionDistance) / m_ExplosionRadius;//算出爆炸範圍與距離的比例關係

        float damage = relativeDistance * m_MaxDamage; //相對比例 * 最大傷害 = 具體傷害

        damage = Mathf.Max(0f, damage);//超出爆炸範圍為負值，為了不讓傷害為負，半段是否大於0
        //取得0代表超出範圍，扣0血，反之亦然

        return damage;

    }
}