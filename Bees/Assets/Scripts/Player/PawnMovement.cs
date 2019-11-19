using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PawnMovement : MonoBehaviour
{
    // Variables
    private CharacterController characterController;
    private Animator anim;
    private Vector3 previousPosition;
    private NoiseMaker noiseMaker;
    public bool isVisible = true;
    private bool usedPanic = false;
    private Vector3 movementVector = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        noiseMaker = GetComponent<NoiseMaker>();

        previousPosition = transform.position;

    }

    void Update()
    {
        characterController.SimpleMove(movementVector);
        movementVector = Vector3.zero;
        if (transform.position != previousPosition)
        {
            anim.SetBool("isMoving", true);
            previousPosition = transform.position;
        }
        else if (anim.GetBool("isMoving"))
        {
            anim.SetBool("isMoving", false);
        }
    }

    //Movement Functions
    public void MoveVertical(float speed)                                               // Move Foward/Back
    {
        if (!isVisible) { return; }

        movementVector.z = speed * Time.deltaTime; // Move By How Fast You Are
        if(speed > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        // Set the volume to the movement volume, unless we are already making a louder sound
        noiseMaker.volume = Mathf.Max(noiseMaker.volume, noiseMaker.moveVolume);
    }

    public void MoveHorizontal(float speed)                                             // Move Left/Right
    {
        if (!isVisible) { return; }

        movementVector.x = speed * Time.deltaTime;    // Move By How Fast You Are
        transform.rotation = Quaternion.Euler(0, 90 * Mathf.Sign(speed), 0);
        // Set the volume to the movement volume, unless we are already making a louder sound
        noiseMaker.volume = Mathf.Max(noiseMaker.volume, noiseMaker.moveVolume);

    }

    public void Stealth()
    {
        isVisible = false;
        noiseMaker.enabled = false;
    }

    public void UnStealth()
    {
        isVisible = true;
        characterController.detectCollisions = true;
        noiseMaker.enabled = true;
    }

    public void PickFlower(float pickRadius)
    {
        Collider[] nearbyFlowers = Physics.OverlapSphere(transform.position, pickRadius);
        foreach(Collider col in nearbyFlowers)
        {
            if (col.GetComponentInParent<SpawnFlower>())
            {
                col.GetComponentInParent<SpawnFlower>().DespawnFlower();
                Debug.Log("Despawned");
                return;
            }
            Debug.Log("Did not despwn");
        }
    }

    public void DropFlower()
    {
        if (GameManager.instance.despawnedFlowerSpawns.Count != 0)
        {
            GameManager.instance.despawnedFlowerSpawns[Random.Range(0, GameManager.instance.despawnedFlowerSpawns.Count - 1)].Spawn();
        }
    }

    public void Panic(Vector2 distanceFromPlayer)
    {
        Debug.Log("Panic");
        if (!usedPanic)
        {
            GameManager.instance.beeManager.BroadcastMessage("RandomPointFromPlayer", distanceFromPlayer);
            usedPanic = true;       
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.GetComponent<BeeAIController>())
        {
            Destroy(gameObject);
        }
    }
}
