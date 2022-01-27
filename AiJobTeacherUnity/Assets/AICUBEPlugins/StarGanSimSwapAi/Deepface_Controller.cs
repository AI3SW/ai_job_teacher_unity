using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks; // Task, is an object that handles threads, in essence its the same as a Coroutine
using System;
using UnityEngine.Events;
public class Deepface_Controller : MonoBehaviour
{

    [System.Serializable]
    public enum Source
    {

        Test,
        Example,
    }
    
    [SerializeField]
    private Source _selectedSrc;

    public Source selectedSrc
    {
        get
        {
            return _selectedSrc;
        }
        set
        {

            _selectedSrc = value;
        }
    }

    private DeepfaceInterface selectedAI;
    public UnityEvent<AIcube.REST.DeepFaceTech.ImageOutput> On_Receive_Results;
    public List<GameObject> AIModels;
    // Start is called before the first frame update
    void Start()
    {
        Init(selectedSrc);
    }


    public void Process_Results(AIcube.REST.DeepFaceTech.ImageOutput data)
    {
        //Debug.Log("process added");
        //_textbox.text = "Debug : " + data;
        On_Receive_Results?.Invoke(data);
        //Debug.Log(data.output_img);
    }

    bool instantiated = false;
    void Init(Source src)
    {
        instantiated = false;
        //Debug.Log("Init");
        selectedAI = Instantiate<GameObject>(AIModels[(int)_selectedSrc], this.transform).GetComponent<DeepfaceInterface>();
        selectedAI.InitConnection();
        selectedAI.On_Receive_Results += Process_Results;
        //recordAndSend();
        //Debug.Log("process added");
        instantiated = true;
        StartSession();
    }

    public AIcube.REST.Options getServerOptions()
    {
        
        return AIModels[(int)_selectedSrc].GetComponent<DeepfaceInterface>().GetServerOptions();
    }

    public void StartSession()
    {
        selectedAI.StartSession();
    }
    public void EndSession()
    {
        selectedAI.EndSession();
    }

    async public Task<bool> SendDeepFace(string imgstr, string user_id, int job_id)
    {
        return await selectedAI.SendDeepFace(imgstr, user_id,job_id);
    }
    async public Task<AIcube.REST.DeepFaceTech.JobListOutput> getJobList()
    {
        
        await Awaiters.Until(() => instantiated == true); ;
        //Debug.Log("isTrue");
        return await selectedAI.GetJobList();
    }

}
