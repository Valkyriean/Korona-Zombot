using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    private int difficulty;
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health = 100;
    public int damageEnemy = 10;
    private float staggered = 0;
    private Vector3 stagger = new Vector3(0f, 0f, 0f);
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 5f;

    //Attacking
    public float timeBetweenAttacks = 2f;
    public float timeAttacksOffset = 1.0f;
    public bool timeDoneAttack = false;
    // private bool alreadyAttacked, firstAttack;
    // [SerializeField] private GameObject projectile;
    // [SerializeField] private float speed = 20f;
    //States
    public float sightRange = 10f, attackRange = 3f;
    public bool playerInSightRange, playerInAttackRange, playerInStartAttackRange;


    private int state = 1;
    public Animator anim;
    private AnimatorClipInfo[] CurrentClipInfo;
    private HealthController healthManager;

    private AudioSource hit, die;
    private void Awake()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty", 1);
        
        if (difficulty >= 1) {
            health += 30;
            damageEnemy += 5;
        }
        if (difficulty >= 2) {
            health += 50;
            damageEnemy += 5;
        }
        player = GameObject.Find("Player").transform;
        healthManager = player.GetChild(0).gameObject.GetComponent<HealthController>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        // timeDoneAttack = false;
        // time = 0;
        anim.SetInteger("bot_state",1);
        exp = GetComponent<ParticleSystem>();
        if (difficulty == 0) {
            if (Random.Range(0f, 1f) <= 0.5f) Destroy(this.gameObject);
        }
        
    }

    private ParticleSystem exp;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Alpha8)) {
        //     putStagger(new Vector3 (0.3f, 0f, 0f));
        // }
        calculateStagger();
        float distance = (player.position - transform.position).sqrMagnitude;
        playerInSightRange = distance < (sightRange * sightRange);
        playerInAttackRange = distance < (attackRange * attackRange);
        playerInStartAttackRange = distance < ((attackRange * 0.7f) * (attackRange * 0.7f));
        if (state == 0){
            return;
        } else if(state == 1){
            if (!playerInSightRange && !playerInStartAttackRange) Patroling();
            if (playerInSightRange && !playerInStartAttackRange) ChasePlayer();
            if (playerInSightRange && playerInStartAttackRange){
                if (InSight()){
                    state = 2;
                    anim.SetInteger("bot_state",state);
                    timeDoneAttack = false;

                    // AttackPlayer();
                } else {
                    ChasePlayer();
                }
            }    
        } else if(state == 2)
        {
            agent.SetDestination(transform.position);
            LookAtPlayer();

            if (!(playerInSightRange && playerInAttackRange && InSight())){
                state = 1;
                anim.SetInteger("bot_state",state);
            } else {
                
                CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
                if (CurrentClipInfo.Length > 0 && CurrentClipInfo[0].clip.name != "bot_attack_S") {
                    // Debug.Log(time);
                    time = 0;
                    timeDoneAttack = false;
                } else if (CurrentClipInfo.Length > 0){
                    AttackPlayer();
                }
            }
        }
        
        

    }

    private bool CanMove(){
        CurrentClipInfo = this.anim.GetCurrentAnimatorClipInfo(0);
        if (CurrentClipInfo[0].clip.name == "bot_Roll") {
            return true;
        }
        return false;
    }

    private bool InSight(){
        RaycastHit hit;
        var ray = new Ray(transform.position, player.position - transform.position);
        return (Physics.Raycast(ray, out hit)) && (hit.transform.CompareTag("Player"));
    }

    private void LookAtPlayer(){
            var lookPos = player.position - transform.position;
            lookPos.y = 0;
            var rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime);
    }

    private void Patroling()
    {


        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Get to the walk point
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;


    }

    private void SearchWalkPoint(){
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
    
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    private void ChasePlayer(){

        if (CanMove()){
            agent.SetDestination(player.position);
        } else {
            agent.SetDestination(transform.position);

        }



    }

    private float time;

    private void AttackPlayer(){

        time += Time.deltaTime;
        if (time > timeAttacksOffset && !timeDoneAttack) {
            timeDoneAttack = true;
            healthManager.ApplyDamage(damageEnemy);
        }
        
        if (time > timeBetweenAttacks) {
            
            time = 0;
            timeDoneAttack = false;
        }


    }


    public void TakeDamage (float damage){
        health -= damage;

        if (health <= 0) {
            die = GetComponents<AudioSource>()[1];
            die.Play();
            state = 0;
            anim.SetInteger("bot_state",state);
            exp.Play();
            Destroy(gameObject, 1f);
        }else {
        hit = GetComponents<AudioSource>()[0];
        hit.Play();
        }
    }

    public void putStagger (Vector3 forceFormOther) {
        stagger = stagger + forceFormOther;
    }

    private void calculateStagger() {
        if (stagger.magnitude > 0.01f) {
            staggered += Time.deltaTime * 3f;
            this.transform.position += stagger.normalized * Time.deltaTime * stagger.magnitude * 15f;
            stagger -= stagger.normalized * Time.deltaTime;
        } else {
            stagger = new Vector3(0f, 0f, 0f);
        }
        // Debug.Log(stagger);
        // Debug.Log(staggered);
        if (staggered > 0f) {
            staggered -= Time.deltaTime;
            if (staggered > 0.5f && state != 0) {
                time = 0;
                state = 1;
                anim.SetInteger("bot_state",state);
            }
        } else {
            staggered = 0f;
        }
    }
}
