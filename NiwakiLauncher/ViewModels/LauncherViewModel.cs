using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Authentication;
using System.Threading.Tasks;
using System.Windows;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.Auth.Microsoft;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Apis.Auth.OAuth2;
using MsBox.Avalonia;
using MsBox.Avalonia.Base;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;
using Newtonsoft.Json;
using NiwakiLauncher.Models;
using NiwakiLauncher.Utils;
using ReactiveUI;
using Icon = MsBox.Avalonia.Enums.Icon;
using Window = Avalonia.Controls.Window;

namespace NiwakiLauncher.ViewModels;

public class LauncherViewModel : ViewModelBase
{
    #region Propriétés

    #region Données statiques

    // Chemin d'accès au fichiers du launcher
    private static readonly string BasePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".niwaki");
    private static readonly string Path = System.IO.Path.Combine(BasePath, "launcher.json");
    private static readonly string BaseUrl = "https://niwaki-mc.fr";
    
    #endregion
    
    #region Propriétés internes

    private string? _userName;
    private string? _connectionStatus;
    private MSession? _session;
    private CMLauncher? _launcher;
    private double _progress;
    private readonly Window _window;
    private string _text = "En attente...";
    private int _selectedRam = 4;
    private int _maxRam;
    private bool _isConnected;
    private readonly SparkleUpdater _sparkle;
    private bool _isUpdateRunning;
    private bool _indeterminateDownload;

    #endregion

    #region Propriétés observables
    
    public int SelectedRam
    {
        get => _selectedRam;
        set => this.RaiseAndSetIfChanged(ref _selectedRam, value);
    }
    public int MaxRam
    {
        get => _maxRam;
        set => this.RaiseAndSetIfChanged(ref _maxRam, value);
    }
    public double Progress
    {
        get => _progress;
        set => this.RaiseAndSetIfChanged(ref _progress, value);
    }
    public string Text
    {
        get => _text;
        set => this.RaiseAndSetIfChanged(ref _text, value);
    }
    private string? UserName
    {
        get => _userName;
        set => this.RaiseAndSetIfChanged(ref _userName, value);
    }
    
    public string? ConnectionStatus
    {
        get => _connectionStatus;
        private set => this.RaiseAndSetIfChanged(ref _connectionStatus, value);
    }

    public bool IsConnected
    {
        get => _isConnected;
        private set => this.RaiseAndSetIfChanged(ref _isConnected, value);
    }
    public bool IsUpdateRunning
    {
        get => _isUpdateRunning;
        private set => this.RaiseAndSetIfChanged(ref _isUpdateRunning, value);
    }

    public string Title 
    { 
        get 
        { 
            Version version = Assembly.GetExecutingAssembly().GetName().Version ?? new Version("99.99.99");
            return $"Niwaki - {version.Major}.{version.Minor}.{version.Build}"; 
        } 
    }

    public bool InderminateTime {  
        get => _indeterminateDownload;
        private set => this.RaiseAndSetIfChanged(ref _indeterminateDownload, value); 
    }

    #endregion
    
    #endregion

    #region Constructeur
    
    /// <summary>
    /// Controller (View-Model) de la fenêtre de base
    /// </summary>
    /// <param name="window">La fenêtre de base afin de pouvoir la cacher ou la montrer</param>
    public LauncherViewModel(Window window)
    {
        _window = window;
        FileHelper.InitFileIfNotExists(Path);

        DataSave? ds = JsonConvert.DeserializeObject<DataSave>(File.ReadAllText(Path));
        if (ds is not null)
        {
            SelectedRam = ds.SelectedRam;
        }
        
        ManagementObject? wmiObject = new ManagementObjectSearcher("select * from Win32_ComputerSystem").Get().Cast<ManagementObject>().First();
        
        MaxRam = Convert.ToInt32((ulong) wmiObject["TotalPhysicalMemory"] / (1024*1024*1024));

        _session = AuthentificationHelper.Authenticate(AuthentificationType.Silent, SetStatuses, _ => EmptyExceptionHandling(), () => {}).Result;
        

        
        _sparkle = new SparkleUpdater($"{BaseUrl}/downloads/launcher/appcast.xml", new Ed25519Checker(SecurityMode.Strict, "EmO7G2yYinLziV2wKkhCvgYUvHCLsRNM+hS6EpgtrZQ="))
        {
            UIFactory = new NetSparkleUpdater.UI.Avalonia.UIFactory(_window.Icon),
            ShowsUIOnMainThread = true
        };
        _sparkle.SecurityProtocolType = System.Net.SecurityProtocolType.Tls12;
        _sparkle.StartLoop(true, true);

        _sparkle.UpdateDetected += (_, _) =>
        {
            IsUpdateRunning = true;
        };
        _sparkle.UserRespondedToUpdate += (_, args) =>
        {
            if (args.Result != UpdateAvailableResult.InstallUpdate)
            {
                IsUpdateRunning = false;
            }
        };

    }

    #endregion

    #region méthodes internes

    /// <summary>
    /// Sets the statuses of the MainWindowViewModel.
    /// </summary>
    /// <param name="session">The authenticated session.</param>
    private void SetStatuses(MSession session)
    {
        UserName = session.Username;
        ConnectionStatus = $"Connecté en temps que {UserName}";
        IsConnected = true;
    }

    /// <summary>
    /// Exception handling method that displays an error message to the user.
    /// </summary>
    /// <param name="e">The exception that occurred.</param>
    private async void ExceptionHandling(Exception e)
    {
        EmptyExceptionHandling();
        await ShowError(string.Empty, e);
    }

    /// <summary>
    /// EmptyExceptionHandling method.
    /// </summary>
    private void EmptyExceptionHandling()
    {
        ConnectionStatus = string.Empty;  
    }


    /// <summary>
    /// Shows an error message box with the given preMessage and exception.
    /// </summary>
    /// <param name="preMessage">The prefix message to display before the exception message.</param>
    /// <param name="e">The exception to display.</param>
    private async Task ShowError(string preMessage, Exception e)
    {
        string message = string.Empty;
        if (!string.IsNullOrWhiteSpace(preMessage))
        {
            message = preMessage + " ";
        }
        
        message +=  $"{e.Message}";
        IMsBox<string>? messageBoxStandardWindow = MessageBoxManager
            // .GetMessageBoxStandard("Erreur", message, ButtonEnum.Ok, Icon.Error);
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions = new []
                {
                    new ButtonDefinition
                    {
                        Name = "OK"
                    },
                    new ButtonDefinition()
                    {
                        Name = "Envoyer le rapport de bug"
                    }
                },
                Icon = Icon.Error,
                ContentTitle = "Erreur",
                ContentHeader = "Une erreur est survenue",
                ContentMessage = message
            });
        string ret = await messageBoxStandardWindow.ShowAsPopupAsync(_window);
        if (ret == "Envoyer le rapport de bug")
        {
            FirebaseClient client = new FirebaseClient(
                "https://niwaki-93197-default-rtdb.europe-west1.firebasedatabase.app/", new FirebaseOptions()
                {
                    AuthTokenAsyncFactory = GetAccessToken, 
                    AsAccessToken = true
                });

            await client.Child("reports").Child(Guid.NewGuid().ToString().Replace("/", "(@)")).PostAsync(new ReportData()
            {
                MinecraftUserName = _session?.Username,
                // ComputerUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                ComputerUserName = Environment.UserName,
                ErrorMessage = message,
                DetailedError = JsonConvert.SerializeObject(e, Formatting.Indented)
            });
        }
        
    }

    private static async Task<string> GetAccessToken()
    {
        GoogleCredential? credential = GoogleCredential.FromFile("Supplementaries/secret_token.json").CreateScoped(new[]
        {
            "https://www.googleapis.com/auth/userinfo.email",
            "https://www.googleapis.com/auth/firebase.database"
        });
        ITokenAccess c = credential;
        
        return await c.GetAccessTokenForRequestAsync();
    }

    #endregion

    #region Methodes Appelée en vue

    /// <summary>
    /// Logs out the current user session.
    /// </summary>
    public async void Logout()
    {
        try
        {
            JELoginHandler loginHandler = JELoginHandlerBuilder.BuildDefault();
            await loginHandler.Signout();
            _session = null;
            ConnectionStatus = string.Empty; 
            UserName = null;
            IsConnected = false;
        } catch (Exception e)
        {
            await ShowError("Erreur lors de la déconnexion :", e);
        }
    }


    public async void CheckUpdate()
    {
        await _sparkle.CheckForUpdatesAtUserRequest();
    }
    
    /// <summary>
    /// Authenticate user with Microsoft
    /// </summary>
    public async void MicrosoftAuthenticationCommand()
    {
        _session = await AuthentificationHelper.Authenticate(AuthentificationType.Interactive, SetStatuses, ExceptionHandling,
            () => { });
    }

    /// <summary>
    /// Launches the game with the specified settings.
    /// </summary>
    /// <remarks>
    /// This method saves the selected RAM value to a JSON file and then proceeds to launch the game using the specified Forge version.
    /// </remarks>
    /// <exception cref="AuthenticationException">
    /// Thrown if the user is not authenticated before launching the game.
    /// </exception>
    public async void LaunchGameCommand()
    {
        try
        {

            if (_session is null)
            {
                throw new AuthenticationException("Veuillez vous authentifier avant de lancer le jeu");
            }

            DataSave ds = new DataSave()
            {
                SelectedRam = SelectedRam
            };

            FileHelper.InitFileIfNotExists(Path);

            await File.WriteAllTextAsync(Path, JsonConvert.SerializeObject(ds, Formatting.Indented));

            await FileHelper.DownloadFileFromWebServerWithProgress(BaseUrl, "neoforge_1.20.1.json", BasePath,
                System.IO.Path.Combine("versions", "neoforge_1.20.1"), (done, total) =>
                {
                    Text = $"Téléchargement de la version du jeu : {done}/{total}";
                    Progress = (done * 1.0 / total * 1.0) * 100;
                });

            Progress = 0.0;
            Text = "Obtention des librairies ...";
            InderminateTime = true;

            await FileHelper.DownloadFileFromWebServerWithProgress(BaseUrl, "libs.zip", BasePath, "libraries",
                (done, total) =>
                {
                    InderminateTime = false;
                    Text = $"Téléchargement des librairies nécessaires : {done}/{total}";
                    Progress = (done * 1.0 / total * 1.0) * 100;
                });

            Progress = 0.0;
            Text = "Extraction des librairies";
            InderminateTime = true;

            await FileHelper.ExtractAndDeleteZip(BasePath, "libraries", "libs.zip");

            InderminateTime = false;

            _launcher = LauncherHelper.InitLauncher(BasePath,
                (e) => { Text = $"Verification : {e.FileKind} ({e.ProgressedFileCount}/{e.TotalFileCount})"; },
                (_, e) => { Progress = e.ProgressPercentage; });



            Process process = await _launcher.CreateProcessAsync("neoforge_1.20.1", new MLaunchOption()
            {
                Session = _session,
                MaximumRamMb = SelectedRam * 1024,
                ScreenWidth = Convert.ToInt32(Math.Floor(SystemParameters.PrimaryScreenWidth)) - 10,
                ScreenHeight = Convert.ToInt32(Math.Floor(SystemParameters.PrimaryScreenHeight)) - 50
            });


            Progress = 0.0;
            Text = "En attente ...";
            process.Start();
            _window.Hide();

            await process.WaitForExitAsync();
            _window.Show();
        }
        catch (AuthenticationException e)
        {
            await ShowError(string.Empty, e);
        }
        catch (Exception e)
        {
            await ShowError("Une erreur est survenue :", e);
        }
        finally
        {
            Progress = 0.0;
            Text = "En attente ...";
        }



    }

    #endregion
}