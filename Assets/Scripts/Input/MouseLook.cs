using DG.Tweening;
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
    public Quaternion originalRotationWorld;
    public static Vector2 deltaRot;

    public static float sensitivity = 5;
    private Vector2 controllerVector;

    GameManager gm;
    PlayerInput inputs;

    public GameObject player;


    private void Start()
    {
        gm = GameManager.Instance;
        inputs = GameManager.Instance.inputs;
        var rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.freezeRotation = true;
        }

        originalRotation = transform.localRotation;
        originalRotationWorld = transform.rotation;
    }


    private void LateUpdate()
    {
        if (!gm.gameActive)
            return;

        deltaRot.x = inputs.Main.MouseX.ReadValue<float>() * MouseLook.sensitivity * 0.01f;
        deltaRot.y = inputs.Main.MouseY.ReadValue<float>() * MouseLook.sensitivity * 0.01f;

        rotationX = MouseLook.ClampAngle(rotationX + deltaRot.x, minimumX, maximumX);
        rotationY = MouseLook.ClampAngle(rotationY + deltaRot.y, minimumY, maximumY);

        transform.localRotation = Quaternion.Euler(rotationY * -1, rotationX, 0);
    }

    public void SetRotation(float x, float y)
    {
        rotationX = MouseLook.ClampAngle(x, minimumX, maximumX);
        rotationY = MouseLook.ClampAngle(y, minimumY, maximumY);

        Quaternion xQuaternion = Quaternion.AngleAxis(x, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(y, -Vector3.right);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
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

