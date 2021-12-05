using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(NavMeshAgent))]
public class CarryObject : InteractiveObject
{
    [Header("Target Component"), Space(10)]
    [SerializeField] private Transform Source = null;
    [SerializeField] private Transform Destination = null;

    private Camera mainCamera = null;
    private NavMeshAgent carryObjectAgent = null;
    private Coroutine carryingCoroutine = null;

    private void Start()
    {
        carryObjectAgent = GetComponent<NavMeshAgent>();
        mainCamera = Camera.main;
    }

    public override void Initialize()
    {
        carryObjectAgent.enabled = true;
    }

    public override void Interact()
    {
        if (carryObjectAgent.IsArrived(transform.position, Destination.position))
            return;

        if (carryObjectAgent.enabled)
            carryObjectAgent.isStopped = false;
        else
        {
            carryObjectAgent.isStopped = false;
            carryObjectAgent.enabled = true;
        }

        if (carryingCoroutine != null)
            StopCoroutine(carryingCoroutine);
        carryingCoroutine = StartCoroutine(GetInPosition());

        IEnumerator GetInPosition()
        {
            carryObjectAgent.SetDestination(Destination.position);
            yield return new WaitUntil(() => carryObjectAgent.IsDone());
            carryObjectAgent.enabled = false;
            ReleaseAllPikmin();

            /*
             * 이전 이동 알고리즘
            var preY = transform.position.y;
            var preScale = transform.localScale;
            var partScale = preScale.x / ((Destination.position.y - preY) / .01f);

            while (preY < Destination.position.y)
            {
                preScale.x -= partScale;
                preScale.y -= partScale;
                preScale.z -= partScale;
                preY += .01f;
                transform.localScale = preScale;
                transform.Translate(0, .01f, 0);
                yield return null;
            }
            */

            var sequenceTime = 1.3f;
            var sequence = DOTween.Sequence();
            sequence.Join(transform.DOMove(Destination.position, sequenceTime).SetEase(Ease.InQuint));
            sequence.Join(transform.DOScale(0, sequenceTime).SetEase(Ease.InQuint));
            yield return new WaitUntil(() => transform.localScale.x.Equals(0));

            var startPosition = Source.position;
            startPosition.y = 0;
            transform.position = startPosition;

            sequence = DOTween.Sequence();
            sequence.Join(transform.DOScale(1, sequenceTime).SetEase(Ease.InQuint));
            yield return new WaitUntil(() => transform.localScale.x.Equals(1));

            Initialize();
        }
    }

    public override void StopInteract()
    {
        if (carryObjectAgent.enabled)
            carryObjectAgent.isStopped = true;
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
