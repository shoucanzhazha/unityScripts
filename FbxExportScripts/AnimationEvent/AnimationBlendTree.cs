using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBlendTree : MonoBehaviour
{
    //组件
    public Animator characterAnimator;
    /// <summary>
    /// Random_Action
    /// </summary>
    protected int Random_ActionID = Animator.StringToHash("Random_Action");
    public int Random_Action;




    /// <summary>
    /// Blend_Tree
    /// </summary>
    //MoveStatus
    protected int MoveStatusID = Animator.StringToHash("MoveStatus");
    protected int Blend_MoveStatusID = Animator.StringToHash("Blend_MoveStatus");

    // Horizontal & Vertical
    protected int HorizontalID = Animator.StringToHash("Horizontal");
    protected int VerticalID = Animator.StringToHash("Vertical");
    protected int Blend_HorizontalID = Animator.StringToHash("Blend_Horizontal");
    protected int Blend_VerticalID = Animator.StringToHash("Blend_Vertical");

    //Walk 
    public float Blend_Walk;
    public float Blend_Walk_Origin;
    public bool AnimationOnWalk;
    protected int Blend_WalkID = Animator.StringToHash("Blend_Walk");

    //Run_Start
    public float Blend_RunStart;
    public float Blend_RunStart_Origin;
    public bool AnimationOnRunStart;
    protected int Blend_RunStartID = Animator.StringToHash("Blend_RunStart");

    //Run_Loop
    public float Blend_RunLoop;
    public float Blend_RunLoop_Origin;
    public bool AnimationOnRunLoop;
    protected int Blend_RunLoopID = Animator.StringToHash("Blend_RunLoop");

    //Fall_Stop
    public float Blend_FallStop;
    public float Blend_FallStop_Origin;
    public bool AnimationOnFallStop;
    protected int Blend_FallStopID = Animator.StringToHash("Blend_FallStop");

    //Fall_Roll
    public float Blend_FallRoll;
    public float Blend_FallRoll_Origin;
    protected int Blend_FallRollID = Animator.StringToHash("Blend_FallRoll");


    /// <summary>
    /// AnimationTime
    /// </summary>
    protected int SidestepTimeID = Animator.StringToHash("SidestepTime");
    public float SidestepTime;




    #region 生命周期函数
    private void Awake()
    {
        characterAnimator = GetComponent<Animator>();
        Blend_Tree_Init();
    }

    private void Update()
    {
        Blend_Tree_Update();
        Random_ActionUpdate();
    }
    #endregion

    /// <summary>
    /// 混乱树初始化
    /// </summary>
    public void Blend_Tree_Init()
    {
        Blend_Walk_Origin = characterAnimator.GetFloat(Blend_WalkID);
        Blend_RunStart_Origin = characterAnimator.GetFloat(Blend_RunStartID);
        Blend_RunLoop_Origin = characterAnimator.GetFloat(Blend_RunLoopID);
        Blend_FallStop_Origin = characterAnimator.GetFloat(Blend_FallStopID);
        Blend_FallRoll_Origin = characterAnimator.GetFloat(Blend_FallRollID);
    }

    /// <summary>
    /// 混乱树更新
    /// </summary>
    public void Blend_Tree_Update()
    {
        characterAnimator.SetFloat(Blend_WalkID, Blend_Walk);
        characterAnimator.SetFloat(Blend_RunStartID, Blend_RunStart);
        characterAnimator.SetFloat(Blend_RunLoopID, Blend_RunLoop);
        characterAnimator.SetFloat(Blend_FallStopID, Blend_FallStop);
        characterAnimator.SetFloat(Blend_FallRollID, Blend_FallRoll);

    }

    /// <summary>
    /// 随机动作
    /// </summary>
    public void Random_ActionUpdate()
    {
        characterAnimator.SetFloat(Random_ActionID, Random_Action);
    }
    
    /// <summary>
    /// 设置Blend_MoveStatus
    /// </summary>
    public void SetBlend_MoveStatus()
    {
        characterAnimator.SetFloat(Blend_MoveStatusID,characterAnimator.GetInteger(MoveStatusID));
    }
    public int GetBlend_MoveStatus()
    {
        return (int)characterAnimator.GetFloat(Blend_MoveStatusID);
    }

    /// <summary>
    /// 设置Blend_HorVert
    /// </summary>
    public void SetBlend_HorVert()
    {
        float hor = Mathf.Abs(characterAnimator.GetFloat(HorizontalID)) > Mathf.Max(Mathf.Abs(characterAnimator.GetFloat(VerticalID)), 0.6f) ? Mathf.Sign(characterAnimator.GetFloat(HorizontalID)) : 0;
        float vert = Mathf.Abs(characterAnimator.GetFloat(VerticalID)) > Mathf.Max(Mathf.Abs(characterAnimator.GetFloat(HorizontalID)), 0.7f) ? Mathf.Sign(characterAnimator.GetFloat(VerticalID)) : 0;
        characterAnimator.SetFloat(Blend_HorizontalID, hor);
        characterAnimator.SetFloat(Blend_VerticalID, vert);
    }



    #region Walk_Loop中控制Walk_Stop动作的帧事件
    public void Blend_Walk_Event(int EventValue)
    {
        if (AnimationOnWalk)
        {
            Blend_Walk = EventValue;
        }
    }

    public void OnWalk_LoopEnter()
    {
        AnimationOnWalk = true;
        Blend_Walk = Blend_Walk_Origin;
    }

    public void OnWalk_StopEnter()
    {
        AnimationOnWalk = false;
    }
    #endregion

    #region Run_Start中控制WalkStart_Stop动作的帧事件
    public void Blend_RunStart_Event(int EventValue)
    {
        if (AnimationOnRunStart)
        {
            Blend_RunStart = EventValue;
        }
    }

    public void OnRun_StartEnter()
    {
        AnimationOnRunStart = true;
        Blend_RunStart = Blend_RunStart_Origin;

    }

    public void OnRunStart_StopEnter()
    {
        AnimationOnRunStart = false;

    }
    #endregion

    #region Run_Loop中控制RunLoop_Stop动作的帧事件
    public void Blend_RunLoop_Event(int EventValue)
    {
        if (AnimationOnRunLoop)
        {
            Blend_RunLoop = EventValue;
        }
    }

    public void OnRun_LoopEnter()
    {
        AnimationOnRunLoop = true;
        Blend_RunLoop = Blend_RunLoop_Origin;

    }

    public void OnRunLoop_StopEnter()
    {
        AnimationOnRunLoop = false;

    }
    #endregion

    #region Fall_Stop中控制FallStop_RunStop动作帧事件

    public void Blend_FallStop_Event(int EventValue)
    {
        if (AnimationOnFallStop)
        {
            Blend_FallStop = EventValue;
        }
    }

    public virtual void OnFall_StopEnter()
    {
        AnimationOnFallStop = true;
        Blend_FallStop = Blend_FallStop_Origin;
        SetBlend_MoveStatus();

        //Blend_MoveStatusUpdate = false;

    }

    public void OnFallStop_RunStopEnter()
    {
        AnimationOnFallStop = false;
    }


    #endregion

    #region Fall_Roll中控制Fall_Roll中的动作的帧事件
    public void OnFall_Roll_Event()
    {
        if (characterAnimator.GetInteger(MoveStatusID) > 0)
        {
            Blend_FallRoll = 1;
        }
        else
        {
            Blend_FallRoll = 0;
        }
    }

    public void OnFall_RollEnter()
    {
        Blend_FallRoll = Blend_FallRoll_Origin;
    }

    #endregion

    #region 锁定Jump_1起跳时的动作阻止Blend_MoveStatus更新  

    public void OnJump_1_Start_0Enter()
    {
        //Blend_MoveStatusUpdate = false;
        SetBlend_MoveStatus();
    }
    public void OnJump_1_Start_1Enter()
    {
        //Blend_MoveStatusUpdate = false;
        SetBlend_MoveStatus();
        Random_Action = Random.Range(0, 2);
    }
    #endregion

    #region 锁定Jump_2起跳时的动作阻止Blend_MoveStatus更新
    public void OnJump_2Enter()
    {
        SetBlend_MoveStatus();
    }

    #endregion

    #region 锁定Fall_Start坠落动作阻止Blend_MoveStatus更新

    public void OnFall_StartEnter()
    {
        SetBlend_MoveStatus();
    }


    #endregion

    #region 锁定落地时的动作阻止Blend_MoveStatus更新

    //public void OnFall_StopEnter()
    //{
    //    SetBlend_MoveStatus();
    //}

    #endregion

    #region 锁定Sidestep_Start开始闪避动作阻止Blend_Hor_Vert更新
    public void OnSidestep_StartEnter()
    {
        characterAnimator.SetInteger(MoveStatusID, 0);
        SetBlend_HorVert();
        //Blend_HorVertUpdate = false;
    }

    public void OnSidestep_LoopExit()
    {
        //Blend_HorVertUpdate= true;
    }

    #endregion

    #region Jump_1_Start_1中随机选择动作
    //public void OnJump_1_Start_1Enter()
    //{
    //    Random_Action = Random.Range(0, 3);
    //}


    #endregion


}
