using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPopup : MonoBehaviour
{
    public float minSpeed = 1.0f;
    public float maxSpeed = 5.0f;
    public float slowDown = 1.0f;

    public float maxSideMove = 0;

    public float sideShakeIntens = 0;

    public float lifetime = 1.0f;
    public bool isFading = true;

    Vector3 velocity;

    void Start()
    {
        velocity = new Vector3(1, 0, 0);
    }

    void Update()
    {
        transform.position += velocity * Time.deltaTime;
    }
}
