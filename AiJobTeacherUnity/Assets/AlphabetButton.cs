using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AlphabetButton : MonoBehaviour
{
    [SerializeField]
    string CurrentLetter;
    [SerializeField]
    private TextMeshProUGUI Alphabet;
    [SerializeField]
    private Button _btn;

    void Start()
    {
        _btn.onClick.AddListener(sendCurrentLetter);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void sendCurrentLetter()
    {

        GuessSystem.OnAddingLetter?.Invoke(CurrentLetter);
    }

    public void setCurrentLetter(string letter)
    {
        Alphabet.text = CurrentLetter = letter.ToUpper();
    }
}
