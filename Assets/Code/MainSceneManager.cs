using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MainSceneManager : MonoBehaviour
{
    public static Vector3 clickPosition;
    public static List<GameObject> myCheckpoints = new List<GameObject>();
    public static double totalDistance;
    public static float startTime;
    private int _placedCheckpoints = 0;
    private int _checkpointsAmount = int.Parse(MainMenu.checkpointsAmount);
    private bool _placeNPC = true;

    [SerializeField]
    private GameObject _NPC;

    [SerializeField]
    private LayerMask _clickMask;

    [SerializeField]
    private GameObject _checkpoint;

    [SerializeField]
    private Text _headerText;

    [SerializeField]
    private Text _algorithmText;

    [SerializeField]
    private Text _checkpointsText;

    [SerializeField]
    private Text _ComputingText;

    [SerializeField]
    private Text _DistanceText;

    [SerializeField]
    private Button _StartButton;


    private void Start()
    {
        _headerText.text = "Place checkpoints: 0/" + MainMenu.checkpointsAmount;
        _checkpointsText.text += MainMenu.checkpointsAmount;
        _algorithmText.text = "ALGORITHM: " + MainMenu.algorithmName.ToUpper();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _placedCheckpoints < _checkpointsAmount)
        {
            clickPosition = -Vector3.one;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 80f, _clickMask))
            {
                clickPosition = hit.point;

                clickPosition.y += (float)0.1;
                GameObject newCheckpoint = (GameObject)Instantiate(_checkpoint, clickPosition, Quaternion.identity);
                myCheckpoints.Add(newCheckpoint);
                _placedCheckpoints++;
                _headerText.text = $"Place checkpoints: {_placedCheckpoints}/{_checkpointsAmount}";

                if (_placeNPC)
                {
                    _NPC.transform.position = clickPosition;
                    _placeNPC = false;
                }
            }
        }
    }

    public void StartButton()
    {
        if (_placedCheckpoints == _checkpointsAmount)
        {
            _StartButton.enabled = false;
            startTime = Time.time;
            var watch = System.Diagnostics.Stopwatch.StartNew();

            AlgorithmManager algorithm = new AlgorithmManager();

            if (MainMenu.algorithmName == "Insertion")
            {
                algorithm.SetAlgorithm(new InsertionAlgorithm());
            }
            else if (MainMenu.algorithmName == "Brute-force")
            {
                
            }
            else if (MainMenu.algorithmName == "Random checkpoints")
            {
                
            }

            algorithm.FindTheShortest();

            totalDistance = ComputeDistance(Algorithm.finalShortest);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            string computingMinutes;
            string computingSeconds;

            if ((elapsedMs / 1000) / 60 < 10)
            {
                computingMinutes = "0" + ((elapsedMs / 1000) / 60).ToString();
            }
            else
            {
                computingMinutes = ((elapsedMs / 1000) / 60).ToString();
            }

            if ((elapsedMs / 1000) < 10)
            {
                computingSeconds = "0" + (((elapsedMs / 1000)) % 60).ToString("f2");
            }
            else
            {
                computingSeconds = (((elapsedMs / 1000)) % 60).ToString("f2");
            }

            _ComputingText.text += "\n" + computingMinutes + ":" + computingSeconds;
            _DistanceText.text += totalDistance.ToString("f2");

            if (Traveling.myNavMeshAgent == null)
            {
                Debug.LogError("NavMeshAgent component isn't attached to " + gameObject.name);
            }
            else
            {
                if (myCheckpoints != null && myCheckpoints.Count >= 2)
                {
                    Traveling.MySetDestination();
                }
                else
                {
                    Debug.Log("Need more checkpoints");
                }
            }
        }
    }

    public void MenuButton()
    {
        Traveling.isTraveling = false;
        myCheckpoints.Clear();
        _headerText.text = "Place checkpoints: 0/" + MainMenu.checkpointsAmount;
        Traveling.currentCheckpointIndex = 0;
        SceneManager.LoadScene(0);
    }

    public static double ComputeDistance(List<int> myList)
    {
        double distance = 0;

        for (int i = 0; i < myList.Count - 1; i++) //10 elements: 0-9, index [8] goes to [9] and we stop there
        {
            distance += Vector3.Distance(MainSceneManager.myCheckpoints[myList[i]].transform.position, MainSceneManager.myCheckpoints[myList[i + 1]].transform.position);
        }

        return distance;
    }
}
