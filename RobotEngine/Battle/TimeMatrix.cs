using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrazyWorld.Engine.Battle.Units;

namespace CrazyWorld.Engine.Battle
{
    /// <summary>
    /// 时间矩阵
    /// </summary>
    public class TimeMatrix
    {
        public class Frame
        {
            public ulong T ;
            public List<UnitStatus> Status = new List<UnitStatus>();
            public List<BattleUnit> Units = new List<BattleUnit>();

            public Frame(ulong Time)
            {
                T = Time;
            }

            public void AddUnit(BattleUnit Unit)
            {
                Status.Add(Unit.GetStatus());
                Units.Add(Unit);
            }
        }
        private Queue<Frame> _frames = new Queue<Frame>();
        private object _locker = new object();

        #region 构造
        public TimeMatrix(BattleWorld World)
        {
            
        }
        #endregion

        /// <summary>
        /// 添加一帧
        /// </summary>
        public void AddFrame(Frame Frame)
        {
            lock(_locker)
            {
                _frames.Enqueue(Frame);
            }
        }

        public Frame GetFrame()
        {
            lock (_locker)
            {
                if (_frames.Count > 0)
                    return _frames.Dequeue();
                else
                    return null;
            }
        }
    }
}
