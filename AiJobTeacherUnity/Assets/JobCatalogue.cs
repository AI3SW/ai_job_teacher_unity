using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AIcube.AITeacher;
public class JobCatalogue : MonoBehaviour
{
    [SerializeField]
    RectTransform parent;
    [SerializeField] JobCatalogueElement prefab;
    //[SerializeField] public List<FullJobData> JobDataList;
    [SerializeField] List<JobCatalogueElement> catalogueList;

    [SerializeField] JobInfoUI infoUI;
    [SerializeField]
    public int currentSelectedJobId { get; private set; }

    public void InitJobCatalogueAndInfo(List<FullJobData> list)
    {

        foreach (var obj in list)
        {
            JobCatalogueElement temp = CreateElement();
            temp.SetJobInfo(obj);
        }
    }

    public FullJobData getJobData(int index)
    {
        return catalogueList[index].jobData;
    }

    JobCatalogueElement CreateElement()
    {
        JobCatalogueElement temp = Instantiate<JobCatalogueElement>(prefab, parent);
        catalogueList.Add(temp);
        return temp;
        //temp.SetJobInfo();
    }
    
    public void updateCurrentSelectedJob(int val)
    {
        currentSelectedJobId = val;
        //Debug.Log(currentSelectedJobId);
    }

    public void RefreshJobInfo()
    {
        //Debug.Log(currentSelectedJobId);
        //Debug.Log(catalogueList[currentSelectedJobId].jobData.jobname);
        //Debug.Log(catalogueList[currentSelectedJobId].jobData.FutureSpr);
        infoUI.updateInfo(catalogueList[currentSelectedJobId].jobData);

        infoUI.updatePhoto(AppManager.Singleton.getMyPhoto());
    }

    public void setAndRefresh()
    {
        currentSelectedJobId = AppManager.Singleton.guessSys.answerId;
        //Debug.Log(currentSelectedJobId);

        RefreshJobInfo();
    }
    public void RefreshAlljobElement()
    {
        foreach(var obj in catalogueList)
        {
            obj.refreshLock();
        }
    }

    public int getIdFromJobName(string jobname)
    {
        int count = 0;
        foreach (var obj in catalogueList)
        {
            if (string.Compare(obj.jobData.name, jobname) == 0) return count;
            ++count;
        }
        return -1;
    }

    public void handleButtonOnJobSelected(int jobindex,Button btn)
    {
        if(catalogueList[jobindex].jobData.lockstate == lockType.Unknown)
        {
            btn.interactable = false;
        } else
        {
            btn.interactable = true;
        }
    }

    public void progressLockState(lockType lockstate, int jobId)
    {
        //Debug.Log(jobId);
        //Debug.Log(catalogueList.Count);
        //Debug.Log(catalogueList[jobId].jobData.lockstate);
        //Debug.Log(lockstate);
        if (catalogueList[jobId].jobData.lockstate < lockstate)
        {
            //Debug.Log("update lock");
            catalogueList[jobId].updateLock(lockstate);
            AppManager.Singleton.updateSaveData(jobId,(int) catalogueList[jobId].jobData.lockstate);
        }
        //extrapolate data push to save
    }

    public void updateAiImage(int jobid,Texture2D texture)
    {
        catalogueList[jobid].jobData.FutureSpr = texture;
    }
}
