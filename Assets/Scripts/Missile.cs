using System;
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
    float _outRange;

    public bool _isMissileValid { get; private set; }
    public bool _isMissileActive { get; private set; }

    public void Awake()
    {
        _isMissileValid = false;
        _isMissileActive = false;
        _renderer = this.GetComponent<SpriteRenderer>();
        _outRange = 20f;

        _basicMissile = Resources.Load<Sprite>("Sprites/BasicMissile");
    }

    public void Clear()
    {
        var color = _renderer.color;
        color.a = 1;
        _renderer.color = color;

        _accTime = 0f;
        _isMissileValid = false;
        _isMissileActive = false;
    }

    public void Spawn(MissileType type, Vector2 currentPlayerPosision)
    {
        // TODO :: 데이터를 빼다가 읽어줘야 됨.
        _spec = new MissileSpec()
        {
            _type = MissileType.Basic,
            _speed = 7f,
            _rotate = 1f,
            _liveTime = 10f
        };

        // TODO :: 스프라이트 렌더러에서 알맞은 미사일을 읽어와야 함.
        _renderer.sprite = _basicMissile;

        var randomUnitVec = new Vector2(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1));
        randomUnitVec.Normalize();
        var spawnPosition = _playerPosition + randomUnitVec * _outRange;

        this.transform.position = spawnPosition;

        _isMissileValid = true;
        _isMissileActive = true;
    }

    private void FixedUpdate()
    {
        GoStraight();
    }

    private void Update()
    {
        CheckDead();
        HandleVisualDirection();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Missile")
        {
            var explosion = Instantiate(Resources.Load("Private/ToonExplosion v1.0/Prefabs/Explosion") as GameObject);
            explosion.transform.localScale = new Vector3(0.25f, 0.25f, 1f);
            explosion.transform.position = this.transform.position;

            _isMissileValid = false;
            _isMissileActive = false;
        }
    }

    private void HandleVisualDirection()
    {
        float angle = Mathf.Atan2(_flightVector.x, _flightVector.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
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
            StartCoroutine("StartDisappear"); 
            _isMissileValid = false;
        }
    }

    IEnumerator StartDisappear()
    {
        var curColor = _renderer.color;

        while (_renderer.color.a > 0.0f)
        {
            curColor.a -= 0.1f;
            _renderer.color = curColor;

            yield return new WaitForSeconds(0.05f);
        }

        _isMissileActive = false;
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
