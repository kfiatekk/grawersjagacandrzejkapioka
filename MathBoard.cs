using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class MathBoard : MonoBehaviour
{
    public GameObject mathPanel; // Panel z zadaniem matematycznym
    public GameObject thankYouPanel; // Panel z komunikatem "Dziękuję"
    public GameObject interactIcon; // Ikona E nad głową gracza
    public Transform playerTransform; // Transform gracza
    public Vector3 interactIconOffset = new Vector3(0, 2, 0); // Przesunięcie ikony E względem gracza
    public Text questionText;
    public InputField answerInput;
    public Text feedbackText;
    public Button submitButton;
    public Text completionText; // Tekst wyświetlany po ukończeniu
    public Text thankYouText; // Tekst wyświetlany w thankYouPanel

    private double correctAnswer;
    private bool isPlayerNear = false;
    private int problemsSolved = 0;
    private const int totalProblems = 3;
    private readonly int[] sqrtNumbers = { 4, 36, 100, 64, 256, 9, 81, 144, 400, 49, 169, 225};
    private bool gameCompleted = false;

    void Start()
    {
        if (mathPanel == null || thankYouPanel == null || interactIcon == null ||
            playerTransform == null || questionText == null || answerInput == null ||
            feedbackText == null || submitButton == null || completionText == null ||
            thankYouText == null)
        {
            Debug.LogError("One or more required components are not assigned in the inspector");
            return;
        }

        mathPanel.SetActive(false);
        completionText.gameObject.SetActive(false); // Ukryj tekst ukończenia
        thankYouPanel.SetActive(false); // Ukryj panel z komunikatem "Dziękuję"
        interactIcon.SetActive(false); // Ukryj ikonę E
        submitButton.onClick.AddListener(CheckAnswer); // Dodaj listener do przycisku

        // Ustaw ikonę E na warstwę UI
        interactIcon.layer = LayerMask.NameToLayer("UI");
    }

    void Update()
    {
        if (isPlayerNear)
        {
            if (playerTransform != null)
            {
                interactIcon.transform.position = playerTransform.position + interactIconOffset;
                interactIcon.SetActive(true);
                Debug.Log("Interact icon position: " + interactIcon.transform.position);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (gameCompleted)
                {
                    ShowThankYouMessage();
                }
                else if (mathPanel.activeSelf)
                {
                    CheckAnswer();
                }
                else
                {
                    ShowMathProblem();
                }
            }
        }
        else
        {
            interactIcon.SetActive(false);
        }
    }

    void ShowMathProblem()
    {
        if (gameCompleted)
        {
            ShowThankYouMessage();
            return;
        }

        if (problemsSolved >= totalProblems)
        {
            StartCoroutine(DisplayCompletionMessage());
            return;
        }

        mathPanel.SetActive(true);

        // Generate a random problem type
        int problemType = UnityEngine.Random.Range(0, 3);
        int a = UnityEngine.Random.Range(1, 11);
        int b = UnityEngine.Random.Range(2, 5);
        int sqrtIndex = UnityEngine.Random.Range(0, sqrtNumbers.Length);

        switch (problemType)
        {
            case 0: // Addition
                correctAnswer = a + b;
                questionText.text = $"Ile to {a} + {b}?";
                break;
            case 1: // Power
                correctAnswer = Math.Pow(a, b);
                questionText.text = $"Ile to {a}{GetSuperscript(b)}?";
                break;
            case 2: // Square Root
                correctAnswer = Math.Sqrt(sqrtNumbers[sqrtIndex]);
                questionText.text = $"Ile to √{sqrtNumbers[sqrtIndex]}?";
                break;
        }

        feedbackText.text = "";
        answerInput.text = ""; // Wyczyść pole odpowiedzi
        answerInput.ActivateInputField(); // Aktywuj pole odpowiedzi
    }

    string GetSuperscript(int number)
    {
        string superscript = "";
        foreach (char c in number.ToString())
        {
            switch (c)
            {
                case '0': superscript += '⁰'; break;
                case '1': superscript += '¹'; break;
                case '2': superscript += '²'; break;
                case '3': superscript += '³'; break;
                case '4': superscript += '⁴'; break;
                case '5': superscript += '⁵'; break;
                case '6': superscript += '⁶'; break;
                case '7': superscript += '⁷'; break;
                case '8': superscript += '⁸'; break;
                case '9': superscript += '⁹'; break;
            }
        }
        return superscript;
    }

    public void CheckAnswer()
    {
        if (double.TryParse(answerInput.text, out double playerAnswer))
        {
            if (Math.Abs(playerAnswer - correctAnswer) < 0.001)
            {
                feedbackText.text = "Dobrze!";
                problemsSolved++;
                if (problemsSolved >= totalProblems)
                {
                    StartCoroutine(DisplayCompletionMessage());
                }
                else
                {
                    ShowMathProblem(); // Pokaż następne zadanie
                }
            }
            else
            {
                feedbackText.text = "Źle, spróbuj ponownie.";
            }
        }
        else
        {
            feedbackText.text = "Podaj liczbę!";
        }
    }

    private IEnumerator DisplayCompletionMessage()
    {
        completionText.gameObject.SetActive(true);
        completionText.text = "Brawo! Rozwiązałeś wszystkie zadania!";
        mathPanel.SetActive(true); // Spraw, aby panel pozostał widoczny
        yield return new WaitForSeconds(3); // Wyświetl przez 3 sekundy
        mathPanel.SetActive(false);
        completionText.gameObject.SetActive(false);
        ShowThankYouMessage();
        gameCompleted = true;
    }

    private void ShowThankYouMessage()
    {
        thankYouPanel.SetActive(true);
        thankYouText.text = "Dziękuję, ale już nie potrzebuję pomocy.";
        interactIcon.SetActive(false); // Ukryj ikonę E
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactIcon.SetActive(true); // Pokaż ikonę E
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            mathPanel.SetActive(false);
            thankYouPanel.SetActive(false);
            completionText.gameObject.SetActive(false);
            interactIcon.SetActive(false); // Ukryj ikonę E
            if (gameCompleted)
            {
                interactIcon.SetActive(false); // Ukryj ikonę E
            }
        }
    }
}