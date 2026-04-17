using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Used to store the users inputs to make the player move
    private Vector2 direction = Vector2.zero;

    private Rigidbody2D rb;

    public float max_speed = 5f;

    private Vector2 velocity;
    public float acceleration = 1.5f;
    public float friction = 0.75f;

    public float gravity = 0.5f;
    public float max_fall_speed = 9.8f;

    //Ground Checking
    public LayerMask mask;
    private RaycastHit2D left_ground_check;
    private RaycastHit2D right_ground_check;

    //Jumping
    private bool jump_check;
    public float jump_force = 15;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //set the input based on the player's key  pressures
        //horitzontal
        if (Input.GetKey(KeyCode.D))
        {
            direction.x = 1;
        }
        else if (Input.GetKey (KeyCode.A))
        {
            direction.x = -1;
        }
        else
        {
            direction.x = 0;
        }

        //verticle
        if (Input.GetKey(KeyCode.W))
        {
            direction.y = 1;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            direction.y = -1;
        }
        else
        {
            direction.y = 0;
        }

        //Make the direction vector have a length of 1 so the player moves at the correct speed
        direction = direction.normalized;

        if (Input.GetKey(KeyCode.Space))
        {
            jump_check = true;
        }
        else
        {
            jump_check = false;
        }
    }


    private void FixedUpdate()
    {
        //Acceleration Player
        velocity.x += acceleration * direction.x;

        //Cap the speed to max_speed
        velocity.x = Mathf.Clamp(velocity.x, -max_speed, max_speed);

        float ground_offset = 0;

        if (direction.x == 0)
        {
            //Make the velocity.x move to zero
            velocity.x = Mathf.MoveTowards(velocity.x, 0, friction);
        }

        //Perform the raycast
        left_ground_check = Physics2D.Raycast(rb.position + new Vector2(-0.5f, 0), Vector2.down, 1, mask);
        right_ground_check = Physics2D.Raycast(rb.position + new Vector2(0.5f, 0), Vector2.down, 1, mask);

        
        //If the player is touching the ground...
        if (left_ground_check || right_ground_check)
        {
            //Find the amount to move the player to the ground
            ground_offset = Mathf.Max(left_ground_check.distance, right_ground_check.distance) - 0.5f;

            velocity.y = 0;

            //Make the player move upwards since the player pressed the jump key
            if (jump_check)
            {
                velocity.y = jump_force;
            }
        }
        else //otherise they are in the air
        {
            //Add a downward force to the player
            velocity.y -= gravity;

            //Make sure the player cannot infinitly accerlate when falling
            velocity.y = Mathf.Max(velocity.y, -max_fall_speed);
        }

        //Move the character
        rb.MovePosition(rb.position + (velocity * Time.fixedDeltaTime) - new Vector2(0, ground_offset));
    }


    private void OnDrawGizmos()
    {
        //Visualize the ground check ray
        Gizmos.color = Color.red;
        Gizmos.DrawRay(GetComponent<Rigidbody2D>().position + new Vector2(-0.5f, 0), Vector2.down);
        Gizmos.DrawRay(GetComponent<Rigidbody2D>().position + new Vector2(0.5f, 0), Vector2.down);

    }
}
