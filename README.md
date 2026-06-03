# Simple Responsive UI

Handling UI transitions between portrait and landscape modes in mobile games can be incredibly frustrating. Instead of writing custom scripts to recalculate positions and scales every time the screen rotates, I built **Simple Responsive UI** to handle it visually.

It is a lightweight, zero-code solution: you just arrange your UI in the Unity Editor for both orientations and save a "snapshot" for each. The system handles the rest automatically.

**Developed by:** Talha Doğan

---

## 🚀 Features

* **Visual Workflow (WYSIWYG):** Design directly on the Canvas. No need to calculate anchor points, offsets, or sizes in code.
* **Live Editor Preview:** Powered by `[ExecuteAlways]`, your UI adapts instantly when you resize the Game window—even in Edit Mode.
* **Deep Grid Support:** Automatically updates `GridLayoutGroup` constraints, cell sizes, spacing, alignment, start corners, and axes based on the screen orientation.
* **Camera FOV Adaptation:** Includes a `ResponsiveCamera` component to dynamically adjust your Camera's Field of View when the device rotates.
* **Canvas Scaler Sync:** Automatically toggles the "Match Width/Height" property to ensure your overall UI scales correctly without distortion.
* **Clean & Performant:** Protected by the `SimpleResponsiveUI` namespace, uses smart caching, and only triggers updates when an actual orientation change is detected.

---

## ⚙️ How It Works

You don't need to write any math or UI logic. Just follow this simple workflow in the Editor:

1. Set up your UI elements (Buttons, Panels, Grids) for **Landscape** mode.
2. Add the `ResponsiveTransform` component to your UI element, right-click the component header, and hit `Save Current As LANDSCAPE`.
3. Switch your Game View to **Portrait** mode.
4. Move and resize your UI elements to fit the portrait screen.
5. Right-click the component again and hit `Save Current As PORTRAIT`.

Once set up, the `OrientationManager` automatically monitors the screen dimensions (`Screen.height > Screen.width`) and snaps to the correct saved layout.

---

## 📌 Requirements

* Unity 2020.3 or newer.
* Standard Unity UI (uGUI).

---

## 📦 What's Included

* `OrientationManager.cs`: A persistent singleton that monitors screen orientation and gracefully handles scene transitions.
* `ResponsiveTransform.cs`: The core component you attach to UI elements to save and apply `RectTransform`, `GridLayoutGroup`, and `Sprite` data.
* `ResponsiveCamera.cs`: An optional component to handle FOV shifts for your Main Camera.

---

## 🖼️ Preview & Setup

Here is a look at how the system adapts the interface dynamically:

<table>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/9c43712a-bf21-48a6-abb6-84d01e22f963" alt="1" width="100%"></td>
    <td><img src="https://github.com/user-attachments/assets/641208c0-552b-4880-8398-0a43bba84bdc" alt="4v1 (2)" width="100%"></td>
  </tr>
  <tr>
    <td><img src="https://github.com/user-attachments/assets/e1b4fedf-e222-4488-bb3d-8745522b60ec" alt="4v1 (1)" width="100%"></td>
    <td><img src="https://github.com/user-attachments/assets/a561cba3-7a99-45ac-b359-fa368ba8d813" alt="2" width="100%"></td>
  </tr>
</table>

### Quick Setup Guide

Because the system is designed to be plug-and-play, you don't need to write initialization scripts. Setting it up takes seconds:

1. Add the `OrientationManager` to an empty GameObject in your initial scene.
2. Attach the `ResponsiveTransform` to the UI elements you want to control.
3. Use the Editor Context Menus (Right-Click) to save your layouts.
4. *(Optional)* Drop `ResponsiveCamera` onto your Main Camera to manage FOV.

If you ever need to interact with the system via code (for example, generating UI at runtime), the API is straightforward:

```csharp
using UnityEngine;
using SimpleResponsiveUI;

public class UIGenerator : MonoBehaviour
{
    public ResponsiveTransform myDynamicPanel;

    void Start()
    {
        // The OrientationManager runs automatically via [ExecuteAlways].
        // However, if you are building UI dynamically via code, you can trigger 
        // layout saves manually just like the Editor context menus do:
        
        // myDynamicPanel.SaveAsLandscape();
        // myDynamicPanel.SaveAsPortrait();
        
        // You can also force a specific orientation check on a specific element:
        bool isPortrait = Screen.height > Screen.width;
        myDynamicPanel.ApplyOrientation(isPortrait);
    }
}
