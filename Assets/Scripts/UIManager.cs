using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TMP_InputField word1Input;
    public TMP_InputField word2Input;
    public TMP_InputField word3Input;
    public TextMeshProUGUI generatedPoemText;
    public TextMeshProUGUI corpusVerseText;
    public TextMeshProUGUI scoreText;

    public GameManager gameManager;

    public void OnSubmitButtonClicked()
    {
        string[] words = {
            word1Input.text.Trim(),
            word2Input.text.Trim(),
            word3Input.text.Trim()
        };

        gameManager.OnPlayerSubmitWords(words);
    }

    public void DisplayGeneratedPoem(string poem)
    {
        generatedPoemText.text = "Poema gerado:\n" + poem;
    }

    public void DisplayCorpusVerse(string verse)
    {
        corpusVerseText.text = "Verso encontrado:\n" + verse;
    }

    public void UpdateScore(int score)
    {
        scoreText.text = "Pontuação: " + score;
    }
}

