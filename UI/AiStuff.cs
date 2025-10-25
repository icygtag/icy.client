// DevTools.cs
// Safe developer helper methods for local/offline use only.
// WARNING: Do not enable these in public multiplayer matches.
// Set RequireOffline = false ONLY if you absolutely understand the consequences.

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public static class DevTools
{
    // --- Safety toggle: defaults to true to prevent accidental online use ---
    // If you set this to false you accept responsibility for using the tools.
    public static bool RequireOffline = false;

    // Replace this check with a proper network-check if your mod has access to Photon/Network APIs.
    private static bool IsOfflineSafe()
    {
        if (!RequireOffline) return true;
        // Best-effort check — if you have access to Photon/Networking, check the real network state.
        // For now we refuse unless RequireOffline is explicitly set false.
        Debug.Log("[DevTools] RequireOffline is ON. To run anyway set DevTools.RequireOffline = false (not recommended).");
        return false;
    }

    // 1) Simple FPS counter overlay (local debug only)
    public static void ToggleFPSCounter()
    {
        if (!IsOfflineSafe()) return;

        var existing = GameObject.Find("DevTools_FPSCounter");
        if (existing != null)
        {
            UnityEngine.Object.Destroy(existing);
            Debug.Log("[DevTools] FPS counter removed.");
            return;
        }

        var go = new GameObject("DevTools_FPSCounter");
        GameObject.DontDestroyOnLoad(go);
        var comp = go.AddComponent<FPSCounterBehaviour>();
        Debug.Log("[DevTools] FPS counter created.");
    }

    // 2) Dump current scene object summary to console (safe)
    public static void DumpSceneObjects()
    {
        if (!IsOfflineSafe()) return;

        var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
        Debug.Log($"[DevTools] Dumping {rootObjects.Length} root objects:");
        foreach (var obj in rootObjects)
        {
            Debug.Log($"- {obj.name} (active: {obj.activeInHierarchy}) children: {obj.transform.childCount}");
        }
    }

    // 3) Spawn a ragdoll-like testing dummy (primitive) at player's position for physics testing
    //    NOTE: purely local GameObject without network ownership; safe for testing in private.
    public static void SpawnPhysicsDummy()
    {
        if (!IsOfflineSafe()) return;

        var player = GameObject.FindWithTag("Player"); // adjust tag lookup to your mod's player object
        Vector3 spawnPos = (player != null) ? player.transform.position + player.transform.forward * 1.5f + Vector3.up * 1.2f : Vector3.zero;

        var go = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        go.name = "DevTools_PhysicsDummy";
        go.transform.position = spawnPos;
        go.transform.localScale = new Vector3(0.6f, 1.5f, 0.6f);
        var rb = go.AddComponent<Rigidbody>();
        rb.mass = 10f;
        rb.angularDrag = 1f;
        rb.drag = 0.5f;

        // simple auto-destroy after 60s so you don't clutter the scene
        UnityEngine.Object.Destroy(go, 60f);
        Debug.Log("[DevTools] Physics dummy spawned at " + spawnPos);
    }

    // 4) Toggle an on-screen dev console that captures Debug.Log messages
    public static void ToggleDevConsole()
    {
        if (!IsOfflineSafe()) return;

        var existing = GameObject.Find("DevTools_Console");
        if (existing != null)
        {
            UnityEngine.Object.Destroy(existing);
            Application.logMessageReceived -= DevConsoleBehaviour.OnLogReceivedStatic;
            Debug.Log("[DevTools] Dev console removed.");
            return;
        }

        var go = new GameObject("DevTools_Console");
        GameObject.DontDestroyOnLoad(go);
        go.AddComponent<DevConsoleBehaviour>();
        Application.logMessageReceived += DevConsoleBehaviour.OnLogReceivedStatic;
        Debug.Log("[DevTools] Dev console created. Press backquote ` to toggle console visibility (if implemented).");
    }

    // 5) Cycle sky/background tint for quick visual polish testing
    public static void CycleSkyTint()
    {
        if (!IsOfflineSafe()) return;

        var current = RenderSettings.ambientLight;
        // rotate hue in a simple way
        Color newColor = Color.HSVToRGB((UnityEngine.Random.value), 0.6f, 1f);
        RenderSettings.ambientLight = newColor;
        Camera.main.backgroundColor = newColor * 0.6f;
        Debug.Log($"[DevTools] Sky tint cycled to {newColor}");
    }

    // 6) Spawn a local practice target (harmless geometry only) for aim/throw testing
    public static void SpawnPracticeTarget()
    {
        if (!IsOfflineSafe()) return;

        var player = GameObject.FindWithTag("Player");
        Vector3 pos = (player != null) ? player.transform.position + player.transform.forward * 3f + Vector3.up * 1f : Vector3.zero;

        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = pos;
        go.transform.localScale = Vector3.one * 0.5f;
        go.name = "DevTools_PracticeTarget";
        var col = go.GetComponent<Collider>();
        if (col != null) col.isTrigger = true;

        var script = go.AddComponent<PracticeTargetBehaviour>();
        UnityEngine.Object.Destroy(go, 120f); // auto-clean

        Debug.Log("[DevTools] Practice target spawned at " + pos);
    }

    // 7) Toggle an on-screen performance telemetry panel (FPS, allocs approximate)
    public static void TogglePerfPanel()
    {
        if (!IsOfflineSafe()) return;

        var existing = GameObject.Find("DevTools_PerfPanel");
        if (existing != null)
        {
            UnityEngine.Object.Destroy(existing);
            Debug.Log("[DevTools] Perf panel removed.");
            return;
        }

        var go = new GameObject("DevTools_PerfPanel");
        GameObject.DontDestroyOnLoad(go);
        go.AddComponent<PerfPanelBehaviour>();
        Debug.Log("[DevTools] Perf panel created.");
    }

    // --- Helper MonoBehaviours used above ---
    // Place these MonoBehaviour classes inside the same file or another file in your mod project.

    private class FPSCounterBehaviour : MonoBehaviour
    {
        float delta = 0f;
        GUIStyle style;
        void Awake()
        {
            style = new GUIStyle();
            style.fontSize = 16;
            style.normal.textColor = Color.white;
        }
        void Update()
        {
            delta += (Time.unscaledDeltaTime - delta) * 0.1f;
        }
        void OnGUI()
        {
            int fps = (int)(1f / delta);
            GUI.Label(new Rect(10, 10, 200, 30), $"FPS: {fps}", style);
        }
        void OnDestroy() { /* cleanup if needed */ }
    }

    private class DevConsoleBehaviour : MonoBehaviour
    {
        static List<string> logs = new List<string>();
        bool visible = true;
        Vector2 scroll = Vector2.zero;
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.BackQuote)) visible = !visible;
        }
        public static void OnLogReceivedStatic(string logString, string stackTrace, LogType type)
        {
            logs.Add($"[{type}] {logString}");
            if (logs.Count > 200) logs.RemoveAt(0);
        }
        void OnGUI()
        {
            if (!visible) return;
            GUI.Box(new Rect(10, 40, Screen.width - 20, 200), "Dev Console");
            scroll = GUI.BeginScrollView(new Rect(20, 70, Screen.width - 40, 160), scroll, new Rect(0, 0, Screen.width - 80, logs.Count * 20));
            for (int i = 0; i < logs.Count; i++)
            {
                GUI.Label(new Rect(0, i * 20, Screen.width - 100, 20), logs[i]);
            }
            GUI.EndScrollView();
        }
    }

    private class PracticeTargetBehaviour : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            Debug.Log("[DevTools] Practice target hit by: " + other.name);
            // Visual feedback
            var r = gameObject.AddComponent<AutoFlash>();
            UnityEngine.Object.Destroy(r, 0.2f);
        }
    }

    private class AutoFlash : MonoBehaviour
    {
        float t = 0f;
        void Update()
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.one * (0.5f + Mathf.Sin(t * 40f) * 0.05f);
            if (t > 0.2f) UnityEngine.Object.Destroy(this);
        }
    }

    private class PerfPanelBehaviour : MonoBehaviour
    {
        GUIStyle style;
        void Awake()
        {
            style = new GUIStyle();
            style.fontSize = 14;
            style.normal.textColor = Color.white;
        }
        void OnGUI()
        {
            float fps = 1f / Time.unscaledDeltaTime;
            long mem = GC.GetTotalMemory(false);
            GUI.Box(new Rect(Screen.width - 210, 10, 200, 70), "Perf");
            GUI.Label(new Rect(Screen.width - 200, 30, 180, 20), $"FPS: {fps:F1}", style);
            GUI.Label(new Rect(Screen.width - 200, 50, 180, 20), $"Managed Mem: {mem / 1024} KB", style);
        }
    }
}
