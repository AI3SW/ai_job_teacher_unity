using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks; // Task, is an object that handles threads, in essence its the same as a Coroutine
using System;
public class StarGan_Controller : MonoBehaviour
{

    [System.Serializable]
    public enum Source
    {
        Free = 0,
        Free2,
        Test,
        Deepface

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

    private StarGanInterface selectedAI;
    public event Action<AIcube.REST.FaceTech.Output> On_Receive_Results;
    public List<GameObject> AIModels;
    // Start is called before the first frame update
    void Start()
    {
        Init(selectedSrc);
    }


    public void Process_Results(AIcube.REST.FaceTech.Output data)
    {
        //_textbox.text = "Debug : " + data;
        On_Receive_Results?.Invoke(data);
        Debug.Log(data.output_img);
    }

    void Init(Source src)
    {
        //Debug.Log("Init");
        selectedAI = Instantiate<GameObject>(AIModels[(int)_selectedSrc], this.transform).GetComponent<StarGanInterface>();
        selectedAI.On_Receive_Results += Process_Results;
        //recordAndSend();
    }

    public void StartSession()
    {
        selectedAI.StartSession();
    }
    public void EndSession()
    {
        selectedAI.EndSession();
    }

    async public Task<bool> useStarGan(string imgstr, int style_id)
    {
        return await selectedAI.SendStarGan(imgstr, style_id);
    }
}
