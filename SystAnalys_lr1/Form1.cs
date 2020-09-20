using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using MetroFramework.Forms;
using MetroFramework;

namespace SystAnalys_lr1
{
    public partial class Form1 : MetroForm
    {
        DrawGraph G;
        GA genom;
        List<Vertex> V;
        List<Edge> E;
        List<Cars> C;
        List<CarStation> N;
        List<Way> W;

        int[] PointStation;
        int[,] AMatrix; //матрица смежности
        int[,] CarsPoint;// матрица для хранения пути кажлой машины
        int[,,] kord;// матрица для хранения пути кажлой машины
        int selected1; //выбранные вершины, для соединения линиями
        int selected2;
        double percent;
        int r;
        int TypeStation;
        int AllCar;
        int CountStation;
        int CountMasKord = 0;
        int sizeCountMasKord = 0;
        int[] CountCarStation;
        int LoadImage;
        bool load = false;
        bool ok = false;
        Log log;
        DateTime now;
        string namekard;
        string nameVertex;
        string nameEdge;
        string nameWay;
        int iteration1;
        int popSize1;
        double mutateRate1;
        int mid2 = 0;

        public Form1()
        {
            InitializeComponent();
            V = new List<Vertex>();
            G = new DrawGraph(sheet.Width, sheet.Height);
            E = new List<Edge>();
            C = new List<Cars>();
            N = new List<CarStation>();
            W = new List<Way>();


            //sheet.Image = G.GetBitmap();
        }

        //кнопка - выбрать вершину
        private void selectButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = false;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            drawVertexStationButton.Enabled = true;
            G.clearSheet(LoadImage);
            G.drawALLGraph(V, E, r);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
        }

        //кнопка - рисовать вершину
        private void drawVertexButton_Click(object sender, EventArgs e)
        {
            drawVertexButton.Enabled = false;
            selectButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            drawVertexStationButton.Enabled = true;
            G.clearSheet(LoadImage);
            G.drawALLGraph(V, E, r);
            sheet.Image = G.GetBitmap();
        }

        //кнопка - рисовать ребро
        private void drawEdgeButton_Click(object sender, EventArgs e)
        {
            drawEdgeButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            deleteButton.Enabled = true;
            drawVertexStationButton.Enabled = true;
            G.clearSheet(LoadImage);
            G.drawALLGraph(V, E, r);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }

        //кнопка - рисовать станцию
        private void drawVertecStationButton_Click(object sender, EventArgs e)


        {
            drawVertexButton.Enabled = false;
            drawVertexStationButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            deleteButton.Enabled = true;
            G.clearSheet(LoadImage);
            G.drawALLGraph(V, E, r);
            sheet.Image = G.GetBitmap();
            selected1 = -1;
            selected2 = -1;
        }

        //кнопка - удалить элемент
        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            drawVertexStationButton.Enabled = true;
            G.clearSheet(LoadImage);
            G.drawALLGraph(V, E, r);
            sheet.Image = G.GetBitmap();
        }

        //кнопка - удалить граф
        private void deleteALLButton_Click(object sender, EventArgs e)
        {
            selectButton.Enabled = true;
            drawVertexButton.Enabled = true;
            drawEdgeButton.Enabled = true;
            deleteButton.Enabled = true;
            drawVertexStationButton.Enabled = true;
            const string message = "Вы действительно хотите полностью удалить граф?";
            const string caption = "Удаление";
            var MBSave = MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (MBSave == DialogResult.Yes)
            {
                V.Clear();
                E.Clear();
                G.clearSheet(LoadImage);
                sheet.Image = G.GetBitmap();
            }
        }

        //кнопка - матрица смежности
        private void buttonAdj_Click(object sender, EventArgs e)
        {
            createAdjAndOut();
        }



        private void sheet_MouseClick(object sender, MouseEventArgs e)
        {
            //нажата кнопка "выбрать вершину", ищем степень вершины
            if (selectButton.Enabled == false)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        if (selected1 != -1)
                        {
                            selected1 = -1;
                            G.clearSheet(LoadImage);
                            G.drawALLGraph(V, E, r);
                            sheet.Image = G.GetBitmap();
                        }
                        if (selected1 == -1)
                        {
                            G.drawSelectedVertex(V[i].x, V[i].y);
                            selected1 = i;
                            sheet.Image = G.GetBitmap();
                            createAdjAndOut();

                            double degree = 0;
                            for (int j = 0; j < V.Count; j++)
                                degree += AMatrix[selected1, j];

                            break;
                        }
                    }
                }
            }
            //нажата кнопка "рисовать вершину"
            if (drawVertexButton.Enabled == false)
            {
                V.Add(new Vertex(e.X, e.Y, 0));
                G.drawVertex(e.X, e.Y, V.Count.ToString(), 0, r);
                sheet.Image = G.GetBitmap();
            }
            //нажата кнопка "рисовать станцию"
            if (drawVertexStationButton.Enabled == false)
            {
                V.Add(new Vertex(e.X, e.Y, 1));
                G.drawVertex(e.X, e.Y, V.Count.ToString(), 1, r);
                sheet.Image = G.GetBitmap();
            }

            //нажата кнопка "рисовать ребро"
            if (drawEdgeButton.Enabled == false)
            {
                if (e.Button == MouseButtons.Left)
                {
                    for (int i = 0; i < V.Count; i++)
                    {
                        if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                        {
                            if (selected1 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected1 = i;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                            if (selected2 == -1)
                            {
                                G.drawSelectedVertex(V[i].x, V[i].y);
                                selected2 = i;
                                E.Add(new Edge(selected1, selected2));
                                G.drawEdge(V[selected1], V[selected2], E[E.Count - 1], E.Count - 1, r);
                                selected1 = -1;
                                selected2 = -1;
                                sheet.Image = G.GetBitmap();
                                break;
                            }
                        }
                    }
                }
                if (e.Button == MouseButtons.Right)
                {
                    if ((selected1 != -1) &&
                        (Math.Pow((V[selected1].x - e.X), 2) + Math.Pow((V[selected1].y - e.Y), 2) <= G.R * G.R))
                    {
                        G.drawVertex(V[selected1].x, V[selected1].y, (selected1 + 1).ToString(), V[selected1].Name, r);
                        selected1 = -1;
                        sheet.Image = G.GetBitmap();
                    }
                }
            }
            //нажата кнопка "удалить элемент"
            if (deleteButton.Enabled == false)
            {
                bool flag = false; //удалили ли что-нибудь по ЭТОМУ клику
                //ищем, возможно была нажата вершина
                for (int i = 0; i < V.Count; i++)
                {
                    if (Math.Pow((V[i].x - e.X), 2) + Math.Pow((V[i].y - e.Y), 2) <= G.R * G.R)
                    {
                        for (int j = 0; j < E.Count; j++)
                        {
                            if ((E[j].v1 == i) || (E[j].v2 == i))
                            {
                                E.RemoveAt(j);
                                j--;
                            }
                            else
                            {
                                if (E[j].v1 > i) E[j].v1--;
                                if (E[j].v2 > i) E[j].v2--;
                            }
                        }
                        V.RemoveAt(i);
                        flag = true;
                        break;
                    }
                }
                //ищем, возможно было нажато ребро
                if (!flag)
                {
                    for (int i = 0; i < E.Count; i++)
                    {
                        if (E[i].v1 == E[i].v2) //если это петля
                        {
                            if ((Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) <= ((G.R + 2) * (G.R + 2))) &&
                                (Math.Pow((V[E[i].v1].x - G.R - e.X), 2) + Math.Pow((V[E[i].v1].y - G.R - e.Y), 2) >= ((G.R - 2) * (G.R - 2))))
                            {
                                E.RemoveAt(i);
                                flag = true;
                                break;
                            }
                        }
                        else //не петля
                        {
                            if (((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) <= (e.Y + 4) &&
                                ((e.X - V[E[i].v1].x) * (V[E[i].v2].y - V[E[i].v1].y) / (V[E[i].v2].x - V[E[i].v1].x) + V[E[i].v1].y) >= (e.Y - 4))
                            {
                                if ((V[E[i].v1].x <= V[E[i].v2].x && V[E[i].v1].x <= e.X && e.X <= V[E[i].v2].x) ||
                                    (V[E[i].v1].x >= V[E[i].v2].x && V[E[i].v1].x >= e.X && e.X >= V[E[i].v2].x))
                                {
                                    E.RemoveAt(i);
                                    flag = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                //если что-то было удалено, то обновляем граф на экране
                if (flag)
                {
                    G.clearSheet(LoadImage);
                    G.drawALLGraph(V, E, r);
                    sheet.Image = G.GetBitmap();
                }
            }
        }

        //создание матрицы смежности 
        private void createAdjAndOut()
        {
            AMatrix = new int[V.Count, V.Count];
            G.fillAdjacencyMatrix(V.Count, E, V, AMatrix);

        }








        //кнопка - матрица смежности
        private void button1_Click(object sender, EventArgs e)
        {
            createAdjAndOut();
        }


        //кнопка сохранить
        private void Save_Click(object sender, EventArgs e)
        {
            //сериализируем вершины 

            if (LoadImage == 1)
            {
                File.Delete("vertex.xml");
                File.Delete("Edge.xml");
                XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
                FileStream file = new FileStream("vertex.xml", FileMode.OpenOrCreate);
                Ver.Serialize(file, V);
                Console.WriteLine("Объект сериализован");
                file.Close();
                //сериализируем ребра
                XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
                FileStream file_2 = new FileStream("Edge.xml", FileMode.OpenOrCreate);
                Edge.Serialize(file_2, E);
                Console.WriteLine("Объект сериализован");
                file_2.Close();
            }
            if (LoadImage == 2)
            {
                File.Delete("vertex1.xml");
                File.Delete("Edge1.xml");
                XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
                FileStream file = new FileStream("vertex1.xml", FileMode.OpenOrCreate);
                Ver.Serialize(file, V);
                Console.WriteLine("Объект сериализован");
                file.Close();
                //сериализируем ребра
                XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
                FileStream file_2 = new FileStream("Edge1.xml", FileMode.OpenOrCreate);
                Edge.Serialize(file_2, E);
                Console.WriteLine("Объект сериализован");
                file_2.Close();
            }
            if (LoadImage == 3)
            {
                File.Delete("vertex2.xml");
                File.Delete("Edge2.xml");
                XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
                FileStream file = new FileStream("vertex2.xml", FileMode.OpenOrCreate);
                Ver.Serialize(file, V);
                Console.WriteLine("Объект сериализован");
                file.Close();
                //сериализируем ребра
                XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
                FileStream file_2 = new FileStream("Edge2.xml", FileMode.OpenOrCreate);
                Edge.Serialize(file_2, E);
                Console.WriteLine("Объект сериализован");
                file_2.Close();
            }
        }
        //кнопка загрузить
        private void Load_Click(object sender, EventArgs e)
        {   //десериализируем вершины

            if (LoadImage == 1)
            {
                FileStream file = new FileStream("vertex.xml", FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
                V = (List<Vertex>)Ver.Deserialize(file);
                file.Close();
                //десериализируем ребра
                FileStream file_2 = new FileStream("Edge.xml", FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
                E = (List<Edge>)Edge.Deserialize(file_2);
                file_2.Close();
            }
            if (LoadImage == 2)
            {
                FileStream file = new FileStream("vertex1.xml", FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
                V = (List<Vertex>)Ver.Deserialize(file);
                file.Close();
                //десериализируем ребра
                FileStream file_2 = new FileStream("Edge1.xml", FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
                E = (List<Edge>)Edge.Deserialize(file_2);
                file_2.Close();
            }
            if (LoadImage == 3)
            {
                FileStream file = new FileStream("vertex2.xml", FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
                V = (List<Vertex>)Ver.Deserialize(file);
                file.Close();
                //десериализируем ребра
                FileStream file_2 = new FileStream("Edge2.xml", FileMode.Open, FileAccess.Read, FileShare.None);
                XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
                E = (List<Edge>)Edge.Deserialize(file_2);
                file_2.Close();
            }
            int a = V.Count;
            if (radioButton6.Checked == true)
                GeneticRun();
            StreamReader print = new StreamReader("input.txt");

            string line = print.ReadLine();

            r = Convert.ToInt32(line);
            line = print.ReadLine();
            if (AllCar > 330)
                AllCar = 330;
            int CarCount = AllCar;
            line = print.ReadLine();
            int tik = Convert.ToInt32(line);
            //MassivTik(tik);
            sizeCountMasKord = tik;
            line = print.ReadLine();
            
            int lenght = 20;
            CarsPoint = new int[CarCount, lenght];

            if (LoadImage == 1)

            {
                StreamReader print2 = new StreamReader("masnew.txt");
                for (int i = 0; i < CarCount; i++)
                {
                    string line1 = print2.ReadLine();
                    string[] fields = line1.Split('|');
                    for (int j = 0; j < lenght; j++)
                    {
                        CarsPoint[i, j] = Convert.ToInt32(fields[j]);
                    }
                }
                print2.Close();
            }
            if (LoadImage == 2)
            {
                StreamReader print2 = new StreamReader("mas7.txt");
                for (int i = 0; i < CarCount; i++)
                {
                    string line1 = print2.ReadLine();
                    string[] fields = line1.Split('|');
                    for (int j = 0; j < lenght; j++)
                    {
                        CarsPoint[i, j] = Convert.ToInt32(fields[j]);
                    }
                }
                print2.Close();
            }
            if (LoadImage == 3)
            {
                StreamReader print2 = new StreamReader("mas13.txt");
                for (int i = 0; i < CarCount; i++)
                {
                    string line1 = print2.ReadLine();
                    string[] fields = line1.Split('|');
                    for (int j = 0; j < lenght; j++)
                    {
                        CarsPoint[i, j] = Convert.ToInt32(fields[j]);
                    }
                }
                print2.Close();
            }


            for (int i = 0; i < CarCount; i++)
            {

                C.Add(new Cars(V[CarsPoint[i, 0]].x, V[CarsPoint[i, 0]].y, 0, 0, 0));
            }
            if (radioButton6.Checked == false)
            {
                for (int i = 0; i < V.Count; i++)
                {

                    V[i].Name = 0;

                }
            }
               
            if (radioButton8.Checked == true)
            {
                StationAdd();
            }
           
            if (radioButton7.Checked == true)
            {
                for (int i = 0; i < V.Count; i++)
                {
                    if (line[i] == '0')
                        V[i].Name = 0;
                    if (line[i] == '1')
                        V[i].Name = 1;


                }
            }

            print.Close();
            MassivTik(tik);

            //CARMOVE();
            G.drawALLGraph(V, E, r);
            sheet.Image = G.GetBitmap();
            //G.clearSheet(LoadImage);
            load = true;

        }

        private void StationAdd()
        {
            if (TypeStation == 1)
            {
                Random rnd = new Random();

                int M;
                for (int i = 0; i < CountStation; i++)
                {
                    M = rnd.Next(V.Count);
                    V[M].Name = 1;

                }


            }
            if (TypeStation == 2)
            {
                Random rnd = new Random();
                CountCarStation = new int[CountStation];
                int M;
                for (int i = 0; i < CountStation; i++)
                {
                    M = rnd.Next(C.Count);
                    C[M].Name = 1;
                    N.Add(new CarStation(C[M].x, C[M].y));
                    CountCarStation[i] = M;

                }


            }
            if (TypeStation == 3)
            {
                Random rnd = new Random();
                CountCarStation = new int[CountStation];
                int M;
                int type;
                int size = 0;
                for (int i = 0; i < CountStation; i++)
                {
                    type = rnd.Next(2);
                    if (type == 1)
                    {
                        M = rnd.Next(C.Count);
                        C[M].Name = 1;
                        N.Add(new CarStation(C[M].x, C[M].y));
                        CountCarStation[size] = M;
                        size++;
                    }
                    if (type != 1)
                    {
                        M = rnd.Next(V.Count);
                        V[M].Name = 1;

                    }
                }



            }
        }
        private void Percent()
        {



            int k = 0;
            for (int i = 0; i < C.Count; i++)
            {

                if (C[i].Massege == 1)
                {
                    k++;

                }

            }


            percent = (AllCar / 100) * k;
            double percent2 = Convert.ToDouble(k / (AllCar / 100M));
            label6.Text = "";
            label6.Text += CountStation;
            label7.Text = "";
            label7.Text += percent2 + "%";
            label8.Text = "";
            label8.Text += AllCar;

        }
        private void CARMOVE()
        {
            createAdjAndOut();
            int CountCar = 20;
            CarsPoint = new int[CountCar, 30];
            int array = V.Count;
            Random rnd = new Random();
            for (int i = 0; i < CountCar; i++)
            {
                CarsPoint[i, 0] = rnd.Next(1, array - 1);
                C.Add(new Cars(V[CarsPoint[i, 0]].x, V[CarsPoint[i, 0]].y, i, 0, 0));
            }
            int m = 0;

            for (int k = 0; k < CountCar; k++)
            {
                for (int i = 1; i < 30; i++)
                {

                    while (m != 1)
                    {
                        int u = rnd.Next(0, array - 1);

                        int n = AMatrix[CarsPoint[k, i - 1], u];
                        if ((n > 0) && (u != 0))
                        {
                            if (CarsPoint[k, i - 1] == u)
                            {
                                m = 1;
                            }

                        }
                        if ((m == 0) && (AMatrix[CarsPoint[k, i - 1], u] > 0))
                        {
                            CarsPoint[k, i] = u;

                            m = 1;
                        }

                        else
                        {
                            m = 0;
                        }

                    }
                    m = 0;

                }
            }
        }

        private void MassivTik(int counttik)
        {
            kord = new int[C.Count, counttik, 2];
            int sizetik = 10;

            for (int j = 0; j < C.Count; j++)
            {
                kord[j, 0, 0] = V[CarsPoint[j, 0]].x;
                kord[j, 0, 1] = V[CarsPoint[j, 0]].y;
                int tik = 0;
                while (tik != counttik)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        int x1 = V[CarsPoint[j, i]].x;
                        int y1 = V[CarsPoint[j, i]].y;
                        int x2 = V[CarsPoint[j, i + 1]].x;
                        int y2 = V[CarsPoint[j, i + 1]].y;
                        int sizeVector = Convert.ToInt32(Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)));
                        sizeVector = sizeVector / sizetik;
                        for (int k = 1; k <= sizeVector; k++)
                        {
                            if (tik >= counttik)
                                continue;
                            int x = Convert.ToInt32(x1 + (10 * k) * (x2 - x1) / Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                            int y = Convert.ToInt32(y1 + (10 * k) * (y2 - y1) / Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
                            if (tik == counttik - 1)
                            {
                                kord[j, tik, 0] = x2;
                                kord[j, tik, 1] = y2;
                            }
                            else
                            {
                                kord[j, tik, 0] = x;
                                kord[j, tik, 1] = y;
                            }
                            tik++;

                        }
                        if (tik == counttik - 1)
                        {
                            kord[j, tik, 0] = x2;
                            kord[j, tik, 1] = y2;
                        }
                        else
                        {
                            if (tik >= counttik)
                                continue;
                            kord[j, tik + 1, 0] = x2;
                            kord[j, tik + 1, 1] = y2;

                        }
                        if (tik >= counttik)
                            continue;

                    }
                }

            }

        }
        //private void Load_Click(object sender, EventArgs e)
        //{   //десериализируем вершины
        //    FileStream file = new FileStream("vertex.xml", FileMode.Open, FileAccess.Read, FileShare.None);
        //    XmlSerializer Ver = new XmlSerializer(typeof(List<Vertex>));
        //    V = (List<Vertex>)Ver.Deserialize(file);
        //    file.Close();
        //    //десериализируем ребра
        //    FileStream file_2 = new FileStream("Edge.xml", FileMode.Open, FileAccess.Read, FileShare.None);
        //    XmlSerializer Edge = new XmlSerializer(typeof(List<Edge>));
        //    E = (List<Edge>)Edge.Deserialize(file_2);
        //    file_2.Close();

        //    CARMOVE();
        //    MassivTik();
        //    Percent();
        //    //G.drawALLGraph(V, E);
        //    //sheet.Image = G.GetBitmap();

        //}
        //private void Percent()
        //{
        //    int ipercent = 0;
        //    int StationCount = 0;
        //    int[,] NameStation;

        //    for (int i = 0; i < V.Count; i++)
        //    {

        //          if (V[i].Name == 1)
        //            {
        //            StationCount++;

        //            }

        //    }

        //    NameStation = new int [StationCount,2];
        //    int k = 0;
        //    for (int i = 0; i < V.Count; i++)
        //    {

        //        if (V[i].Name == 1)
        //        {
        //            NameStation[k,0] = i;
        //            k++;

        //        }

        //    }
        //    for (int i=0; i < C.Count; i++)
        //    {
        //        for (int j = 0; j < 7; j++)
        //        {
        //            if (V[CarsPoint[i, j]].Name == 1)
        //            {
        //                for (int d = 0; d < StationCount; d++)
        //                {
        //                    if ((NameStation[d,0]==CarsPoint[i, j])&& (NameStation[d, 1]!=1))
        //                    {
        //                        NameStation[d,1] = 1;
        //                        ipercent++;

        //                    }
        //                }


        //            }
        //        }
        //    }
        //    percent = (StationCount / 100) * ipercent;
        //    double percent2 = Convert.ToDouble(ipercent / (StationCount / 100M));
        //    label1.Text += StationCount;
        //    label2.Text += percent2 + "%";
        //    string sOut = "    ";
        //    for (int i = 0; i < 7; i++)
        //        sOut += (i + 1) + " ";

        //    for (int i = 0; i < 60; i++)
        //    {
        //        sOut = (i + 1) + " | ";
        //        for (int j = 0; j < 7; j++)
        //            sOut += CarsPoint[i, j] + " ";
        //        listBox1.Items.Add(sOut);
        //    }
        //}
        //private void CARMOVE()
        //{
        //    createAdjAndOut();
        //    int CountCar = 60;
        //    CarsPoint = new int[CountCar, 10];
        //    int array = V.Count;
        //    Random rnd = new Random();
        //    for (int i = 0; i < CountCar; i++)
        //    {
        //        CarsPoint[i, 0] = rnd.Next(1, array - 1);
        //        C.Add(new Cars(V[CarsPoint[i, 0]].x, V[CarsPoint[i, 0]].y, i));
        //    }
        //    int m = 0;

        //    for (int k = 0; k < CountCar; k++)
        //    {
        //        for (int i = 1; i <10; i++)
        //        {

        //            while (m != 1)
        //            {
        //                int u = rnd.Next(0, array - 1);

        //                int n = AMatrix[CarsPoint[k, i - 1], u];
        //                if ((n > 0) && (u != 0))
        //                {   
        //                        if (CarsPoint[k, i-1] == u)
        //                        {
        //                            m = 1;
        //                        }

        //                    }
        //                    if ((m == 0) && (AMatrix[CarsPoint[k, i-1], u] > 0))
        //                    {
        //                        CarsPoint[k, i] = u;

        //                        m = 1;
        //                    }

        //                else {
        //                    m = 0;
        //                }

        //            }
        //            m = 0;

        //        }
        //    }
        //}

        //private void MassivTik()
        //{
        //    kord = new int[C.Count, 30, 2];
        //    int sizetik = 10;

        //    for (int j = 0; j < C.Count; j++)
        //    {
        //        kord[j, 0, 0] = V[CarsPoint[j, 0]].x;
        //        kord[j, 0, 1] = V[CarsPoint[j, 0]].y;
        //        int tik = 0;
        //        while (tik != 30)
        //        {
        //            for (int i = 0; i < 4; i++)
        //            {
        //                int x1 = V[CarsPoint[j, i]].x;
        //                int y1 = V[CarsPoint[j, i]].y;
        //                int x2 = V[CarsPoint[j, i + 1]].x;
        //                int y2 = V[CarsPoint[j, i + 1]].y;
        //                int sizeVector = Convert.ToInt32(Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2)));
        //                sizeVector = sizeVector / sizetik;
        //                for (int k = 1; k <= sizeVector; k++)
        //                {
        //                    if (tik >= 30)
        //                        continue;
        //                    int x = Convert.ToInt32(x1 + (10 * k) * (x2 - x1) / Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
        //                    int y = Convert.ToInt32(y1 + (10 * k) * (y2 - y1) / Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2)));
        //                    if (tik == 29)
        //                    {
        //                        kord[j, tik, 0] = x2;
        //                        kord[j, tik, 1] = y2;
        //                    }
        //                    else
        //                    {
        //                        kord[j, tik, 0] = x;
        //                        kord[j, tik, 1] = y;
        //                    }
        //                    tik++;

        //                }
        //                if (tik == 29)
        //                {
        //                    kord[j, tik, 0] = x2;
        //                    kord[j, tik, 1] = y2;
        //                }
        //                else
        //                {
        //                    if (tik >= 30)
        //                        continue;
        //                    kord[j, tik + 1, 0] = x2;
        //                    kord[j, tik + 1, 1] = y2;

        //                }
        //                if (tik >= 30)
        //                    continue;

        //            }
        //        }

        //    }

        //}

        private void Start_Click(object sender, EventArgs e)
        {

            if (load == true)
            {
                timer2.Enabled = true;
                timer2.Start();
            }
            if (load == false)
            {
                MessageBox.Show("Загрузите рабочую область", "Не выполнена загрузка");
            }


        }




        private void Stop_Click(object sender, EventArgs e)
        {
            timer2.Stop();
            timer2.Enabled = false;
            Percent();


        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            if (CountMasKord < sizeCountMasKord - 2)
            {
                G.drawCarMove(C, E, V, N, W, CarsPoint, kord, CountMasKord, r, CountCarStation, TypeStation, LoadImage);
                sheet.Image = G.GetBitmap();
                CountMasKord++;
            }
            else
            {
                G.drawCarMove(C, E, V, N, W, CarsPoint, kord, CountMasKord, r, CountCarStation, TypeStation, LoadImage);
                sheet.Image = G.GetBitmap();
                CountMasKord = 0;


            }
            timer1.Enabled = true;
            timer1.Start();

        }

        
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {


            if (radioButton1.Checked == true)
                TypeStation = 1;
            if (radioButton2.Checked == true)
                TypeStation = 2;
            if (radioButton3.Checked == true)
                TypeStation = 3;
            if (radioButton4.Checked == true)
                LoadImage = 1;
            if (radioButton5.Checked == true)
                
            { LoadImage = 2;
                OpenFileDialog opfd = new OpenFileDialog();
                if (opfd.ShowDialog(this) == DialogResult.OK)
                {
                    //Открываем в пикчур бокс
                    namekard = opfd.FileName;
                }

            }
            //if (radioButton9.Checked == true)
            //{       LoadImage = 3;
            //    OpenFileDialog opfd = new OpenFileDialog();
            //    if (opfd.ShowDialog(this) == DialogResult.OK)
            //    {
            //        //Открываем в пикчур бокс
            //        namekard = opfd.FileName;
            //    }

            //}


            if ((textBox1.Text != "" ) && (textBox2.Text != "") && ((LoadImage == 1) || (LoadImage == 2)/* || (LoadImage == 3)*/))
            {
                AllCar = int.Parse(textBox1.Text);
                CountStation = int.Parse(textBox2.Text);
                if (radioButton6.Checked == true)
                {
                    Form ifrm = new Form3();
                    ifrm.Show();
                }


                G.clearSheet(LoadImage);
            }


        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private void searchPOint()
        {
            int j = 0;
            PointStation = new int[CountStation];
            for (int i = 0; i < V.Count; i++)
                if (V[i].Name == 1)
                {
                    PointStation[j] = i;
                    j++;
                }

        }
        public double GeneticRun1(int key)

        {
            int car = AllCar;
            double limitCostRate = CountStation; //ограничение на стоимость
            //StreamReader print = new StreamReader("gen.txt");
            //string line = print.ReadLine();
            int iteration = /*Convert.ToInt32(*/iteration1;  //количество итерации
            //line = print.ReadLine();
            int popSize = popSize1;  //размер популяции
            //line = print.ReadLine();

            double mutateRate = mutateRate1;  //коэффицент мутации
            double maxCost = limitCostRate;
            //line = print.ReadLine();
            int timepr = 10;
            //print.Close();
            var time1 = DateTime.Now;
            log = new Log("random_150V" + limitCostRate);
            Log.WriteLog("Cost limit : " + limitCostRate);
            Log.WriteLog("Iterations : " + iteration);
            Log.WriteLog("Population size : " + popSize);
            Log.WriteLog("MutateRate : " + mutateRate);
            Log.WriteLog("Car: " + car);
            Log.WriteLog("Time: " + timepr);
            GA1 genome = new GA1(V, E, PointStation, popSize, iteration, maxCost, limitCostRate, mutateRate, car, key);//создааем объект класса
            genome.Go();//запускаем генетический алгоритм
            System.Console.WriteLine("done");
            var time = DateTime.Now;
            var time2 = time - time1;
            Log.WriteLog("Time(real): " + time2);
            Log.WriteLog("Better Chromosome");
            Log.WriteLog("Fitness:" + genome.betterChromosome.Fitness);
            string split = " ";
            for (int i = 0; i < genome.betterChromosome.Ch.Length; i++)
            {
                split += genome.betterChromosome.Ch[i] + " ";
            }
            Log.WriteLog(split);
            Log.WriteLog(split);
            int SHET = 0;
            for (int i = 0; i < genome.betterChromosome.Ch.Length; i++)
            {
                if (genome.betterChromosome.Ch[i] == true)
                {
                   
                        V[i].Name = 1;
                    SHET++;

                   
                }

                else
                    V[i].Name = 0;
            }

            Log.WriteLog("Cost" + SHET);//Log.WriteLog("Sheiner Points in Better Chromosome: " + shPCountinBetterChromosome);
            return Math.Round(genome.betterChromosome.Fitness);
        }


        public void GeneticRun()

        {
            int car = AllCar;
            double limitCostRate = CountStation; //ограничение на стоимость
            StreamReader print = new StreamReader("gen.txt");
            string line = print.ReadLine();
            int iteration = Convert.ToInt32(line);  //количество итерации
            line = print.ReadLine();
            int popSize = Convert.ToInt32(line);  //размер популяции
            line = print.ReadLine();

            double mutateRate = Convert.ToDouble(line); ;  //коэффицент мутации
            double maxCost = limitCostRate;
            line = print.ReadLine();
            int timepr = 10;
            print.Close();
            var time1 = DateTime.Now;
            log = new Log("random_150V" + limitCostRate);
            Log.WriteLog("Cost limit : " + limitCostRate);
            Log.WriteLog("Iterations : " + iteration);
            Log.WriteLog("Population size : " + popSize);
            Log.WriteLog("MutateRate : " + mutateRate);
            Log.WriteLog("Car: " + car);
            Log.WriteLog("Time: " + timepr);
            GA genome = new GA(V, E, PointStation, popSize, iteration, maxCost, limitCostRate, mutateRate, car);//создааем объект класса
            genome.Go();//запускаем генетический алгоритм
            System.Console.WriteLine("done");
            var time = DateTime.Now;
            var time2 = time - time1;
            Log.WriteLog("Time(real): " + time2);
            Log.WriteLog("Better Chromosome");
            Log.WriteLog("Fitness:" + genome.betterChromosome.Fitness);
            string split = " ";
            for (int i = 0; i < genome.betterChromosome.Ch.Length; i++)
            {
                split += genome.betterChromosome.Ch[i] + " ";
            }
            Log.WriteLog(split);
            Log.WriteLog(split);
            int SHET = 0;
            for (int i = 0; i < genome.betterChromosome.Ch.Length; i++)
            {
                if (genome.betterChromosome.Ch[i] == true)
                {
                    if (SHET < CountStation)
                    {
                        V[i].Name = 1;

                    }
                    else
                    {
                        break;
                    }
                }

                else
                    V[i].Name = 0;
            }

            //Log.WriteLog("Sheiner Points in Better Chromosome: " + shPCountinBetterChromosome);
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 form = new Form1();
            form.Show();

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Form ifrm = new Form2();
            ifrm.Show();

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
                    }

        private void button3_Click(object sender, EventArgs e)
        {
            AllCar = 300;
            CountStation = 8;
            iteration1 = 100;
             popSize1 =100;
            mutateRate1 = 0.01;

            int key = int.Parse("0"); //тут должен быть тхтбокс3
            BinarySearch_Rec_Wrapper(V.Count, key);
            //double pro = 0;
            //int GenPercent = int.Parse(textBox3.Text);
            //while (pro < GenPercent)
            //{

            //    pro = GeneticRun1(GenPercent);
            //    CountStation = CountStation + 1;
            //}
            //int y = 5;


        }
        public int BinarySearch_Rec(int countVertex,/* bool descendingOrder,*/ int key, int left, int right)
        {
            int mid = left + (right - left) / 2;
            
            double pro = 0;
            CountStation = mid;
            pro = GeneticRun1(key);
            if (left >= right)
                return -(1 + left);

            if (pro == key)
            {
                if ((mid2 == mid))
                    return mid;
              
                if (mid2>mid)
                {
                mid2 = mid;
                return BinarySearch_Rec(countVertex, key, left, mid);
                }
                if (mid2 < mid)
                {
                    return BinarySearch_Rec(countVertex, key, left, mid);
                }
                else
                    return BinarySearch_Rec(countVertex, key, mid + 1, right);

            }
                

            else if (pro > key)
                return BinarySearch_Rec(countVertex,  key, left, mid);
            else
                return BinarySearch_Rec(countVertex, key, mid + 1, right);
        }

        public int BinarySearch_Rec_Wrapper(int countVertex, int key)
        {
            if (countVertex == 0)
                return -1;

            //bool descendingOrder = array[0] > array[array.Length - 1];
            return BinarySearch_Rec(countVertex, /*descendingOrder,*/ key, 0, countVertex);
        }
    }
}

