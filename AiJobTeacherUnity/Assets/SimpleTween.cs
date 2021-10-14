using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
public class SimpleTween : MonoBehaviour
{
    [SerializeField]
    float xDisplacement;
    [SerializeField]
    float yDisplacement;
    [SerializeField]
    float totalTweenduration = 1f;
    [SerializeField]
    bool loop = true;

    RectTransform _rectT;

    // Start is called before the first frame update
    void Start()
    {
        
        _rectT = GetComponent<RectTransform>();
        Vector3 startPosition = _rectT.localPosition;
        Vector3 tweenPosition = _rectT.localPosition;
        tweenPosition.x += xDisplacement;
        tweenPosition.y += yDisplacement;
        if (loop) totalTweenduration /= 2f;

        Sequence mySequence = DOTween.Sequence();
        mySequence.Append(_rectT.DOLocalMove(tweenPosition, totalTweenduration).SetEase(Ease.Linear));
        mySequence.Append(_rectT.DOLocalMove(startPosition, totalTweenduration).SetEase(Ease.Linear));
        mySequence.SetLoops(-1);
        mySequence.Play();
    }

}
