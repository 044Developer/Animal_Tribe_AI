using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToNextAreaState : StateBase
{
    private const string IsRunning = "IsWalking";
    private const float DistanceOffset = 0.2f;

    public MoveToNextAreaState(AnimalAIController controller, AnimalStats animalStats, Animator animator, NavMeshAgent navMeshAgent) : base(controller, animalStats, animator, navMeshAgent)
    {
        CurrentStateName = States.MoveToNextArea;
    }

    public override void Enter()
    {
        base.Enter();
        InitializeState();
    }

    private void InitializeState()
    {
        currentAnimator.SetBool(IsRunning, true);
        currentNavAgent.isStopped = false;
        currentNavAgent.speed = animalStats.AnimalWalkSpeed;
    }

    public override void Update()
    {
        base.Update();
        currentNavAgent.SetDestination(animalController.ParentTransform.position);

        if (currentNavAgent.remainingDistance <= DistanceOffset)
        {
            nextState = GetRandomState(nextState);
            CurrentStageEvent = Event.Exit;
        }
    }

    public override void Exit()
    {
        base.Exit();
        ClearStateSettings();
    }

    private void ClearStateSettings()
    {
        currentAnimator.SetBool(IsRunning, false);
        currentNavAgent.isStopped = true;
        currentNavAgent.speed = 0f;
    }
}
