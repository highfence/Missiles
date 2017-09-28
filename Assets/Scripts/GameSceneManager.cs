using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    #region VARIABLES

    Player _player;
    Vector2 _playerPosition;

    #endregion

    #region INITIALIZE METHODS

    void Start()
    {
        VariableInitialize();
        PlayerInitialize();
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
        _playerPosition = new Vector2(0.5f, 0.3f);
    }

    #endregion

    void Update()
    {

    }
}
