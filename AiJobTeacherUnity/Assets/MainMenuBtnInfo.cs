using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MainMenuBtnInfo : MonoBehaviour , ScrollViewElement
{
    [SerializeField]
    Sprite IconImage;
    [SerializeField]
    Image btnImage;
    [SerializeField]
    public TextMeshProUGUI UIBtnText;
    [SerializeField]
    public string BtnText;

    
    // Start is called before the first frame update
    void Awake()
    {
        btnImage.sprite = IconImage;
        UIBtnText.text = BtnText;
        //UIIconText.text = IconText;
    }

    public string GetText()
    {
        return BtnText;
    }
}
