using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * 게임 시작전에 초기화해야 할 친구들을 초기화해주는 클래스.
 * TODO :: 초기화 하는 동안 로딩 씬을 재생해준다.
 */
public class LaunchManager : MonoBehaviour
{
    SpriteRenderer _loadingRenderer;

    private void Start()
    {
        LoadingSceneSettings();

        SetScreenOptions();

        Debug.Log("Jobs Done!");
    }

    // 화면 관련 옵션들을 정리해주는 메소드.
    private void SetScreenOptions()
    {
        Screen.SetResolution(540, 960, false);
    }

    // 로딩 씬 관련 메소드.
    private void LoadingSceneSettings()
    {
        _loadingRenderer = this.GetComponent<SpriteRenderer>();

        // TODO :: 렌더러 transform 이용하여 스케일을 잘 맞도록 해주어야 함.

        StartCoroutine("PlayLoadingScene");
    }

    // 로딩 씬 재생 메소드.
    IEnumerator PlayLoadingScene()
    {
        var curColor = _loadingRenderer.color;
        curColor.a = 0.0f;
        _loadingRenderer.color = curColor;

        while (_loadingRenderer.color.a < 1.0f)
        {
            curColor.a += 0.05f;
            _loadingRenderer.color = curColor;

            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene("1. Game");
    }
}
