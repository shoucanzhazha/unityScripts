using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class CharacterInputSystem_RootMotion_JoyStick : MonoBehaviour
{
    public InputController_RootMotion_JoyStick inputController;
    public Animator characterAnimator;
    public PlayerActionManager playerActionManager;

    public bool canInputControl;



    #region 输入信息
    //移动    Movement 左摇杆
    public Vector2 GetPlayerMovement { get => inputController.PlayerInput.Movement.ReadValue<Vector2>(); }
    public Vector2 playerMovement;
    [Header("轻推手柄")]
    public bool nudge;
    [SerializeField]
    private float nudgeTimer;
    [SerializeField]
    private float Timer;

    //摄像机   CameraLook 右摇杆
    public Vector2 GetCameraLook { get => inputController.PlayerInput.CameraLook.ReadValue<Vector2>(); }
    public Vector2 cameraLook;

    //鼠标滚轮
    public float GetMouseScroll { get => inputController.PlayerInput.Switch_Weapon.ReadValue<float>(); }
    public float mouseScroll;

    //闪避    Sidestep 右扳机
    public bool sidestepInput;
    public float sidestepInputTime;
    private float sidestepInputTimer;
    protected int Sidestep_PressID = Animator.StringToHash("Sidestep_Press");
    protected int Sidestep_HoldID = Animator.StringToHash("Sidestep_Hold");

    //跳跃    Jump A键
    protected int Jump_PressID = Animator.StringToHash("Jump_Press");
    protected int Jump_HoldID = Animator.StringToHash("Jump_Hold");

    //轻攻击   LAtk X键
    protected int LightAttack_PressID = Animator.StringToHash("LightAttack_Press");
    protected int LightAttack_HoldID = Animator.StringToHash("LightAttack_Hold");

    //重攻击  HAtk Y键
    protected int HeavyAttack_PressID = Animator.StringToHash("HeavyAttack_Press");
    protected int HeavyAttack_HoldID = Animator.StringToHash("HeavyAttack_Hold");


    #endregion

    protected int MoveStatusID = Animator.StringToHash("MoveStatus");


    private void Awake()
    {
        if(inputController == null)
            inputController = new InputController_RootMotion_JoyStick();

        InputInteractionInit();
        playerActionManager = GetComponent<PlayerActionManager>();


    }

    private void OnEnable()
    {
        inputController.Enable();
    }

    private void OnDisable()
    {
        inputController.Disable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        UpdateInput();
        CloseInput();
    }

    /// <summary>
    /// inputsystem事件注册
    /// </summary>
    private void InputInteractionInit()
    {
        #region Sidestep的inputInteraction

        inputController.PlayerInput.Sidestep.started += inputInteraction =>
        {
            if( playerActionManager.sidestepCD && playerActionManager.sidestepCounter < playerActionManager.sidestepMaxCount)
            {
                sidestepInput = true;
                sidestepInputTimer = 0;
                
                characterAnimator.SetTrigger(Sidestep_PressID);
                
                if(!playerActionManager.IsOnGround)
                {
                    playerActionManager.sidestepCounter++;
                }
            }

        };

        inputController.PlayerInput.Sidestep.performed += inputInteraction =>
        {
            if (inputInteraction.interaction is HoldInteraction)
            {
                characterAnimator.SetBool(Sidestep_HoldID, true);
            }
        };

        inputController.PlayerInput.Sidestep.canceled += inputInteraction =>
        {
            characterAnimator.SetBool(Sidestep_HoldID,false);
        };

        #endregion

        #region Jump的inputInteraction
        inputController.PlayerInput.Jump.started += inputInteraction =>
        {
            if (playerActionManager.jumpCounter < playerActionManager.jumpMaxCount)
            {
                characterAnimator.SetTrigger(Jump_PressID);
                if (!playerActionManager.IsOnGround)
                {
                    playerActionManager.jumpCounter++;
                }
                
            }

        };

        inputController.PlayerInput.Jump.performed += inputInteraction =>
        {
            if (inputInteraction.interaction is HoldInteraction)
            {
                characterAnimator.SetBool(Jump_HoldID, true);
                characterAnimator.SetInteger(MoveStatusID, 2);

            }
        };

        inputController.PlayerInput.Jump.canceled += inputInteraction =>
        {
            characterAnimator.SetBool(Jump_HoldID, false);
        };



        #endregion
    }





    /// <summary>
    /// 更新输入信息
    /// </summary>
    private void UpdateInput()
    {
        //右摇杆 摄像机
        cameraLook = GetCameraLook;

        mouseScroll = GetMouseScroll;

        if (!canInputControl)
        {
            playerMovement = Vector2.zero;
            return;
        }

        //左摇杆 移动
        if (GetPlayerMovement.magnitude > 0.8f)
        {
            nudgeTimer = 0;
            playerMovement = GetPlayerMovement.normalized;
            nudge = false;
        }
        else if (GetPlayerMovement.magnitude > 0.01f)
        {
            nudgeTimer += Time.deltaTime;
            if (nudgeTimer > 0.4f)
            {
                nudgeTimer = 0;
                playerMovement = GetPlayerMovement.normalized;
                nudge = true;
            }
        }
        else
        {
            Timer += Time.deltaTime;
            if (characterAnimator.GetInteger(MoveStatusID)<3||(characterAnimator.GetInteger(MoveStatusID) == 3 && Timer > 0.2f))
            {
                Timer = 0;
                playerMovement = Vector2.zero;
            }
        }



    }

    /// <summary>
    /// 关闭输入信号
    /// </summary>
    private void CloseInput()
    {
        if(sidestepInput)
            sidestepInputTimer += Time.deltaTime;
        sidestepInput = sidestepInputTimer < sidestepInputTime;
    }
}
