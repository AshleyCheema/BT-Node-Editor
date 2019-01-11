using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Composite
{
    public Parallel()
    {
        Init();
    }


    // Update is called once per frame
    public override BEHAVIOUR_STATUS Update()
    {
        BEHAVIOUR_STATUS returnStatus = BEHAVIOUR_STATUS.FAILURE;

        for (int i = 0; i < GetChildBehaviours().Count; i++)
        {
            Nodes currentBehaviour = GetChildBehaviours()[i];
            returnStatus = currentBehaviour.Update();

            if(returnStatus == BEHAVIOUR_STATUS.SUCCESS)
            {

            }
            else
            {
                break;
            }
        }

        if (returnStatus == BEHAVIOUR_STATUS.SUCCESS || returnStatus == BEHAVIOUR_STATUS.FAILURE)
        {
            Reset();
        }

        return returnStatus;
    }
}