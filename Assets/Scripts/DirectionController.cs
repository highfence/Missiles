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

        _maxRadius = 30f;
    }

    private void Update()
    {
        DetectControll();        
    }

    public void DetectControll()
    {
        if (Input.GetMouseButton(0))
        {
            var screenMousePosition = Input.mousePosition;
            screenMousePosition.z = 0;

            var worldMousePosition = Camera.main.ScreenToWorldPoint(screenMousePosition);
            worldMousePosition.z = 0;

            // 마우스 위치와 중앙 위치 사이의 거리 계산.
            var distanceFromCenter = Vector2.Distance(new Vector2(screenMousePosition.x, screenMousePosition.y), _centerPosition);

            // 거리가 최대 반지름보다 작다면 
            if (distanceFromCenter <= _maxRadius)
            {
                // 마우스 위치에 컨트롤러를 놔준다.
                this.transform.position = worldMousePosition;
            }
            // 거리가 최대 반지름보다 크다면
            else
            {
                // 그 방향으로 반지름 크기만큼 진행한 위치를 집어넣어준다.
                var maxPosition = Camera.main.ScreenToWorldPoint(Vector3.MoveTowards(_centerPosition, screenMousePosition, _maxRadius));
                maxPosition.z = 0f;
                this.transform.position = maxPosition;
            }

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
