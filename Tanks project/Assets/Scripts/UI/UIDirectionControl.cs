using UnityEngine;

public class UIDirectionControl : MonoBehaviour
{
    public bool m_UseRelativeRotation = true;  //�O�_�ϥά۹���ਤ��


    private Quaternion m_RelativeRotation;    //�@�}�l�����ਤ�� 


    private void Start()
    {
        m_RelativeRotation = transform.parent.localRotation;//�@�}�l�����o�����󪺱��ਤ��(�u����@��)
    }


    private void Update()
    {
        if (m_UseRelativeRotation) 
            transform.rotation = m_RelativeRotation; //���a���ਤ�� = �����󪺱���(�����@�ӱ��ਤ��)
    }
}
