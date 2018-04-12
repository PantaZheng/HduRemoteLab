using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;
using System.Collections.Generic;

namespace HduRemoteLab
{
    /// <summary>
    /// 交互信息格式定义
    /// </summary>
    //登录信息
    public class IdPwd
    {
        public string id { get; set; }
        public string password { get; set; }
    }
    public class IdNewPwd
    {
        public string id { get; set; }
        public string new_password { get; set; }
    }
    //账户信息
    public class Account
    {
        public string log_id { get; set; }
        public string id { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }
    //modbus交互
    public class Modbus
    {
        public string slave { get; set; }
        public string function_code { get; set; }
        public string starting_address { get; set; }
        public string quantity_x { get; set; }
        public string output_value { get; set; }
    }
    //日志获取
    public class LogDeviceModbus
    {
        public string log_id { get; set; }
        public Modbus modbus { get; set; }
    }
    public class LogDeviceDocement
    {
        public string log_id { get; set; }
        public string device { get; set; }
        public string docement_name { get; set; }
    }

    //控制器信息
    public class Slave
    {
        public string id { get; set; }
        public string name { get; set; }
        public string state { get; set; }
        public List<string> experiments { get; set; }
    }
    public class ExperimentData
    {
        public List<Slave> slaves { get; set; }
    }
    //远程控制信息
    public class OperateData
    {
        public string log_id { get; set; }
        public string slave_anme { get; set; }
        public string expriment_name { get; set; }
        public Modbus modbus;
    }
    
    public class FlagData
    {
        public string flag { get; set; }
        public JObject data { get; set; }
    }

    //返回信息
    public class MesData
    {
        public string code { get; set; }
        public JObject message { get; set; }
    }
    public class BackMes
    {
        public string message { get; set; }
    }
}
