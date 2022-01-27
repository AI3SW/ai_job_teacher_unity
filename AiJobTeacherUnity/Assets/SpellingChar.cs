using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpellingChar : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI CurrentChar;
    [SerializeField]
    private Image line;
    private Color original;
    void Awake()
    {
        clearLetter();
        original = line.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public string getLetter()
    {
        return CurrentChar.text;
    }
    public void setLetter(string letter)
    {
        CurrentChar.text = letter;
    }

    public void disableWord ()
    {
        line.enabled = false;
    }
    public void clearLetter()
    {
        CurrentChar.text = "";
    }
    public void enableWord()
    {
        line.enabled = true;
    }
}
