using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    private void Start()
    {
        _renderer = this.GetComponent<SpriteRenderer>();
    }
}
