using System;
using UnityEngine;

public enum PlayerType
{
    Basic = 0,
}

public struct PlayerSpec
{
    [SerializeField]
    public PlayerType _type;

    [SerializeField]
    public float _speed;

    [SerializeField]
    public float _rotate;

    public static PlayerSpec CreateFromText(string text)
    {
        PlayerSpec instance;

        try
        {
            instance = JsonUtility.FromJson<PlayerSpec>(text);
        }
        catch (Exception e)
        {
            Debug.LogErrorFormat("[PlayerSpec] Cannot parse PlayerSpec from source - {0}, {1}", text, e.Message);
            throw;
        }

        return instance;
    }
}