using CustomUtilityScripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private EnemyController _enemyPrefab;

    [SerializeField]
    private PlayerMovement _player;

    [SerializeField]
    private int _maxNumberOfEnemies = 4;

    private List<EnemyController> _enemies;

    public void SpawnEnemy(Vector3 position)
    {
        if (_enemies != null && _enemies.Count >= _maxNumberOfEnemies)
        {
            return;
        }

        EnemyController newEnemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
        newEnemy.transform.position = position.SetY(_player.transform.position.y);
        newEnemy.SetPlayerTarget(_player);

        if (_enemies == null)
        {
            _enemies = new();
        }

        _enemies.Add(newEnemy);
    }

    public void InitEnemies()
    {
        foreach (var enemy in _enemies)
        {
            enemy.Init();
        }
    }
}
