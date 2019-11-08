using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class TempTarget : MonoBehaviour
{

    // Variables
    private Collider col;                       // The collider component
    public GameObject targetOwner;             // The game object that is searching for this project
    private float lifeSpanAfterReached = 1f;    // How long until the game object destroys once the owner reaches it
    private float maxLifeSpan = 60f;

    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;
        Destroy(gameObject, maxLifeSpan);
    }

    void OnTriggerEnter(Collider other)
    {
        if (targetOwner == other.gameObject)
        {
            Destroy(gameObject, lifeSpanAfterReached);
        }
    }

}
