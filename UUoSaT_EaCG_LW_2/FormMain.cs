using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UUoSaT_EaCG_LW_2
{
    public partial class FormMain : Form
    {
        // размеры окна
        double ScreenW, ScreenH;
        // отношения сторон окна визуализации
        // для корректного перевода координат мыши в координаты,
        // принятые в программе
        private float devX;
        private float devY;
        // массив, который будет хранить значения x,y точек графика
        private float[,] GrapValuesArray;
        // количество элементов в массиве
        private int elements_count = 0;
        // флаг, означающий, что массив с значениями координат графика пока еще не заполнен
        private bool not_calculate = true;
        // номер ячейки массива, из которой будут взяты координаты для красной точки,
        // для визуализации текущего кадра
        private int pointPosition = 0;
        // вспомогательные переменные для построения линий от курсора мыши к координатным осям
        private float lineX, lineY;
        // текущие координаты курсора мыши
        private float Mcoord_X = 0, Mcoord_Y = 0;

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
            if((float)AnT.Width <= (float)AnT.Height)
            {
                ScreenW = 30f;
                ScreenH = 30f * (float)AnT.Height / (float)AnT.Width;
                GL.Ortho(0f, ScreenW, 0f, ScreenH, -1, 1);
            }
            else
            {
                ScreenH = 30f;
                ScreenW = 30f * (float)AnT.Width / (float)AnT.Height;
                GL.Ortho(0.0, 30.0 * (float)AnT.Width / (float)AnT.Height, 0.0, 30.0, -1, 1);
            }

            devX = (float)ScreenH / (float)AnT.Width;
            devY = (float)ScreenW / (float)AnT.Height;

            GL.MatrixMode(MatrixMode.Modelview);

            PointInGrap.Start();
        }

        private void AnT_MouseMove(object sender, MouseEventArgs e)
        {
            Mcoord_X = e.X;
            Mcoord_Y = e.Y;

            lineX = devX * e.X;
            lineY = (float)(ScreenH - devY * e.Y);
        }

        private void functionCalculation()
        {
            float x = 0, y = 0;
            GrapValuesArray = new float[300, 2];

            elements_count = 0;

            for(x = -2; x <= 3; x += 0.1f)
            {
                y = 2 * x * x - 3 * x - 8;
                GrapValuesArray[elements_count, 0] = x;
                GrapValuesArray[elements_count, 1] = y;

                elements_count++;
            }

            not_calculate = false;
        }

        private void DrawDiagram()
        {
            if (not_calculate)
            {
                functionCalculation();
            }

            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex2(GrapValuesArray[0, 0], GrapValuesArray[0, 1]);

            for(int ax = 1; ax < elements_count; ax += 2)
            {
                GL.Vertex2(GrapValuesArray[ax, 0], GrapValuesArray[ax, 1]);
            }

            GL.End();

            GL.PointSize(5);
            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(GrapValuesArray[pointPosition, 0], GrapValuesArray[pointPosition, 1]);

            GL.End();

            GL.PointSize(1);

        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void PointInGrap_Tick(object sender, System.EventArgs e)
        {
            if(pointPosition == elements_count - 1)
            {
                pointPosition = 0;
            }

            Draw();

            pointPosition++;
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
                for (float bx = -15; bx < 15; bx ++)
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

            GL.Vertex2(0, 15);
            GL.Vertex2(0.1, 14.5);
            GL.Vertex2(0, 15);
            GL.Vertex2(-0.1, -14.5);

            GL.Vertex2(15, 0);
            GL.Vertex2(14.5, 0.1);
            GL.Vertex2(15, 0);
            GL.Vertex2(-14.5 , - 0.1);

            GL.End();

            DrawDiagram();
            GL.PopMatrix();

            GL.Color3(Color.Red);
            GL.Begin(PrimitiveType.Lines);
            GL.Vertex2(lineX, 15);
            GL.Vertex2(lineX, lineY);
            GL.Vertex2(20, lineY);
            GL.Vertex2(lineX, lineY);
            GL.End();

            AnT.SwapBuffers();
        }
    }
}
