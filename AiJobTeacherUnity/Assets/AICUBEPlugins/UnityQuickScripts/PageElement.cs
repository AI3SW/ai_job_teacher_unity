using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using DG.Tweening;

[RequireComponent(typeof(CanvasGroup))]
public class PageElement : MonoBehaviour
{
    public CanvasGroup canvaPage;

    [SerializeField]
    public UnityEvent TransitIn;
    [SerializeField]
    public UnityEvent TransitOut;
    [SerializeField]
    public string PageTitle;
    // Start is called before the first frame update
    void Awake()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        
    }
    public void deactivatePageInteraction()
    {
        canvaPage.blocksRaycasts = false;
        canvaPage.interactable = false;
    }
    public void activatePageInteraction()
    {
        canvaPage.blocksRaycasts = true;
        canvaPage.interactable = true;
    }
    public void deactivatePage()
    {
        canvaPage.alpha = 0;
        deactivatePageInteraction();
        canvaPage.gameObject.SetActive(false);
    }
    public void activatePage()
    {
        canvaPage.alpha = 1;
        activatePageInteraction();
        canvaPage.gameObject.SetActive(true);
    }

    public void fadePage(float endValue, float duration, TweenCallback action)
    {
        canvaPage.DOFade(endValue, duration).OnComplete(action);
    }
}
