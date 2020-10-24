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

    private State state = default;

    private InteractiveObject objective;
    private PikminManager Pmanager;

    private Coroutine updateTarget = default;
    private Transform target = default;

    private bool isFlying = false;
    private bool isGettingIntoPosition = false;
    public bool IsFlying { get => isFlying; }
    public bool IsGettingIntoPosition { get => isGettingIntoPosition; }

    private void Awake()
    {
        target = GameObject.Find("Follow_Position").transform;
        Pmanager = PikminManager.instance;
        agent = GetComponent<NavMeshAgent>();
    }

    /* SetTarget 함수 대용 */
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("ControlObject"))
        {
            if (state.Equals(State.Follow))
                return;

            state = State.Follow;
            Pmanager.GetPikmin(this);

            if (updateTarget != null)
                StopCoroutine(updateTarget);
            WaitForSeconds wait = new WaitForSeconds(.25f);
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
        /* 반지름이 1인 Raycast 구체로 충돌체 확인 */
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f);

        foreach(Collider collider in colliders)
        {
            if(collider.CompareTag("InteractiveObject"))
            {
                objective = collider.GetComponent<InteractiveObject>();
                objective.AssignPikmin();
                StartCoroutine(GetInPosition());
            }
        }

        IEnumerator GetInPosition()
        {
            isGettingIntoPosition = true;

            agent.SetDestination(objective.GetPosition());
            yield return new WaitUntil(() => agent.IsDone());
            agent.enabled = false;
            state = State.Interact;

            if(objective)
            {
                transform.parent = objective.transform;

                Vector3 pos = objective.transform.position;
                pos.y = transform.position.y;
                transform.DOLookAt(pos, .2f);
            }

            isGettingIntoPosition = false;
        }
    }


    public void Throw(Vector3 target, float time, float delay)
    {
        /* 비행 소리 필요 */
        isFlying = true;
        state = State.Idle;
        agent.enabled = false;
        StopCoroutine(updateTarget);

        transform.DOJump(target, 2, 1, time)
                 .SetDelay(delay)
                 .SetEase(Ease.Linear)
                 .OnComplete(() =>
                 {
                     isFlying = false;
                     agent.enabled = true;
                     /* 착지 소리 필요 */
                 });

        Vector3 pos = target;
        pos.y = transform.position.y;
        transform.LookAt(pos);
        
        /* PikminVisualHandler */
    }
}
