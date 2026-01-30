using UnityEngine;

public class OrientationManager : MonoBehaviour
{
    // Singleton (Opsiyonel, erişim kolaylığı için kalsın)
    public static OrientationManager Instance;

    private bool _lastIsPortrait;
    private bool _firstRun = true;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        // Oyun açıldığında mevcut durumu kontrol et ve uygula
        CheckOrientationAndRefresh();
    }

    void Update()
    {
        // Her karede ekran oranını kontrol ediyoruz.
        // Eğer oran değişirse UI'ı güncelleyeceğiz.
        CheckOrientationAndRefresh();
    }

    private void CheckOrientationAndRefresh()
    {
        // Ekranın yüksekliği genişliğinden büyükse DİKEY (Portrait) moddayız demektir.
        bool currentIsPortrait = Screen.height > Screen.width;

        // Eğer mod değiştiyse VEYA oyun yeni açıldıysa (_firstRun)
        if (currentIsPortrait != _lastIsPortrait || _firstRun)
        {
            _lastIsPortrait = currentIsPortrait;
            _firstRun = false;

            // Değişikliği uygula
            UpdateAllPanels(currentIsPortrait);

            Debug.Log("Ekran Döndü! Yeni Mod: " + (currentIsPortrait ? "Dikey (Portrait)" : "Yatay (Landscape)"));
        }
    }

    // Sahnedeki akıllı objeleri bulup günceller
    void UpdateAllPanels(bool isPortrait)
    {
        // Sahnedeki ResponsiveTransform scriptine sahip tüm objeleri bulur (Gizliler dahil)
        ResponsiveTransform[] allUI = FindObjectsByType<ResponsiveTransform>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var uiElement in allUI)
        {
            uiElement.ApplyOrientation(isPortrait);
        }
    }
}