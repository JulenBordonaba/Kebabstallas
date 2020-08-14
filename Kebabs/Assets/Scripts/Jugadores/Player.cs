using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    private Animator anim;
    private Rigidbody2D rigid;
    private bool Walking = false, WalkLeft = false, WalkRight = false, WalkUp = false, WalkDown = false;
    private float velocity = 0.35f;
    private Vector2 lastDirection = Vector2.down;
    private CircleCollider2D CirCol;
    public GameObject CurrentInterObject = null;
    public Interactivo CurrentInterObjectScript = null;
    private GameController GC;


    // Use this for initialization
    void Start () {
        GC = GameController.FindObjectOfType<GameController>();
        anim = this.GetComponent<Animator>();
        rigid = this.GetComponent<Rigidbody2D>();
        CirCol = GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (!Walking)
        {
            if (Input.GetKey(KeyCode.A) | WalkLeft)
            {
                lastDirection = Vector2.left;
                anim.SetInteger("Walk", 3);
                rigid.transform.Translate(Vector2.left * velocity * Time.deltaTime);
                CirCol.offset = new Vector2(-0.29f, 0);
            }
            else if (Input.GetKey(KeyCode.D) | WalkRight)
            {
                lastDirection = Vector2.right;
                anim.SetInteger("Walk", 0);
                rigid.transform.Translate(Vector2.right * velocity * Time.deltaTime);
                CirCol.offset = new Vector2(0.29f, 0);
            }
            else if (Input.GetKey(KeyCode.W) | WalkUp)
            {
                lastDirection = Vector2.up;
                anim.SetInteger("Walk", 1);
                rigid.transform.Translate(Vector2.up * velocity * Time.deltaTime);
                CirCol.offset = new Vector2(0, 0.29f);
            }
            else if (Input.GetKey(KeyCode.S) | WalkDown)
            {
                lastDirection = Vector2.down;
                anim.SetInteger("Walk", 2);
                rigid.transform.Translate(Vector2.down * velocity * Time.deltaTime);
                CirCol.offset = new Vector2(0, -0.29f);
            }
            else
            {
                anim.SetInteger("Walk", 4);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (CurrentInterObject != null)
                {
                    GameManager.LoadScene("MainTitle");
                }
            }
        }
    }

    public void MoveRightOn()
    {
        WalkRight = true;
    }
    public void MoveRightOff()
    {
        WalkRight = false;
    }
    public void MoveUptOn()
    {
        WalkUp = true;
    }
    public void MoveUpOff()
    {
        WalkUp = false;
    }
    public void MoveLeftOn()
    {
        WalkLeft = true;
    }
    public void MoveLeftOff()
    {
        WalkLeft = false;
    }
    public void MoveDownOn()
    {
        WalkDown = true;
    }
    public void MoveDownOff()
    {
        WalkDown = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CurrentInterObject = collision.gameObject;
            CurrentInterObjectScript = CurrentInterObject.GetComponent<Interactivo>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CurrentInterObject = null;
        }
    }
}
