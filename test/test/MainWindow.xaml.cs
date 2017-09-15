using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.Charts.Navigation;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay.ViewportRestrictions;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random = new Random();
        string target;
    
        public MainWindow()
        {
            InitializeComponent();
            //SetRange(1, 2, 2, 2);

        }

        private void SetRange(double x,double y,double width,double height)
        {

            plot.Visible = new Rect(x, y, width, height);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            target = T1.Text;
            plot.Children.RemoveAll(typeof(LineGraph));
            using (MySingnal db = new MySingnal())
            {
                var ypolotlist = (from a in db.MySingnalSet
                                  where a.Target == target
                                  select a.C2).ToList();
                setModels(ypolotlist);


            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            target = T1.Text;
            plot.Children.RemoveAll(typeof(LineGraph));
            using (MySingnal db = new MySingnal())
            {
                var ypolotlist = (from a in db.MySingnalSet
                                  where a.Target == target
                                  select a.C3).ToList();

                
                setModels(ypolotlist);


            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            target = T1.Text;
            plot.Children.RemoveAll(typeof(LineGraph));
            using (MySingnal db = new MySingnal())
            {
                var ypolotlist = (from a in db.MySingnalSet
                                  where a.Target == target
                                  select a.C1 ).ToList();
                setModels(ypolotlist);
            }
        }

        private void setModels(List<double> ypolotlist)
        {
            
            double[] x = new double[ypolotlist.Count];
            for (int i = 0; i < x.Length; i++)
            {
                x[i] = i;
            }
            var xDDR = new EnumerableDataSource<double>(x);
            xDDR.SetXMapping(u=>u);
            var yDDR = new EnumerableDataSource<double>(ypolotlist);
            yDDR.SetYMapping(u => u);
            CompositeDataSource cds = new CompositeDataSource(xDDR, yDDR);
            plot.AddLineGraph(cds, Colors.Red,1,"a");

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
     
            if (ofd.ShowDialog() == true)
            {
                WAVReader RE = new WAVReader();
                RE.ReadWAVFile(ofd.FileName);
                MessageBox.Show("OK!!");
            }
            else
            {
                MessageBox.Show("Not select");
            }
            
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
             
                T1.Text =ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\')+1);
                
            }
            else
            {
                MessageBox.Show("Not select");
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            ViewportAxesRangeRestriction restr = new ViewportAxesRangeRestriction();
            restr.XRange = new DisplayRange(1, 105);
            plot.Viewport.Restrictions.Add(restr);
            //var axis = (DateTimeAxis)productPlot.MainHorizontalAxis;
            //double yMin = 0;
            //double yMax = 100;
            //Rect domainRect = new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
            ////xMin and xMax are left to your discretion based on your DateTimeAxis
            //
            //plot.vi = domainRect;

        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            double x;
            double y;
            double width;
            double heigth;

            if (double.TryParse(t1.Text, out x) && double.TryParse(t2.Text, out y) && double.TryParse(t3.Text, out width) && double.TryParse(t4.Text, out heigth))
                SetRange(x,y,width,heigth);

        }

        double MinX;
        double MaxX;
        double MinY;
        double MaxY;
        private void plot_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
        
            }
        }

        
        private void plot_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = mouseTrack.Position;
            var tranform = plot.Viewport.Transform;
            Point mouseData = mousePos.ScreenToData(tranform);

            MinX = mouseData.X;
            MinY = mouseData.Y;


        }
        private CursorCoordinateGraph mouseTrack;
        private void plot_Loaded(object sender, RoutedEventArgs e)
        {
            mouseTrack = new CursorCoordinateGraph();
            mouseTrack.Visibility = Visibility.Hidden;
            plot.Children.Add(mouseTrack);

        }

        private void plot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void plot_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl)
            {
                Point mousePos = mouseTrack.Position;
                var tranform = plot.Viewport.Transform;
                Point mouseData = mousePos.ScreenToData(tranform);

                MaxX = mouseData.X;
                MaxY = mouseData.Y;

               
                ReRangeAxis();

            }
        }

        private void ReRangeAxis()
        {
            double DX = MaxX - MinX;

            double DY = MaxY - MinY;
            if (DX < 0)
            {
                if (DY < 0)
                {

                    SetRange(MaxX, MaxY, Math.Abs(DX), Math.Abs(DY));
                }
                else
                {
                    SetRange(MaxX, MinY, Math.Abs(DX), Math.Abs(DY));
                }
            }
            else
            {
                if (DY < 0)
                {

                    SetRange(MinX, MaxY, Math.Abs(DX), Math.Abs(DY));
                }
                else
                {
                    SetRange(MinX, MinY, Math.Abs(DX), Math.Abs(DY));
                }
            }
        }
    }

    public class DisplayRange
    {
        public double Start { get; set; }
        public double End { get; set; }

        public DisplayRange(double start, double end)
        {
            Start = start;
            End = end;
        }
    }

    public class ViewportAxesRangeRestriction : IViewportRestriction
    {
        public DisplayRange XRange = null;
        public DisplayRange YRange = null;

        public Rect Apply(Rect oldVisible, Rect newVisible, Viewport2D viewport)
        {
            if (XRange != null)
            {
                newVisible.X = XRange.Start;
                newVisible.Width = XRange.End - XRange.Start;
            }

            if (YRange != null)
            {
                newVisible.Y = YRange.Start;
                newVisible.Height = YRange.End - YRange.Start;
            }

            return newVisible;
        }

        public event EventHandler Changed;
    }
    class WAVReader
    {
        List<short> listData = new List<short>();
        #region RIFF WAVE Chunk
        private string Id; //文件标识
        private double Size;  //文件大小
        private string Type; //文件类型
        #endregion

        #region Format Chunk
        private string formatId;
        private double formatSize;      //数值为16或18，18则最后又附加信息
        private int formatTag;
        private int num_Channels;       //声道数目，1--单声道；2--双声道
        private int SamplesPerSec;      //采样率
        private int AvgBytesPerSec;     //每秒所需字节数 
        private int BlockAlign;         //数据块对齐单位(每个采样需要的字节数) 
        private int BitsPerSample;      //每个采样需要的bit数
        private string additionalInfo;  //附加信息（可选，通过Size来判断有无）
        /*
         * 以'fmt'作为标示。一般情况下Size为16，此时最后附加信息没有；
         * 如果为18则最后多了2个字节的附加信息。
         * 主要由一些软件制成的wav格式中含有该2个字节的附加信息
         */
#endregion

        #region Fact Chunk(可选)
        /*
                * Fact Chunk是可选字段，一般当wav文件由某些软件转化而成，则包含该Chunk。
     
             * */
        private string factId;
        private int factSize;
        private string factData;
        #endregion

        #region Data Chunk
        private string dataId;
        private int dataSize;
        private List<double> wavdata = new List<double>();  //默认为单声道
        #endregion


        /// <summary>
        /// 读取波形文件并显示
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadWAVFile(string filePath)
        {
            if (filePath == "") return;
            byte[] id = new byte[4];
            byte[] size = new byte[4];
            byte[] type = new byte[4];

            byte[] formatid = new byte[4];
            byte[] formatsize = new byte[4];
            byte[] formattag = new byte[2];
            byte[] numchannels = new byte[2];
            byte[] samplespersec = new byte[4];
            byte[] avgbytespersec = new byte[4];
            byte[] blockalign = new byte[2];
            byte[] bitspersample = new byte[2];
            byte[] additionalinfo = new byte[2];    //可选

            byte[] factid = new byte[4];
            byte[] factsize = new byte[4];
            byte[] factdata = new byte[4];

            byte[] dataid = new byte[4];
            byte[] datasize = new byte[4];


            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.UTF8))
                {
                    #region  RIFF WAVE Chunk
                    br.Read(id, 0, 4);
                    br.Read(size, 0, 4);
                    br.Read(type, 0, 4);



                    this.Id = getString(id, 4);
                    long longsize = bytArray2Int(size);//十六进制转为十进制
                    this.Size = longsize * 1.0;
                    this.Type = getString(type, 4);
                    #endregion

                    #region Format Chunk
                    br.Read(formatid, 0, 4);
                    br.Read(formatsize, 0, 4);
                    br.Read(formattag, 0, 2);
                    br.Read(numchannels, 0, 2);
                    br.Read(samplespersec, 0, 4);
                    br.Read(avgbytespersec, 0, 4);
                    br.Read(blockalign, 0, 2);
                    br.Read(bitspersample, 0, 2);
                    if (getString(formatsize, 2) == "18")
                    {
                        br.Read(additionalinfo, 0, 2);
                        this.additionalInfo = getString(additionalinfo, 2);  //附加信息
                    }

                    this.formatId = getString(formatid, 4);

                    this.formatSize = bytArray2Int(formatsize);

                    byte[] tmptag = composeByteArray(formattag);
                    this.formatTag = bytArray2Int(tmptag);

                    byte[] tmpchanels = composeByteArray(numchannels);
                    this.num_Channels = bytArray2Int(tmpchanels);                //声道数目，1--单声道；2--双声道

                    this.SamplesPerSec = bytArray2Int(samplespersec);            //采样率

                    this.AvgBytesPerSec = bytArray2Int(avgbytespersec);          //每秒所需字节数   

                    byte[] tmpblockalign = composeByteArray(blockalign);
                    this.BlockAlign = bytArray2Int(tmpblockalign);              //数据块对齐单位(每个采样需要的字节数)

                    byte[] tmpbitspersample = composeByteArray(bitspersample);
                    this.BitsPerSample = bytArray2Int(tmpbitspersample);        // 每个采样需要的bit数     
                    #endregion

                    #region  Fact Chunk
                    //byte[] verifyFactChunk = new byte[2];
                    //br.Read(verifyFactChunk, 0, 2);
                    //string test = getString(verifyFactChunk, 2);
                    //if (getString(verifyFactChunk, 2) == "fa")
                    //{
                    //    byte[] halffactId = new byte[2];
                    //    br.Read(halffactId, 0, 2);

                    //    byte[] factchunkid = new byte[4];
                    //    for (int i = 0; i < 2; i++)
                    //    {
                    //        factchunkid[i] = verifyFactChunk[i];
                    //        factchunkid[i + 2] = halffactId[i];
                    //    }

                    //    this.factId = getString(factchunkid, 4);

                    //    br.Read(factsize, 0, 4);
                    //    this.factSize = bytArray2Int(factsize);

                    //    br.Read(factdata, 0, 4);
                    //    this.factData = getString(factdata, 4);
                    //}
                    #endregion

                    #region Data Chunk
                    List<MySingnalSet> singnallist = new List<MySingnalSet>();
                    byte[] d_flag = new byte[1];
                    while (true)
                    {
                        br.Read(d_flag, 0, 1);
                        if (getString(d_flag, 1) == "d")
                        {
                            break;
                        }

                    }
                    byte[] dt_id = new byte[4];
                    dt_id[0] = d_flag[0];
                    br.Read(dt_id, 1, 3);
                    this.dataId = getString(dt_id, 4);

                    br.Read(datasize, 0, 4);

                    this.dataSize = bytArray2Int(datasize);

                    List<string> testl = new List<string>();

                    if (BitsPerSample == 8)
                    {

                        for (int i = 0; i < this.dataSize; i++)
                        {
                            byte wavdt = br.ReadByte();
                            wavdata.Add(wavdt);
                            //Console.WriteLine(wavdt);
                        }
                    }
                    else if (BitsPerSample == 16)
                    {
                        string pathTarget = filePath.Substring(filePath.LastIndexOf('\\') + 1);
                        MySingnalSet singnal = new MySingnalSet();
                        for (int i = 0; i < this.dataSize / 2; i++)
                        {
                            short wavdt = br.ReadInt16();
                            wavdata.Add(wavdt);
                            // Console.WriteLine(wavdt);

                            if (num_Channels == 3)
                            {
                                singnal.Target = pathTarget;
                                if (i % num_Channels == 0)
                                {
                                    singnal.C1 = wavdt * 0.01496;
                                    continue;
                                }
                                if (i % num_Channels == 1)
                                {
                                    singnal.C2 = wavdt * 0.5;
                                    continue;
                                }
                                if (i % 3 == 2)
                                {
                                    singnal.C3 = wavdt * 0.01496;
                                }
                            }
                            if (num_Channels == 2)
                            {
                                singnal.Target = pathTarget;
                                if (i % num_Channels == 0)
                                {
                                    singnal.C1 = wavdt * 0.01496;
                                    continue;
                                }
                                if (i % num_Channels == 1)
                                {
                                    singnal.C2 = wavdt * 0.5;

                                }
                            }
                            else
                            {
                                singnal.Target = pathTarget;
                                
                                    singnal.C1 = wavdt;
                                  
                                
                            }

                            singnallist.Add(singnal);
                            singnal = new MySingnalSet();
                            //listData.Add(wavdt);
                        }

                        //foreach (var item in listData)
                        //{
                        //    if (listData.IndexOf(item)%3 == 0)
                        //    {
                        //        singnal.c1 = item.ToString();
                        //        continue;
                        //    }
                        //    if (listData.IndexOf(item)%3 == 1)
                        //    {
                        //        singnal.c2 = item.ToString();
                        //        continue;
                        //    }
                        //    if (listData.IndexOf(item)%3 == 2)
                        //    {
                        //        singnal.c3 = item.ToString();
                        //    }
                        //    singnallist.Add(singnal);
                        //    singnal = new singnalSet();

                        //}
                        using (MySingnal db = new MySingnal()){

                            db.BulkInsert(singnallist);
                            db.BulkSaveChanges();
                        }
                        
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 数字节数组转换为int
        /// </summary>
        /// <param name="bytArray"></param>
        /// <returns></returns>
        private int bytArray2Int(byte[] bytArray)
        {
            return bytArray[0] | (bytArray[1] << 8) | (bytArray[2] << 16) | (bytArray[3] << 24);
        }

        /// <summary>
        /// 将字节数组转换为字符串
        /// </summary>
        /// <param name="bts"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        private string getString(byte[] bts, int len)
        {
            char[] tmp = new char[len];
            for (int i = 0; i < len; i++)
            {
                tmp[i] = (char)bts[i];
            }
            return new string(tmp);
        }

        /// <summary>
        /// 组成4个元素的字节数组
        /// </summary>
        /// <param name="bt"></param>
        /// <returns></returns>
        private byte[] composeByteArray(byte[] bt)
        {
            byte[] tmptag = new byte[4] { 0, 0, 0, 0 };
            tmptag[0] = bt[0];
            tmptag[1] = bt[1];
            return tmptag;
        }
    }
}
