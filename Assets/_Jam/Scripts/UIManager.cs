using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtGoal;
    [SerializeField] private GameObject gameOverPanel;
	[SerializeField] private GameObject gameCanvas;
	
    private void Start()
    {
       ShowGameOver(false);
    }

    public void SetGoalText(int soFar, int goal)
    {
        txtGoal.text = soFar.ToString() + "/" + goal.ToString();
    }

    public void ShowGameOver(bool show)
    {
        if (show)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            gameOverPanel.SetActive(false);
        }
    }
    
	public void ShowGameCanvas(bool _show)
	{
		gameCanvas.SetActive(_show);
	}
}
