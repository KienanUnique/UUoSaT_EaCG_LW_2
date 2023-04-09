using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UUoSaT_EaCG_LW_2
{
    public partial class FormMain : Form
    {
        private readonly List<PointF> _graphicPoinsList = new List<PointF>();
        private SizeF _graphSize;
        private float _screenRatioX;
        private float _screenRatioY;
        private PointF mousePoint;
        private bool _pointsIsNotReady = true;
        private int _movingOnGraphPointFrame = 0;

        private void AnT_Load(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, AnT.Width, AnT.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0, -1, 1);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void FormMain_Load(object sender, System.EventArgs e)
        {
            if ((float)AnT.Width <= (float)AnT.Height)
            {
                _graphSize.Width = 30f;
                _graphSize.Height = 30f * (float)AnT.Height / (float)AnT.Width;
                GL.Ortho(0f, _graphSize.Width, 0f, _graphSize.Height, -1, 1);
            }
            else
            {
                _graphSize.Height = 30f;
                _graphSize.Width = 30f * (float)AnT.Width / (float)AnT.Height;
                GL.Ortho(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0, -1, 1);
            }

            _screenRatioX = (float)_graphSize.Height / (float)AnT.Width;
            _screenRatioY = (float)_graphSize.Width / (float)AnT.Height;

            GL.MatrixMode(MatrixMode.Modelview);

            PointInGrap.Start();
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            mousePoint.X = _screenRatioX * e.X + 13;
            mousePoint.Y = (float)(_graphSize.Height - _screenRatioY * e.Y + 10);
        }

        private void functionCalculation()
        {
            float x = 0, y = 0;
            _graphicPoinsList.Clear();

            for (x = -2; x <= 3; x += 0.1f)
            {
                y = 2 * x * x - 3 * x - 8;
                _graphicPoinsList.Add(new PointF(x, y));
            }

            _pointsIsNotReady = false;
        }

        private void DrawDiagram()
        {
            if (_pointsIsNotReady)
            {
                functionCalculation();
            }

            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(_graphicPoinsList[0].X, _graphicPoinsList[0].Y);

            for (int ax = 1; ax < _graphicPoinsList.Count; ax += 2)
            {
                GL.Vertex2(_graphicPoinsList[ax].X, _graphicPoinsList[ax].Y);
            }

            GL.End();

            GL.PointSize(5);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(_graphicPoinsList[_movingOnGraphPointFrame].X, _graphicPoinsList[_movingOnGraphPointFrame].Y);

            GL.End();

            GL.PointSize(1);

        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void PointInGrap_Tick(object sender, System.EventArgs e)
        {
            if (_movingOnGraphPointFrame == _graphicPoinsList.Count - 1)
            {
                _movingOnGraphPointFrame = 0;
            }

            Draw();

            _movingOnGraphPointFrame++;
        }

        private void Draw()
        {
            GL.ClearColor(Color.Bisque);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();
            GL.Color3(0, 0, 0);
            GL.PushMatrix();
            GL.Translate(20, 15, 0);
            GL.Begin(PrimitiveType.Points);

            for (float ax = -2; ax <= 3; ax += 0.1f)
            {
                for (float bx = -15; bx < 15; bx++)
                {
                    GL.Vertex2(ax, bx);
                }
            }

            GL.End();

            GL.Begin(PrimitiveType.Lines);

            GL.Vertex2(0, -15);
            GL.Vertex2(0, 15);
            GL.Vertex2(-15, 0);
            GL.Vertex2(15, 0);

            GL.End();

            DrawDiagram();

            GL.PopMatrix();

            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(mousePoint.X, 15);
            GL.Vertex2(mousePoint.X, mousePoint.Y);
            GL.Vertex2(20, mousePoint.Y);
            GL.Vertex2(mousePoint.X, mousePoint.Y);
            GL.End();

            AnT.SwapBuffers();
        }
    }
}
