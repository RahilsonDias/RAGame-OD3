using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CorpusManager : MonoBehaviour
{
    private List<string> verses = new List<string>()
    {
        "A lua brilha na rua fria",
        "Noite escura, a lua guia",
        "Na montanha clara a lua flutua",
        "Sob a rua a luz da lua",
        "O vento canta e a lua continua"
    };

    // Simples busca por rima (checa final da palavra)
    public string GetVerseWithRhyme(string targetWord)
    {
        return verses.FirstOrDefault(v => v.ToLower().EndsWith(targetWord.ToLower()))
               ?? "Nenhum verso encontrado no corpus.";
    }
}

