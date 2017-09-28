using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameSceneState
{
    Home = 0,
    InGame = 1,
    Result = 2
}

public class GameSceneManager : MonoBehaviour
{
    #region VARIABLES

    Player              _player;
    Vector2             _playerInitialPosition;
    Background          _background;
    DirectionController _controller;
    GameObject          _playButton;
    GameObject          _title;
    Camera              _gameCamera;
    MissileShooter      _missileShooter;
    public GameSceneState      _state;
    #endregion

    #region INITIALIZE METHODS

    void Start()
    {
        _state = GameSceneState.Home;

        VariableInitialize();
        UIInitialize();
        PlayerInitialize();
        CameraInitialize();
        BackgroundInitialize();
        MissileShooterInitialize();

        StopInGameObjects();
        StopResultObjects();
    }


    private void MissileShooterInitialize()
    {
        _missileShooter = Instantiate(Resources.Load("Prefabs/MissileShooter") as GameObject).GetComponent<MissileShooter>();
        _missileShooter.Initialize();
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

    void UIInitialize()
    {
        _controller = DirectionController.Factory.Create();
        var controllerPosition = new Vector2();
        controllerPosition.x = Screen.width / 2;
        controllerPosition.y = Screen.height * 0.15f;

        _controller.SetInitialPosition(controllerPosition);

        var uiSystem = FindObjectOfType<UISystem>();
        uiSystem.AttachUI(_controller.gameObject);

        _playButton = Instantiate(Resources.Load("Prefabs/PlayButton")) as GameObject;
        var buttonPosition = Camera.main.ScreenToWorldPoint(controllerPosition + new Vector2(0, 100));
        buttonPosition.z = 0;
        _playButton.transform.position = buttonPosition;
        _playButton.GetComponent<Button>().onClick.AddListener(OnPlayButtonClicked);

        uiSystem.AttachUI(_playButton);

        _title = Instantiate(Resources.Load("Prefabs/Title")) as GameObject;
        var titlePosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height * 0.8f, 0));
        titlePosition.z = 0;
        _title.transform.position = titlePosition;
        uiSystem.AttachUI(_title);
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
        fixedPosition.y = Screen.height * 3 / 5;
        _playerInitialPosition = Camera.main.ScreenToWorldPoint(fixedPosition);
    }

    #endregion

    void Update()
    {
        PositionSync();

        HomeUpdate();
        InGameUpdate();
        ResultUpdate();
    }

    private void ResultUpdate()
    {
        if (_state != GameSceneState.Result) return;
    }

    private void HomeUpdate()
    {
        if (_state != GameSceneState.Home) return;

    }

    void InGameUpdate()
    {
        if (_state != GameSceneState.InGame) return;

        if (_player._isPlayerDead == true)
        {
            _state = GameSceneState.Result;
        }

        _missileShooter.DistributePlayerInfo(_player.transform.position);
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

    void StopInGameObjects()
    {
        _controller.gameObject.SetActive(false);
        _missileShooter.gameObject.SetActive(false);
    }

    void StopResultObjects()
    {
    }

    void StopHomeObjects()
    {
        _title.gameObject.SetActive(false);
        _playButton.gameObject.SetActive(false);
    }

    public void OnPlayButtonClicked()
    {
        StartCoroutine("GoToInGameState");
    }

    IEnumerator GoToInGameState()
    {
        if (_state == GameSceneState.Home)
        {
            var titleColor = _title.GetComponent<Image>().color;
            var buttonColor = _playButton.GetComponent<Image>().color;

            while (true)
            {
                titleColor.a -= 0.1f;
                buttonColor.a -= 0.1f;

                _title.GetComponent<Image>().color = titleColor;
                _playButton.GetComponent<Image>().color = buttonColor;

                if (titleColor.a <= 0.0f) break;

                yield return new WaitForSeconds(0.05f);
            }

            var originTitleColor = _title.GetComponent<Image>().color;
            var originButtonColor = _playButton.GetComponent<Image>().color;

            originTitleColor.a = 1;
            originButtonColor.a = 1;

            _title.GetComponent<Image>().color = originTitleColor;
            _playButton.GetComponent<Image>().color = originButtonColor;

        }

        StopHomeObjects();
        StartInGameObjects();

        _state = GameSceneState.InGame;
    }

    private void StartInGameObjects()
    {
        _controller.gameObject.SetActive(true);
        _missileShooter.gameObject.SetActive(true);
    }
}
