using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Composite : Nodes
{
    private List<Nodes> childNodes;
    protected int currentIndex = 0;

    public List<Nodes> GetChildBehaviours()
    {
        return childNodes;
    }

    public void AddChild(Nodes newChild)
    {
        childNodes.Add(newChild);
    }

    protected void Reset()
    {
        currentIndex = 0;
    }

    protected void Init()
    {
        currentIndex = 0;
        childNodes = new List<Nodes>();
    }

}
