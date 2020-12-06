using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshTyper : MonoBehaviour
{
	public bool isActive;
	public float timeInSeconds;
	public float timeToChange;
	public TextMeshTyper nextPhrase;
	
	TextMeshProUGUI textMesh;
	string [] textCharacter;
	bool waitStarted = false;
	float timer;
	int charCount;
	
    // Start is called before the first frame update
    void Start()
    {
	    textMesh = GetComponent<TextMeshProUGUI>();
	    textCharacter = new string[textMesh.text.Length];
	    for(int i = 0; i < textMesh.text.Length; i++)
	    {
	    	textCharacter[i] = textMesh.text.Substring(i,1);
	    }
	    textMesh.text = "";
	    charCount = 0;
	    timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
	    if(isActive)
	    {
	    	if(charCount < textCharacter.Length)
	    	{
	    		timer += Time.deltaTime;
	    		if(timer >= timeInSeconds)
	    		{
	    			textMesh.text += textCharacter[charCount];
	    			charCount++;
	    			timer = 0;
	    		}
	    	}
	    	if(charCount == textCharacter.Length)
	    	{
	    		if(transform.childCount > 0)
	    		{
	    			transform.GetChild(0).GetComponent<TextMeshTyper>().isActive = true;
	    			charCount++;
	    		}
	    		else
	    		{
	    			if(waitStarted)
		    			return;
		    		StartCoroutine(WaitToChangeText());	
	    			print("Phrase finished");
	    		}
	    	} 	
	    }
    }
    
	public void ActivateText()
	{
		isActive = true;
	}
	
	IEnumerator WaitToChangeText()
	{
		waitStarted = true;
		yield return new WaitForSeconds(timeToChange);
		if(nextPhrase != null)
		{
			nextPhrase.gameObject.SetActive(true);
			
		}		
		else
		{
			print("Fin Statistics");
			FindObjectOfType<PostProcessController>().TurnOffDepth(false);
		}
		gameObject.SetActive(false);
	}
}
