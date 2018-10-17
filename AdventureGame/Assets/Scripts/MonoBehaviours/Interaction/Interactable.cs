using UnityEngine;

public class Interactable : MonoBehaviour
{
    //全部的邏輯與事件
    //拿來去連結"判斷"與"反映"
    
    public Transform interactionLocation;//玩家到達此位置
    public ConditionCollection[] conditionCollections = new ConditionCollection[0];//連結"條件"
    public ReactionCollection defaultReactionCollection;


    public void Interact ()//判斷是否符合條件
    {
        for (int i = 0; i < conditionCollections.Length; i++)
        {
            if (conditionCollections[i].CheckAndReact ())
                return;
        }

        defaultReactionCollection.React ();//相對的反應
    }
}
