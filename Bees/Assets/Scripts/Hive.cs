﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hive : MonoBehaviour
{

    // Variables
    public List<Flower> flowerList = new List<Flower>(); // The list of flowers

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
        tempTransform.GetComponent<TempTarget>().targetOwner = ai;
        return tempTransform.transform;
    }

    public Transform GetRandomFlower()
    {
        return flowerList[Random.Range(0, flowerList.Count - 1)].flower.transform;
    }

    public void ForgetFlowers(BeeMovement submittedBy)
    {
        foreach (Flower flower in flowerList)
        {
            if (flower.submittedBy == submittedBy)
            {
                flowerList.Remove(flower);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BeeAIController>())
        {
            other.GetComponent<BeeAIController>().TouchHive();
        }
    }

    public struct Flower
    {
        public GameObject flower;          //The flower game object
        public BeeMovement submittedBy;    //The bee who submitted the flower
    }

    public void AddFlower(GameObject flower, BeeMovement submittedBy)
    {
        Flower flowerToAdd = new Flower();
        flowerToAdd.flower = flower;
        flowerToAdd.submittedBy = submittedBy;
        flowerList.Add(flowerToAdd);
    }

}
