using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEnd : MonoBehaviour
{
    //组件
    public Animator characterAnimator;
    public AnimationTimer animationTimer;
    public PlayerActionManager playerActionManager;
    public PlayerComboManager playerComboManager;
    public PlayerWeaponManager playerWeaponManager;

    //MoveStatus
    protected int MoveStatusID = Animator.StringToHash("MoveStatus");
    //AniamtionEndToIdle
    protected int AnimationEndToIdleID = Animator.StringToHash("AnimationEndToIdle");
    public bool AnimationEndToIdle;
    //AniamtionEndToRunStart
    protected int AnimationEndToRunStartID = Animator.StringToHash("AnimationEndToRunStart");
    public bool AnimationEndToRunStart;
    //AnimationEndToWalk
    protected int AnimationEndToWalkID = Animator.StringToHash("AnimationEndToWalk");
    public bool AnimationEndToWalk;
    //AnimationEndToGlide
    protected int AnimationEndToGlideID = Animator.StringToHash("AnimationEndToGlide");
    public bool AnimationEndToGlide;
    //AnimatinoNotEndToJump
    protected int AnimationNotEndToJumpID = Animator.StringToHash("AnimationNotEndToJump");
    public bool AnimationNotEndToJump;
    //AnimationNotEndToFall
    protected int AnimationNotEndToFallID = Animator.StringToHash("AnimationNotEndToFall");
    public bool AnimationNotEndToFall;


    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        animationTimer = GetComponent<AnimationTimer>();
        playerActionManager = GetComponentInParent<PlayerActionManager>();
        playerComboManager = GetComponentInParent<PlayerComboManager>();
        playerWeaponManager = GetComponentInParent<PlayerWeaponManager>();
    }

    private void Update()
    {
        //更新animator中有关AnimationEnd的参数
        characterAnimator.SetBool(AnimationEndToIdleID, AnimationEndToIdle);
        characterAnimator.SetBool(AnimationEndToWalkID, AnimationEndToWalk);
        characterAnimator.SetBool(AnimationEndToRunStartID, AnimationEndToRunStart);
        CheckAnimationNotEndToJump();
        characterAnimator.SetBool(AnimationNotEndToJumpID, AnimationNotEndToJump);
        CheckAnimationEndToGlide();
        characterAnimator.SetBool(AnimationEndToGlideID,AnimationEndToGlide);
        characterAnimator.SetBool(AnimationNotEndToFallID, AnimationNotEndToFall);
    }

    #region AnimationEndToIdle控制
    public void AnimationEndToIdle_Event()
    {
        AnimationEndToIdle = true;
    }

    public void OnIdleEnter()
    {
        AnimationEndToIdle = false;
    }
    #endregion

    #region AnimationEndToWalk AnimationEndToRunStart 控制
    public void AnimationEndToRunStart_Event()
    {
        AnimationEndToRunStart = true;
        AnimationEndToWalk = true;
    }
    public void OnRun_StartEnter()
    {
        AnimationEndToRunStart = false;
        AnimationEndToWalk = false;
    }

    public void OnSidestep_StartEnter()
    {
        AnimationEndToRunStart = false;
        AnimationEndToWalk = false;
    }

    #endregion

    #region 某些动画片段阻止进入到Walk和runstart状态
    //1.起跳后首先判断是否进入落地动画
    //public void OnJump_1_Start_0Enter()
    //{
    //    AnimationEndToIdle = false;
    //    AnimationEndToWalk = false;
    //    AnimationEndToRunStart = false;
    //}



    #endregion

    #region AnimationNotEndToJump控制

    public void OnJump_1_Start_0Enter()
    {
        AnimationNotEndToJump = true;
        AnimationNotEndToFall = true;
        AnimationEndToGlide = false;
        AnimationEndToIdle = false;
        AnimationEndToWalk = false;
        AnimationEndToRunStart = false;
    }


    public void OnJump_1_Start_1Exit()
    {
        AnimationNotEndToJump = false;
    }

    public void CheckAnimationNotEndToJump()
    {
        if (animationTimer.Jump_1Timer > playerActionManager.jump_1EffectTime)
            AnimationNotEndToJump = false;
    }


    #endregion

    #region AnimationEndToGlide控制
    public void AnimationEndToGlide_Event()
    {
       // AnimationEndToGlide = true;
    }


    //public void OnGlideEnter()
    //{
    //    AnimationEndToGlide = false;
    //}
    //public void OnJump_1_Start_0Enter()
    //{
    //    AnimationEndToGlide = false;
    //}
    public void OnJump_2Enter()
    {
        AnimationEndToGlide = false;
        AnimationNotEndToFall = true;
    }

        public void CheckAnimationEndToGlide()
    {
        if(animationTimer.Jump_1Timer > 0.4f || animationTimer.Jump_2Timer >0.4f)
        {
            AnimationEndToGlide = true;
        }
    }

    public void OnGlideEnter()
    {
        AnimationEndToGlide = false;
        AnimationNotEndToFall = false;
    }

    #endregion

    #region AnimationNotEndToFall控制
    //阻止跳跃的一系列动作进入Fall
    //public void OnJump_1_Start_0Enter()
    //{
    //    AnimationNotEndToFall = true;
    //}

    public void OnJump_1_Start_1Enter()
    {
        AnimationNotEndToFall = true;
    }

    //滑翔状态可以进入Fall
    //public void OnGlideEnter()
    //{
    //    AnimationNotEndToFall = false;
    //}


    //跳跃一系列动作到达一定时间之后不再阻止进入Fall
    public void AnimationNotEndToFall_Event()
    {
        AnimationNotEndToFall = false;
    }

    //落地之后将不再阻止进入Fall
    public void OnFall_StartEnter()
    {
        AnimationNotEndToFall = false;
    }
    public void OnFall_StopEnter()
    {
        AnimationNotEndToFall = false;
    }

    public void OnFall_RollEnter()
    {
        AnimationNotEndToFall = false;
    }

    #endregion

    #region AtkState_StandEnd控制
    public void AtkState_StandEnd()
    {   
        playerWeaponManager.ResetWeaponsState();
    }

    #endregion
}
