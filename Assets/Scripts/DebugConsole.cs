using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugConsole : MonoBehaviour
{
    [SerializeField] private TMP_Text debugText;
    private Queue<string> debugMessages = new Queue<string>();
    private int maxMessages = 10;

    private void Awake()
    {
        if (debugText == null)
        {
            Debug.LogError("Debug Text is not assigned in the DebugConsole script.");
            return;
        }

        Application.logMessageReceived += HandleLog;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if (debugMessages.Count >= maxMessages)
        {
            debugMessages.Dequeue();
        }

        debugMessages.Enqueue(logString);
        string text = "";

        foreach (string message in debugMessages)
        {
            text += message + "\n";
        }

        debugText.text = text;
    }
}
