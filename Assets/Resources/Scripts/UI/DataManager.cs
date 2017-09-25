using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour 
{

	public static DataManager instance;

    public string nick;
    public string address;
	public float sfxLevel; 
	public float musicLevel;

	void Awake() 
	{
		if (instance == null)
		{
			instance = this;	
		} 
		else if (instance != this) 
		{
			Destroy(gameObject);
		}

		DontDestroyOnLoad(gameObject);
	}

    public void setupPlayerInputField()
    {
        string nickFieldValue = GameObject.Find("NickTextInput").GetComponent<InputField>().text;
        if(nickFieldValue != "")
        {
            nick = nickFieldValue;
        }
        else
        {
            nick = "anonymousPlayer";
        }
    }
}
