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

    public void ProcessScore()
    {
        var score = currentImpression + currentInspiration;
        currentScore += score;

        currentInspiration = 0;
        currentImpression = 0;

        if (currentScore >= targetScore)
        {
            currentRound++;
            
            if(currentRound > targetScores.Count)
            {
                // Gameover Win
                UIManager.instance.ShowEndScreen(true);
            }
            
            StartRound();
        }
        else if(suggestions <= 0)
        {
            // Gameover Loss
            UIManager.instance.ShowEndScreen(false);
        }
    }
}

