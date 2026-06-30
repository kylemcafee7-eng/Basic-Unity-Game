using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    // 1
    public Vector3 camOffset = new Vector3(0f, 1.2f, -2.6f);

    // 2
    private Transform target;

    // Start is called before the first frame update
    void Start()
    {
        // 3
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 5
        this.transform.position = target.TransformPoint(camOffset);

        // 6
        this.transform.LookAt(target);
    }
}
