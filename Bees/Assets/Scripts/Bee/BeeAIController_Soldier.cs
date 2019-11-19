using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeAIController_Soldier : BeeAIController
{

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        // When soldier.mainTarget is null and soldier.altTarget is null,
        // soldier.mainTarget is set to a random point near the hive.
        if (!mainTarget && !secondTarget)
        {
            mainTarget = hive.GetRandomPointFromHive(gameObject, minDistanceFromHive, maxDistanceFromHive);
        }
        else if (!mainTarget)
        {
            mainTarget = hive.transform;
        }
    }

    public override void TouchHive()
    {
        mainTarget = null;
        secondTarget = null;
    }

    void ReachedDestination()
    {
        base.ReachedDestination();
        if (secondTarget)
        {
            Debug.Log(secondTarget);
            // When the soldier reaches soldier.mainTarget and soldier.altTarget is NOT null,
            // soldier.mainTarget is set to a random point near the soldier and
            // soldier.altTarget is set to null
            float walkRadius = Random.Range(minDistanceFromHive, maxDistanceFromHive);
            Vector3 randomDirection;
            randomDirection = Random.insideUnitSphere * walkRadius;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
            GameObject tempTransform = new GameObject();
            tempTransform.name = name + "'s TempTarget";
            tempTransform.transform.position = hit.position;
            tempTransform.AddComponent<SphereCollider>();
            tempTransform.AddComponent<TempTarget>();
            tempTransform.GetComponent<TempTarget>().targetOwner = gameObject;

            mainTarget = tempTransform.transform;

            secondTarget = null;
        }
    }

    protected override void SeeSoldier(BeeAIController_Soldier other)
    {
        // When a soldier "sees" another soldier and other.altTarget is 
        // NOT null, soldier.mainTarget is set to other.altTarget.
        if (other.secondTarget)
        {
            mainTarget = other.secondTarget;
        }
    }

    protected override void SeePlayer(GameObject player)
    {
        base.SeePlayer(player);
        mainTarget = player.transform;
        secondTarget = player.transform;
    }
}
