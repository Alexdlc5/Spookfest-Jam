using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class face_camera : MonoBehaviour
{
    public bool is_still_object = true;
    public float active_distance = 30;
    public float offset;
    public GameObject camera;
    private SpriteRenderer sr;
    private GameObject player;
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sr = GetComponentInChildren<SpriteRenderer>();
        transform.rotation = Quaternion.Euler(-60, 0, 0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!is_still_object || Vector2.Distance(player.transform.position, transform.position) <= active_distance)
        {
            //get angle between mouse and object, rotate accordingly
            Vector3 targetPos = camera.transform.position - transform.position;
            float angle_yx = Mathf.Atan2(targetPos.y, -targetPos.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle_yx + offset, Vector3.right);
            sr.sortingOrder = -(int)(10 * transform.position.y);
        }
    }
}
