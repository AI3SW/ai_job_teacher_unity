using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellingChar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI CurrentChar;
    void Awake()
    {
        clearLetter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setLetter(string letter)
    {
        CurrentChar.text = letter;
    }
    public void clearLetter()
    {
        CurrentChar.text = "";
    }
}
