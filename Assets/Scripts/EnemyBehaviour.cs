using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyBehaviour : MonoBehaviour
{
    public NavMeshAgent Enemy;

    EnemyState _enemyState;
    enum EnemyState
    {
        Wandering,
        FollowPlayer,
        ReturnToSpawn
    }

    public int MinWanderDist = 2;
    public int MaxWanderDist = 8;
    public float MinWanderCooldown = 6.0f;
    public float MaxWanderCooldown = 20.0f;

    [Space]
    public int MaxFollowDistance = 15;
    public int PlayerDetectRange = 7;
    public int AttackRange = 1;

    [Space]
    public float TargetPosOffset;

    [Space]
    public float Damage = 2.0f;
    public float AttackCooldown = 2.0f;

    Vector3 _startPos;
    Vector3 _targetPos;

    float _attackTimer;
    float _wanderTimer;
    float _wanderCooldown;

    void Awake()
    {
        _enemyState = EnemyState.Wandering;

        _startPos = this.transform.position;
        _targetPos = GetTargetPosition();

        _wanderCooldown = Random.Range(MinWanderCooldown, MaxWanderCooldown);
    }

    void Update()
    {
        if (!PauzeMenu.pauzeMenu.IsGamePauzed)
        {
            switch (_enemyState)
            {
                default:
                case EnemyState.Wandering:
                    Wander();
                    IsPlayerNearby();
                    break;
                case EnemyState.FollowPlayer:
                    InAttackRange();
                    IsPlayerOutsideRange();
                    break;
                case EnemyState.ReturnToSpawn:
                    _targetPos = _startPos;
                    IsBackAtStartPos();
                    IsPlayerNearby();
                    break;
            }
            _attackTimer += Time.fixedDeltaTime;

            Enemy.SetDestination(_targetPos);
        }
        else
        {
            Enemy.SetDestination(this.transform.position);
        }
    }

    private void AttackPlayer()
    {
        if (_attackTimer >= AttackCooldown)
        {
            LivesManager.Manager.ReducePlayerHP(Damage);

            _attackTimer = 0;
        }
    }

    private void IsBackAtStartPos()
    {
        if (Vector3.Distance(this.transform.position, _targetPos) < TargetPosOffset)
        {
            _enemyState = EnemyState.Wandering;
        }
    }

    private void IsPlayerOutsideRange()
    {
        if (Vector3.Distance(this.transform.position, PlayerController.Player.transform.position) > MaxFollowDistance)
        {
            _enemyState = EnemyState.ReturnToSpawn;
        }
    }

    private void InAttackRange()
    {
        if (Vector3.Distance(this.transform.position, PlayerController.Player.transform.position) < AttackRange)
        {
            _targetPos = this.transform.position;
            AttackPlayer();
        }
        else
        {
            _targetPos = PlayerController.Player.transform.position;
        }
    }

    private void IsPlayerNearby()
    {
        if (Vector3.Distance(this.transform.position, PlayerController.Player.transform.position) < PlayerDetectRange)
        {
            _enemyState = EnemyState.FollowPlayer;
        }
    }

    private void Wander()
    {
        if (Vector3.Distance(this.transform.position, _targetPos) < TargetPosOffset)
        {
            _wanderTimer += Time.fixedDeltaTime;

            if (_wanderTimer >= _wanderCooldown)
            {
                _targetPos = GetTargetPosition();

                _wanderCooldown = Random.Range(MinWanderCooldown, MaxWanderCooldown);
                _wanderTimer = 0;
            }
        }
    }

    private Vector3 GetTargetPosition()
    {
        //Creates a random direction to walk in, and adds a random distance.
        Vector3 direction = new Vector3(Random.Range(-1, 2), 0, Random.Range(-1, 2)).normalized;
        int distance = Random.Range(MinWanderDist, MaxWanderDist);

        return _startPos + (direction * distance);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_startPos, 0.1f);
        Gizmos.DrawWireSphere(this.transform.position, PlayerDetectRange);


        Gizmos.color = Color.black;
        Gizmos.DrawLine(_startPos, _targetPos);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(_targetPos, 0.1f);

    }
}
