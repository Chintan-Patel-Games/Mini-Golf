using MiniGolf.Main;
using UnityEngine;

namespace MiniGolf.Ball
{
    public class BallController
    {
        #region Variables
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
        #endregion

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
                GameService.Instance.GameStateManager.ChangeState(GameState.GameState.LevelComplete);
        }

        public void OnCollisionEnter(Collision collision)
        {
            // Check if the ball hit the ground, wall, or obstacle
            if (collision.gameObject.CompareTag("Level"))
            {
                float impactForce = collision.relativeVelocity.magnitude;

                if (impactForce > 0.2f) // threshold to avoid tiny sounds
                {
                    GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.BALL_HIT);
                }
            }
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

                GameService.Instance.GameStateManager.ChangeState(GameState.GameState.PlayerInput);
            }
        }

        public void TickFixedUpdate()
        {
            if (!canShoot) return;

            canShoot = false;
            ballIsStatic = false;
            GameService.Instance.GameStateManager.ChangeState(GameState.GameState.BallMoving);

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

            view.LineRenderer.SetPosition(1, view.transform.InverseTransformPoint(startPos - dragVector));
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