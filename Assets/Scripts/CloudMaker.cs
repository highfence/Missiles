using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMaker : MonoBehaviour
{
    GameObject[] _cloudPool = null;
    public float _accTime = 0f;
    int _poolSize = 20;
    Vector2 _currentPlayerPosition = Vector2.zero;

    public void Initialize()
    {
        _cloudPool = new GameObject[_poolSize];

        for (var i = 0; i < _poolSize; ++i)
        {
            _cloudPool[i] = Instantiate(Resources.Load("Prefabs/Cloud")) as GameObject;
            _cloudPool[i].name = "Cloud_" + i;
            _cloudPool[i].SetActive(false);
        }
    }

    private void Update()
    {
        MakeCloud();
        CheckDeadCloud();
    }

    private void CheckDeadCloud()
    {
        for (var i = 0; i < _poolSize; ++i)
        {
            var cloud = _cloudPool[i];

            if (cloud.activeSelf == false)
                continue;

            var instance = cloud.GetComponent<Cloud>();

            if (instance._isCloudEnd == true)
            {
                instance.Clear();
                cloud.SetActive(false);
            }
        }
    }

    private void MakeCloud()
    {
        _accTime += Time.deltaTime;

        if (_accTime > 1.5)
        {
            for (var i = 0; i < _poolSize; ++i)
            {
                var cloud = _cloudPool[i];

                if (cloud.activeSelf == true)
                    continue;

                var randomUnitVec = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
                randomUnitVec.Normalize();
                var spawnPosition = new Vector3(_currentPlayerPosition.x + randomUnitVec.x * 40f, _currentPlayerPosition.y + randomUnitVec.y * 20f, 0f);

                var randomSpriteNumber = UnityEngine.Random.Range(0, 4);

                cloud.gameObject.SetActive(true);
                cloud.GetComponent<Cloud>().Spawn(randomSpriteNumber);
                cloud.transform.position = spawnPosition;

                _accTime = 0f;
                break;
            }

        }
    }

    public void DistributePlayerInfo(Vector3 playerPosition)
    {
        _currentPlayerPosition = new Vector2(playerPosition.x, playerPosition.y);
    }
}
