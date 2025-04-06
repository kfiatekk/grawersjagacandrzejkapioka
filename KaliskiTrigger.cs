using UnityEngine;
using UnityEngine.UI;

public class KaliskiTrigger : MonoBehaviour
{
    public GameObject displayPanel; // Panel do wyswietlania grafiki i tekstu
    public Image kaliskiImage; // Obraz Sylwestra Kaliskiego
    public Text kaliskiHeaderText; // Naglowek z nazwiskiem Sylwestra Kaliskiego
    public Text kaliskiDescriptionText; // Tekst z informacjami o Sylwestrze Kaliskim
    public GameObject interactIcon; // Ikona E nad glowa gracza
    public Transform playerTransform; // Transform gracza
    public Vector3 interactIconOffset = new Vector3(0, 2, 0); // Przesuniecie ikony E wzgledem gracza

    private bool isPlayerNear = false;

    void Start()
    {
        Debug.Log("Start method called");

        if (displayPanel == null) Debug.LogError("Display Panel is not assigned in the inspector");
        if (kaliskiImage == null) Debug.LogError("Kaliski Image is not assigned in the inspector");
        if (kaliskiHeaderText == null) Debug.LogError("Kaliski Header Text is not assigned in the inspector");
        if (kaliskiDescriptionText == null) Debug.LogError("Kaliski Description Text is not assigned in the inspector");
        if (interactIcon == null) Debug.LogError("Interact Icon is not assigned in the inspector");
        if (playerTransform == null) Debug.LogError("Player Transform is not assigned in the inspector");

        displayPanel?.SetActive(false); // Ukryj panel na poczatku
        interactIcon?.SetActive(false); // Ukryj ikone E na poczatku
    }

    void Update()
    {
        if (isPlayerNear)
        {
            if (playerTransform != null)
            {
                interactIcon.transform.position = playerTransform.position + interactIconOffset;
                interactIcon.SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                ShowSylwesterKaliskiInfo();
            }
        }
        else
        {
            interactIcon.SetActive(false);
        }
    }

    void ShowSylwesterKaliskiInfo()
    {
        displayPanel.SetActive(true);
        kaliskiHeaderText.text = "SYLWESTER KALISKI";
        kaliskiDescriptionText.text =
            "<b>1. Wybitny fizyk i organizator nauki</b>\n" +
            "<i>Profesor zwyczajny, czlonek rzeczywisty PAN</i>\n" +
            "Autor ponad 550 publikacji naukowych.\n" +
            "Tworca Instytutu Fizyki Plazmy i kierunku fizyka techniczna.\n\n" +
            "<b>2. Pionierskie osiagniecia naukowe</b>\n" +
            "<i>Badania nad mikrosynteza termojadrowa z uzyciem laserow</i>\n" +
            "Tworca teorii wzmacniania ultradzwiekow i podstaw fononiki.\n" +
            "Wynalazca urzadzenia „faser”.\n\n" +
            "<b>3. Dzialacz publiczny i wojskowy</b>\n" +
            "<i>Komendant WAT (1967–1974)</i>\n" +
            "Minister Nauki, Szkolnictwa Wyzszego i Techniki (1974–1978).\n" +
            "Autor prognoz technicznego rozwoju sil zbrojnych PRL, posel na Sejm.";

        Debug.Log("Header Text: " + kaliskiHeaderText.text);
        Debug.Log("Description Text: " + kaliskiDescriptionText.text);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactIcon.SetActive(true); // Pokaz ikone E
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            displayPanel.SetActive(false);
            interactIcon.SetActive(false); // Ukryj ikone E
        }
    }
}