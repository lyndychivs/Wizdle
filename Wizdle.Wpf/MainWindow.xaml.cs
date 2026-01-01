namespace Wizdle.Wpf;

using System.Text;
using System.Windows;
using System.Windows.Controls;

using Wizdle.Models;

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
    }

    private static char GetLetterFromTextBox(TextBox textbox)
    {
        return string.IsNullOrWhiteSpace(textbox.Text)
            ? '?'
            : textbox.Text[0];
    }
}
