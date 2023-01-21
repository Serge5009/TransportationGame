using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPopup : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 2.0f;
    public float slowDown = 1.0f;

    public float maxSideMove = 0.2f;

    public float sideShakeIntens = 0;

    public float minLifetime = 1.0f;
    public float maxLifetime = 2.0f;
    float lifetime;

    public bool isFading = true;

    Vector3 velocity;

    void Start()
    {
        lifetime = Random.Range(minLifetime, maxLifetime);

        float x = Random.Range(-maxSideMove, maxSideMove);
        float y = Random.Range(minSpeed, maxSpeed);

        velocity = new Vector3(x, y, 0);    //
    }

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);


        transform.position += velocity * Time.deltaTime;
    }
}
