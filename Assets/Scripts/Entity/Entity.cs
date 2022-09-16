using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] protected float _maxHp = 100f;
    protected float _nowHp = 100f;
    [SerializeField] Image _hpBar = null;

    protected virtual void Start()
    {
        _nowHp = _maxHp;        
    }
    public void Damage(float dmg)
    {
        _nowHp -= dmg;
        if (_nowHp > _maxHp)
        {
            _nowHp = _maxHp;
        }
        if (_nowHp <= 0)
        {
            _nowHp = 0;
            Dead();
        }
        _hpBar.fillAmount = _nowHp / _maxHp;
    }

    protected virtual void Dead()
    {

    }
}
