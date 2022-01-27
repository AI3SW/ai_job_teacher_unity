using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class GuessSystem : MonoBehaviour
{
    GuessSystemUI gSysUI;

    private string answer;
    public int answerId;
    private StringBuilder guess = new StringBuilder();

    [SerializeField]
    private RectTransform SpellingArea;
    public SpellingChar SpellingCharPrefab;
    [SerializeField]
    private List<SpellingChar> spellingCharList;

    [SerializeField]
    private List<AlphabetButton> AlphabetList;
    private HashSet<char> UniqueAlphabets;

    public static UnityEventString OnAddingLetter = new UnityEventString();
    public static UnityEvent OnRemovingLetter = new UnityEvent();

    [SerializeField]
    UnityEvent OnCorrectGuess;
    [SerializeField]
    UnityEvent OnWrongGuess;

    [SerializeField]
    Button StartButton;

    [SerializeField]
    public GuessSystemUI guessUI;
    // Start is called before the first frame update
    void Start()
    {
        gSysUI=GetComponent<GuessSystemUI>();
        OnAddingLetter.AddListener(AddLetter);
        OnRemovingLetter.AddListener(RemoveLetter);
        UniqueAlphabets = new HashSet<char>();
        InitializeGuessingGame();
    }

    void AddLetter(string singleCharString)
    {
        if (guess.Length == answer.Length) return;

        
        if(answer[guess.Length] == ' ')
        {
            guess.Append(" ");
            spellingCharList[guess.Length - 1].setLetter(" ");
        }
            
        guess.Append(singleCharString);
        spellingCharList[guess.Length - 1].setLetter(singleCharString);
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if(answer.Length == guess.Length )
        {
            if (string.Compare(guess.ToString(), answer) == 0)
                OnCorrectGuess?.Invoke();
            else
                OnWrongGuess?.Invoke();
        } else
        {
            //Debug.Log("nothing happens");

        }
    }

    public void RemoveLetter()
    {
        if (guess.Length == 0) return;
        spellingCharList[guess.Length - 1].clearLetter();
        //Debug.Log(guess.Length);
        guess.Remove(guess.Length - 1,1);
        if(guess.Length > 1)
            if(spellingCharList[guess.Length - 1].getLetter() == " ")
            {
                //spellingCharList[guess.Length - 1].clearLetter();
                guess.Remove(guess.Length - 1, 1);
            }
        //guess.TrimEnd();
        //Debug.Log(guess.Length);

    }

    public void ResetSpellingArea()
    {
        guess.Remove(0, guess.Length);
        foreach (var obj in spellingCharList)
        {
            obj.clearLetter();
            obj.gameObject.SetActive(false);
        }
    }
    void InitializeSpellChar(int count)
    {
        //Look for longest wordcount in DB
        for(int i = 0; i < count; ++i)
        {
            SpellingChar temp = Instantiate<SpellingChar>(SpellingCharPrefab, SpellingArea);
            spellingCharList.Add(temp);
        }
        
        
    }

    List<string> tempList = new List<string>();

    void InitializeGuessingGame() // has psudo data pushed in as argument from API call from Main system
    {
       
        InitializeSpellChar(12);

        StartButton.interactable = false;

    }

    public void ClearAndPopulateGameList(List<AIcube.AITeacher.FullJobData> jobList,bool localData)
    {
        tempList.Clear();
        foreach(var job in jobList)
        {
            if (localData && job.lockstate == AIcube.AITeacher.lockType.Unknown) continue;
            tempList.Add(job.name);
        }
        StartButton.interactable = true;
    }

    void randomizeJob()
    {
        Debug.Log(tempList.Count);
        int randomID = UnityEngine.Random.Range(0, tempList.Count);
        GenerateWord(tempList[randomID]);
        answerId = AppManager.Singleton.job_Catalogue.getIdFromJobName(tempList[randomID]);
        ShuffleButtons();

    }

    void generateSpecificword(int id)
    {
        GenerateWord(tempList[id]);
        ShuffleButtons();
    }

    void ShuffleButtons()
    {
        //Generate RandomAlphabets
        //repeated alphabet if existed
        int invalidLetterCount = 10 - UniqueAlphabets.Count;
        HashSet<char> allAlphabets = CreateAllEnglishAlphabets();
        
        string alphabetsToUse = getValidAlphabet(UniqueAlphabets, allAlphabets); 
        alphabetsToUse += RandomizeInvalidAlphabets(invalidLetterCount, allAlphabets);
        Debug.Log(alphabetsToUse);

        List<int> stringCount = new List<int>();
        populateListWithIndex(stringCount, alphabetsToUse.Length);
        List<int> buttonCount = new List<int>();
        populateListWithIndex(buttonCount, 10);

        
        //Get RandomAlphabetButton with random alphabet
        do
        {
            int randomCharIndex = UnityEngine.Random.Range(0, stringCount.Count);
            int randombtnIndex = UnityEngine.Random.Range(0, stringCount.Count);
            int charValue = stringCount[randomCharIndex];
            int btnValue = buttonCount[randombtnIndex];
            char tempchar = alphabetsToUse.ToCharArray()[charValue];
            AlphabetButton tempButton = AlphabetList[btnValue];
            tempButton.setCurrentLetter(tempchar.ToString());

            stringCount.RemoveAt(randomCharIndex);
            buttonCount.RemoveAt(randombtnIndex);

        }
        while (stringCount.Count != 0 && buttonCount.Count != 0 );

        
    }

    private void populateListWithIndex(List<int> list, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            list.Add(i);
        }
    }
    private HashSet<char> CreateAllEnglishAlphabets() 
    {
        HashSet<char> usedAlphabetsId = new HashSet<char>();

        for(char i = 'A'; i <= 'Z'; ++i)
        {
            usedAlphabetsId.Add(i);
        }
        return usedAlphabetsId;
    }
    private string RandomizeInvalidAlphabets(int count, HashSet<char> currentAlphabetList)
    {
        string alphabetsToUse = "";
        for(int i = 0; i < count; )
        {
            //Get Random Alphabet from Existing Pool
            int rng = UnityEngine.Random.Range(0, 26);
            char randomChar = (char)('A' + (char)rng);
            if (currentAlphabetList.Contains(randomChar))
            {
                currentAlphabetList.Remove(randomChar);
                alphabetsToUse += randomChar.ToString();
                ++i;

            }
                
        }
        return alphabetsToUse;
    }

    private string getValidAlphabet(HashSet<char> uniqueAlphabetHash, HashSet<char> currentAlphabetHash)
    {
        List<char> currentAlphabetList = new List<char>(uniqueAlphabetHash);
        string validAlphabet = "";
        foreach (char letter in uniqueAlphabetHash)
        {
            //Debug.Log(letter);
            validAlphabet += letter.ToString();
            currentAlphabetHash.Remove(letter);
        }

        return validAlphabet;

    }

    void GenerateWord(string newWord)
    {
        answer = newWord.ToUpper();
        answer.Replace(" ", "");
        ResetSpellingArea();
        UniqueAlphabets.Clear();
        //Debug.Log(SplitIntoUniqueList(("Hellllllo").ToCharArray()).Count); // test , 4
        UniqueAlphabets =  SplitIntoUniqueList(answer.Replace(" ", "").ToCharArray());
        //Debug.Log(UniqueAlphabets.Count);
        int count = answer.Length;
        foreach(var obj in spellingCharList)
        {
            if (count > 0) {
                obj.gameObject.SetActive(true);
                obj.clearLetter();
                if(answer[answer.Length - count] != ' ')
                {
                    obj.enableWord();
                    
                } else
                {
                    obj.disableWord();
                }
            }
            else
            {
                break;
            }
            --count;
        }
    }

    HashSet<char> SplitIntoUniqueList(char[] words)
    {
        HashSet<char> uniqueAlphabets = new HashSet<char>();
        foreach(char letter in words)
        {
            uniqueAlphabets.Add(letter);
        }
        return uniqueAlphabets;
    }

    public async void SkipWord()
    {
        AppManager.Singleton.loadingScreen.SetActive(true);
        randomizeJob();
        bool isconnected = await AppManager.Singleton.getOnlineJobAIPhoto();

        /*
        if (!isconnected)
        {
            
            while (string.IsNullOrEmpty(AppManager.Singleton.SaveData.JobProfile[answerId].ImageStrAI))
            {
                answerId = UnityEngine.Random.Range(0, AppManager.Singleton.SaveData.JobProfile.Count);

            }
            generateSpecificword(answerId);
        }
        */
        gSysUI.updateUIData(answerId);
        AppManager.Singleton.loadingScreen.SetActive(false);
    }


    public void StartGame()
    {
        AppManager.Singleton.pageControl.transitPage(1);
        SkipWord();
    }
}
