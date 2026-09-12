using HangarDesk.Final.Application;
using HangarDesk.Final.Configuration;
using HangarDesk.Final.Infrastructure;
using HangarDesk.Final.UI.ConsoleUI;
using HangarDesk.Final.UI.WinFormsUI;

namespace HangarDesk.Final;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        AppSettings settings = AppSettings.Load();
        DatabaseInitializer initializer = new(settings.Database.ConnectionString);
        initializer.Initialize();
        HangarDeskRepository repository = new(settings.Database.ConnectionString);
        HangarDeskService service = new(repository, new PasswordHasher());

        string ui = GetArgument(args, "--ui") ?? "console";
        if (args.Contains("--smoke-test"))
        {
            SmokeTest.Run(service);
            return;
        }

        if (ui.Equals("console", StringComparison.OrdinalIgnoreCase))
        {
            new ConsoleApplication(service).Run();
            return;
        }

        if (ui.Equals("winforms", StringComparison.OrdinalIgnoreCase))
        {
            ApplicationConfiguration.Initialize();
            if (!service.HasUsers())
            {
                using FirstRunForm firstRun = new(service);
                if (firstRun.ShowDialog() != DialogResult.OK) return;
            }
            using LoginForm login = new(service);
            if (login.ShowDialog() == DialogResult.OK)
                System.Windows.Forms.Application.Run(new MainForm(service, login.Session));
            return;
        }

        Console.WriteLine("Kullanım: HangarDesk.Final.exe --ui=console|winforms");
    }

    private static string GetArgument(string[] args, string name)
    {
        string prefix = name + "=";
        return args.FirstOrDefault(arg => arg.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))?.Substring(prefix.Length);
    }
}
