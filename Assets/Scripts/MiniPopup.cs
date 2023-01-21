using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPopup : MonoBehaviour
{
    public float minSpeed = 0.5f;
    public float maxSpeed = 2.0f;
    public float slowDown = 0.1f;   //  Limit between 0 and 1

    public float maxSideMove = 0.2f;

    public float sideShakeIntens = 0;
    float shakeInterval;
    float shakeIntervalTimer;
    bool isShakingRight;

    public float minLifetime = 1.0f;
    public float maxLifetime = 2.0f;
    float lifetime;

    public bool isFading = true;    //  TO DO: implement fading

    Vector3 velocity;

    void Start()
    {
        //  Liftime
        lifetime = Random.Range(minLifetime, maxLifetime);
        //  implement fading here

        //  Initial speed
        float x = Random.Range(-maxSideMove, maxSideMove);
        float y = Random.Range(minSpeed, maxSpeed);
        velocity = new Vector3(x, y, 0);

        //  Shaking
        if (sideShakeIntens == 0.0f)    //  Ignore if there's no shake
            return;
        int shakes = Random.Range(2, 4);    //  How many direction changes we're gonna get
        shakeInterval = lifetime / shakes;
        shakeIntervalTimer = shakeInterval;
        isShakingRight = (Random.value > 0.5f); //  If first shake to the right or left
    }

    void Update()
    {
        //  Lifetime reduction
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);
        shakeIntervalTimer -= Time.deltaTime;
        if(shakeIntervalTimer <= 0)
        {
            isShakingRight = !isShakingRight;   //  Inverse shake
            shakeIntervalTimer = shakeInterval;
        }

        //  Slow down
        velocity -= velocity * slowDown * Time.deltaTime;

        //  Move
        transform.position += velocity * Time.deltaTime;

        //  Shake
        float shakeMove = sideShakeIntens;
        if (!isShakingRight)
            shakeMove *= -1;
        Vector3 shakeVec = new Vector3(shakeMove, 0, 0);
        transform.position += shakeVec * Time.deltaTime;
    }
}
