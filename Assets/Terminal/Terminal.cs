﻿using UnityEngine;
using System.Reflection;
using UnityEngine.UI;

public class Terminal : MonoBehaviour
{

    public static Text screenText;

    public DisplayBuffer displayBuffer;
    InputBuffer inputBuffer;

    static Terminal primaryTerminal;

    private void Awake()
    {
        if (primaryTerminal == null) { primaryTerminal = this; } 
        inputBuffer = new InputBuffer();
        displayBuffer = new DisplayBuffer(inputBuffer);
        inputBuffer.onCommandSent += NotifyCommandHandlers;
    }

    public string GetDisplayBuffer(int width, int height)
    {
        return displayBuffer.GetDisplayBuffer(Time.time, width, height);
    }
    


    public void ReceiveFrameInput(string input)
    {
        inputBuffer.ReceiveFrameInput(input);
    }

    public static void ClearScreen()
    {
        primaryTerminal.displayBuffer.Clear();
    }

    public static void WriteLine(string line)
    {
        primaryTerminal.displayBuffer.WriteLine(line);
    }

    public static void RemoveLast()
    {
        primaryTerminal.displayBuffer.RemoveLine();
    }

    public static void WriteKey(string key)
    {
        primaryTerminal.displayBuffer.WriteKey(key);
    }

    public void NotifyCommandHandlers(string input)
    {
        var allGameObjects = FindObjectsOfType<MonoBehaviour>();
        foreach (MonoBehaviour mb in allGameObjects)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;
            var targetMethod = mb.GetType().GetMethod("OnUserInput", flags);
            if (targetMethod != null)
            {
                object[] parameters = new object[1];
                parameters[0] = input;
                targetMethod.Invoke(mb, parameters);
            }
        }
    }
}