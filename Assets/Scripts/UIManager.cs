using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;

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
    public static UIManager instance;

    [Header("Screens")]
    public GameObject victoryScreen;
    public GameObject gameOverScreen;

    public GameManager gameManager;

    private void Awake()
    {
        instance = this;
    }

    public void OnGenerateButtonClicked()
    {
        if (GameManager.instance.suggestions <= 0) return;

        GameManager.instance.suggestions--;

        string[] words = {
            word1Input.text.Trim(),
            word2Input.text.Trim(),
            word3Input.text.Trim()
        };

        RAGController.instance.CallPostGeneratePoem(words);
    }

    public void OnSubmitButtonClicked()
    {
        if (GameManager.instance.mana <= 0) return;

        GameManager.instance.mana--;

        string[] words = {
            word1Input.text.Trim(),
            word2Input.text.Trim(),
            word3Input.text.Trim()
        };

        RAGController.instance.CallPostEvaluatePoem(words, GameManager.instance.specialRules[GameManager.instance.currentRound]);
    }

    public void SkipRound()
    {
        GameManager.instance.currentInspiration = 9999;
        GameManager.instance.ProcessScore();
    }

    public void OpenInspirationPanel(string generatedPoem, string classification, string scoreTotal)
    {
        inspirationSheet.SetActive(true);

        generatedPoemText.text = generatedPoem;
        inspirationClassificationText.text = classification;
        inspirationScoreTotalText.text = scoreTotal;
    }

    public void CloseInspirationPanel(int inspiration)
    {
        inspirationSheet.SetActive(false);

        GameManager.instance.currentInspiration = inspiration;
    }

    public void OpenEvaluationPanel(string generatedPoem, string classification, string scoreTotal)
    {
        impressionSheet.SetActive(true);

        impressionGeneratedPoemText.text = generatedPoem;
        impressionClassificationText.text = classification;
        impressionScoreText.text = scoreTotal;
    }

    public void CloseEvaluationPanel(int impression)
    {
        impressionSheet.SetActive(false);

        GameManager.instance.currentImpression = impression;
    }

    public void ShowEndScreen(bool hasWon)
    {
        if (hasWon) victoryScreen.SetActive(true);
        else gameOverScreen.SetActive(true);
    }

    public void Update()
    {
        currentRoundText.text = $"Round {GameManager.instance.currentRound + 1} de 6";
        currentCharacterNameText.text = $"{GameManager.instance.characterNames[GameManager.instance.currentRound]}";
        currentCharacterRuleText.text = $"{GameManager.instance.specialRules[GameManager.instance.currentRound]}";

        suggestionsLeftText.text = GameManager.instance.suggestions.ToString();
        manaLeftText.text = GameManager.instance.mana.ToString();

        insipirationScoreText.text = GameManager.instance.currentInspiration.ToString();
        impressionScoreText.text = GameManager.instance.currentImpression.ToString();
        currentScoreText.text = GameManager.instance.currentScore.ToString();
        targetScoreText.text = GameManager.instance.targetScore.ToString();
    }
}

