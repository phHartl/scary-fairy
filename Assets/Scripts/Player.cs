using UnityEngine;
using UnityEngine.UI;

public class Player : MovingObj
{
    public float maxVerticalDistance = 8.0f;
    public float maxHorizontalDistance = 12.0f;
    public float currentDistance;
    private GameObject[] players = new GameObject[2];
    private Vector2 horizontalMovement;
    private Vector2 verticalMovement;
    private string axisVertical;
    private string axisHorizontal;
    private Vector2 lastMove;
    protected int currentDir;
    // This gameObject has to be set in every classes prefab within the Unity inspector
    public GameObject nextClassPrefab;
    public Sprite WarriorPortrait;
    public Sprite RangerPortrait;
    public GameObject PortraitSpritePlayer1;
    public GameObject PortraitSpritePlayer2;

    // Use this for initialization
    void Start()
    {
        base.Start();
        players[0] = GameObject.FindGameObjectWithTag("Player1");
        players[1] = GameObject.FindGameObjectWithTag("Player2");
        SetAxis();
        animator = GetComponent<Animator>();
    }

    protected void SetAxis()
    {
        if (gameObject.tag == "Player1")
        {
            axisVertical = "Vertical";
            axisHorizontal = "Horizontal";
        } else if (gameObject.tag == "Player2")
        {
            axisVertical = "Verticalp2";
            axisHorizontal = "Horizontalp2";
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
    }

     protected override void Update ()
    {
        animator.SetFloat("MoveX", Input.GetAxisRaw(axisHorizontal));
        animator.SetFloat("MoveY", Input.GetAxisRaw(axisVertical));
        animator.SetBool("PlayerMoving", isMoving);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
        animator.SetBool("PlayerAttack", isAttacking);
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

    /**
     * This method is used to distinguish between the two players and 
     * to give them seperate inputs ("r"-key for Player1 and "0" for Player2)
     */
    protected void ChangeClassInput()
    {
        if (gameObject.tag == "Player1")
        {
            if (Input.GetKeyDown("r"))
            {
                ChangeClass();
                ChangePortrait();
            }
        } else if (gameObject.tag == "Player2")
        {
            if (Input.GetKeyDown("0"))
            {
                ChangeClass();
                ChangePortrait();
            }
        }
    }

    /*
     * This method exchanges the GameObject the script is attached to(Player1 or Player2) with
     * an instance of the prefab that has been set as the nextClassPrefab(Warrior/Ranger/Fairy) in the Unity inspector.
     */
    protected void ChangeClass()
    {
        // Setting the correct player tag
        nextClassPrefab.tag = gameObject.tag;
        // Instanzietes the new GameObject
        GameObject newObject = Instantiate(nextClassPrefab,
            gameObject.transform.position,
            gameObject.transform.rotation,
            gameObject.transform.parent) as GameObject;

        Destroy(this.gameObject);

        
    }

    
    protected void ChangePortrait() 
    {


    }
}
