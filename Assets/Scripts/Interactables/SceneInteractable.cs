using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class SceneInteractable : Interactable
{
    [SerializeField] 
    private SceneID sceneId;
    [SerializeField] 
    private Vector3 spawnPos;
    [SerializeField] 
    [Tooltip("If true, always spawn at the specified position on scene entry. Otherwise, only use specified position if it is player's first entry to the scene, and by defualt, spawns at last position recorded by player during previous exit of scene.")]
    private bool isSpawnPosFixed = true;

    public override void Interact()
    {
        GameplayManager.Instance.EnterSubScene(sceneId, spawnPos, isSpawnPosFixed);
    }

   
}
