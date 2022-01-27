using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using AIcube.REST;

public class StarGanFree : MonoBehaviour , StarGanInterface
{


    public event Action<AIcube.REST.FaceTech.Output> On_Receive_Results;
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
        setupConnection(new Options(ip, port, secured));
    }
    async void Start()
    {
        //StartSession();
            1);
    }
    public void setupConnection(Options serverOptions)
    {
        _serverOptions = serverOptions;
        RESTServer = new RESTinterface(_serverOptions);
        RESTServer.debugOn = debugOn;
    }
    public string GetName()
    {
        return "StarGanFree";
    }

    public void StartSession()
    {
        RESTServer.StartSession();
    }
    public void EndSession()
    {
        RESTServer.EndSession();
    }
    async public Task<bool> SendStarGan(string imgstr, int style_id)
    {
        AIcube.REST.FaceTech.Input jsonObject = new AIcube.REST.FaceTech.Input();
        jsonObject.img = imgstr;
        jsonObject.session_id = RESTinterface.sessionID;
        jsonObject.style_id = style_id;
        var connectionResult = await RESTServer.PostJsonResult<AIcube.REST.FaceTech.Output, AIcube.REST.FaceTech.Input>("predict", jsonObject);

        bool isConnected = (connectionResult != null && connectionResult.isConnected);
        if (isConnected)
        {
            Debug.Log(connectionResult.jsonData);
            //if (photo not valid) {
            //connectionResult.isConnected = false;
            //} else {
            On_Receive_Results?.Invoke(connectionResult.jsonData);
            //}
        }
        return connectionResult.isConnected;
    }



}