using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PauseManager : MonoBehaviourSingleton<PauseManager>
{
    public event Action OnPause;
    public event Action OnUnpause;

    public bool isPaused { get; private set; } = false;

    public void Pause() {
        OnPause?.Invoke();
        isPaused = true;
    }

    public void UnPause() {
        OnUnpause?.Invoke();
        isPaused = false;
    }
}
