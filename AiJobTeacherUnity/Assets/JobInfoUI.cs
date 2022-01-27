using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AIcube.AITeacher;
using System.Text;
public class JobInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Jobname;
    [SerializeField] TextMeshProUGUI Description;
    [SerializeField] RectTransform DescriptionRect;
    [SerializeField] RawImageWithRatio OriginalJob;
    [SerializeField] RawImageWithRatio Future;
    [SerializeField] RawImageWithRatio User;
    [SerializeField] Button OriginalBtn;
    [SerializeField] Button FutureBtn;
    
    [SerializeField] Image lockImage;
    [SerializeField] Sprite lockedSpr;
    [SerializeField] Sprite unlockedSpr;

    [SerializeField] AIcube.AITeacher.lockType lockstatus;

    public static string mergeString(List<string> stringList)
    {
        StringBuilder sb = new StringBuilder();
        foreach (string str in stringList)
        {
            sb.Append(str + " ");
        }
        return sb.ToString();
    }

    public void updateInfo(FullJobData data)
    {
        Jobname.text = data.name;
        
        Description.text = mergeString(data.descriptions);
        OriginalJob.setTextureAndRatio(data.OriginalSpr, AspectRatioFitter.AspectMode.EnvelopeParent);
        Future.setTextureAndRatio(data.FutureSpr, AspectRatioFitter.AspectMode.EnvelopeParent);

        setLock((AIcube.AITeacher.lockType)data.lockstate);
        refreshInfo();
    }

    public void updatePhoto(Texture2D texture)
    {
        User.setTextureAndRatio(texture, AspectRatioFitter.AspectMode.WidthControlsHeight);
    }
    public void refreshInfo()
    {

        OriginalBtn.image.raycastTarget = false;
        switch (lockstatus)
        {
            case lockType.Unknown:
                FutureBtn.interactable = false;
                FutureBtn.image.raycastTarget = false;
                break;
            case lockType.Locked:
                FutureBtn.interactable = true;
                FutureBtn.image.raycastTarget = false;
                break;
            case lockType.Unlocked:
                FutureBtn.interactable = true;
                FutureBtn.image.raycastTarget = true;
                break;
        }
        OriginalJob.gameObject.SetActive(true);
        Future.gameObject.SetActive(false);
    }

    public void setLock(AIcube.AITeacher.lockType locknum)
    {
        lockstatus = locknum;
        switch (locknum)
        {
            case lockType.Unknown:
                //FutureBtn.interactable = false;
                lockImage.sprite = lockedSpr;
                break;
            case lockType.Locked:
                //FutureBtn.interactable = false;
                lockImage.sprite = lockedSpr;
                break;
            case lockType.Unlocked:
                //FutureBtn.interactable = true;
                lockImage.sprite = unlockedSpr;
                break;
        }
    }
}
