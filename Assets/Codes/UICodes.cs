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
    public RectTransform noPassBtn, yesPassBtn;
    public Slider wordSlider;
    public Image openingCounterImg, soundBtnImg;
    public Sprite number3, number2, number1, soundOffImg;
    [HideInInspector] public int gameLevel;
    [HideInInspector] public int wordLimit;
    [HideInInspector] public int passAmount;
    [HideInInspector] public int currentTeam = 0;
    [HideInInspector] public string firstTeamName, secondTeamName;
    [HideInInspector] public bool isMusicPlaying = true;
    private List<string> mainWordsList, mainWordsList2, bannedWordsList;
    private string text;


    private void Start()
    {
        wordLimit = 40;
        mainWordsList = new List<string>();
        mainWordsList2 = new List<string>();
        getWords(0);
    }
    private void getWords(int gamelevel)
    {
        if (gamelevel == 0)
        {
            text = easyMainWordsFile.text;
        }
        else if (gamelevel == 1)
        {
            text = midMainWordsFile.text;
        }
        else if (gamelevel == 2)
        {
            text = hardMainWordsFile.text;
        }
        else
        {
            text = easyMainWordsFile.text + midMainWordsFile.text + hardMainWordsFile.text;
        }
        
        StringReader stringReader = new StringReader(text);
        string line;
        while ((line = stringReader.ReadLine()) != null)
        {
            mainWordsList.Add(line);          
        }
               
        for (int i = 0; i < wordLimit; i++)
        {
            int currentNumber = Random.Range(0, mainWordsList.Count - 1);
            mainWordsList2.Add(mainWordsList[currentNumber]);
            mainWordsList.RemoveAt(currentNumber);   
        }
        mainWordText.text = mainWordsList2[Random.Range(0, mainWordsList2.Count -1)];
        
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
