using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Esse script é o ponto de integração com IA Externa (RAG + LLM)
public class PoemGenerator : MonoBehaviour
{
    public void GeneratePoem(string[] words, Action<string> callback)
    {
        // MOCK: geração simples (substitua por chamada à API de IA)
        string mockPoem = $"Na calma da rua,\nbrilha a clara {words[2]},\nmemória que flutua.";

        // Simula atraso da IA
        StartCoroutine(ReturnPoemAfterDelay(mockPoem, callback));
    }

    private IEnumerator ReturnPoemAfterDelay(string poem, Action<string> callback)
    {
        yield return new WaitForSeconds(1f);
        callback(poem);
    }
}

