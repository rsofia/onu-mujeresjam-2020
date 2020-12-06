using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
	public float timeInSeconds;
	
	public void OnStartGameClicked()
	{
		
	}
	
	public void OnExitClicked()
	{
		StartCoroutine(QuitGame());
	}
	
	IEnumerator QuitGame()
	{
		yield return new WaitForSeconds(timeInSeconds);
		Application.Quit();
	}
}
