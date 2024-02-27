using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostManager : MonoBehaviour
{

    public GameObject Ghost;
    public GameObject Player1;
    public GameObject Player2;
    public Rigidbody2D RB;
    public GameController GameController;
    public Animator Animator;

    public bool IsDead = false;
    public int Frightened = 0;
    protected float DefaultSpeed;
    protected float ActualSpeed;

    float ShortPath;    // za merjenje, kam it na kriziscu, kera je najkrajsa pot
    float LongPath;     // enako kot zgoraj, vendar za daljso pot (frightened mode)
    float BlinkingTime = 2f;
    Vector3 SpawnPoint;

    protected Vector2 Dest;
    protected Vector2 Dir = Vector2.up;
    Vector2 TempDir;

    void Awake()
    {
    
        SpawnPoint = Ghost.transform.position;
        Dest = (Vector2)Ghost.transform.position;
        switch (OptionsController.Difficulty)
        {

            case 0:
                DefaultSpeed = 0.06f;
                break;
            case 1:
                DefaultSpeed = 0.07f;
                break;
            case 2:
                DefaultSpeed = 0.08f;
                break;

        }

    }

    protected void Dead() {

        ResetPosition();
        ResetDestination();

        Animator.SetBool("IsDead", true);
        if (!GameController.GetComponent<GameController>().Freeze)
        {
            Ghost.GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(Blink());
        }

    }

    protected void Chase(Vector2 Target) {

        ShortPath = 0f;
        TempDir = Dir;

        if (CheckRay(Vector2.up) && TempDir != Vector2.down)    // drug pogoj: da ne gre nazaj v smer, iz katere je prišel, enako pri ostalih 3eh if stavkih
        {

            ShortPath = Vector2.Distance((Vector2)Ghost.transform.position + Vector2.up, Target);
            Dir = Vector2.up;

        }

        if (CheckRay(Vector2.down) && ((Vector2.Distance((Vector2)Ghost.transform.position + Vector2.down, Target) < ShortPath) || ShortPath == 0f) && TempDir != Vector2.up)
        {

            ShortPath = Vector2.Distance((Vector2)Ghost.transform.position + Vector2.down, Target);
            Dir = Vector2.down;

        }

        if (CheckRay(Vector2.left) && ((Vector2.Distance((Vector2)Ghost.transform.position + Vector2.left, Target) < ShortPath) || ShortPath == 0f) && TempDir != Vector2.right)
        {

            ShortPath = Vector2.Distance((Vector2)Ghost.transform.position + Vector2.left, Target);
            Dir = Vector2.left;

        }

        if (CheckRay(Vector2.right) && ((Vector2.Distance((Vector2)Ghost.transform.position + Vector2.right, Target) < ShortPath) || ShortPath == 0f) && TempDir != Vector2.left)
        {

            Dir = Vector2.right;

        }

        Dest += Dir;

    }

    protected void MoveFrightened(GameObject Player)
    {

        LongPath = 0f;
        TempDir = Dir;

        if (CheckRay(Vector2.up) && TempDir != Vector2.down)
        {

            LongPath = Vector2.Distance((Vector2)Ghost.transform.position + Vector2.up, (Vector2)Player.transform.position);
            Dir = Vector2.up;

        }

        if (CheckRay(Vector2.down) && ((Vector2.Distance((Vector2)Ghost.transform.position + Vector2.down, (Vector2)Player.transform.position) > LongPath) || LongPath == 0f) && TempDir != Vector2.up)
        {

            LongPath = Vector2.Distance((Vector2)Ghost.transform.position + Vector2.down, (Vector2)Player.transform.position);
            Dir = Vector2.down;

        }

        if (CheckRay(Vector2.left) && ((Vector2.Distance((Vector2)Ghost.transform.position + Vector2.left, (Vector2)Player.transform.position) > LongPath) || LongPath == 0f) && TempDir != Vector2.right)
        {

            LongPath = Vector2.Distance((Vector2)Ghost.transform.position + Vector2.left, (Vector2)Player.transform.position);
            Dir = Vector2.left;

        }

        if (CheckRay(Vector2.right) && ((Vector2.Distance((Vector2)Ghost.transform.position + Vector2.right, (Vector2)Player.transform.position) > LongPath) || LongPath == 0f) && TempDir != Vector2.left)
        {

            Dir = Vector2.right;

        }

        Dest += Dir;

    }

    protected bool CheckRay(Vector2 Direction)
    {

        RaycastHit2D t = Physics2D.Linecast((Vector2)Ghost.transform.position + Direction, (Vector2)Ghost.transform.position);

        if (t.collider != null) return !(t.collider.tag == "Wall");        //true = prazna pot, false = stena
        else return true;

    }

    protected bool CheckForWall(GameObject Player, Vector2 Direction)
    {      // ce je wall 2 enoti (ali manj) pred pacmanom -> false ; ce je prosto, ali je drugo pred njim -> true

        RaycastHit2D t1 = Physics2D.Linecast((Vector2)Player.transform.position + (Direction / 2), (Vector2)Player.transform.position + (Direction * 2));
        RaycastHit2D t2 = Physics2D.Linecast((Vector2)Player.transform.position + (Direction * 2), (Vector2)Player.transform.position);

        if (t1.collider != null && t2.collider != null)
        {

            if (t1.collider.tag != "Wall" && t2.collider.tag != "Wall") return true;
            return false;

        }

        else if (t1.collider == null && t2.collider != null)
        {

            if (t2.collider.tag != "Wall") return true;
            return false;

        }

        else if (t1.collider != null && t2.collider == null)
        {

            if (t1.collider.tag != "Wall") return true;
            return false;

        }

        return true;    //else ni potreben

    }

    protected bool CheckForPlayer(Vector2 Direction)
    {

        RaycastHit2D hit = Physics2D.Linecast((Vector2)Ghost.transform.position + (Direction / 2), (Vector2)Ghost.transform.position + (Direction * 2));

        if (hit.collider != null) return (hit.collider.tag == "Player" && hit.distance != 0);   // true = pacman je v bližini, false = ni

        return false;

    }

    protected void WorldLoop()
    {

        if (Ghost.transform.position.x > GameController.WorldWidth)
        {

            Ghost.transform.Translate(new Vector2(-2*GameController.WorldWidth, 0f));
            Dest.x = -GameController.WorldWidth + 0.5f;

        }

        else if (Ghost.transform.position.x < -GameController.WorldWidth)
        {

            Ghost.transform.Translate(new Vector2(2*GameController.WorldWidth, 0f));
            Dest.x = GameController.WorldWidth - 0.5f;

        }

    }

    protected Vector2 MirrorVector(Vector2 a, Vector2 b)
    {    // vektor a preslika cez vektor b in vrne koordinate novega vektorja (tocke)

        float newx = b.x * 2 - a.x;
        float newy = b.y * 2 - a.y;

        return (new Vector2(newx, newy));

    }

    protected bool Player2Closer() {

        if (Vector2.Distance(Ghost.transform.position, Player2.transform.position) < Vector2.Distance(Ghost.transform.position, Player1.transform.position)) return true;
        return false;

    }

    IEnumerator Blink()
    {

        yield return new WaitForSecondsRealtime(BlinkingTime);
        Ghost.GetComponent<BoxCollider2D>().enabled = true;
        IsDead = false;
        Frightened = 0;
        TurnAlive();

    }

    protected void CheckFrightened()
    {

        if (Frightened == 0) Animator.SetInteger("IsFrightened", 0);
        else if (Frightened == 1) Animator.SetInteger("IsFrightened", 1);
        else if (Frightened == 2) Animator.SetInteger("IsFrightened", 2);

    }

    protected void CheckDeath()
    {

        if (GameController.GetComponent<GameController>().Freeze) ActualSpeed = 0f;
        else ActualSpeed = DefaultSpeed;

    }

    public void TurnAlive()
    {

        Animator.SetBool("IsDead", false);

    }

    void ResetPosition() {

        Ghost.transform.position = SpawnPoint;

    }

    void ResetDestination() {

        Dest = (Vector2) Ghost.transform.position;

    }

}