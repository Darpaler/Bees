using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BeeAIController_Queen : BeeAIController
{
    // Variables
    

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();

        // When queen.mainTarget is null, set queen.mainTarget to a the hive.
        if (!mainTarget)
        {
            mainTarget = hive.transform;
        }
    }

    protected override void SeePlayer(GameObject player)
    {
        Debug.Log("Queen is seeing player");
        foreach (BeeAIController bee in GameManager.instance.bees)
        {
            bee.mainTarget = hive.transform;
        }
    }

    void HearPlayer(GameObject player)
    {
        Debug.Log("Queen is hearing player");
        // Look at the player
        Vector3 targetDirection = player.transform.position - transform.position;
        Vector3 lookDirection = Vector3.RotateTowards(transform.forward, targetDirection, beeData.moveSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(lookDirection);

        // Warn other bees
        foreach (BeeAIController bee in GameManager.instance.bees)
        {
            bee.mainTarget = hive.GetRandomPointFromHive(bee.gameObject, minDistanceFromHive, maxDistanceFromHive);
        }
    }
}
