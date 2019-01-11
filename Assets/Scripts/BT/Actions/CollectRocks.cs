using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This state is the rock collecting state.
/// The penguin will go to this station and collect rocks.
/// Had to change how this talked to the agent script
/// Otherwise this window editor would never know if this script was active or not
/// </summary>
public class CollectRocks : Action
{
    private float rocksAmount;
    private bool gotRocks;
    [SerializeField]
    private Agent agent;

    public CollectRocks(Agent ownerBrain) : base(ownerBrain)
    {
        //Set the correct enum
        type = Type.ROCK;
        agent = ownerBrain;
    }

    public override BEHAVIOUR_STATUS Update()
    {
        agent.activeStates = this;

        //Active state as running for the window editor
        isActive = true;

        //Check whether enough resources has been collected if so
        //Go to the resource station
        //Slowly unload the rocks
        if (gotRocks)
        {
            agent.GetNavMesh().SetDestination(agent.resourceStation.transform.position);

            if (agent.transform.position.x == agent.resourceStation.transform.position.x)
            {
                rocksAmount -= 0.1f;
                Debug.Log(rocksAmount);
                if(rocksAmount <= 0)
                {
                    gotRocks = false;
                    isActive = false;
                    return BEHAVIOUR_STATUS.SUCCESS;
                }
            }
        }
        //Else checks the array of jobs to do and finds the one containing the name Rocks
        //Go to the appropriate station and collect rocks until amount is 25
        //Then deposit them
        else
        {
            for (int i = 0; i < agent.resource.Length; i++)
            {
                if (agent.resource[i].name == "Rocks")
                {
                    agent.GetNavMesh().SetDestination(agent.resource[i].transform.position);

                    if (agent.transform.position.x == agent.resource[i].transform.position.x)
                    {
                        if (rocksAmount >= 25)
                        {
                            gotRocks = true;
                        }
                        else
                        {
                            rocksAmount += 0.1f;
                            Debug.Log(rocksAmount);
                        }
                    }
                }
            }
        }
        return BEHAVIOUR_STATUS.RUNNING;
    }
}
