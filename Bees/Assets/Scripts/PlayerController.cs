using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PawnData))]
[RequireComponent(typeof(PawnMovement))]

public class PlayerController : MonoBehaviour
{
    // Variables
    public PawnData data;
    public PawnMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        data = GetComponent<PawnData>();
        movement = GetComponent<PawnMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement.MoveVertical(data.speed);
        }
        //Down
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement.MoveVertical(-data.speed);
        }
        //Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            movement.MoveHorizontal(data.speed);
        }
        //Left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement.MoveHorizontal(-data.speed);
        }
    }
}
