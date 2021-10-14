using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Astar.REST;
using Astar.REST.DeepFaceTech;

public class DeepFace : MonoBehaviour , DeepfaceInterface
{


    public event Action<Astar.REST.DeepFaceTech.ImageOutput> On_Receive_Results;
    private RESTinterface RESTServer;
    private Options _serverOptions;

    [SerializeField]
    private string ip;
    [SerializeField]
    private string port;
    [SerializeField]
    private bool secured;
    [SerializeField]
    private bool debugOn = false;

    private void Awake()
    {
        //setupConnection(new Options(ip, port, secured));

    }
    async void Start()
    {
        //StartSession();
            1);*/
    }

    bool instantiated = false;
    void setupConnection(Options serverOptions)
    {
        if(!instantiated)
        {
            _serverOptions = serverOptions;
            RESTServer = new RESTinterface(_serverOptions);
            RESTServer.debugOn = debugOn;
            instantiated = true;
            
        }
    }

    public void InitConnection()
    {
        setupConnection(new Options(ip, port, secured));    }

    public string GetName()
    {
        return "DeepFaceRemote";
    }

    public void StartSession()
    {
        RESTServer.StartSession();
    }
    public void EndSession()
    {
        RESTServer.EndSession();
    }
    async public Task<bool> SendDeepFace(string imgstr, string user_id, int job_id)
    {
        Astar.REST.DeepFaceTech.ImageInput jsonObject = new Astar.REST.DeepFaceTech.ImageInput();
        jsonObject.img = imgstr;
        jsonObject.user_id = user_id;
        jsonObject.job_id = job_id;
        //jsonObject.style_id = style_id;
        //Debug.Log(jsonObject.img);
        var connectionResult = 
            await RESTServer.PostJsonResult<Astar.REST.DeepFaceTech.ImageOutput, Astar.REST.DeepFaceTech.ImageInput>("image", jsonObject);

        bool isConnected = (connectionResult != null && connectionResult.isConnected);
        //Debug.Log(connectionResult.jsonData);
        if (isConnected)
        {
            //Debug.Log(connectionResult.jsonData);
            //if (string.IsNullOrEmpty(connectionResult.jsonData.output_img)) {
            ///connectionResult.isConnected = false;
            //} else {
            On_Receive_Results?.Invoke(connectionResult.jsonData);
            //}
        }
        return connectionResult.isConnected;
    }
    async public Task<JobListOutput> GetJobList()
    {
        JobListOutput result = null;

        var connectionResult = await RESTServer.getJsonData<Astar.REST.DeepFaceTech.JobListOutput>("job");
        bool isConnected = (connectionResult != null && connectionResult.isConnected);
        //Debug.Log(connectionResult.jsonData);
        if (isConnected)
        {
            //Debug.Log(connectionResult.jsonData);
            result = connectionResult.jsonData;
        }

        return result;
    }

    public Astar.REST.Options GetServerOptions()
    {
        return new Options(ip, port, secured);
    }


}