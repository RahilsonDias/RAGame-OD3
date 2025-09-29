using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text.RegularExpressions;

public class ScoreManager : MonoBehaviour
{
    public int CalculateScore(string[] words, string poem)
    {
        int baseScore = 0;

        // +10 pontos se palavra aparece no poema
        foreach (var word in words)
        {
            if (poem.ToLower().Contains(word.ToLower()))
                baseScore += 10;
        }

        // Verifica se há rima (comparando finais das palavras)
        string[] poemLines = poem.Split('\n');
        if (poemLines.Length >= 2)
        {
            string lastWord1 = GetLastWord(poemLines[0]);
            string lastWord2 = GetLastWord(poemLines[1]);

            if (lastWord1.EndsWith(lastWord2) || lastWord2.EndsWith(lastWord1))
                baseScore += 20; // bônus de rima
        }

        // Multiplicadores (exemplo simples: se todas palavras começam com mesma letra = aliteração)
        bool allSameInitial = words.All(w => !string.IsNullOrEmpty(w) && w[0] == words[0][0]);
        int multiplier = allSameInitial ? 2 : 1;

        return baseScore * multiplier;
    }

    private string GetLastWord(string line)
    {
        var words = Regex.Split(line.Trim(), @"\W+");
        return words.Length > 0 ? words[words.Length - 1] : "";
    }
}

