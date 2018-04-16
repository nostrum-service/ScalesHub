using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.UserSkins;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Net;
using NLog;
using ScalesHubPlugin;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace ScalesHubConsole
{
    static class Program
    {
        public static NxDecoderPlugin Plugins;
        public static String dbPath = "";

        public static List<Action<string, string>> subscribers = new List<Action<string, string>>();

        public static BindingList<ScalesChannel> MeasuringChannels = new BindingList<ScalesChannel>();

        public static void PostMessage(String msid, String message)
        {
            Task.Run(() =>
                {
                    foreach (var tmp in subscribers.ToArray())
                    {
                        Action<string, string> tmpAction = tmp;
                        try
                        {
                            tmpAction(msid, message);
                        }
                        catch
                        {

                        }
                    }
                });
        }

        public static XMainForm mainForm = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            LogManager.GetLogger(Application.ProductName).Info("Запуск приложения. V" + typeof(XMainForm).Assembly.GetName().Version);

            Application.ApplicationExit += (sender, e) => { 
                LogManager.GetLogger(Application.ProductName).Info("Завершение приложения");
                LogManager.Flush();
                mainForm = null;
            };

            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { 
                LogManager.GetLogger(Application.ProductName).Error("Ошибка приложения {0}", (e.ExceptionObject as Exception).Message);
                LogManager.Flush();
            };

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            UserLookAndFeel.Default.SetSkinStyle("Office 2013");

            try
            {
                Plugins = new NxDecoderPlugin();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }

            Application.Run(mainForm = new XMainForm());
        }

    }
    /// <summary>
    /// Класс импорта декодеров
    /// </summary>
    public class NxDecoderPlugin
    {
        [ImportMany(typeof(IDataFrame), AllowRecomposition = true)]
        public IEnumerable<Lazy<IDataFrame, IDataFrameMetadata>> DecoderModules;
        public CompositionContainer HostContainer;
        public NxDecoderPlugin()
        {
            var catalog = new AggregateCatalog(
                new AssemblyCatalog(System.Reflection.Assembly.GetExecutingAssembly()),
                new DirectoryCatalog(Path.Combine(Application.StartupPath, "Plugins"), "*.dll"),
                new DirectoryCatalog(Application.StartupPath, "ScalesHubPlugin.dll")
            );
            HostContainer = new CompositionContainer(catalog);
            try
            {
                HostContainer.ComposeParts(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(ex.ToString());
            }
        }
    }
}
