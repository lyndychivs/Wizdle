namespace Wizdle.Wpf;

using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
using System.Diagnostics;
using System.Windows.Navigation;

using Wizdle.Models;
using System.Globalization;

public partial class MainWindow : Window
{
    private readonly WizdleEngine _wizdleEngine;

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
    }

    private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = e.Uri.AbsoluteUri,
            UseShellExecute = true
        });
        e.Handled = true;
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
}
