using UnityEngine;
using UnityEngine.UI;

public class ResponsiveTransform : MonoBehaviour
{
    [System.Serializable]
    public class LayoutSettings
    {
        public Vector2 anchorMin;
        public Vector2 anchorMax;
        public Vector2 anchoredPosition;
        public Vector2 sizeDelta;
        public Vector3 localScale = Vector3.one;
        public int gridConstraintCount = 2; // Grid kullanıyorsan sütun sayısı
    }

    // Editörden ayarları buraya kaydedeceğiz
    public LayoutSettings landscapeSettings; // Yatay
    public LayoutSettings portraitSettings;  // Dikey

    private RectTransform rect;
    private GridLayoutGroup grid;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
    }

    // OrientationManager tarafından otomatik çağrılır
    public void ApplyOrientation(bool isPortrait)
    {
        if (rect == null) rect = GetComponent<RectTransform>();
        if (grid == null) grid = GetComponent<GridLayoutGroup>();

        // Hangi ayarı yükleyelim?
        LayoutSettings target = isPortrait ? portraitSettings : landscapeSettings;

        // Ayarları uygula
        rect.anchorMin = target.anchorMin;
        rect.anchorMax = target.anchorMax;
        rect.anchoredPosition = target.anchoredPosition;
        rect.sizeDelta = target.sizeDelta;
        rect.localScale = target.localScale;

        if (grid != null)
        {
            grid.constraintCount = target.gridConstraintCount;
        }
    }

    // --- SAĞ TIK MENÜSÜ (EDİTÖR İÇİN) ---

    [ContextMenu("Şu anki hali YATAY (Landscape) olarak kaydet")]
    public void SaveAsLandscape()
    {
        rect = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        landscapeSettings = GetCurrentSettings();
        Debug.Log(gameObject.name + " için YATAY ayarlar kaydedildi.");
    }

    [ContextMenu("Şu anki hali DİKEY (Portrait) olarak kaydet")]
    public void SaveAsPortrait()
    {
        rect = GetComponent<RectTransform>();
        grid = GetComponent<GridLayoutGroup>();
        portraitSettings = GetCurrentSettings();
        Debug.Log(gameObject.name + " için DİKEY ayarlar kaydedildi.");
    }

    private LayoutSettings GetCurrentSettings()
    {
        LayoutSettings s = new LayoutSettings();
        s.anchorMin = rect.anchorMin;
        s.anchorMax = rect.anchorMax;
        s.anchoredPosition = rect.anchoredPosition;
        s.sizeDelta = rect.sizeDelta;
        s.localScale = rect.localScale;
        if (grid != null) s.gridConstraintCount = grid.constraintCount;
        return s;
    }
}