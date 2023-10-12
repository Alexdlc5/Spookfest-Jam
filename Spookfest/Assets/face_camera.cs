using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class face_camera : MonoBehaviour
{
    public float offset;
    public GameObject camera;
    private SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    // Update is called once per frame
    void Update()
    {
        //get angle between mouse and object, rotate accordingly
        Vector3 targetPos = camera.transform.position - transform.position;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle + offset, Vector3.right);

        sr.sortingOrder = -(int)(10 * transform.position.y);
    }
}
