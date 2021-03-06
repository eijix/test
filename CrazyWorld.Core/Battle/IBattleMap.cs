﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyWorld.Battle
{
    /// <summary>
    /// 战斗地图
    /// </summary>
    public interface IBattleMap
    {
        /// <summary>
        /// 战场宽（单位距离）
        /// </summary>
        double Width { get; }

        /// <summary>
        /// 战场高（单位距离）
        /// </summary>
        double Height { get; }
    }
}
