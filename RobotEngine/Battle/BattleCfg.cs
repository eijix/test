using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyWorld.Engine.Battle
{
    /// <summary>
    /// 战场配置
    /// </summary>
    public class BattleCfg
    {
        /// <summary>
        /// 每帧所需要的时间
        /// </summary>
        private uint _oneFramMs;

        private int _framsPers;

        /// <summary>
        /// 每秒帧数
        /// </summary>
        public int FramsPerS 
        { 
            get { return _framsPers; }
            set{ 
                _framsPers = value;
                _oneFramMs = (uint)((double)1000 / (double)_framsPers);
            }
        }
        

        /// <summary>
        /// 单位距离所对应的像素
        /// </summary>
        public int UnitPixel { get; set; }

        /// <summary>
        /// 一个移动时间单位(MS - 一个DISTANCE)
        /// </summary>
        public int MoveUnitTimeMs { get; set; }

        /// <summary>
        /// 一个旋转时间单位(MS - 1度)
        /// </summary>
        public int RotateUnitTimeMs { get; set; }

        /// <summary>
        /// 每帧所需要的时间
        /// </summary>
        public uint OneFramMs { get { return _oneFramMs; } }

        /// <summary>
        /// 控制帧间隔 间隔时间 = (1000/FramsPerS) * CtlTickStep
        /// </summary>
        public int CtlTickStep { get; set; }

    }
}
