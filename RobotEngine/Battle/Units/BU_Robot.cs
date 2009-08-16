using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrazyWorld.Battle.Units;

namespace CrazyWorld.Engine.Battle.Units
{
    public class BU_Robot:BattleUnit,IRobotControl
    {

        class RobotAction
        {
            public RobotActionType Type;
            public RobotActionStatus Status;
            public object P1;
            public object P2;
            public object P3;

            public RobotAction(RobotActionType Type, object P1, object P2, object P3)
            {
                this.Type = Type;
                this.P1 = P1;
                this.P2 = P2;
                this.P3 = P3;
                Status =  RobotActionStatus.NotStart;

            }
        }
        #region 私有成员
        private XRobot _xRob;
        private BattleWorld _world;
        private RobotAction _curAction = null;                                      //当前任务
        private Queue<RobotStatus> _curActionSteps = new Queue<RobotStatus>();      //未来所有状态
        private double _hp;
        #endregion

        #region 构造
        public BU_Robot(XRobot XRobot, BattleWorld World)
            : base(World)
        {
            _xRob = XRobot;
            _world = World;
            XRobot.RegistedToWorld(this, World.Env,World.Map);

            //机器人默认行进速度1.0
            Speed = 2;

            _hp = 100;
        }
        #endregion

        /// <summary>
        /// 击中别人
        /// </summary>
        /// <param name="Unit"></param>
        public override void OnHitAnother(BattleUnit Unit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 被别人击中
        /// </summary>
        /// <param name="Unit"></param>
        public override void OnHitByAnother(BattleUnit Unit)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 撞墙
        /// </summary>
        public override void OnHitWall()
        {
            throw new NotImplementedException();
        }

        public override void GoAhead(double Distance)
        {
            //验证


            _curAction = new RobotAction(RobotActionType.GoAhead, Distance, null, null);
            
        }

        public override void GoBack(double Distance)
        {
            //验证

            _curAction = new RobotAction(RobotActionType.GoBack, Distance, null, null);
        }
        public void TurnLeft(double Angle)
        {
            //验证

            _curAction = new RobotAction(RobotActionType.TurnLeft, Angle, null, null);
        }

        public void TurnRight(double Angle)
        {
            //验证

            _curAction = new RobotAction(RobotActionType.TurnRight, Angle, null, null);
        }
        /// <summary>
        /// 停止当前正在执行的动作
        /// </summary>
        public override void Stop()
        {
            _curAction = null;
            _curActionSteps.Clear();
        }

        public double HP
        {
            get { return _hp; }
            set { 
                _hp = value;
                if (_hp <= 0)
                    IsDead = true;
            
            }
            
        }
        public override void SetAngle(double Angle)
        {
            throw new NotImplementedException();
        }



        public override void OnBattleStart()
        {
            
            _xRob.OnBattleStart();
        }

        public override void OnBattleEnd()
        {
            _xRob.OnBattleStop();
        }

        public override void OnTurn()
        {
            _xRob.OnTurn();
        }

        /// <summary>
        /// 当前回合结束,计算当前这个回合所产生的变化
        /// </summary>
        public override void OnTrunFinished()
        {
            if (_curAction != null && _curAction.Status ==  RobotActionStatus.NotStart)
            {
                _curActionSteps.Clear();
                switch (_curAction.Type)
                {
                    case RobotActionType.GoAhead:
                        CalAct_GoAhead((double)_curAction.P1);
                        break;
                    case RobotActionType.GoBack:
                        CalAct_GoBack((double)_curAction.P1);
                        break;
                    case RobotActionType.TurnLeft:
                        CalAct_TurnLeft((double)_curAction.P1);
                        break;
                    case RobotActionType.TurnRight:
                        CalAct_TrunRight((double)_curAction.P1);
                        break;
                    default:
                        throw new Exception("Can't support action \"" + _curAction.Type.ToString() + "\"");
                }
                _curAction.Status =  RobotActionStatus.Running;
            }
        }


        /// <summary>
        /// 计算向前走
        /// </summary>
        /// <param name="Distance"></param>
        private void CalAct_GoAhead(double Distance)
        {
            Go(Distance);
        }

        /// <summary>
        /// 前后走
        /// </summary>
        /// <param name="Distance"></param>
        private void Go(double Distance)
        {
            double tmpDistance, tmpAngle;
            if (Distance >= 0)
            {
                tmpDistance = Distance;
                tmpAngle = Angle;
            }
            else
            {
                tmpDistance = Distance * -1;
                tmpAngle = (Angle + 180) % 360;
            }

            //计算所需要的时间
            //假设每一个Distance为一个时间单位 Time = Distance * Speed * UnitTimeMs;
            double time = tmpDistance * Speed * _world.Config.MoveUnitTimeMs;
            //计算所需要的帧数 Frames = Time / (1000 / FramsPerS )
            double frms = time / (1000 / _world.Config.FramsPerS);
            if (frms < 1) frms = 1.0;   //帧数不能小于1
            //每一步按照帧数等分
            double step = tmpDistance / (frms * _world.Config.UnitPixel);

            for (double cur = step; cur <= tmpDistance; cur += step)
            {
                double oy = Math.Sin(ToRadian(tmpAngle)) * cur;
                double ox = Math.Cos(ToRadian(tmpAngle)) * cur;

                _curActionSteps.Enqueue(new RobotStatus(X + ox, Y - oy, Angle));
            }
        }

        /// <summary>
        /// 计算向后走
        /// </summary>
        /// <param name="Distance"></param>
        private void CalAct_GoBack(double Distance)
        {
            Go(Distance * -1);
        }

        /// <summary>
        /// 左转
        /// </summary>
        /// <param name="Angle"></param>
        private void CalAct_TurnLeft(double Angle)
        {
            //计算所需要的时间
            //假设每一个Angle为一个时间单位 Time = Angle * Speed * RotateUnitTimeMs;
            double time = Angle * Speed * _world.Config.RotateUnitTimeMs;
            //计算所需要的帧数 Frames = Time / (1000 / FramsPerS )
            double frms = time / (1000 / (double)_world.Config.FramsPerS);
            double tagAngle = this.Angle + Angle;
            if (frms < 1) frms = 1.0;   //帧数不能小于1
            //每一步按照帧数等分
            double step = Angle / frms;
            for (double tmpAngle = this.Angle + step; tmpAngle <= tagAngle; tmpAngle += step)
            {
                //tmpAngle = tmpAngle % 360;
                _curActionSteps.Enqueue(new RobotStatus(X, Y, tmpAngle % 360));
            }
        }

        /// <summary>
        /// 右转
        /// </summary>
        /// <param name="Angle"></param>
        private void CalAct_TrunRight(double Angle)
        {
            //计算所需要的时间
            //假设每一个Angle为一个时间单位 Time = Angle * Speed * RotateUnitTimeMs;
            double time = Angle * Speed * _world.Config.RotateUnitTimeMs;
            //计算所需要的帧数 Frames = Time / (1000 / FramsPerS )
            double frms = time / (1000 / (double)_world.Config.FramsPerS);
            double tagAngle = this.Angle - Angle;
            
            if (frms < 1) frms = 1.0;   //帧数不能小于1
            //每一步按照帧数等分
            double step = Angle / frms;
            for (double tmpAngle = this.Angle + step; tmpAngle >= tagAngle; tmpAngle -= step)
            {
                //tmpAngle = tmpAngle % 360;
                _curActionSteps.Enqueue(new RobotStatus(X, Y,tmpAngle < 0 ? 360+tmpAngle : tmpAngle));
            }
        }

       

        public void CancelAction()
        {
            _curAction = null;
            _curActionSteps.Clear();
        }

        /// <summary>
        /// 系统回合
        /// </summary>
        public override void OnSysTurn()
        {
            if (_curActionSteps.Count > 0)
            {
                RobotStatus rs = _curActionSteps.Dequeue();
                //设定机器人当前状态
                X = rs.X;
                Y = rs.Y;
                Angle = rs.Angle;
                //AddFrameToMatrix(_world.Env.Ticket, rs);
            }
            else
            {
                _curAction = null;
            }
        }

        #region IRobotControl 成员


        public RobotActionType? CurrentActionType
        {
            get {
                if (_curAction != null)
                    return _curAction.Type;
                else
                    return null;
            
            }
        }

        public RobotActionStatus? CurrentActionStatus
        {
            get {
                if (_curAction != null)
                    return _curAction.Status;
                else
                    return null;
            }
        }

        #endregion



        public override UnitStatus GetStatus()
        {
            RobotStatus ret = new RobotStatus(X, Y, Angle);
            return ret;
        }

        private static double ToRadian(double Angle)
        {
            return (Angle * Math.PI) / 180.00;
        }
    }
}
