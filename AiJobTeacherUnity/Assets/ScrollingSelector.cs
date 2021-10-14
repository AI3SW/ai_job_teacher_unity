using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
public class ScrollingSelector : MonoBehaviour
{
    [SerializeField]
    Scrollbar scrollbar;
    [SerializeField]
    public TextMeshProUGUI ModeInfo;
    [SerializeField]
    public Button btn;
    List<ScrollViewElement> btnList;

    [SerializeField] UnityEventInt onModeChange;
    [SerializeField] UnityEvent<int,Button> onModeChangeWithButton;
    private void Start()
    {
        btnList = new List<ScrollViewElement >(GetComponentsInChildren<ScrollViewElement>());
    }
    private void Update()
    {
        for (float i = 0; i < btnList.Count; ++i)
        {
            //Debug.Log(i);

            if (scrollbar.value < (i+1) / (float)btnList.Count)
            {
                onModeChange?.Invoke((int)i);
                onModeChangeWithButton?.Invoke((int)i, btn);
                //ModeInfo.text = btnList[(int)i].GetText();
                return;
            }
        }

    }
    public void updateText(int i)
    {
        ModeInfo.text = btnList[i].GetText();
    }
}
