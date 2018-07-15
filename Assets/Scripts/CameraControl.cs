using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 // Approximate time for the camera to refocus.

    private float distance;
    private Camera m_Camera;                        // Used for referencing the camera.
    public float ZoomSpeed;                      // Reference speed for the smooth damping of the orthographic size.


    private void Awake()
    {
        m_Camera = GetComponent<Camera>();
    }


    private void FixedUpdate()
    {
        // zoom the camera based on hit distance

        distance = GetComponentInParent<rayWalk>().hitDistance;
        m_Camera.orthographicSize = Mathf.SmoothDamp(8f, 8f + (1.5f * distance), ref ZoomSpeed, m_DampTime);
    }
}
