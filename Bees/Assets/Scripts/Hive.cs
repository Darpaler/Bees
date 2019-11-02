using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hive : MonoBehaviour
{

    public Transform GetRandomPointFromHive(GameObject ai, float minDistanceFromHive, float maxDistanceFromHive)
    {
        float walkRadius = Random.Range(minDistanceFromHive, maxDistanceFromHive);
        Vector3 randomDirection;
        randomDirection = Random.insideUnitSphere * walkRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        GameObject tempTransform = new GameObject();
        tempTransform.name = ai.name + "'s TempTarget";
        tempTransform.transform.position = hit.position;
        tempTransform.AddComponent<SphereCollider>();
        tempTransform.AddComponent<TempTarget>();
        return tempTransform.transform;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Touched " + other.gameObject.name);
        if (other.GetComponent<BeeAIController>())
        {
            Debug.Log("Found bee");
            other.GetComponent<BeeAIController>().TouchHive();
        }
    }

}
