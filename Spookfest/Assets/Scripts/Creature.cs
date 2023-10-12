using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Creature : MonoBehaviour
{
    //public
    public float movement_speed = 2;
    public float untamed_movement_speed_debuff = .75f;
    public float panic_movement_speed = 4.5f;
    public float movement_slowing = 1;
    public float untamed_flee_distance = 5;
    public float untamed_panic_time = 3;
    //private
    private Rigidbody rb;
    private bool tamed = false;
    private bool panic_mode = false;    
    private bool[] directional_input = {false, false, false, false};
    private GameObject player;
    //feel free to add more boosts/perks these are all i could think of so far
    public string[] boosts = { "Health", "Speed", "Sneak", "Sight"};
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        //creature behavior
        if (tamed)
        {
            //tamed behavior
        }
        else
        {
            //untamed behavior
            Vector2 player_pos = player.transform.position;
            float player_distance = Vector2.Distance(player_pos, transform.position);
            bool player_above = player.transform.position.y > transform.position.y;
            bool player_right = player.transform.position.x > transform.position.x;
            //check if player is visible and close
            if (player_distance < untamed_flee_distance && player.GetComponent<Player_Movement>().visibility > -1)
            {
                panic_mode = true;
            }
            //while panicking
            if (panic_mode)
            {
                if (player_above)
                {
                    directional_input[2] = true;
                    directional_input[0] = false;
                }
                else
                {
                    directional_input[2] = false;
                    directional_input[0] = true;
                }
                if (player_right)
                {
                    directional_input[1] = true;
                    directional_input[3] = false;
                }
                else
                {
                    directional_input[1] = false;
                    directional_input[3] = true;
                }
            }
            //while passive
            else
            {
                //passive behavior
                directional_input[0] = false;
                directional_input[1] = false;
                directional_input[2] = false;
                directional_input[3] = false;
            }
        }
    }
    private void FixedUpdate()
    {
        float effected_movement_speed = movement_speed;
        //PANIC MODE
        if (panic_mode && untamed_panic_time <= 3 && untamed_panic_time >= 0)
        {
            effected_movement_speed = panic_movement_speed;
            untamed_panic_time -= Time.fixedDeltaTime;
        }
        else if (panic_mode && untamed_panic_time < 0)
        {
            panic_mode = false;
            untamed_panic_time = 3;
        }
        //UNTAMED MOVEMENT SPEED
        if (!tamed) effected_movement_speed -= untamed_movement_speed_debuff;
        //creature movement
        if (directional_input[3] == true) //right
        {
            float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
            rb.AddForce(new Vector2((rb.mass * effected_movement_speed - rb.mass * velocityInDirection) / Time.fixedDeltaTime, 0)); //Ft = Mv - Mu
        }
        else if (directional_input[1] == true) //left
        {
            float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
            rb.AddForce(new Vector2((rb.mass * -effected_movement_speed - rb.mass * velocityInDirection) / Time.fixedDeltaTime, 0)); //Ft = Mv - Mu
        }
        else if (rb.velocity.x != 0)
        {
            float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.right);
            rb.AddForce(new Vector2((0 - rb.mass * velocityInDirection) / Time.fixedDeltaTime * movement_slowing, 0)); //Ft = Mv - Mu
        }
        if (directional_input[0] == true) //up
        {
            float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.up);
            rb.AddForce(new Vector2(0, (rb.mass * effected_movement_speed - rb.mass * velocityInDirection) / Time.fixedDeltaTime)); //Ft = Mv - Mu
        }
        else if (directional_input[2] == true) //down
        {
            float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.up);
            rb.AddForce(new Vector2(0, (rb.mass * -effected_movement_speed - rb.mass * velocityInDirection) / Time.fixedDeltaTime)); //Ft = Mv - Mu
        }
        else if (rb.velocity.y != 0)
        {
            float velocityInDirection = Vector3.Dot(rb.velocity, Vector2.up);
            rb.AddForce(new Vector2(0, (0 - rb.mass * velocityInDirection) / Time.fixedDeltaTime * movement_slowing)); //Ft = Mv - Mu
        }
    }
    public void tame()
    {
        //make creature follow player 
        Player_Inventory.addCreature(gameObject);
        tamed = true;
    }
}
