using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PikminController Pcontrol = null;
    [SerializeField] private PikminManager Pmanager = null;
    [SerializeField] private CameraMovement Cmovement = null;

    private void Update()
    {
        Cmovement.RotateCamera();
        Pmanager.ControlPikmin();
        Pcontrol.UpdateMousePosition();
    }
}
