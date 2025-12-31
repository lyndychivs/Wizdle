namespace Wizdle.Windows;

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

partial class WizdleForm : Form
{
    private void InitializeComponent()
    {
        var resources = new ComponentResourceManager(typeof(WizdleForm));
        _correctLetter1 = new TextBox();
        _correctLetter2 = new TextBox();
        _correctLetter3 = new TextBox();
        _correctLetter4 = new TextBox();
        _correctLetter5 = new TextBox();
        _misplacedLetter1 = new TextBox();
        _misplacedLetter2 = new TextBox();
        _misplacedLetter3 = new TextBox();
        _misplacedLetter4 = new TextBox();
        _misplacedLetter5 = new TextBox();
        _excludedLetters = new TextBox();
        _solveButton = new Button();
        _logsRtb = new RichTextBox();
        _logsLbl = new Label();
        _wordsLbl = new Label();
        _wordsRtb = new RichTextBox();
        _githubLbl = new LinkLabel();
        SuspendLayout();
        // 
        // _correctLetter1
        // 
        _correctLetter1.BackColor = Color.YellowGreen;
        _correctLetter1.BorderStyle = BorderStyle.FixedSingle;
        _correctLetter1.CharacterCasing = CharacterCasing.Upper;
        _correctLetter1.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _correctLetter1.ForeColor = SystemColors.WindowText;
        _correctLetter1.Location = new Point(205, 12);
        _correctLetter1.MaxLength = 1;
        _correctLetter1.Multiline = true;
        _correctLetter1.Name = "_correctLetter1";
        _correctLetter1.PlaceholderText = "?";
        _correctLetter1.Size = new Size(50, 50);
        _correctLetter1.TabIndex = 0;
        _correctLetter1.TextAlign = HorizontalAlignment.Center;
        // 
        // _correctLetter2
        // 
        _correctLetter2.BackColor = Color.YellowGreen;
        _correctLetter2.BorderStyle = BorderStyle.FixedSingle;
        _correctLetter2.CharacterCasing = CharacterCasing.Upper;
        _correctLetter2.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _correctLetter2.ForeColor = SystemColors.WindowText;
        _correctLetter2.Location = new Point(261, 12);
        _correctLetter2.MaxLength = 1;
        _correctLetter2.Multiline = true;
        _correctLetter2.Name = "_correctLetter2";
        _correctLetter2.PlaceholderText = "?";
        _correctLetter2.Size = new Size(50, 50);
        _correctLetter2.TabIndex = 3;
        _correctLetter2.TextAlign = HorizontalAlignment.Center;
        // 
        // _correctLetter3
        // 
        _correctLetter3.BackColor = Color.YellowGreen;
        _correctLetter3.BorderStyle = BorderStyle.FixedSingle;
        _correctLetter3.CharacterCasing = CharacterCasing.Upper;
        _correctLetter3.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _correctLetter3.ForeColor = SystemColors.WindowText;
        _correctLetter3.Location = new Point(317, 12);
        _correctLetter3.MaxLength = 1;
        _correctLetter3.Multiline = true;
        _correctLetter3.Name = "_correctLetter3";
        _correctLetter3.PlaceholderText = "?";
        _correctLetter3.Size = new Size(50, 50);
        _correctLetter3.TabIndex = 4;
        _correctLetter3.TextAlign = HorizontalAlignment.Center;
        // 
        // _correctLetter4
        // 
        _correctLetter4.BackColor = Color.YellowGreen;
        _correctLetter4.BorderStyle = BorderStyle.FixedSingle;
        _correctLetter4.CharacterCasing = CharacterCasing.Upper;
        _correctLetter4.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _correctLetter4.ForeColor = SystemColors.WindowText;
        _correctLetter4.Location = new Point(373, 12);
        _correctLetter4.MaxLength = 1;
        _correctLetter4.Multiline = true;
        _correctLetter4.Name = "_correctLetter4";
        _correctLetter4.PlaceholderText = "?";
        _correctLetter4.Size = new Size(50, 50);
        _correctLetter4.TabIndex = 5;
        _correctLetter4.TextAlign = HorizontalAlignment.Center;
        // 
        // _correctLetter5
        // 
        _correctLetter5.BackColor = Color.YellowGreen;
        _correctLetter5.BorderStyle = BorderStyle.FixedSingle;
        _correctLetter5.CharacterCasing = CharacterCasing.Upper;
        _correctLetter5.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _correctLetter5.ForeColor = SystemColors.WindowText;
        _correctLetter5.Location = new Point(429, 12);
        _correctLetter5.MaxLength = 1;
        _correctLetter5.Multiline = true;
        _correctLetter5.Name = "_correctLetter5";
        _correctLetter5.PlaceholderText = "?";
        _correctLetter5.Size = new Size(50, 50);
        _correctLetter5.TabIndex = 6;
        _correctLetter5.TextAlign = HorizontalAlignment.Center;
        // 
        // _misplacedLetter1
        // 
        _misplacedLetter1.BackColor = Color.Khaki;
        _misplacedLetter1.BorderStyle = BorderStyle.FixedSingle;
        _misplacedLetter1.CharacterCasing = CharacterCasing.Upper;
        _misplacedLetter1.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _misplacedLetter1.ForeColor = SystemColors.WindowText;
        _misplacedLetter1.Location = new Point(205, 68);
        _misplacedLetter1.MaxLength = 1;
        _misplacedLetter1.Multiline = true;
        _misplacedLetter1.Name = "_misplacedLetter1";
        _misplacedLetter1.PlaceholderText = "?";
        _misplacedLetter1.Size = new Size(50, 50);
        _misplacedLetter1.TabIndex = 8;
        _misplacedLetter1.TextAlign = HorizontalAlignment.Center;
        // 
        // _misplacedLetter2
        // 
        _misplacedLetter2.BackColor = Color.Khaki;
        _misplacedLetter2.BorderStyle = BorderStyle.FixedSingle;
        _misplacedLetter2.CharacterCasing = CharacterCasing.Upper;
        _misplacedLetter2.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _misplacedLetter2.ForeColor = SystemColors.WindowText;
        _misplacedLetter2.Location = new Point(261, 68);
        _misplacedLetter2.MaxLength = 1;
        _misplacedLetter2.Multiline = true;
        _misplacedLetter2.Name = "_misplacedLetter2";
        _misplacedLetter2.PlaceholderText = "?";
        _misplacedLetter2.Size = new Size(50, 50);
        _misplacedLetter2.TabIndex = 9;
        _misplacedLetter2.TextAlign = HorizontalAlignment.Center;
        // 
        // _misplacedLetter3
        // 
        _misplacedLetter3.BackColor = Color.Khaki;
        _misplacedLetter3.BorderStyle = BorderStyle.FixedSingle;
        _misplacedLetter3.CharacterCasing = CharacterCasing.Upper;
        _misplacedLetter3.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _misplacedLetter3.ForeColor = SystemColors.WindowText;
        _misplacedLetter3.Location = new Point(317, 68);
        _misplacedLetter3.MaxLength = 1;
        _misplacedLetter3.Multiline = true;
        _misplacedLetter3.Name = "_misplacedLetter3";
        _misplacedLetter3.PlaceholderText = "?";
        _misplacedLetter3.Size = new Size(50, 50);
        _misplacedLetter3.TabIndex = 10;
        _misplacedLetter3.TextAlign = HorizontalAlignment.Center;
        // 
        // _misplacedLetter4
        // 
        _misplacedLetter4.BackColor = Color.Khaki;
        _misplacedLetter4.BorderStyle = BorderStyle.FixedSingle;
        _misplacedLetter4.CharacterCasing = CharacterCasing.Upper;
        _misplacedLetter4.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _misplacedLetter4.ForeColor = SystemColors.WindowText;
        _misplacedLetter4.Location = new Point(373, 68);
        _misplacedLetter4.MaxLength = 1;
        _misplacedLetter4.Multiline = true;
        _misplacedLetter4.Name = "_misplacedLetter4";
        _misplacedLetter4.PlaceholderText = "?";
        _misplacedLetter4.Size = new Size(50, 50);
        _misplacedLetter4.TabIndex = 11;
        _misplacedLetter4.TextAlign = HorizontalAlignment.Center;
        // 
        // _misplacedLetter5
        // 
        _misplacedLetter5.BackColor = Color.Khaki;
        _misplacedLetter5.BorderStyle = BorderStyle.FixedSingle;
        _misplacedLetter5.CharacterCasing = CharacterCasing.Upper;
        _misplacedLetter5.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _misplacedLetter5.ForeColor = SystemColors.WindowText;
        _misplacedLetter5.Location = new Point(429, 68);
        _misplacedLetter5.MaxLength = 1;
        _misplacedLetter5.Multiline = true;
        _misplacedLetter5.Name = "_misplacedLetter5";
        _misplacedLetter5.PlaceholderText = "?";
        _misplacedLetter5.Size = new Size(50, 50);
        _misplacedLetter5.TabIndex = 12;
        _misplacedLetter5.TextAlign = HorizontalAlignment.Center;
        // 
        // _excludedLetters
        // 
        _excludedLetters.BackColor = Color.LightGray;
        _excludedLetters.BorderStyle = BorderStyle.FixedSingle;
        _excludedLetters.CharacterCasing = CharacterCasing.Upper;
        _excludedLetters.Font = new Font("Impact", 27.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
        _excludedLetters.ForeColor = SystemColors.WindowText;
        _excludedLetters.Location = new Point(205, 124);
        _excludedLetters.MaxLength = 26;
        _excludedLetters.Name = "_excludedLetters";
        _excludedLetters.Size = new Size(274, 53);
        _excludedLetters.TabIndex = 14;
        _excludedLetters.TextAlign = HorizontalAlignment.Center;
        // 
        // _solveButton
        // 
        _solveButton.BackColor = Color.DarkSeaGreen;
        _solveButton.FlatAppearance.BorderSize = 2;
        _solveButton.FlatStyle = FlatStyle.Flat;
        _solveButton.Font = new Font("Gadugi", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
        _solveButton.ForeColor = Color.Black;
        _solveButton.Location = new Point(373, 183);
        _solveButton.Name = "_solveButton";
        _solveButton.Size = new Size(106, 53);
        _solveButton.TabIndex = 15;
        _solveButton.Text = "SOLVE";
        _solveButton.UseVisualStyleBackColor = false;
        _solveButton.Click += SolveBtn_Click;
        // 
        // _logsRtb
        // 
        _logsRtb.BackColor = SystemColors.GradientInactiveCaption;
        _logsRtb.Location = new Point(12, 256);
        _logsRtb.Name = "_logsRtb";
        _logsRtb.Size = new Size(656, 90);
        _logsRtb.TabIndex = 17;
        _logsRtb.Text = "";
        // 
        // _logsLbl
        // 
        _logsLbl.AutoSize = true;
        _logsLbl.Font = new Font("Calibri", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
        _logsLbl.ForeColor = SystemColors.ActiveCaptionText;
        _logsLbl.Location = new Point(12, 227);
        _logsLbl.Name = "_logsLbl";
        _logsLbl.Size = new Size(57, 26);
        _logsLbl.TabIndex = 18;
        _logsLbl.Text = "Logs:";
        // 
        // _wordsLbl
        // 
        _wordsLbl.AutoSize = true;
        _wordsLbl.Font = new Font("Calibri", 15.75F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
        _wordsLbl.ForeColor = SystemColors.ActiveCaptionText;
        _wordsLbl.Location = new Point(12, 349);
        _wordsLbl.Name = "_wordsLbl";
        _wordsLbl.Size = new Size(73, 26);
        _wordsLbl.TabIndex = 19;
        _wordsLbl.Text = "Words:";
        // 
        // _wordsRtb
        // 
        _wordsRtb.BackColor = SystemColors.GradientInactiveCaption;
        _wordsRtb.Font = new Font("Impact", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
        _wordsRtb.Location = new Point(12, 378);
        _wordsRtb.Name = "_wordsRtb";
        _wordsRtb.Size = new Size(656, 402);
        _wordsRtb.TabIndex = 20;
        _wordsRtb.Text = "";
        // 
        // _githubLbl
        // 
        _githubLbl.AutoSize = true;
        _githubLbl.Location = new Point(475, 783);
        _githubLbl.Name = "_githubLbl";
        _githubLbl.Size = new Size(193, 15);
        _githubLbl.TabIndex = 21;
        _githubLbl.TabStop = true;
        _githubLbl.Text = "GitHub - lyndychivs - Wizdle - 2025";
        _githubLbl.LinkClicked += GitHubLbl_LinkClicked;
        // 
        // WizdleForm
        // 
        BackColor = SystemColors.InactiveCaption;
        ClientSize = new Size(680, 805);
        Controls.Add(_githubLbl);
        Controls.Add(_wordsRtb);
        Controls.Add(_wordsLbl);
        Controls.Add(_logsLbl);
        Controls.Add(_logsRtb);
        Controls.Add(_solveButton);
        Controls.Add(_excludedLetters);
        Controls.Add(_misplacedLetter5);
        Controls.Add(_misplacedLetter4);
        Controls.Add(_misplacedLetter3);
        Controls.Add(_misplacedLetter2);
        Controls.Add(_misplacedLetter1);
        Controls.Add(_correctLetter5);
        Controls.Add(_correctLetter4);
        Controls.Add(_correctLetter3);
        Controls.Add(_correctLetter2);
        Controls.Add(_correctLetter1);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        Name = "WizdleForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Wizdle";
        ResumeLayout(false);
        PerformLayout();
    }
    private TextBox _correctLetter1;
    private TextBox _correctLetter2;
    private TextBox _correctLetter3;
    private TextBox _correctLetter4;
    private TextBox _correctLetter5;
    private TextBox _misplacedLetter1;
    private TextBox _misplacedLetter2;
    private TextBox _misplacedLetter3;
    private TextBox _misplacedLetter4;
    private TextBox _misplacedLetter5;
    private TextBox _excludedLetters;
    private Button _solveButton;
    private RichTextBox _logsRtb;
    private Label _logsLbl;
    private Label _wordsLbl;
    private RichTextBox _wordsRtb;
    private LinkLabel _githubLbl;
}
