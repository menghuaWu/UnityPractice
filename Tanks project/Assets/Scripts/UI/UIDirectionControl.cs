using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;  //是否使用相對旋轉角度


    private Quaternion m_RelativeRotation;    //一開始的旋轉角度 


    private void Start()
    {
        m_RelativeRotation = transform.parent.localRotation;//一開始先取得父物件的旋轉角度(只執行一次)
    }


    private void Update()
    {
        if (m_UseRelativeRotation) 
            transform.rotation = m_RelativeRotation; //本地旋轉角度 = 父物件的旋轉(維持一個旋轉角度)
    }
}
