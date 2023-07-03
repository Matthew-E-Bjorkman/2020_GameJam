using UnityEngine;

public class AttackPhaseSystem<T>
{
    public AttackPhase<T> currentState { get; private set; }
    public T Owner;

    public AttackPhaseSystem(T _owner)
    {
        Owner = _owner;
        currentState = null;
    }

    public AttackPhase<T> ChangeState(AttackPhase<T> _newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(Owner);
        }

        currentState = _newState;
        currentState.EnterState(Owner);

        return currentState;
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(Owner);
        }
    }
}