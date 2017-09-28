using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region VARIABLES

    public PlayerSpec _spec { get; private set; }
    public Vector2 _flightVec { get; private set; }

    [SerializeField]
    Animator _animator;

    [SerializeField]
    SpriteRenderer _renderer;

    #endregion

    #region INITIALIZE METHODS

    public void Init(PlayerSpec spec)
    {
        _spec = spec;
        _flightVec = new Vector2(0f, -1f);

        SpriteInitialize();
    }

    void SpriteInitialize()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
        var spritePath = "Private/Type_" + _spec._type.ToString() + "/Animated/1";
        _renderer.sprite = Resources.Load(spritePath) as Sprite;

        _animator = this.GetComponent<Animator>();
        _animator.runtimeAnimatorController = Resources.Load("Private/Type_" + _spec._type.ToString() + "/Controller_" + _spec._type.ToString()) as RuntimeAnimatorController;
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
