using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribeSpawnPoint : MonoBehaviour
{
    public Transform SpawnPoint { get; private set; }
    public bool IsOccupied { get; private set; } = false;

    private void Awake()
    {
        SpawnPoint = this.transform;
    }

    // Sets current area as occupied
    public void OccupieSpot()
    {
        IsOccupied = true;
    }

    // Sets current area as free, thus another tribe can choose it as a new desired location
    public void UnOccupieSpot()
    {
        IsOccupied = false;
    }
}
