using UnityEngine;

public abstract class Reaction : ScriptableObject
{
    //各種反應行為
    //被繼承，進而創造出各種不同情況的反應
    public void Init ()
    {
        SpecificInit ();
    }


    protected virtual void SpecificInit()
    {}


    public void React (MonoBehaviour monoBehaviour)
    {
        ImmediateReaction ();
    }


    protected abstract void ImmediateReaction ();
}
