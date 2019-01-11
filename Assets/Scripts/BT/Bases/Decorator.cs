using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decorator : Nodes
{
    Nodes childNode;

    public Decorator()
    {

    }
	
	// Update is called once per frame
	public override BEHAVIOUR_STATUS Update ()
    {
        return BEHAVIOUR_STATUS.NONE;
	}

    public void AddChild(Nodes nodes)
    {
        childNode = nodes;
    }

    public Nodes GetChildBehaviour()
    {
        return childNode;
    }
}
 