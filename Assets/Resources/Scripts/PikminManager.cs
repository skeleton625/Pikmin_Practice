using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;


[System.Serializable] public class PikminEvent : UnityEvent<int> { }
[System.Serializable] public class PlayerEvent : UnityEvent<Vector3> { } 
public class PikminManager : MonoBehaviour
{
    [SerializeField] private SphereCollider CylinderCollider = null;
    [SerializeField] private Transform PikminThrowPosition = null;
    [SerializeField] private Transform VisualCylinder = null;
    [SerializeField] private Pikmin PikminPrefab = null;

    private PikminController controller = null;
    private List<Pikmin> playerPikminList = null;
    private List<Pikmin> allPikminList = null;

    public static PikminManager instance = null;

    private void Awake()
    {
        instance = this;

        InitializePikmin();
    }

    private void InitializePikmin()
    {
        allPikminList = new List<Pikmin>();
        playerPikminList = new List<Pikmin>();
        controller = GetComponent<PikminController>();

        PikminSpawner[] spawners = FindObjectsOfType(typeof(PikminSpawner)) as PikminSpawner[];
        
        for(int i = 0; i < spawners.Length; i++)
        {
            List<Pikmin> partPikminList = spawners[i].SpawnPikmin(PikminPrefab, i);
            allPikminList.AddRange(partPikminList);
        }
    }

    public void GetPikmin(Pikmin pikmin)
    {
        playerPikminList.Add(pikmin);
    }

    public void SetWhistleCylinder(bool on)
    {
        if (on)
        {
            CylinderCollider.enabled = true;
            VisualCylinder.localScale = Vector3.zero;
            VisualCylinder.DOScaleX(4, .4f);
            VisualCylinder.DOScaleZ(4, .4f);
            VisualCylinder.DOScaleY(0.7f, .3f).SetDelay(.2f);
        }
        else
        {
            CylinderCollider.enabled = false;
            VisualCylinder.DOKill();
            VisualCylinder.DOScaleX(0, .15f);
            VisualCylinder.DOScaleZ(0, .15f);
            VisualCylinder.DOScaleY(0, .05f);
        }
    }

    public void ControlPikmin()
    {
        if (Input.GetMouseButtonDown(1))
            SetWhistleCylinder(true);
        if (Input.GetMouseButtonUp(1))
            SetWhistleCylinder(false);

        if (Input.GetMouseButtonDown(0))
        {
            if (playerPikminList.Count < 1)
                return;

            for(int i = 0; i < playerPikminList.Count; i++)
            {
                if(playerPikminList[i].agent.IsDone())
                {
                    Pikmin pikmin = playerPikminList[i];
                    playerPikminList.RemoveAt(i);
                    pikmin.agent.enabled = false;
                    pikmin.transform.DOMove(PikminThrowPosition.position, .05f);
                    pikmin.Throw(VisualCylinder.position, .5f, .05f);
                    break;
                }
            }
            
            /* Pikmin 던지는 소리, 움직이는 소리 필요 */
        }
    }
}
