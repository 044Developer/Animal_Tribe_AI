using UnityEngine;
using UnityEngine.AI;

public class EatState : StateBase
{
    private const string IsEating = "IsEating";
    private float _eatTime;
    private float _timer;

    public EatState(AnimalAIController controller, AnimalStats animalStats, Animator animator, NavMeshAgent navMeshAgent) : base(controller, animalStats, animator, navMeshAgent)
    {
        CurrentStateName = States.Eat;
    }

    public override void Enter()
    {
        base.Enter();
        InitializeState();
    }
    private void InitializeState()
    {
        _eatTime = animalStats.GetRandomEatTime();
        currentAnimator.SetBool(IsEating, true);
        _timer = 0f;
    }

    public override void Update()
    {
        base.Update();
        EatTimeCountdown();
    }

    public override void Exit()
    {
        base.Exit();
        ClearStateSetting();
    }

    private void ClearStateSetting()
    {
        _eatTime = 0f;
        currentAnimator.SetBool(IsEating, false);
    }

    private void EatTimeCountdown()
    {
        if (_timer < _eatTime)
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
