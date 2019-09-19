using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    List<string> algorithms = new List<string>() { "Insertion", "Brute-force", "Random checkpoints" };
    List<string> checkpoints = new List<string>() { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };
    public Dropdown checkpointsDropdown;
    public Dropdown algorithmDropdown;
    public static string algorithmName;
    public static string checkpointsAmount;

    void Start()
    {
        algorithmDropdown.AddOptions(algorithms);
        checkpointsDropdown.AddOptions(checkpoints);
    }

    public void dropdownAlgorithm_IndexChanged(int index)
    {
        algorithmName = algorithms[index];
        Debug.Log(algorithmName);
    }

    public void dropdownCheckpoints_IndexChanged(int index)
    {
        checkpointsAmount = checkpoints[index];
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
