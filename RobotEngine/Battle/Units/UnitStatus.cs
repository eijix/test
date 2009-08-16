using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CrazyWorld.Engine.Battle.Units
{
    public class UnitStatus
    {
        public double X { get;set;}
        public double Y { get; set; }
        //public BattleUnit Unit { get; set; }

        public UnitStatus(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
            //this.Unit = Unit;
        }
    }

    /// <summary>
    /// 机器人瞬间状态
    /// </summary>
    public class RobotStatus : UnitStatus
    {
        public double Angle { get; set; }

        public RobotStatus(double X, double Y, double Angle)
            :base(X,Y)
        {
            this.Angle = Angle;
        }
    }
}
