using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    #region VARIABLES

    Player _player;
    Vector2 _playerPosition;
    DirectionController _controller;

    #endregion

    #region INITIALIZE METHODS

    void Start()
    {
        VariableInitialize();
        ControllerInitialize();
        PlayerInitialize();
    }

    void ControllerInitialize()
    {
        _controller = DirectionController.Factory.Create();
        var controllerPosition = new Vector2();
        controllerPosition.x = Screen.width / 2;
        controllerPosition.y = Screen.height * 0.15f;
        var worldPosition = Camera.main.ScreenToWorldPoint(controllerPosition);
        worldPosition.z = 0f;

        _controller.SetInitialPosition(controllerPosition);
    }

    void PlayerInitialize()
    {
        // TODO :: 추후에 입력 설정을 받아서 PlayerType 결정.
        // 그러면 Player Initialize 함수는 GameSceneManager의 Start 메소드가 아니라 Start 버튼을 누르면 호출.
        _player = Player.Factory.Create(PlayerType.Basic);
        _player.transform.position = _playerPosition;
    }

    void VariableInitialize()
    {
        var fixedPosition = new Vector2();
        fixedPosition.x = Screen.width / 2;
        fixedPosition.y = Screen.height / 2;
        _playerPosition = Camera.main.ScreenToWorldPoint(fixedPosition);
    }

    #endregion

    void Update()
    {

    }
}
