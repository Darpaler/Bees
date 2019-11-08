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

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        previousPosition = transform.position;
    }

    void Update()
    {
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
        characterController.SimpleMove(Vector3.forward * speed * Time.deltaTime);     // Move By How Fast You Are
        if(speed > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    public void MoveHorizontal(float speed)                                             // Move Left/Right
    {
        characterController.SimpleMove(Vector3.right * speed * Time.deltaTime);     // Move By How Fast You Are
        transform.rotation = Quaternion.Euler(0, 90 * Mathf.Sign(speed), 0);

    }
}
