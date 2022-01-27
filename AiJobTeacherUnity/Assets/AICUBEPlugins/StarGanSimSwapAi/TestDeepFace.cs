using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using AIcube.REST;
using AIcube.REST.DeepFaceTech;

public class TestDeepFace : MonoBehaviour , DeepfaceInterface
{
    public event Action<AIcube.REST.DeepFaceTech.ImageOutput> On_Receive_Results;

    [SerializeField]
    private string ip;
    [SerializeField]
    private string port;
    [SerializeField]
    private bool secured;
    //[SerializeField]
    //private bool debugOn = false;
    [SerializeField]
    private bool returnVal = false;

    event Action<ImageOutput> DeepfaceInterface.On_Receive_Results
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    private void Awake()
    {
    }

    public string GetName()
    {
        return "DeepFaceTest";
    }

    public void StartSession()
    {
    }
    public void EndSession()
    {
    }

    async public Task<bool> SendDeepFace(string imgstr, string user_id, int job_id)
    {
        //Debug.Log("receive sendDeepface");
        AIcube.REST.DeepFaceTech.ImageOutput result = new AIcube.REST.DeepFaceTech.ImageOutput();
        result.output_img = imgstr;
        On_Receive_Results?.Invoke(result);
        return returnVal;
    }

    async public Task<JobListOutput> GetJobList()
    {

        JobListOutput result = new JobListOutput();
        result.jobs = new List<Job>();
        Job temp = new Job();
        temp.descriptions = new List<string>();
        temp.descriptions.Add("testing");
        temp.id = -1;
        temp.name = "Tester";
        result.jobs.Add(temp);
        //Debug.Log(temp.name);
        return result;
    }

    public void InitConnection()
    {
        Debug.Log("No Rest, this is test");
        return;
    }

    Options DeepfaceInterface.GetServerOptions()
    {
        return new Options(ip, port, secured);
    }
}
