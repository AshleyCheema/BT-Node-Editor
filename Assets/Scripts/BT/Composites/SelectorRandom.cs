using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorRandom : Composite
{
    private bool isCompleted = true;
    private Nodes currentNode;

    public SelectorRandom()
    {
        Init();
    }

    public override BEHAVIOUR_STATUS Update()
    {
        BEHAVIOUR_STATUS returnStatus = BEHAVIOUR_STATUS.FAILURE;

        if(isCompleted == true)
        {
            currentNode = GetChildBehaviours()[Random.Range(0, GetChildBehaviours().Count)];
            isCompleted = false;
        }

        BEHAVIOUR_STATUS behaviourStatus = currentNode.Update();

        if (behaviourStatus == BEHAVIOUR_STATUS.FAILURE)
        {
            returnStatus = BEHAVIOUR_STATUS.FAILURE;
        }
        else
        {
            returnStatus = behaviourStatus;
        }

        if (returnStatus == BEHAVIOUR_STATUS.SUCCESS || returnStatus == BEHAVIOUR_STATUS.FAILURE)
        {
            isCompleted = true;
            Reset();
        }

        return returnStatus;
    }
}
