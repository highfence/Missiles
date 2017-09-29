using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    Sprite[] _clouds = null;
    SpriteRenderer _renderer;
    public bool _isCloudEnd = false;
    float _accTime = 0f;

    public void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _clouds = new Sprite[4];

        for (var i = 0; i < 4; ++i)
        {
            _clouds[i] = Resources.Load<Sprite>("Private/Cloud_" + i);
        }
    }

    public void Spawn(int randomNumbers)
    {
        _isCloudEnd = false;
        _renderer.sprite = _clouds[randomNumbers];
        var color = _renderer.color;
        color.a = 0;

        _renderer.color = color;

        StartCoroutine("CloudRise", color);
    }

    public void Clear()
    {
        _accTime = 0f;
        _isCloudEnd = false;
    }

    IEnumerator CloudRise(Color color)
    {
        var curColor = color;

        while (true)
        {
            if (curColor.a >= 1) break;

            curColor.a += 0.1f;

            _renderer.color = curColor;

            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator CloudDown(Color color)
    {
        var curColor = color;

        while (true)
        {
            if (curColor.a <= 0) break;

            curColor.a -= 0.1f;

            _renderer.color = curColor;

            yield return new WaitForSeconds(0.05f);
        }

        _accTime = 0;
        _isCloudEnd = true;
    }

    private void Update()
    {
        _accTime += Time.deltaTime; 

        if (_accTime > 10)
        {
            StartCoroutine("CloudDown", _renderer.color);
        }
    }
}
