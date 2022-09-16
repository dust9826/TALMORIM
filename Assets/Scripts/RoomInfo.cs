using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILECODE { NULL, FLOOR, DOOR, MAIN }
public class RoomInfo : MonoBehaviour
{
    [SerializeField] Vector2Int _mapSize;
    [SerializeField] Vector2Int[] _doorPositions;
    [SerializeField] EnemySpawner _spawner = null;
    [SerializeField] Door[] _doors;
    [SerializeField] bool _isBoss;
    [SerializeField] GameObject _boss;
    [SerializeField] Transform[] _openTransform;
    [SerializeField] Transform[] _closeTransform;
    [SerializeField] GameObject _camera;
    [SerializeField] CameraController _cameraController;
    [SerializeField] AudioClip _clip;
    [SerializeField] Player _player;
    [SerializeField] GameObject _bossHP;
    [SerializeField] GameObject _bossName;
    private TILECODE[,] map;
    private bool isClear = false;
    private bool isActive = false;
    private Boss boss = null;
    void Awake()
    {
        map = new TILECODE[(int)_mapSize.x, (int)_mapSize.y];
        for(int x = 0; x < _mapSize.x; x++)
        {
            for(int y=0;y< _mapSize.y;y++)
            {
                map[x, y] = TILECODE.FLOOR;
            }
        }
        map[0, 0] = TILECODE.MAIN;
        foreach (Vector2Int doorPosition in _doorPositions)
        {
            map[doorPosition.x, doorPosition.y] = TILECODE.DOOR;
        }
        if (_isBoss)
        {
            boss = _boss.GetComponent<Boss>();
            _bossHP.SetActive(false);
            _bossName.SetActive(false);
        }
    }

    private void Update()
    {
        if(_spawner != null)
        {
            if (_spawner.isClear() && !isClear)
            {
                StartCoroutine(CloseCameraShow());
                foreach (Door door in _doors)
                {
                    door.CloseDoor(false);
                }
                isClear = true;
            }
        }
       if(_isBoss == true)
        {
            if (boss.IsDead && !isClear)
            {
                StartCoroutine(CloseCameraShow());
                foreach (Door door in _doors)
                {
                    door.CloseDoor(false);
                }
                isClear = true;
                _bossHP.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag.Equals("Player") && !isActive)
        {
            StartCoroutine(OpenCameraShow());
            foreach(Door door in _doors)
            {
                door.CloseDoor(true);
            }
            StartCoroutine(Spawn());
            isActive = true;
        }
    }

    IEnumerator OpenCameraShow()
    {
        Vector3 originPosition = _camera.transform.localPosition;
        Quaternion originRotation = _camera.transform.localRotation;
        _camera.transform.parent = _camera.transform.parent.parent;
        _player.Status = STATUS.WAIT;
        foreach (Transform transform in _openTransform)
        {
            _camera.transform.position = transform.position;
            _camera.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(_cameraController.ShakeCamera(0.5f, 0.2f));
            yield return new WaitForSeconds(0.5f);
        }
        _camera.transform.parent = _cameraController.gameObject.transform;
        _camera.transform.localPosition = originPosition;
        _camera.transform.localRotation = originRotation;
        _player.Status = STATUS.NULL;
    }
    IEnumerator CloseCameraShow()
    {
        Vector3 originPosition = _camera.transform.localPosition;
        Quaternion originRotation = _camera.transform.localRotation;
        _camera.transform.parent = _camera.transform.parent.parent;
        _player.Status = STATUS.WAIT;
        foreach (Transform transform in _closeTransform)
        {
            _camera.transform.position = transform.position;
            _camera.transform.rotation = transform.rotation;
            yield return new WaitForSeconds(1.5f);
            StartCoroutine(_cameraController.ShakeCamera(0.5f, 0.2f));
            yield return new WaitForSeconds(0.5f);
        }
        _camera.transform.parent = _cameraController.gameObject.transform;
        _camera.transform.localPosition = originPosition;
        _camera.transform.localRotation = originRotation;
        _player.Status = STATUS.NULL;
    }
    IEnumerator Spawn()
    {

        if (_isBoss)
        {
            _boss.SetActive(true);
            _bossHP.SetActive(true);
            _bossName.SetActive(true);
            if (SoundController.Instance != null) SoundController.Instance.PlayMusic(_clip);
            yield return new WaitForSeconds(1.5f);
            _bossName.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(1.5f);
            _spawner.Spawn();
        }
    }
}
