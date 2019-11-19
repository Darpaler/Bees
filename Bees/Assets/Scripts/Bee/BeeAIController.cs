using System.Collections;
using System.Collections.Generic;
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
    protected BeeData beeData;

    private NavMeshAgent agent; // The Nav Mesh Agent Component

    [SerializeField]
    private float sightDistance;
    [SerializeField]
    private float fovAngle;

    [SerializeField]
    protected float maxDistanceFromHive; // This should be far for workers and close for soldiers  
    [SerializeField]
    protected float minDistanceFromHive; // This should be far for workers and close for soldiers  

    [SerializeField]
    protected float maxDistanceFromPlayer; // This should be far for workers and close for soldiers  
    [SerializeField]
    protected float minDistanceFromPlayer; // This should be far for workers and close for soldiers  

    public Transform mainTarget = null; // Bees will always seek their main target location

    public Transform secondTarget = null; // An additional target location that they can store and change

    private GameObject player;              // The player
    private NoiseMaker playerNoiseMaker;    // The player's noise maker component
    [SerializeField]
    private float hearingRadius = 3f;       // How far we can hear 
    [SerializeField]
    private float hearingRefreshPeriod = 3f;
    private float currentHearingRefreshPeriod = 0f;

    // Start is called before the first frame update
    protected void Start()
    {
        // Get Components
        hive = GameManager.instance.hive;

        agent = GetComponent<NavMeshAgent>();
        beeMovement = GetComponent<BeeMovement>();
        beeData = GetComponent<BeeData>();

        // Add self to game manager
        GameManager.instance.bees.Add(this);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (hive == null)
        {
            hive = GameManager.instance.hive;
        }
        //if (player == null) { return;}

        Sight();

        if(currentHearingRefreshPeriod > 0)
        {
            currentHearingRefreshPeriod -= Time.deltaTime;
        }

        if (mainTarget != player && currentHearingRefreshPeriod <= 0)
        {
            Hearing();
            currentHearingRefreshPeriod = hearingRefreshPeriod;
        }

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

    protected void Hearing()
    {
        // If the bee is heading towards the hive, don't interrupt
        if (mainTarget != null && mainTarget.gameObject == hive.gameObject) { return; }

        // If the noise maker is not set
        if (playerNoiseMaker == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerNoiseMaker = player.GetComponent<NoiseMaker>();
        }

        // If we did not fail to find the player's noise maker
        if(playerNoiseMaker != null)
        {
            // If we're close enough to hear the player
            if (Vector3.Distance(player.transform.position, transform.position) <= playerNoiseMaker.volume + hearingRadius)
            {
                HearPlayer(player);
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
        if (!player.GetComponent<PawnMovement>().isVisible)
        {
            return;
        }
    }
    protected void HearPlayer(GameObject player)
    {
        Vector2 distanceFromPlayer = new Vector2(minDistanceFromPlayer, maxDistanceFromPlayer);

        RandomPointFromPlayer(distanceFromPlayer);
    }

    public void RandomPointFromPlayer(Vector2 distanceFromPlayer)
    {
        // Get a random distance
        float walkRadius = Random.Range(distanceFromPlayer.x, distanceFromPlayer.y);

        // Get a random direction
        Vector3 randomDirection;
        randomDirection = player.transform.position + Random.insideUnitSphere * walkRadius;

        // Grab a point on the nav mesh at the random distance and direction
        NavMeshHit hit;

        if(NavMesh.SamplePosition(randomDirection, out hit, walkRadius, NavMesh.AllAreas))
        {
            // Create a temporary target there
            GameObject tempTransform = new GameObject();
            tempTransform.name = name + "'s TempTarget";
            tempTransform.transform.position = hit.position;
            tempTransform.AddComponent<SphereCollider>();
            tempTransform.AddComponent<TempTarget>();
            tempTransform.GetComponent<TempTarget>().targetOwner = gameObject;

            // Set the main target to that target
            mainTarget = tempTransform.transform;
        }
        else
        {
            mainTarget = null;
        }
    }

}
