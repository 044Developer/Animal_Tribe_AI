using UnityEngine;
using UnityEngine.AI;

public abstract class StateBase
{
    // State Types
    public enum States
    {
        Idle,
        Eat,
        Rest,
        WalkAround,
        JumpAround,
        MoveToNextArea
    }

    // State Events
    public enum Event
    {
        Enter,
        Update,
        Exit
    }

    public States CurrentStateName;
    public Event CurrentStageEvent;
    protected AnimalAIController animalController;
    protected AnimalStats animalStats;
    protected Animator currentAnimator;
    protected StateBase nextState;
    protected NavMeshAgent currentNavAgent;

    public StateBase(AnimalAIController controller, AnimalStats stats, Animator animator, NavMeshAgent navMeshAgent)
    {
        animalController = controller;
        animalStats = stats;
        currentAnimator = animator;
        currentNavAgent = navMeshAgent;
        CurrentStageEvent = Event.Enter;
    }

    public virtual void Enter() { CurrentStageEvent = Event.Update; }
    public virtual void Update() { CurrentStageEvent = Event.Update; }
    public virtual void Exit() { CurrentStageEvent = Event.Exit; }

    // State Update function to run current state in AIController
    public StateBase Process()
    {
        if (CurrentStageEvent == Event.Enter)
        {
            Enter();
        }

        if (CurrentStageEvent == Event.Update)
        {
            Update();
        }

        if (CurrentStageEvent == Event.Exit)
        {
            Exit();
            return nextState;
        }
        return this;
    }

    // Gets random next state. Better to create a better probability check afterwards
    public StateBase GetRandomState(StateBase nextState)
    {
        int randomStateID = Random.Range(1, 6);

        switch (randomStateID)
        {
            case 1:
                nextState = new EatState(animalController, animalStats, currentAnimator, currentNavAgent);
                break;
            case 2:
                nextState = new RestState(animalController, animalStats, currentAnimator, currentNavAgent);
                break;
            case 3:
                nextState = new WalkAroundState(animalController, animalStats, currentAnimator, currentNavAgent);
                break;
            case 4:
                nextState = new IdleState(animalController, animalStats, currentAnimator, currentNavAgent);
                break;
            case 5:
                nextState = new JumpAroundState(animalController, animalStats, currentAnimator, currentNavAgent);
                break;
            default:
                break;
        }

        return nextState;
    }
}
