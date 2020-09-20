using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;

namespace SystAnalys_lr1
{

    public class Vertex
    {
        //public int x, y;
        public int x { get; set; }
        public int y { get; set; }
        public int Name { get; set; }
        public Vertex()
        { }

        public Vertex(int x, int y, int Name)
        {
            this.Name = Name;
            this.x = x;
            this.y = y;
        }
    }
    public class CarStation
    {
        //public int x, y;
        public int x { get; set; }
        public int y { get; set; }

        public CarStation()
        { }

        public CarStation(int x, int y)
        {

            this.x = x;
            this.y = y;
        }
    }
    public class Way
    {
        //public int x, y;
        public int x { get; set; }
        public int y { get; set; }

        public Way()
        { }

        public Way(int x, int y)
        {

            this.x = x;
            this.y = y;
        }
    }
    public class Cars

    {
        //public int x, y;
        public int x { get; set; }
        public int y { get; set; }
        public int Name { get; set; }
        public int Image { get; set; }
        public int Massege { get; set; }
        public Cars()
        { }

        public Cars(int x, int y, int Name, int Image, int Massege)
        {
            this.x = x;
            this.y = y;
            this.Name = Name;
            this.Image = Image;
            this.Massege = Massege;
        }
    }

    public class Edge
    {
        ///public int v1, v2;
        public int v1 { get; set; }
        public int v2 { get; set; }
        public Edge()
        { }

        public Edge(int v1, int v2)
        {
            this.v1 = v1;
            this.v2 = v2;
        }
    }

    class DrawGraph
    {
        Bitmap bitmap;
        Pen blackPen;
        Pen bluePen;
        Pen redPen;
        Pen darkGoldPen;
        Pen OrangePen;
        Pen OPen;
        Graphics gr;
        Font fo;
        Brush br;
        Brush b = new SolidBrush(Color.FromArgb(128, 178, 34, 34));
        Brush c = new SolidBrush(Color.FromArgb(1, 134, 24, 134));
        PointF point;
        Image car1;
        Image car2;
        Image car3;
        Image car4;
        public int R = 2; //радиус окружности вершины
        Point position;

        public DrawGraph(int width, int height)
        {
            bitmap = new Bitmap("12.png", true);
            gr = Graphics.FromImage(bitmap);
            //clearSheet();
            blackPen = new Pen(Color.Black);
            blackPen.Width = 1;
            bluePen = new Pen(Color.Blue);
            bluePen.Width = 2;
            OPen = new Pen(Color.Yellow);
            OPen.Width = 2;
            OrangePen = new Pen(Color.Green);
            OrangePen.Width = 2;
            redPen = new Pen(Color.Red);
            redPen.Width = 2;
            darkGoldPen = new Pen(Color.Black);
            darkGoldPen.Width = 2;
            fo = new Font("Arial", 5);
            br = Brushes.Black;
            car1 = Image.FromFile("1a.png");
            car2 = Image.FromFile("2a.png");
            car3 = Image.FromFile("1b.png");
            car4 = Image.FromFile("2b.png");
            position = new Point(0, 0);

        }

        public Bitmap GetBitmap()
        {
            return bitmap;
        }

        public void clearSheet(int LoadImage)
        {
            Graphics.FromImage(bitmap).Clear(Color.Wheat);
            if (LoadImage==1)
            bitmap = new Bitmap("12.png", true);
            if (LoadImage == 2)
                bitmap = new Bitmap("11.png", true);
            gr = Graphics.FromImage(bitmap);

        }

        public void drawVertex(int x, int y, string number, int Name, int r)
        {
            if (Name == 0)
            {
                gr.FillEllipse(Brushes.White, (x - R), (y - R), 2 * R, 2 * R);
                gr.DrawEllipse(blackPen, (x - R), (y - R), 2 * R, 2 * R);
                point = new PointF(x - 9, y - 9);
                gr.DrawString(number, fo, br, point);
            }
            if (Name == 1)
            {
                gr.FillEllipse(Brushes.Red, (x - r), (y - r), 4 * r, 4 * r);
                gr.FillEllipse(b, (x - 30*r), (y - 30*r), 60 * r, 60 * r);
                //gr.DrawEllipse(redPen, (x - R), (y - R), R, R);

                //point = new PointF(x - 9, y - 9);
                //gr.DrawString(number, fo, br, point);
            }
        }

        public void RefreshCarStation(List<Cars> C, List<CarStation> N, int[] CountCarStation)
        {
            for (int i = 0; i < N.Count; i++)
            {
                int NumberCar;
                NumberCar = CountCarStation[i];

                N[i].x = C[NumberCar].x;
                N[i].y =C[NumberCar].y;
            }
        }

        public void drawCar(Cars C, List<CarStation> N,  int[,] cars, List<Vertex> V, int r, int TypeStation)
        {


            int X1;
            int X2;
            int Y1;
            int Y2;
            int j = 0;
            int R1 = r;
            if (TypeStation != 2)
            {
                for (int i = 0; i < V.Count; i++)
                {

                    if (V[i].Name == 1)
                    {
                        X1 = V[i].x;
                        Y1 = V[i].y;
                        X2 = X1 + 60 * R;
                        Y2 = Y1 - 60 * R;
                        if (((Math.Pow((C.x - X1), 2) + Math.Pow((C.y - Y1), 2)) <= Math.Pow((30 * R), 2)))
                        {
                            switch (C.Image)
                            {
                                case 1:
                                    gr.DrawImage(car3, C.x, C.y);

                                    break;

                                case 2:
                                    gr.DrawImage(car4, C.x, C.y);
                                    break;
                            }
                            C.Massege = 1;
                            j = 2;
                        }
                        if (j == 0)
                        {
                            switch (C.Image)
                            {
                                case 1:
                                    gr.DrawImage(car1, C.x, C.y);
                                    break;
                                case 2:
                                    gr.DrawImage(car2, C.x, C.y);
                                    break;
                            }


                        }


                    }

                }
            }
            if (TypeStation != 1)
            {
                if (C.Name == 0)
                {
                    for (int i = 0; i < N.Count; i++)
                    {
                        X1 = N[i].x;
                        Y1 = N[i].y;
                        X2 = X1 + 60 * R1;
                        Y2 = Y1 - 60 * R1;
                        if (((Math.Pow((C.x - X1), 2) + Math.Pow((C.y - Y1), 2)) <= Math.Pow((30 * R1), 2)))
                        {
                            switch (C.Image)
                            {
                                case 1:
                                    gr.DrawImage(car3, C.x, C.y);

                                    break;

                                case 2:
                                    gr.DrawImage(car4, C.x, C.y);
                                    break;
                            }
                            j = 2;
                            C.Massege = 1;

                        }
                        if (j == 0)
                        {
                            switch (C.Image)
                            {
                                case 1:
                                    gr.DrawImage(car1, C.x, C.y);
                                    break;
                                case 2:
                                    gr.DrawImage(car2, C.x, C.y);
                                    break;
                            }


                        }


                    }



                }
                if (C.Name == 1)
                {
                    switch (C.Image)
                    {
                        case 1:
                            gr.DrawImage(car3, C.x, C.y);
                            break;
                        case 2:
                            gr.DrawImage(car4, C.x, C.y);
                            break;

                    }
                    C.Massege = 1;
                    //W.Add(new Way(C.x, C.y));

                    //gr.FillEllipse(Brushes.Red, (C.x - r), (C.y - r), 4 * r, 4 * r);
                    gr.FillEllipse(b, (C.x - 30 * r), (C.y - 30 * r), 60 * r, 60 * r);
                    //drawWay(W, r);
                }
            }

}

        public void drawWay(List<Way> W, int r)
        {
            for (int i = 0; i < W.Count; i++)

            {

                gr.FillEllipse(c, (W[i].x - 30 * r), (W[i].y - 30 * r), 60 * r, 60 * r);
            }
        }





        public void drawCarMove(List<Cars> C, List<Edge> E, List<Vertex> V, List<CarStation> N, List<Way> W, int[,] cars, int[,,] kord, int yMove, int r, int[] CountCarStation, int TypeStation, int LoadImage)
        {


            clearSheet(LoadImage);
           
            drawALLGraph(V, E, r);
            drawCarAll(C,E,V,N,W, cars, kord, yMove, r, CountCarStation,  TypeStation);
        }


        public void CarBegin(List<Cars> C, List<CarStation> N, List<Way> W, int i, int[,] cars, int[,,] kord, int j, List<Vertex> V, int r, int[] CountCarStation, int TypeStation)
        {
            int X1 = kord[i, j, 0];
            int Y1 = kord[i, j, 1];
            int X2 = C[i].x;
            int Y2 = C[i].y;


            if (((X1 > X2) && (Y1 > Y2)) || ((X1 > X2) && (Y1 == Y2)))

                C[i].Image = 1;

            else
            {
                if (((X1 > X2) && (Y1 > Y2)) || ((X1 == X2) && (Y1 < Y2)))
                    C[i].Image = 2;
                else
                {
                    if (((X1 < X2) && (Y1 < Y2)) || ((X1 < X2) && (Y1 == Y2)))
                        C[i].Image = 1;
                    else
                    {
                        if (((X1 < X2) && (Y1 > Y2)) || ((X1 == X2) && (Y1 > Y2)))
                            C[i].Image = 2;
                    }


                }


            }

            C[i].x = kord[i, j, 0];
            C[i].y = kord[i, j, 1];

            drawCar(C[i], N, cars, V, r,  TypeStation);
            RefreshCarStation(C, N, CountCarStation);


        }

    
        public void VoidC(List<Cars> C, List<CarStation> N, List<Way> W,  int i, int[,] cars, int[,,] kord, int j, List<Vertex> V, int r, int[] CountCarStation, int TypeStation)
        {
           
          
            

                int X1 = kord[i, j, 0];
                int Y1 = kord[i, j, 1];
                int X2 = C[i].x;
                int Y2 = C[i].y;


            if (((X1 > X2) && (Y1 > Y2)) || ((X1 > X2) && (Y1 == Y2)))

               C[i].Image = 1;

            else
            {
                if (((X1 > X2) && (Y1 > Y2)) || ((X1 == X2) && (Y1 < Y2)))
                    C[i].Image = 2;
                else
                {
                    if (((X1 < X2) && (Y1 < Y2)) || ((X1 < X2) && (Y1 == Y2)))
                        C[i].Image = 1;
                    else
                    {
                        if (((X1 < X2) && (Y1 > Y2)) || ((X1 == X2) && (Y1 > Y2)))
                            C[i].Image = 2;
                    }


                }


            }

                C[i].x = kord[i, j, 0];
                C[i].y = kord[i, j, 1];

                drawCar(C[i], N,  cars, V, r, TypeStation);
            if ((TypeStation == 2) || (TypeStation == 3))
                RefreshCarStation(C, N, CountCarStation);  


        }


        public void drawCarAll(List<Cars> C, List<Edge> E, List<Vertex> V, List<CarStation> N, List<Way> W, int[,] cars, int[,,] kord, int CountMasKord, int r, int[] CountCarStation, int TypeStation) {
            int j = CountMasKord;
            for (int i = 0; i < C.Count; i++)

            {
                
                VoidC(C, N,W, i, cars, kord, j,  V, r, CountCarStation,  TypeStation);
                //drawALLGraph(V, E);

            }

        }

  
        public void drawSelectedVertex(int x, int y)
        {
            gr.DrawEllipse(redPen, (x - R), (y - R), 2 * R, 2 * R);
        }

        public void drawEdge(Vertex V1, Vertex V2, Edge E, int numberE, int r)
        {
            if (E.v1 == E.v2)
            {
                gr.DrawArc(darkGoldPen, (V1.x - 2 * R), (V1.y - 2 * R), 2 * R, 2 * R, 90, 270);
                //point = new PointF(V1.x - (int)(2.75 * R), V1.y - (int)(2.75 * R));
                //gr.DrawString(((char)('a' + numberE)).ToString(), fo, br, point);
                drawVertex(V1.x, V1.y, (E.v1 + 1).ToString(),V1.Name, r);
            }
            else
            {
                gr.DrawLine(darkGoldPen, V1.x, V1.y, V2.x, V2.y);
                //point = new PointF((V1.x + V2.x) / 2, (V1.y + V2.y) / 2);
                //gr.DrawString(((char)('a' + numberE)).ToString(), fo, br, point);
                drawVertex(V1.x, V1.y, (E.v1 + 1).ToString(), V1.Name, r);
                drawVertex(V2.x, V2.y, (E.v2 + 1).ToString(), V1.Name, r);
            }
        }

        public void drawALLGraph(List<Vertex> V, List<Edge> E, int r)
        {
            //рисуем ребра
            for (int i = 0; i < E.Count; i++)
            {
                if (E[i].v1 == E[i].v2)
                {
                    gr.DrawArc(darkGoldPen, (V[E[i].v1].x - 2 * R), (V[E[i].v1].y - 2 * R), 2 * R, 2 * R, 90, 270);
                    //    point = new PointF(V[E[i].v1].x - (int)(2.75 * R), V[E[i].v1].y - (int)(2.75 * R));
                    //    gr.DrawString(((char)('a' + i)).ToString(), fo, br, point);
                }
                else
                {
                    gr.DrawLine(darkGoldPen, V[E[i].v1].x, V[E[i].v1].y, V[E[i].v2].x, V[E[i].v2].y);
                    //point = new PointF((V[E[i].v1].x + V[E[i].v2].x) / 2, (V[E[i].v1].y + V[E[i].v2].y) / 2);
                    //gr.DrawString(((char)('a' + i)).ToString(), fo, br, point);
                }
            }
            //рисуем вершины
            for (int i = 0; i < V.Count; i++)
            {
                drawVertex(V[i].x, V[i].y, (i + 1).ToString(), V[i].Name, r);
            }
        }

        //заполняет матрицу смежности
        public void fillAdjacencyMatrix(int numberV, List<Edge> E, List<Vertex> V,int[,] matrix)
        {
            for (int i = 0; i < numberV; i++)
                for (int j = 0; j < numberV; j++)
                    matrix[i, j] = 0;
            for (int i = 0; i < E.Count; i++)
            {
               int X1 = V[E[i].v1].x;
                int Y1 = V[E[i].v1].y;
                int X2 = V[E[i].v2].x;
               int Y2 = V[E[i].v2].y;
                int sizeVector = Convert.ToInt32(Math.Sqrt(Math.Pow(X2 - X1, 2) + Math.Pow(Y2 - Y1, 2)));
                sizeVector = sizeVector / 10;
                sizeVector = sizeVector * 10;
                matrix[E[i].v1, E[i].v2] = sizeVector;
                matrix[E[i].v2, E[i].v1] = sizeVector; 
            }
        }

        //заполняет матрицу инцидентности
     

        
    }
}