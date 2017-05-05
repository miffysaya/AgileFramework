using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AgileFramework
{
    /// <summary>
    /// Windows系统服务操作帮助类
    /// </summary>
    public static class AgileWindowsServices
    {
        /// <summary>
        /// 操作类型枚举
        /// </summary>
        private enum AgileServiceOperationType
        {
            /// <summary>
            /// 开始
            /// </summary>
            Start,

            /// <summary>
            /// 停止
            /// </summary>
            Stop,

            /// <summary>
            /// 继续
            /// </summary>
            Continue,

            /// <summary>
            /// 暂停
            /// </summary>
            Pause
        }

        /// <summary>
        /// 获取当前计算机的所有服务
        /// </summary>
        public static List<ServiceController> AllServices
        {
            get
            {
                return new List<ServiceController>(ServiceController.GetServices());
            }
        }

        /// <summary>
        /// 获取当前计算机所有服务的名称
        /// </summary>
        public static List<string> AllServicesName
        {
            get
            {
                var linq = from service in AllServices select service.ServiceName;

                return linq.ToList();
            }
        }

        /// <summary>
        /// 更改服务的操作状态
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <param name="operationType">操作类型</param>
        private static void ChangeState(string serviceName, AgileServiceOperationType operationType)
        {
            foreach (var service in AllServices)
            {
                if (service.ServiceName.ToLower() == serviceName.ToLower())
                {
                    try
                    {
                        switch (operationType)
                        {
                            case AgileServiceOperationType.Start:
                                service.Start();
                                break;
                            case AgileServiceOperationType.Stop:
                                service.Stop();
                                break;
                            case AgileServiceOperationType.Continue:
                                service.Continue();
                                break;
                            case AgileServiceOperationType.Pause:
                                service.Pause();
                                break;
                        }
                    }
                    catch
                    {

                    }
                    break;
                }
            }
        }

        /// <summary>
        /// 启动服务（被禁止的服务无法启动）
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Start(string serviceName)
        {
            ChangeState(serviceName, AgileServiceOperationType.Start);
        }

        /// <summary>
        /// 继续服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Continue(string serviceName)
        {
            ChangeState(serviceName, AgileServiceOperationType.Continue);
        }

        /// <summary>
        /// 暂停服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Pause(string serviceName)
        {
            ChangeState(serviceName, AgileServiceOperationType.Pause);
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        public static void Stop(string serviceName)
        {
            ChangeState(serviceName, AgileServiceOperationType.Stop);
        }

        /// <summary>
        /// 根据服务名判断服务是否存在
        /// </summary>
        /// <param name="serviceName">服务名称</param>
        /// <returns>是否存在：true = 存在，false = 不存在</returns>
        public static bool Exists(string serviceName)
        {
            bool isExisted = false;

            AllServicesName.ForEach(one =>
            {
                if (one.ToLower() == serviceName.ToLower())
                {
                    isExisted = true;
                    return;
                }
            });
            return isExisted;
        }

        /// <summary>
        /// 根据用户名获取服务状态
        /// </summary>
        /// <param name="serviceName">服务名</param>
        /// <returns>服务状态（不存在该服务时，返回停止）</returns>
        public static ServiceControllerStatus GetServiceStatus(string serviceName)
        {
            var status = ServiceControllerStatus.Stopped;

            AllServices.ForEach(one =>
            {
                if (one.ServiceName.ToLower() == serviceName.ToLower())
                {
                    status = one.Status;
                }
            });
            return status;
        }

        /// <summary>
        /// 根据状态获取所有服务
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>所有服务列表</returns>
        public static List<ServiceController> GetAllServices(ServiceControllerStatus status)
        {
            var linq = from service in AllServices where service.Status == status select service;
            return linq.ToList();
        }

        /// <summary>
        /// 根据状态获取所有服务名称
        /// </summary>
        /// <param name="status">状态</param>
        /// <returns>所有服务名称列表</returns>
        public static List<string> GetAllServicesName(ServiceControllerStatus status)
        {
            var linq = from service in AllServices where service.Status == status select service.ServiceName;
            return linq.ToList();
        }
    }
}
