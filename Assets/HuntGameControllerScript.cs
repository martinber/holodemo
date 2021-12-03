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
        public GameObject infoText;

        private List<GameObject> prizes;
        private GameObject prize = null;
        private PrizeScript prizeScript;

        private KeywordRecognizer keywordRecognizer;
        private string[] keywords = { "start line", "abracadabra" };

        public uint numOfPrizes = 50;
        private bool creatingPrizes = false;
        public float scanTime = 10.0f;
        private System.Random rnd;

        // TODO: Move SpatialMapping object to other layer?

        void Start()
        {
            prizes = new List<GameObject>();
            // TODO: Remove
            prizes.Add((GameObject)Instantiate(prizePrefab, new Vector3(0, 0, 0), Quaternion.identity));

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

            ShowInfo("Game ready", "");
        }

        /// <summary>
        /// Start tracking a random prize
        /// </summary>
        private void SelectPrize()
        {
            int i = Random.Range(0, prizes.Count);
            prize = prizes[i];
            prizeScript = prize.GetComponent<PrizeScript>();
        }

        private void HandUpdated(UnityEngine.XR.WSA.Input.InteractionSourceState state)
        {

            if (prize != null)
            {
                Vector3 pos;
                if (state.sourcePose.TryGetPosition(out pos))
                {
                    clue.transform.position = pos;

                    float dist = Vector3.Distance(pos, prize.transform.position);
                    clue.GetComponent<Renderer>().material.color = Color.Lerp(closeClueColor, farClueColor, dist / maxClueDist);

                    if (dist < 0.3)
                    {
                        prizeScript.ActAsFound();
                    }
                }
            }
        }

        private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
        {
            clueScript.EnablePower();
        }

        IEnumerator WaitScanTime()
        {
            creatingPrizes = false;
            ShowInfo("Waiting scanning time", "");

            yield return new WaitForSeconds(scanTime);

            creatingPrizes = true;
            ShowInfo("Finished waiting scanning time", "");
        }

        void Update()
        {
            if (prize == null)
            {
                SelectPrize();
                //prizeScript.ActAsFound();
            }
            else
            {
                clue.transform.LookAt(prize.transform.position);
            }
       
            if (creatingPrizes && prizes.Count < numOfPrizes)
            {
                List<Mesh> meshList = SpatialMappingManager.Instance.GetMeshes();

                Mesh mesh;
                do
                {
                    int meshIdx = rnd.Next(0, meshList.Count);
                    mesh = meshList[meshIdx];
                    mesh.RecalculateNormals();
                } while (mesh == null); // To guarantee that the mesh is loaded is not empty

                Debug.Log($"Number of meshes: {meshList.Count}");
                Debug.Log($"Number of vertices: {mesh.vertices.Length}");
                //Debug.Log("Mesh = " + String.Join("\n",
                //    new List<Vector3>(mesh.vertices)
                //    .ConvertAll(i => i.ToString("F4"))
                //    .ToArray()));

                int prizePosIdx = rnd.Next(0, mesh.vertices.Length);
                Vector3 prizePosition = mesh.vertices[prizePosIdx];
                prizes.Add((GameObject)Instantiate(prizePrefab, prizePosition, Quaternion.identity));
                Debug.Log($"Created prize in: {prizePosition}");
            }
            SpatialMappingManager.Instance.StopObserver();
        }

        private void ShowInfo(string line1, string line2)
        {
            infoText.GetComponent<TextMesh>().text = $"{line1}\n{line2}.";
        }
    }
}