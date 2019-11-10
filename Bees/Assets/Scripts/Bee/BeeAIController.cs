using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.PlayerLoop;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(BeeMovement))]
[RequireComponent(typeof(NavMeshAgent))]

public class BeeAIController : MonoBehaviour
{
    // Variables
    protected Hive hive;

    protected BeeMovement beeMovement;  // The Bee Movement component
    private NavMeshAgent agent; // The Nav Mesh Agent Component

    [SerializeField]
    private float sightDistance;
    [SerializeField]
    private float fovAngle;

    [SerializeField]
    protected float maxDistanceFromHive; // This should be far for workers and close for soldiers  
    [SerializeField]
    protected float minDistanceFromHive; // This should be far for workers and close for soldiers  

    public Transform mainTarget = null; // Bees will always seek their main target location

    public Transform secondTarget = null; // An additional target location that they can store and change

    // Start is called before the first frame update
    protected void Start()
    {
        // Get Components
        hive = GameObject.FindGameObjectWithTag("Hive").GetComponent<Hive>();

        agent = GetComponent<NavMeshAgent>();
        beeMovement = GetComponent<BeeMovement>();
    }

    // Update is called once per frame
    protected void Update()
    {
        Sight();

        if (mainTarget)
        {
            agent.SetDestination(mainTarget.position);
            if (CheckDestinationReached(mainTarget.position))
            {
                ReachedDestination();
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

    protected void ReachedDestination()
    {
        if (!secondTarget)
        {
            mainTarget = hive.transform;
        }
    }

    protected void Sight()
    {
        // If the bee is heading towards the hive, don't interrupt
        if(mainTarget != null && mainTarget.gameObject == hive.gameObject) { return; }

        // Check all colliders around the bee
        Collider[] cols = Physics.OverlapSphere(transform.position, sightDistance);
        Vector3 vectorToCollider;
        foreach (Collider collider in cols)
        {
            vectorToCollider = (collider.transform.position - transform.position).normalized;

            // If the collider is within our field of view
            if (Vector3.Dot(vectorToCollider, transform.forward) >= Mathf.Cos(fovAngle))
            {
                // If the collider is a flower
                if (collider.gameObject.tag == "Flower")
                {
                    SeeFlower(collider.gameObject);
                }
                // If the collider is a soldier
                if (collider.GetComponent<BeeAIController_Soldier>())
                {
                    SeeSoldier(collider.GetComponent<BeeAIController_Soldier>());
                }
                // If the collider is the player
                if (collider.gameObject.tag == "Player")
                {
                    SeePlayer(collider.gameObject);
                }
            }
        }
    }

    protected virtual void SeeFlower(GameObject flower)
    {

    }
    protected virtual void SeeSoldier(BeeAIController_Soldier other)
    {

    }
    protected virtual void SeePlayer(GameObject player)
    {

    }

}
