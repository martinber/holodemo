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
    private string[] keywords = { "start line", "stop line" };

    // Start is called before the first frame update
    void Start()
    {
        targetLine.SetActive(true);
        userLine.SetActive(true);
        targetLineScript = targetLine.GetComponent<TargetLineScript>();
        userLineScript = userLine.GetComponent<UserLineScript>();

        Vector3[] user_line = {
            new Vector3(0f, 1f, 0f),
            new Vector3(0f, 1f, 1f),
            new Vector3(0f, 1f, 2f),
            new Vector3(0f, 1f, 3f),
            new Vector3(0f, 1f, 4f),
        };
        Vector3[] target_line = {
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 0.5f),
            new Vector3(0f, 0f, 1f),
            new Vector3(0f, 0f, 1.5f),
            new Vector3(0f, 0f, 2f),
            new Vector3(0f, 0f, 2.5f),
            new Vector3(0f, 0f, 3f),
            new Vector3(0f, 0f, 3.5f),
            new Vector3(0f, 0f, 4f),
        };
        Debug.Log("Line distance");
        Debug.Log(LineCompare(user_line, target_line));

        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
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
        StringBuilder builder = new StringBuilder();
        builder.AppendFormat("{0} ({1}){2}", args.text, args.confidence, Environment.NewLine);
        builder.AppendFormat("\tTimestamp: {0}{1}", args.phraseStartTime, Environment.NewLine);
        builder.AppendFormat("\tDuration: {0} seconds{1}", args.phraseDuration.TotalSeconds, Environment.NewLine);
        Debug.Log(builder.ToString());

        if (args.text == "start line")
        {
            userLineScript.StartDrawing();
            infoText.GetComponent<TextMesh>().text = "Starting drawing";
            Debug.Log("Start Drawing");
        }
        if (args.text == "stop line")
        {
            userLineScript.StopDrawing();
            infoText.GetComponent<TextMesh>().text = "Finished";

            Vector3[] userVertices = userLineScript.GetVertices();
            Vector3[] targetVertices = targetLineScript.GetVertices();
            float distance = LineCompare(userVertices, targetVertices);
            Debug.Log("Stop Drawing");
            infoText.GetComponent<TextMesh>().text = distance.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
