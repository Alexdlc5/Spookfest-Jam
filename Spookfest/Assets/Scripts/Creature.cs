using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Creature : MonoBehaviour
{
    //public
    public int type = 0;
    public float movement_speed = 2;
    public float tamed_movement_speed_debuff = .75f;
    public float panic_movement_speed = 4.5f;
    public float movement_slowing = 1;
    public float untamed_flee_distance = 5;
    public float untamed_panic_time = 3;
    public Animator anim;
    //private
    private Rigidbody rb;
    private bool tamed = false; 
    private bool movement_active_mode = false;    
    private bool[] directional_input = {false, false, false, false};
    private GameObject player;
    private AudioSource footsteps;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        footsteps = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        anim.SetBool("p", movement_active_mode);
        //creature behavior
        if (tamed)
        {
            //tamed behavior
            Vector2 player_pos = player.transform.position;
            bool[] player_direction = playerDirection(player_pos);
            float player_distance = Vector2.Distance(player_pos, transform.position);
            //check if player is far
            if (player_distance > untamed_flee_distance)
            {
                movement_active_mode = true;
            }
            else
            {
                movement_active_mode = false;
                directional_input[0] = false;
                directional_input[1] = false;
                directional_input[2] = false;
                directional_input[3] = false;
            }
            //while too far from player
            if (movement_active_mode)
            {
                transform.position = Vector3.MoveTowards(transform.position, player.transform.position, movement_speed * Time.deltaTime);
            }
        }
        else
        {
            //untamed behavior
            Vector2 player_pos = player.transform.position;
            //UDRL*
            bool[] player_direction = playerDirection(player_pos);
            float player_distance = Vector2.Distance(player_pos, transform.position);
            //check if player is visible and close
            if (player_distance < untamed_flee_distance && player.GetComponent<Player_Movement>().visibility > -1)
            {
                movement_active_mode = true;
            }
            //while panicking
            if (movement_active_mode)
            {
                if (player_direction[0])
                {
                    directional_input[2] = true;
                    directional_input[0] = false;
                }
                else if (player_direction[2])
                {
                    directional_input[2] = false;
                    directional_input[0] = true;
                }
                if (player_direction[1])
                {
                    directional_input[1] = true;
                    directional_input[3] = false;
  
                }
                else if (player_direction[3])
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
        //manage animations
        if (movement_active_mode)
        {
            transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            if (directional_input[3])
            {
                transform.localRotation = Quaternion.Euler(new Vector3(60, 0, 180));
            }
        }
        foreach (bool i in directional_input)
        {
            if (i)
            {
                footsteps.Play();
                break;
            }
            else
            {
                footsteps.Pause();
            }
        }
    }
    private void FixedUpdate()
    {
        float effected_movement_speed = movement_speed;
        //PANIC MODE
        if (!tamed && movement_active_mode && untamed_panic_time <= 3 && untamed_panic_time >= 0)
        {
            effected_movement_speed = panic_movement_speed;
            untamed_panic_time -= Time.fixedDeltaTime;
        }
        else if (!tamed && movement_active_mode && untamed_panic_time < 0)
        {
            movement_active_mode = false;
            untamed_panic_time = 3;
        }
        //TAMED MOVEMENT SPEED
        if (tamed) effected_movement_speed -= tamed_movement_speed_debuff;
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
    private bool[] playerDirection(Vector2 player_pos)
    {
        bool player_above = player_pos.y > transform.position.y;
        bool player_right = player_pos.x > transform.position.x;
        bool player_below = player_pos.y < transform.position.y;
        bool player_left = player_pos.x < transform.position.x;
        return new bool[]{player_above, player_below, player_right, player_left};
    }
    private float[] playerDistanceInDirection(Vector2 player_pos)
    {
        float player_above = Mathf.Abs(player_pos.y - transform.position.y);
        float player_right = Mathf.Abs(player_pos.x - transform.position.x);
        float player_below = Mathf.Abs(player_pos.y - transform.position.y);
        float player_left = Mathf.Abs(player_pos.x - transform.position.x);
        return new float[] {player_above, player_below, player_right, player_left };
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && !tamed)
        {
            //make creature follow player 
            player.GetComponent<Player_Movement>().creatures_caught[type] += 1;
            tamed = true;
        }
    }
}
