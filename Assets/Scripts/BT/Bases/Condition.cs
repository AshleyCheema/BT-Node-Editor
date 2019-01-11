using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition : Nodes
{

    private Agent nodeOwner;

    public Condition(Agent ownerBrain)
    {
        nodeOwner = ownerBrain;
    }
	
	// Update is called once per frame
	public override BEHAVIOUR_STATUS Update()
    {
        return BEHAVIOUR_STATUS.NONE;
	}

    public Agent GetOwner()
    {
        return nodeOwner;
    }
}
