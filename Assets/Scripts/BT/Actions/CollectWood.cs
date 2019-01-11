using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This state is the wood collecting state.
/// The penguin will go to this station and collect wood
/// Had to change how this talked to the agent script
/// Otherwise this window editor would never know if this script was active or not
/// </summary>
public class CollectWood : Action
{
    private float woodAmount;
    private bool gotWood; //pun intended
    [SerializeField]
    private Agent agent;

    public CollectWood(Agent ownerBrain) : base(ownerBrain)
    {
        //Set the correct enum
        type = Type.WOOD;
        agent = ownerBrain;
    }

    public override BEHAVIOUR_STATUS Update()
    {
        agent.activeStates = this;

        //Active state as running for the window editor
        isActive = true;

        //Check whether enough resources has been collected if so
        //Go to the resource station
        //Slowly unload the wood
        if (gotWood)
        {
            agent.GetNavMesh().SetDestination(agent.resourceStation.transform.position);

            if (agent.transform.position.x == agent.resourceStation.transform.position.x)
            {
                woodAmount -= 0.1f;
                Debug.Log(woodAmount);
                if (woodAmount <= 0)
                {
                    gotWood = false;
                    isActive = false;
                    return BEHAVIOUR_STATUS.SUCCESS;
                }
            }
        }
        //Else checks the array of jobs to do and finds the one containing the name Rocks
        //Go to the appropriate station and collect wood until amount is 25
        //Then deposit them
        else
        {
            for (int i = 0; i < agent.resource.Length; i++)
            {
                if (agent.resource[i].name == "Wood")
                {
                    agent.GetNavMesh().SetDestination(agent.resource[i].transform.position);

                    if (agent.transform.position.x == agent.resource[i].transform.position.x)
                    {
                        if (woodAmount >= 25)
                        {
                            gotWood = true;
                        }
                        else
                        {
                            woodAmount += 0.1f;
                            Debug.Log(woodAmount);
                        }
                    }
                }
            }
        }

        return BEHAVIOUR_STATUS.RUNNING;
    }
}
