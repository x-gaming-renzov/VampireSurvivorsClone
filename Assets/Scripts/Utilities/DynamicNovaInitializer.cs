using UnityEngine;
using Nova.SDK;
using Vampire;

public class DynamicNovaInitializer : MonoBehaviour
{
    private const string QueryParamKey = "sdkkey";

    void Awake()
    {
        Debug.Log("DynamicNovaInitializer Awake");
        Debug.Log("Application.absoluteURL: " + Application.absoluteURL);

#if UNITY_WEBGL && !UNITY_EDITOR
        string sdkKey = GetSdkKeyFromUrl();
        if (!string.IsNullOrEmpty(sdkKey))
        {
            var novaSettings = Resources.Load<NovaSettings>("NovaSettings");
            if (novaSettings != null)
            {
                novaSettings.SdkKey = sdkKey;
                Debug.Log("NovaSettings updated with dynamic SdkKey from URL");
                SubscribeToNovaEvents();
            }
            else
            {
                Debug.LogError("NovaSettings asset not found in Resources.");
            }
        }
        else
        {
            Debug.LogWarning($"No '{QueryParamKey}' query parameter found in URL.");
        }
#endif
    }

    private string GetSdkKeyFromUrl()
    {
        try
        {
            var url = Application.absoluteURL;
            if (string.IsNullOrEmpty(url)) return null;
            var uri = new System.Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query[QueryParamKey];
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Failed to parse sdk key from URL: " + e.Message);
            return null;
        }
    }

    private void SubscribeToNovaEvents()
    {
        if (NovaSDK.Instance != null && NovaSDK.Instance.IsInitialized)
        {
            NovaManager.ReloadConfiguration();
        }
        else
        {
            NovaManager.OnNovaInitialized += OnNovaInitialized;
        }
    }

    private void OnNovaInitialized()
    {
        NovaManager.ReloadConfiguration();
        NovaManager.OnNovaInitialized -= OnNovaInitialized;
    }

    private void OnDestroy()
    {
        NovaManager.OnNovaInitialized -= OnNovaInitialized;
    }
}