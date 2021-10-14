using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAppFunctions : MonoBehaviour
{

    public void QuitApp()
    {
        Application.Quit();
    }
    public void DebugText(string text)
    {
        Debug.Log(text);
    }

}
