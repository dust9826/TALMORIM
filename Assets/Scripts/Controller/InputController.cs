using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class InputController : MonoBehaviour
{
    [SerializeField] LayerMask _floor;
    private Dictionary<KeyCode, UnityEvent> _keyDictionary;

    public UnityEvent Skill1Down;
    public UnityEvent Skill2Down;
    public UnityEvent Skill3Down;
    public UnityEvent Skill4Down;
    public UnityEvent AttackDown;
    public UnityEvent DashDown;
    public UnityEvent MoveDown;
    public UnityEvent SelectDown;
    public UnityEvent RotateRightDown;
    public UnityEvent RotateLeftDown;
    public UnityEvent ZoomUpDown;
    public UnityEvent ZoomDownDown;
    public UnityEvent StopDown;
    void Start()
    {
        KeyPreset.Instance.AddKeyDictionary(KeyCode.Q, Skill1Down, "Skill1");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.W, Skill2Down, "Skill2");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.E, Skill3Down, "Skill3");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.R, Skill4Down, "Skill4");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.A, AttackDown, "Attack");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.Space, DashDown, "Dash");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.Mouse0, SelectDown, "Select");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.Mouse1, MoveDown, "Move");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.LeftArrow, RotateRightDown, "RotateRight");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.RightArrow, RotateLeftDown, "RotateLeft");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.UpArrow, ZoomUpDown, "ZoomUpDown");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.DownArrow, ZoomDownDown, "ZoomDownDown");
        KeyPreset.Instance.AddKeyDictionary(KeyCode.S, StopDown, "Stop");
    }
    void Update()
    {
        if(Input.anyKeyDown && !KeyPreset.Instance.IsChanging)
        {
            foreach(var dic in KeyPreset.Instance.KeyDictionary)
            {
                if(Input.GetKeyDown(dic.Key))
                {
                    dic.Value.Invoke();
                }
            }
        }
        else if(Input.GetKey(KeyPreset.Instance.EventDictionary[RotateLeftDown]))
        {
            RotateLeftDown.Invoke();
        }
        else if (Input.GetKey(KeyPreset.Instance.EventDictionary[RotateRightDown]))
        {
            RotateRightDown.Invoke();
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll>0)
        {
            ZoomUpDown.Invoke();
        }
        else if(scroll<0)
        {
            ZoomDownDown.Invoke();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity, _floor))
        {
            return hit.point;
        }
        return Vector3.zero;
    }
}
