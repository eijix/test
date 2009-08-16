using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyWorld.Battle.Units
{
    public enum RobotActionType
    {
        GoAhead,
        GoBack,
        SetAngle,
        TurnLeft,
        TurnRight,
        Fire
    }
    public enum RobotActionStatus
    {
        NotStart,
        Running,
        Finished
    }
    public interface IRobotControl
    {
        /// <summary>
        /// 向前走
        /// </summary>
        /// <param name="Distance">距离</param>
        void GoAhead(double Distance);

        /// <summary>
        /// 向前走
        /// </summary>
        /// <param name="Distance">距离</param>
        void GoBack(double Distance);

        /// <summary>
        /// 设置方向
        /// </summary>
        /// <param name="Angle">绝对角度 0:左</param>
        void SetAngle(double Angle);

        /// <summary>
        /// 左转
        /// </summary>
        /// <param name="Angle">相对角度</param>
        void TurnLeft(double Angle);

        /// <summary>
        /// 右转
        /// </summary>
        /// <param name="Angle">相对角度</param>
        void TurnRight(double Angle);


        /// <summary>
        /// 当前方向角度
        /// </summary>
        double Angle { get; }

        /// <summary>
        /// 生命
        /// </summary>
        double HP { get; }

        /// <summary>
        /// X
        /// </summary>
        double X { get; }

        /// <summary>
        /// Y
        /// </summary>
        double Y { get; }

        /// <summary>
        /// 当前正在执行的动作
        /// </summary>
        RobotActionType? CurrentActionType { get; }

        /// <summary>
        /// 当前执行动作的状态
        /// </summary>
        RobotActionStatus? CurrentActionStatus { get; }
    }
}
