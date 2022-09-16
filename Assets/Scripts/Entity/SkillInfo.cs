using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SkillInfo
{
    [SerializeField] float _delay = 0f;
    public float Delay
    {
        get { return _delay; }
    }
    [SerializeField] float _effect_liveTime = 1f;
    public float Effect_LiveTime
    {
        get { return _effect_liveTime; }
    }
    [SerializeField] float _skill_liveTime = 1f;
    public float Skill_LiveTime
    {
        get { return _skill_liveTime; }
    }
    [SerializeField] float _strMulti = 1f;
    public float StrMulti
    {
        get { return _strMulti; }
    }
    [SerializeField] GameObject _skillPrefab = null;
    public GameObject SkillPrefab
    {
        get { return _skillPrefab; }
    }

    [SerializeField] float _coolTime = 3f;
    public float CoolTime
    {
        get { return _coolTime; }
    }
    [SerializeField] float _coolDown = 0f;
    public float CoolDown
    {
        get { return _coolDown; }
        set { _coolDown = value; }
    }
    [SerializeField] Transform _position;
    public Transform Position
    {
        get { return _position; }
        set { _position = value; }
    }
    [SerializeField] float _range = 0f;
    public float Range
    {
        get { return _range; }
        set { _range = value; }
    }
}