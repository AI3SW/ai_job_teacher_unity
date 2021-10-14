using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;


public class fadeAnimation : MonoBehaviour
{
    [SerializeField] float initialDelay = 1f;

    [SerializeField]
    RawImage Main;
    [SerializeField]
    RawImage Sub;
    Sequence transitition;
    private void Awake()
    {

        transitition = DOTween.Sequence();
        float t = 0;
        transitition.InsertCallback(t, () => {
            Sub.DOFade(1, 2f).SetEase(Ease.Linear);
            Main.DOFade(0, 2f).SetEase(Ease.Linear);
        });
        transitition.InsertCallback(t += 3, () => {
            Sub.DOFade(0, 2f).SetEase(Ease.Linear);
            Main.DOFade(1, 2f).SetEase(Ease.Linear);
        });
        transitition.SetDelay(initialDelay);
        transitition.SetAutoKill(false);
        transitition.OnComplete(() =>
        {
            transitition.Rewind();
        });
    }

    public void playTransition()
    {
        if (!transitition.IsPlaying())
            transitition.Play();
    }
}
