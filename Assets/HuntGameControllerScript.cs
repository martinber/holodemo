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
        public Color farClueColor = new Color(1f, 1f, 1f, 0.4f); // Transparent White
        public Color closeClueColor = new Color(1f, 0f, 0f, 0.4f); // Transparent Red
        public float maxClueDist = 6; // 6 meters, the distance where the clue will be of color farClueColor
        public float findDist;
        public GameObject infoText;

        private GameObject prize = null;
        private PrizeScript prizeScript;

        private KeywordRecognizer keywordRecognizer;
        private string[] keywords = { "start line", "abracadabra" };

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
            clueScript.EnablePower();
        }

        IEnumerator WaitScanTime()
        {
            creatingPrizes = false;

            yield return new WaitForSeconds(scanTime);

            creatingPrizes = true;
        }

        void Update()
        {
            ShowInfo($"creatingPrizes: {creatingPrizes}", $"prize: {prize != null}");
            if (prize != null)
            {
                clue.transform.LookAt(prize.transform.position);

                float dist = Vector3.Distance(clue.transform.position, prize.transform.position);
                //clue.GetComponent<Renderer>().material.color = Color.Lerp(closeClueColor, farClueColor, dist / maxClueDist);

                if (dist < findDist)
                {
                    prizeScript.ActAsFound();
                }
            }
       
            if (creatingPrizes && prize == null)
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
                Vector3 prizePosition = transform.TransformPoint(mesh.vertices[prizePosIdx]);

                prize = (GameObject)Instantiate(prizePrefab, prizePosition, Quaternion.identity);
                prizeScript = prize.GetComponent<PrizeScript>();
            }
            //SpatialMappingManager.Instance.StopObserver();
        }

        private void ShowInfo(string line1, string line2)
        {
            infoText.GetComponent<TextMesh>().text = $"{line1}\n{line2}.";
        }
    }
}