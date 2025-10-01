using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class RAGController : MonoBehaviour
{
    public static RAGController instance;

    private string apiUrl = "http://127.0.0.1:8000/generate_poem";

    // Exemplo de chamada
    private void Awake()
    {
        instance = this;
    }

    public void CallPostGeneratePoem(string[] palavras)
    {
        StartCoroutine(PostGeneratePoem(palavras));
    }

    public IEnumerator PostGeneratePoem(string[] keywords)
    {
        // Monta o JSON manualmente
        string jsonBody = JsonHelper.ToJson(new KeywordWrapper(keywords));

        // Converte para bytes
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        // Cria a requisição
        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Envia
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Erro: " + request.error);
        }
        else
        {
            Debug.Log("Resposta: " + request.downloadHandler.text);
        }
    }

    // Classe auxiliar para serializar corretamente
    [System.Serializable]
    public class KeywordWrapper
    {
        public string[] keywords;
        public KeywordWrapper(string[] words) => keywords = words;
    }

    // Helper para serializar arrays
    public static class JsonHelper
    {
        public static string ToJson<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }
}
