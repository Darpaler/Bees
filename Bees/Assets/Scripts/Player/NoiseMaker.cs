using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{

    // Variables
    public float moveVolume = 3f;            // How loud the pawn's movement is
    public float volume;                     // The pawn's current volume
    [SerializeField]
    private float decay = 1f;                // How much the volume decreases per second

    // Update is called once per frame
    void Update()
    {
        // Decay Volume
        if (volume > 0)
        {
            volume -= decay * Time.deltaTime;
        }
    }
}
