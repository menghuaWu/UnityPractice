using UnityEngine;

public class ConditionCollection : ScriptableObject
{

    public string description;//描述
    public Condition[] requiredConditions = new Condition[0];//所需要的條件
    public ReactionCollection reactionCollection;//會有甚麼反映

    public bool CheckAndReact()
    {
        for (int i = 0; i < requiredConditions.Length; i++)
        {
            if (!AllConditions.CheckCondition(requiredConditions[i]))
            {
                return false;
            }

            if (reactionCollection)
            {
                reactionCollection.React();
            }
        }
        return true;
    }
}