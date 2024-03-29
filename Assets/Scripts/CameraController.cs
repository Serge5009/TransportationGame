using UnityEngine;

public class CameraController : MonoBehaviour
{
    //  Input tracking
    Vector3 touchStart;
    Vector3 touchStartLocal;
    Vector3 screenTouchPosition;

    //  Zoom settings
    [SerializeField] float minZoom = 2.0f;
    [SerializeField] float maxZoom = 50.0f;
    [SerializeField] float zoomSens = 1.0f;
    [SerializeField] float mouseZoomSens = 1.0f;

    //  How far can you move the finger before lifting it to still count it as a click, not swipe
    [SerializeField] float clickRadius = 10.0f;

    private void Start()
    {
        zoomSens *= 0.01f;  //  Some default ajustments
        mouseZoomSens *= 5; //  TO DO: new zoom multiplier logic would be nice
    }

    //  Tutorial
    float totalMove = 0.0f;

    private void Update()
    {
        if (GameManager.gm.gState == GAME_STATE.INMENU) //  If there's a menu open - will ignore camera controlls
            return;

        //Debug.Log(Input.mousePosition);

        //  TO DO: Try to make touch controls smooth
        screenTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // https://www.youtube.com/watch?v=K_aAnBn5khA
        if (Input.GetMouseButtonDown(0))    //  Remember the tocuh position every time player touches the screen
        {
            touchStart = screenTouchPosition;
            touchStartLocal = Input.mousePosition;
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

            //  Tutorial
            if(totalMove >= 500.0f)
                ProgressController.pControll.OnCameraMove();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 touchEndLocal = Input.mousePosition;

            if (Vector3.Distance(touchStartLocal, touchEndLocal) <= clickRadius)
                Click();

            totalMove += Vector2.Distance(touchStartLocal, touchEndLocal);
        }

        //  TO DO: Add keyboard input

        Zoom(Input.GetAxis("Mouse ScrollWheel") * mouseZoomSens);
    }

    void Click()
    {
        float edgeRadiusIgnore = 70.0f; //  This is done in order to ignore click while interacting with the UI
        
        if (Input.mousePosition.x <= edgeRadiusIgnore || Input.mousePosition.y <= edgeRadiusIgnore || Input.mousePosition.x >= Screen.width - edgeRadiusIgnore || Input.mousePosition.y >= Screen.height - edgeRadiusIgnore)
            return;

        if(GameManager.gm.gState == GAME_STATE.BUILD)
        {
            RoadNetwork.rn.PlaceTempNode(screenTouchPosition);    //  TO DO: switch to temp placement
        }

        //  Tutorial
        ProgressController.pControll.OnCameraClick();
    }

    float totalZoom = 0.0f;
    void Zoom(float amount)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - amount, minZoom, maxZoom);

        //  Tutorial
        if (amount < 0) //  TO DO: should move zoom and move tracking to a ProgressController
            amount *= -1;
        totalZoom += amount;
        if(totalZoom >= 10.0f)
            ProgressController.pControll.OnCameraZoom();
    }

    //  TO DO:
    //  Try to implement somne sort of raycast to select objects

}
