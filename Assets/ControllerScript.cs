using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ControllerScript : MonoBehaviour
{
    public GameObject huntGameController;
    public GameObject evalGameController;
    public GameObject infoText;
    public GameObject imageTarget;
    public GameObject wandBall;

    private KeywordRecognizer keywordRecognizer;
    private string[] keywords = { "play game", "draw lines" };

    public bool wandAvailable = false; // Becomes true while the wand is visible
    private Transform wandTransform;

    // Start is called before the first frame update
    void Start()
    {
        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        ShowInfo("Say \"Play Game\"", "or \"Draw Lines\"");
    }

    public Vector3? GetWandPosition()
    {
        if (wandAvailable)
        {
            ShowInfo($"working {wandAvailable}, {wandBall == null}", "");
            return wandBall.transform.position;
        }
        else
        {
            ShowInfo($"{wandAvailable}, {wandBall == null}", "");
            return null;
        }
    }

    public void TargetFound()
    {
        wandAvailable = true;
        wandTransform = imageTarget.transform.Find("Wand");
    }

    public void TargetLost()
    {
        wandAvailable = false;
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