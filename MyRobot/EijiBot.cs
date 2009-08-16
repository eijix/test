using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrazyWorld.Battle.Units;

namespace MyRobot
{
    public class EijiBot:XRobot
    {
        private int cnt = 0;
        public override void OnBattleStart()
        {
            //throw new NotImplementedException();
            Console.WriteLine("Started");
        }

        public override void OnBattleStop()
        {
            Console.WriteLine("Stopped");
        }

        public override void OnTurn()
        {
            if (!Robot.CurrentActionType.HasValue)
            {
                //Console.WriteLine("MyTurn");
                switch (cnt)
                {
                    case 0:
                        Robot.TurnRight(90);
                        break;
                    case 1:
                        Robot.GoAhead(50);
                        break;
                }
                cnt++;
                if (cnt > 1) cnt = 0;
            }
            /*
            if (!Robot.CurrentActionType.HasValue)
            {
                //Robot.GoAhead(10);  //前进10
                Robot.TurnLeft(60);
                Random r = new Random();
                //Robot.TurnLeft(r.Next(0, 360));
            }*/
        }
    }
}
