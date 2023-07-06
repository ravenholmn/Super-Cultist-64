using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Transform player;
    public Transform mesh;
    public Vector3 walkPoint;
    public SphereCollider sightCollider;
    public BoxCollider attackCollider;
    public AudioSource source;
    private bool pointSet;
    public float walkPointRange;
    public float timeBetweenAttacks;
    private bool alreadyAttacked;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    private bool dead;

    private void Start()
    {
        player = PlayerController.Instance.playerTransform;
        sightCollider.radius = sightRange;
        attackCollider.size = new Vector3(attackRange, 0.75f, attackRange);
    }

    private void Update()
    {
        if (dead) return;
        if (!playerInAttackRange && !playerInSightRange) Patrol();
        if (!playerInAttackRange && playerInSightRange) Chase();
        if (playerInAttackRange && playerInSightRange) Attack();
        
    }

    void Patrol()
    {
        if (!pointSet) SearchWalkPoint();

        if (pointSet) Agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = Agent.transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude <= 0.25f)
        {
            pointSet = false;
        }
    }

    private void SearchWalkPoint()
    {
        float randomZ = CalculateRange();
        float randomX = CalculateRange();

        var position = transform.position;
        walkPoint = new Vector3(position.x + randomX, position.y, position.z + randomZ);

        pointSet = true;
    }

    private float CalculateRange()
    {
        return Random.Range(-walkPointRange, walkPointRange);
    }

    void Chase()
    {
        Agent.SetDestination(player.position);
    }

    void Attack()
    {
        Agent.SetDestination(player.position);

        if (!alreadyAttacked)
        {
            PlayerController.Instance.GotHit(PlayerController.Instance.playerTransform.position - Agent.transform.position);
            if (PlayerController.Instance.PlayerDied)
            {
                playerInAttackRange = false;
                playerInSightRange = false;
            }
            alreadyAttacked = true;
            SoundEffectPlayer.instance.PlaySfx(SoundEffectPlayer.Sfx.Hit);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void PlayerInRange(PlayerDetection.RangeType rangeType, bool inRange)
    {
        if (rangeType == PlayerDetection.RangeType.Sight)
        {
            playerInSightRange = inRange;
        }
        else
        {
            playerInAttackRange = inRange;
        }
    }

    public void GetHit()
    {
        dead = true;
        source.Play();
        sightCollider.gameObject.SetActive(false);
        attackCollider.gameObject.SetActive(false);
        mesh.DOScaleY(0.1f, 0.15f).OnComplete(() =>
        {
            Destroy(gameObject, 0.5f);
        });
    }
}
