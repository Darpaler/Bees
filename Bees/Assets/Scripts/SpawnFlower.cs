using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFlower : MonoBehaviour
{
    // Variables
    [SerializeField]
    private GameObject flower;      // The flower to spawn
    [SerializeField]
    private float respawnTime = 30;     // The amount of time the flower takes to respawn
    private float currentRespawnTime = 0;   // The current amount of time left till respawn
    [SerializeField]
    private int maxCollect = 3;      // The max of times this flower can be collected
    public int currentCollect;      // The current amount times the flower can be collected

    private Hive hive;


    // Start is called before the first frame update
    void Start()
    {
        hive = GameObject.FindGameObjectWithTag("Hive").GetComponent<Hive>();

        currentCollect = maxCollect;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentRespawnTime > 0) { currentRespawnTime -= Time.deltaTime; }

        if (currentCollect <= 0)
        {
            DespawnFlower();
        }

        if(!flower.activeSelf && currentRespawnTime <= 0)
        {
            Spawn();
        }
    }

    public void DespawnFlower()
    {
        GameManager.instance.despawnedFlowerSpawns.Add(this);
        currentRespawnTime = respawnTime;
        foreach (Hive.Flower hiveFlower in hive.flowerList)
        {
            if (hiveFlower.flower == flower)
            {
                hive.flowerList.Remove(hiveFlower);
            }
        }
        flower.SetActive(false);
    }

    public void Spawn()
    {
        GameManager.instance.despawnedFlowerSpawns.Remove(this);
        currentCollect = maxCollect;
        flower.SetActive(true);
    }

}
