using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    #region Fields
    [Header("General")]
    public PlayerMovement pm; //Player Movement

    public GameObject cam; //Camera
    PlayerInput inputs;

    #endregion

    #region Start, Awake, Update


    private void Awake()
    {
        GameManager.Instance.player = this;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = GetComponent<PlayerMovement>();

        inputs = GameManager.Instance.inputs;
        inputs.Main.Fire1.performed += OnFireKey;
        inputs.Main.Interact.performed += OnInteractKey;

        //transform.position = GameManager.Instance.lm.GetSpawnPoint(spawnIdx);

    }

    protected void OnDestroy()
    {
        inputs.Main.Fire1.performed -= OnFireKey;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if (!gm.gameActive)
        //    return;
    }

    #endregion

    private void OnEnable()
    {
        //inputs.Enable();
        inputs = GameManager.Instance.inputs;
    }

    private void OnDisable()
    {
    }

    private void OnFireKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Fire();
        }
    }

    private void OnInteractKey(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Interact();
        }
    }


    protected virtual void Interact()
    {

    }

    protected virtual void Fire()
    {

    }




}
