using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CarryObject : InteractiveObject
{
    [SerializeField] private Transform Destination = null;

    private float agentSpeed = 0f;
    private Camera mainCamera = null;
    private NavMeshAgent agent = null;
    private Coroutine carryingCoroutine = null;

    public override void Initialize()
    {
        base.Initialize();
        agent = GetComponent<NavMeshAgent>();
        agentSpeed = agent.speed;
        mainCamera = Camera.main;
    }

    public override void Interact()
    {
        if (agent.IsArrived(transform.position, Destination.position))
            return;

        if (agent.enabled)
            agent.isStopped = false;
        else
        {
            agent.isStopped = false;
            agent.enabled = true;
        }

        if (carryingCoroutine != null)
            StopCoroutine(carryingCoroutine);
        carryingCoroutine = StartCoroutine(GetInPosition());

        IEnumerator GetInPosition()
        {
            agent.SetDestination(Destination.position);
            while (!agent.IsDone())
                yield return null;
            agent.enabled = false;
        }
    }

    public override void StopInteract()
    {
        if(agent.enabled)
            agent.isStopped = true;
        if (carryingCoroutine != null)
        {
            StopCoroutine(carryingCoroutine);
            carryingCoroutine = null;
        }
    }

    private void Update()
    {
        /* 할당된 Pikmin 수 표시 UI */
        if (fractionObject != null)
            fractionObject.transform.position = mainCamera.WorldToScreenPoint(transform.position + uiOffset);
    }
}
