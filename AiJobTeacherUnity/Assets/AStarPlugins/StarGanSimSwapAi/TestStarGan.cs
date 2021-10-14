using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Astar.REST;

public class TestStarGan : MonoBehaviour , StarGanInterface
{
    public event Action<Astar.REST.FaceTech.Output> On_Receive_Results;

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
    private void Awake()
    {
    }

    public string GetName()
    {
        return "StarGanTest";
    }

    public void StartSession()
    {
    }
    public void EndSession()
    {
    }

    async public Task<bool> SendStarGan(string imgstr, int style_id)
    {

        Astar.REST.FaceTech.Output result = new Astar.REST.FaceTech.Output();
        result.output_img = imgstr;
        On_Receive_Results?.Invoke(result);
        return returnVal;
    }

}
