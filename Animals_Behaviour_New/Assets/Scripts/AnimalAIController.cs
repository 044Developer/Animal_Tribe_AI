using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalAIController : MonoBehaviour
{
    private const float DistanceOffset = 0.2f;

    [Header("Animal Stats")]
    [SerializeField]
    private AnimalStats _animalStats;

    [Header("Animal Cashed Components")]
    [SerializeField]
    private NavMeshAgent _navMeshAgent;
    [SerializeField]
    private Animator _animalAnimator;

    public Transform ParentTransform { get; private set; }

    private StateBase _currentState;
    private List<Vector3> _patrolPositions = new List<Vector3>();
    private int _currentPositionIndex = 0;

    // Initialize this animal controller on a moment of its creation
    public void InitializeController(Transform spawnPosition)
    {
        ParentTransform = spawnPosition;
        SetPatrolPositions();
    }

    private void Update()
    {
        _currentState = _currentState.Process();
    }

    // Sets animal patrol positions around him on a specified angle and distance
    private void SetPatrolPositions()
    {
        for (int angle = 0; angle < 360; angle += _animalStats.SpawnAngle)
        {
            var newSpawnPoint = transform.position + Quaternion.Euler(0, angle, 0) * Vector3.forward * Random.Range(_animalStats.MinSpawnOffset, _animalStats.MaxSpawnOffset);

            _patrolPositions.Add(newSpawnPoint);
        }

        _currentState = new EatState(this, _animalStats, _animalAnimator, _navMeshAgent);
    }

    // Returns next destination for navMesh agent
    public void GetNextPatrolPosition()
    {
        if (_currentPositionIndex < _patrolPositions.Count -1)
        {
            _currentPositionIndex++;
        }
        else
        {
            _currentPositionIndex = 0;
        }

        Vector3 nextDestinationPos = _patrolPositions[_currentPositionIndex];

        _navMeshAgent.SetDestination(nextDestinationPos);
    }

    // Checks, if destination was reached or not
    public bool IsReachedPosition()
    {
        if (_navMeshAgent.remainingDistance < DistanceOffset)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // When tribe need to move to next area, this Method triggers current agent to act ChangeArea state
    public void TriggerChangeLocation()
    {
        _currentState.Exit();
        _currentState = new MoveToNextAreaState(this, _animalStats, _animalAnimator, _navMeshAgent);
    }
}
