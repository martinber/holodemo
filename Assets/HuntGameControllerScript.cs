using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using UnityEngine.Windows.Speech;

namespace Academy.HoloToolkit.Unity
{
    public class HuntGameControllerScript : MonoBehaviour
    {
        public GameObject clue;
        private ClueScript clueScript;
        public GameObject prizePrefab;
        public float findDist;
        public GameObject infoText;

        private int score = 0;

        private GameObject prize = null;
        private PrizeScript prizeScript;

        private KeywordRecognizer keywordRecognizer;
        private string[] keywords = { "super power", "start line", "abracadabra", "help me", "show prize", "show mesh", "hide mesh", "new prize" };

        private bool creatingPrizes = false;
        public float scanTime = 10.0f;
        private System.Random rnd;

        // TODO: Move SpatialMapping object to other layer?

        void Start()
        {
            clue.SetActive(true);
            clueScript = clue.GetComponent<ClueScript>();

            InteractionManager.InteractionSourceUpdatedLegacy += HandUpdated;
            clue.transform.position = new Vector3(0, 0, 0);

            keywordRecognizer = new KeywordRecognizer(keywords);
            keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
            keywordRecognizer.Start();

            rnd = new System.Random();
            SpatialMappingManager.Instance.StartObserver();
            StartCoroutine(WaitScanTime());

            ShowInfo("Game starting", "");
        }

        private void AddScore()
        {
            score++;
            ShowInfo($"Score: {score}", "");
        }

        private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
        {
            Vector3 handPos;
            if (state.sourcePose.TryGetPosition(out handPos))
            {
                clueScript.UpdatePosition(handPos);
            }
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            if (args.text == "show prize")
            {
                if (prize != null)
                {
                    prizeScript.MakeVisible();
                }
            }
            else if (args.text == "new prize")
            {
                if (prize != null)
                {
                    prizeScript.ActAsFound();
                    AddScore();
                }
            }
            else if (args.text == "show mesh")
            {
                SpatialMappingManager.Instance.drawVisualMeshes = true;
            }
            else if (args.text == "hide mesh")
            {
                SpatialMappingManager.Instance.drawVisualMeshes = false;
            }
            else
            {
                clueScript.EnablePower();
            }
        }

        IEnumerator WaitScanTime()
        {
            creatingPrizes = false;

            yield return new WaitForSeconds(scanTime);

            creatingPrizes = true;
            ShowInfo("Game started", "");
        }

        void Update()
        {
            if (prize != null)
            {
                clue.transform.LookAt(prize.transform.position);

                float dist = Vector3.Distance(clue.transform.position, prize.transform.position);
                //clue.GetComponent<Renderer>().material.color = Color.Lerp(closeClueColor, farClueColor, dist / maxClueDist);

                if (dist < findDist)
                {
                    prizeScript.ActAsFound();
                    AddScore();
                }
            }

            if (creatingPrizes && prize == null)
            {
                Vector3? prizePosition = SelectPrizePosition();
                
                // Check if returned a value
                if (prizePosition is Vector3 pos)
                {
                    prize = (GameObject)Instantiate(prizePrefab, pos, Quaternion.identity);
                    prizeScript = prize.GetComponent<PrizeScript>();

                    //SpatialMappingManager.Instance.StopObserver();
                }
            }
        }

        private void ShowInfo(string line1, string line2)
        {
            infoText.GetComponent<TextMesh>().text = $"{line1}\n{line2}.";
        }

        /**
         * Returns Vector3 position for a prize, selected from a random vertex of the world
         * taking into account that the vertex is not too high or low
         * 
         * Can return null if couldnt find a vertex, in that case just try again next frame.
         */
        private Vector3? SelectPrizePosition()
        {
            int maxTries = 10;
            for (int i = 0; i < maxTries; i++)
            {
                List<MeshFilter> meshFilterList = SpatialMappingManager.Instance.GetMeshFilters();

               /*
               foreach (MeshFilter mf in meshFilterList)
               {

                   Mesh m = mf.sharedMesh;
                   Transform t = mf.transform;
                   Debug.Log("Mesh = " + string.Join("\n",
                       new List<Vector3>(m.vertices)
                       .ConvertAll(i => t.TransformPoint(i).ToString("F4"))
                       .ToArray()));
               }
               */

                Mesh mesh;
                Transform transform;
                do
                {
                    int meshFilterIdx = rnd.Next(0, meshFilterList.Count);

                    mesh = meshFilterList[meshFilterIdx].sharedMesh;
                    transform = meshFilterList[meshFilterIdx].transform;

                    //mesh.RecalculateNormals();
                } while (mesh == null); // To guarantee that the mesh is loaded is not empty

                int prizePosIdx = rnd.Next(0, mesh.vertices.Length);

                Vector3 candidatePosition = transform.TransformPoint(mesh.vertices[prizePosIdx]);

                if (-0.5f < candidatePosition.y && candidatePosition.y < 0.5f)
                {
                    return candidatePosition;
                }
            }
            return null;
        }
    }
}