using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileShooter : MonoBehaviour
{
    private float _accTime = 0f;
    GameObject[] _missilePool = null;
    private int _poolSize = 20;
    Vector2 _currentPlayerPosition = Vector2.zero;

    public void Initialize()
    {
        InitMissilePool();
    }

    private void InitMissilePool()
    {
        _missilePool = new GameObject[_poolSize];

        for (var i = 0; i < _poolSize; ++i)
        {
            _missilePool[i] = Instantiate(Resources.Load("Prefabs/Missile")) as GameObject;
            _missilePool[i].name = "Missile_" + i;
            _missilePool[i].SetActive(false);
        }
    }

    private void Update()
    {
        ShootMissile();
    }

    private void ShootMissile()
    {
        _accTime += Time.deltaTime;

        // TODO :: 이 부분을 Data로 빼서 읽어올 수 있도록 해야함.
        if (_accTime > 3)
        {
            for (var i = 0; i < _poolSize; ++i)
            {
                var missile = _missilePool[i];

                if (missile.activeSelf == true)
                    continue;

                missile.SetActive(true);
                missile.GetComponent<Missile>().Spawn(MissileType.Basic, _currentPlayerPosition);
                _accTime = 0f;
                break;
            }
        }
    }

    public void DistributePlayerInfo(Vector3 playerPosition)
    {
        _currentPlayerPosition = new Vector2(playerPosition.x, playerPosition.y);

        for (var i = 0; i < _poolSize; ++i)
        {
            var missile = _missilePool[i];

            if (missile.activeSelf == false)
                continue;

            missile.GetComponent<Missile>().SetPlayerPosition(_currentPlayerPosition);
        }
    }
}
