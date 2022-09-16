using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossQuestion : MonoBehaviour
{
    [SerializeField] GameObject _canvas;
    [SerializeField] Player _player;
    [SerializeField] Transform _bossRoom;
    [SerializeField] Transform _hallWay;
    [SerializeField] Door _door;
    [SerializeField] GameObject _camera;
    [SerializeField] CameraController _cameraController;
    [SerializeField] Transform _openTransform;
    public void GotoBoss(bool gotoBoss)
    {
        _canvas.SetActive(false);
        if(gotoBoss)
        {
            StartCoroutine(OpenCameraShow());
        }
        else
        {
            _player.Move(_hallWay.position);
            _player.Status = STATUS.NULL;
        }
    }

    IEnumerator OpenCameraShow()
    {
        Vector3 originPosition = _camera.transform.localPosition;
        Quaternion originRotation = _camera.transform.localRotation;
        _camera.transform.parent = _camera.transform.parent.parent;
        _player.Status = STATUS.WAIT;
        _camera.transform.position = _openTransform.position;
        _camera.transform.rotation = _openTransform.rotation;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(_cameraController.ShakeCamera(1.5f, 0.2f));
        _door.CloseDoor(false);
        yield return new WaitForSeconds(2.0f);
        _camera.transform.parent = _cameraController.gameObject.transform;
        _camera.transform.localPosition = originPosition;
        _camera.transform.localRotation = originRotation;
        _player.Status = STATUS.NULL;
        _player.Move(_bossRoom.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            _player.Status = STATUS.WAIT;
            _player.Move(_player.gameObject.transform.position);
            _canvas.SetActive(true);
        }
    }
}
