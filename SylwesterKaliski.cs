using UnityEngine;
using UnityEngine.UI;

public class SylwesterKaliski : MonoBehaviour
{
    public GameObject displayPanel; // Panel do wyœwietlania grafiki i tekstu
    public Image kaliskiImage; // Obraz Sylwestra Kaliskiego
    public Text kaliskiText; // Tekst z informacjami o Sylwestrze Kaliskim
    public GameObject interactIcon; // Ikona E nad g³ow¹ gracza
    public Transform playerTransform; // Transform gracza
    public Vector3 interactIconOffset = new Vector3(0, 2, 0); // Przesuniêcie ikony E wzglêdem gracza

    private bool isPlayerNear = false;

    void Start()
    {
        if (displayPanel == null || kaliskiImage == null || kaliskiText == null || interactIcon == null || playerTransform == null)
        {
            Debug.LogError("One or more required components are not assigned in the inspector");
            return;
        }

        displayPanel.SetActive(false); // Ukryj panel na pocz¹tku
        interactIcon.SetActive(false); // Ukryj ikonê E na pocz¹tku
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
        kaliskiText.text = "Sylwester Kaliski by³ wybitnym polskim naukowcem, in¿ynierem i profesorem. Jest patronem naszej szko³y ze wzglêdu na jego znacz¹cy wk³ad w rozwój technologii i edukacji.";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            interactIcon.SetActive(true); // Poka¿ ikonê E
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            displayPanel.SetActive(false);
            interactIcon.SetActive(false); // Ukryj ikonê E
        }
    }
}