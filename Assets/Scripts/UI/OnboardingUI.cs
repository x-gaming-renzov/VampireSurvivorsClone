using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vampire;

public class OnboardingUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_Dropdown countryDropdown;
    [SerializeField] private Button continueButton;

    [Header("Roots")]
    [SerializeField] private GameObject onboardingRoot;  // OnboardingCanvas or Panel
    [SerializeField] private GameObject mainMenuRoot;     // Your existing "Main Menu" root

    private const string KeyName = "profile.name";
    private const string KeyCountry = "profile.country";

    private List<string> _countries;

    private void Awake()
    {
        // Populate country list (English names, sorted)
        _countries = CultureInfo
            .GetCultures(CultureTypes.SpecificCultures)
            .Select(c => new RegionInfo(c.LCID).EnglishName)
            .Distinct()
            .OrderBy(n => n)
            .ToList();

        // Fallback minimal list if something goes wrong
        if (_countries.Count == 0)
            _countries = new List<string> { "India", "United States", "Canada", "United Kingdom" };

        countryDropdown.ClearOptions();
        countryDropdown.AddOptions(_countries);

        // Load previously saved profile
        var hasName = PlayerPrefs.HasKey(KeyName);
        var hasCountry = PlayerPrefs.HasKey(KeyCountry);

        if (hasName) nameInput.text = PlayerPrefs.GetString(KeyName);
        if (hasCountry)
        {
            var saved = PlayerPrefs.GetString(KeyCountry);
            var idx = _countries.FindIndex(x => x == saved);
            if (idx >= 0) countryDropdown.value = idx;
        }

        // Show/hide screens
        bool hasProfile = hasName && hasCountry && !string.IsNullOrWhiteSpace(PlayerPrefs.GetString(KeyName));
        onboardingRoot.SetActive(true);
        mainMenuRoot.SetActive(false);

        continueButton.onClick.AddListener(Submit);
    }

    public void Submit()
    {
        var playerName = nameInput.text.Trim();
        if (string.IsNullOrEmpty(playerName))
        {
            nameInput.ActivateInputField();
            return;
        }

        var country = countryDropdown.options[countryDropdown.value].text;

        PlayerPrefs.SetString(KeyName, playerName);
        PlayerPrefs.SetString(KeyCountry, country);
        PlayerPrefs.Save();

        // Initialize Nova SDK with user profile data
        Debug.Log($"🚀 Initializing Nova SDK for user: {playerName} from {country}");

        onboardingRoot.SetActive(false);
        mainMenuRoot.SetActive(true);
        NovaManager.InitializeNovaAfterOnboarding();
    }
}
