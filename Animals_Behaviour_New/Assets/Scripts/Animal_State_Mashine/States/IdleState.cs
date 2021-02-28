using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateBase
{
    private const string IdleTrigger = "IsIdle";
    private float _idleTime;
    private float _timer;

    public IdleState(AnimalAIController controller, AnimalStats animalStats, Animator animator, NavMeshAgent navMeshAgent) : base(controller, animalStats, animator, navMeshAgent)
    {
        CurrentStateName = States.Idle;
    }

    public override void Enter()
    {
        base.Enter();
        InitializeState();
    }

    private void InitializeState()
    {
        currentAnimator.SetBool(IdleTrigger, true);
        _idleTime = animalStats.GetRandomIdleTime();
        _timer = 0f;
    }

    public override void Update()
    {
        base.Update();
        IdleTimeCountdown();
    }

    public override void Exit()
    {
        base.Exit();
        ClearStateSettings();
    }

    private void ClearStateSettings()
    {
        _idleTime = 0f;
        currentAnimator.SetBool(IdleTrigger, false);
    }

    private void IdleTimeCountdown()
    {
        if (_timer < _idleTime)
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
