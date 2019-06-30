using MultiLanguage;
using UnityEngine;
using UnityEngine.UI;

public class LanguageTest : MonoBehaviour
{
    [SerializeField] private Transform languagesTransform;
    [SerializeField] private GameObject languageButton;
    private void Start()
    {
        var languages = MultiLanguageManager.GetLanguages();
        foreach (var language in languages.LanguageList)
        {
            var languageObject = Instantiate(languageButton, languagesTransform);
            languageObject.SetActive(true);
            languageObject.GetComponentInChildren<Text>().text = language.LanguageString;
            languageObject.GetComponent<Button>().onClick.AddListener(() =>
                {
                    MultiLanguageManager.CurrentLanguage = language.LanguageCode;
                });
        }
    }
}
