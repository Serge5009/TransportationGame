using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 touchStart;
    [SerializeField] float minZoom = 2;
    [SerializeField] float maxZoom = 50;

    [SerializeField] float zoomSens = 1;
    [SerializeField] float mouseZoomSens = 1;
    private void Start()
    {
        zoomSens *= 0.01f;  //  Some default ajustments
        mouseZoomSens *= 5;
    }

    private void Update()
    {
        Vector3 screenTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // https://www.youtube.com/watch?v=K_aAnBn5khA
        if (Input.GetMouseButtonDown(0))    //  Remember the tocuh position every time player touches the screen
        {
            touchStart = screenTouchPosition;
        }
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 t0OldPos = t0.position - t0.deltaPosition;
            Vector2 t1OldPos = t1.position - t1.deltaPosition;

            float newDist = Vector2.Distance(t0.position, t1.position);
            float oldDist = Vector2.Distance(t0OldPos, t1OldPos);

            float difference = newDist - oldDist;

            Zoom(difference * zoomSens);
        }
        else if (Input.GetMouseButton(0))        //  While finger is down - move relatively to the start position
        {
            Vector3 camMoveDirection = touchStart - screenTouchPosition;
            Camera.main.transform.position += camMoveDirection;
        }

        //  TO DO: Add keyboard input

        Zoom(Input.GetAxis("Mouse ScrollWheel") * mouseZoomSens);
    }


    void Zoom(float amount)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - amount, minZoom, maxZoom);
    }


    //  Failed to use raycast for object selection
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
