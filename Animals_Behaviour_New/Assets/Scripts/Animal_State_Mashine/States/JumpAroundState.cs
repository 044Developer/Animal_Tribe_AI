using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class JumpAroundState : StateBase
{
    private const string IsJumping = "IsJumping";
    private float _jumpTime;
    private float _timer;

    public JumpAroundState(AnimalAIController controller, AnimalStats animalStats, Animator animator, NavMeshAgent navMeshAgent) : base(controller, animalStats, animator, navMeshAgent)
    {
        CurrentStateName = States.JumpAround;
    }

    public override void Enter()
    {
        base.Enter();
        InitializeState();
    }

    private void InitializeState()
    {
        _jumpTime = animalStats.GetRandomJumpTime();
        currentAnimator.SetBool(IsJumping, true);
        currentNavAgent.isStopped = false;
        currentNavAgent.speed = animalStats.AnimalJumpSpeed;
        animalController.GetNextPatrolPosition();
        _timer = 0f;
    }

    public override void Update()
    {
        base.Update();
        JumpTimeCountdown();

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
        _jumpTime = 0f;
        currentAnimator.SetBool(IsJumping, false);
        currentNavAgent.speed = 0f;
    }

    private void JumpTimeCountdown()
    {
        if (_timer < _jumpTime)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            currentNavAgent.speed = 0f;
            currentNavAgent.isStopped = true;
            nextState = GetRandomState(nextState);
            CurrentStageEvent = Event.Exit;
        }
    }
}
