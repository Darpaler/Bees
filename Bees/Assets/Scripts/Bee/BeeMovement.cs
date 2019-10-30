using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeeData))]

public class BeeMovement : MonoBehaviour
{
    // Variables
    private BeeData data;   // The Bee Data Component

    // Start is called before the first frame update
    void Start()
    {
        // Get Components
        data = GetComponent<BeeData>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
