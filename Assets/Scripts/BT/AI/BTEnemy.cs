using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTEnemy : BehaviourTree
{
    private Selector rootSelector;
    private Sequence charging;
    private SelectorRandom randomJob;
    //private Inverter checkInverter;
    private Parallel parallelDetection;

    private Recharge recharge;
    private NeedsCharging needsCharging;
    private CollectWood collectWood;
    private CollectRocks collectRocks;
    private CollectPower collectPower;


	public BTEnemy(Agent ownerBrain) : base(ownerBrain)
    {
        rootSelector = new Selector();
        charging = new Sequence();
        parallelDetection = new Parallel();
        //checkInverter = new Inverter();
        randomJob = new SelectorRandom();

        recharge = new Recharge(GetOwner());
        needsCharging = new NeedsCharging(GetOwner());
        collectWood = new CollectWood(GetOwner());
        collectRocks = new CollectRocks(GetOwner());
        collectPower = new CollectPower(GetOwner());

        ////////////////////////////////
        rootSelector.AddChild(charging);
        rootSelector.AddChild(randomJob);

        charging.AddChild(needsCharging);
        charging.AddChild(recharge);

        randomJob.AddChild(collectWood);
        randomJob.AddChild(collectRocks);
        randomJob.AddChild(collectPower);

        //A parallel to check for the player
        //checkInverter.AddChild(detection);

        //Patrol alongside checking for the player
        //parallelDetection.AddChild(patrolSequence);
        //patrolSequence.AddChild(patrol);
        //patrolSequence.AddChild(wait);
        //patrolSequence.AddChild(turnToPoint);
        //
        ////Root -> Adding sequences
        //rootSelector.AddChild(suspiciousSequence);
        //rootSelector.AddChild(ChaseSequence);
        //rootSelector.AddChild(DistractionSequence);
        //
        ////Distraction State
        ////DistractionSequence.AddChild(distraction);
        //DistractionSequence.AddChild(wait);
        //
        ////Suspicious state
        ////suspiciousSequence.AddChild(suspicious);
        //suspiciousSequence.AddChild(suspiciousAlert);
        ////suspiciousSequence.AddChild(moveToLKP);
        //suspiciousSequence.AddChild(wait);
        //
        ////Chase state
        ////ChaseSequence.AddChild(seen);
        ////ChaseSequence.AddChild(chase);
        //ChaseSequence.AddChild(wait);

    }

    // Update is called once per frame
    public override void Update ()
    {
        rootSelector.Update();
	}
}
