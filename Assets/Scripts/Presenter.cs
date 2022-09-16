using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Presenter : MonoBehaviour
{
    [SerializeField] InputController _inputController = null;
    [SerializeField] PlayerController _playerController = null;
    [SerializeField] CameraController _cameraController = null;
    // Start is called before the first frame update
    void Start()
    {
        _inputController.Skill1Down.AddListener(OnSkill1Down);
        _inputController.Skill2Down.AddListener(OnSkill2Down);
        _inputController.Skill3Down.AddListener(OnSkill3Down);
        _inputController.Skill4Down.AddListener(OnSkill4Down);
        _inputController.AttackDown.AddListener(OnAttackDown);
        _inputController.DashDown.AddListener(OnDashDown);
        _inputController.MoveDown.AddListener(OnMoveDown);
        _inputController.SelectDown.AddListener(OnSelectDown);
        _inputController.RotateLeftDown.AddListener(OnRotateLeftDown);
        _inputController.RotateRightDown.AddListener(OnRotateRightDown);
        _inputController.ZoomUpDown.AddListener(OnZoomUpDown);
        _inputController.ZoomDownDown.AddListener(OnZoomDownDown);
        _inputController.StopDown.AddListener(OnStopDown);
    }

    void OnSkill1Down()
    {
        Vector3 dir = _inputController.GetMousePosition();
        _playerController.Skill(1, dir);
    }
    void OnSkill2Down()
    {
        Vector3 dir = _inputController.GetMousePosition();
        _playerController.Skill(2, dir);
    }
    void OnSkill3Down()
    {
        Vector3 dir = _inputController.GetMousePosition();
        _playerController.Skill(3, dir);
    }
    void OnSkill4Down()
    {
        Vector3 dir = _inputController.GetMousePosition();
        _playerController.Skill(4, dir);
    }
    void OnAttackDown()
    {
        Vector3 dir = _inputController.GetMousePosition();
        _playerController.Skill(0, dir);
    }
    void OnDashDown()
    {
        Vector3 dir = _inputController.GetMousePosition();
        _playerController.DashPlayer(dir);
    }
    void OnMoveDown()
    {
        Vector3 dir = _inputController.GetMousePosition();
        if (dir == Vector3.zero)
            return;
        _playerController.MovePlayer(dir);
    }
    void OnSelectDown()
    {
        Debug.Log("Select");
    }
    void OnRotateLeftDown()
    {
        _cameraController.RotateCamera(false);
    }
    void OnRotateRightDown()
    {
        _cameraController.RotateCamera(true);
    }
    void OnZoomUpDown()
    {
        _cameraController.ZoomCamera(true);
    }
    void OnZoomDownDown()
    {
        _cameraController.ZoomCamera(false);
    }
    void OnStopDown()
    {
        _playerController.StopPlayer();
    }
}
