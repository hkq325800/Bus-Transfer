using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;

namespace busSearch
{
    class GlobalVariants
    {
        //public static int stopcount;
        public static bool examOne = false;//检测是否有直达线路
        public static bool examTwo = false;//检测是否有一次换乘
        public static bool examThree = false;//检测是否有二次换乘
        public static OleDbConnection conn;//全局的连接
        public static string text1 = null;//用来确定按下确认按钮时cmb和lst中的第一个是否匹配
        public static string text2 = null;//用来确定按下确认按钮时cmb和lst中的第一个是否匹配
        public static int upmax = 0,downmax=0,roundmax=0;//用来记录上下行 回环的站点数目
        public static int flag = 1;//用来判断修改信息来自上下行还是回环
        public static bool gap1 = false, gap2 = false;//判断是否进行修改信息的selectchange gap2在修改线路选项卡初始化后始终为true gap1负责indexchangged的开关
        public static string builder = null;
        public static int clear1 = 0, clear2 = 0, clear3 = 0, clear4 = 0, clear5 = 0;//1.Line 2.Stop 3.Start 4.End 5.ModifyLine 6.Delete
    }
}
