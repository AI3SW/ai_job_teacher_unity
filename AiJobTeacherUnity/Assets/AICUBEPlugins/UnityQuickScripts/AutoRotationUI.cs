using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AutoRotationUI : MonoBehaviour
{
    public RectTransform rectTF;
    // Start is called before the first frame update
    Tween tween;
    private void Awake()
    {
        tween = rectTF.DOLocalRotate(new Vector3(0, 0, -180), 1f).SetEase(Ease.Linear);
        tween.SetLoops(-1);
    }
    void OnEnabled()
    {
        tween.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
