using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YunPos.MessageService.Host
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            if (args.Length < 1)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            else
            {
                var command = args[0];
                if (command == "--service")//以windows服务服务形式启动
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                        new CoreService()
                    };
                    ServiceBase.Run(ServicesToRun);
                }
                else if (command.Equals("--install", StringComparison.OrdinalIgnoreCase))//安装到windows服务
                {
                    var name = args.Length == 2 ? args[1] : GetServiceName();
                    InstallService(name);
                }
                else if (command.Equals("--uninstall", StringComparison.OrdinalIgnoreCase))//从windows服务卸载
                {
                    var name = args.Length == 2 ? args[1] : GetServiceName();
                    UninstallService(name);
                }
                else if (command.Equals("--start", StringComparison.OrdinalIgnoreCase))//启动windows服务
                {
                    var name = args.Length == 2 ? args[1] : GetServiceName();
                    StartService(name);
                }
                else if (command.Equals("--stop", StringComparison.OrdinalIgnoreCase))//停止windows服务
                {
                    var name = args.Length == 2 ? args[1] : GetServiceName();
                    StopService(name);
                }
            }
        }

        /// <summary>
        /// 获取windows service 名称
        /// </summary>
        /// <returns></returns>
        static string GetServiceName()
        {
            var serviceName = AppContext.Settings.ServiceName;
            if (string.IsNullOrEmpty(serviceName))
            {
                serviceName = "Yun.MessageService";
                CoreService.Logger.Info("系统未配置ServiceName,系统默认使用[Yun.MessageService]");
            }
            return serviceName;
        }

        /// <summary>
        /// 安装windows服务
        /// </summary>
        /// <param name="name"></param>
        private static void InstallService(string name)
        {
            string binPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Assembly.GetEntryAssembly().GetName().Name + ".exe") + " --service";

            string depend = "";

            var arg = string.Format(" create {0} start= auto displayname= \"{0}\" binpath= \"{1}\" depend= \"{2}\"",
                name, binPath, depend);
            CoreService.Logger.InfoFormat("开始安装windows服务, 参数[{0}]", arg);
            var ps = new ProcessStartInfo("sc.exe", arg);
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.UseShellExecute = false;
            var p = Process.Start(ps);
            p.WaitForExit();
        }

        /// <summary>
        /// 卸载windows服务
        /// </summary>
        /// <param name="name"></param>
        private static void UninstallService(string name)
        {
            var arg = string.Format(" delete {0} ", name);
            CoreService.Logger.InfoFormat("开始卸载windows服务, 参数[{0}]", arg);
            var ps = new ProcessStartInfo("sc.exe", arg);
            ps.WindowStyle = ProcessWindowStyle.Hidden;
            ps.UseShellExecute = false;
            var p = Process.Start(ps);
            p.WaitForExit();
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="name"></param>
        private static void StartService(string name)
        {
            CoreService.Logger.InfoFormat("开始启动 [{0}] 服务", name);
            using (var sc = new ServiceController(name))
            {
                CoreService.Logger.InfoFormat("服务 [{0}] 状态 [{1}]", name, sc.Status);
                if (sc.Status != ServiceControllerStatus.Running)
                {
                    try
                    {
                        sc.Start();
                        CoreService.Logger.InfoFormat("服务 [{0}] 已启动", name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        /// <param name="name"></param>
        private static void StopService(string name)
        {
            CoreService.Logger.InfoFormat("开始启动 [{0}] 服务", name);
            using (var sc = new ServiceController(name))
            {
                CoreService.Logger.InfoFormat("服务 [{0}] 状态 [{1}]", name, sc.Status);
                if (sc.Status == ServiceControllerStatus.Running)
                {
                    try
                    {
                        sc.Stop();
                        CoreService.Logger.InfoFormat("服务 [{0}] 已停止", name);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
