using System.Collections;
using UnityEngine;

public class Player : MovingObj
{
    public float maxVerticalDistance = 8.0f;
    public float maxHorizontalDistance = 12.0f;
    public float currentDistance;
    private Vector2 horizontalMovement;
    private Vector2 verticalMovement;
    public string axisVertical;
    public string axisHorizontal;
    [HideInInspector]
    public Vector2 lastMove;
    protected int baseDamage;
    protected int currentDir; // Current facing direction north(1), east(2), south(3), west(4)
    // These GameObjects have to be set in every classes prefab within the Unity inspector
    public GameObject nextClassPrefab;
    public GameObject alternativePrefab;
    public Sprite WarriorPortrait;
    public Sprite RangerPortrait;
    public GameObject PortraitSpritePlayer1;
    public GameObject PortraitSpritePlayer2;

    // Use this for initialization
    protected void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        animator.SetFloat("MoveX", Input.GetAxisRaw(axisHorizontal));
        animator.SetFloat("MoveY", Input.GetAxisRaw(axisVertical));
        animator.SetBool("PlayerMoving", isMoving);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("PlayerAttack", isAttacking);
        //animator.SetBool("IceEnchantment", iceEnchantment);
        //animator.SetBool("FireEnchantment", fireEnchantment);
    }

    protected override Vector2 Move()
    {
        isMoving = false;
        horizontalMovement = new Vector2(0, 0);
        verticalMovement = new Vector2(0, 0);
        float axisH = Input.GetAxisRaw(axisHorizontal);
        float axisV = Input.GetAxisRaw(axisVertical);
        float nextDistance;

        if (axisH > 0.5f || axisH < -0.5f)
        {
            currentDistance = rb2D.position.x - CameraControl.camPosition.x;
            nextDistance = Vector2.Distance(rb2D.position + new Vector2(axisH * moveSpeed * Time.deltaTime, 0),
                CameraControl.camPosition);

            if (Mathf.Abs(currentDistance) < maxHorizontalDistance || (currentDistance > 0 && axisH < 0) || (currentDistance < 0 && axisH > 0))
            {
                horizontalMovement = new Vector2(axisH * moveSpeed * Time.deltaTime, 0);
                isMoving = true;
                lastMove = new Vector2(axisH, 0);
                if (axisH > 0)
                {
                    currentDir = 2;
                }
                else
                {
                    currentDir = 4;
                }
            }
        }

        if (axisV > 0.5f || axisV < -0.5f)
        {
            currentDistance = rb2D.position.y - CameraControl.camPosition.y;
            //movement possible if player below distance threshold or moving towards other player
            if (Mathf.Abs(currentDistance) < maxVerticalDistance || (currentDistance > 0 && axisV < 0) || (currentDistance < 0 && axisV > 0))
            {
                verticalMovement = new Vector2(0, axisV * moveSpeed * Time.deltaTime);
                isMoving = true;
                lastMove = new Vector2(0, axisV);
                if (axisV > 0)
                {
                    currentDir = 1;
                }
                else
                {
                    currentDir = 3;
                }
            }
        }
        newPos = rb2D.position + horizontalMovement + verticalMovement;
        Debug.DrawLine(rb2D.position, rb2D.position + lastMove);
        return newPos;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking == true && other.CompareTag("CasualEnemy"))
        {
            CasualEnemy ce = other.GetComponent<CasualEnemy>();
            ce.applyDamage(_damage);
            if (iceEnchantment)
            {
                print("IceEnchanted Attack");
            }
            if (fireEnchantment)
            {
                print("FireEnchanted Attack");
            }
            if (!iceEnchantment && !fireEnchantment)
            {
                print("normal Attack");
            }
        }
    }

    public void AttemptAttack()
    {
        if (!isOnCoolDown)
        {
            StartCoroutine(Attack()); //Coroutines don't need to be finished within the updateframe
        }
    }

    protected virtual IEnumerator Attack()
    {
        return null;
    }

    protected void CheckForEnchantment()
    {
        _damage = baseDamage;
        if (iceEnchantment)
        {
            _damage = baseDamage * 2;
        }
        else if (fireEnchantment)
        {
            _damage = baseDamage * 3;
        }
    }

    /*
     * This method exchanges the GameObject the script is attached to (Player1 or Player2) with
     * an instance of the prefab that has been set as the nextClassPrefab(Warrior/Ranger/Fairy) in the Unity inspector.
     * It afterwards sets the new player GameObject as the new target of the camera.
     */
    public void ChangeClass(int index)
    {
        GameObject otherPlayer;
        if (index == 0)
        {
            otherPlayer = GameObject.FindGameObjectWithTag("Player2");
        } else
        {        
            otherPlayer = GameObject.FindGameObjectWithTag("Player1");
        }

        // This prohibits both players being a fairy
        if(nextClassPrefab.name == "fairy" && otherPlayer.GetComponent<Fairy>())
        {
            nextClassPrefab = alternativePrefab;
        }

        // Setting the correct player tag
        nextClassPrefab.tag = gameObject.tag;

        // Instanzietes the new GameObject
        Destroy(this.gameObject);
        GameObject newPlayer = Instantiate(nextClassPrefab,
            gameObject.transform.position,
            gameObject.transform.rotation,
            gameObject.transform.parent) as GameObject;

        if (otherPlayer.GetComponent<Fairy>())
        {
            // The other player is a fairy and the target needs to be set
            otherPlayer.GetComponent<Fairy>().target = newPlayer.GetComponent<MovingObj>();
        }

        if (nextClassPrefab.name == "fairy") { //Quick and dirty method - should be down better later
            if(index == 0)
            {
               newPlayer.GetComponent<Fairy>().target = GameObject.FindGameObjectWithTag("Player2").GetComponent<MovingObj>();
            }
            if(index == 1)
            {
                newPlayer.GetComponent<Fairy>().target = GameObject.FindGameObjectWithTag("Player1").GetComponent<MovingObj>();
            }
        }

        // Setting the new player as the new target of the camera
        GameObject cameraRig = GameObject.Find("CameraRig");
        cameraRig.GetComponent<CameraControl>().SetTarget(index, newPlayer);
        ChangePortrait();
    }


    protected void ChangePortrait()
    {
    }
}
