namespace Wizdle.Maui;

using System;
using System.Linq;
using System.Text;

using Microsoft.Maui.Controls;

using Wizdle.Models;

public partial class MainPage : ContentPage
{
    private readonly WizdleEngine _wizdleEngine;

    public MainPage(WizdleEngine wizdleEngine)
    {
        InitializeComponent();
        _wizdleEngine = wizdleEngine ?? throw new ArgumentNullException(nameof(wizdleEngine));
    }

    private void OnSolveClicked(object sender, EventArgs e)
    {
        var correctLetters = new StringBuilder();
        correctLetters.Append(Correct1.Text ?? "?");
        correctLetters.Append(Correct2.Text ?? "?");
        correctLetters.Append(Correct3.Text ?? "?");
        correctLetters.Append(Correct4.Text ?? "?");
        correctLetters.Append(Correct5.Text ?? "?");

        var misplacedLetters = new StringBuilder();
        misplacedLetters.Append(Misplaced1.Text ?? "?");
        misplacedLetters.Append(Misplaced2.Text ?? "?");
        misplacedLetters.Append(Misplaced3.Text ?? "?");
        misplacedLetters.Append(Misplaced4.Text ?? "?");
        misplacedLetters.Append(Misplaced5.Text ?? "?");

        var wizdleRequest = new WizdleRequest()
        {
            CorrectLetters = correctLetters.ToString(),
            MisplacedLetters = misplacedLetters.ToString(),
            ExcludeLetters = Excluded.Text ?? string.Empty,
        };

        WizdleResponse wizdleResponse = _wizdleEngine.ProcessWizdleRequest(wizdleRequest);

        ResultsLabel.IsVisible = wizdleResponse.Words.Any();
        WordsCollectionView.ItemsSource = wizdleResponse.Words;
    }
}
