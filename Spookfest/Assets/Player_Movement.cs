using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Player_Movement : MonoBehaviour
{
    //public 
    public float movement_speed = 1;
    public float movement_slowing = 1;
    public float jump_speed = 1;
    public float gravity_strength = 1;
    //private 
    private Rigidbody2D rb;
    private GroundCheck ground_check;
    private bool[] directional_input = {false,false,false,false}; //in order (up,left,down,right)
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ground_check = GetComponentInChildren<GroundCheck>();
        rb.gravityScale = gravity_strength;
    }

    // Update is called once per frame
    void Update()
    {
        //Input
        if (Input.GetKey(KeyCode.Space)) directional_input[0] = true; //Up
        else directional_input[0] = false;
        if (Input.GetKey(KeyCode.A)) directional_input[1] = true; //Left
        else directional_input[1] = false;
        if (Input.GetKey(KeyCode.S)) directional_input[2] = true; //Down
        else directional_input[2] = false;
        if (Input.GetKey(KeyCode.D)) directional_input[3] = true; //Right
        else directional_input[3] = false;
    }
    //FixedUpdate called every fixed framerate frame
    private void FixedUpdate()
    {
        if (ground_check.touching_ground == true)
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
    }
}
