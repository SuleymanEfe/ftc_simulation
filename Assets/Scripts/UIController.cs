using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    private bool isFileChosen = false;
    private string filePath;
    private string fileName;

    [SerializeField] TextMeshProUGUI selectedFileNameText;
    [SerializeField] private Button startButton;


    [SerializeField] TextMeshProUGUI progressBarText;

    public int progressBarMax = 0;
    public int progressBarCurrent = 0;

    [SerializeField] private Image progressBarMask;

    public void openExplorer()
    {
        filePath = EditorUtility.OpenFilePanel("Chose a file to continue", "", "json");
        fileName = filePath.Split("/")[^1];

        isFileChosen = true;
    }

    void Start()
    {
        
    }

    void Update()
    {
        // Main menu - chose file button & text
        if (isFileChosen) startButton.interactable = true;
        else startButton.interactable = false;

        if (isFileChosen) selectedFileNameText.text = fileName;


        // Progress bar
        progressBarMask.fillAmount = (float)progressBarCurrent / (float)progressBarMax;
        progressBarText.text = progressBarCurrent + " / " + progressBarMax;
    }

    public string getFilePath()
    {
        return filePath;
    }
}
