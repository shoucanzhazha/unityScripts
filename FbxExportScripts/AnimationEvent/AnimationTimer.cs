using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTimer : MonoBehaviour
{
    public Animator characterAnimator;
    public PlayerActionManager playerActionManager;

    [Header("Sidestep计时器"), Header("===== 计时器 =====")]
    public float SidestepTimer;
    public float SidestepCDTimer;
    [Header("Jump_1效果")]
    //public bool Jump_1Effect;
    public float Jump_1Timer;
    [Header("Jump_2效果")]
    //public bool Jump_2Effect;
    public float Jump_2Timer;
    [Header("Sprint转向")]
    public float Sprint_Loop_TurnDirTimer;

    #region 生命周期函数
    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        playerActionManager = GetComponentInParent<PlayerActionManager>();
    }


    private void Update()
    {
        CDTimerUpdate();
    }



    public void CDTimerUpdate()
    {
        //在冷却之中才进行时间更新
        if (!playerActionManager.sidestepCD)
        {
            SidestepCDTimer += Time.deltaTime;
        }
    }


    #endregion

    #region 通过进入某些动画实现的计时器
    
    #region Sidestep_Loop中的计时器
    public void OnSidestep_StartEnter()
    {
        SidestepTimer = 0;
        SidestepCDTimer = 0;
    }

    public void OnSidestep_LoopEnter()
    {

    }

    public void OnSidestep_StartUpdate()
    {
        SidestepTimer += Time.deltaTime;
    }

    public void OnSidestep_LoopUpdate()
    {
        SidestepTimer += Time.deltaTime;
    }

    public void OnSidestep_LoopExit()
    {
        SidestepTimer = 0;
    }
    #endregion

    #region Jump_1_Start_1中的计时器
    public void OnJump_1_Start_1Enter()
    {
        Jump_1Timer = 0;
    }
    public void OnJump_1_Start_1Update()
    {
        Jump_1Timer += Time.deltaTime;
    }

    public void OnJump_1_Start_1Exit()
    {
        Jump_1Timer = 0;
    }

    #endregion

    #region Jump_2中的计时器
    public void OnJump_2Enter()
    {
        Jump_2Timer = 0;
    }

    public void OnJump_2Update()
    {
        Jump_2Timer +=Time.deltaTime;
    }

    public void OnJump_2Exit()
    {

    }


    #endregion

    #region Sprint_Loop_TurnDir中的计时器

    public void OnSprint_Loop_TurnDirEnter()
    {
        Sprint_Loop_TurnDirTimer = 0;
    }

    public void OnSprint_Loop_TurnDirUpdate()
    {
        Sprint_Loop_TurnDirTimer += Time.deltaTime;
    }

    public void OnSprint_Loop_TurnDirExit()
    {
        Sprint_Loop_TurnDirTimer = 0;
    }

    #endregion

    #region Sidestep_SprintStart中的计时器

    #endregion

    #endregion
}
