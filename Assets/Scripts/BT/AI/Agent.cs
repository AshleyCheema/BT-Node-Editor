using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Agent : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private BTEnemy behaviourTree;
    public Nodes activeStates;
    public float energy = 100.0f;
    public GameObject rechargeStation;
    public GameObject resourceStation;
    public GameObject[] resource;
    public bool charging = false;


    // Use this for initialization
    void Start()
    {
        behaviourTree = new BTEnemy(this);
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        activeStates = gameObject.GetComponentInChildren<Nodes>();
    }

    // Update is called once per frame
    void Update()
    {
        behaviourTree.Update();
        //If it's in the charging station I don't want him to lose energy
        if(!charging)
        {
            energy -= Time.deltaTime;
        }
        Debug.Log(activeStates);
    }

    public NavMeshAgent GetNavMesh()
    {
        return navAgent;
    }

    public Transform Position()
    {
        return gameObject.transform;
    }

    public Nodes Blackboard()
    {
        return activeStates;
    }

    //When the robot is low this will slowly increase his energy levels
    public void Recharging()
    {
        charging = true;
        if (energy < 100)
        {
            energy += 0.5f;
        }
        if (energy >= 100)
        {
            energy = 100;
        }
    }

    //public int Wood(int wood)
    //{
    //    return wood;
    //}
    //
    //public int Rocks(int rocks)
    //{
    //    return rocks;
    //}
    //
    //public int Power(int power)
    //{
    //    return power;
    //}
}