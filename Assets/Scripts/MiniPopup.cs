using UnityEngine;

//  Attach this script to an object (particle, popup, effect) to make it move up with adjustable slow down and side movements
//  Includes periodical moves like shake and smooth shake and allows randomization
//  Created by Serhii Marchenko    https://www.linkedin.com/in/serhiimarchenko/ for the https://github.com/Serge5009/TransportationGame
//  There will be more changes later ;)

//  Resources used:
//  MathF Sin/Cos https://docs.unity3d.com/ScriptReference/Mathf.Sin.html
//  Tidy random bool https://forum.unity.com/threads/random-randomboolean-function.83220/

public class MiniPopup : MonoBehaviour
{
    //  Vertical start settings
    public float minSpeed = 0.5f;
    public float maxSpeed = 2.0f;
    public float slowDown = 0.1f;   //  Limit between 0 and 1

    //  Horizontal start settings
    public float maxSideMove = 0.2f;

    //  Shake periodic settings
    public float sideShakeIntens = 0;
    public bool isSmoothShake = false;
    float shakeInterval;
    float shakeIntervalTimer;
    bool isShakingRight;

    //  Lifetime settings
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
        if (sideShakeIntens == 0.0f)    //  Ignore if there's no shake
            return;

        float shakeMove = sideShakeIntens;

        if (isSmoothShake)  //  If the shake is smooth - multiply the speed by SIN of the interval
        {
            float smoothFactor = Mathf.Sin(shakeIntervalTimer / shakeInterval * Mathf.PI);  //  Get the multiplier depending on what part of the interval we're in, start - 0, middle - 1, end - 0
            shakeMove *= smoothFactor;                                                      //  Apply smoothing multiplier
        }

        if (!isShakingRight)    //  Direction inversion
            shakeMove *= -1;
                
        Vector3 shakeVec = new Vector3(shakeMove, 0, 0);    //  Calculate the new vector
        transform.position += shakeVec * Time.deltaTime;    //  Apply the shake
    }
}
