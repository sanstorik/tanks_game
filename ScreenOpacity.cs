using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using DG.Tweening;

public class ScreenOpacity : MonoBehaviour {
    static private ScreenOpacity instance;

    [SerializeField]
    Image screenOpacity;

    float duration = 1f;

    private void Awake()
    {
        instance = this;
    }

    public void MakeSmoothDarker(float opacity)
    {
        MakeSmoothDarker(opacity, duration);
    }

    public void MakeSmoothDarker(float opacity, float duration)
    {
        screenOpacity.DOFade(opacity, duration);
    }

    public void MakeSmoothNormal()
    {
        MakeSmoothNormal(duration);
    }

    public void MakeSmoothNormal(float duration)
    {
        screenOpacity.DOFade(0, duration);
    }

    public void BlockScreen()
    {
        screenOpacity.gameObject.SetActive(true);
    }

    public void UnblockScreen()
    {
        screenOpacity.gameObject.SetActive(false);
    }

    static public ScreenOpacity INSTANCE
    {
        get { return instance; }
    }


}
