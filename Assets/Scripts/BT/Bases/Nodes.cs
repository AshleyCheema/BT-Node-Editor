using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BEHAVIOUR_STATUS
{
    SUCCESS,
    FAILURE,
    RUNNING,
    NONE
}

//This is used to distinguish between the scripts
public enum Type
{
    POWER,
    ROCK,
    WOOD,
    RECHARGE,
    NEEDSCHARGING
}

[System.Serializable]
public class Nodes : MonoBehaviour
{
    //This is the bool used to change the window colour
    public bool isActive = false;

    public Type type = Type.POWER;

    private BEHAVIOUR_STATUS behaviourStatus;

    public Nodes()
    {
        behaviourStatus = BEHAVIOUR_STATUS.NONE;
    }

    public virtual BEHAVIOUR_STATUS Update()
    {
        return BEHAVIOUR_STATUS.NONE;
    }
    

}
