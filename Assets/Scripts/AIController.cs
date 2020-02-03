using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class AIController : Unit
{
    private IEnumerator _CurrentState;
    private NavMeshAgent _Agent;
    private Outpost _TargetOutpost;

    protected override void UnitAwake()
    {
        _Agent = GetComponent<NavMeshAgent>();
    }

    private new void Start()
    {
        base.Start();
        SetState(State_Idle());
    }

    private void SetState(IEnumerator newState)
    {
        if (_CurrentState != null)
        {
            StopCoroutine(_CurrentState);
        }

        _CurrentState = newState;
        StartCoroutine(_CurrentState);
    }

    private IEnumerator State_Idle()
    {
        while (_TargetOutpost == null)
        {
            _TargetOutpost = Outpost.GetRandomOutpost();
            yield return null;
        }

        SetState(State_MovingToOutpost());
    }

    private IEnumerator State_MovingToOutpost()
    {
        _Agent.SetDestination(_TargetOutpost.transform.position);
        while (_Agent.remainingDistance > _Agent.stoppingDistance)
        {
            yield return null;
        }

        _TargetOutpost = null;
        SetState(State_Idle());
    }
}
