using UnityEngine;


public class PlayerMovement : MonoBehaviour
{

    CharacterController controller;
    public float speed = 10f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Vector3 velocity;
    public bool isGrounded;

    public float gravity = -9.81f;

    public float jumpHeight;

    [Header("Jump")]
    public float fallMultiplier = 2.5f;
    public float lowJumpModifier = 2f;

    public float interactRadius = 1000f;




    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        
        if (velocity.y < 0)
        {
            if (isGrounded)
            {
                // keep some so we don't hover
                velocity.y = -2f;
            } else
            {

                    velocity.y = velocity.y + gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
        } else if(velocity.y > 0 && !Input.GetButton("Jump")) {
            velocity.y = velocity.y + gravity * (lowJumpModifier - 1) * Time.deltaTime;
        }

        //Jump stuff
        

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += (gravity * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log("pressed interact");
            Collider[] hits = Physics.OverlapSphere(transform.position, interactRadius);
            foreach (Collider hit in hits) {
                Debug.Log(hit.transform.name);
                if (hit.gameObject.GetComponent<IInteractable>() != null) {
                    hit.gameObject.GetComponent<IInteractable>().interact(gameObject);

                    break;
                }
            }
        }
    }
}
