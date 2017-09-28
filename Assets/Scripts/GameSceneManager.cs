using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    #region VARIABLES

    Player              _player;
    Vector2             _playerInitialPosition;
    Background          _background;
    DirectionController _controller;
    Camera              _gameCamera;

    #endregion

    #region INITIALIZE METHODS

    void Start()
    {
        VariableInitialize();
        ControllerInitialize();
        PlayerInitialize();
        CameraInitialize();
        BackgroundInitialize();
    }

    private void CameraInitialize()
    {
        _gameCamera = Instantiate(Resources.Load("Prefabs/Game Camera") as GameObject).GetComponent<Camera>();
        _gameCamera.enabled = true;
    }

    private void BackgroundInitialize()
    {
        var prefab = Resources.Load("Prefabs/Background") as GameObject;
        _background = Instantiate(prefab).GetComponent<Background>();

        var bgPosition = transform.position;
        bgPosition.z = 10;
        _background.transform.position = bgPosition;
    }

    void ControllerInitialize()
    {
        _controller = DirectionController.Factory.Create();
        var controllerPosition = new Vector2();
        controllerPosition.x = Screen.width / 2;
        controllerPosition.y = Screen.height * 0.15f;

        _controller.SetInitialPosition(controllerPosition);

        var uiSystem = FindObjectOfType<UISystem>();
        uiSystem.AttachUI(_controller.gameObject);
    }

    void PlayerInitialize()
    {
        // TODO :: 추후에 입력 설정을 받아서 PlayerType 결정.
        // 그러면 Player Initialize 함수는 GameSceneManager의 Start 메소드가 아니라 Start 버튼을 누르면 호출.
        _player = Player.Factory.Create(PlayerType.Basic);
        _player.transform.position = _playerInitialPosition;
        _player._mainController = this._controller;
    }

    void VariableInitialize()
    {
        var fixedPosition = new Vector2();
        fixedPosition.x = Screen.width / 2;
        fixedPosition.y = Screen.height / 2;
        _playerInitialPosition = Camera.main.ScreenToWorldPoint(fixedPosition);
    }

    #endregion

    void Update()
    {
        PositionSync();
    }

    void PositionSync()
    {
        // 플레이어 포지션과 카메라 싱크.
        var curPlayerPosition = _player.transform.position;
        curPlayerPosition.z = -20;
        _gameCamera.transform.position = curPlayerPosition;

        // 플레이어 포지션과 백그라운드 싱크.
        curPlayerPosition.z = 20;
        _background.transform.position = curPlayerPosition;
    }
}
