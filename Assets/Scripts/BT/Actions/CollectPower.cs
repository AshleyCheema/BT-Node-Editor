using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This state is the power collecting state.
/// The penguin will go to this station and collect power
/// Had to change how this talked to the agent script
/// Otherwise this window editor would never know if this script was active or not
/// </summary>
[System.Serializable]
public class CollectPower : Action
{
    private float powerAmount;
    private bool gotPower;
    [SerializeField]
    private Agent agent;

    public CollectPower(Agent ownerBrain) : base(ownerBrain)
    {
        //Set the correct enum
        type = Type.POWER;
        agent = ownerBrain;
    }

    public override BEHAVIOUR_STATUS Update()
    {
        agent.activeStates = this;

        //Active state as running for the window editor
        isActive = true;

        //Check whether enough resources has been collected if so
        //Go to the resource station
        //Slowly unload the power
        if (gotPower)
        {
            agent.GetNavMesh().SetDestination(agent.resourceStation.transform.position);

            if (agent.transform.position.x == agent.resourceStation.transform.position.x)
            {
                powerAmount -= 0.1f;
                Debug.Log(powerAmount);
                if (powerAmount <= 0)
                {
                    gotPower = false;
                    isActive = false;
                    return BEHAVIOUR_STATUS.SUCCESS;
                }
            }
        }
        //Else checks the array of jobs to do and finds the one containing the name Rocks
        //Go to the appropriate station and collect power until amount is 25
        //Then deposit them
        else
        {
            for (int i = 0; i < agent.resource.Length; i++)
            {
                if (agent.resource[i].name == "Power")
                {
                    agent.GetNavMesh().SetDestination(agent.resource[i].transform.position);

                    if (agent.transform.position.x == agent.resource[i].transform.position.x)
                    {
                        if (powerAmount >= 25)
                        {
                            gotPower = true;
                        }
                        else
                        {
                            powerAmount += 0.1f;
                            Debug.Log(powerAmount);
                        }
                    }
                }
            }
        }

        return BEHAVIOUR_STATUS.RUNNING;
    }
}
