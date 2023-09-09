using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovementBase_RootMotion : MonoBehaviour
{
    //组件

    [Header("========== 组件 ==========")]
    public Animator characterAnimator;
    public CharacterController characterController;
    [SerializeField]
    protected GetModelRootMotion getModelRootMotion;
    public CharacterInputSystem_RootMotion_JoyStick characterInputSystem_JoyStick;

    //移动向量

    [Header("移动向量"), Header("========== 角色移动 ==========")]
    public Vector3 movementDir;
    [Header("垂直向量")]
    public Vector3 verticalDir;
    [Header("动画位移")]
    public Vector3 deltaPosition;
    [Header("效果")]
    public Vector3 effectDir;


    [SerializeField, Header("重力倍率"), Header("========== 角色重力 ==========")]
    protected float characterGravity;
    private float defaultCharacterGravity;
    [SerializeField, Header("移动速度")]
    protected float characterCurrentMoveSpeed;
    [SerializeField, Header("判定掉落的时间")]
    protected float characterFallTime;
    protected float characterFallOutDeltaTimer;//计时器
    [SerializeField, Header("默认掉落速率")]
    protected float defaultVerticalSpeed;
    [SerializeField, Header("最大掉落速度")]
    protected float maxVerticalSpeed;
    [SerializeField, Header("实际掉落速度")]
    protected float currentVerticalSpeed;


    [Header("地面检测"), Header(" ========== 检测 ========== ")]
    public float groundDetectionRadius;
    public float groundDetectionOffest;

    [Header("障碍物检测")]
    public float obstacleDetectionDistance;
    public float obstacleDetectionOffest;

    [SerializeField, Header("斜坡射线检测范围")]
    protected float slopRayExtent;
    [SerializeField, Header("斜坡角度")]
    protected float slopAngle;
    [SerializeField, Header("地面层级")]
    protected LayerMask groundLayer;
    [SerializeField, Header("障碍物层级")]
    protected LayerMask obsLayer;


    //判断
    [Header("========== 基础判断 ==========")]
    //是否需要地面判断
    public bool needCheckOnGround;
    //地面判断
    public bool isOnGround;
    public bool isHover;//浮空状态
    public bool isGlide;//滑翔状态
    public bool canAnimationMove;

    //AnimationID
    protected int OnGroundID = Animator.StringToHash("OnGround");


    protected virtual void Awake()
    {
        characterAnimator = GetComponentInChildren<Animator>();
        characterController = GetComponent<CharacterController>();
        characterInputSystem_JoyStick = GetComponent<CharacterInputSystem_RootMotion_JoyStick>();
        getModelRootMotion = GetComponentInChildren<GetModelRootMotion>();
    }

    protected virtual void Start()
    {
        Init();
    }

    protected virtual void Update()
    {
        GetDeltaPosition();
        CharacterGravity();
        CheckOnGround();


    }

    #region 内部函数

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        defaultCharacterGravity = characterGravity;
    }

    /// <summary>
    /// 角色重力
    /// </summary>
    private void CharacterGravity()
    {
        //如果角色在地面上并且需要判断是否在地面上
        //将垂直方向的速度设置为 默认的垂直速度
        if (needCheckOnGround && isOnGround)
        {
            //Debug.Log("NeedOnGround IsOnGround");
            characterFallOutDeltaTimer = characterFallTime;
            if (currentVerticalSpeed < 0.01f)
            {
                currentVerticalSpeed = defaultVerticalSpeed;
            }
        }
        else
        {
            if (characterFallOutDeltaTimer >= 0.0f)
            {
                characterFallOutDeltaTimer -= Time.deltaTime;
                characterFallOutDeltaTimer = Mathf.Clamp(characterFallOutDeltaTimer, 0, characterFallTime);
            }
        }

        if (currentVerticalSpeed > maxVerticalSpeed && !isOnGround && !isHover && characterFallOutDeltaTimer == 0)
        {
            currentVerticalSpeed += characterGravity * Time.deltaTime;
        }
    }

    /// <summary>
    /// 地面检测
    /// </summary>
    private void CheckOnGround()
    {
        //如果需要地面检测才进行地面检测，不需要地面检测时，isOnGround为false
        if (!needCheckOnGround)
        {
            isOnGround = false;
        }
        else
        {
            Vector3 CheckOnGroundPos = new Vector3(transform.position.x, transform.position.y - groundDetectionOffest, transform.position.z);
            isOnGround = Physics.CheckSphere(CheckOnGroundPos, groundDetectionRadius, groundLayer, QueryTriggerInteraction.Ignore);
        }
        characterAnimator.SetBool(OnGroundID, isOnGround);
    }

    /// <summary>
    /// 获取动画的位移
    /// </summary>
    private void GetDeltaPosition()
    {
        deltaPosition = getModelRootMotion.deltaPosition;
    }

    /// <summary>
    /// 动画位移检测
    /// </summary>
    private bool CheckAnimationMove(Vector3 dir)
    {
        Vector3 CheckObstaclePos = new Vector3(transform.position.x, transform.position.y - obstacleDetectionOffest, transform.position.z);

        canAnimationMove = Physics.Raycast(CheckObstaclePos, dir.normalized * deltaPosition.magnitude, out var hit, obstacleDetectionDistance, obsLayer);
        return canAnimationMove;
    }

    /// <summary>
    /// 坡度检测，并将速度调整为适应坡度的速度
    /// </summary>
    /// <param name="dir">角色当前的移动方向</param>
    /// <returns></returns>
    protected Vector3 ResetMoveDirectionOnSlop(Vector3 dir)
    {
        //如果玩家当前 needOnGround 并且 IsOnGround
        if (needCheckOnGround && isOnGround)
        {
            Physics.Raycast(transform.position, -Vector3.up, out var hit, slopRayExtent);
            //检测脚下坡度的度数
            float angle = Vector3.Dot(Vector3.up, hit.normal);
            slopAngle = Vector3.Angle(Vector3.up, hit.normal);
            if (angle != 0 && currentVerticalSpeed <= 0)
            {
                //当前移动方向在坡度这个向量上的投影
                return Vector3.ProjectOnPlane(dir, hit.normal);
            }

        }
        return dir;
    }
    #endregion


    /// <summary>
    /// 角色移动的公共接口
    /// </summary>
    /// <param name="moveDir"></param>
    /// <param name="useGravity"></param>
    public virtual void CharacterMoveInterface(Vector3 moveDir, bool useGravity)
    {
        if (!CheckAnimationMove(moveDir))
        {
            movementDir = ResetMoveDirectionOnSlop(moveDir.normalized);
        }

        if (useGravity)
        {
            verticalDir.Set(0f, currentVerticalSpeed, 0f);
        }
        else
        {
            verticalDir = Vector3.zero;
        }
    }

    /// <summary>
    /// 角色重置重力速度的接口
    /// </summary>
    public virtual void CharacterResetGravity()
    {
        currentVerticalSpeed = defaultVerticalSpeed;
        characterGravity = defaultCharacterGravity;
    }

    /// <summary>
    /// 角色重力为零的接口
    /// </summary>
    public virtual void CharacterNoGravity()
    {
        currentVerticalSpeed = 0f;
    }

    /// <summary>
    /// 角色进入悬浮状态，加速度，速度均为0
    /// </summary>
    public virtual void CharacterEnterHover()
    {
        characterGravity = 0;
        currentVerticalSpeed = 0f;
        needCheckOnGround = false;
        isHover = true;

    }

    /// <summary>
    /// 角色退出悬浮状态
    /// </summary>
    public virtual void CharacterExitHover(float FallSpeed, float FallGravity)
    {
        currentVerticalSpeed = FallSpeed;
        characterGravity = FallGravity;
        needCheckOnGround = true;
        isHover = false;
    }

    /// <summary>
    /// 角色退出悬浮状态，恢复默认数值
    /// </summary>
    public virtual void CharacterDafultExitHover()
    {
        currentVerticalSpeed = defaultVerticalSpeed;
        characterGravity = defaultCharacterGravity;
        needCheckOnGround = true;
        isHover = false;

    }
}
