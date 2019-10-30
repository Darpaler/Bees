using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Transform GetRandomPointFromHive(float minDistanceFromHive, float maxDistanceFromHive)
    {
        float walkRadius = Random.Range(minDistanceFromHive, maxDistanceFromHive);
        Vector3 randomDirection;
        randomDirection = Random.insideUnitSphere * walkRadius;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, walkRadius, 1);
        Debug.Log("Instantiating");
        GameObject tempTransform = Instantiate(new GameObject(), hit.position, Quaternion.identity);
        return tempTransform.transform;
    }

}
