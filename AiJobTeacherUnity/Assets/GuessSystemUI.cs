using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class GuessSystemUI : MonoBehaviour
{
/*    [SerializeField]
    TextMeshProUGUI hint;*/
    [SerializeField]
    TextMeshProUGUI description;
    [SerializeField]
    TextMeshProUGUI jobName;
    [SerializeField]
    RawImageWithRatio gameImg;
    [SerializeField]
    RawImageWithRatio LoseImg;

    [SerializeField]
    RawImageWithRatio WinImgFuture;
    [SerializeField]
    RawImageWithRatio WinCurrent;

    Texture2D tempText = null;

    public void updateUIData(int jobid)
    {
        AIcube.AITeacher.FullJobData temp = AppManager.Singleton.getJobData(jobid);
        jobName.text = temp.name;
        description.text = temp.descriptions[Random.Range(0, temp.descriptions.Count)];
        LoseImg.setTextureAndRatio(temp.OriginalSpr, AspectRatioFitter.AspectMode.EnvelopeParent);
        gameImg.setTextureAndRatio(temp.FutureSpr, AspectRatioFitter.AspectMode.EnvelopeParent);
        WinImgFuture.setTextureAndRatio(temp.FutureSpr, AspectRatioFitter.AspectMode.EnvelopeParent);
        WinCurrent.setTextureAndRatio(AppManager.Singleton.getMyPhoto(), AspectRatioFitter.AspectMode.WidthControlsHeight);
        Debug.Log("updateData");
    }

    public Texture2D updateAIImage(string strimg)
    {
        //Debug.Log("test1");
        tempText = WebcamController.createTexture2DFromStr(strimg);
        //Debug.Log("test2");
        gameImg.setTextureAndRatio(tempText, AspectRatioFitter.AspectMode.EnvelopeParent);
        //Debug.Log("test3");
        WinImgFuture.setTextureAndRatio(tempText, AspectRatioFitter.AspectMode.EnvelopeParent);
        //Debug.Log("test4");
        //Debug.Log("updateAIImage");
        return tempText;
    }

}
