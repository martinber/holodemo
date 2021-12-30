using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ControllerScript : MonoBehaviour
{
    public GameObject huntGameController;
    public GameObject evalGameController;
    public GameObject infoText;

    private KeywordRecognizer keywordRecognizer;
    private string[] keywords = { "play game", "draw lines" };

    // Start is called before the first frame update
    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        ShowInfo("Say \"Play Game\"", "or \"Draw Lines\"");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetFound()
    {
        ShowInfo("Target found", "");
    }

    public void TargetLost()
    {
        ShowInfo("Target lost", "");
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        if (args.text == "play game")
        {
            huntGameController.SetActive(true);
            ShowInfo("Started Game", "");
            keywordRecognizer.Stop();
        }
        if (args.text == "draw lines")
        {
            evalGameController.SetActive(true);
            ShowInfo("Started Line Evaluator", "");
            keywordRecognizer.Stop();
        }
    }

    private void ShowInfo(string line1, string line2)
    {
        infoText.GetComponent<TextMesh>().text = $"{line1}\n{line2}.";
    }
}