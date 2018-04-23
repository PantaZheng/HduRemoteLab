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
    
    /********登录改密格式**************
    function：登录或修改密码所用
    id：账户id
    password：用户密码
    */
    public class IdPwd
    {
        public string id { get; set; }
        public string password { get; set; }
    }
    /********返回账户信息格式**************
    function：登录后返回账户信息所用
    id：账户id
    password：用户密码
    name：用户姓名
    role：用户角色
    */
    public class Account
    {
        public string id { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string role { get; set; }
    }
    /****************获取信息格式*************
    function：向数据库获取信息
    flag：定义所取信息种类
    id：用于记录日志的用户id
    */
    public class Info
    {
        public string flag { get; set; }
        public string id { get; set; }
    }
    /****************实验信息*************
    function：取出实验信息
    name: 实验名字
    */
    public class Experiments
    {
        public string name { get; set; }
    }
    /****************控制器信息*************
    function：取出控制器信息
    name: 控制器名字
    id：控制器编号
    kind：控制器种类
    experiments：控制器所包含的实验
    state：状态
    */
    public class Slaves
    {
        public string name { get; set; }
        public int id { get; set; }
        public string kind { get; set; }
        public JArray experiments { get; set; }
        public string state { get; set; }
    }
    /****************日志信息*************
    function：取出日志信息
    time：事件戳
    data：信息域
    */
    public class Log
    {
        public string time { get; set; }
        public string data { get; set; }
    }
    /***************操控格式*****************/
    /****************文件信息*************
    function：上传或下载的文件信息
    name：名字
    content：内容
    */
    public class Docement
    {
        public string name;
        public string content;
    }
    /****************从机控制器信息*************
    function:用来上传选定从机控制器信息
    id：编号
    kind：种类
    name：名字
    state：状态
    */
    public class Slave
    {
        public int id;
        public string kind;
        public string name;
        public string state;
    }
    /****************文件上传信息*************
      function:封装文件上传信息
      flag：路径标记
      id：账户编号
      slave：需要上传到的从机信息
      docement：文件信息
      */
    public class UploadData
    {
        public string flag = "upload";
        public string id { get; set; }
        public Slave slave { get; set; }
        public Docement docement { get; set; }
    }
    /****************文件下载信息*************
      function:封装文件上传信息
      flag：路径标记
      id：账户编号
      slave：需要上传到的从机信息
      docement：文件信息
      */
    public class DownLoadData
    {
        public string flag = "download";
        public string id { get; set; }
        public Slave slave { get; set; }
        public string experiment { get; set; }
    }
    /****************实验启动信息*************
     function:封装实验启动信息
     flag：路径标记
     id：账户编号
     slave：需要上传到的从机信息
     experiment：实验信息
     */
    public class StartData
    {
        public string flag = "start";
        public string id { get; set; }
        public Slave slave { get; set; }
        public string experiment { get; set; }
    }
    /****************Mobuds信息*************
     function:封装modbus信息
     function_code：功能码
     starting_address：起始位
     quantity_of_x:数量
     */
    public class ModbusMes
    {
        public int function_code { get; set; }
        public int starting_address { get; set; }
        public int quantity_of_x { get; set; }
    }
    /****************Modbus操作信息*************
     function:封装modbus操作信息
     flag：路径标记
     id：账户编号
     slave：需要上传到的从机信息
     modbus：modbus信息
     experiment：实验信息
     */
    public class ModbusData
    {
        public string flag = "modbus";
        public string id { get; set; }
        public Slave slave { get; set; }
        public ModbusMes modbus { get; set; }
        public string experiment { get; set; }
    }
    /****************实验停止信息*************
      function:封装实验停止信息
      flag：路径标记
      id：账户编号
      slave：需要上传到的从机信息
      experiment：实验信息
     */
    public class StopData
    {
        public string flag = "stop";
        public string id { get; set; }
        public Slave slave { get; set; }
        public string experiment { get; set; }
    }
    /******************basic格式*************************/
    /****************基础回传信息*************
    function:解析回传信息
    code：代码
    mes：消息
    data：数据
    */
    public class BasicData
    {
        public string code { get; set; }
        public string mes { get; set; }
        public string data { get; set; }
    }
    /****************账户回传信息*************
    function:解析回传信息
    code：代码
    mes：消息
    data：数据,格式为待解析的json数据
    */
    public class AccountData
    {
        public string code { get; set; }
        public string mes { get; set; }
        public JObject data { get; set; }
    }
    /****************实验回传信息*************
    function:解析回传信息
    code：代码
    mes：消息
    data：数据，格式为带解析的jarry信息
    */
    public class SlavesData
    {
        public string code { get; set; }
        public string mes { get; set; }
        public JArray data { get; set; }
    }
    /****************功能码映射信息*************
    function:解析功能码信息，以让前台显示
    function：功能名称
    id：代号
    */
    public class Funcnction_id
    {
        public string function { get; set; }
        public int id { get; set; }
    }
}
