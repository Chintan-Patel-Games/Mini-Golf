using UnityEngine;

/// <summary>
/// Script which detect mouse click and decide who will take input Ball or Camera
/// </summary>
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    [SerializeField] private float clickDistanceLimit = 0.2f; //variable to decide who will take input Ball or Camera

    private float distanceBetweenBallAndMouseClick;             //variable to track the distance
    private bool canRotate = false;

    private bool ballInputEnabled = true;
    private bool cameraInputEnabled = true;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// Enable or disable ball input independently
    /// </summary>
    public void EnableBallInput(bool enabled) => ballInputEnabled = enabled;

    /// <summary>
    /// Enable or disable camera input independently
    /// </summary>
    public void EnableCameraInput(bool enabled) => cameraInputEnabled = enabled;

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !canRotate)          //if mouse button is clicked and canRotate is false
        {
            GetDistance();                                      //get the distance between mouseClick point and ball
            canRotate = true;                                   //set canRotate to true

            //if distance is less than the limit allowed
            if (distanceBetweenBallAndMouseClick <= clickDistanceLimit && ballInputEnabled)
                BallController.Instance.MouseDownMethod();         //we control the ball
        }

        if (canRotate)                                          //if canRotate is true
        {
            if (Input.GetMouseButton(0))                        //if mousebutton is clicked
            {   //if distance is less than the limit allowed
                if (distanceBetweenBallAndMouseClick <= clickDistanceLimit && ballInputEnabled)
                    BallController.Instance.MouseNormalMethod();   //call ball method
                else if (cameraInputEnabled)                                         //else call camera method
                    CameraManager.Instance.RotateCamera(Input.GetAxis("Mouse X"));
            }

            if (Input.GetMouseButtonUp(0))                      //on mouse click is left
            {
                canRotate = false;                              //canRotate is set false
                //if distance is less than the limit allowed
                if (distanceBetweenBallAndMouseClick <= clickDistanceLimit && ballInputEnabled)
                    BallController.Instance.MouseUpMethod();       //call ball method
            }
        }
    }

    /// <summary>
    /// Method which give us distance between click point in world and ball
    /// </summary>
    private void GetDistance()
    {
        if (Camera.main == null) return;

        //we create a plane whose mid point is at ball position and whose normal is toward Camera
        var plane = new Plane(Camera.main.transform.forward, BallController.Instance.transform.position);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //create a ray
        float dist;                                                     //varibale to get ditance
        if (plane.Raycast(ray, out dist))
        {
            var v3Pos = ray.GetPoint(dist);                             //get the point at the given distance
            //calculate the distance
            distanceBetweenBallAndMouseClick = Vector3.Distance(v3Pos, BallController.Instance.transform.position);
        }
    }
}