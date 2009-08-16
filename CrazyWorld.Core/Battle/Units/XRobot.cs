using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyWorld.Battle.Units
{
    public abstract class XRobot
    {

        /// <summary>
        /// 注册入战场
        /// </summary>
        /// <param name="Robot"></param>
        /// <param name="Env"></param>
        public void RegistedToWorld(IRobotControl Robot, IBattleEnv Env,IBattleMap Map)
        {
            this.Robot = Robot;
            this.Env = Env;
            this.Map = Map;
        }

        #region 保护方法
        /// <summary>
        /// 战斗开始
        /// </summary>
        public abstract void OnBattleStart();

        /// <summary>
        /// 战斗结束
        /// </summary>
        public abstract void OnBattleStop();

        /// <summary>
        /// 自己的回合
        /// </summary>
        public abstract void OnTurn();
        #endregion

        #region 保护属性
        protected IRobotControl Robot { get; set; }
        protected IBattleEnv Env { get; set; }
        protected IBattleMap Map { get; set; }
        #endregion

    }
}
