using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalStats 
{
    [Header("Nav Mesh Speed")]
    [SerializeField]
    private float _animalWalkSpeed;
    [SerializeField]
    private float _animalJumpSpeed;

    [Header("Animal Time Settings")]
    [SerializeField]
    private float _minIdleTime;
    [SerializeField]
    private float _maxIdleTime;
    [SerializeField]
    private float _minEatTime;
    [SerializeField]
    private float _maxEatTime;
    [SerializeField]
    private float _minRestTime;
    [SerializeField]
    private float _maxRestTime;
    [SerializeField]
    private float _minWalkTime;
    [SerializeField]
    private float _maxWalkTime;
    [SerializeField]
    private float _minJumpTime;
    [SerializeField]
    private float _maxJumpTime;

    [Header("Patrol Spawn Points")]
    [Range(0, 360)]
    [SerializeField]
    private int _spawnAngle;
    [SerializeField]
    private float _minSpawnOffset;
    [SerializeField]
    private float _maxSpawnOffset;

    public float AnimalWalkSpeed { get => _animalWalkSpeed; }
    public float AnimalJumpSpeed { get => _animalJumpSpeed; }
    public int SpawnAngle { get => _spawnAngle; }
    public float MinSpawnOffset { get => _minSpawnOffset; }
    public float MaxSpawnOffset { get => _maxSpawnOffset; }

    // When state starts, this method generetes random time to act in current state
    public float GetRandomIdleTime()
    {
        return Random.Range(_minIdleTime, _maxIdleTime);
    }

    // When state starts, this method generetes random time to act in current state
    public float GetRandomEatTime()
    {
        return Random.Range(_minEatTime, _maxEatTime);
    }

    // When state starts, this method generetes random time to act in current state
    public float GetRandomRestTime()
    {
        return Random.Range(_minRestTime, _maxRestTime);
    }

    // When state starts, this method generetes random time to act in current state
    public float GetRandomWalkTime()
    {
        return Random.Range(_minWalkTime, _maxWalkTime);
    }

    // When state starts, this method generetes random time to act in current state
    public float GetRandomJumpTime()
    {
        return Random.Range(_minJumpTime, _maxJumpTime);
    }
}
