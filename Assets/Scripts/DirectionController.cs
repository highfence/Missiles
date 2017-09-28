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
    public Vector2        _centorPosition { get; private set; }
    public SpriteRenderer _controllSprite { get; private set; }

    public void Initialize()
    {
        // 컨트롤 스프라이트 렌더러 받아오기.
        _controllSprite = this.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        DetectControll();        
    }

    public void DetectControll()
    {
        if (Input.GetMouseButton(0))
        {
            
        }
    }

    public void SetInitialPosition(Vector3 worldPosition)
    {
        _centorPosition = new Vector2(worldPosition.x, worldPosition.y);

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
