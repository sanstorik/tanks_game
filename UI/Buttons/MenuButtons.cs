using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour {

    public GameObject levelLoadPanel;
    public GameObject notImplementedPanel;

    public void StartSingleGame()
    {
        ScreenOpacity.INSTANCE.BlockScreen();
        ScreenOpacity.INSTANCE.MakeSmoothDarker(0.3f, 2f);
        levelLoadPanel.SetActive(true);
        StartCoroutine(_LoadLevel());
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ButtonNotImplemented_OnClick()
    {
        ScreenOpacity.INSTANCE.BlockScreen();
        ScreenOpacity.INSTANCE.MakeSmoothDarker(0.3f, 2f);

        notImplementedPanel.SetActive(true);
    }

    public void ExitNotImplementedButton_OnClick()
    {
        ScreenOpacity.INSTANCE.UnblockScreen();
        notImplementedPanel.SetActive(false);
    }

    IEnumerator _LoadLevel()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(1);
        yield return async;
    }
}
