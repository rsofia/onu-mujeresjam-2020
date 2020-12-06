using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostProcessController : MonoBehaviour
{
	private Volume gameVolume;
	private DepthOfField depthProperty;
	
    // Start is called before the first frame update
    void Start()
    {
	    gameVolume = GetComponent<Volume>();
	    gameVolume.profile.TryGet(out depthProperty);
    }

	public void TurnOffDepth(bool _active){
		depthProperty.active = _active;
	}
}
