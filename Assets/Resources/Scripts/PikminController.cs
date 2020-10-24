using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikminController : MonoBehaviour
{
    [SerializeField] private Vector3 targetOffset = Vector3.zero;
    [SerializeField] private Transform Target;
    [SerializeField] private Transform Follower;
    [SerializeField] private Transform visualCylinder;
    [SerializeField] private LineRenderer TargetLine;

    private int colliderLayer;
    private Camera camera = default;
    private readonly int linePointsCount = 5;

    private void Awake()
    {
        /* Layer Collider 제외 숫자 */
        colliderLayer = -257;
        camera = FindObjectOfType<Camera>();
        TargetLine.positionCount = linePointsCount;
    }

    public void UpdateMousePosition()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, colliderLayer))
        {
            Target.position = hit.point + targetOffset;
            Target.up = Vector3.Lerp(Target.up, hit.normal, .3f);
            visualCylinder.position = Target.position;

            for (int i = 0; i < linePointsCount; i++)
            {
                Vector3 linePos = Vector3.Lerp(Follower.position, Target.position, i * linePointsCount);
                TargetLine.SetPosition(i, linePos);
            }
        }
    }
}
