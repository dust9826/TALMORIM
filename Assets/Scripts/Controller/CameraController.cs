using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] GameObject _target = null;
    [SerializeField] GameObject _mainCamera;
    [SerializeField] float _rotateSpeed = 100f;
    [SerializeField] float _zoomSpeed = 300f;
    [SerializeField] Vector3 _offset;

    private void Start()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main.gameObject;
    }
    void Update()
    {
        transform.position = _target.transform.position + _offset;
    }

    public void RotateCamera(bool isRIght)
    {
        if(isRIght)
        {
            transform.eulerAngles += new Vector3(0, 1, 0) * Time.deltaTime * _rotateSpeed;
        }
        else
        {
            transform.eulerAngles += new Vector3(0, -1, 0) * Time.deltaTime * _rotateSpeed;
        }
    }
    float Distance;
    public void ZoomCamera(bool isUp)
    {
        if(isUp)
        {
            _mainCamera.transform.localPosition -= new Vector3(0, 0.4f, -0.3f) * Time.deltaTime * _zoomSpeed;
        }
        else
        {
            _mainCamera.transform.localPosition += new Vector3(0, 0.4f, -0.3f) * Time.deltaTime * _zoomSpeed;
        }

        if(_mainCamera.transform.localPosition.y > 16)
        {
            _mainCamera.transform.localPosition = new Vector3(0, 16, -12);
        }
        if(_mainCamera.transform.localPosition.y < 4)
        {
            _mainCamera.transform.localPosition = new Vector3(0, 4, -3);
        }
    }  
    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        Vector3 originPosition = _mainCamera.transform.localPosition;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;
            float z = Random.Range(-1, 1) * magnitude;

            _mainCamera.transform.localPosition = originPosition + new Vector3(x, y, z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        _mainCamera.transform.localPosition = originPosition;
    }
}
