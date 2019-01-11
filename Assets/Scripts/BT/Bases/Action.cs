using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action : Nodes
{
    private Agent nodeOwnerBrain;

    public Action(Agent ownerBrain)
    {
        nodeOwnerBrain = ownerBrain;
    }
	
	// Update is called once per frame
	public override BEHAVIOUR_STATUS Update()
    {
        return BEHAVIOUR_STATUS.NONE;
	}

    public Agent GetOwner()
    {
        return nodeOwnerBrain;
    }
}
