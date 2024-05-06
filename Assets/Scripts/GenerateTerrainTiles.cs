using CustomUtilityScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GenerateTerrainTiles : MonoBehaviour
{
    [SerializeField]
    private GameObject _grassTilePrefab;

    [SerializeField]
    private GameObject _obstacle;
    [SerializeField]
    private float _maxObstacleScale = 4f;

    [SerializeField]
    private Vector2Int _gridSize;

    [SerializeField]
    private NavMeshSurface _navigationSurface;

    [SerializeField]
    [Range(0, 0.8f)]
    private float _maxObjstacleDensity;

    [SerializeField]
    private GameController _gameController;

    void Start()
    {
        RefreshTerrain();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void RefreshTerrain()
    {
        if (_grassTilePrefab == null
            || (_gridSize.x == 0 && _gridSize.y == 0))
        {
            return;
        }

        int indexXMin = -1;
        int indexXMax = -1;
        int indexZMin = -1;
        int indexZMax = -1;

        indexXMin = -(_gridSize.x / 2);
        indexXMax = _gridSize.x - Mathf.Abs(indexXMin) - 1;

        indexZMin = -(_gridSize.y / 2);
        indexZMax = _gridSize.y - Mathf.Abs(indexZMin) - 1;


        System.Random rand = new System.Random((int)(DateTime.Now.Ticks / 1000));

        for (int i = indexXMin; i < indexXMax; i++)
        {
            for (int j = indexZMin; j < indexZMax; j++)
            {
                GameObject newTile = Instantiate(_grassTilePrefab, _grassTilePrefab.transform.position, Quaternion.identity);
                newTile.transform.SetParent(transform);
                newTile.transform.position = (_grassTilePrefab.transform.position +
                    new Vector3((i * 2), 0, (j * 2)));

                int absI = Math.Abs(i);
                int absJ = Math.Abs(j);

                if (absI < indexXMax && absI > indexZMax - 2
                    && absJ < indexZMax && absJ > indexZMax - 2)
                {
                    _gameController.SpawnEnemy(newTile.transform.position);
                }

                if (Mathf.Abs(i) >= (0.8f * indexXMax) || Mathf.Abs(j) >= (0.8f * indexZMax))
                {
                    continue;
                }

                float randomNumber = (float)rand.NextDouble();

                if (randomNumber <= _maxObjstacleDensity)
                {
                    GameObject obstacle = Instantiate(_obstacle, _obstacle.transform.position, Quaternion.identity);
                    obstacle.transform.SetParent(transform);
                    Vector3 pos = newTile.transform.position;
                    obstacle.transform.position = pos.SetY(_obstacle.transform.position.y);
                    obstacle.SetActive(true);

                    obstacle.transform.Rotate(0, 360 * (float)rand.NextDouble(), 0);
                    obstacle.transform.localScale = (1 + (float)((_maxObstacleScale - 1) * rand.NextDouble())) * Vector3.one;
                }
            }
        }

        _navigationSurface.BuildNavMesh();
        _gameController.InitEnemies();
    }
}
