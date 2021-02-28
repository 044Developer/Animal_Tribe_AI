using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RestState : StateBase
{
    private const string IsResting = "IsResting";
    private float _restTime;
    private float _timer;

    public RestState(AnimalAIController controller, AnimalStats animalStats, Animator animator, NavMeshAgent navMeshAgent) : base(controller, animalStats, animator, navMeshAgent)
    {
        CurrentStateName = States.Rest;
    }

    public override void Enter()
    {
        base.Enter();
        InitializeState();
    }

    private void InitializeState()
    {
        _restTime = animalStats.GetRandomRestTime();
        currentAnimator.SetBool(IsResting, true);
        _timer = 0f;
    }

    public override void Update()
    {
        base.Update();
        RestTimeCountdown();
    }

    public override void Exit()
    {
        base.Exit();
        ClearStateSettings();
    }

    private void ClearStateSettings()
    {
        _restTime = 0f;
        currentAnimator.SetBool(IsResting, false);
    }

    private void RestTimeCountdown()
    {
        if (_timer < _restTime)
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
