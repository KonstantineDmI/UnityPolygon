using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIEnemy : MonoBehaviour
{
    public float fpsTargetDistance;
    public float enemyLookDistance;
    public float attackDistance;
    public float enemyMovementSpeed;
    public float damping;
    public NavMeshAgent agent;
    public GameObject player;
    public Transform target;
    public float damage;
    public float health;
    Renderer myRender;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        health = 1f;
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player");
        target = player.transform;
        anim = GetComponent<Animator>();
        gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            
            damage = 0.0003f;
            health -= damage;
        }
       
        
    }

    void FixedUpdate()
    {
       
        fpsTargetDistance = Vector3.Distance(target.position, transform.position);
        if (fpsTargetDistance < enemyLookDistance && DialogueWithAI.fight == true)
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.yellow;
            agent.SetDestination(target.position);
            
            //lookAtPlayer();
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        //if (fpsTargetDistance < attackDistance)
        //{
        //    gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        //    agent.SetDestination(target.position);
           
        //    //anim.SetBool("IsRunning", true);
        //}
        //else
        //{
        //    //anim.SetBool("IsRunning", false);
        //}
    }

    void lookAtPlayer()
    {
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);

    }

    
}
