using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class EnemyStateManager: MonoBehaviour
{
    private EnemyBaseState currentState;
    private EnemyBaseState stateBeforePause;

    // concrete states   
    public EnemyPatrolState patrolState { get; private set; } = new EnemyPatrolState();
    public EnemyChaseState chaseState { get; private set; } = new EnemyChaseState();
    public EnemyPauseState pauseState { get; private set; } = new EnemyPauseState();

    // can be adjusted per enemy
    [field: Header("Enemy Movement settings")]
        // path that enemy would follow
        [field: SerializeField] private Transform path; 

        // if true, the enemy would reverse down the path once the path is ended, else it'll loop back to the first waypoint 
        [field: SerializeField] public bool loopReverse { get; private set; } = true; 

        // speeds
        [field: SerializeField] public float moveSpeed { get; private set; } = 1.2f;
        [field: SerializeField] public float rotationSpeed { get; private set; } = 5f;
    
    [field: Header("Object References")]
        public Transform deathzone;
        public GameObject aggroZone;
        public GameObject captureZone;
        public AIPath aiPath;
        public List<Transform> waypoints { get; private set; } = new List<Transform>(); 

    void Start() {
        foreach (Transform child in path) {
            waypoints.Add(child);
        }

        aiPath = GetComponent<AIPath>();

        currentState = patrolState;
        currentState.EnterState(this);

        PauseManager.Instance.OnPause += Pause;
        PauseManager.Instance.OnUnpause += Unpause;
        FishStallSceneManager.instance.OnRespawn += Respawn;
        FishStallSceneManager.instance.ChaseBegin += EnableChase;
    }

    void Update()
    {
        currentState?.UpdateState(this);
    }

    public void ChangeState(EnemyBaseState state)
    {
        currentState?.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }

    public EnemyBaseState CheckCurrentState()
    {
        return currentState;
    }

    private void Pause() {
        stateBeforePause = currentState;
        ChangeState(pauseState);
    }

    private void Unpause() {
        ChangeState(stateBeforePause);
    }

    // if game over and they restart, put them back into their starting positions
    public void Respawn() {
        transform.position = waypoints[0].transform.position;
        ChangeState(patrolState);
    }

    // enable aggro zone so that enemies can chase you
    public void EnableChase() {
        aggroZone.SetActive(true);
    }
}
