using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using TMPro;

public class FairyBossBehaviour : MonoBehaviour
{
    [Header("Bounds")]
    [SerializeField] private PolygonCollider2D bossBounds;

    private bool isTeleporting = true;

    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f, attackSpeed;

    [SerializeField]
    private float attackDistance = 4f;

    [SerializeField]
    private float minDistance = 3f;

    //Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    private Vector2 movementInput;

    [SerializeField]
    private ContextSolver movementDirectionSolver;

    bool following = false;

    private Transform targetTransform = null;

    public Transform TargetTransform { get => targetTransform; set => targetTransform = value; }
    private List<Transform> colliders;
    private Coroutine myCoroutine;

    public Material glowMaterial;
    public SpriteRenderer entitySpriteRenderer;
    private Material originalMaterial;

    private IEnumerator TeleportingDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        RandomTeleporting();

        isTeleporting = true;
    }
    private void RandomTeleporting()
    {
        Vector3 randomPoint = CalculateTeleportingLocation();

        StartCoroutine(GlowEffect(randomPoint));
    }
    private IEnumerator GlowEffect(Vector3 pos)
    {
        entitySpriteRenderer.material = glowMaterial;
        yield return new WaitForSeconds(0.2f);
        entitySpriteRenderer.material = originalMaterial;

        movementInput = Vector2.zero;
        following = false;
        transform.position = pos;

        entitySpriteRenderer.material = glowMaterial;
        yield return new WaitForSeconds(0.2f);
        entitySpriteRenderer.material = originalMaterial;
    }
    private Vector3 CalculateTeleportingLocation()
    {
        var bounds = bossBounds.bounds;

        float x = 0;
        float y = 0;
        Vector3 randomPos;
        do
        {
            x = Random.Range(bounds.min.x, bounds.max.x);
            y = Random.Range(bounds.min.y, bounds.max.y);
            randomPos = new Vector3(x, y);

        } while (!bossBounds.OverlapPoint(randomPos));

        float distanceFromPlayer = Vector2.Distance(aiData.currentTarget.position, randomPos);

        if (distanceFromPlayer > 6)
        {
            Vector3 directionToPlayer = (aiData.currentTarget.position - randomPos).normalized;
            randomPos += directionToPlayer * (distanceFromPlayer - 6);
        }

        return randomPos;
    }

    private void Start()
    {
        originalMaterial = entitySpriteRenderer.material;
        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
        attackSpeed = GetComponent<FighterStats>().AttackSpeed; 
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        //Enemy AI movement based on Target availability
        if (targetTransform != null)
        {
            aiData.currentTarget = targetTransform;
            aiData.targets = new List<Transform>() { targetTransform };
            //Looking at the Target
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        //Moving the Agent
        OnMovementInput?.Invoke(movementInput);
    }

    private IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            if (isTeleporting)
            {
                isTeleporting = false;
                myCoroutine = StartCoroutine(TeleportingDelay(5));
            }
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                if (distance < minDistance)
                {
                    // Move away
                    movementInput = (minDistance * (transform.position - aiData.currentTarget.position)).normalized;
                }
                else
                {
                    // Attack logic
                    movementInput = Vector2.zero;
                    OnAttackPressed?.Invoke();
                    yield return new WaitForSeconds(attackSpeed);
                }
            }
            else
            {
                // Chase logic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
            }

            yield return new WaitForSeconds(aiUpdateDelay);
            StartCoroutine(ChaseAndAttack());
        }
    }

    public void StartTeleportingOnHit()
    {
        if (myCoroutine != null)
        {
            StopCoroutine(myCoroutine);
        }
        myCoroutine = StartCoroutine(TeleportingDelay(0));
    }
}
