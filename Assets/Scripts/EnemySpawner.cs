using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab = null;
    [SerializeField] GameObject _player = null;
    [SerializeField] Transform[] _spawners = null;
    private List<GameObject> enemies = new List<GameObject>();
    private bool isActive = false;

    public bool isClear()
    {
        if(enemies.Count == 0 && isActive == true)
        {
            return true;
        }
        return false;
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }
    public void Spawn()
    {
        foreach(Transform spawner in _spawners)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, spawner.position, Quaternion.identity, this.transform);
            newEnemy.GetComponent<Monster>().SetPlayer(_player);
            newEnemy.GetComponent<Monster>().SetSpawner(this);
            enemies.Add(newEnemy);
        }
        isActive = true;
    }
}
