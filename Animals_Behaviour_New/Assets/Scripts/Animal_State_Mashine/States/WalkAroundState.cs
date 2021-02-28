using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkAroundState : StateBase
{
    private const string IsWalking = "IsWalking";
    private float _walkAroundTime;
    private float _timer;

    public WalkAroundState(AnimalAIController controller, AnimalStats animalStats, Animator animator, NavMeshAgent navMeshAgent) : base(controller, animalStats, animator, navMeshAgent)
    {
        CurrentStateName = States.WalkAround;
    }

    public override void Enter()
    {
        base.Enter();
        InitializeState();
    }

    private void InitializeState()
    {
        _walkAroundTime = animalStats.GetRandomWalkTime();
        currentAnimator.SetBool(IsWalking, true);
        currentNavAgent.isStopped = false;
        currentNavAgent.speed = animalStats.AnimalWalkSpeed;
        animalController.GetNextPatrolPosition();
        _timer = 0f;
    }

    public override void Update()
    {
        base.Update();

        WalkTimeCountdown();

        if (animalController.IsReachedPosition())
        {
           animalController.GetNextPatrolPosition();
        }
    }

    public override void Exit()
    {
        base.Exit();
        ClearStateSettings();
    }

    private void ClearStateSettings()
    {
        _walkAroundTime = 0f;
        currentAnimator.SetBool(IsWalking, false);
        currentNavAgent.isStopped = true;
        currentNavAgent.speed = 0f;
    }

    private void WalkTimeCountdown()
    {
        if (_timer < _walkAroundTime)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            nextState = GetRandomState(nextState);
            CurrentStageEvent = Event.Exit;
        }
    }
}
