using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float forceMultiplier = 10f;

    void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Test: press Space to push ball forward
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.forward * forceMultiplier, ForceMode.Impulse);
        }
    }
}