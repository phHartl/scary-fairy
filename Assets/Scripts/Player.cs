using System.Collections;
using UnityEngine;

public class Player : MovingObj
{
    public float maxVerticalDistance = 8.0f;
    public float maxHorizontalDistance = 12.0f;
    public float currentDistance;
    private GameObject[] players = new GameObject[2];
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
        _hitpoints = 100;
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
        SetAxis();
        animator = GetComponent<Animator>();
    }

    /*
     * This method assigns the controll axis for the player according to their 
     * corresponding tags (Player1/Player2).
     * The controll-axis are set in the input settings (Edit -> Project Settings -> Input).
     */
    protected void SetAxis()
    {
        if (gameObject.tag == "Player1")
        {
            axisVertical = "Vertical";
            axisHorizontal = "Horizontal";
        }
        else if (gameObject.tag == "Player2")
        {
            axisVertical = "Verticalp2";
            axisHorizontal = "Horizontalp2";
        }
    }

    protected void FixedUpdate()
    {
        rb2D.MovePosition(Move());
    }

    // Update is called once per frame
    protected override void Update()
    {
        animator.SetFloat("MoveX", Input.GetAxisRaw(axisHorizontal));
        animator.SetFloat("MoveY", Input.GetAxisRaw(axisVertical));
        animator.SetBool("PlayerMoving", isMoving);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("PlayerAttack", isAttacking);
        animator.SetInteger("Hitpoints", _hitpoints);
        //animator.SetBool("IceEnchantment", iceEnchantment);
        //animator.SetBool("FireEnchantment", fireEnchantment);
        if(_hitpoints <= 0)
        {
            Subject.Notify("Player died"); //Notify gamemanager and reload level see gamemanger
        }
        ChangeClassInput();
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
    //Overrides applyDamage in MovingObj, player gets invincible for 0.5 seconds if hit by an enemy
    public override void applyDamage(int damage)
    {
        if (!isInvincible)
        {
            base.applyDamage(damage);
            StartCoroutine(playerInvincible());
        }
    }

       IEnumerator playerInvincible()
    {
        isInvincible = true;
        setPlayerTransparency(0.5f); // 50% transparent
        yield return new WaitForSeconds(0.5f);
        setPlayerTransparency(1.0f);
        isInvincible = false;
    }


    //This methodes makes the player transparent, input variable is transparency in percent
    private void setPlayerTransparency(float alpha)
    {
        gameObject.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
    }
    /**
     * This method is used to distinguish between the two players and 
     * to give them seperate inputs ("0"-key for Player1 and "r"-key for Player2)
     */
    protected void ChangeClassInput()
    {
        if (gameObject.tag == "Player1")
        {
            if (Input.GetKeyDown("0"))
            {
                ChangeClass(0);
                ChangePortrait();
            }
        }
        else if (gameObject.tag == "Player2")
        {
            if (Input.GetKeyDown("r"))
            {
                ChangeClass(1);
                ChangePortrait();
            }
        }
    }

    protected void checkForEnchantment()
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
    private void ChangeClass(int index)
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
            otherPlayer.GetComponent<Fairy>().buff = newPlayer.GetComponentInChildren<ParticleSystem>();
        }

        if (nextClassPrefab.name == "fairy") { //Quick and dirty method - should be down better later
            if(index == 0)
            {
               newPlayer.GetComponent<Fairy>().target = GameObject.FindGameObjectWithTag("Player2").GetComponent<MovingObj>();
                newPlayer.GetComponent<Fairy>().buff = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<ParticleSystem>();

            }
            if(index == 1)
            {
                newPlayer.GetComponent<Fairy>().target = GameObject.FindGameObjectWithTag("Player1").GetComponent<MovingObj>();
                newPlayer.GetComponent<Fairy>().buff = GameObject.FindGameObjectWithTag("Player1").GetComponentInChildren<ParticleSystem>();
            }
        }

        // Setting the new player as the new target of the camera
        GameObject cameraRig = GameObject.Find("CameraRig");
        cameraRig.GetComponent<CameraControl>().SetTarget(index, newPlayer);
    }


    protected void ChangePortrait()
    {
    }
}
