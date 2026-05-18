using UnityEngine;

public class BallShooter : MonoBehaviour
{
    public float shootForce = 30f;
    public float ballMass = 5f;
    public float ballSize = 0.5f;
    public float destroyDelay = 4f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBall();
        }
    }

    void ShootBall()
    {
        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("No camera tagged MainCamera found in the scene");
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 shootDirection;

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f))
        {
            shootDirection = (hit.point - cam.transform.position).normalized;
        }
        else
        {
            shootDirection = ray.direction;
        }

        GameObject ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        
        ball.transform.position = cam.transform.position + shootDirection * 1.5f;
        ball.transform.localScale = Vector3.one * ballSize;

        Renderer rend = ball.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = new Color(Random.value, Random.value, Random.value);
        }

        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.mass = ballMass;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        rb.AddForce(shootDirection * shootForce, ForceMode.Impulse);

        Destroy(ball, destroyDelay);
    }
}
