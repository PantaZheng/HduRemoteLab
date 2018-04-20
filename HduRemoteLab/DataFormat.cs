using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Collections.Generic;
using Newtonsoft.Json.Bson;


namespace HduRemoteLab
{
    /// <summary>
    /// 交互信息格式定义
    /// </summary>
    /// 
    /*************登录改密格式**************/
    //登录或修改密码
    public class IdPwd
    {
        public string id { get; set; }
        public string password { get; set; }
    }
    //返回账户信息
    public class Account
    {
        public string id { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }
    /****************获取信息格式*************/
    //获取信息
    public class Info
    {
        public string flag { get; set; }
        public string id { get; set; }
    }
    public class Experiments
    {
        public string name { get; set; }
    }

    //设备信息
    public class Slaves
    {
        public string name { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public JArray experiments { get; set; }
        public string state { get; set; }
    }
    
    //返回日志信息
    public class Log
    {
        public string time { get; set; }
        public string data { get; set; }
    }

    /***************操控格式*****************/

    public class Docement
    {
        public string name;
        public string content;
    }

    public class Slave
    {
        public int id;
        public string kind;
        public string name;
        public string state;
    }
    //上传文件
    public class UploadData
    {
        public string flag = "upload";
        public string id { get; set; }
        public Slave slave { get; set; }
        public Docement docement { get; set; }
    }
    //下载文件
    public class DownLoadData
    {
        public string flag = "download";
        public string id { get; set; }
        public Slave slave { get; set; }
        public string experiment { get; set; }
    }
    //远程控制开始
    public class StartData
    {
        public string flag = "start";
        public string id { get; set; }
        public Slave slave { get; set; }
        public string experiment { get; set; }
    }
    public class ModbusMes
    {
        public int function_code { get; set; }
        public int starting_address { get; set; }
        public int quantity_x { get; set; }
    }

    //远程控制Modbus指令
    public class ModbusData
    {
        public string flag = "modbus";
        public string id { get; set; }
        public Slave slave { get; set; }
        public ModbusMes modbus { get; set; }
    }
   //远程控制停止
    public class StopData
    {
        public string flag = "stop";
        public string id { get; set; }
        public Slave slave { get; set; }
        public string experiment { get; set; }
    }

    /******************basic格式*************************/


    //返回信息
    public class BasicData
    {
        public string code { get; set; }
        public string mes { get; set; }
        public string data { get; set; }
    }
    public class AccountData
    {
        public string code { get; set; }
        public string mes { get; set; }
        public JObject data { get; set; }
    }
    public class SlavesData
    {
        public string code { get; set; }
        public string mes { get; set; }
        public JArray data { get; set; }
    }
}
