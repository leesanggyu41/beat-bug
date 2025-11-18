using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField]
    private bool isAttack;
    public Boss Boss;
    public Circlerotate circlerotate;
    public GameManager gameManager;
    public spikeEdier spikeedier;
    private bool isAttacked;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAttack && !isAttacked && Input.GetKeyDown(KeyCode.Space))
        {
            Boss.Hit(1);
            isAttacked = true;
            Invoke("rr", 0.1f);
        }

    }

    private void rr()
    {
        isAttacked = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Eneny")
        //{
        //    circlerotate.isDie = true;
        //    gameManager.playerDie();
        //}

        if (collision.tag == "www")
        {
           if(spikeedier != null)spikeedier.reset();  
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "AttackZone")
        {
            isAttack = true;
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "AttackZone")
        {
            isAttack = false;
            
        }
    }
}
