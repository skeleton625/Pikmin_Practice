using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Pikmin : MonoBehaviour
{
    public enum State { Idle, Follow, Interact };
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public State PikminState { get => state; }

    private State state = default;
    private Transform target = null;
    private Coroutine updateTarget = null;
    private PikminManager pManager = null;

    private int objectiveID = -1;
    private InteractiveObject objective = null;
    private WaitForSeconds wait = new WaitForSeconds(.25f);


    private void Awake()
    {
        target = GameObject.Find("Follow_Position").transform;
        agent = GetComponent<NavMeshAgent>();
        pManager = PikminManager.instance;
    }

    /* SetTarget 함수 대용 */
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ControlObject"))
        {
            if (state.Equals(State.Follow))
                return;
            else if (state.Equals(State.Interact))
            {
                objective.ReleasePikmin(objectiveID);
                objective = null;
                objectiveID = -1;
            }

            state = State.Follow;
            pManager.GetPikmin(this);

            if (updateTarget != null)
                StopCoroutine(updateTarget);

            updateTarget = StartCoroutine(UpdateTarget());

            IEnumerator UpdateTarget()
            {
                agent.enabled = true;
                while (true)
                {
                    if (agent.enabled)
                        agent.SetDestination(target.position);
                    yield return wait;
                }
            }
        }
    }

    private void CheckInteraction()
    {
        /* 반지름이 1인 Raycast 구체로 충돌체 확인, 없을 시 종료 */
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);
        if (colliders.Length.Equals(0))
            return;

        /* pikmin 주위의 한 IntractiveObject만 상호작용 */
        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("InteractiveObject"))
            {
                objective = collider.GetComponent<InteractiveObject>();
                objectiveID = objective.AssignPikmin(this);
                break;
            }
        }
    }


    public void Throw(Vector3 target, float time, float delay)
    {
        /* 비행 소리 필요 */
        state = State.Idle;

        if(updateTarget != null)
            StopCoroutine(updateTarget);
        agent.enabled = false;

        transform.DOJump(target, 2, 1, time).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
        {
            agent.enabled = true;
            CheckInteraction();

            /* 착지 소리 필요 */
        });

        Vector3 pos = target;
        pos.y = transform.position.y;
        transform.LookAt(pos);
        
        /* PikminVisualHandler */
    }
}
