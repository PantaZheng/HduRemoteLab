using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows;

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
    public class IdNewpwd
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
        public string function_id { get; set; }
        public string starting_address { get; set; }
        public string quantity_x { get; set; }
        public string output_value { get; set; }

    }
    //log交互
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
    //返回信息
    public class MesData
    {
        public string code { get; set; }
        public JObject mes { get; set; }
    }
    public class BackMes
    {
        public string message { get; set; }
    }
}
