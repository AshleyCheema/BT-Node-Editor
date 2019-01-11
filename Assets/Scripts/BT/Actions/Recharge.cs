
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This state actives if the penguin needs charging
/// If the penguin needs charging it will return to the charging station
/// And wait there until it is at 100 energy again
/// </summary>
public class Recharge : Action
{
    [SerializeField]
    private Agent agent;

    public Recharge(Agent ownerBrain) : base(ownerBrain)
    {
        //Set the correct enum
        type = Type.RECHARGE;
        agent = ownerBrain;
    }

    public override BEHAVIOUR_STATUS Update()
    {
        agent.activeStates = this;

        //Active state as running for the window editor
        isActive = true;

        agent.GetNavMesh().SetDestination(agent.rechargeStation.transform.position);

        if (agent.transform.position.x == agent.rechargeStation.transform.position.x)
        {
            if (agent.energy == 100)
            {
                agent.charging = false;
                isActive = false;
                return BEHAVIOUR_STATUS.SUCCESS;
            }
            else
            {
                agent.Recharging();
            }
        }
        return BEHAVIOUR_STATUS.RUNNING;
    }
}
