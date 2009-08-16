using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectDraw;
using CrazyWorld.Engine.Battle;
using CrazyWorld.Engine.Battle.Units;
using System.Threading;
using System.Drawing;


namespace BattleConsole
{
    public partial class BattleGUI : Form
    {
        private Device display;

        private Surface front = null;
        private Surface back = null;
        private Surface title = null;
        private Surface text = null;
        private Clipper clip = null;

        private long frmCnt = 0;
        private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

        private Thread drawThread;
        private bool drawing;

        private Surface[] _tankBody = new Surface[360];

        /// <summary>
        /// 战场
        /// </summary>
        private BattleWorld _batWorld;

        private TimeMatrix.Frame _curFrame = null;

        private int lastTick = System.Environment.TickCount;

        /// <summary>
        /// 显示单位(像素)
        /// </summary>
        public int DisplayUnit = 2;


        public BattleGUI()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            InitDDraw();
            LoadPics();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            DrawFrame();
        }

        public void Start(BattleWorld World)
        {
            _batWorld = World;
            SetBoundsCore(0, 0, ToPixel(_batWorld.Map.Width), ToPixel(_batWorld.Map.Height), BoundsSpecified.All);
            sw.Start();
            //tmAmi.Enabled = true;
            drawing = true;
            drawThread = new Thread(new ThreadStart(DrawProc));
            drawThread.Priority = ThreadPriority.Highest;
            drawThread.IsBackground = true;
            drawThread.Start();
        }

        public void Stop()
        {
            drawing = false;
            drawThread.Join();
            _batWorld = null;
            tmAmi.Enabled = false;
        }

        /// <summary>
        /// 初始化DirectDraw
        /// </summary>
        private void InitDDraw()
        {
            #region 初始化视频
            // Init the Device
            display = new Device(CreateFlags.HardwareOnly);
            
            // Set the Cooperative Level and parent, Setted to Full Screen Exclusive to the form)
            display.SetCooperativeLevel(this, CooperativeLevelFlags.Normal);
            // Set the resolution and color depth used in full screen(640x480, 16 bit color)
            //display.SetDisplayMode(640, 480, 16, 0, false);

            #endregion


            // Used to describe a Surface
            SurfaceDescription description = new SurfaceDescription();


            description.SurfaceCaps.PrimarySurface = true;
            //description.SurfaceCaps.Flip = true;
            //description.SurfaceCaps.Complex = true;
            // Set the Back Buffer count
            //description.BackBufferCount = 1;
            // Create the Surface with specifed description and device)
            front = new Surface(description, display);

            description.Clear();

            description.Width = front.SurfaceDescription.Width;
            description.Height = front.SurfaceDescription.Height;
            description.SurfaceCaps.OffScreenPlain = true;
            back = new Surface(description, display);

            // Create the Clipper
            clip = new Clipper(display);
            /// Set the region to this form
            clip.Window = this;
            //clip.ClipList = new Rectangle[] { new Rectangle(10, 10, 100, 100) };
            // Set the clipper for the front Surface
            front.Clipper = clip;

            description.Clear();
            description.SurfaceCaps.VideoMemory = true;
            description.Width = 100;
            description.Height = 100;
            description.SurfaceCaps.OffScreenPlain = true;
            title = new Surface(description, display);
            title.ColorFill(System.Drawing.Color.Tomato);
            title.ForeColor = System.Drawing.Color.Blue;
            title.FillColor = System.Drawing.Color.Yellow;
            title.DrawBox(0, 0, 10, 10);

        }

        private void UninitDDraw()
        {
            display.RestoreDisplayMode();

        }

        private void Draw()
        {
            Rectangle rc = this.RectangleToScreen(ClientRectangle);
            //RectangleToScreen(new Rectangle(0, 0, 1, 1));
            //display.RestoreAllSurfaces();
            back.ColorFill(System.Drawing.Color.Tan);
            back.DrawFast(rc.Left, rc.Top, title, DrawFastFlags.Wait);
            //back.DrawLine(10, 10, 50, 50);
            //back.FillColor = System.Drawing.Color.Blue;
            front.Draw(back, DrawFlags.Wait);

            //front.Flip(back, FlipFlags.Wait);
            //front.DrawFast(0, 0, back,new Rectangle(0,0,100,100), DrawFastFlags.Wait);
            //front.ColorFill(System.Drawing.Color.Red);

            //back.FillColor = System.Drawing.Color.Red;
            //back.DrawBox(0, 0, 100, 100);
            //back.FillColor = System.Drawing.Color.Red;
            //back.DrawBox(0, 0, 100, 100);

        }


        private int ToPixel(double UnitData)
        {
            return (int)(UnitData * _batWorld.Config.UnitPixel);
        }

        private void tmAmi_Tick(object sender, EventArgs e)
        {
            TimeMatrix.Frame tmpFrame = _batWorld.TimeMatrix.GetFrame();
            if (tmpFrame != null)
            {
                _curFrame = tmpFrame;
                DrawFrame();
            }
        }

        private void DrawFrame()
        {
            if (_curFrame != null)
            {
                Console.WriteLine(System.Environment.TickCount - lastTick);
                lastTick = System.Environment.TickCount;
                frmCnt++;

                back.ColorFill(System.Drawing.Color.AliceBlue);
                //画出所有物件
                for (int i = 0; i < _curFrame.Units.Count;i++ )
                {
                    BattleUnit unit = _curFrame.Units[i];
                    UnitStatus status = _curFrame.Status[i];
                    if (unit is BU_Robot)
                    {
                        DrawRobot((RobotStatus)status, (BU_Robot)unit,back);
                    }
                }

                //送到主画面
                front.Draw(back, DrawFlags.Wait);
            }
            else
            {
                //存黑
                back.ColorFill(System.Drawing.Color.Black);
                front.Draw(back, DrawFlags.Wait);
            }
        }

        private void DrawRobot(RobotStatus Status, BU_Robot Robot,Surface Surface)
        {
            Rectangle rc = this.RectangleToScreen(ClientRectangle);
            //Rectangle rect = new Rectangle(ToPixel(Status.X), ToPixel(Status.Y), 20, 20);
            int left = ToPixel(Status.X) + rc.Left;
            int top = ToPixel(Status.Y) + rc.Top;
            int right = left + _tankBody[(int)Status.Angle].SurfaceDescription.Width;
            int bottom = top + _tankBody[(int)Status.Angle].SurfaceDescription.Height;

//            Surface.ForeColor = System.Drawing.Color.Blue;
//            Surface.FillColor = System.Drawing.Color.Yellow;
//            Surface.DrawBox(left, top, right, bottom);Surface

            if (left >= 0 && top >= 0 && right <= Surface.SurfaceDescription.Width && bottom <= Surface.SurfaceDescription.Height)
                Surface.DrawFast(left, top, _tankBody[(int)Status.Angle], DrawFastFlags.Wait | DrawFastFlags.SourceColorKey);
        }

        private void DrawProc()
        {
            while (drawing)
            {
                TimeMatrix.Frame tmpFrame = _batWorld.TimeMatrix.GetFrame();
                if (tmpFrame != null)
                {
                    _curFrame = tmpFrame;
                    DrawFrame();
                }

                Thread.Sleep(10);
            }
        }

        private void BattleGUI_FormClosing(object sender, FormClosingEventArgs e)
        {

            Stop();
        }

        private void LoadPics()
        {
            //Image img = Image.FromFile("pic\\body.png");
            Bitmap bmp = (Bitmap)Bitmap.FromFile("pics\\body.png");
            SurfaceDescription description = new SurfaceDescription();
            description.SurfaceCaps.VideoMemory = true;
            description.Width = 60;
            description.Height = 60;
            description.SurfaceCaps.OffScreenPlain = true;
            ColorKey ck = new ColorKey();
            ck.ColorSpaceLowValue = 0;
            ck.ColorSpaceHighValue = 0;
            for (int i = 0; i <360; i++)
            {
                Bitmap bmpN = ImageUtility.Rotate2(bmp, i * -1, Color.Black);
                _tankBody[i] = new Surface(bmpN, description, display);
                _tankBody[i].SetColorKey(ColorKeyFlags.SourceDraw, ck);
                bmpN.Dispose();
            }

            //_tankBody.ColorFill(Color.Red);
            
            

            bmp.Dispose();
            
        }

       


    }
}
