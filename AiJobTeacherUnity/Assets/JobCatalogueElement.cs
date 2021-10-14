using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Astar.App.AITeacher;
public class JobCatalogueElement : MonoBehaviour, ScrollViewElement
{
    [SerializeField]
    RawImageWithRatio photo;
    [SerializeField] Image lockIcon;
    [SerializeField] TextMeshProUGUI nameUI;
    int jobId;
    string AIImageStr;
    string OriginalJobImageStr;

    public FullJobData jobData;

    [SerializeField] Sprite lockedSpr;
    [SerializeField] Sprite unlockedSpr;
    [SerializeField] Sprite unknownImage;

    [SerializeField] Color unknownColor;
    [SerializeField] Color unlockColor;
    [SerializeField] Color lockColor;
    public string GetText()
    {
        if (jobData.lockstate == lockType.Unknown) return "????";
        else return jobData.name;
    }

    public void SetJobInfo(FullJobData data)
    {
        jobData = data;
        jobId = jobData.id;
        //Debug.Log(jobData.joblock);
        refreshLock();
    }

    public void updateLock(lockType newlock)
    {
        jobData.lockstate = newlock;
        refreshLock();
    }
    public void refreshLock()
    {
        //call catalogue system for lock assets
        switch(jobData.lockstate)
        {
            case lockType.Unknown: 
                photo.setTextureAndRatio(unknownImage.texture, AspectRatioFitter.AspectMode.EnvelopeParent);
                photo.setColor(unknownColor);
                lockIcon.sprite = lockedSpr;
                lockIcon.color = Color.grey;
                nameUI.text = "????";
                break;
            case lockType.Locked: 
                photo.setTextureAndRatio(jobData.OriginalSpr, AspectRatioFitter.AspectMode.EnvelopeParent);
                photo.setColor(lockColor);
                lockIcon.sprite = lockedSpr;
                lockIcon.color = Color.grey;
                nameUI.text = jobData.name;
                break;
            case lockType.Unlocked: 
                photo.setTextureAndRatio(jobData.FutureSpr, AspectRatioFitter.AspectMode.EnvelopeParent);
                photo.setColor(unlockColor);
                lockIcon.sprite = unlockedSpr;
                lockIcon.color = Color.white;
                nameUI.text = jobData.name;
                break;
        }
    }
}
