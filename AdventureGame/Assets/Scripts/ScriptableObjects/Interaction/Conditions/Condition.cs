using UnityEngine;

public class Condition : ScriptableObject
{
    //條件容器，儲存條件敘述，以及條件是否成立
    public string description;
    public bool satisfied;
    public int hash;
}