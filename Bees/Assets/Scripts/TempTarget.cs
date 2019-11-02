using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class TempTarget : MonoBehaviour
{

    // Variables
    private Collider col;   // The collider component

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        BeeAIController beeAI = other.GetComponent<BeeAIController>();
        if (!beeAI) { return; }

        if (beeAI.mainTarget.gameObject == gameObject)
        {
            Destroy(gameObject,1);
        }
    }

}
