# Simple Responsive UI

Dealing with UI when a mobile game swaps between portrait and landscape has always been a headache. I got pretty tired of constantly writing scripts just to recalculate UI positions every time the screen flips, so I put together **Simple Responsive UI**. 

It takes a completely visual approach. Instead of messing with code, you just arrange your UI for portrait and landscape right in the Editor and save a "snapshot" for each.

**Developed by:** Talha Doğan

---

## How it Works

Instead of doing math in your scripts, you just use the Unity Editor:

1. Set up your layout (Buttons, Panels, Grids, etc.) for Landscape mode.
2. Right-click the UI component and hit `Save as Landscape`.
3. Flip your Game View over to Portrait mode.
4. Move and resize your UI elements so they fit nicely.
5. Right-click again and hit `Save as Portrait`.

That's really it. The `Orientation Manager` handles the rest. It catches whenever the device rotates and snaps the right layout into place. It even works while you're in Edit Mode, so you don't have to constantly build to test it.

---

## Why Use It?

* **Visual Design:** You handle everything right on the Canvas. No need to write a single line of code to calculate positions.
* **Instant Preview:** The moment you resize your Game window, the UI adapts on the fly.
* **Native Fit:** Works out of the box with Unity's built-in `RectTransform` and `GridLayoutGroup` components.
* **Clean Foundation:** Wrapped in a namespace so it won't clash with your existing codebase, and optimized to keep things running smooth.

---

## Key Features

* **Dual Layout Saving:** Stores independent Anchor, Position, Size, and Scale values for both orientations.
* **Canvas Scaler Sync:** Automatically tweaks the "Match Width/Height" value so your UI doesn't scale weirdly when rotating.
* **Dynamic Grids:** Need 4 columns in landscape but only 2 in portrait? It picks up on `GridLayoutGroup` changes and applies them dynamically.
* **Quick Menus:** Everything is just a right-click away in the Editor for a fast workflow.

---

## Requirements

* Unity 2020.3 or newer.
* Standard Unity UI (uGUI).

---

## What's in the Box?

* `OrientationManager.cs`: Drop this into your scene to keep an eye on screen changes in the background.
* `ResponsiveTransform.cs`: The main component you'll attach to the UI elements you want to control.
* A demo scene and quick docs to get you up and running immediately.

---

## Screenshots & Quick Setup

Here is a look at how the system adapts the interface:

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

You don't actually need to write any code to use this tool, but if you're wondering how the architecture kicks off in the background, here's a quick peek:

```csharp
using UnityEngine;
using SimpleResponsiveUI;

public class UIManager : MonoBehaviour
{
    // Make sure you have OrientationManager in your active scene.
    // It automatically listens for screen rotation events.
    
    void Start()
    {
        // The OrientationManager reads the initial device orientation on startup
        // and instantly applies the correct layout to your UI.
    }
}
