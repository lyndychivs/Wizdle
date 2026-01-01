namespace Wizdle.Wpf;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;

using Wizdle.Models;

public partial class MainWindow : Window
{
    private readonly WizdleEngine _wizdleEngine;
    private DispatcherTimer? _snackbarTimer;

    public MainWindow(WizdleEngine wizdleEngine)
    {
        InitializeComponent();
        _wizdleEngine = wizdleEngine ?? throw new ArgumentNullException(nameof(wizdleEngine));
    }

    public void SolveButton_Click(object sender, RoutedEventArgs e)
    {
        var correctLetters = new StringBuilder();
        correctLetters.Append(GetLetterFromTextBox(CorrectTextBox1));
        correctLetters.Append(GetLetterFromTextBox(CorrectTextBox2));
        correctLetters.Append(GetLetterFromTextBox(CorrectTextBox3));
        correctLetters.Append(GetLetterFromTextBox(CorrectTextBox4));
        correctLetters.Append(GetLetterFromTextBox(CorrectTextBox5));

        var misplacedLetters = new StringBuilder();
        misplacedLetters.Append(GetLetterFromTextBox(MisplacedTextBox1));
        misplacedLetters.Append(GetLetterFromTextBox(MisplacedTextBox2));
        misplacedLetters.Append(GetLetterFromTextBox(MisplacedTextBox3));
        misplacedLetters.Append(GetLetterFromTextBox(MisplacedTextBox4));
        misplacedLetters.Append(GetLetterFromTextBox(MisplacedTextBox5));

        var wizdleRequest = new WizdleRequest()
        {
            CorrectLetters = correctLetters.ToString(),
            MisplacedLetters = misplacedLetters.ToString(),
            ExcludeLetters = ExcludedTextBox.Text,
        };

        WizdleResponse wizdleResponse = _wizdleEngine.ProcessWizdleRequest(wizdleRequest);

        ResultsListBox.ItemsSource = wizdleResponse.Words.Select(w => w.ToUpper(CultureInfo.InvariantCulture));

        Visibility visibility = GetVisibility(wizdleResponse.Words.Any());
        ResultsLabel.Visibility = visibility;
        ResultsListBox.Visibility = visibility;

        if (wizdleResponse.Messages.Any())
        {
            ShowSnackbar(string.Join(Environment.NewLine, wizdleResponse.Messages));
        }
    }

    private static char GetLetterFromTextBox(TextBox textbox)
    {
        return string.IsNullOrWhiteSpace(textbox.Text)
            ? '?'
            : textbox.Text[0];
    }

    private static Visibility GetVisibility(bool isVisible)
    {
        return isVisible ? Visibility.Visible : Visibility.Hidden;
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = e.Uri.AbsoluteUri,
            UseShellExecute = true,
        });
        e.Handled = true;
    }

    private void ShowSnackbar(string message)
    {
        SnackbarText.Text = message;

        var fadeIn = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromMilliseconds(300),
        };

        SnackbarBorder.Visibility = Visibility.Visible;
        SnackbarBorder.BeginAnimation(OpacityProperty, fadeIn);

        _snackbarTimer?.Stop();
        _snackbarTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(3),
        };
        _snackbarTimer.Tick += (s, e) =>
        {
            HideSnackbar();
            _snackbarTimer.Stop();
        };
        _snackbarTimer.Start();
    }

    private void HideSnackbar()
    {
        var fadeOut = new DoubleAnimation
        {
            From = 1,
            To = 0,
            Duration = TimeSpan.FromMilliseconds(300),
        };

        fadeOut.Completed += (s, e) =>
        {
            SnackbarBorder.Visibility = Visibility.Collapsed;
        };

        SnackbarBorder.BeginAnimation(OpacityProperty, fadeOut);
    }
}
