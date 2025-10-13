using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Linq;


[System.Serializable]
public class QuestionData {
    public string question;
    public Dictionary<string, string> choices;
    public string correct;
}

public class QuestionManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public List<Button> choiceButtons; // A-B-C
    public GameObject resultPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI scoreText;

    public GameObject correctAnim;
    public GameObject wrongAnim;
    public float feedbackDuration = 1.5f;

    private List<QuestionData> questions;
    private int currentIndex = 0;
    private int score = 0;

    private SoundEffectPlayer soundPlayer;
    private AudioSource winSound;

    void Start()
    {
        soundPlayer = FindObjectOfType<SoundEffectPlayer>();
        winSound = GameObject.FindWithTag("Victory")?.GetComponent<AudioSource>();
        StartCoroutine(LoadQuestions());
    }

 IEnumerator LoadQuestions()
{
    string path = Path.Combine(Application.streamingAssetsPath, "quiz_complete_abcd_shuffled.json");


#if UNITY_ANDROID && !UNITY_EDITOR
    UnityWebRequest www = UnityWebRequest.Get(path);
    yield return www.SendWebRequest();

    if (www.result == UnityWebRequest.Result.Success)
    {
        string json = www.downloadHandler.text;
        questions = JsonConvert.DeserializeObject<List<QuestionData>>(json);
        questions = questions.OrderBy(q => Random.value).ToList(); // ✅ Acak soal
        ShowQuestion();
        UpdateScore();
    }
    else
    {
        Debug.LogError("Gagal load soal di Android: " + www.error);
    }

    yield break; // ✅ agar semua jalur ada return
#else
    string json = File.ReadAllText(path);
    questions = JsonConvert.DeserializeObject<List<QuestionData>>(json);
    questions = questions.OrderBy(q => Random.value).ToList(); // ✅ Acak soal
    ShowQuestion();
    UpdateScore();
    yield break; // ✅ non-Android jalur juga aman
#endif
}



    void ShowQuestion()
    {
        if (currentIndex >= questions.Count)
        {
            ShowResult();
            return;
        }

        QuestionData q = questions[currentIndex];
        questionText.text = $"Soal {currentIndex + 1}: {q.question}";

        string[] keys = new List<string>(q.choices.Keys).ToArray();

        for (int i = 0; i < choiceButtons.Count; i++)
        {
            string choiceKey = keys[i];
            string choiceText = q.choices[choiceKey];

            var btn = choiceButtons[i];
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{choiceKey}. {choiceText}";
            btn.onClick.RemoveAllListeners();

            if (choiceKey == q.correct)
                btn.onClick.AddListener(() => AnswerCorrect());
            else
                btn.onClick.AddListener(() => AnswerWrong());
        }
    }

    void AnswerCorrect()
    {
        score += 10;
        UpdateScore();
        soundPlayer?.PlayCorrect();
        StartCoroutine(ShowFeedback(true));
    }

    void AnswerWrong()
    {
        soundPlayer?.PlayWrong();
        StartCoroutine(ShowFeedback(false));
    }

    IEnumerator ShowFeedback(bool isCorrect)
    {
        if (isCorrect && correctAnim != null)
            correctAnim.SetActive(true);
        else if (!isCorrect && wrongAnim != null)
            wrongAnim.SetActive(true);

        yield return new WaitForSeconds(feedbackDuration);

        if (correctAnim != null) correctAnim.SetActive(false);
        if (wrongAnim != null) wrongAnim.SetActive(false);

        Next();
    }

    void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = $"Score : {score} / {questions.Count * 10}";
    }

    void Next()
    {
        currentIndex++;
        ShowQuestion();
    }

    void ShowResult()
    {
        BackgroundMusic music = FindObjectOfType<BackgroundMusic>();
        music?.SetVolume(0.1f); // kecilkan saat result muncul

        if (winSound != null)
            winSound.Play();

        resultPanel.SetActive(true);
        resultText.text = $"Skor Akhir: {score} / {questions.Count * 10}";
    }
}
