using UnityEngine;
using UnityEngine.UI;

public class Display : MonoBehaviour
{
    [SerializeField] Terminal connectedToTerminal;

    // TODO calculate these two if possible
    [SerializeField] int charactersWide = 40;
    [SerializeField] int charactersHigh = 20;

    Display primaryDisplay;

    Text screenText;

    private void Awake()
    {
        if (primaryDisplay == null) { primaryDisplay = this; } // Be the one
    }

    private void Start()
    {
        screenText = GetComponentInChildren<Text>();
        WarnIfTerminalNotConneced();
    }

    private void WarnIfTerminalNotConneced()
    {
        if (!connectedToTerminal)
        {
            Debug.LogWarning("Display not connected to a terminal");
        }
    }

    // Akin to monitor refresh
    public void Update()
    {
        if (connectedToTerminal)
        {
            screenText.text = connectedToTerminal.GetDisplayBuffer(charactersWide, charactersHigh);
        }
    }
} 