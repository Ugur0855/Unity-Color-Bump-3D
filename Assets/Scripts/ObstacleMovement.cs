using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : MonoBehaviour
{
    [SerializeField] public float speedValue = 10f;

    public float MaxPosX = 4f;
    public float MinPosX = 0.5f;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Mathf.PingPong(Time.time * speedValue, MaxPosX) + MinPosX, transform.position.y, transform.position.z);
    }
}
