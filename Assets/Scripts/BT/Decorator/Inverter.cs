using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inverter : Decorator
{
    public Inverter()
    {

    }

    public override BEHAVIOUR_STATUS Update()
    {
        Nodes currentNode = GetChildBehaviour();

        BEHAVIOUR_STATUS currentBehaviour = currentNode.Update();

        if(currentBehaviour == BEHAVIOUR_STATUS.SUCCESS)
        {
            return BEHAVIOUR_STATUS.FAILURE;
        }

        else if(currentBehaviour == BEHAVIOUR_STATUS.FAILURE)
        {
            return BEHAVIOUR_STATUS.SUCCESS;
        }
        else
        {
            return currentBehaviour;
        }
    }
}
