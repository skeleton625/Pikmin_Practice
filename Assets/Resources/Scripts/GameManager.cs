using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PikminManager Pmanager;
    [SerializeField] private PikminController Pcontrol;
    [SerializeField] private CameraMovement Cmovement;

    private void Update()
    {
        Cmovement.RotateCamera();
        Pmanager.ControlPikmin();
        Pcontrol.UpdateMousePosition();
    }
}
