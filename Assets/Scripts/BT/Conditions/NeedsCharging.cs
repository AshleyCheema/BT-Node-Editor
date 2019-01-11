using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This checks whether the penguin is low on enery
/// </summary>
public class NeedsCharging : Condition
{
    [SerializeField]
    private Agent agent;

    public NeedsCharging(Agent ownerBrain) : base(ownerBrain)
    {
        type = Type.NEEDSCHARGING;
        agent = ownerBrain;
    }

    // Update is called once per frame
    public override BEHAVIOUR_STATUS Update()
    {
        agent.activeStates = this;
        //Active state as running for the window editor
        isActive = true;

        if (agent.energy <= 50.0f)
        {
            isActive = false;
            return BEHAVIOUR_STATUS.SUCCESS;
        }
        else
        {
            isActive = false;
        }
        return BEHAVIOUR_STATUS.FAILURE;
    }
}
