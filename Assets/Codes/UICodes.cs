using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class UICodes : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip counterClip, clickClip;
    public GameObject startPanel, rulesPanel, levelChoosePanel, optionsPanel, openingGamePanel, gamePanel, pauseGamePanel;
    public Text firstInputText, secondInputText, yourTurnTeamNameText;
    public Text mainWordText, bannedWordsText;
    public TextAsset easyMainWordsFile, midMainWordsFile, hardMainWordsFile;
    public TextAsset mainWordsFile, bannedWords1File, bannedWords2File, bannedWords3File, bannedWords4File, levelWordsFile, wordsExpFile;
    public RectTransform noPassBtn, yesPassBtn;
    public Slider wordSlider;
    public Image openingCounterImg, soundBtnImg;
    public Sprite number3, number2, number1;
    [HideInInspector] public int gameLevel;
    [HideInInspector] public int wordLimit;
    [HideInInspector] public int passAmount;
    [HideInInspector] public int currentTeam = 0;
    [HideInInspector] public string firstTeamName, secondTeamName;
    [HideInInspector] public bool isMusicPlaying = true;
    private List<string> mainWordsList, bannedWordsList1, bannedWordsList2, bannedWordsList3, bannedWordsList4, levelWordsList, wordsExpList;
    private string text;


    private void Start()
    {
        wordLimit = 40;
        mainWordsList = new List<string>();
        bannedWordsList1 = new List<string>();
        bannedWordsList2 = new List<string>();
        bannedWordsList3 = new List<string>();
        bannedWordsList4 = new List<string>();
        levelWordsList = new List<string>();
        wordsExpList = new List<string>();
       
        getWords(0);
    }
    private void getWords(int gamelevel)
    {

        readTextsToLines(mainWordsFile, mainWordsList);
        readTextsToLines(bannedWords1File, bannedWordsList1);
        readTextsToLines(bannedWords2File, bannedWordsList2);
        readTextsToLines(bannedWords3File, bannedWordsList3);
        readTextsToLines(bannedWords4File, bannedWordsList4);
        readTextsToLines(levelWordsFile, levelWordsList);
        readTextsToLines(wordsExpFile, wordsExpList);
        
        if (gamelevel == 0)
        {
            for(int i = 0; i< levelWordsList.Count; i++)
            {
                if (levelWordsList[i] != "KOLAY" )
                {
                    levelWordsList.RemoveAt(i);
                    mainWordsList.RemoveAt(i);
                    bannedWordsList1.RemoveAt(i);
                    bannedWordsList2.RemoveAt(i);
                    bannedWordsList3.RemoveAt(i);
                    bannedWordsList4.RemoveAt(i);
                    wordsExpList.RemoveAt(i);
                }
            }
        }
        else if (gamelevel == 1)
        {
            for (int i = 0; i < levelWordsList.Count; i++)
            {
                if (levelWordsList[i] != "ORTA")
                {
                    levelWordsList.RemoveAt(i);
                    mainWordsList.RemoveAt(i);
                    bannedWordsList1.RemoveAt(i);
                    bannedWordsList2.RemoveAt(i);
                    bannedWordsList3.RemoveAt(i);
                    bannedWordsList4.RemoveAt(i);
                    wordsExpList.RemoveAt(i);
                }
            }
        }
        else if (gamelevel == 2)
        {
            for (int i = 0; i < levelWordsList.Count; i++)
            {
                if (levelWordsList[i] != "ZOR")
                {
                    levelWordsList.RemoveAt(i);
                    mainWordsList.RemoveAt(i);
                    bannedWordsList1.RemoveAt(i);
                    bannedWordsList2.RemoveAt(i);
                    bannedWordsList3.RemoveAt(i);
                    bannedWordsList4.RemoveAt(i);
                    wordsExpList.RemoveAt(i);
                }
            }
        }

        
        mainWordText.text = mainWordsList[Random.Range(0, mainWordsList.Count - 1)];
        bannedWordsText.text = bannedWordsList1[Random.Range(0, mainWordsList.Count - 1)] + 
            "\n" + bannedWordsList2[Random.Range(0, mainWordsList.Count - 1)] + "\n" +
            bannedWordsList2[Random.Range(0, mainWordsList.Count - 1)] + "\n" +
            bannedWordsList3[Random.Range(0, mainWordsList.Count - 1)] + "\n" +
            bannedWordsList4[Random.Range(0, mainWordsList.Count - 1)];

    }
    void readTextsToLines(TextAsset textAsset, List<string> wordList)
    {
        text = textAsset.text;
        StringReader stringReader = new StringReader(text);
        string line;
        while ((line = stringReader.ReadLine()) != null)
        {
            
            wordList.Add(line);
        }
    }
    
    #region StartPanel
    public void CloseAllPanels()
    {
        startPanel.SetActive(false);
        rulesPanel.SetActive(false);
        levelChoosePanel.SetActive(false);
        optionsPanel.SetActive(false);
        openingGamePanel.SetActive(false);
        gamePanel.SetActive(false);
        pauseGamePanel.SetActive(false);
    }

    public void StartBtnClicked()
    {
        CloseAllPanels();
        levelChoosePanel.SetActive(true);
    }
    public void RulesBtnClicked()
    {
        CloseAllPanels();
        rulesPanel.SetActive(true);
    }
    public void SoundBtnClicked()
    {
        if (isMusicPlaying)
        {
            audioSource.Stop();
            isMusicPlaying = false;
        }
        else
        {
            audioSource.Play();
            isMusicPlaying = true;
        }
    }
    public void ShareBtnClicked()
    {

    }

    #endregion

    #region RulesPanel
    public void BackFromRulesBtnClicked()
    {
        CloseAllPanels();
        startPanel.SetActive(true);
    }
    #endregion

    #region LevelChoosePanel

    public void EasyBtnClicked()
    {
        gameLevel = 0;
        CloseAllPanels();
        optionsPanel.SetActive(true);
    }
    public void MidBtnClicked()
    {
        gameLevel = 1;
        CloseAllPanels();
        optionsPanel.SetActive(true);
    }
    public void HardBtnClicked()
    {
        gameLevel = 2;
        CloseAllPanels();
        optionsPanel.SetActive(true);
    }
    public void MixedBtnClicked()
    {
        gameLevel = 3;
        CloseAllPanels();
        optionsPanel.SetActive(true);
    }
    public void BackFromLevelBtnClicked()
    {
        CloseAllPanels();
        startPanel.SetActive(true);
    }
    #endregion

    #region OptionsPanel - OpeningGamePanel
    public IEnumerator StartOpeningCounter(float seconds)
    {
        audioSource.Stop();
        audioSource.PlayOneShot(counterClip);
        openingCounterImg.sprite = number3;
        yield return new WaitForSeconds(seconds);
        openingCounterImg.sprite = number2;
        yield return new WaitForSeconds(seconds);
        openingCounterImg.sprite = number1;
        yield return new WaitForSeconds(seconds);
        CloseAllPanels();
        gamePanel.SetActive(true);
        
    }
    public void WordSliderValueChanged()
    {
        if (wordSlider.value != 0f || wordSlider.value != 0.5f || wordSlider.value != 1f)
        {
            if (wordSlider.value >= 0.75f)
            {
                wordSlider.value = 1f;
                wordLimit = 60;
            }
            else if (wordSlider.value <= 0.25f)
            {
                wordSlider.value = 0f;
                wordLimit = 40;
            }
            else
            {
                wordSlider.value = 0.5f;
                wordLimit = 50;
            }

        }
        
    }
    public void ForwardBtnClicked()
    {
        if (firstInputText.text.Length != 0 && secondInputText.text.Length != 0)
        {
            currentTeam = 0;
            firstTeamName = firstInputText.text;
            secondTeamName = secondInputText.text;
            if (firstTeamName.Length >= 11)
            {
                firstTeamName = firstTeamName.Substring(0, 11);
            }
            if (secondTeamName.Length >= 11)
            {
                secondTeamName = secondTeamName.Substring(0, 11);
            }
            yourTurnTeamNameText.text = firstTeamName + " Takýmý";
            CloseAllPanels();
            openingGamePanel.SetActive(true);
            StartCoroutine(StartOpeningCounter(1));
        }
        
    }
    public void BackFromOptionsBtnClicked()
    {
        CloseAllPanels();
        levelChoosePanel.SetActive(true);
    }
    public void NoPassBtnClicked()
    {
        passAmount = 0;
        noPassBtn.localScale = new Vector3(1.25f, 1.25f, 1);
        yesPassBtn.localScale = Vector3.one;
    }
    public void YesPassBtnClicked()
    {
        passAmount = 3;
        yesPassBtn.localScale = new Vector3(1.25f, 1.25f, 1);
        noPassBtn.localScale = Vector3.one;
    }
    #endregion

    #region GamePanel - PauseGamePanel

    public void PauseBtnClicked()
    {
        CloseAllPanels();
        pauseGamePanel.SetActive(true);
    }
    public void ReturnCurrentGameBtnClicked()
    {
        pauseGamePanel.SetActive(false);
        gamePanel.SetActive(true);
    }
    #endregion

}
