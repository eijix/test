using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrazyWorld.Engine;

namespace CrazyWorld.Engine.Battle.Units
{
    /// <summary>
    /// 战斗元素抽象类
    /// </summary>
    public abstract class BattleUnit
    {
        #region 私有成员
        /// <summary>
        /// 时间矩阵
        /// </summary>
        private BattleWorld  _world;
        private double _area;
        private double _width;
        private double _height;
        #endregion

        #region 属性
        /// <summary>
        /// 唯一编号
        /// </summary>
        public Guid UnitID { get; set; }

        /// <summary>
        /// X坐标
        /// </summary>
        public double X { get; set; }
        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public double Width { get { return _width; } set { _width = value; _area = _width * _height; } }

        /// <summary>
        /// 高
        /// </summary>
        public double Height { get { return _height; } set { _height = value; _area = _width * _height; } }

        /// <summary>
        /// 移动速度(0 < n < 2) 0:最快
        /// </summary>
        public double Speed { get; set; }

        /// <summary>
        /// 是否死亡
        /// </summary>
        public bool IsDead { get; set; }

        /// <summary>
        /// 当前方向角度
        /// </summary>
        public double Angle { get;set;}

        /// <summary>
        /// 面积
        /// </summary>
        public double Area { get { return _area; } }
        #endregion

        /// <summary>
        /// 撞击到其他物体
        /// </summary>
        /// <param name="Unit"></param>
        public abstract void OnHitAnother(BattleUnit Unit);

        /// <summary>
        /// 被其他物体撞到
        /// </summary>
        /// <param name="Unit"></param>
        public abstract void OnHitByAnother(BattleUnit Unit);

        /// <summary>
        /// 撞墙
        /// </summary>
        public abstract void OnHitWall();

        /// <summary>
        /// 战斗开始
        /// </summary>
        public abstract void OnBattleStart();

        /// <summary>
        /// 战斗结束
        /// </summary>
        public abstract void OnBattleEnd();

        /// <summary>
        /// 当前回合
        /// </summary>
        public abstract void OnTurn();

        /// <summary>
        /// 向前移动
        /// </summary>
        /// <param name="Distance">距离</param>
        public abstract void GoAhead(double Distance);

        /// <summary>
        /// 向后移动
        /// </summary>
        /// <param name="Distance">距离</param>
        public abstract void GoBack(double Distance);

        /// <summary>
        /// 当前回合结束,计算当前这个回合所产生的变化
        /// </summary>
        public abstract void OnTrunFinished();

        /// <summary>
        /// 系统回合,只执行预先设定的动作
        /// </summary>
        public abstract void OnSysTurn();

        /// <summary>
        /// 当前角度
        /// </summary>
        /// <param name="Angle">角度</param>
        public abstract void SetAngle(double Angle);

        /// <summary>
        /// 停止当前正在执行的动作
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// 取得物件的当前状态
        /// </summary>
        public abstract UnitStatus GetStatus();

        /// <summary>
        /// 检测是否碰撞
        /// </summary>
        /// <param name="UnitA"></param>
        /// <param name="UnitB"></param>
        /// <returns></returns>
        public static bool IsCollision(BattleUnit UnitA,BattleUnit UnitB)
        {
            if (Math.Abs(UnitA.X - UnitB.X) < (UnitA.Width + UnitB.Width) / 2
              && Math.Abs(UnitA.Y - UnitB.Y) < (UnitA.Height + UnitB.Height) / 2)
                return true;
            else
                return false;
        }

        #region 构造
        public BattleUnit(BattleWorld World)
        {
            UnitID = Guid.NewGuid();
            _world = World;
        }
        #endregion




    }
}
