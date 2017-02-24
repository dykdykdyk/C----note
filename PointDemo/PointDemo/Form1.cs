using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Timers;

namespace PointDemo
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer timerBlock;//更新器
        private ArrayList _blocks;//蛇块列表
        private Color _bgColor;//背景色
        Graphics gpp;
        private Direction _direction;//方向
        private Block _food;//当前食物

        private bool starttest = false;
        private int width = 15;
        private int height = 15;
        private int size = 20;

        //游戏速度
        private int gametimelevel =800;
        public Form1()
        {
            InitializeComponent();
            this._blocks = new ArrayList();


        }
        //private void button1_Click(object sender, EventArgs e)
        //{
        //    Graphics g = Graphics.FromHwnd(this.pictureBox1.Handle);
        //    gpp = g;
        //    _bgColor = this.pictureBox1.BackColor;
        //    //出现一个画笔  
        //    //Pen pen = new Pen(Brushes.Red);
        //    ////因为创建矩形需要point对象与size对象  
        //    //Point p = new Point(50, 50);
        //    //Size s = new Size(60, 60);
        //    //Rectangle r = new Rectangle(p, s);
        //    SolidBrush s = new SolidBrush(Color.Red);
        //    //长方形
        //    this._blocks.Insert(0, new Block(Color.Red, this._size, new Point(1, 1)));
        //    //this._blocks.Insert(0, new Block(Color.Red, this._size, new Point(2, 2)));
        //    this._direction = Direction.Right;
        //    start();
        //    //g.FillRectangle(s,20,60,20,20);
        //    //g.FillRectangle(s, 41, 60, 20, 20);
        //    //g.FillRectangle(s, 62, 60, 20, 20);
        //    //g.FillRectangle(s, 83, 60, 20, 20);
        //    //g.FillRectangles(s,new Rectangle[]{30});
        //    //g.DrawRectangle(pen, r);  
        //    //画一个填充颜色的圆
        //    //Graphics gp = this.CreateGraphics();
        //    //SolidBrush s = new SolidBrush(Color.Red);
        //    //gp.FillEllipse(s, 50, 50, 50, 50);
        //}
        public void start() {
            starttest = true;
            this._food = fond();
            timerBlock = new System.Timers.Timer(gametimelevel);
            timerBlock.Elapsed += new System.Timers.ElapsedEventHandler(OnBlockTimeEvent);
            timerBlock.AutoReset = true;
            timerBlock.Start();
        }

        //定时更新
        private void OnBlockTimeEvent(object sourse, ElapsedEventArgs e)
        {
            //this.move();//前进一个单位
            //if (this.CheckDead())//检查是否结束
            //{
            //    this.timerBlock.Stop();
            //    //释放所有资源
            //    this.timerBlock.Dispose();
            //    System.Windows.Forms.MessageBox.Show("Score:" + this._blocks.Count, "Game Over");
            //}
            Point p;//下一个坐标位置
            Block head = (Block)this._blocks[0];
            if (this._direction == Direction.Up)
                p = new Point(head.Point.X, head.Point.Y - 1);
            else if (this._direction == Direction.Down)
                p = new Point(head.Point.X, head.Point.Y + 1);
            else if (this._direction == Direction.Left)
                p = new Point(head.Point.X - 1, head.Point.Y);
            else
                p = new Point(head.Point.X + 1, head.Point.Y);


            Block b = new Block(Color.Red, this.size, p);
            //如果下一个坐标不是食物坐标，就删除最后一个蛇块
            if (this._food.Point != p)
            {
                this._blocks.RemoveAt(_blocks.Count - 1);
            }else
            {//如果下一个坐标和食物重合，就生成一个新食物
                this._food = this.fond();
              
            }

            
            //将蛇身体部分的颜色变成白色
            for (int i = 0; i < _blocks.Count; i++) 
            {
                //Block headd = (Block)_blocks[i];
                //Block aa = new Block(Color.Purple, this._size,new Point(headd.Point.X, headd.Point.Y));
                //_blocks.RemoveAt(i);
                //_blocks.Add(aa);

            }
            this._blocks.Insert(0, b);
                //把下一个坐标插入蛇块列表第一个，使其成为蛇头 
                this.PaintPalette(this.gpp);
            Console.WriteLine("  _blocks.Count " + _blocks.Count);

            if (this.CheckDead())//检查是否结束
            {
                this.timerBlock.Stop();
                this.timerBlock.Dispose();
                System.Windows.Forms.MessageBox.Show("Score:" + this._blocks.Count, "Game Over");
            }

            if (this.CheckDead())//检查是否结束
            {
                this.timerBlock.Stop();
                this.timerBlock.Dispose();
                System.Windows.Forms.MessageBox.Show("Score:" + this._blocks.Count, "Game Over");
            }

            
                
        }

        //检查是否游戏结束
        private bool CheckDead()
        {
            Block head = (Block)(this._blocks[0]);//取蛇头
            //检查是否超出画布范围
            if (head.Point.X < 0 || head.Point.Y < 0 || head.Point.X + 1 > this.width || head.Point.Y + 1 > this.height)
            {
                return true;
            }
            for (int i = 1; i < this._blocks.Count; i++)
            {
                Block b = (Block)this._blocks[i];
                if (head.Point.X == b.Point.X && head.Point.Y == b.Point.Y)
                {
                    return true;
                }
            }
            return false;
        }

        //更新画板
        public void PaintPalette(Graphics gp)
        {
            gp.Clear(this._bgColor);
            //gp.Clear(Color.Purple);
            this._food.Paint(gp);
            foreach (Block b in this._blocks)
                b.Paint(gp);
        } 
        //生成一个食物
        private Block fond() {
            Block food = null;
            Random r = new Random();
            bool redo = false;
            while (true)
            {
                redo = false;
                int x = r.Next(width);
                int y = r.Next(height);
                for(int i=0;i<this._blocks.Count;i++)
                {
                    Block b =(Block)(this._blocks[i]);
                    if (b.Point.X == x && b.Point.Y == y)
                    {
                        redo = true;
                    }

                }
                if (redo == false) 
                {
                    food = new Block(Color.Black, this.size, new Point(x, y));
                    break;
                }

            }
            return food;
        
        }
        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        public Direction mDirection
        {
            get
            {
                return this._direction;
            }
            set
            {
                this._direction = value;
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.W || e.KeyCode == Keys.Up) && mDirection != Direction.Down)
            {

                mDirection = Direction.Up;
            }
            if ((e.KeyCode == Keys.S || e.KeyCode == Keys.Down) && mDirection != Direction.Up)
            {
                mDirection = Direction.Down;
            }
            if ((e.KeyCode == Keys.A || e.KeyCode == Keys.Left) && mDirection != Direction.Right)
            {
               mDirection = Direction.Left;
            }
            if ((e.KeyCode == Keys.D || e.KeyCode == Keys.Right) && mDirection != Direction.Left)
            {
                mDirection = Direction.Right;
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void 新游戏ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            this.pictureBox1.Width = width * size;
            this.pictureBox1.Height = height * size;
            //pictureBox1.Size = new Size(Width, Height);

            //设置程序窗口的大小
            this.Width = pictureBox1.Width + 60;
            this.Height = pictureBox1.Height + 120;



            Graphics g = Graphics.FromHwnd(this.pictureBox1.Handle);
            gpp = g;
            _bgColor = this.pictureBox1.BackColor;
            //
            //出现一个画笔  
            //Pen pen = new Pen(Brushes.Red);
            ////因为创建矩形需要point对象与size对象  
            //Point p = new Point(50, 50);
            //Size s = new Size(60, 60);
            //Rectangle r = new Rectangle(p, s);
            SolidBrush s = new SolidBrush(Color.Red);
            //长方形

          

            this._blocks.Insert(0, new Block(Color.Red, this.size, new Point(1, 1)));
            //this._blocks.Insert(0, new Block(Color.Red, this._size, new Point(2, 2)));
            this._direction = Direction.Right;
            start();
            //g.FillRectangle(s,20,60,20,20);
            //g.FillRectangle(s, 41, 60, 20, 20);
            //g.FillRectangle(s, 62, 60, 20, 20);
            //g.FillRectangle(s, 83, 60, 20, 20);
            //g.FillRectangles(s,new Rectangle[]{30});
            //g.DrawRectangle(pen, r);  
            //画一个填充颜色的圆
            //Graphics gp = this.CreateGraphics();
            //SolidBrush s = new SolidBrush(Color.Red);
            //gp.FillEllipse(s, 50, 50, 50, 50);
        }

        private void 慢ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            width = 15;
            height = 15;
            size = 30;
            gametimelevel = 800;
            慢ToolStripMenuItem.Checked = true;
            一般ToolStripMenuItem.Checked = false;
            快ToolStripMenuItem.Checked = false;

        }

        private void 一般ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            width = 20;
            height = 20;
            size = 30;
            gametimelevel = 400;
            慢ToolStripMenuItem.Checked = false;
            一般ToolStripMenuItem.Checked = true;
            快ToolStripMenuItem.Checked = false;
        }

        private void 快ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            width = 20;
            height = 25;
            gametimelevel = 100;
            慢ToolStripMenuItem.Checked = false;
            一般ToolStripMenuItem.Checked = false;
            快ToolStripMenuItem.Checked = true;
        }

        private void 游戏ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("wsad或者方向键控制方向" + "\r\n" + "有3种难度选择", "不要撞墙哦", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void 暂停ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            //暂停ToolStripMenuItem.Text = "继续";
            //start = false;
            if (starttest)
            {
                暂停ToolStripMenuItem.Text = "继续";
                this.timerBlock.Stop();
                starttest = false;
            }
            else {
                暂停ToolStripMenuItem.Text = "暂停";
                starttest = true;
                this.timerBlock.Start();
            }
        }
    }
}
