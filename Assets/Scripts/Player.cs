using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region VARIABLES

    public PlayerSpec          _spec           { get; private set; }
    public Vector2             _flightVector   { get; private set; }
    public Vector2             _aimVector      { get; private set; }
    public DirectionController _mainController { get;         set; }

    [SerializeField]
    Animator _animator;

    [SerializeField]
    SpriteRenderer _renderer;

    #endregion

    #region UPDATE METHODS

    private void FixedUpdate()
    {
        HandleLogicDirection();
        GoStraight();
    }

    private void GoStraight()
    {
        var nextPosition = this.transform.position;
        var delta = Time.deltaTime * new Vector3(_flightVector.x, _flightVector.y, 0) * _spec._speed;

        this.transform.position = nextPosition + delta;
    }

    private void Update()
    {
        HandleVisualDirection();
    }

    // 스프라이트의 방향이 방향 벡터와 알맞게 되도록 맞춰주는 메소드.
    void HandleVisualDirection()
    {
        float angle        = Mathf.Atan2(_flightVector.x, _flightVector.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }

    // DirectionController의 방향과 flightVector를 맞춰가는 메소드.
    void HandleLogicDirection()
    {
        if (_mainController._controllVector != Vector2.zero)
        {
            _aimVector = _mainController._controllVector;
        }

        // TODO :: 방향이 차차 변하도록
        _flightVector = _aimVector;
    }

    #endregion

    #region INITIALIZE METHODS

    public void Init(PlayerSpec spec)
    {
        _spec = spec;
        _flightVector = new Vector2(0f, -1f);

        SpriteInitialize();
    }

    void SpriteInitialize()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
        var spritePath = "Private/Type_" + _spec._type.ToString() + "/Animated/1";
        _renderer.sprite = Resources.Load(spritePath) as Sprite;

        _animator = this.GetComponent<Animator>();
        var animatorPath = "Private/Type_" + _spec._type.ToString() + "/Controller_" + _spec._type.ToString();
        _animator.runtimeAnimatorController = Resources.Load(animatorPath) as RuntimeAnimatorController;
    }

    #endregion

    #region FACTORY METHOD

    public static class Factory
    {
        public static Player Create(PlayerType type)
        {
            // Get Player Spec Struct
            var playerSpecText = Resources.Load<TextAsset>("Data/" + type.ToString()).text;
            var playerSpec = PlayerSpec.CreateFromText(playerSpecText);

            // Make Player with Spec
            var prefab = Resources.Load("Prefabs/Player") as GameObject;
            var instance = Instantiate(prefab).GetComponent<Player>();

            if (instance == null)
            {
                Debug.LogError("Player Instantiate Failed");
                return null;
            }

            instance.Init(playerSpec);

            return instance;
        }
    }

    #endregion

}
