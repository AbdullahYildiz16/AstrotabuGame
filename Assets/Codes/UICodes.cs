using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class UICodes : MonoBehaviour
{
    #region Variables
    public AudioSource audioSource;
    public AudioClip counterClip, clickClip;
    public GameObject startPanel, rulesPanel, levelChoosePanel, optionsPanel, openingGamePanel, gamePanel, pauseGamePanel, expPanel, roundFinishedPanel, gameFinishedPanel;
    public Text firstInputText, secondInputText, yourTurnTeamNameText;
    public Text mainWordText, bannedWordsText, expText, expCounterText, gameCounterText;
    public Text roundScoreText,firstTeamNameText, secondTeamNameText, firstRoundScoreText, afterTeamText,
        firstTeamFinishedText, secondTeamFinishedText, firstTeamFinishedScoreText, secondTeamFinishedScoreText, winnerText1, winnerText2;
    public TextAsset easyMainWordsFile, midMainWordsFile, hardMainWordsFile;
    public TextAsset mainWordsFile, bannedWords1File, bannedWords2File, bannedWords3File, bannedWords4File, levelWordsFile, wordsExpFile;
    public RectTransform noPassBtn, yesPassBtn;
    public Slider wordSlider;
    public Image openingCounterImg, soundBtnImg, timeRocket, timeTarget;
    public Sprite number3, number2, number1, soundOn, soundOff;
    public int firstTeamScore, secondTeamScore;
    [HideInInspector] public int gameLevel;
    [HideInInspector] public int timeLimit;
    [HideInInspector] public int passAmount = 0;
    public int currentTeam = 1;
    [HideInInspector] public string firstTeamName, secondTeamName;
    private List<string> mainWordsList, bannedWordsList1, bannedWordsList2, bannedWordsList3, bannedWordsList4, levelWordsList, wordsExpList;
    private string text;
    private bool isMusicPlaying = true;
    private bool isExpPanelOpened;
    bool isGamePaused = false;
    private Vector3 rocketStarterPos;
    #endregion


    private void Start()
    {
        currentTeam = 1;
        timeLimit = 40;
        mainWordsList = new List<string>();
        bannedWordsList1 = new List<string>();
        bannedWordsList2 = new List<string>();
        bannedWordsList3 = new List<string>();
        bannedWordsList4 = new List<string>();
        levelWordsList = new List<string>();
        wordsExpList = new List<string>();
        SceneManager sceneManager = new SceneManager();
        rocketStarterPos = timeRocket.rectTransform.anchoredPosition;
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
        roundFinishedPanel.SetActive(false);
        gameFinishedPanel.SetActive(false);
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
            soundBtnImg.sprite = soundOff;
            
        }
        else
        {
            audioSource.Play();
            isMusicPlaying = true;
            soundBtnImg.sprite = soundOn;
        }
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
        ReadFiles(gameLevel);
    }
    public void MidBtnClicked()
    {
        gameLevel = 1;
        CloseAllPanels();
        optionsPanel.SetActive(true);
        ReadFiles(gameLevel);
    }
    public void HardBtnClicked()
    {
        gameLevel = 2;
        CloseAllPanels();
        optionsPanel.SetActive(true);
        ReadFiles(gameLevel);
    }
    public void MixedBtnClicked()
    {
        gameLevel = 3;
        CloseAllPanels();
        optionsPanel.SetActive(true);
        ReadFiles(gameLevel);
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
        OpenGamePanel();
        
        
    }
    public void OpenGamePanel()
    {
        CloseAllPanels();
        GetNewWord();
        gamePanel.SetActive(true);
        StartCoroutine(gameCounter());
    }
    public void WordSliderValueChanged()
    {
        if (wordSlider.value != 0f || wordSlider.value != 0.5f || wordSlider.value != 1f)
        {
            if (wordSlider.value >= 0.75f)
            {
                wordSlider.value = 1f;
                timeLimit = 60;
            }
            else if (wordSlider.value <= 0.25f)
            {
                wordSlider.value = 0f;
                timeLimit = 40;
            }
            else
            {
                wordSlider.value = 0.5f;
                timeLimit = 50;
            }

        }
        
    }
    public void ForwardBtnClicked()
    {
        if (firstInputText.text.Length != 0 && secondInputText.text.Length != 0)
        {
            currentTeam = 1;
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

    #region GamePanel - PauseGamePanel - RoundFinishedPanel

    private void ReadFiles(int gamelevel)
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
            for (int i = 0; i < levelWordsList.Count; i++)
            {
                if (levelWordsList[i] != "KOLAY")
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
        GetNewWord();
    }
    private void GetNewWord()
    {
        int randomNumber = Random.Range(0, mainWordsList.Count - 1);
        mainWordText.text = mainWordsList[randomNumber];
        bannedWordsText.text = bannedWordsList1[randomNumber] +
            "\n" + bannedWordsList2[randomNumber] + "\n" +
            bannedWordsList3[randomNumber] + "\n" +
            bannedWordsList4[randomNumber];
        expText.text = wordsExpList[randomNumber];
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
    IEnumerator gameCounter()
    {
        timeRocket.rectTransform.anchoredPosition = rocketStarterPos;
        Vector2 distance = (timeTarget.rectTransform.anchoredPosition - timeRocket.rectTransform.anchoredPosition)
            - (Vector2.up * timeTarget.rectTransform.rect.height / 2 + Vector2.up * timeRocket.rectTransform.rect.height / 2);
        
        
        for (int i = timeLimit; i >=0; i--)
        {
            if (isGamePaused)
            {
                i++;
            }
            if (i < 10)
            {
                gameCounterText.text = "00:0" + i;
            }
            else
            {
                gameCounterText.text = "00:" + i;
            }           
            timeRocket.rectTransform.anchoredPosition += (distance / (timeLimit));
            yield return new WaitForSeconds(1f);
        }
        CloseAllPanels();
        firstTeamNameText.text = firstTeamName;
        secondTeamNameText.text = secondTeamName;
        afterTeamText.text = secondTeamName;
        
        if (currentTeam == 2)
        {
            OnGameFinished();
            gameFinishedPanel.SetActive(true);
        }
        else
        {          
            roundScoreText.text = "" + firstTeamScore;
            firstRoundScoreText.text = "" + firstTeamScore;
            roundFinishedPanel.SetActive(true);
            currentTeam = 2;
        }
        
    }
    public void PauseBtnClicked()
    {
        CloseAllPanels();
        isGamePaused = true;
        pauseGamePanel.SetActive(true);
    }
    public void ReturnCurrentGameBtnClicked()
    {
        pauseGamePanel.SetActive(false);
        isGamePaused = false;
        gamePanel.SetActive(true);
    }
    IEnumerator keyBtnClickedNum()
    {
        expPanel.SetActive(true);
        expCounterText.text = "5";
        yield return new WaitForSeconds(1f);
        expCounterText.text = "4";
        yield return new WaitForSeconds(1f);
        expCounterText.text = "3";
        yield return new WaitForSeconds(1f);
        expCounterText.text = "2";
        yield return new WaitForSeconds(1f);
        expCounterText.text = "1";
        yield return new WaitForSeconds(1f);
        expPanel.SetActive(false);

    }
    public void KeyBtnClicked()
    {
        if (!isExpPanelOpened)
        {
            StartCoroutine(keyBtnClickedNum());
            isExpPanelOpened = true;
        }
        else
        {
            expPanel.SetActive(false);
        }
        
    }
    public void PassBtnClicked()
    {
        if (passAmount > 0)
        {
            GetNewWord();
            passAmount--;
        }
    }
    public void TrueBtnClicked()
    {
        if (currentTeam == 1)
        {
            firstTeamScore++;
        }
        else
        {
            secondTeamScore++;
        }
        GetNewWord();     
    }
    public void FalseBtnClicked()
    {
        if (currentTeam == 1)
        {
            firstTeamScore--;
        }
        else
        {
            secondTeamScore--;
        }
        GetNewWord();     
    }

    #endregion

    #region GameFinishedPanel

    void OnGameFinished()
    {
        firstTeamFinishedText.text = firstTeamName;
        secondTeamFinishedText.text = secondTeamName;
        firstTeamFinishedScoreText.text = "" + firstTeamScore;
        secondTeamFinishedScoreText.text = "" + secondTeamScore;
        if (firstTeamScore > secondTeamScore)
        {
            winnerText1.text = "Kazanan!";
        }
        else if (secondTeamScore > firstTeamScore)
        {
            winnerText2.text = "Kazanan!";
        }
        else
        {
            winnerText1.text = "Berabere!";
            winnerText2.text = "Berabere!";
        }
        GetNewWord();
    }
    public void PlayAgainClicked()
    {
        SceneManager.LoadScene(0);
    }
    #endregion
}
