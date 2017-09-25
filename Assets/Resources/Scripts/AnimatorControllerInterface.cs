using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorControllerInterface : MonoBehaviour {

    public Animator overrideAnimator = null;
    private Animator _animator;

    void Start()
    {
        if (overrideAnimator != null)
        {
            _animator = overrideAnimator;
        }
        else
        {
            _animator = GetComponent<Animator>();
        }
    }

#region PublicInterface
    public void SetBool(string key)
    {
        _animator.SetBool(key, true);
    }

    public void UnsetBool(string key)
    {
        _animator.SetBool(key, false);
    }
#endregion
}
