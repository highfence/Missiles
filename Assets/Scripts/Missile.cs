﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MissileType
{
    Basic = 0,
}

public class Missile : MonoBehaviour
{
    MissileSpec _spec;
    Vector2 _flightVector;
    Vector2 _playerPosition;
    SpriteRenderer _renderer;
    Sprite _basicMissile;
    float _accTime;

    public bool _isMissileValid { get; private set; }

    public void Awake()
    {
        _isMissileValid = false;
        _renderer = this.GetComponent<SpriteRenderer>();

        _basicMissile = Resources.Load<Sprite>("Sprites/BasicMissile");
    }

    public void Spawn(MissileType type)
    {
        // TODO :: 데이터를 빼다가 읽어줘야 됨.
        _spec = new MissileSpec()
        {
            _type = MissileType.Basic,
            _speed = 15f,
            _rotate = 1f,
            _liveTime = 10f
        };

        // TODO :: 스프라이트 렌더러에서 알맞은 미사일을 읽어와야 함.
        _renderer.sprite = _basicMissile;

        // TODO :: 미사일 위치를 랜덤하게 해주어야 함.
        _isMissileValid = true;
    }

    private void Update()
    {
        GoStraight();
        CheckDead();
    }

    public void SetPlayerPosition(Vector2 playerPosition)
    {
        _playerPosition = playerPosition;
    }

    private void CheckDead()
    {
        if (_accTime < _spec._liveTime)
        {
            _accTime += Time.deltaTime;
        }
        else if (_isMissileValid == true)
        {
            _isMissileValid = false;
        }
    }

    private void GoStraight()
    {
        var vectorToPlayer = new Vector2(_playerPosition.x - this.transform.position.x, _playerPosition.y - this.transform.position.y);
        vectorToPlayer.Normalize();

        // TODO :: 여기서 방향 전환각을 이용해 주어야 함.
        _flightVector = vectorToPlayer;

        var nextPosition = this.transform.position;
        var delta = Time.deltaTime * new Vector3(_flightVector.x, _flightVector.y, 0f) * _spec._speed;

        nextPosition = nextPosition + delta;
        nextPosition.z = 10;

        this.transform.position = nextPosition;
    }
}

public struct MissileSpec
{
    [SerializeField]
    public MissileType _type;

    [SerializeField]
    public float _speed;

    [SerializeField]
    public float _rotate;

    [SerializeField]
    public float _liveTime;

    public static MissileSpec CreateFromText(string text)
    {
        MissileSpec instance;

        try
        {
            instance = JsonUtility.FromJson<MissileSpec>(text);
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("[PlayerSpec] Cannot parse PlayerSpec from source - {0}, {1}", text, e.Message);
            throw;
        }

        return instance;
    }
}
