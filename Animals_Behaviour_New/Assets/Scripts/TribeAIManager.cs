using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TribeAIManager : MonoBehaviour
{
    [Header("Animal Types")]
    [SerializeField]
    private GameObject _leaderAnimalPrefab;
    [SerializeField]
    private GameObject _parentAnimalPrefab;
    [SerializeField]
    private GameObject _childAnimalPrefab;

    [Header("Spawn Points")]
    [SerializeField]
    private int _leaderSpawnID;
    [SerializeField]
    private Transform _spawnPointPrefab;
    [SerializeField]
    private float _timeToChangeLocation;
    [SerializeField]
    private List<TribeSpawnPoint> _tribeLocations;

    [Header("Spawn Distance Settings")]
    [SerializeField]
    private float _minParentAnimalSpawnDistance;
    [SerializeField]
    private float _maxParentAnimalSpawnDistance;
    [SerializeField]
    private float _minChildAnimalSpawnDistance;
    [SerializeField]
    private float _maxChildAnimalSpawnDistance;

    [Header("Spawn Angle Settings")]
    [Range(0, 360)]
    [SerializeField]
    private int _parentSpawnAngle;
    [Range(0, 360)]
    [SerializeField]
    private int _childSpawnAngle;

    private Transform _leaderSpawnPoint;
    private int _currentTribeLocationIndex;
    private List<Vector3> _parentsSpawnPoints = new List<Vector3>();
    private List<Vector3> _childSpawnPoints = new List<Vector3>();
    private List<AnimalAIController> _allAnimalControllers = new List<AnimalAIController>();

    private void Start()
    {
        InitializeTribe();

        StartCoroutine(ChangeLocationLoop());
    }

    private void InitializeTribe()
    {
        SpawnLeader();
        GetRandomParentAnimalSpawn();
        GetRandomChildAnimalSpawn();
    }

    // Spawns Leader of the tribe in a specified location of an area and sets this area as occupied
    private void SpawnLeader()
    {
        _currentTribeLocationIndex = _leaderSpawnID;
        var leaderPosition = Instantiate(_leaderAnimalPrefab, _tribeLocations[_leaderSpawnID].gameObject.transform.position, Quaternion.identity);
        _tribeLocations[_leaderSpawnID].OccupieSpot();
        _leaderSpawnPoint = leaderPosition.transform;
    }

    // Generates Parent animals around a leader of the tribe on a specified angle and distance
    private void GetRandomParentAnimalSpawn()
    {
        for (int angle = 0; angle < 360; angle += _parentSpawnAngle)
        {
            var newSpawnPoint = _leaderSpawnPoint.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * RandomSpawnPoint(_minParentAnimalSpawnDistance, _maxParentAnimalSpawnDistance);
            _parentsSpawnPoints.Add(newSpawnPoint);

            var targetPoint = Instantiate(_spawnPointPrefab, newSpawnPoint, Quaternion.identity, _leaderSpawnPoint);

            GameObject animalObject = Instantiate(_parentAnimalPrefab, newSpawnPoint, Quaternion.identity, transform);
            AnimalAIController animalController = animalObject.GetComponent<AnimalAIController>();
            animalController.InitializeController(targetPoint);
            _allAnimalControllers.Add(animalController);
        }
    }

    // Generates Child animals around a parent of the tribe on a specified angle and distance
    private void GetRandomChildAnimalSpawn()
    {
        for (int parent = 0; parent < _parentsSpawnPoints.Count; parent++)
        {
            for (int angle = 0; angle < 360; angle += _childSpawnAngle)
            {
                var newSpawnPoint = _parentsSpawnPoints[parent] + Quaternion.Euler(0, angle, 0) * Vector3.forward * RandomSpawnPoint(_minParentAnimalSpawnDistance, _maxParentAnimalSpawnDistance);
                var targetPoint = Instantiate(_spawnPointPrefab, newSpawnPoint, Quaternion.identity, _leaderSpawnPoint);
                _childSpawnPoints.Add(newSpawnPoint);

                GameObject animalObject = Instantiate(_childAnimalPrefab, newSpawnPoint, Quaternion.identity, transform);
                AnimalAIController animalController = animalObject.GetComponent<AnimalAIController>();
                animalController.InitializeController(targetPoint);
                _allAnimalControllers.Add(animalController);
            }
        }
    }

    // Returns random distance offset of the target spawned
    private float RandomSpawnPoint(float minPos, float maxPos)
    {
        float spawnOffset = Random.Range(minPos, maxPos);
        return spawnOffset;
    }

    // Timer to trigger current tribe to change to a new location
    private IEnumerator ChangeLocationLoop()
    {
        yield return new WaitForSeconds(_timeToChangeLocation);

        SetNewTribeLocation();
        TriggerChange();

        StartCoroutine(ChangeLocationLoop());
    }

    // Checks for an new, not occupied location to move current tribe to
    private void SetNewTribeLocation()
    {
        var previousLocation = _currentTribeLocationIndex;

        while (_tribeLocations[_currentTribeLocationIndex].IsOccupied)
        {
            if (_currentTribeLocationIndex < _tribeLocations.Count -1)
            {
                _currentTribeLocationIndex++;
            }
            else
            {
                _currentTribeLocationIndex = 0;
            }
        }

        _tribeLocations[previousLocation].UnOccupieSpot();
        _tribeLocations[_currentTribeLocationIndex].OccupieSpot();
        _leaderSpawnPoint.position = _tribeLocations[_currentTribeLocationIndex].SpawnPoint.position;
    }

    // Triggers all controllers to act area change state
    private void TriggerChange()
    {
        foreach (var agent in _allAnimalControllers)
        {
            agent.TriggerChangeLocation();
        }
    }
}
