using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player_Movement : MonoBehaviour
{
    //public 
    public bool is_platformer_movement = true;
    public float movement_speed = 1;
    public float crouched_movement_speed = 1;
    public float movement_slowing = 1;
    public float jump_speed = 1;
    public float gravity_strength = 1;
    public bool is_sneaking = false;
    public float groundDist;
    public LayerMask terrainLayer;
    //private 
    private Rigidbody rb;
    private GroundCheck ground_check;
    private bool[] directional_input = {false,false,false,false,false}; //in order (up,left,down,right)

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        if (Input.GetKey(KeyCode.Space) && is_platformer_movement || Input.GetKey(KeyCode.W) && !is_platformer_movement) directional_input[0] = true; //Up
        else directional_input[0] = false;
        if (Input.GetKey(KeyCode.A)) directional_input[1] = true; //Left
        else directional_input[1] = false;
        if (Input.GetKey(KeyCode.S)) directional_input[2] = true; //Down 
        else directional_input[2] = false;
        if (Input.GetKey(KeyCode.D)) directional_input[3] = true; //Right
        else directional_input[3] = false;
        if (Input.GetKey(KeyCode.LeftShift)) directional_input[4] = true; //Down 
        else directional_input[4] = false;

        //Raycast in order to maintain set distance above terrain :p
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        if(Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }
    }
    //FixedUpdate called every fixed framerate frame
    private void FixedUpdate()
    {
        //PLATFORMER MOVEMENT
        if (is_platformer_movement && ground_check.touching_ground == true)
        {
            if (directional_input[3] == true) //right
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
                rb.AddForce(new Vector2((rb.mass * movement_speed - rb.mass * velocityInDirection) / Time.fixedDeltaTime, 0)); //Ft = Mv - Mu
            }
            if (directional_input[1] == true) //left
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
                rb.AddForce(new Vector2((rb.mass * -movement_speed - rb.mass * velocityInDirection) / Time.fixedDeltaTime, 0)); //Ft = Mv - Mu
            }
            if (directional_input[0] == true) //up
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.up);
                rb.AddForce(new Vector2(0, (rb.mass * jump_speed - rb.mass * velocityInDirection) / Time.fixedDeltaTime)); //Ft = Mv - Mu
            }
        }
        //TOP DOWN MOVEMENT
        if (!is_platformer_movement)
        {
            float effected_movement = movement_speed;
            if (directional_input[4])
            {
                effected_movement = crouched_movement_speed;
                is_sneaking = true;
            }
            else
            {
                is_sneaking = false;
            }

            if (directional_input[3] == true) //right
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
                rb.AddForce(new Vector2((rb.mass * effected_movement - rb.mass * velocityInDirection) / Time.fixedDeltaTime, 0)); //Ft = Mv - Mu
            }
            else if (directional_input[1] == true) //left
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
                rb.AddForce(new Vector2((rb.mass * -effected_movement - rb.mass * velocityInDirection) / Time.fixedDeltaTime, 0)); //Ft = Mv - Mu
            }
            else if (rb.velocity.x != 0)
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
                rb.AddForce(new Vector2((0 - rb.mass * velocityInDirection) / Time.fixedDeltaTime * movement_slowing, 0)); //Ft = Mv - Mu
            }
            if (directional_input[0] == true) //up
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.up);
                rb.AddForce(new Vector2(0, (rb.mass * effected_movement - rb.mass * velocityInDirection) / Time.fixedDeltaTime)); //Ft = Mv - Mu
            }
            else if (directional_input[2] == true) //down
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.up);
                rb.AddForce(new Vector2(0, (rb.mass * -effected_movement - rb.mass * velocityInDirection) / Time.fixedDeltaTime)); //Ft = Mv - Mu
            }
            else if (rb.velocity.y != 0)
            {
                float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.up);
                rb.AddForce(new Vector2(0, (0 - rb.mass * velocityInDirection) / Time.fixedDeltaTime * movement_slowing)); //Ft = Mv - Mu
            }
        }
    }
}
