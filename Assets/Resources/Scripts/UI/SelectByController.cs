using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectByController : MonoBehaviour 
{

	public EventSystem eventSystem;
	public GameObject selectedObject;

	private bool _buttonSelected;
	
	void Update () 
	{
		if (Input.GetAxisRaw ("Vertical") != 0 && !_buttonSelected) {
			eventSystem.SetSelectedGameObject(selectedObject);
			_buttonSelected = true;
		}	
	}

	private void OnDisable() 
	{
		_buttonSelected = false;
	}
}
