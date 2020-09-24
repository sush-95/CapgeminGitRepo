using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Common;
using AppLogger;

using System.Configuration;
using CaseCreationEngineService.App_Code;

namespace CaseCreationEngineService
{
    public partial class Service1 : ServiceBase
    {
       
            public Service1()
            {
                AppLog.Logger = new AppExtendLogger("Case Creation Engine Service");
                InitializeComponent();
            }
            Timer timer = new Timer();
            public bool isEngineExecuting;
            protected override void OnStart(string[] args)
            {
                AppLog.Info("Service Starting");
                string executeIntervalSeconds = Convert.ToString(ConfigurationManager.AppSettings["ExecuteIntervalSeconds"]);
                int executeInterval = string.IsNullOrWhiteSpace(executeIntervalSeconds) ? 300 : int.Parse(executeIntervalSeconds);
           // AppLog.Info("Service Starting1");

            isEngineExecuting = false;
           // AppLog.Info("Service Starting2"+ isEngineExecuting);
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
                timer.Interval = executeInterval * 1000;
                timer.Enabled = true;
           // AppLog.Info("Service Starting4");
        }

            private void OnElapsedTime(object source, ElapsedEventArgs e)
            {
            AppLog.Info("Service Starting5");
            if (!isEngineExecuting)
                {
                    try
                    {
                    AppLog.Info("Service Starting6");
                    isEngineExecuting = true;
                        Exceution();
                    }
                    catch (Exception ex)
                    {
                        AppLog.Error("CaseCreation Engine execution failure. Error Message: -\n" + ex.Message);
                        //TO-DO write code to send email.
                    }
                    finally
                    {
                        isEngineExecuting = false;
                    }

                }

            }

            protected override void OnStop()
            {
                AppLog.Info("Service Stoped");
            }
            internal void Exceution()
            {
                try
                {
                    AppLog.Error("Service is Started at " + DateTime.Now);
               
                using (FetchDataManager caseCreation = new FetchDataManager())
                {
                    caseCreation.getData();
                }
                }
                catch (Exception ex)
                {
                    AppLog.Error(Constants.Applogger.logg.error + ex.ToString());
                }
            }
        }
    }