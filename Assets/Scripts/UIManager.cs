using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI Top Part")]
    public TextMeshProUGUI currentRoundText;
    public TextMeshProUGUI currentCharacterNameText;
    public TextMeshProUGUI currentCharacterRuleText;

    [Header("Display Sheets")]
    public GameObject inspirationSheet;
    public TextMeshProUGUI generatedPoemText;
    public TextMeshProUGUI inspirationClassificationText;
    public TextMeshProUGUI inspirationScoreTotalText;

    public GameObject impressionSheet;
    public TextMeshProUGUI impressionGeneratedPoemText;
    public TextMeshProUGUI impressionClassificationText;
    public TextMeshProUGUI impressionScoreTotalText;

    [Header("UI Bottom Part")]
    public TextMeshProUGUI suggestionsLeftText;
    public TextMeshProUGUI manaLeftText;
    public TextMeshProUGUI insipirationScoreText;
    public TextMeshProUGUI impressionScoreText;
    public TextMeshProUGUI currentScoreText;
    public TextMeshProUGUI targetScoreText;
    public TMP_InputField word1Input;
    public TMP_InputField word2Input;
    public TMP_InputField word3Input;
    //public TextMeshProUGUI generatedPoemText;
    //public TextMeshProUGUI corpusVerseText;
    //public TextMeshProUGUI scoreText;
    public static UIManager instance;

    public GameManager gameManager;

    private void Awake()
    {
        instance = this;
    }

    public void OnSubmitButtonClicked()
    {
        string[] words = {
            word1Input.text.Trim(),
            word2Input.text.Trim(),
            word3Input.text.Trim()
        };

        RAGController.instance.CallPostGeneratePoem(words);
    }

    public void SkipRound()
    {
        GameManager.instance.ProcessScore(9999);
    }

    public void DisplayGeneratedPoem(string poem)
    {
        generatedPoemText.text = "Poema gerado:\n" + poem;
    }

    public void DisplayCorpusVerse(string verse)
    {
        //corpusVerseText.text = "Verso encontrado:\n" + verse;
    }

    public void UpdateScore(int score)
    {
        //scoreText.text = "Pontuação: " + score;
    }

    public void Update()
    {
        currentRoundText.text = $"Round {GameManager.instance.currentRound + 1} de 6";
        currentCharacterNameText.text = $"{GameManager.instance.characterNames[GameManager.instance.currentRound]}";
        currentCharacterRuleText.text = $"{GameManager.instance.specialRules[GameManager.instance.currentRound]}";

        suggestionsLeftText.text = GameManager.instance.suggestions.ToString();
        manaLeftText.text = GameManager.instance.mana.ToString();

        insipirationScoreText.text = GameManager.instance.currentInspiration.ToString();
        impressionScoreText.text = GameManager.instance.currentInspiration.ToString();
        currentScoreText.text = GameManager.instance.currentScore.ToString();
        targetScoreText.text = GameManager.instance.targetScore.ToString();
    }
}

