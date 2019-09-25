﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static string algorithmName;
    public static string checkpointsAmount;
    private List<string> _algorithmsOptions = new List<string>() { "Insertion", "Brute-force", "Random checkpoints" };
    private List<string> _checkpointsOptions = new List<string>() { "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };
    private List<string> _resolutionsOptions = new List<string>();
    private Resolution[] _resolutions;

    [SerializeField]
    private Dropdown _algorithmDropdown;

    [SerializeField]
    private Dropdown _checkpointsDropdown;

    [SerializeField]
    private Dropdown _resolutionsDropdown;



    private void Start()
    {
        _algorithmDropdown.AddOptions(_algorithmsOptions);
        _checkpointsDropdown.AddOptions(_checkpointsOptions);

        _resolutions = Screen.resolutions;

        int currentResolutionIndex = 0;
        for (int i = 0; i < _resolutions.Length; i++)
        {
            string resolutionOption = _resolutions[i].width + " x " + _resolutions[i].height;
            _resolutionsOptions.Add(resolutionOption);

            if (_resolutions[i].width == (Screen.width) &&
                _resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        _resolutionsDropdown.AddOptions(_resolutionsOptions);
        _resolutionsDropdown.value = currentResolutionIndex;
        _resolutionsDropdown.RefreshShownValue();
    }

    public void DropdownAlgorithm_IndexChanged(int index)
    {
        algorithmName = _algorithmsOptions[index];
        Debug.Log(algorithmName);
    }

    public void DropdownCheckpoints_IndexChanged(int index)
    {
        checkpointsAmount = _checkpointsOptions[index];
        Debug.Log(checkpointsAmount);
    }

    public void DropdownResolution_IndexChanged (int index)
    {
        Resolution resolution = _resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
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
