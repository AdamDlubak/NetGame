using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuMonitor : MonoBehaviour {

    public GameObject escapeMenu;
    private bool _isShowing;
    public static event System.Action OnEscapeMenuOpened;

    private void OnLevelWasLoaded(int level)
    {
        print("OnLevelWasLoaded with index " + level);
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            _isShowing = !_isShowing;
            ChangeMenuState(_isShowing);
        }
    }

    public void ChangeMenuState(bool state)
    {
        _isShowing = state;
        escapeMenu.SetActive(state);

        print(state);
        
        if(state == true)
        {
            if (OnEscapeMenuOpened != null)
                OnEscapeMenuOpened();
        }
    }
}
