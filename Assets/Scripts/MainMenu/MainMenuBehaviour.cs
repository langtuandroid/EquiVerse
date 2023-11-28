using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Color = System.Drawing.Color;

public class MainMenuBehaviour : MonoBehaviour
{
    public Image transitionOverlay;
    
    public void ClickPlay()
    {
        transitionOverlay.DOFade(1f, 2.0f).SetEase(Ease.InCubic).OnComplete((() =>
        {
            SceneManager.LoadSceneAsync(1);
        }));
    }

    public void ClickOptions()
    {
        
    }

    public void ClickQuit()
    {
        Application.Quit();
    }
}
