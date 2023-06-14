using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] CharacterController characterController;
    [SerializeField] Animator animator;
    [SerializeField] Joystick joystick;

    [SerializeField] float speed = 6f;
    [SerializeField] float walkValue = 0.05f;
    [SerializeField] float runValue = .5f;

    private void OnValidate()
    {
        float offset = .1f;
        walkValue = Mathf.Clamp(walkValue, 0.001f, runValue - offset);
        runValue = Mathf.Clamp(runValue, walkValue + offset, 1f);
    }

    void Update()
    {
        float animationValue = Mathf.Max(Mathf.Abs(joystick.Horizontal), Mathf.Abs(joystick.Vertical));
        SetAnimationByValue(animationValue);

        if (animationValue < walkValue)
            return;

        Vector3 moveVector = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y) * speed;
        characterController.SimpleMove(moveVector);

        Quaternion rotation = Quaternion.LookRotation(moveVector, Vector3.up);
        transform.rotation = rotation;

    }

    void SetAnimationByValue(float value)
    {
        animator.SetBool("idling", value < walkValue);
        animator.SetBool("walking", walkValue <= value && value < runValue);
        animator.SetBool("running", value >= runValue);
    }
}
