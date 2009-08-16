using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyRobot;
using CrazyWorld.Battle.Units;
using CrazyWorld.Engine.Battle;

namespace BattleConsole
{
    public partial class Form1 : Form
    {
        BattleGUI _battleGUIFrm = new BattleGUI();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyRobot.EijiBot bot = new EijiBot();
            BattleMap map = new BattleMap(400,300);
            BattleCfg cfg = new BattleCfg();
            cfg.UnitPixel = 2;
            cfg.FramsPerS = 100;
            cfg.RotateUnitTimeMs = 1;
            cfg.MoveUnitTimeMs = 2;    //10ms 一个时间单位
            cfg.CtlTickStep = 5;    //控制帧间隔,间隔 = (1000/FramsPerS) * CtlTickStep
            BattleEnv env = new BattleEnv();
            BattleWorld bw = new BattleWorld(new XRobot[] { bot}, map, cfg, env);
            
            bw.StartBattle();
            _battleGUIFrm.Start(bw);
            _battleGUIFrm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _battleGUIFrm.Show();
        }


    }
}
