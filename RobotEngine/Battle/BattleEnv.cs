using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyWorld.Engine.Battle
{
    /// <summary>
    /// 战场环境
    /// </summary>
    public class BattleEnv : CrazyWorld.Battle.IBattleEnv
    {
        /// <summary>
        /// 战场宽（单位距离）
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 战场高（单位距离）
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// 回合数,每过一回合Ticket+1
        /// </summary>
        public ulong Ticket { get; set; }
    }
}
