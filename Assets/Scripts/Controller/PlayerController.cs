using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject _playerObject = null;
    [SerializeField] float distance = 0f;
    [SerializeField] int _dash_count;
    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = _playerObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(Vector3 direction)
    {
        if (_player.Status == STATUS.NULL)
        {
            _player.Move(direction);
        }
    }
    public void DashPlayer(Vector3 direction)
    {
        Vector3 move = (direction + new Vector3(0,1,0) - _playerObject.transform.position).normalized * distance;
        if (_player.Status == STATUS.NULL)
        {
            _player.Dash(move);
        }
    }
    public void StopPlayer()
    {
        if (_player.Status != STATUS.ATTACK)
        {
            _player.Move(_playerObject.transform.position - new Vector3(0,1,0));
        }
    }

    public void Skill(int code, Vector3 direction)
    {
        Vector3 dir = (direction + new Vector3(0, 1, 0) - _playerObject.transform.position).normalized;
        float angle = Mathf.Atan2(- dir.z, dir.x) * Mathf.Rad2Deg + 90;
        _playerObject.transform.eulerAngles = new Vector3(0, angle, 0);
        if (_player.Status != STATUS.ATTACK)
        {
            _player.Skill(code);
        }
    }
}
