using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using AIcube.REST;
using AIcube.REST.DeepFaceTech;

public class DeepFace : MonoBehaviour , DeepfaceInterface
{


    public event Action<AIcube.REST.DeepFaceTech.ImageOutput> On_Receive_Results;
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
        AIcube.REST.DeepFaceTech.ImageInput jsonObject = new AIcube.REST.DeepFaceTech.ImageInput();
        jsonObject.img = imgstr;
        jsonObject.user_id = user_id;
        jsonObject.job_id = job_id;
        //jsonObject.style_id = style_id;
        //Debug.Log(jsonObject.img);

        bool connectionResult = false;
        /*
        Ping ping = new Ping(ip);
        float timeout = 2000;
        float timer = Time.realtimeSinceStartup;

        await new WaitUntil(() => { return ping.isDone; });
        timer = Time.realtimeSinceStartup - timer;
        Debug.Log(timer);


        if (ping.time < 0 || ping.time >= timeout)
        {
            //do failure stuff
        }
        else
        {

        }
        */
        var connection =
await RESTServer.PostJsonResult<AIcube.REST.DeepFaceTech.ImageOutput, AIcube.REST.DeepFaceTech.ImageInput>("image", jsonObject);

        bool isConnected = (connection != null && connection.isConnected);
        //Debug.Log(connectionResult.jsonData);
        if (isConnected)
        {
            //Debug.Log(connectionResult.jsonData);
            //if (string.IsNullOrEmpty(connectionResult.jsonData.output_img)) {
            ///connectionResult.isConnected = false;
            //} else {
            On_Receive_Results?.Invoke(connection.jsonData);
            //}
        }
        connectionResult = connection.isConnected;
        return connectionResult;
    }
    async public Task<JobListOutput> GetJobList()
    {
        JobListOutput result = null;

        var connectionResult = await RESTServer.getJsonData<AIcube.REST.DeepFaceTech.JobListOutput>("job");
        bool isConnected = (connectionResult != null && connectionResult.isConnected);
        //Debug.Log(connectionResult.jsonData);
        if (isConnected)
        {
            //Debug.Log(connectionResult.jsonData);
            result = connectionResult.jsonData;
        }

        return result;
    }

    public AIcube.REST.Options GetServerOptions()
    {
        return new Options(ip, port, secured);
    }


}