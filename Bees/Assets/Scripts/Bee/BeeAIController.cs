using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;

[RequireComponent(typeof(BeeMovement))]
[RequireComponent(typeof(NavMeshAgent))]

public class BeeAIController : MonoBehaviour
{
    // Variables
    protected Hive hive;

    private NavMeshAgent agent; // The Nav Mesh Agent Component

    [SerializeField] protected float maxDistanceFromHive; // This should be far for workers and close for soldiers  
    [SerializeField] protected float minDistanceFromHive; // This should be far for workers and close for soldiers  

    public Transform mainTarget = null; // Bees will always seek their main target location

    public Transform secondTarget = null; // An additional target location that they can store and change

    // Start is called before the first frame update
    protected void Start()
    {
        // Get Components
        agent = GetComponent<NavMeshAgent>();
        hive = GameObject.FindGameObjectWithTag("Hive").GetComponent<Hive>();

    }

    // Update is called once per frame
    protected void Update()
    {
        if (mainTarget)
        {
            agent.SetDestination(mainTarget.position);
            if (CheckDestinationReached(mainTarget.position))
            {
                if (!secondTarget)
                {
                    mainTarget = hive.transform;
                }
            }
        }
    }

    protected bool CheckDestinationReached(Vector3 target)
    {
        float distanceToTarget = Vector3.Distance(transform.position, target);
        if (distanceToTarget < agent.stoppingDistance)
        {
            return true;
        }
        return false;
    }

    public virtual void TouchHive() { }

}
