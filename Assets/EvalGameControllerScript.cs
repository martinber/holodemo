using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.XR.WSA.Input;

public class EvalGameControllerScript : MonoBehaviour
{
    public GameObject targetLine;
    private TargetLineScript targetLineScript;
    public GameObject userLine;
    private UserLineScript userLineScript;
    public GameObject infoText;

    private KeywordRecognizer keywordRecognizer;
    private string[] keywords = { "start line", "stop line", "move target", "next target" };

    private string infoLine1 = "";
    private string infoLine2 = "";

    // Start is called before the first frame update
    void Start()
    {
        targetLine.SetActive(true);
        userLine.SetActive(true);

        targetLineScript = targetLine.GetComponent<TargetLineScript>();
        userLineScript = userLine.GetComponent<UserLineScript>();

        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();

        ShowInfo("Recording drawing", "Say \"start line\", \"stop line\", \"move target\",\"next target\"");
    }

    // Calculates distance between two lines
    private float LineCompare(Vector3[] user_line, Vector3[] target_line)
    {
        float accum_dist = 0; // Sum of min_dist
        foreach (Vector3 u_p in user_line)
        {
            float min_dist = 10; // metres
            foreach (Vector3 t_p in target_line)
            {
                float dist = Vector3.Distance(u_p, t_p);
                if (dist < min_dist)
                {
                    min_dist = dist;
                }
            }
            accum_dist += min_dist;
        }
        float avg_dist = accum_dist / (float)user_line.Length;
        return avg_dist;
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        ShowInfo(args.text, null);
        if (args.text == "start line")
        {
            userLineScript.StartDrawing();
            ShowInfo("Recording drawing", null);
        }
        if (args.text == "stop line")
        {
            userLineScript.StopDrawing();

            Vector3[] userVertices = userLineScript.GetVertices();
            Vector3[] targetVertices = targetLineScript.GetVertices();
            float distance = LineCompare(userVertices, targetVertices);
            ShowInfo($"Stopped drawing. Distance: {distance}", null);

            Debug.Log("Line = " + String.Join("\n",
                new List<Vector3>(userVertices)
                .ConvertAll(i => i.ToString("F4"))
                .ToArray()));
        }
        if (args.text == "move target")
        {
            targetLine.transform.position = userLineScript.lastHandPos;
        }
        if (args.text == "next target")
        {
            targetLineScript.NextTarget();
        }
    }

    // Update text, the second line is not modified if null
    private void ShowInfo(string line1, string line2)
    {
        infoLine1 = line1;
        if (line2 != null)
        {
            infoLine2 = line2;
        }

        infoText.GetComponent<TextMesh>().text = $"{infoLine1}\n{infoLine2}.";
    }
}
