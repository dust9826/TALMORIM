using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum STATUS
{
    ATTACK,
    DASH,
    RUN,
    WAIT,
    NULL
}

public class Player : Attackable
{
    [SerializeField] SkillInfo[] _skills = null;
    [SerializeField] Image[] _coolImage = null;
    [SerializeField] Text[] _coolText = null;
    [SerializeField] Animator _ani = null;
    [SerializeField] LayerMask _enemy;
    [SerializeField] CapsuleCollider _collider = null;
    [SerializeField] AudioClip _swordAudio = null;
    [SerializeField] CameraController _camera = null;
    [SerializeField] GameObject _gameOver = null;
    private STATUS _status = STATUS.NULL;
    private NavMeshAgent _nav;
    private delegate IEnumerator SkillMethod(int code);
    private SkillMethod[] _skillMethod = new SkillMethod[5];
    private float _dashCoolDown = 0f;
    private float _dashCoolTime = 2f;
    private int _dashMaxNum = 2;
    private int _dashNum = 2;
    public STATUS Status
    {
        get { return _status; }
        set { _status = value; }
    }
    protected override void Start()
    {
        base.Start();
        _nav = GetComponent<NavMeshAgent>();
        _collider = GetComponent<CapsuleCollider>();
        _skillMethod[0] = new SkillMethod(Skillmethod0); 
        _skillMethod[1] = new SkillMethod(Skillmethod1); 
        _skillMethod[2] = new SkillMethod(Skillmethod2); 
        _skillMethod[3] = new SkillMethod(Skillmethod3); 
        _skillMethod[4] = new SkillMethod(Skillmethod4);
        _gameOver.SetActive(false);
        StartCoroutine(DashCoolDown());
    }

    private void Update()
    {
        if (Vector3.Distance(_nav.destination + new Vector3(0, 1, 0), transform.position) <= 0.5f)
            _ani.SetBool("isWalk", false);
    }

    public void Skill(int code)
    {
        if (_skills[code].CoolDown > 0f)
        {
            Debug.Log("CoolTime");
            return;
        }
        Move(transform.position);
        _status = STATUS.ATTACK;
        _skills[code].CoolDown = _skills[code].CoolTime;

        StartCoroutine(SkillEffect(code));
        StartCoroutine(SetStatus(code));
        StartCoroutine(CoolDown(code));
        StartCoroutine(_skillMethod[code](code));

        _ani.SetFloat("SkillCode", (float)code /4);
        _ani.SetBool("isSkill", true);
    }

    public void Move(Vector3 direction)
    {
        _nav.SetDestination(direction);
        _ani.SetBool("isWalk", true);
    }
    public IEnumerator SetSpeed(float speed, float time)
    {
        float temp = _nav.speed;
        _nav.speed = speed;
        _ani.SetBool("isDash", true);
        _status = STATUS.DASH;
        yield return new WaitForSeconds(time);
        _nav.speed = temp;
        _ani.SetBool("isDash", false);
        _status = STATUS.NULL;
    }
    public IEnumerator SetInvincibility(float time)
    {
        _collider.enabled = false;
        yield return new WaitForSeconds(time);
        _collider.enabled = true;
    }

    IEnumerator SkillEffect(int code)
    {
        yield return new WaitForSeconds(_skills[code].Delay);
        GameObject vfx = GameObject.Instantiate(_skills[code].SkillPrefab, transform.position, transform.rotation) as GameObject;
        vfx.transform.position += new Vector3(0, -1, 0);
        GameObject.Destroy(vfx, _skills[code].Effect_LiveTime);
    }
    IEnumerator SetStatus(int code)
    {
        yield return new WaitForSeconds(_skills[code].Skill_LiveTime);
        _status = STATUS.NULL;
        _ani.SetFloat("SkillCode", 0);
        _ani.SetBool("isSkill", false);
    }
    IEnumerator CoolDown(int code)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _skills[code].CoolDown -= 0.1f;

            if (_skills[code].CoolDown <= 0)
            {
                if (_coolImage[code] != null) _coolImage[code].fillAmount = 0;
                if (_coolText[code] != null)
                {
                    switch(code)
                    {
                        case 0:
                            _coolText[code].text = KeyPreset.Instance.StringToKeyCode("Attack").ToString();
                            break;
                        case 1:
                            _coolText[code].text = KeyPreset.Instance.StringToKeyCode("Skill1").ToString();
                            break;
                        case 2:
                            _coolText[code].text = KeyPreset.Instance.StringToKeyCode("Skill2").ToString();
                            break;
                        case 3:
                            _coolText[code].text = KeyPreset.Instance.StringToKeyCode("Skill3").ToString();
                            break;
                        case 4:
                            _coolText[code].text = KeyPreset.Instance.StringToKeyCode("Skill4").ToString();
                            break;
                    }
                }
                break;
            }

            if (_coolImage[code] != null) _coolImage[code].fillAmount = _skills[code].CoolDown / _skills[code].CoolTime;
            if (_coolText[code] != null) _coolText[code].text = _skills[code].CoolDown.ToString("F1");
        }
    }

    IEnumerator Skillmethod0(int code)
    {
        yield return new WaitForSeconds(_skills[code].Delay);
        if (SoundController.Instance != null) SoundController.Instance.Play(_swordAudio);
        Collider[] colliders = Physics.OverlapSphere(_skills[code].Position.position, _skills[code].Range, _enemy);
        foreach (Collider collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();
            entity.Damage(_str * _skills[code].StrMulti);
            //Debug.Log(entity);
        }
    }
    IEnumerator Skillmethod1(int code)
    {
        yield return new WaitForSeconds(_skills[code].Delay);
        Collider[] colliders = Physics.OverlapSphere(_skills[code].Position.position, _skills[code].Range, _enemy);
        foreach (Collider collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();
            entity.Damage(_str * _skills[code].StrMulti);
            //Debug.Log(entity);
        }
    }
    IEnumerator Skillmethod2(int code)
    {
        yield return new WaitForSeconds(_skills[code].Delay);
        float temp = _nav.speed;
        _nav.speed = 14;
        _ani.SetBool("isRun", true);
        yield return new WaitForSeconds(15.0f);
        _nav.speed = temp;
        _ani.SetBool("isRun", false);
    }
    IEnumerator Skillmethod3(int code)
    {
        yield return new WaitForSeconds(_skills[code].Delay);
        StartCoroutine(_camera.ShakeCamera(_skills[code].Skill_LiveTime, 0.3f));
        float radian = (_skills[code].Position.eulerAngles.y) * Mathf.Deg2Rad;
        Vector3 position = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * _skills[code].Range / 2;
        Collider[] colliders = Physics.OverlapBox(_skills[code].Position.position + position, new Vector3(1, 1, _skills[code].Range / 2), _skills[code].Position.rotation, _enemy);
        foreach (Collider collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();
            entity.Damage(_str * _skills[code].StrMulti);
            //Debug.Log(entity);
        }
    }
    IEnumerator Skillmethod4(int code)
    {
        yield return new WaitForSeconds(_skills[code].Delay);
        if (SoundController.Instance != null) SoundController.Instance.Play(_swordAudio);
        float radian = (_skills[code].Position.eulerAngles.y) * Mathf.Deg2Rad;
        Vector3 position = new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian)) * _skills[code].Range / 2;
        Collider[] colliders = Physics.OverlapBox(_skills[code].Position.position + position, new Vector3(1, 1, _skills[code].Range / 2), _skills[code].Position.rotation, _enemy);
        foreach (Collider collider in colliders)
        {
            Entity entity = collider.GetComponent<Entity>();
            entity.Damage(_str * _skills[code].StrMulti);
            //Debug.Log(entity);
        }
    }

    public void Dash(Vector3 move)
    {
        if (--_dashNum < 0)
        {
            _dashNum = 0;
            return;
        }
        if(_dashNum==1)
        {
            _dashCoolDown = _dashCoolTime;
        }
        if (_coolText[5] != null) _coolText[5].text = _dashNum.ToString();
        StartCoroutine(SetSpeed(1000, 0.14f));
        StartCoroutine(SetInvincibility(0.5f));
        Move(transform.position + move);
    }
    IEnumerator DashCoolDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            _dashCoolDown -= 0.1f;
            if (_dashCoolDown <= 0)
            {
                _dashCoolDown = _dashCoolTime;
                _dashNum++;
                if (_dashNum >= _dashMaxNum)
                {
                    _dashNum = _dashMaxNum;
                    _dashCoolDown = 0f;
                }
                if (_coolImage[5] != null) _coolImage[5].fillAmount = 0;
                if (_coolText[5] != null) _coolText[5].text = _dashNum.ToString();
            }

            if (_coolImage[5] != null) _coolImage[5].fillAmount = _dashCoolDown / _dashCoolTime;
        }
    }
    protected override void Dead()
    {
        _gameOver.SetActive(true);
        _status = STATUS.WAIT;
        KeyPreset.Instance.RemoveAll();
        //SceneManager.LoadScene("TitleScene");
    }
}
