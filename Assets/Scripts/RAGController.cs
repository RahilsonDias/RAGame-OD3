using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;

public class RAGController : MonoBehaviour
{
    public static RAGController instance;
    public RespostaAPI dados;

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
        string apiUrl = "http://127.0.0.1:8000/evaluate_poem";

        //Chamada para UI
        UIManager.instance.OpenInspirationPanel("Gerando...", "Aguarde...", "Total: 0");

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
            //Debug.Log("Resposta: " + request.downloadHandler.text);
            string json = request.downloadHandler.text;
            dados = JsonUtility.FromJson<RespostaAPI>(json);
            UIManager.instance.OpenInspirationPanel(dados.geracao.poema_gerado, $"Presença: {dados.avaliacao.presenca}\n\nDistância: {dados.avaliacao.distancia}\n\n" +
                $"Potencial de Rima: {dados.avaliacao.potencial_rima}\n\nFigura de Som: {dados.avaliacao.figura_de_som}\n\nComplexidade Lexical: {dados.avaliacao.complexidade_lexical}",
                $"Total: {dados.avaliacao.total}");
        }
    }

    public void CallPostEvaluatePoem(string[] keywords, string currentRule)
    {
        UIManager.instance.CloseInspirationPanel(dados.avaliacao.total);

        StartCoroutine(PostEvaluatePoem(keywords, currentRule));
    }

    public IEnumerator PostEvaluatePoem(string[] keywords, string currentRule)
    {
        string apiUrl = "http://127.0.0.1:8000/calculate_final_score_with_rule";

        //Chamada para UI
        UIManager.instance.OpenEvaluationPanel("Enviando...", "Aguarde...", "Total: 0");

        // Monta o objeto com os dados
        PoemEvaluationRequest body = new PoemEvaluationRequest
        {
            poema_gerado = dados.geracao.poema_gerado,
            poema_base = dados.geracao.poema_base,
            autor = dados.geracao.autor,
            avaliacao_tecnica = dados.avaliacao.total,
            keywords = keywords,
            regra_especial = currentRule
        };

        // Serializa em JSON
        string jsonBody = JsonUtility.ToJson(body);

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
            string json = request.downloadHandler.text;

            // Agora desserializa para o retorno esperado
            RespostaImpressao dadosImpressao = JsonUtility.FromJson<RespostaImpressao>(json);

            UIManager.instance.OpenEvaluationPanel(
                $"{dados.geracao.poema_gerado}",
                $"Originalidade: {dadosImpressao.avaliacao_impressao.originalidade}\n\n" +
                $"Variedade Lexical: {dadosImpressao.avaliacao_impressao.variedade_lexical}\n\n" +
                $"Número de Versos: {dadosImpressao.avaliacao_impressao.numero_de_versos}\n\n" +
                $"Clareza Semântica: {dadosImpressao.avaliacao_impressao.clareza_semantica}\n\n" +
                $"Criatividade: {dadosImpressao.avaliacao_impressao.criatividade}\n\n" +
                $"Regra Respeitada: {dadosImpressao.regra_respeitada}\n\n",
                //$"Total Impressão: {dadosImpressao.total_impressao}\n",
                $"Total Final: {dadosImpressao.total_impressao}"
            );

            yield return new WaitForSeconds(10f);

            UIManager.instance.CloseEvaluationPanel(dadosImpressao.total_impressao);

            yield return new WaitForSeconds(2f);

            GameManager.instance.ProcessScore();
        }
    }


    // Classe auxiliar para serializar corretamente
    [System.Serializable]
    public class KeywordWrapper
    {
        public string[] keywords;
        public KeywordWrapper(string[] words) => keywords = words;
    }

    [System.Serializable]
    public class PoemEvaluationRequest
    {
        public string poema_gerado;
        public string poema_base;
        public string autor;
        public int avaliacao_tecnica;
        public string[] keywords;
        public string regra_especial;
    }


    // Helper para serializar arrays
    public static class JsonHelper
    {
        public static string ToJson<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }
    }

    // Helper para receber a resposta
    [System.Serializable]
    public class RespostaAPI
    {
        public Geracao geracao;
        public Avaliacao avaliacao;
    }

    [System.Serializable]
    public class Geracao
    {
        public string poema_gerado;
        public string poema_base;
        public string autor;
    }

    [System.Serializable]
    public class Avaliacao
    {
        public int presenca;
        public int distancia;
        public int potencial_rima;
        public int figura_de_som;
        public int complexidade_lexical;
        public int total;
    }


    [System.Serializable]
    public class RespostaImpressao
    {
        public AvaliacaoImpressao avaliacao_impressao;
        public bool regra_respeitada;
        public int total_impressao;
        public int total_final;
    }

    [System.Serializable]
    public class AvaliacaoImpressao
    {
        public int originalidade;
        public int variedade_lexical;
        public int numero_de_versos;
        public int clareza_semantica;
        public int criatividade;
    }

}
