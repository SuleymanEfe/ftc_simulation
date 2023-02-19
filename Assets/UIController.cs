using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class UIController : MonoBehaviour
{

    private bool isFileChosen = false;
    private string filePath;
    private string fileName;

    [SerializeField] TextMeshProUGUI selectedFileNameText;

    public void openExplorer()
    {
        filePath = EditorUtility.OpenFilePanel("Chose a file to continue", "", "json");
        fileName = filePath.Split("/")[^1];

        isFileChosen = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (isFileChosen) selectedFileNameText.text = fileName;
    }
}
