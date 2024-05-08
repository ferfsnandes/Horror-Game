using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public GameObject hand;
    public GameObject[] wayPoints;
    public Animator animator;

    public bool followPlayer = false;
    public int currentPIndex = 0;
    public float distanceVision = 15f;

    public enum status {
        walk = 0,
        run = 1,
        crouch = 2,
        atack = 3
    }

    // Start is called before the first frame update
    void Start() {
        agent = GetComponent<NavMeshAgent>();
        hand.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if(followPlayer) {
            huntPlayer();
        } else {
            patrol();
        }
    }

    // faz o inimigo andar pelo mapa seguindo, aleatoriamente, os pontos pré definidos
    void patrol() {
        agent.speed = 4;
        animator.SetInteger("move", (int)status.walk);

        crouchOnDoors();

        if(Vector3.Distance(transform.position, wayPoints[currentPIndex].transform.position) < 3) {
            currentPIndex = Random.Range(0, wayPoints.Length);
        } else {
            agent.SetDestination(wayPoints[currentPIndex].transform.position); 
            if(onVision())
                followPlayer = true;
        }
    }
    
    bool onVision() {
        float angle = Vector3.Angle((player.transform.position - transform.position), transform.forward);
        float distancePlayer = Vector3.Distance(player.transform.position, transform.position);
        RaycastHit target;
        
        if(angle < 90 && distancePlayer < distanceVision) {
            //cria um raio do inimigo até o player e verifica se bate no player
            if(Physics.Linecast(transform.position, player.transform.position, out target)) {
                if(target.transform.tag == "Player") {
                    return true;
                }
            } 
        }
        return false;
    }

    void huntPlayer() {
        animator.SetInteger("move", (int)status.run);
        agent.speed = 11f;
        agent.SetDestination(player.transform.position);

        if(Vector3.Distance(transform.position, player.transform.position) < 4.4f) {
            hand.SetActive(true);
            StartCoroutine("atackAction");
        }

        if(!onVision()) 
            StartCoroutine("stopHunting");
    }

    void crouchOnDoors() {
        if(agent.isOnOffMeshLink) {
            agent.speed = 2;
            animator.SetInteger("move", (int)status.crouch);
        } 
    }

    IEnumerator atackAction() {
        agent.speed = 0;
        animator.SetInteger("move", (int)status.atack);
        yield return new WaitForSeconds(0.8f);
        hand.SetActive(false); 
    }

    IEnumerator stopHunting() {
        yield return new WaitForSeconds(1.8f);
        followPlayer = false;
    }
}