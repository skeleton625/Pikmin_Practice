using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikminController : MonoBehaviour
{
    [HideInInspector] public Vector3 HitPoint = Vector3.zero;
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private Transform Target;
    [SerializeField] private Transform Follower;
    [SerializeField] private LineRenderer TargetLine;

    private int colliderLayer;
    private readonly int linePointsCount = 5;

    private Camera camera = default;
    private void Awake()
    {
        camera = Camera.main;
        colliderLayer = -257;
        TargetLine.positionCount = linePointsCount;
    }

    private void Update()
    {
        UpdateMousePosition();
    }

    private void UpdateMousePosition()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit, 1000, colliderLayer))
        {
            HitPoint = hit.point;
            Target.position = HitPoint + targetOffset;
            Target.up = Vector3.Lerp(Target.up, hit.normal, .3f);

            float lerpValue = 1f / linePointsCount;
            for(int i = 0; i < linePointsCount; i++)
            {
                Vector3 linePos = Vector3.Lerp(Follower.position, Target.position, i * linePointsCount);
                TargetLine.SetPosition(i, linePos);
            }
        }
    }
}
