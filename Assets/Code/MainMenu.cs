using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static string algorithmName;
    public static string checkpointsAmount;
    private List<string> _algorithms = new List<string>() { "Insertion", "Brute-force", "Random checkpoints" };
    private List<string> _checkpoints = new List<string>() { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

    [SerializeField]
    private Dropdown _algorithmDropdown;

    [SerializeField]
    private Dropdown _checkpointsDropdown;
    

    private void Start()
    {
        _algorithmDropdown.AddOptions(_algorithms);
        _checkpointsDropdown.AddOptions(_checkpoints);
    }

    public void DropdownAlgorithm_IndexChanged(int index)
    {
        algorithmName = _algorithms[index];
        Debug.Log(algorithmName);
    }

    public void DropdownCheckpoints_IndexChanged(int index)
    {
        checkpointsAmount = _checkpoints[index];
        Debug.Log(checkpointsAmount);
    }

    public void NextButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitButton()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
