using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum BOSS_STATUS { IDLE, CHASE, DASH, HEADATTACK }

[System.Serializable]
public class PatternInfo
{
    [SerializeField] BOSS_STATUS patternCode;
    public BOSS_STATUS PatternCode
    {
        get { return patternCode; }
    }
    [SerializeField] float minRange = 0f;
    public float MinRange
    {
        get { return minRange; }
    }
    [SerializeField] float maxRange = 10f;
    public float MaxRange
    {
        get { return maxRange; }
    }
    [SerializeField] int probability = 5;
    public int Probability
    {
        get { return probability; }
    }
    [SerializeField] float liveTime = 0.5f;
    public float LiveTime
    {
        get { return liveTime; }
    }
}

public class Boss : Attackable
{
    [SerializeField] GameObject _player = null;
    [SerializeField] PatternInfo[] _patterninfos = null;
    [SerializeField] float _patternWaitTime = 2f;
    [SerializeField] GameObject _effect = null;
    [SerializeField] CameraController _camera = null;
    [SerializeField] Animator _ani = null;
    [SerializeField] GameObject _gameClear = null;
    private NavMeshAgent _nav;
    private BOSS_STATUS _status = BOSS_STATUS.IDLE;
    private delegate IEnumerator PatternDelegate();
    private PatternDelegate[] patterns = new PatternDelegate[4];
    private Vector3 _offset = new Vector3(0,0.5f,0);
    private float _delta = 0f;
    private bool _isDead = false;
    public bool IsDead { get { return _isDead; } }
    protected override void Start()
    {
        base.Start();
        _nav = GetComponent<NavMeshAgent>();
        patterns[(int)BOSS_STATUS.HEADATTACK] = new PatternDelegate(HeadAttack);
        patterns[(int)BOSS_STATUS.DASH] = new PatternDelegate(Dash);
        patterns[(int)BOSS_STATUS.CHASE] = new PatternDelegate(Chase);
        //patterns[(int)BOSS_STATUS.IDLE] = null;
        _gameClear.SetActive(false);
        StartCoroutine(SelectStatus(_patternWaitTime));
    }
    private void Update()
    {
        if(_status == BOSS_STATUS.IDLE)
        {
            _nav.SetDestination(transform.position);
        }
        if(_status == BOSS_STATUS.CHASE)
        {
            if(_player != null) _nav.SetDestination(_player.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            other.GetComponent<Entity>().Damage(_str);
            _delta = 0f;
        }
    }
    private void  OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _delta += Time.deltaTime;
            if(_delta > 2f)
            {
                other.GetComponent<Entity>().Damage(_str);
                _delta = 0f;
            }
        }
    }
    protected override void Dead()
    {
        _isDead = true;
        _gameClear.SetActive(true);
        KeyPreset.Instance.RemoveAll();
        Destroy(this.gameObject);
    }

    IEnumerator HeadAttack()
    {
        float speed = _nav.speed;
        float angularSpeed = _nav.angularSpeed;
        _nav.speed = 20f;
        _nav.angularSpeed = 1000f;
        Vector3 dir = Vector3.zero;
        if (_player != null) dir = (_player.transform.position + _offset - transform.position).normalized * 2f;
        _nav.SetDestination(transform.position + dir);
        StartCoroutine(_camera.ShakeCamera(_patterninfos[(int)BOSS_STATUS.HEADATTACK].LiveTime, 0.7f));
        _ani.SetBool("isDash", true);
        yield return new WaitForSeconds(_patterninfos[(int)BOSS_STATUS.HEADATTACK].LiveTime);
        _nav.speed = speed;
        _nav.angularSpeed = angularSpeed;
        _ani.SetBool("isDash", false);
    }
    IEnumerator Chase()
    {
        _ani.SetBool("isChase", true);
        yield return new WaitForSeconds(_patterninfos[(int)BOSS_STATUS.CHASE].LiveTime + _patternWaitTime);
        _ani.SetBool("isChase", false);
    }
    IEnumerator Dash()
    {
        float speed = _nav.speed;
        float angularSpeed = _nav.angularSpeed;
        _nav.speed = 20f;
        _nav.angularSpeed = 1000f;
        Vector3 dir = Vector3.zero;
        if (_player != null) dir = (_player.transform.position + _offset - transform.position).normalized * 20f;
        _nav.SetDestination(transform.position + dir);
        StartCoroutine(_camera.ShakeCamera(_patterninfos[(int)BOSS_STATUS.DASH].LiveTime, 0.7f));
        _ani.SetBool("isDash", true);
        yield return new WaitForSeconds(_patterninfos[(int)BOSS_STATUS.DASH].LiveTime);
        _nav.speed = speed;
        _nav.angularSpeed = angularSpeed;
        _ani.SetBool("isDash", false);
    }

    IEnumerator SelectStatus(float time)
    {
        yield return new WaitForSeconds(_patterninfos[0].LiveTime);
        while (true)
        {
            int probability = 0;
            List<PatternInfo> patternInfos = new List<PatternInfo>();
            float distance = 0f;
            if (_player != null) distance = Vector3.Distance(this.transform.position, _player.transform.position + _offset);
            foreach (PatternInfo patterninfo in _patterninfos)
            {
                if(distance >= patterninfo.MinRange && distance <= patterninfo.MaxRange)
                {
                    probability += patterninfo.Probability;
                    patternInfos.Add(patterninfo);
                }
            }
            int code = Random.Range(0, probability);

            for (int i=0; i < patternInfos.Count;i++)
            {
                code -= patternInfos[i].Probability;
                if (code < 0)
                {
                    Debug.Log(patternInfos[i].PatternCode);
                    _nav.SetDestination(transform.position);
                    _status = patternInfos[i].PatternCode;
                    StartCoroutine(patterns[(int)_status]());
                    yield return new WaitForSeconds(patternInfos[i].LiveTime);
                    break;
                }
            }
            yield return new WaitForSeconds(time);
        }
    }
}
