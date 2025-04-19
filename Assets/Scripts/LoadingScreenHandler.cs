using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreenHandler : MonoBehaviourSingleton<LoadingScreenHandler>
{
    [SerializeField] private GameObject loadingScreen;

    void Start()
    {
        loadingScreen.SetActive(false);
    }
    public void ToggleLoadingScreen(bool visible) {
        loadingScreen.SetActive(visible);
    }

}
