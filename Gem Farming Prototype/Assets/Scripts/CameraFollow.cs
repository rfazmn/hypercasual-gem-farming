using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 offset;

    void Start()
    {
        offset = target.position - transform.position;
    }

    void Update()
    {
        Vector3 followPos = target.position - offset;
        followPos.y = transform.position.y;
        transform.position = followPos;
    }
}
