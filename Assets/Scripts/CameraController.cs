using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 touchStart;

    private void Start()
    {
        
    }

    private void Update()
    {
        Vector3 screenTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // https://www.youtube.com/watch?v=K_aAnBn5khA
        if (Input.GetMouseButtonDown(0))    //  Remember the tocuh position every time player touches the screen
        {
            touchStart = screenTouchPosition;
        }
        if (Input.GetMouseButton(0))        //  While finger is down - move relatively to the start position
        {
            Vector3 camMoveDirection = touchStart - screenTouchPosition;
            Camera.main.transform.position += camMoveDirection;
        }
    }





    //void FixedUpdate()
    //{
    //    RaycastHit hit;
    //    if(Physics.Raycast(transform.position, Vector3.forward, out hit, Mathf.Infinity))
    //    {
    //        Debug.Log("Wow");
    //    }
    //    Debug.DrawRay(transform.position, Vector3.forward, Color.red);

    //}



}
