namespace Wizdle.Windows
{
    using System.Diagnostics;

    using Microsoft.Extensions.Logging;

    using Serilog;
    using Serilog.Events;
    using Serilog.Extensions.Logging;
    using Serilog.Sinks.RichTextBoxForms.Themes;

    using Wizdle.Models;

    public partial class WizdleForm : Form
    {
        private readonly WizdleEngine _wizdleEngine;

        public WizdleForm()
        {
            InitializeComponent();

            _wizdleEngine = new WizdleEngine(
                new SerilogLoggerFactory(
                    new LoggerConfiguration()
                    .WriteTo.RichTextBox(
                        _logsRtb,
                        LogEventLevel.Information,
                        theme: ThemePresets.Light)
                    .CreateLogger()).CreateLogger<WizdleEngine>());
        }

        private void SolveBtn_Click(object sender, EventArgs e)
        {
            List<char> correctLetters = [];
            correctLetters.Add(GetLetter(_correctLetter1.Text));
            correctLetters.Add(GetLetter(_correctLetter2.Text));
            correctLetters.Add(GetLetter(_correctLetter3.Text));
            correctLetters.Add(GetLetter(_correctLetter4.Text));
            correctLetters.Add(GetLetter(_correctLetter5.Text));

            List<char> misplacedLetters = [];
            misplacedLetters.Add(GetLetter(_misplacedLetter1.Text));
            misplacedLetters.Add(GetLetter(_misplacedLetter2.Text));
            misplacedLetters.Add(GetLetter(_misplacedLetter3.Text));
            misplacedLetters.Add(GetLetter(_misplacedLetter4.Text));
            misplacedLetters.Add(GetLetter(_misplacedLetter5.Text));

            var request = new WizdleRequest()
            {
                CorrectLetters = new string([.. correctLetters]),
                MisplacedLetters = new string([.. misplacedLetters]),
                ExcludeLetters = _excludedLetters.Text.Trim(),
            };

            _logsRtb.Clear();
            _wordsRtb.Clear();

            WizdleResponse response = _wizdleEngine.ProcessWizdleRequest(request);
            if (response.Words.Any())
            {
                _wordsRtb.Text = string.Join(", ", response.Words);
            }
        }

        private static char GetLetter(string input)
        {
            if (input.Length != 1)
            {
                return '?';
            }

            if (!char.IsLetter(input[0]))
            {
                return '?';
            }

            return input[0];
        }

        private void GitHubLbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://github.com/lyndychivs/Wizdle",
                    UseShellExecute = true,
                });
            }
            catch
            {
            }
        }
    }
}