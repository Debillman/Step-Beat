using UnityEngine;
using UnityEngine.InputSystem; // 새 입력 시스템 네임스페이스 추가

public class Player : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    private bool isTurn = false;
    private Vector3 startPosition;
    private Vector3 oldPosition;

    private int moveCnt = 0;
    private int TurnCnt = 0;
    private int SpawnCnt = 0;

    private bool isDie = false;

    private AudioSource sound;

    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();

        startPosition = transform.position;
        Init();
    }

    private void Init()
    {
        anim.SetBool("Die", false);
        transform.position = startPosition;
        oldPosition = startPosition;
        moveCnt = 0;
        TurnCnt = 0;
        SpawnCnt = 0;
        isTurn = false;
        spriteRenderer.flipX = isTurn;
        isDie = false;
    }

    public void CharTurn()
    {
        isTurn = !isTurn;
        spriteRenderer.flipX = isTurn;
    }

    public void CharMove()
    {
        if (isDie)
            return;

        sound.Play();

        moveCnt++;

        MoveDirection();

        if(isFailTurn())
        {
            CharDie();
            return;
        }

        if(moveCnt > 5)
        {
            RespawnStair();
        }

        GameManager.Instance.AddScore();
    
    }

    private void MoveDirection()
    {
        if (isTurn) //left 
        {
            oldPosition += new Vector3(-0.75f, 0.5f, 0);
        }
        else
        {
            oldPosition += new Vector3(0.75f, 0.5f, 0);
        }

        transform.position = oldPosition;
        anim.SetTrigger("Move");
    }

    private bool isFailTurn()
    {
        bool resurt = false;

        if (GameManager.Instance.isTurn[TurnCnt] != isTurn)
        {
            resurt = true;
        }

        TurnCnt++;

        if(TurnCnt > GameManager.Instance.Stairs.Length - 1)
        {
            TurnCnt = 0;
        }
        return resurt;
    }

    private void RespawnStair()
    {
        GameManager.Instance.SpawnStair(SpawnCnt);

        SpawnCnt++;

        if(SpawnCnt > GameManager.Instance.Stairs.Length -1)
        {
           SpawnCnt = 0;
        }
    }

    private void CharDie()
    {
        GameManager.Instance.GameOver();
        anim.SetBool("Die", true);
        isDie = true;
    }

    public void ButtonRestart()
    {
        Init();
        GameManager.Instance.Init();
        GameManager.Instance.InitStairs();
    }
}