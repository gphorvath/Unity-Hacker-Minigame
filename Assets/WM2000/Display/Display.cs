using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    [SerializeField] Terminal connectedToTerminal;

    // TODO calculate these two if possible
    [SerializeField] int charactersWide = 40;
    [SerializeField] int charactersHigh = 14;

    Text screenText;

    private void Start()
    {
        screenText = GetComponentInChildren<Text>();
        WarnIfTerminalNotConnected();
    }

    private void WarnIfTerminalNotConnected()
    {
        if (!connectedToTerminal)
        {
            Debug.LogWarning("Display not connected to a terminal");
        }
    }

    // Akin to monitor refresh
    private void FixedUpdate()
    {
        if (connectedToTerminal)
        {
            screenText.text = connectedToTerminal.GetDisplayBuffer(charactersWide, charactersHigh);
        }
    }
} 