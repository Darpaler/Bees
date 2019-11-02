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
            mainTarget = hive.GetRandomPointFromHive(gameObject, minDistanceFromHive, maxDistanceFromHive);
        }
    }

    public override void TouchHive()
    {
        Debug.Log("Touched Hive");
        if (secondTarget)
        {
            // Todo: the hive saves the location of the flower and the bee that submitted it. Then,
            // Todo: worker.mainTarget is set to a random flower (according to the hive). 
        }
        else if (false)
        {
            // Todo:  the hive "forgets" any flowers that worker has submitted. Then,
            // Todo: worker.mainTarget is set to a random flower (according to the hive).
        }
        else
        {
            mainTarget = null;
        }
    }
}
