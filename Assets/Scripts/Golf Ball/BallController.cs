//using MiniGolf.Main;
//using MiniGolf.Utilities;
//using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//public class BallController : GenericMonoSingleton<BallController>
//{
//    #region Properties
//    public int CurrentPower { get; private set; }

//    [SerializeField] private Transform startPosition;       //assign empty GameObject at start of level
//    [SerializeField] private LineRenderer lineRenderer;     //reference to lineRenderer child object
//    [SerializeField] private float MaxForce = 10f;          //maximum force that an be applied to ball
//    [SerializeField] private float dragSensitivity = 10f;   //how much drag translates to power
//    [SerializeField] private GameObject areaAffector;       //reference to sprite object which show area around ball to click
//    [SerializeField] private LayerMask rayLayer;            //layer allowed to be detected by ray

//    private Vector3 lastSafePosition;
//    private Quaternion lastSafeRotation;
//    private float force;                                    //actuale force which is applied to the ball
//    private Rigidbody rgBody;                               //reference to rigidbody attached to this gameobject
//    private Vector3 startPos, endPos;
//    private bool canShoot = false, ballIsStatic = true;     //bool to make shooting stopping ball easy
//    private Vector3 direction;                              //direction in which the ball will be shot
//    private int strokes = 0;
//    #endregion

//    #region Lifecycle methods
//    protected override void Awake()
//    {
//        base.Awake();
//        rgBody = GetComponent<Rigidbody>();
//    }

//    private void Start()
//    {
//        // Set initial safe position
//        lastSafePosition = startPosition.position;
//        lastSafeRotation = startPosition.rotation;

//        // Optional: move ball to start
//        transform.position = lastSafePosition;
//        transform.rotation = lastSafeRotation;
//    }

//    private void Update()
//    {
//        if (rgBody.velocity == Vector3.zero && !ballIsStatic)   //if velocity is zero and ballIsStatic is false
//        {
//            ballIsStatic = true;
//            lastSafePosition = transform.position;
//            lastSafeRotation = transform.rotation;
//            rgBody.angularVelocity = Vector3.zero;              //set angular velocity to zero
//            areaAffector.SetActive(true);                       //activate areaAffector
//            GameService.Instance.GameStateManager.ChangeState(GameState.PlayerInput);
//        }
//    }

//    private void FixedUpdate()
//    {
//        if (!canShoot) return;

//        canShoot = false;                                       //set canShoot to false
//        ballIsStatic = false;                                   //set ballIsStatic to false
//        GameService.Instance.GameStateManager.ChangeState(GameState.BallMoving);
//        direction = startPos - endPos;                          //get the direction between 2 vectors from start to end pos
//        rgBody.AddForce(direction * force, ForceMode.Impulse);  //add force to the ball in given direction
//        areaAffector.SetActive(false);                          //deactivate areaAffector
//        //UIManager.instance.PowerBar.fillAmount = 0;             //reset the powerBar to zero
//        force = 0;                                              //reset the force to zero
//        startPos = endPos = Vector3.zero;                       //reset the vectors to zero
//    }

//    // Unity native Method to detect colliding objects
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.name == "Destroyer")     //if the object name is Destroyer
//            ResetBall();
//        else if (other.name == "Hole")     //if the object name is Hole
//            GameService.Instance.GameStateManager.ChangeState(GameState.LevelComplete);
//    }
//    #endregion

//    #region Input Methods
//    public void MouseDownMethod()                                           //method called on mouse down by InputManager
//    {
//        if (!ballIsStatic) return;                                          //no mouse detection if ball is moving
//        startPos = ClickedPoint();                                          //get the vector in word space
//        lineRenderer.gameObject.SetActive(true);                            //activate lineRenderer
//        lineRenderer.SetPosition(0, lineRenderer.transform.localPosition);  //set its 1st position
//    }

//    public void MouseNormalMethod()
//    {
//        if (!ballIsStatic) return;

//        endPos = ClickedPoint();
//        Vector3 dragVector = endPos - startPos;
//        float dragDistance = dragVector.magnitude;

//        // Clamp the force to MaxForce
//        force = Mathf.Clamp(dragDistance * dragSensitivity, 0, MaxForce);

//        // Limit the visual drag distance proportionally
//        if (dragDistance * dragSensitivity > MaxForce)
//        {
//            dragVector = dragVector.normalized * (MaxForce / dragSensitivity);
//            endPos = startPos + dragVector;
//        }

//        CurrentPower = Mathf.RoundToInt((force / MaxForce) * 100f);
//        GameService.Instance.UIService.SetPower(CurrentPower);

//        // Update line renderer
//        lineRenderer.SetPosition(1, transform.InverseTransformPoint(endPos));
//    }

//    public void MouseUpMethod()                                         //method called by InputManager
//    {
//        if (!ballIsStatic) return;                                      //no mouse detection if ball is moving
//        canShoot = true;                                                //set canShoot true
//        lineRenderer.gameObject.SetActive(false);                       //deactive lineRenderer
//        GameService.Instance.UIService.SetPower(0);
//        GameService.Instance.UIService.SetStrokes(strokes++);
//    }

//    private Vector3 ClickedPoint()
//    {
//        Vector3 position = Vector3.zero;                                //get a new Vector3 varialbe
//        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);    //create a ray from camera in mouseposition direction
//        RaycastHit hit = new RaycastHit();                              //create a RaycastHit
//        if (Physics.Raycast(ray, out hit, Mathf.Infinity, rayLayer))    //check for the hit
//            position = hit.point;                                       //save the hit point in position
//        return position;                                                //return position
//    }
//    #endregion

//    private void ResetBall()
//    {
//        rgBody.velocity = Vector3.zero;
//        rgBody.angularVelocity = Vector3.zero;
//        transform.position = lastSafePosition;
//        transform.rotation = lastSafeRotation;
//    }
//}

using MiniGolf.Main;
using UnityEngine;

namespace MiniGolf.Ball
{
    public class BallController
    {
        private readonly BallView view;
        private readonly BallSO model;

        private Vector3 lastSafePosition;
        private Quaternion lastSafeRotation;
        private Vector3 startPos, endPos, direction;
        private float force;
        private bool canShoot = false;
        private bool ballIsStatic = true;
        private int strokes = 0;

        public int CurrentPower { get; private set; }
        public BallView View => view;

        public BallController(BallView view, BallSO model)
        {
            this.view = view;
            this.model = model;

            view.SetController(this);

            ResetToStart();
        }

        public void SubscribeToEvents()
        {
            GameService.Instance.EventService.OnMouseDown.AddListener(OnMouseDown);
            GameService.Instance.EventService.OnMouseNormal.AddListener(OnMouseDrag);
            GameService.Instance.EventService.OnMouseUp.AddListener(OnMouseUp);
        }

        public void UnSubscribeToEvents()
        {
            GameService.Instance.EventService.OnMouseDown.RemoveListener(OnMouseDown);
            GameService.Instance.EventService.OnMouseNormal.RemoveListener(OnMouseDrag);
            GameService.Instance.EventService.OnMouseUp.RemoveListener(OnMouseUp);
        }

        private void ResetToStart()
        {
            lastSafePosition = view.StartPosition.position;
            lastSafeRotation = view.StartPosition.rotation;
            view.transform.position = lastSafePosition;
            view.transform.rotation = lastSafeRotation;
        }

        private void ResetBall()
        {
            view.Rb.velocity = Vector3.zero;
            view.Rb.angularVelocity = Vector3.zero;
            view.transform.position = lastSafePosition;
            view.transform.rotation = lastSafeRotation;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.name == "Destroyer")
                ResetBall();
            else if (other.name == "Hole")
                GameService.Instance.GameStateManager.ChangeState(GameState.LevelComplete);
        }

        #region Update methods
        public void TickUpdate()
        {
            if (view.Rb.velocity == Vector3.zero && !ballIsStatic)
            {
                ballIsStatic = true;
                lastSafePosition = view.transform.position;
                lastSafeRotation = view.transform.rotation;
                view.Rb.angularVelocity = Vector3.zero;
                view.AreaAffector.SetActive(true);

                GameService.Instance.GameStateManager.ChangeState(GameState.PlayerInput);
            }
        }

        public void TickFixedUpdate()
        {
            if (!canShoot) return;

            canShoot = false;
            ballIsStatic = false;
            GameService.Instance.GameStateManager.ChangeState(GameState.BallMoving);

            direction = startPos - endPos;
            view.Rb.AddForce(direction * force, ForceMode.Impulse);
            view.AreaAffector.SetActive(false);

            force = 0;
            startPos = endPos = Vector3.zero;
        }
        #endregion

        #region Input Methods
        public void OnMouseDown()
        {
            if (!ballIsStatic) return;

            startPos = ClickedPoint();
            view.LineRenderer.gameObject.SetActive(true);
            view.LineRenderer.SetPosition(0, view.LineRenderer.transform.localPosition);
        }

        public void OnMouseDrag()
        {
            if (!ballIsStatic) return;

            endPos = ClickedPoint();
            Vector3 dragVector = endPos - startPos;
            float dragDistance = dragVector.magnitude;

            force = Mathf.Clamp(dragDistance * model.dragSensitivity, 0, model.maxForce);

            // clamp visual line
            if (dragDistance * model.dragSensitivity > model.maxForce)
            {
                dragVector = dragVector.normalized * (model.maxForce / model.dragSensitivity);
                endPos = startPos + dragVector;
            }

            CurrentPower = Mathf.RoundToInt((force / model.maxForce) * 100f);
            GameService.Instance.UIService.SetPower(CurrentPower);

            view.LineRenderer.SetPosition(1, view.transform.InverseTransformPoint(endPos));
        }

        public void OnMouseUp()
        {
            if (!ballIsStatic) return;

            canShoot = true;
            view.LineRenderer.gameObject.SetActive(false);
            GameService.Instance.UIService.SetPower(0);
            GameService.Instance.UIService.SetStrokes(++strokes);
        }

        private Vector3 ClickedPoint()
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray, out var hit, Mathf.Infinity, model.rayLayer) ? hit.point : Vector3.zero;
        }
        #endregion
    }
}