using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 비행기 방향을 조작하는 친구.
 */
public class DirectionController : MonoBehaviour
{
    public Vector2        _controllVector { get; private set; }

    [SerializeField]
    public Vector2        _centerPosition { get; private set; }

    public float          _maxRadius      { get; private set; }
    public SpriteRenderer _controllSprite { get; private set; }

    public void Initialize()
    {
        // 컨트롤 스프라이트 렌더러 받아오기.
        _controllSprite = this.GetComponent<SpriteRenderer>();

        _maxRadius = 100f;
    }

    private void Update()
    {
        DetectControll();        
    }

    public void DetectControll()
    {
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            this.transform.position = mousePosition;
        }
        else
        {
            var worldCenter = Camera.main.ScreenToWorldPoint(_centerPosition);
            worldCenter.z = 0f;
            this.transform.position = worldCenter;
        }
    }

    public void SetInitialPosition(Vector2 screenPosition)
    {
        _centerPosition = new Vector2(screenPosition.x, screenPosition.y);

        var worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;

        this.transform.position = worldPosition;
    }

    #region FACTORY METHOD

    public static class Factory
    {
        public static DirectionController Create()
        {
            DirectionController instance;

            try
            {
                instance = Instantiate(Resources.Load("Prefabs/DirectionController") as GameObject).GetComponent<DirectionController>();
                instance.Initialize();
            }
            catch (Exception e)
            {
                Debug.LogErrorFormat("DirectionController Create Failed {0}", e.Message);
                throw;
            }

            return instance;
        }
    }

    #endregion
}
