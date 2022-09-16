using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SKEL_STATUS { CHASE, ATTACK }
public class Monster : Attackable
{
    [SerializeField] GameObject _player = null;
    [SerializeField] SkillInfo _skill = null;
    [SerializeField] Animator _ani;
    [SerializeField] LayerMask _enemy;
    [SerializeField] Vector3 _offset;
    [SerializeField] EnemySpawner _spawner = null;
    private NavMeshAgent _nav;
    private SKEL_STATUS _status;

    protected override void Start()
    {
        base.Start();
        _nav = GetComponent<NavMeshAgent>();
        _status = SKEL_STATUS.CHASE;
    }
    private void Update()
    {
        if(_player != null)
        {
            float distance = Vector3.Distance(this.transform.position, _player.transform.position + _offset);

            if (_status == SKEL_STATUS.CHASE)
            {
                if (distance < 10f)
                {
                    _nav.SetDestination(_player.transform.position);
                }
            }

            if (distance < 3f)
            {
                Skill();
            }
        }
    }

    public void SetPlayer(GameObject player)
    {
        _player = player;
    }
    public void SetSpawner(EnemySpawner spawner)
    {
        _spawner = spawner;
    }

    protected override void Dead()
    {
        if (_spawner != null) _spawner.RemoveEnemy(this.gameObject);
        Destroy(this.gameObject);
    }
    public void Skill()
    {
        if (_skill.CoolDown > 0f)
        {
            return;
        }
        _nav.SetDestination(transform.position);
        _status = SKEL_STATUS.ATTACK;
        _skill.CoolDown = _skill.CoolTime;

        StartCoroutine(SkillEffect());
        StartCoroutine(SetStatus());
        StartCoroutine(CoolDown());
        StartCoroutine(AttackPlayer());

        _ani.SetBool("isAttack", true);
    }
    IEnumerator SkillEffect()
    {
        yield return new WaitForSeconds(_skill.Delay);
        GameObject vfx = GameObject.Instantiate(_skill.SkillPrefab, transform.position, transform.rotation) as GameObject;
        vfx.transform.position += new Vector3(0, -1, 0);
        GameObject.Destroy(vfx, _skill.Effect_LiveTime);
    }
    IEnumerator SetStatus()
    {
        yield return new WaitForSeconds(_skill.Skill_LiveTime);
        _status = SKEL_STATUS.CHASE;
        _ani.SetBool("isAttack", false);
    }
    IEnumerator CoolDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _skill.CoolDown -= 0.1f;

            if (_skill.CoolDown <= 0)
            {
                break;
            }
        }
    }
    IEnumerator AttackPlayer()
    {
        yield return new WaitForSeconds(_skill.Delay);
        Collider[] colliders = Physics.OverlapSphere(_skill.Position.position, _skill.Range, _enemy);
        foreach (Collider collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();
            entity.Damage(_str * _skill.StrMulti);
            //Debug.Log(entity);
        }
    }
}
