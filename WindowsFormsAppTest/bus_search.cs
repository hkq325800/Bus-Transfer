using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Configuration;

namespace busSearch
{
    
    public partial class frmTest : Form
    {
        public const int max = 100;
        public string[,] a1 = new string[max, 2];
        public int[,] a2 = new int[max, 3];
        public string[,] b1 = new string[max, 4];
        public int[,] b2 = new int[max, 4];
        public string[,] c1 = new string[max, 8];
        public int[,] c2 = new int[max, 6];
        int index = 0;
        string sql=null;
        OleDbDataReader reader;
        OleDbCommand cmd;
        static LogInfo log = new LogInfo();
        public frmTest()
        {
            InitializeComponent();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            string connstr = ConfigurationManager.AppSettings["connectionstring"];
            //string SqlConnString = "Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Password=123;Initial Catalog=mydb;Data Source=192.168.0.133";
            //string SqlConn=@"Provider=SQLOLEDB.1;Persist Security Info=False;User ID=sa;Password=123;Data Source=2012-20131018GT;Initial File Name=D:\桌面\程序\12108413\WindowsFormsAppTest\WindowsFormsAppTest\bin\mydb.mdf";
            GlobalVariants.conn = new OleDbConnection(connstr);
            GlobalVariants.conn.Open();
        }

        private void frmTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            GlobalVariants.conn.Close();
        }
        private void btnLine_Click(object sender, EventArgs e)//若未找到返回message
        {
            if (lstLine.SelectedItem != null && cmbLine.Text != lstLine.SelectedItem.ToString())
            {
                cmbLine.Text = lstLine.SelectedItem.ToString();
            }
            int a = 0, b = 0, c = 0;//用来跳转标签
            tabControl2.SelectedTab = tabPageUp;
            if (GlobalVariants.clear1 > 8)//到达11条自动清除
            {
                cmbLine.Items.Clear();
                GlobalVariants.clear1 = 0;
            }
            GlobalVariants.clear1++;
            cmbLine.Items.Add(cmbLine.Text);//用来记录历史数据
            string sql1 = @"select stopname from buslinesinfo where linename = '" + cmbLine.Text + @"' and linedirection = 1 order by linestopindex";
            string sql2 = @"select stopname from buslinesinfo where linename = '" + cmbLine.Text + @"' and linedirection = 2 order by linestopindex";
            string sql3 = @"select stopname from buslinesinfo where linename = '" + cmbLine.Text + @"' and linedirection = 0 order by linestopindex";
            lst1.Items.Clear();
            lst2.Items.Clear();
            lst3.Items.Clear();
            OleDbCommand command1 = new OleDbCommand(sql1, GlobalVariants.conn);
            OleDbCommand command2 = new OleDbCommand(sql2, GlobalVariants.conn);
            OleDbCommand command3 = new OleDbCommand(sql3, GlobalVariants.conn);
            OleDbDataReader reader1 = command1.ExecuteReader();
            OleDbDataReader reader2 = command2.ExecuteReader();
            OleDbDataReader reader3 = command3.ExecuteReader();

            while (reader1.Read())
            {
                a++;
                lst1.Items.Add("(" + a + ")"+reader1["stopname"].ToString());
            }
             GlobalVariants.upmax=a ;
            while (reader2.Read())
            {
                b++;
                lst2.Items.Add("(" + b + ")"+reader2["stopname"].ToString());
            }
            GlobalVariants.downmax=b;
            while (reader3.Read())
            {
                c++;
                lst3.Items.Add("(" + c + ")"+reader3["stopname"].ToString());
            }
            GlobalVariants.roundmax=c;
            reader1.Close();
            reader2.Close();
            reader3.Close();
            command1.Dispose();
            command2.Dispose();
            command3.Dispose();
            if (c != 0)
            {
                tabControl2.SelectedTab = tabPageRound;

            }
            lstLine.Visible = false;
            focus_Line(sender, e);
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (lstStop.SelectedItem != null && cmbStop.Text != lstStop.SelectedItem.ToString())
            {
                //this.AcceptButton = btnStop;
                cmbStop.Text = lstStop.SelectedItem.ToString();
                //btnStop_Click(sender, e);
            }
            lst4.Items.Clear();
            if (GlobalVariants.clear2 > 8)
            {
                cmbStop.Items.Clear();
                GlobalVariants.clear2 = 0;
            }
            GlobalVariants.clear2++;
            cmbStop.Items.Add(cmbStop.Text);//保留此次查询记录
            sql = @"select distinct linename from buslinesinfo where stopname = '" + cmbStop.Text + @"'";
            lstStop.Items.Clear();cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lst4.Items.Add(reader["linename"].ToString());
            }
            reader.Close();
            lstStop.Visible = false;
            focus_Stop(sender, e);
        }
        private void btnExchange_Click(object sender, EventArgs e)
        {
            if (lstStart.SelectedItem != null && cmbStart.Text != lstStart.SelectedItem.ToString())//将lststart中查到的数据填充到cmb中
            {
                cmbStart.Text = lstStart.SelectedItem.ToString();
                lstStart.Visible = false;
                focus_End(sender, e);
                return;
            }
            if (lstEnd.SelectedItem != null && cmbEnd.Text != lstEnd.SelectedItem.ToString())//将lstend中查到的数据填充到cmb中//&& cmbEnd.Text != lstEnd.SelectedItem.ToString()
            {
                cmbEnd.Text = lstEnd.SelectedItem.ToString();
                lstEnd.Visible = false;
                btnExchange_Click(sender, e);
                return;
            }
            if (cmbStart.Text == "" || cmbEnd.Text == "")
            {
                if (cmbStart.Text == "")
                {
                    focus_Start(sender, e);
                }
                else focus_End(sender, e);
                lstStart.Visible = false;
                lstEnd.Visible = false;
                return;
            }
            if (cmbEnd.Text != GlobalVariants.text2 || cmbStart.Text != GlobalVariants.text1)
            {
                MessageBox.Show("起点或终点不存在！");
                return;
            }
            if (GlobalVariants.clear3 > 8)
            {
                cmbStart.Items.Clear();
                GlobalVariants.clear3 = 0;
            }
            GlobalVariants.clear3++;
            cmbStart.Items.Add(cmbStart.Text);
            if (GlobalVariants.clear4 > 8)
            {
                cmbEnd.Items.Clear();
                GlobalVariants.clear4 = 0;
            }
            GlobalVariants.clear4++;
            cmbEnd.Items.Add(cmbEnd.Text);
            GlobalVariants.examOne = false;
            GlobalVariants.examTwo = false;
            GlobalVariants.examThree = false;
            int A = 0, B = 0, C = 0, D = 0, E = 0;
            a1[0, 0] = null;
            string through = null;
            lstExchange.Text = null;
            string sql1 = "select t1.linename,t1.linedirection as direction,(t2.linestopindex-t1.linestopindex) as stopcount,t1.linestopindex as indexstart,t2.linestopindex as indexend from (select linename,linedirection,linestopindex from busLinesInfo where stopname='" + cmbStart.Text + "') t1 join (select linename,linedirection,linestopindex from busLinesInfo where stopname='" + cmbEnd.Text + "') t2 on t1.linename=t2.linename where (t2.linestopindex-t1.linestopindex)> 0 and t1.linedirection=t2.linedirection order by linename,stopcount,t1.LINEDIRECTION";
            OleDbCommand command1 = new OleDbCommand(sql1, GlobalVariants.conn);
            OleDbDataReader reader1 = command1.ExecuteReader();
            while (reader1.Read())
            {
                a1[A, 0] = reader1["linename"].ToString();//线路名
                A++;
                a1[B, 1] = reader1["direction"].ToString();//方向
                B++;
                a2[C, 0] = int.Parse(reader1["stopcount"].ToString());//经历站数
                C++;
                a2[D, 1] = int.Parse(reader1["indexstart"].ToString());//开始站数
                D++;
                a2[E, 2] = int.Parse(reader1["indexend"].ToString());//结束站数
                E++;
            }
            reader1.Close();
            command1.Dispose();
            if (A != B && A != C && A != D && A != E)
            {
                MessageBox.Show("发生未知错误");//此时A=B=C=D=E=总搜索条数
            }
            if (a1[0, 0] != null)//若查到任意一条线路
            {
                for (A = 0; A < B; A++)//A为当前条数、B为总条数
                {
                    //C=indexstart
                    D = a2[A, 2];//D=indexend
                    through = null;
                    for (C = a2[A, 1]; C <= D; C++)//从C开始 到D结束
                    {
                        through += "" + Stop(a1[A, 0], C, a1[A, 1]) + "   ";//a1[A,0]=linename,C=index,direction=a1[A,1]
                    }
                    lstExchange.Text += ("直达线路：" + a1[A, 0] + "" + '\n' + "");
                    lstExchange.Text += ("经过" + through + "" + '\n' + "");
                    lstExchange.Text += ("共经过 " + a2[A, 0] + "个站" + '\n' + '\n' + "");
                    //log.writeIn(lstExchange.Text);

                }
                GlobalVariants.examOne = true;//检查是否有直达
                focus_Start(sender, e);//最后跳回cmbstart
                lstExchange.Visible = true;
                lstEnd.Visible = false;//两个lst隐藏
                lstStart.Visible = false;
                log.writeIn(lstExchange.Text);
                return;
            }
            else if (GlobalVariants.examOne != true)//若无直达
            {
                b1[0, 0] = null; b1[0, 1] = null;
                A = 0; B = 0; C = 0; D = 0; E = 0;
                int F = 0, G = 0, H = 0;
                string through1 = null;
                lstExchange.Text = null;
                string sql2 = "select distinct top 5 t2.linename as linestart,t4.LINENAME as lineend ,t2.t2index-differ1 as startindex1 ,t4.t4index-differ2 startindex2,t2.t2index as transstart,t4.t4index as transend,t2.t1direction,t4.t3direction,differ1+differ2 from (select distinct t1.linename,stopname,bus1.linestopindex as t2index,t1index,bus1.LINESTOPINDEX-t1index as differ1,t1.linedirection as t1direction from BusLinesInfo bus1 join (select distinct linename,linestopindex as t1index,linedirection from busLinesInfo where stopname='" + cmbStart.Text + "') t1 on bus1.LINENAME=t1.LINENAME where bus1.linestopindex-t1index>0 and t1.LINEDIRECTION=bus1.LINEDIRECTION ) t2 join (select distinct t3.linename,stopname,bus2.LINESTOPINDEX as t4index,t3index,bus2.LINESTOPINDEX-t3index as differ2,t3.linedirection as t3direction from BusLinesInfo bus2 join (select distinct linename,linestopindex as t3index,linedirection from busLinesInfo where stopname='" + cmbEnd.Text + "') t3 on bus2.LINENAME=t3.LINENAME  where bus2.linestopindex-t3index>0 and t3.LINEDIRECTION=bus2.LINEDIRECTION ) t4 on t2.stopNAME=t4.STOPNAME order by differ1+differ2";
                OleDbCommand command2 = new OleDbCommand(sql2, GlobalVariants.conn);
                OleDbDataReader reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    b1[A, 0] = reader2["linestart"].ToString();//起点站线路
                    A++;
                    b1[B, 1] = reader2["lineend"].ToString();//终点站线路
                    B++;
                    b2[C, 0] = int.Parse(reader2["startindex1"].ToString());//第一辆车开始的站序列C
                    C++;
                    b2[D, 1] = int.Parse(reader2["startindex2"].ToString());//第二辆车结束的站序列D
                    D++;
                    b2[E, 2] = int.Parse(reader2["transstart"].ToString());//第一辆车结束的站序列E
                    E++;
                    b2[F, 3] = int.Parse(reader2["transend"].ToString());//第二辆车开始的站序列F
                    F++;
                    b1[G, 2] = reader2["t1direction"].ToString();//第一辆车方向
                    G++;
                    b1[H, 3] = reader2["t3direction"].ToString();//第二辆车方向
                    H++;
                }
                reader2.Close();
                command2.Dispose();
                if (A != B && A != C && A != D && A != E && A != F && A != G && A != H)
                {
                    MessageBox.Show("发生未知错误");//此时A=B=C=D=E=F=总搜索条数
                }
                if (b1[0, 0] != null && b1[0, 1] != null) //若有符合的两条线路
                {
                    string stop1 = null;
                    string through2 = null;
                    int i = 1;
                    for (A = 0; A < B; A++)//A为当前条数、B为总条数
                    {
                        //C=startindex1
                        E = b2[A, 2];//E=transstart
                        through1 = null;
                        for (C = b2[A, 0]; C <= E; C++)//从C开始 到E结束
                        {
                            through1 += "" + Stop(b1[A, 0], C, b1[A, 2]) + "   ";//b1[A,0]=linestart,C=startindex,direction=b1[A,2]
                        }
                        //D=startindex2
                        D = b2[A, 1];//F=transend
                        through2 = null; G = 0; H = 0;
                        for (F = b2[A, 3]; D <= F; F--)//从D开始 到F结束
                        {
                            through2 += "" + Stop(b1[A, 1], F, b1[A, 3]) + "   ";//b1[A,1]=lineend,F=transend,direction=b1[A,3]

                        }
                        stop1 = "" + Stop(b1[A, 1], b2[A, 3], b1[A, 3]) + "";
                        int sum = b2[A, 3] - b2[A, 1] + b2[A, 2] - b2[A, 0];
                        lstExchange.Text += ("一次换乘方案 " + i + ":" + '\n' + "乘坐线路  " + b1[A, 0] + "在" + stop1 + "转线路   " + b1[A, 1] + "" + '\n' + "");
                        lstExchange.Text += ("经过" + through1 + "" + through2 + "" + '\n' + " 共 " + sum + "个站" + '\n' + '\n' + "");

                        i++;
                    }
                    lstExchange.Visible = true;
                    GlobalVariants.examTwo = true;//检查是否有一次换乘方案
                    focus_Start(sender, e);//最后跳回cmbstart
                    lstStart.Visible = false;
                    lstEnd.Visible = false;
                    log.writeIn(lstExchange.Text);
                    return;
                }
                else if (GlobalVariants.examTwo != true)//若没有一次换乘方案
                {
                    int I = 0, J = 0, K = 0, L = 0, M = 0, N = 0;
                    c1[0, 0] = null; c1[0, 3] = null; c1[0, 6] = null;
                    string sql3 = "select distinct top 3 t5.t3line,t5.t3direction,t5.t5stop,t5.t5line,t5.t5direction,t6.t6stop,t6.t4line,t6.t4direction,t5.t1index,t5.b1index,t5.t5index,t6.t6index,t6.t2index,t6.b2index,b1index-t1index+t6index-t5index+b2index-t2index as scount from (select distinct B1.LINENAME t5line,B1.STOPNAME as t5stop,B1.LINESTOPINDEX as t5index,B1.LINEDIRECTION as t5direction,t3.LINENAME as t3line,t3.STOPNAME as t3stop,t3.t1index,t3.b1index,t3.LINEDIRECTION as t3direction,t3.b1index-t3.t1index as differ1 from BusLinesInfo B1 join (select distinct t1.linename,stopname,t1.LINESTOPINDEX as t1index,b1.linestopindex as b1index,b1.linedirection from BusLinesInfo b1 join (select distinct linename,linestopindex,linedirection from busLinesInfo where stopname='" + cmbStart.Text + "') t1 on b1.LINENAME=t1.LINENAME and b1.LINEDIRECTION=t1.LINEDIRECTION where b1.LINESTOPINDEX-t1.LINESTOPINDEX >=0 ) t3 on B1.STOPNAME=t3.STOPNAME where B1.LINENAME<>t3.LINENAME ) t5 join (select distinct B2.LINENAME t6line,B2.STOPNAME as t6stop,B2.LINESTOPINDEX as t6index,B2.LINEDIRECTION as t6direction,t4.LINENAME as t4line,t4.STOPNAME as t4stop,t4.t2index,t4.b2index,t4.LINEDIRECTION as t4direction,t4.b2index-t4.t2index as differ2 from BusLinesInfo B2 join (select distinct t2.linename,stopname,t2.LINESTOPINDEX as t2index,b2.linestopindex as b2index,b2.linedirection from BusLinesInfo b2 join (select distinct linename,linestopindex,linedirection from busLinesInfo where stopname='" + cmbEnd.Text + "') t2 on b2.LINENAME=t2.LINENAME and b2.LINEDIRECTION=t2.LINEDIRECTION where b2.LINESTOPINDEX-t2.LINESTOPINDEX >=0 ) t4 on B2.STOPNAME=t4.STOPNAME where B2.LINENAME<>t4.LINENAME ) t6 on t5.t5line=t6.t6line where t5.t5direction=t6.t6direction and b1index-t1index+t6index-t5index+b2index-t2index>20 and t6index-t5index >=0 order by scount,t5.t3line ";
                    OleDbCommand command3 = new OleDbCommand(sql3, GlobalVariants.conn);
                    OleDbDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        c1[A, 0] = reader3["t3line"].ToString();//
                        A++;
                        c1[B, 1] = reader3["t3direction"].ToString();//
                        B++;
                        c1[C, 2] = reader3["t5stop"].ToString();//
                        C++;
                        c1[D, 3] = reader3["t5line"].ToString();//
                        D++;
                        c1[E, 4] = reader3["t5direction"].ToString();//
                        E++;
                        c1[F, 5] = reader3["t6stop"].ToString();//
                        F++;
                        c1[G, 6] = reader3["t4line"].ToString();//
                        G++;
                        c1[H, 7] = reader3["t4direction"].ToString();//
                        H++;
                        c2[I, 0] = int.Parse(reader3["t1index"].ToString());//
                        I++;
                        c2[J, 1] = int.Parse(reader3["b1index"].ToString());//
                        J++;
                        c2[K, 2] = int.Parse(reader3["t5index"].ToString());//
                        K++;
                        c2[L, 3] = int.Parse(reader3["t6index"].ToString());//
                        L++;
                        c2[M, 4] = int.Parse(reader3["t2index"].ToString());//
                        M++;
                        c2[N, 5] = int.Parse(reader3["b2index"].ToString());//
                        N++;
                    }
                    command3.Dispose();
                    reader3.Close();
                    if (A != B && A != C && A != D && A != E && A != F && A != G && A != H && A != I && A != J && A != K && A != L && A != M && A != N)
                    {
                        MessageBox.Show("发生未知错误");//此时A=B=C=D=E=F=I=J=K=L=M=N=总搜索条数
                    }
                    if (c1[0, 0] != null && c1[0, 3] != null && c1[0, 6] != null)
                    {

                        if (c1[0, 0] != null && c1[0, 3] != null && c1[0, 6] != null)
                        {
                            lstExchange.Text = null;
                            through1 = null;
                            string through2 = null;
                            string through3 = null;
                            int i = 1;
                            for (A = 0; A < B; A++)//A为当前条数、B为总条数
                            {
                                string stop1 = null;
                                string stop2 = null;
                                //C=t1index
                                D = c2[A, 1];//D=b1index
                                for (C = c2[A, 0] + 1; C <= D; C++)//从C开始 到E结束
                                {
                                    through1 += "" + Stop(c1[A, 0], C, c1[A, 1]) + "   ";//c1[A,0]=t3line,C=t1index,direction=c1[A,1]
                                }
                                //E=t5index
                                F = c2[A, 3];//F=t6index
                                for (E = c2[A, 2] + 1; E <= F; E++)//从D开始 到F结束
                                {
                                    through2 += "" + Stop(c1[A, 3], E, c1[A, 4]) + "   ";//c1[A,3]=t5line,E=t5index,direction=c1[A,4]

                                }
                                G = c2[A, 4];//G=t2index
                                //H=b2index
                                for (H = c2[A, 5] - 1; G <= H; H--)
                                {
                                    through3 += "" + Stop(c1[A, 6], H, c1[A, 7]) + "   ";//c1[A,6]=t4line,G=t2index,direction=c1[A,7]
                                }
                                stop1 = "" + c1[A, 2] + "";
                                stop2 = "" + c1[A, 5] + "";
                                int sum = c2[A, 1] - c2[A, 0] + c2[A, 3] - c2[A, 2] + c2[A, 5] - c2[A, 4];
                                lstExchange.Text += ("二次换乘方案 " + i + ":乘坐线路 " + c1[A, 0] + "" + '\n' + "经" + through1 + "" + '\n' + "到" + stop1 + "转线路 " + c1[A, 3] + "" + '\n' + "经" + through2 + "" + '\n' + "到" + stop2 + "转线路 " + c1[A, 6] + "" + '\n' + "经" + through3 + "  到达 共" + sum + "站路" + '\n' + "" + '\n' + "");
                                i++;
                            }
                        }
                        lstExchange.Visible = true;
                        GlobalVariants.examThree = true;//检查是否有二次换乘方案
                        focus_Start(sender, e);//最后跳回cmbstart
                        lstStart.Visible = false;
                        lstEnd.Visible = false;
                        log.writeIn(lstExchange.Text);
                        return;
                    }
                    else if (GlobalVariants.examThree != true)//若没有二次换乘方案
                    {
                        MessageBox.Show("没有合适的抵达方案！");
                    }
                }
            }

        }

        private void btnNext_Click(object sender, EventArgs e)//生成当前窗体中包含信息的sql语句
        {
            if (GlobalVariants.text1 == cmbNewLine.Text || cmbNewLine.Text == ""||txtNewStop.Text=="")
            {
                if (GlobalVariants.builder != null)
                { 
                    txtNewStop.Focus();
                    return;
                }
                else
                {
                    if (GlobalVariants.text1 == cmbNewLine.Text)
                    {
                        MessageBox.Show("线路已存在!");
                        cmbNewLine.Text = "";
                        cmbNewLine.Focus(); return;
                    }
                    else if(cmbNewLine.Text=="")
                    {
                        MessageBox.Show("请输入要新增的线路名称！");
                        cmbNewLine.Text = "";
                        cmbNewLine.Focus();return;
                    }
                    
                }
            }
            else
            {
                int direction = 1;
                if (cmbNewDirection.Text == "上行")
                {
                    direction = 1;
                }
                else if (cmbNewDirection.Text == "下行") { direction = 2; }
                else direction = 0;
                cmbNewLine.Enabled = false;//
                lstNewLine.Visible = false;
                btnRound.Visible = false;
                index = int.Parse(txtNewIndex.Text);
                txtNewIndex.Clear();
                txtNewIndex.Text = index.ToString();//设置index++
                GlobalVariants.builder += "insert into BusLinesInfo values ('" + cmbNewLine.Text + "','" + txtNewStop.Text + "'," + txtNewIndex.Text + "," + direction + ")";
                index++; 
                txtNewIndex.Text = index.ToString();
                txtNewStop.Clear();
                txtNewStop.Focus();
            }
        }
        private void btnRe_Click(object sender, EventArgs e)
        {
            if (GlobalVariants.builder == null)
            {
                MessageBox.Show("上行尚未填写完整！");
                return;
            }
            else
            {
                index = 0;
                Point x;
                x = new Point(469, 227);
                txtNewIndex.Text = "1";
                focus_New(sender, e);
                cmbNewDirection.Text = "下行";
                txtNewStop.Focus();
                txtNewStop.SelectAll();
                btnRe.Visible = false;
                btnNew.Location = x;
            }
        }
        private void btnswap_Click(object sender, EventArgs e)
        {
            string temp = null;
            temp = cmbStart.Text;
            cmbStart.Text = cmbEnd.Text;
            cmbEnd.Text = temp;
            this.AcceptButton = btnExchange;
        }
        private void btnLineUpdate_Click(object sender, EventArgs e)
        {
            GlobalVariants.gap1 = true;
            GlobalVariants.gap2 = true;
            cmbModifyIndex.Items.Clear();
            if (tabControl2.SelectedTab == tabPageUp) { sender = lst1; cmbModifyDirection.Text = "上行"; GlobalVariants.flag = 1; }
            else if (tabControl2.SelectedTab == tabPageDown) { sender = lst2; cmbModifyDirection.Text = "下行"; GlobalVariants.flag = 2; }
            else { sender = lst3; cmbModifyDirection.Text = "回环"; GlobalVariants.flag = 3; }//设置GlobalVariants.flag的值
            if ((sender as ListBox).SelectedItem == null)
                MessageBox.Show("请在右边表格中选择一个站点进行修改！");
            else 
            {
                string text = null;
                lstModifyLine.Visible = false;
                if (GlobalVariants.clear5 > 8)//到达11条自动清除
                {
                    cmbModifyLine.Items.Clear();
                    GlobalVariants.clear5 = 0;
                }
                GlobalVariants.clear5++;
                cmbModifyLine.Items.Add(cmbLine.Text);//用来记录历史数据
                tabControl1.SelectedTab = tabPage4;//转换标签
                tabControl3.SelectedTab = tabPage6;//转换标签
                cmbModifyLine.Text = cmbLine.Text;//设置cmbModifyLine的初始值
                txtModifyStop.Text = (sender as ListBox).SelectedItem.ToString();//设置txtModifyStop的初始值
                txtModifyStop.Text = txtModifyStop.Text.Remove(0, 3);
                if (txtModifyStop.Text.StartsWith(")"))
                {
                    txtModifyStop.Text = txtModifyStop.Text.Remove(0, 1);
                }//设置txtModifyStop的初始值
                text = (sender as ListBox).SelectedItem.ToString();//设置cmbModifyIndex的初始值
                text = text.Remove(0, 1);
                text = text.Remove(2);
                if (text.EndsWith(")"))
                {
                    text = text.Remove(1, 1);
                }
                
                int index = 0;
                if (GlobalVariants.flag == 1)//设置cmbModifyIndex的集合 GlobalVariants.flag为方便Index的设置
                {
                    for (index = 1; index <= GlobalVariants.upmax; index++)
                        cmbModifyIndex.Items.Add(index.ToString());
                }
                else if (GlobalVariants.flag == 2)
                {
                    for (index = 1; index <= GlobalVariants.downmax; index++)
                        cmbModifyIndex.Items.Add(index.ToString());
                }
                else
                {
                    for (index = 1; index <= GlobalVariants.roundmax; index++)
                        cmbModifyIndex.Items.Add(index.ToString());
                }
                cmbModifyIndex.Text = text;//设置cmbModifyIndex的初始值
                if (GlobalVariants.flag != 3)//设置cmbModifyDirection的集合
                {
                    cmbModifyDirection.Items.Clear();
                    cmbModifyDirection.Items.Add("上行");
                    cmbModifyDirection.Items.Add("下行");
                }
                else 
                { 
                    cmbModifyDirection.Items.Clear(); 
                    cmbModifyDirection.Items.Add("回环"); 
                }
                cmbModifyDirection.Text = tabControl2.SelectedTab.Text;//设置cmbModifyDirection的初始值
                cmbModifyDirection.Enabled = true;
                cmbModifyIndex.Enabled = true;
                btnModify.Visible = true;
                btnConfirm.Visible = false;
                btnNewStop.Visible = true;
                focus_Modify(sender, e);
            }
        }
        private void btnReturn_Click(object sender, EventArgs e)
        {
            GlobalVariants.gap1 = true;
            cmbModifyIndex.Items.RemoveAt(cmbModifyIndex.SelectedIndex);
            cmbModifyIndex.Text = (cmbModifyIndex.Items.Count).ToString();
            int direction = 1;
            if (cmbModifyDirection.Text == "上行")
            {
                direction = 1;
            }
            else if (cmbModifyDirection.Text == "下行") { direction = 2; }
            else direction = 0;
            sql = "select stopname from BusLinesInfo where linename='" + cmbModifyLine.Text + "' and LINESTOPINDEX=" + cmbModifyIndex.Text + " and LINEDIRECTION=" + direction + "";
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            //reader.Read();
            while (reader.Read())
            { txtModifyStop.Text = reader["stopname"].ToString(); }
            tabControl1.SelectedTab = tabPage1;
            cmbModifyDirection.Enabled = true;
            cmbModifyIndex.Enabled = true;
            btnModify.Visible = true;
            btnConfirm.Visible = false;
            btnNewStop.Visible = true;
            focus_Line(sender, e);
            btnLine_Click(sender,e);
            GlobalVariants.gap1 = false;
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            cmd = new OleDbCommand(GlobalVariants.builder, GlobalVariants.conn);
            if (MessageBox.Show("你确定要执行吗?", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            else
            {
                if (GlobalVariants.builder != null)
                {
                    cmd.ExecuteNonQuery();
                    btnReset_Click(sender, e);
                    MessageBox.Show("添加线路成功！");
                }
                else 
                {
                    MessageBox.Show("提交出错!");
                }
            }

        }
        private void btnNewStop_Click(object sender, EventArgs e)
        {
            GlobalVariants.gap1 = true;
            txtModifyStop.Text = null;
            cmbModifyIndex.Items.Add((cmbModifyIndex.Items.Count + 1).ToString());
            cmbModifyIndex.Text = (cmbModifyIndex.Items.Count).ToString(); 
            //cmbModifyLine.Enabled = false;
            cmbModifyIndex.Enabled = false;
            cmbModifyDirection.Enabled = false;
            btnConfirm.Visible = true;
            btnModify.Visible = false;
            txtModifyStop.Focus();
            btnNewStop.Visible = false;
            GlobalVariants.gap1 = false;
        }
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            int direction = 0;
            if (cmbModifyDirection.Text == "上行")
            {
                direction = 1;
            }
            else if (cmbModifyDirection.Text == "下行") { direction = 2; }
            else direction = 0;
            if (txtModifyStop.Text == "")
            {
                MessageBox.Show("请输入新站点名称！");
                return; 
            }
            sql = "insert into BusLinesInfo values ('" + cmbModifyLine.Text + "','" + txtModifyStop.Text + "'," + cmbModifyIndex.Text + "," + direction + ")";
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            cmd.ExecuteNonQuery();
            cmbModifyDirection.Enabled = true;
            cmbModifyIndex.Enabled = true;
            btnModify.Visible = true;
            btnConfirm.Visible = false;
            btnNewStop.Visible = true;
            txtModifyStop.Focus();
            MessageBox.Show("新增成功！");
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            GlobalVariants.gap1 = true;
            cmbNewLine.Text = "";
            txtNewStop.Text = "";
            GlobalVariants.builder = null;//builder清空
            cmbNewDirection.Text = "上行";
            cmbNewLine.Enabled = true;
            txtNewIndex.Text = "1";
            Point x;
            x = new Point(469, 273);
            focus_New(sender, e);
            btnRe.Visible = true;
            btnRound.Visible = true;
            btnNew.Location = x;
            GlobalVariants.gap1 = false;
        }
        private void btnRound_Click(object sender, EventArgs e)
        {
            cmbNewDirection.Text = "回环";
            Point x;
            x = new Point(469, 227);
            focus_New(sender, e);
            btnRe.Visible = false;
            btnRound.Visible = false;
            btnNew.Location = x;
        }
        private void btnModify_Click(object sender, EventArgs e)
        {
            int direction = 0;
            if (cmbModifyDirection.Text == "上行")
            {
                direction = 1;
            }
            else if (cmbModifyDirection.Text == "下行") { direction = 2; }
            else direction = 0;
            sql = "update BusLinesInfo set STOPNAME='" + txtModifyStop.Text + "' where LINEDIRECTION=" + direction + " and LINENAME='" + cmbModifyLine.Text + "' and LINESTOPINDEX=" + cmbModifyIndex.Text;
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cmbDeleteLine.Text == "")
            {
                MessageBox.Show("请输入要删除的线路！");
                focus_Delete(sender, e);
                return;
            }//|| lstDeleteLine.Items.ToString() == ""
            if (GlobalVariants.text1 != cmbDeleteLine.Text )
            {
                MessageBox.Show("请输入当前存在的线路！");
                return;
            }
            cmbDeleteLine.Items.Add(cmbDeleteLine.Text);//用来记录历史数据
            sql = "delete from buslinesinfo where linename='" + cmbDeleteLine.Text + "'";
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            if (MessageBox.Show("你确定要执行吗?", "确认", MessageBoxButtons.YesNo) == DialogResult.No)
                return;
            else cmd.ExecuteNonQuery();
            MessageBox.Show("删除成功！");
            cmbDeleteLine.Text = "";
            focus_Delete(sender, e);
            cmd.Dispose();
        }
        
        private string Stop(string linename, int index, string direction)//输入线路、站点索引、方向返回站名
        {
            string stop = null;
            sql = "select stopname from busLinesInfo where linename='" + linename + "' and linestopindex = " + index + " and linedirection = '" + direction + "'";
            OleDbCommand command = new OleDbCommand(sql, GlobalVariants.conn);
            reader = command.ExecuteReader();
            reader.Read();
            stop = reader["stopname"].ToString();
            reader.Close();
            command.Dispose();
            return stop;
        }
        private void lst123_DoubleClick(object sender, EventArgs e)//lst表中双击转到对应的标签中查询相关信息
        {
            if ((sender as ListBox).SelectedIndex < 0)
                return;
            tabControl1.SelectedTab = tabPage2;
            cmbStop.Text = ((sender) as ListBox).SelectedItem.ToString();
            if (cmbStop.Text.StartsWith("("))//去除（数字）来查找站点
            {
                cmbStop.Text=cmbStop.Text.Remove(0, 3);
                if(cmbStop.Text.StartsWith(")"))
                    cmbStop.Text=cmbStop.Text.Remove(0,1);
            }
            btnStop_Click(sender, e);
            focus_Stop(sender, e);
        }
        private void lst4_DoubleClick(object sender, EventArgs e)
        {
            if (lst4.SelectedIndex < 0)//防止在表4中双击空白处崩溃
                return;
            tabControl1.SelectedTab = tabPage1;//转到标签1
            cmbLine.Text = lst4.SelectedItem.ToString();//4中选定的值附给标签1中的text
            btnLine_Click(sender, e);//自动完成点击任务（函数）
            focus_Line(sender, e);
        }

        private void tabControl1_Click(object sender, EventArgs e)//转换tab标签时fous转换
        {
            if (this.tabControl1.SelectedTab == this.tabPage1)
            {
                focus_Line(sender, e);
            }
            if (this.tabControl1.SelectedTab == this.tabPage2)
            {
                focus_Stop(sender, e);
            }
            if (this.tabControl1.SelectedTab == this.tabPage3)
            {
                focus_Start(sender, e);
                //lstExchange.Visible = false;
            }
            if (this.tabControl1.SelectedTab == this.tabPage4)
            {
                if (GlobalVariants.gap2 == false)
                {
                    tabControl3.SelectedTab = tabPage5;
                }
                focus_New(sender, e);
                cmbNewDirection.Text = "上行";
            }
        }
        private void tabControl3_Click(object sender, EventArgs e)//转换tab标签时fous转换
        {
            if (this.tabControl3.SelectedTab == this.tabPage5)
            {
                focus_New(sender, e);
            }
            if (this.tabControl3.SelectedTab == this.tabPage6)
            {
                if (GlobalVariants.gap2 == false)
                {
                    MessageBox.Show("请在查询线路中先进行数据初始化！");
                    this.tabControl1.SelectedTab = tabPage1;
                    focus_Line(sender, e);
                    
                }else focus_Modify(sender, e);
            }
            if (this.tabControl3.SelectedTab == this.tabPage7)
            {
                focus_Delete(sender, e);
            }
        }
        private void focus_Line(object sender, EventArgs e)
        {
            cmbLine.Focus();//第一个要选的cmb自动点击
            cmbLine.SelectAll();//若对应cmb中有数据则全选方便修改
            this.AcceptButton = btnLine;//修改回车键对应的按钮
        }

        private void focus_Stop(object sender, EventArgs e)
        {
            cmbStop.Focus();
            cmbStop.SelectAll();
            this.AcceptButton = btnStop;
        }

        private void focus_Start(object sender, EventArgs e)
        {
            cmbStart.Focus();
            cmbStart.SelectAll();
            this.AcceptButton = btnExchange;
            lstEnd.Visible = false;
            cmbStart_Click(sender, e);
        }
        private void focus_Modify(object sender, EventArgs e)
        {
            txtModifyStop.Focus();
            txtModifyStop.SelectAll();
            this.AcceptButton = btnModify;
        }
        private void focus_Delete(object sender, EventArgs e)
        {
            cmbDeleteLine.Focus();
            cmbDeleteLine.SelectAll();
            this.AcceptButton = btnDelete;
        }
        private void focus_End(object sender, EventArgs e)
        {
            cmbEnd.Focus();
            cmbEnd.SelectAll();
            this.AcceptButton = btnExchange;
            lstStart.Visible = false;
            cmbEnd_Click(sender, e);
        }
        private void focus_New(object sender, EventArgs e)
        {
            lstNewLine.Visible = false;
            //cmbNewDirection.Text = "上行";
            cmbNewLine.Focus();
            //txtNewLine.SelectAll();
            this.AcceptButton = btnNext;
        }
        private void cmbLine_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            lstLine.Visible = true;
            sql = @"select distinct top 50 linename from buslinesinfo where linename like'" + cmbLine.Text + "%' order by linename";
            lstLine.Items.Clear();
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstLine.Items.Add(reader["linename"].ToString());
                if (i == 0)
                    GlobalVariants.text1 = reader["linename"].ToString();
                i++;
            }
            reader.Close();
            cmd.Dispose();
            if (i > 8) i = 8;//设置lst按可能的结果查询后自适应长度
            lstLine.Size = new Size(142, 4 + i * 18);
            this.AcceptButton = btnLine;
        }
        private void cmbStop_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            lstStop.Visible = true;sql = @"select distinct top 50 stopname from buslinesinfo where stopname like'" + cmbStop.Text + "%' order by stopname";
            lstStop.Items.Clear();
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstStop.Items.Add(reader["stopname"].ToString());
                if (i == 0)
                    GlobalVariants.text1 = reader["stopname"].ToString();
                i++;
            }
            reader.Close();
            cmd.Dispose();
            if (i > 8) i = 8;
            lstStop.Size = new Size(249, 4 + i * 18);
            this.AcceptButton = btnStop;
        }
        private void cmbStart_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            lstStart.Visible = true;
            sql = @"select distinct top 50 stopname from buslinesinfo where stopname like'" + cmbStart.Text + "%' order by stopname";
            lstStart.Items.Clear();
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstStart.Items.Add(reader["stopname"].ToString());
                if (i == 0)
                    GlobalVariants.text1 = reader["stopname"].ToString();
                i++;
            }
            reader.Close();
            cmd.Dispose();
            if (i > 7) i = 7;
            lstStart.Size = new Size(222, 4 + i * 18);
            this.AcceptButton = btnExchange;
        }

        private void cmbEnd_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            lstEnd.Visible = true;
            sql = @"select distinct top 50 stopname from buslinesinfo where stopname like'" + cmbEnd.Text + "%' order by stopname";
            lstEnd.Items.Clear();
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstEnd.Items.Add(reader["stopname"].ToString());
                if (i == 0)
                    GlobalVariants.text2 = reader["stopname"].ToString();
                i++;
            }
            reader.Close();
            cmd.Dispose();
            if (i > 7) i = 7;
            lstEnd.Size = new Size(232, 4 + i * 18);
        }
        private void cmbNewLine_TextChanged(object sender, EventArgs e)
        {
            int i = 0; GlobalVariants.text1 = null;
            sql = @"select distinct top 10 linename from buslinesinfo where linename like'" + cmbNewLine.Text + "%' order by linename";
            lstNewLine.Items.Clear();
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstNewLine.Items.Add(reader["linename"].ToString());
                if (i == 0)
                    GlobalVariants.text1 = reader["linename"].ToString();
                i++;
            }
            reader.Close();
            cmd.Dispose();
            if (i > 8) i = 8;//设置lst按可能的结果查询后自适应长度
            lstNewLine.Size = new Size(120, 4 + i * 18);
            lstNewLine.Visible = true;
        }
        private void cmbModifyLine_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            //lstModify.Visible = true;
            sql = @"select distinct top 50 linename from buslinesinfo where linename like'" + cmbModifyLine.Text + "%' order by linename";
            lstModifyLine.Items.Clear();
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstModifyLine.Items.Add(reader["linename"].ToString());
                if (i == 0)
                    GlobalVariants.text1 = reader["linename"].ToString();
                i++;
            }
            reader.Close();
            cmd.Dispose();
            if (i > 8) i = 8;//设置lst按可能的结果查询后自适应长度
            lstModifyLine.Size = new Size(120, 4 + i * 18);
            this.AcceptButton = btnModify;
        }
        private void cmbDeleteLine_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            lstDeleteLine.Visible = true;
            sql = @"select distinct top 50 linename from buslinesinfo where linename like'" + cmbDeleteLine.Text + "%' order by linename";
            lstDeleteLine.Items.Clear();
            cmd = new OleDbCommand(sql, GlobalVariants.conn);
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                lstDeleteLine.Items.Add(reader["linename"].ToString());
                if (i == 0)
                    GlobalVariants.text1 = reader["linename"].ToString();
                i++;
            }
            reader.Close();
            cmd.Dispose();
            if (i > 8) i = 8;//设置lst按可能的结果查询后自适应长度
            lstDeleteLine.Size = new Size(120, 4 + i * 18);
            this.AcceptButton = btnDelete;
        }
        private void txtNewStop_TextChanged(object sender, EventArgs e)
        {
            if (GlobalVariants.gap1 == false)
            {
                if (GlobalVariants.text1 == cmbNewLine.Text)
                {
                    MessageBox.Show("线路已存在!");
                    cmbNewLine.Text = "";
                    cmbNewLine.Focus();
                }
                else if (cmbNewLine.Text == "")
                {
                    MessageBox.Show("请输入要新增的线路名称！");
                    cmbNewLine.Text = "";
                    cmbNewLine.Focus();
                }
            }
        }
        private void cmbModifyIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalVariants.gap1 == false) 
            {
                int direction = 0;
                if (cmbModifyDirection.Text == "上行")
                {
                    direction = 1;
                }
                else if (cmbModifyDirection.Text == "下行") { direction = 2; }
                else direction = 0;
                sql = "select STOPNAME from buslinesinfo where LINEDIRECTION =" + direction + " and LINENAME='" + cmbModifyLine.Text + "' and LINESTOPINDEX=" + cmbModifyIndex.Text;
                cmd = new OleDbCommand(sql, GlobalVariants.conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    txtModifyStop.Text = reader["stopname"].ToString();
                }
                cmd.Dispose();
                reader.Close();
                GlobalVariants.gap1 = true;
            }
        }
        private void cmbModifyDirection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalVariants.gap1 == false)//防止因为index没有数据使得btnUpdateLine报错
            {
                int direction = 0;
                int temp = 0;
                if (cmbModifyDirection.Text == "上行")
                {
                    direction = 1;
                }
                else if (cmbModifyDirection.Text == "下行") { direction = 2; }
                else direction = 0;//direction赋值
                temp = cmbModifyIndex.SelectedIndex;
                cmbModifyIndex.Items.Clear();
                sql = "select linestopindex from buslinesinfo where LINENAME='" + cmbModifyLine.Text + "' and LINEDIRECTION =" + direction + " order by LINESTOPINDEX";
                cmd = new OleDbCommand(sql, GlobalVariants.conn);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    cmbModifyIndex.Items.Add(reader["linestopindex"].ToString());
                }
                if (temp>cmbModifyIndex.Items.Count-1)
                {
                temp = 0;//防止由于更改时溢出报错故不设为原值
                }
                cmbModifyIndex.SelectedIndex = temp;
                sql = "select STOPNAME from buslinesinfo where LINEDIRECTION =" + direction + " and LINENAME='" + cmbModifyLine.Text + "' and LINESTOPINDEX=" + cmbModifyIndex.Text;
                cmd = new OleDbCommand(sql, GlobalVariants.conn);//ModifyStop赋值
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    txtModifyStop.Text = reader["stopname"].ToString();
                }
                cmd.Dispose();
                reader.Close();
                GlobalVariants.gap1 = true;
            }
        }
        private void lstLine_Click(object sender, EventArgs e)
        {
            cmbLine.Text = ((sender) as ListBox).SelectedItem.ToString();
            focus_Line(sender, e);
            lstLine.Visible = false;
            btnLine_Click(sender, e);
        }

        private void lstStop_Click(object sender, EventArgs e)
        {
            cmbStop.Text = ((sender) as ListBox).SelectedItem.ToString();
            focus_Stop(sender, e);
            lstStop.Visible = false;
            btnStop_Click(sender, e);
        }

        private void lstStart_Click(object sender, EventArgs e)
        {
            cmbStart.Text = ((sender) as ListBox).SelectedItem.ToString();
            btnExchange_Click(sender, e);
            focus_End(sender, e);
            lstStart.Visible = false;
        }

        private void lstEnd_Click(object sender, EventArgs e)
        {
            cmbEnd.Text = ((sender) as ListBox).SelectedItem.ToString();
            btnExchange_Click(sender, e);
            focus_End(sender, e);
            lstEnd.Visible = false;
        }
        private void lstNewLine_Click(object sender, EventArgs e)
        {
            cmbNewLine.Text = ((sender) as ListBox).SelectedItem.ToString();
            btnNew_Click(sender, e);
            focus_New(sender, e);
            lstNewLine.Visible = false;
        }
        private void lstModifyLine_Click(object sender, EventArgs e)
        {
            cmbModifyLine.Text = ((sender) as ListBox).SelectedItem.ToString();
            btnModify_Click(sender, e);
            focus_Modify(sender, e);
            lstModifyLine.Visible = false;
        }
        private void lstDeleteLine_Click(object sender, EventArgs e)
        {
            cmbDeleteLine.Text = ((sender) as ListBox).SelectedItem.ToString();
            btnDelete_Click(sender, e);
            focus_Delete(sender, e);
            lstDeleteLine.Visible = false;
        }
        private void cmbStart_Click(object sender, EventArgs e)
        {
            if (cmbStart.Text != "")
            {
                lstStart.Visible = true;
                cmbStart_TextChanged(sender, e);
            }
            lstEnd.Visible = false;
            this.AcceptButton = btnExchange;
        }

        private void cmbEnd_Click(object sender, EventArgs e)
        {
            if (cmbEnd.Text != "")
            {
                lstEnd.Visible = true;
                cmbEnd_TextChanged(sender, e);
            }
            lstStart.Visible = false;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            txtNewStop.Focus();
            txtNewStop.SelectAll();
        }
        private void cmbLine_DropDown(object sender, EventArgs e)
        {
            lstLine.Visible = false;
        }
        private void cmbStop_DropDown(object sender, EventArgs e)
        {
            lstStop.Visible = false;
        }
        private void cmbStart_DropDown(object sender, EventArgs e)
        {
            lstStart.Visible = false;
        }
        private void cmbEnd_DropDown(object sender, EventArgs e)
        {
            lstEnd.Visible = false;
        }
        private void cmbNewLine_DropDown(object sender, EventArgs e)
        {
            lstNewLine.Visible = false;
        }
        private void cmbModifyLine_DropDown(object sender, EventArgs e)
        {
            lstModifyLine.Visible = false;
        }
        private void cmbDelete_DropDown(object sender, EventArgs e)
        {
            lstDeleteLine.Visible = false;
        }
        private void cmbModifyIndex_DropDown(object sender, EventArgs e)//对修改中的两个下拉框极其重要
        {
            GlobalVariants.gap1 = false;
        }

        private void cmbModifyDirection_DropDown(object sender, EventArgs e)//对修改中的两个下拉框极其重要
        {
            GlobalVariants.gap1 = false;
        }
        private void cmbLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstLine.SelectedIndex < lstLine.Items.Count - 1)
            {
                lstLine.SelectedIndex += 1;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up && lstLine.SelectedIndex > 0)
            {
                lstLine.SelectedIndex -= 1;
                e.Handled = true;
            }

        }
        private void cmbStop_KeyDown(object sStoper, KeyEventArgs e)
        
        {
            if (e.KeyCode == Keys.Down && lstStop.SelectedIndex < lstStop.Items.Count - 1)
            {
                lstStop.SelectedIndex += 1;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up && lstStop.SelectedIndex > 0)
            {
                lstStop.SelectedIndex -= 1;
                e.Handled = true;
            }
        }
        private void cmbStart_KeyDown(object sStarter, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstStart.SelectedIndex < lstStart.Items.Count - 1)
            {
                lstStart.SelectedIndex += 1;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up && lstStart.SelectedIndex > 0)
            {
                lstStart.SelectedIndex -= 1;
                e.Handled = true;
            }
            //if (e.KeyCode == Keys.Back)
            //{
            //    return;
            //}
        }
        private void cmbEnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstEnd.SelectedIndex < lstEnd.Items.Count - 1)
            {
                lstEnd.SelectedIndex += 1;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up && lstEnd.SelectedIndex > 0)
            {
                lstEnd.SelectedIndex -= 1;
                e.Handled = true;
            }
        }

        private void cmbModifyLine_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstModifyLine.SelectedIndex < lstModifyLine.Items.Count - 1)
            {
                lstModifyLine.SelectedIndex += 1;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up && lstModifyLine.SelectedIndex > 0)
            {
                lstModifyLine.SelectedIndex -= 1;
                e.Handled = true;
            }
        }

        private void cmbDelete_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down && lstDeleteLine.SelectedIndex < lstDeleteLine.Items.Count - 1)
            {
                lstDeleteLine.SelectedIndex += 1;
                e.Handled = true;
            }
            if (e.KeyCode == Keys.Up && lstDeleteLine.SelectedIndex > 0)
            {
                lstDeleteLine.SelectedIndex -= 1;
                e.Handled = true;
            }
        }













    }
}
