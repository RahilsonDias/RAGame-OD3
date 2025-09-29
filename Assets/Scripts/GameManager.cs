using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public UIManager uiManager;
    public CorpusManager corpusManager;
    public PoemGenerator poemGenerator;
    public ScoreManager scoreManager;

    private string[] playerWords;

    public void OnPlayerSubmitWords(string[] words)
    {
        playerWords = words;

        // Busca no corpus (exemplo: sempre rimando com a última palavra)
        string targetWord = words[words.Length - 1];
        string verseFromCorpus = corpusManager.GetVerseWithRhyme(targetWord);

        // Gera poema com IA
        poemGenerator.GeneratePoem(words, (generatedPoem) =>
        {
            // Mostra no UI
            uiManager.DisplayGeneratedPoem(generatedPoem);
            uiManager.DisplayCorpusVerse(verseFromCorpus);

            // Calcula pontuação
            int score = scoreManager.CalculateScore(words, generatedPoem);
            uiManager.UpdateScore(score);
        });
    }
}

