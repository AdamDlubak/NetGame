using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SetSoundLevels : MonoBehaviour 
{

	public Slider sfxLevel;
	public Slider musicLevel;

	public void SetSfxLevel() 
	{
		DataManager.instance.sfxLevel = sfxLevel.value;
	}

	public void SetMusicLevel() 
	{
		DataManager.instance.musicLevel = musicLevel.value;
	}
}
