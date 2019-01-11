using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourTree
{
    private Agent nodeOwnerBrain;

    public BehaviourTree(Agent ownerBrain)
    {
        nodeOwnerBrain = ownerBrain;
    }

    public virtual void Update() { }
    public virtual void Interrupt() { }

    protected Agent GetOwner()
    {
        return nodeOwnerBrain;
    }
}
