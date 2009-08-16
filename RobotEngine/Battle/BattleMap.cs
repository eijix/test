using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyWorld.Engine.Battle
{
    /// <summary>
    /// 战斗地图
    /// </summary>
    public class BattleMap : CrazyWorld.Battle.IBattleMap
    {
        /// <summary>
        /// 战场宽（单位距离）
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// 战场高（单位距离）
        /// </summary>
        public double Height { get; set; }

        #region 构造
        public BattleMap(double Width, double Height)
        {
            this.Width = Width;
            this.Height = Height;
        }
        #endregion
    }
}
