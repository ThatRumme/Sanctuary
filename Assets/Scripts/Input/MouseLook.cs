using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    public enum RotationAxes
    {
        MouseXAndY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXAndY;

    public float minimumX = -360F;
    public float maximumX = 360F;

    public float minimumY = -85F;
    public float maximumY = 85F;

    public float rotationX;
    public float rotationY;

    public Quaternion originalRotation;
    public static Vector2 deltaRot;

    public static float sensitivity = 10;
    private Vector2 controllerVector;

    GameManager gm;
    PlayerInput inputs;

    private void Start()
    {
        gm = GameManager.Instance;
        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.freezeRotation = true;
        }

        originalRotation = transform.localRotation;

        inputs = GameManager.Instance.inputs;

    }

    private void Update()
    {
        if (!gm.gameActive)
            return;

        deltaRot.x = inputs.Main.MouseX.ReadValue<float>() * MouseLook.sensitivity * 0.01f;
        deltaRot.y = inputs.Main.MouseY.ReadValue<float>() * MouseLook.sensitivity * 0.01f;

        switch (axes)
        {
            case RotationAxes.MouseXAndY:
                {
                    rotationX = MouseLook.ClampAngle(rotationX + deltaRot.x, minimumX, maximumX);
                    rotationY = MouseLook.ClampAngle(rotationY + deltaRot.y, minimumY, maximumY);

                    Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                    Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

                    transform.localRotation = originalRotation * xQuaternion * yQuaternion;
                    break;
                }

            case RotationAxes.MouseX:
                {
                    rotationX = MouseLook.ClampAngle(rotationX + deltaRot.x, minimumX, maximumX);

                    Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
                    transform.localRotation = originalRotation * xQuaternion;
                    break;
                }

            default:
                {
                    rotationY = MouseLook.ClampAngle(rotationY + deltaRot.y, minimumY, maximumY);

                    Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
                    transform.localRotation = originalRotation * yQuaternion;
                    break;
                }
        }
    }

    public void SetRotation(float x, float y)
    {
        rotationX = MouseLook.ClampAngle(x, minimumX, maximumX);
        rotationY = MouseLook.ClampAngle(y, minimumY, maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis(x, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(y, -Vector3.right);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
        //Debug.Log(transform.localRotation.eulerAngles);
    }



    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
        {
            angle += 360F;
        }

        if (angle > 360F)
        {
            angle -= 360F;
        }

        return Mathf.Clamp(angle, min, max);
    }
}