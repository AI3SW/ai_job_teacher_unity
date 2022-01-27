using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using AIcube.AITeacher;
using System.Threading.Tasks;
using AIcube.REST;

public class AppManager : MonoBehaviour
{
    #region Singleton
    public static AppManager Singleton;
    void InitSingleton()
    {
        if (Singleton == null)
        {
            Singleton = this;
            DontDestroyOnLoad(Singleton);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion
    private RESTinterface RESTServer;
    //private Options _serverOptions;
    //[SerializeField]
    //public string ip = "10.2.1.153";
    //[SerializeField]
    //public string port = "5000";

    public string getServerAddress()
    {
        return "http://" + RESTServer.serverInfo._ip + ":" + RESTServer.serverInfo._port;
    }

    private void Awake()
    {
        
        InitSingleton();
        //ip = deepface.AIModels.
        //_serverOptions = new Options(ip, port, false, false);
        //RESTServer = new RESTinterface(_serverOptions);

        webcam = GetComponent<WebcamController>();
        webcam.PrepareCamera();


    }

    private void Start()
    {
        InitData();
    }

    

    [SerializeField]
    public UnityDecoupledBehavior.PageController pageControl;

    [SerializeField] 
    public Deepface_Controller deepface;

    [SerializeField]
    public JobCatalogue  job_Catalogue;

    [SerializeField]
    public GuessSystem guessSys;

    [SerializeField]
    public WebcamController webcam;

    [SerializeField]
    public Sprite defaultPotraitForMissingImage;

    [SerializeField]
    public PlayerJsonSaveData SaveData;

    [SerializeField]
    public GameObject loadingScreen;
    [SerializeField]
    public GameObject ErrorScreen;
    async void InitData()
    {
        loadingScreen.gameObject.SetActive(true);
        LoadSavedFile();

        
        RESTServer = new RESTinterface(deepface.getServerOptions());
        Debug.Log(RESTServer.serverInfo.finalUrl);
        /*
        Ping ping = new Ping(RESTServer.serverInfo._ip);
        float timeout = 2000;
        float timer = Time.realtimeSinceStartup;

        await new WaitUntil(() => { return ping.isDone; });
        timer = Time.realtimeSinceStartup - timer;
        Debug.Log(timer);

        bool connectionResult = false;

        if (ping.time < 0 || ping.time >= timeout)
        {
            //do failure stuff
        }
        else
        {
        }
        */
        bool localdataOnly = false;
        string versionString = await getDataVersion();
        List<FullJobData> JobDataList = new List<FullJobData>();
        if (!string.IsNullOrEmpty( versionString) && string.Compare(versionString,SaveData.version) != 0)
        {
            Debug.Log("getting online data");

            JobDataList = await getDataOnlineAndMerge();
            SaveData.version = versionString;


            Debug.Log("merging online and local data");

        } else if(string.IsNullOrEmpty(versionString))
        {
            JobDataList = getDataFromSave();
            localdataOnly = true;

            Debug.Log("only loading local data");
        } else
        {
            JobDataList = getDataFromSave();


            //Debug.Log(JobDataList.Count);

            Debug.Log("No updates with working connections");
        }

        Debug.Log("Populating Game System");
        //Debug.Log(JobDataList.Count);
        guessSys.ClearAndPopulateGameList(JobDataList , localdataOnly);
        job_Catalogue.InitJobCatalogueAndInfo(JobDataList);
        loadingScreen.gameObject.SetActive(false);
        if (JobDataList.Count > 0) Debug.Log("data retrieved");
        else
        {
            Debug.Log("dataNotFound");
            ErrorScreen.gameObject.SetActive(true);

        }
    }

    /// <summary>
    /// returns a null string if connection failes
    /// </summary>
    /// <returns></returns>
    public async Task<string> getDataVersion()
    {
        //if(RESTServer == null) 
        var result = await RESTServer.getJsonData<AIcube.AITeacher.Version>("version");
        string versionNumber = (result.isConnected)? result.jsonData.version : "";
        //Debug.Log(versionNumber);
        return versionNumber;
    }

    public void updateImage(AIcube.REST.DeepFaceTech.ImageOutput data)
    {
       // Debug.Log("has come in");
        //Debug.Log(data.output_img);
        if (!string.IsNullOrEmpty(data.output_img))
        {
            //Debug.Log("has image1");
            SaveData.JobProfile[guessSys.answerId].ImageStrAI = data.output_img;
            saveUserData();
            setJobTolocked();
            //Debug.Log("has image2");
            Texture2D temptext = guessSys.guessUI.updateAIImage(data.output_img);
            job_Catalogue.updateAiImage(guessSys.answerId, temptext);
            Debug.Log("has updateImage");
        }
        else
        {
            Debug.Log("Error");
        }
    }

    public async Task<bool> getOnlineJobAIPhoto()
    {
        //Debug.Log(guessSys.answerId);
        bool isConnected = false;
        //Debug.Log("Data : "+SaveData.JobProfile[guessSys.answerId].ImageStrAI);
        if (string.IsNullOrEmpty( SaveData.JobProfile[guessSys.answerId].ImageStrAI))
        {
            isConnected = await deepface.SendDeepFace(SaveData.imageStr, SaveData.user_id, guessSys.answerId + 1);

            Debug.Log("use deepface");
        } else
        {
            job_Catalogue.updateAiImage(guessSys.answerId, job_Catalogue.getJobData(guessSys.answerId).FutureSpr);
            Debug.Log("use save image");
        }

        return isConnected;
    }

    public void setJobToUnlocked()
    {
        job_Catalogue.progressLockState(lockType.Unlocked, guessSys.answerId);
    }


    public void setJobTolocked()
    {
        job_Catalogue.progressLockState(lockType.Locked, guessSys.answerId);
    }



    public void createNewJobProfile(AIcube.REST.DeepFaceTech.Job data)
    {
        JobData temp = new JobData();
        temp.id = data.id;
        temp.name = data.name;
        temp.descriptions = new List<string>(data.descriptions);
        temp.lockstate = 0;
        temp.ImageStrOriginal = data.img;
        temp.ImageStrAI = "";
        SaveData.JobProfile.Add(temp);
    }



    public Texture2D getFutureMe(int jobid)
    {
        return job_Catalogue.getJobData(jobid).FutureSpr;
    }

    Texture2D myPhoto;
    public Texture2D getMyPhoto()
    {
        if (myPhoto == null)
        {
            myPhoto = WebcamController.createTexture2DFromStr(SaveData.imageStr);
        }
        return myPhoto;
    }
    public FullJobData getJobData(int jobid)
    {
        return job_Catalogue.getJobData(jobid);
    }

    [SerializeField]
    RawImageWithRatio PhotoTakingTemp;
    public void Snapshot()
    {
        webcam.generateWebcamTextureString(WebcamController.ImageFormat.JPEG);
        myPhoto = WebcamController.createTexture2DFromStr(webcam.imagestring);
        PhotoTakingTemp.setTextureAndRatio(myPhoto, UnityEngine.UI.AspectRatioFitter.AspectMode.None);
         
    }
    public void SaveUserPhotoImageAsString()
    {
        
        SaveData.imageStr = webcam.imagestring;
        saveUserData();
    }

    public List<FullJobData> getDataFromSave()
    {
        List<FullJobData> dataList = new List<FullJobData>();

        foreach(var job in SaveData.JobProfile)
        {
            FullJobData temp = new FullJobData();
            temp.id = job.id;
            temp.descriptions = job.descriptions;
            temp.name = job.name;
            temp.lockstate = (lockType) job.lockstate;
            temp.OriginalSpr = (!string.IsNullOrEmpty(job.ImageStrOriginal)) ? WebcamController.createTexture2DFromStr(job.ImageStrOriginal) : defaultPotraitForMissingImage.texture;
            temp.FutureSpr = (!string.IsNullOrEmpty(job.ImageStrOriginal)) ? WebcamController.createTexture2DFromStr(job.ImageStrAI) : defaultPotraitForMissingImage.texture;
            dataList.Add(temp);
        }

        return dataList;
    }

    public async Task<List<FullJobData>> getDataOnlineAndMerge()
    {
        List<FullJobData> dataList = new List<FullJobData>();

        //await db
        AIcube.REST.DeepFaceTech.JobListOutput JobDataOnline = await deepface.getJobList();

        int i = 0;

        for (int j = 0; j < JobDataOnline.jobs.Count; ++j,++i)
        {
            FullJobData temp = new FullJobData();
            //Debug.Log(JobDataOnline.jobs[j].name);
            temp.descriptions = JobDataOnline.jobs[j].descriptions;
            temp.name = JobDataOnline.jobs[j].name;
            temp.id = JobDataOnline.jobs[j].id;
            //temp.hint = JobDataOnline[i].hint;
            //temp.OriginalSpr = defaultPotraitForMissingImage.texture;
            temp.OriginalSpr = WebcamController.createTexture2DFromStr(JobDataOnline.jobs[j].img);
            dataList.Add(temp);
        }
        dataList.Sort(new JobComparer());

        MergeSaveData(ref dataList, JobDataOnline);
        return dataList;
    }

    public class JobComparer : IComparer<FullJobData>
    {

        int IComparer<FullJobData>.Compare(FullJobData x, FullJobData y)
        {
            if (x.id > y.id)
                return 1;
            if (x.id < y.id)
                return -1;
            else
                return 0;
        }
    }
    void CreateNewSave()
    {
        SaveData = new PlayerJsonSaveData();
        SaveData.JobProfile = new List<JobData>();
        SaveData.user_id = SystemInfo.deviceUniqueIdentifier;
        SaveData.imageStr = "";
        SaveData.version = "";
        SaveData.policy = new PrivacyPolicy();
        SaveData.policy.agree = false;
    }

    public void PopulateSaveFile(AIcube.REST.DeepFaceTech.JobListOutput rawDataList)
    {
        for (int i = SaveData.JobProfile.Count; i < rawDataList.jobs.Count;++i)
        {
            createNewJobProfile(rawDataList.jobs[i]);
        }
    }

    void MergeSaveData(ref List<FullJobData> dataList, AIcube.REST.DeepFaceTech.JobListOutput rawDataList)
    {
        Debug.Log(SaveData.JobProfile.Count);
        int k = 0;
        for (; k < SaveData.JobProfile.Count; ++k)
        {
            SaveData.JobProfile[k].name = dataList[k].name;
            SaveData.JobProfile[k].descriptions = dataList[k].descriptions;
            SaveData.JobProfile[k].ImageStrOriginal = rawDataList.jobs[k].img;
            SaveData.JobProfile[k].id = dataList[k].id;

            dataList[k].lockstate = (lockType)SaveData.JobProfile[k].lockstate;
            dataList[k].FutureSpr = WebcamController.createTexture2DFromStr(SaveData.JobProfile[k].ImageStrAI);
        }
        if (k < rawDataList.jobs.Count)
            PopulateSaveFile(rawDataList);


    }
    string SaveFilePath = "/SaveFile";
    string SaveFileDirectory= "/Test";

    void LoadSavedFile()
    {
        string result = AIcube.Utils.IOUtils.loadStringTextfromFile(SaveFileDirectory+SaveFilePath);
        
        if (string.IsNullOrEmpty(result))
        {
            CreateNewSave();
            saveUserData();
        } else
        {
            SaveData = JsonUtility.FromJson<PlayerJsonSaveData>(result);


        }
        promptPrivacyPolicy();
    }

    [SerializeField]
    GameObject PrivacyPolicy;
    void promptPrivacyPolicy()
    {
        //Debug.Log(SaveData.policy.agree);
        PrivacyPolicy.gameObject.SetActive(!SaveData.policy.agree);
    }
    void saveUserData()
    {
        string data = JsonUtility.ToJson(SaveData);
        AIcube.Utils.IOUtils.saveDataToFile(SaveFileDirectory, SaveFilePath, data);
    }

    public void agreeToPolicy()
    {
        SaveData.policy.agree = true;
        saveUserData();
    }

    bool wasRedirected = false;
    public void checkIfUserPhotoisEmptyAndRedirect()
    {
        
        if (string.IsNullOrEmpty(SaveData.imageStr))
        {
            wasRedirected = true;
            pageControl.transitPage((int)UnityDecoupledBehavior.PageController.pageName.Selfie_Phototaking);
        } else {
            pageControl.transitPage((int)UnityDecoupledBehavior.PageController.pageName.Game_Guess);
            guessSys.StartGame();
        }
    }
    public void redirectBackToGuessing()
    {
        if (wasRedirected)
        {
            pageControl.transitPage((int)UnityDecoupledBehavior.PageController.pageName.Game_Guess);
            guessSys.StartGame();
        }
        else
        {
            pageControl.transitPage((int)UnityDecoupledBehavior.PageController.pageName.HomePage);
        }
    }

    public void updateSaveData(int jobIndex,int lockNum)
    {
        SaveData.JobProfile[jobIndex].lockstate = lockNum;
        //Debug.Log(imgstr.EncodeToJPG());
        //Debug.Log(jobIndex);
        //jobCatalagoue.
        saveUserData();
    }

    public void savePhotoToGallery()
    {
        FullJobData temp = getJobData(job_Catalogue.currentSelectedJobId);
        ShareController.SaveImageToGallery(temp.FutureSpr, Application.productName, temp.name);
    }
    public void SharePhoto()
    {
        FullJobData temp = getJobData(job_Catalogue.currentSelectedJobId);

        string paragraph = "What the job is described as is as Follows "+ JobInfoUI.mergeString(temp.descriptions);
        ShareController.SharePhoto(temp.FutureSpr, "This is how i look like when i grow up as a " + temp.name, temp.name, paragraph);
    }
}
