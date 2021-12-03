using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class ClueScript : MonoBehaviour
{
    public ParticleSystem continuousParticleSystem;
    public ParticleSystem burstParticleSystem;

    private KeywordRecognizer keywordRecognizer;
    private string[] keywords = { "play video" };

    // Start is called before the first frame update
    void Start()
    {
        burstParticleSystem.Stop();

        keywordRecognizer = new KeywordRecognizer(keywords);
        keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        burstParticleSystem.Play();
    }
}
