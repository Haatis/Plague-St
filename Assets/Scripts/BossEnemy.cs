using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : log
{
    public bool isTimer;
    public bool isSpin;
    public bool isFire;
    public bool isAttacked;
    private float timer;
    private float fireTimer;
    private float Attacktimer;
    public Slider healthBar;
    public float closeRadius;
    [SerializeField] GameObject bullet;
    
    
   
    // Start is called before the first frame update
   void Start () {
        currentState = EnemyState.idle;
        target = GameObject.FindWithTag("Player").transform;
        GetComponent<Pathfinding.AIPath>().enabled = false;
        timer = 5f;
        //enable pathfinding
        fireTimer = 5f;
        Attacktimer = 2f;

	}
    // Update is called once per frame
    void Update()
    {
        healthBar.value = health;
        if(isTimer == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isTimer = false;
                timer = 5f;
            }
        }
        if(isSpin == true){
            timer -= Time.deltaTime;
            if(timer <= 0){
                isSpin = false;
                timer = 5f;
            }
        }
        if(isFire == true){
            fireTimer -= Time.deltaTime;
            if(fireTimer <= 0){
                isFire = false;
                fireTimer = 5f;
            }
        }
        if(isAttacked ==true){
            Attacktimer -= Time.deltaTime;
            if(Attacktimer <= 0){
                isAttacked = false;
                Attacktimer = 2f;
            }
        }
    }
       
   
    public override void CheckDistance()
    {
        if (Vector3.Distance(target.position,
                            transform.position) <= chaseRadius
             && Vector3.Distance(target.position,
                               transform.position) > attackRadius)
        {
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isFire==false && isAttacked==false)
            {
                isFire=true;
                isAttacked=true;
                Instantiate(bullet, transform.position, Quaternion.identity);
                
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().enabled = true;
                ChangeState(EnemyState.walk);
            }
        }
        else if (Vector3.Distance(target.position,
                    transform.position) <= chaseRadius
                    && Vector3.Distance(target.position,
                    transform.position) <= attackRadius &&
                    Vector3.Distance(target.position,
                    transform.position) > closeRadius
                    )
        {
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isTimer==false && isAttacked==false)
            {
                
                StartCoroutine(AttackCo());
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().enabled = true;
                ChangeState(EnemyState.walk);
                

            }
        } 
        else if (Vector3.Distance(target.position,
                    transform.position) <= chaseRadius
                    && Vector3.Distance(target.position,
                    transform.position) <= attackRadius &&
                    Vector3.Distance(target.position,
                    transform.position) <= closeRadius)
        {   
            if (currentState == EnemyState.walk
                && currentState != EnemyState.stagger && isSpin == false && isAttacked==false)
            {
                StartCoroutine(SpinCo());
                
            }
            else if (currentState == EnemyState.idle || currentState == EnemyState.walk
                && currentState != EnemyState.stagger && currentState != EnemyState.attack)
            {
                Vector3 temp = Vector3.MoveTowards(transform.position,
                                                         target.position,
                                                         moveSpeed * Time.deltaTime);
                changeAnim(temp - transform.position);
                GetComponent<Pathfinding.AIPath>().enabled = true;
                ChangeState(EnemyState.walk);
                

            }
        }
        else if (Vector3.Distance(target.position,
                            transform.position) > chaseRadius)
        {
            GetComponent<Pathfinding.AIPath>().enabled = false;
        }
    else if (Vector3.Distance(target.position, transform.position) > chaseRadius)
        {
            GetComponent<Pathfinding.AIPath>().enabled = false;
            ChangeState(EnemyState.idle);
        }

    }

    public IEnumerator AttackCo()
    {
        isTimer=true;
        isAttacked=true;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        currentState = EnemyState.attack;
        anim.SetBool("attack", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("attack", false);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f;
    }
    public IEnumerator SpinCo()
    {
        isSpin=true;
        isAttacked=true;
        GetComponent<Pathfinding.AIPath>().maxSpeed = 0f;
        currentState = EnemyState.attack;
        anim.SetBool("spin", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.walk;
        anim.SetBool("spin", false);
        GetComponent<Pathfinding.AIPath>().maxSpeed = 3f;
    }
   
}