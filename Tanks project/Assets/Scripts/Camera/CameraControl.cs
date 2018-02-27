using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 //重新調整Camera對焦的時間       
    public float m_ScreenEdgeBuffer = 4f;           //視野範圍
    public float m_MinSize = 6.5f;                  //最小的Camera大小
    [HideInInspector] public Transform[] m_Targets; //螢幕涵蓋的所有Tank目標


    private Camera m_Camera;                        //Camera 參數物件
    private float m_ZoomSpeed;                      //Camera 縮放的速度(給SmoothDamp用)
    private Vector3 m_MoveVelocity;                 //Camera 移動速度(給SmoothDamp用)
    private Vector3 m_DesiredPosition;              //Camera 將要移動的位置


    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();//尋找子物件的<Camera>Conponemt(只有一個子物件的前提下)
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();
                                                                                                           //達到目標的時間設定
        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);//Camera的移動平滑處裡，可限制最大速度
                                                //目前所在位置      //目標位置         //當前的速度(是一個返回值)
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();                //
        int numTargets = 0;                                //Tank數量初始化
                                                           //
        for (int i = 0; i < m_Targets.Length; i++)         //
        {                                                  //
            if (!m_Targets[i].gameObject.activeSelf)       //如果Tank不是active則尋找下一個
                continue;                                  //
                                                           //
            averagePos += m_Targets[i].position;           //所有Tank位置相加
            numTargets++;                                  //Tank總數加1
        }                                                  //
                                                           //
        if (numTargets > 0)                                //如果Tank總數大於0，平均位置 = 位置總和 / Tank總數
            averagePos /= numTargets;                      //
                                                           //
        averagePos.y = transform.position.y;               //算出的平均位置Y一樣是原本的位置，所以將原本Y座標賦予算出的平均位置變數
                                                           //
        m_DesiredPosition = averagePos;                    //Cameera的朝向位置 = 算出的平均位置(Y一樣不變)
    }


    private void Zoom()
    {
        float requiredSize = FindRequiredSize();
        m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
    }


    private float FindRequiredSize()
    {
        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);//將平均座標轉換成本地座標(可想像成將本地當作父物件座標)

        float size = 0f;//初始化鏡頭的size

        for (int i = 0; i < m_Targets.Length; i++)
        {
            if (!m_Targets[i].gameObject.activeSelf)
                continue;

            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);//將所有Tank的座標轉換成本地座標

            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;//所有Tank和平均位置的距離

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.y));//取得size與|desiredPosToTarget.y|的最大值

            size = Mathf.Max (size, Mathf.Abs (desiredPosToTarget.x) / m_Camera.aspect);//取得|desiredPosToTarget.x| / aspect(固定的) 與size的最大值

            //可以想像成，距離中心點的X、Y哪個值較大就取得那個最大值當作螢幕的size
        }
        
        size += m_ScreenEdgeBuffer;//將算出的size加進原本的距離

        size = Mathf.Max(size, m_MinSize);//不要小於鏡頭的最小值

        return size;
    }


    public void SetStartPositionAndSize() //直接鏡頭切換，用於初始(無移動)
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}