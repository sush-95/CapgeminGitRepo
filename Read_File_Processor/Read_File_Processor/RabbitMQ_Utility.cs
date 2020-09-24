
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess_Utility;
using System.Configuration;
using MessageService;

namespace Read_File_Processor
{
    public class RabbitMQ_Utility
    {
        public const string RabbitMQUrl = "localhost";
        public const string RabbitMQRequestQueue = "Request";
        public const string RabbitMQResponseQueue = "Response";
        public const string ServerQueue = "serverqueue";

        public bool Rabbit_Send(string message, string queue, string hostname, out string error)
        {
            error = string.Empty;
            try
            {
                string strRabbitMQUrl = "";
                string strRabbitMQRequestQueue = "";
                string strRabbitMQResponseQueue = "";
                // Get HostName //
                Get_Data_Utility objGet = new Get_Data_Utility();
                List<tbl_config_value> lstConfig = objGet.Get_Cofig_Details("RABITMQ");
                foreach (var ob in lstConfig)
                {
                    strRabbitMQUrl = ob.configstring;
                    strRabbitMQRequestQueue = ob.RequestQueue;
                    strRabbitMQResponseQueue = ob.ResponseQueue;
                }
                if (!string.IsNullOrEmpty(strRabbitMQUrl))
                {
                    //queue = queue.ToLower() == "response" ? strRabbitMQResponseQueue : strRabbitMQRequestQueue;
                    queue = strRabbitMQRequestQueue;// strRabbitMQRequestQueue;
                    hostname = strRabbitMQUrl;
                    var factory = new ConnectionFactory() { HostName = hostname };
                    string UserName = ConfigurationManager.AppSettings["r_username"];
                    string Password = ConfigurationManager.AppSettings["r_password"];
                    factory.UserName = UserName;
                    factory.Password = Password;
                    
                    using (var connection = factory.CreateConnection())

                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                //return false;
                throw ex;
            }

        }
        public bool ServerRabbit_Send(string message, string queue, string hostname)
        {
            string jason_data_RAbbitMQ = "";
            string MachinePath = ConfigurationManager.AppSettings["ServerQueueURL"];
            string RequestQueueName = ConfigurationManager.AppSettings["ServerQueue"];
            string userName = ConfigurationManager.AppSettings["r_username"];
            string Password = ConfigurationManager.AppSettings["r_password"];
            jason_data_RAbbitMQ = message;
            MessageProducer msgProducer1 = new MessageProducer(MachinePath, userName, Password);
            msgProducer1.declareQueue(RequestQueueName);
            msgProducer1.SendMessage(jason_data_RAbbitMQ, RequestQueueName);
            //return jason_data_RAbbitMQ;
            return true;
        }
        //public bool ServerRabbit_Send1(string message, string queue, string hostname, out string error)
        //{
        //    error = string.Empty;
        //    try
        //    {
        //        string strRabbitMQUrl = "";
        //        string strRabbitMQRequestQueue = "";
        //        string strRabbitMQResponseQueue = "";
        //        // Get HostName //
        //        Get_Data_Utility objGet = new Get_Data_Utility();
        //        List<tbl_config_value> lstConfig = objGet.Get_Cofig_Details("RABITMQ");
        //        foreach (var ob in lstConfig)
        //        {
        //            strRabbitMQUrl = ob.configstring;
        //            strRabbitMQRequestQueue = ob.RequestQueue;
        //            strRabbitMQResponseQueue = ob.ResponseQueue;
        //        }
        //        if (!string.IsNullOrEmpty(strRabbitMQUrl))
        //        {
        //            //queue = queue.ToLower() == "response" ? strRabbitMQResponseQueue : strRabbitMQRequestQueue;
        //            strRabbitMQUrl = ConfigurationManager.AppSettings["ServerQueueURL"];
        //            queue = ConfigurationManager.AppSettings["ServerQueue"];// strRabbitMQRequestQueue;
        //            hostname = strRabbitMQUrl;
        //            var factory = new ConnectionFactory() { HostName = hostname };
        //            string UserName = ConfigurationManager.AppSettings["r_username"];
        //            string Password = ConfigurationManager.AppSettings["r_password"];
        //            factory.UserName = UserName;
        //            factory.Password = Password;

        //            using (var connection = factory.CreateConnection())

        //            using (var channel = connection.CreateModel())
        //            {
        //                //channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
        //                var body = Encoding.UTF8.GetBytes(message);
        //                channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
        //            }
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message.ToString();
        //        //return false;
        //        throw ex;
        //    }

        //}

        public bool Rabbit_Send_Response_Queue(string message, string queue, string hostname, out string error)
        {
            error = string.Empty;
            try
            {
                string strRabbitMQUrl = "";
                string strRabbitMQRequestQueue = "";
                string strRabbitMQResponseQueue = "";
                // Get HostName //
                Get_Data_Utility objGet = new Get_Data_Utility();
                List<tbl_config_value> lstConfig = objGet.Get_Cofig_Details("RABITMQ");
                foreach (var ob in lstConfig)
                {
                    strRabbitMQUrl = ob.configstring;
                    strRabbitMQRequestQueue = ob.RequestQueue;
                    strRabbitMQResponseQueue = ob.ResponseQueue;
                }
                if (!string.IsNullOrEmpty(strRabbitMQUrl))
                {
                    //queue = queue.ToLower() == "response" ? strRabbitMQResponseQueue : strRabbitMQRequestQueue;
                    queue = strRabbitMQResponseQueue;// strRabbitMQRequestQueue;
                    hostname = strRabbitMQUrl;
                    var factory = new ConnectionFactory() { HostName = hostname };
                    using (var connection = factory.CreateConnection())

                    using (var channel = connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                        var body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                //return false;
                throw ex;
            }

        }

        public bool Receive1(string queue, out string data, out string error)
        {
            error = string.Empty;
            data = string.Empty;
            try
            {
                string strRabbitMQUrl = "";
                string strRabbitMQRequestQueue = "";
                string strRabbitMQResponseQueue = "";
                // Get HostName //
                Get_Data_Utility objGet = new Get_Data_Utility();
                List<tbl_config_value> lstConfig = objGet.Get_Cofig_Details("RABITMQ");
                foreach (var ob in lstConfig)
                {
                    strRabbitMQUrl = ob.configstring;
                    strRabbitMQRequestQueue = ob.RequestQueue;
                    strRabbitMQResponseQueue = ob.ResponseQueue;
                }
                if (!string.IsNullOrEmpty(strRabbitMQUrl))
                {
                    queue = strRabbitMQResponseQueue;
                    using (IConnection connection = new ConnectionFactory().CreateConnection())
                    {
                        using (IModel channel = connection.CreateModel())
                        {
                            channel.QueueDeclare(queue, true, false, false, null);
                            var consumer = new EventingBasicConsumer(channel);
                            BasicGetResult result = channel.BasicGet(queue, true);
                            if (result != null)
                            {
                                data =
                                Encoding.UTF8.GetString(result.Body);
                                Console.WriteLine(data);
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                error = ex.Message.ToString();
                throw ex;
                //return false;
            }
        }
        public bool Receive(string queue, out string data, out string error)
        {
            error = string.Empty;
            try
            {
                string strRabbitMQUrl = "";
                string strRabbitMQRequestQueue = "";
                string strRabbitMQResponseQueue = "";
                data = string.Empty;
                // Get HostName //
                Get_Data_Utility objGet = new Get_Data_Utility();
                DML_Utility objDML = new DML_Utility();

                List<tbl_config_value> lstConfig = objGet.Get_Cofig_Details("RABITMQ");
                foreach (var ob in lstConfig)
                {
                    strRabbitMQUrl = ob.configstring;
                    strRabbitMQRequestQueue = ob.RequestQueue;
                    strRabbitMQResponseQueue = ob.ResponseQueue;
                }
                if (!string.IsNullOrEmpty(strRabbitMQUrl))
                {
                    //queue = queue.ToLower() == "response" ? strRabbitMQResponseQueue : strRabbitMQRequestQueue;
                    queue = strRabbitMQResponseQueue;// strRabbitMQRequestQueue;
                    //hostname = strRabbitMQUrl;
                    var factory = new ConnectionFactory() { HostName = strRabbitMQUrl };
                    string UserName = ConfigurationManager.AppSettings["r_username"];
                    string Password = ConfigurationManager.AppSettings["r_password"];
                    factory.UserName = UserName;
                    factory.Password = Password;
                    //objDML.Add_Exception_Log(strRabbitMQUrl, "");

                    using (var connection = factory.CreateConnection())

                    using (var channel = connection.CreateModel())
                    {
                        //channel.QueueDeclare(queue: queue, durable: true, exclusive: false, autoDelete: false, arguments: null);
                        //var body = Encoding.UTF8.GetBytes(message);
                        //channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: body);
                       // objDML.Add_Exception_Log(queue, "");
                        channel.QueueDeclare(queue, true, false, false, null);
                        var consumer = new EventingBasicConsumer(channel);
                        BasicGetResult result = channel.BasicGet(queue, true);
                       // objDML.Add_Exception_Log(result.Body.ToString(), "");

                        if (result != null)
                        {
                            data =
                            Encoding.UTF8.GetString(result.Body);
                            //Console.WriteLine(data);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message.ToString();
                //return false;
                throw ex;
            }
        }

    }
}
