using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
public class ErrorPage : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textbox;
    [SerializeField]
    CanvasGroup canva;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Activate(string msg = "")
    {
        gameObject.SetActive(true);
        canva.DOFade(1, 0.5f);
        
        textbox.text = msg;
        textbox.text += "\nClick anywhere to continue";
    }
    public void DeActivate()
    {
        
        canva.DOFade(0, 0.5f).OnComplete(() => { gameObject.SetActive(false); });
    }
}
