using System;
using System.Windows;
using System.Windows.Input;

using Microsoft.Web.WebView2.Core;

namespace View;
/// <summary>
/// Interaction logic for SpellCheckWindow.xaml
/// </summary>
public partial class SpellCheckWindow : Window
{
    public SpellCheckWindow()
    {
        InitializeComponent();
        ConfigureSite();
    }

    private async void ConfigureSite()
    {
        await web.EnsureCoreWebView2Async();
        web.Source = new Uri("https://quillbot.com/grammar-check");
        web.NavigationCompleted += Web_NavigationCompleted;
    }

    private async void Web_NavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        var msg = Clipboard.GetText();
        await web.ExecuteScriptAsync($"document.getElementById('grammarbot').setAttribute('data-mce-placeholder', 'Press CTRL + V, To paste the text')");
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }
}
