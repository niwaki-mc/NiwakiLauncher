using Avalonia.Controls;
using NiwakiLauncher.ViewModels;

namespace NiwakiLauncher.Views;

public partial class Launcher : Window
{
    public Launcher()
    {
        InitializeComponent();
        DataContext = new LauncherViewModel(this);
    }
}