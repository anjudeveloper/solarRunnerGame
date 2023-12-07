using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int extraJumps = 1;
    public Transform groundCheck;
    public LayerMask groundLayer;

    public float rollColliderHeight = -1.0f; // Height adjustment for rolling

    private Rigidbody2D rb;
    private bool isGrounded;
    private int extraJumpsLeft;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip JumpSound;

    private Vector2 fingerDownPosition;
    private Vector2 fingerUpPosition;
    private bool detectSwipeOnlyAfterRelease = true;

    private Collider2D playerCollider;
    private Vector2 normalColliderSize;

    private Vector2 startTouchPosition; // To track the start position of the swipe

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsLeft = extraJumps;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        playerCollider = GetComponent<Collider2D>();
        normalColliderSize = playerCollider.bounds.size;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, groundLayer);

        if (isGrounded)
        {
            extraJumpsLeft = extraJumps;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            fingerUpPosition = Input.GetTouch(0).position;
            fingerDownPosition = fingerUpPosition;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            fingerDownPosition = Input.GetTouch(0).position;
            DetectSwipe();
        }

        if (detectSwipeOnlyAfterRelease && Input.GetMouseButtonUp(0))
        {
            fingerDownPosition = Input.mousePosition;
            DetectSwipe();
        }

        // Trigger jump animation when jumping
        animator.SetBool("IsJumping", !isGrounded);
        // Detect rolling by swipe down or touch drag
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    break;

                case TouchPhase.Moved:
                    Vector2 touchDelta = touch.position - startTouchPosition;

                    // Check if swipe direction is downward and larger than a threshold
                    if (touchDelta.y < -50f)
                    {
                        StartRoll();
                    }
                    break;

                case TouchPhase.Ended:
                    DetectSwipe();
                    break;
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            startTouchPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - startTouchPosition;

            // Check if mouse swipe direction is downward and larger than a threshold
            if (mouseDelta.y < -50f)
            {
                StartRoll();
            }
            else
            {
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {
        if (SwipeDistanceCheckMet())
        {
            Vector2 direction = fingerDownPosition - fingerUpPosition;

            if (direction.y > 0)
            {
                Jump();
                audioSource.PlayOneShot(JumpSound);
            }
        }
    }

    bool SwipeDistanceCheckMet()
    {
        return fingerDownPosition.y - fingerUpPosition.y > 50f;
    }

    void Jump()
    {
        if (isGrounded || extraJumpsLeft > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            if (!isGrounded)
            {
                extraJumpsLeft--;
            }
        }
    }

    void StartRoll()
    {
        animator.SetBool("IsRolling", true); // Trigger the rolling animation
        // Assuming you're using a BoxCollider2D
        BoxCollider2D boxCollider = playerCollider as BoxCollider2D;
        if (boxCollider != null)
        {
            boxCollider.size = new Vector2(boxCollider.size.x, normalColliderSize.y + rollColliderHeight);
        }
        Invoke("StopRoll", 1.0f); // Assuming the rolling animation lasts for 1 second
    }

    void StopRoll()
    {
        animator.SetBool("IsRolling", false); // Stop the rolling animation
        // Assuming you're using a BoxCollider2D
        BoxCollider2D boxCollider = playerCollider as BoxCollider2D;
        if (boxCollider != null)
        {
            boxCollider.size = normalColliderSize; // Reset the collider size
        }
    }

}
