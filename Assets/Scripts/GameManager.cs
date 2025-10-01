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

    public int currentInspiration;
    public int currentImpression;
    public int currentScore;
    public int targetScore;
    public int suggestions;
    public int maxSuggestions;
    public int mana;
    public int maxMana;

    public int currentRound;

    public List<string> characterNames;
    public List<string> specialRules;
    public List<int> targetScores;
    public List<GameObject> characters;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartRound();
    }

    public void StartRound()
    {
        targetScore = targetScores[currentRound];
        currentScore = 0;
        suggestions = maxSuggestions;
        mana = maxMana;

        if(currentRound > 0) characters[currentRound - 1].SetActive(false);
        
        characters[currentRound].SetActive(true);
    }

    public void ProcessScore(int score)
    {
        currentScore += score;

        if(currentScore >= targetScore)
        {
            currentRound++;
            
            if(currentRound > targetScores.Count)
            {
                // Gameover
            }
            
            StartRound();
        }
    }

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

