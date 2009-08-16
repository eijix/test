using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrazyWorld.Battle.Units;
using CrazyWorld.Engine.Battle.Units;
using System.Threading;

namespace CrazyWorld.Engine.Battle
{
    /// <summary>
    /// 战场类
    /// </summary>
    public class BattleWorld
    {
        #region 私有成员
        private Dictionary<Guid, BU_Robot> _robs = new Dictionary<Guid,BU_Robot>();             //机器人集合
        private Dictionary<Guid, BattleUnit> _units = new Dictionary<Guid, BattleUnit>();       //所有物件集合
        private TimeMatrix _matrix;
        private Thread _battleThread;
        private System.Diagnostics.Stopwatch _sw = new System.Diagnostics.Stopwatch();
        #endregion

        #region 构造
        public BattleWorld(XRobot[] Robots,BattleMap Map,BattleCfg Cfg,BattleEnv Env)
        {
            //_robs = new BU_Robot[Robots.Length];
            foreach (XRobot bot in Robots)
            {
                //添加机器人到战场
                AddUnit(new BU_Robot(bot, this));
            }

            _matrix = new TimeMatrix(this);
            this.Config = Cfg;
            this.Map = Map;
            this.Env = Env;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 开始战斗
        /// </summary>
        public void StartBattle()
        {
            _battleThread = new Thread(new ThreadStart(BattleProc));
            _battleThread.IsBackground = true;
            _battleThread.Start();
        }

        /// <summary>
        /// 终止战斗
        /// </summary>
        public void StopBattle()
        {
        }

        #endregion

        #region 属性
        public BattleCfg Config { get; set; }
        public BattleEnv Env { get; set; }
        public BattleMap Map { get; set; }
        public TimeMatrix TimeMatrix { get { return _matrix; } }
        #endregion


        #region 私有函数
        /// <summary>
        /// 战斗线程
        /// </summary>
        private void BattleProc()
        {
            Random r = new Random();
            foreach (BU_Robot rob in _robs.Values)
            {
                //随机位置
                rob.X = r.NextDouble() * Map.Width;
                rob.Y = r.NextDouble() * Map.Height;

                rob.OnBattleStart();
            }

            Env.Ticket = 0;
            //回合循环
            while (true)
            {
                _sw.Reset();
                _sw.Start();
                //一个回合，循环所有机器人,
                //只有获得控制权的时候才会执行
                foreach (BU_Robot rob in _robs.Values)
                {
                    if (!rob.IsDead && rob.CurrentActionType == null)
                    {
                        rob.OnTurn();
                        //计算本轮结果
                        rob.OnTrunFinished();
                    }
                }
                //if (Env.Ticket % (ulong)Config.CtlTickStep ==  0)
                //{
                //    foreach (BU_Robot rob in _robs.Values)
                //    {
                //        if (!rob.IsDead && rob.CurrentActionType == null)
                //        {
                //            rob.OnTurn();
                //            //计算本轮结果
                //            rob.OnTrunFinished();
                //        }
                //    }
                //}

                //每个回合都要执行
                foreach (BattleUnit  unit in _units.Values)
                {
                    if (!unit.IsDead)
                    {
                        //分步执行设定好的动作
                        unit.OnSysTurn();
                    }
                }
                
                //计算当前回合(帧)所产生的影响
                BU_Robot[] tmpBots = _robs.Values.ToArray();
                //检测撞墙
                foreach (BU_Robot bot in tmpBots)
                {
                    if (bot.X > Map.Width)
                    {

                    }
                }

                //检测机器人间碰撞
                for (int i = 0; i < tmpBots.Length - 1; i++)    //两两相比
                {
                    for (int k = 1; k < tmpBots.Length; k++)
                    {
                        //检测I与K是否碰撞
                        if (BattleUnit.IsCollision(tmpBots[i], tmpBots[k]))
                        {
                            //撞了,总共扣 20HP,按照面积比分配,面积大扣的少
                            double sum = tmpBots[i].Area + tmpBots[k].Area;

                            tmpBots[i].HP -= 20 * (1.0 - tmpBots[i].Area / sum);
                            tmpBots[k].HP -= 20 * (1.0 - tmpBots[k].Area / sum);
                            
                            //让他们全停下来
                            tmpBots[i].Stop();
                            tmpBots[k].Stop();

                            //调用他们的碰撞事件
                            tmpBots[i].OnHitByAnother(tmpBots[k]);
                            tmpBots[k].OnHitByAnother(tmpBots[i]);

                            ////分开他们两
                            //double xDiff = Math.Abs( tmpBots[i].X - tmpBots[k].X);
                            //double yDiff = tmpBots[i].Y - tmpBots[k].Y;



                            //if (Math.Abs(xDiff) > Math.Abs(yDiff))
                            //{
                            //    //横向分开
                            //    if (tmpBots[i].Area > tmpBots[k].Area)
                            //    {
                            //        //i比k大,i 不动k动,取消k动作
                            //        double distance;

                            //    }
                            //    else if (tmpBots[i].Area < tmpBots[k].Area)
                            //    {
                            //        //k比i大,k 不动i动,取消i动作

                            //    }
                            //    else
                            //    {
                            //        //一样大,每人动一半,都取消动作

                            //    }
                            //}
                            //else
                            //{
                            //    //纵向分开
                            //}
                        }
                        else
                        {
                            //没撞
                        }
                    }
                }

                //将所有活着的加入TIMEMATRIX
                BattleUnit[] units = _units.Values.ToArray();
                TimeMatrix.Frame f = new TimeMatrix.Frame(Env.Ticket);
                foreach (BattleUnit unit in units)
                {
                    if (!unit.IsDead)
                    {
                        
                        f.AddUnit(unit);
                    }
                    else
                    {
                        //已经死亡的物件从集合中清除
                        RemoveUnit(unit);
                    }
                }
                _matrix.AddFrame(f);
                _sw.Stop();
                int slep = (int)Config.OneFramMs - (int)_sw.ElapsedMilliseconds;
                if(slep >0)
                    Thread.Sleep(slep);

                Env.Ticket++;
            }
        }

        /// <summary>
        /// 向战场添加一个物件
        /// </summary>
        /// <param name="Unit">物件</param>
        private void AddUnit(BattleUnit Unit)
        {
            BU_Robot bot = Unit as BU_Robot;
            if (bot != null)
                _robs.Add(Unit.UnitID, bot);

            _units.Add(Unit.UnitID, bot);
        }

        /// <summary>
        /// 清除一个物件
        /// </summary>
        /// <param name="Unit"></param>
        private void RemoveUnit(BattleUnit Unit)
        {
            if (_robs.ContainsKey(Unit.UnitID))
                _robs.Remove(Unit.UnitID);

            if (_units.ContainsKey(Unit.UnitID))
                _units.Remove(Unit.UnitID);
        }
        #endregion
    }
}
