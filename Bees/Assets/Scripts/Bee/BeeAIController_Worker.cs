﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeAIController_Worker : BeeAIController
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

        Debug.Log(name + "'s Update function.");

        // When worker.mainTarget is null, set worker.mainTarget to a random point far from the hive.
        if (!mainTarget)
        {
            mainTarget = hive.GetRandomPointFromHive(gameObject, minDistanceFromHive, maxDistanceFromHive);
        }
    }

    public override void TouchHive()
    {
        Debug.Log(name + " touched hive.");
        if (secondTarget)
        {
            Debug.Log(name + " reported " + secondTarget.name + " to the hive.");
            // The hive saves the location of the flower and the bee that submitted it. Then,
            // worker.mainTarget is set to a random flower (according to the hive). 
            hive.AddFlower(secondTarget.gameObject, beeMovement);
            mainTarget = hive.GetRandomFlower();
        }
        else if (hive.flowerList.Count != 0)
        {
            Debug.Log(name + " touched the hive while second target is null");
            // The hive "forgets" any flowers that worker has submitted. Then,
            // worker.mainTarget is set to a random flower (according to the hive).
            hive.ForgetFlowers(beeMovement);
            mainTarget = hive.GetRandomFlower();

        }
        else
        {
            Debug.Log(name + " touched the hive while flower list was empty.");
            mainTarget = null;
        }
    }

    void ReachedDestination()
    {
        base.ReachedDestination();
        if (secondTarget)
        {
            mainTarget = secondTarget;
            secondTarget = null;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log(name + " touched " + other.gameObject.name);
        if (other.gameObject.tag == "Flower")
        {
            mainTarget = hive.transform;
            secondTarget = other.transform;
        }
    }

}
