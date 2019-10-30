using System.Collections;
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

        // When worker.mainTarget is null, set worker.mainTarget to a random point far from the hive.
        if (!mainTarget)
        {
            Debug.Log("main is null");
            mainTarget = hive.GetRandomPointFromHive(minDistanceFromHive, maxDistanceFromHive);
        }
    }
}
