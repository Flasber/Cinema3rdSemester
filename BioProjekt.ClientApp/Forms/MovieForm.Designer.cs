using System;
using System.Windows.Forms;

namespace BioProjekt.ClientApp.Forms
{
    partial class MovieForm
    {
        private System.ComponentModel.IContainer components = null;

        private Label lblTitle;
        private TextBox txtTitle;
        private Label lblGenre;
        private TextBox txtGenre;
        private Label lblDuration;
        private TextBox txtDuration;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblLanguage;
        private TextBox txtLanguage;
        private Label lblAgeRating;
        private TextBox txtAgeRating;
        private Label lblPosterUrl;
        private TextBox txtPosterUrl;
        private Button btnChoosePoster;
        private Button btnCreateMovie;
        private Label lblScreeningDate;
        private DateTimePicker dtpScreeningDate;
        private Label lblScreeningTime;
        private TextBox txtScreeningTime;
        private Label lblLanguageVersion;
        private TextBox txtLanguageVersion;
        private CheckBox chkIs3D;
        private Label lblSoundSystem;
        private TextBox txtSoundSystem;
        private Label lblAuditorium;
        private ComboBox cmbAuditorium;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lblTitle = new Label();
            txtTitle = new TextBox();
            lblGenre = new Label();
            txtGenre = new TextBox();
            lblDuration = new Label();
            txtDuration = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblLanguage = new Label();
            txtLanguage = new TextBox();
            lblAgeRating = new Label();
            txtAgeRating = new TextBox();
            lblPosterUrl = new Label();
            txtPosterUrl = new TextBox();
            btnChoosePoster = new Button();
            lblScreeningDate = new Label();
            dtpScreeningDate = new DateTimePicker();
            lblScreeningTime = new Label();
            txtScreeningTime = new TextBox();
            lblLanguageVersion = new Label();
            txtLanguageVersion = new TextBox();
            chkIs3D = new CheckBox();
            lblSoundSystem = new Label();
            txtSoundSystem = new TextBox();
            lblAuditorium = new Label();
            cmbAuditorium = new ComboBox();
            btnCreateMovie = new Button();

            SuspendLayout();

            lblTitle.Location = new System.Drawing.Point(30, 20);
            lblTitle.Text = "Titel";
            txtTitle.Location = new System.Drawing.Point(150, 20);
            txtTitle.Size = new System.Drawing.Size(250, 23);

            lblGenre.Location = new System.Drawing.Point(30, 60);
            lblGenre.Text = "Genre";
            txtGenre.Location = new System.Drawing.Point(150, 60);
            txtGenre.Size = new System.Drawing.Size(250, 23);

            lblDuration.Location = new System.Drawing.Point(30, 100);
            lblDuration.Text = "Varighed (min)";
            txtDuration.Location = new System.Drawing.Point(150, 100);
            txtDuration.Size = new System.Drawing.Size(250, 23);

            lblDescription.Location = new System.Drawing.Point(30, 140);
            lblDescription.Text = "Beskrivelse";
            txtDescription.Location = new System.Drawing.Point(150, 140);
            txtDescription.Size = new System.Drawing.Size(250, 23);

            lblLanguage.Location = new System.Drawing.Point(30, 180);
            lblLanguage.Text = "Sprog";
            txtLanguage.Location = new System.Drawing.Point(150, 180);
            txtLanguage.Size = new System.Drawing.Size(250, 23);

            lblAgeRating.Location = new System.Drawing.Point(30, 220);
            lblAgeRating.Text = "Aldersgrænse";
            txtAgeRating.Location = new System.Drawing.Point(150, 220);
            txtAgeRating.Size = new System.Drawing.Size(250, 23);

            lblPosterUrl.Location = new System.Drawing.Point(30, 260);
            lblPosterUrl.Text = "Plakat (sti)";
            txtPosterUrl.Location = new System.Drawing.Point(150, 260);
            txtPosterUrl.Size = new System.Drawing.Size(200, 23);
            btnChoosePoster.Location = new System.Drawing.Point(360, 260);
            btnChoosePoster.Size = new System.Drawing.Size(40, 23);
            btnChoosePoster.Text = "...";
            btnChoosePoster.Click += new EventHandler(btnChoosePoster_Click);

            lblScreeningDate.Location = new System.Drawing.Point(30, 300);
            lblScreeningDate.Text = "Dato";
            dtpScreeningDate.Location = new System.Drawing.Point(150, 300);
            dtpScreeningDate.Size = new System.Drawing.Size(250, 23);

            lblScreeningTime.Location = new System.Drawing.Point(30, 340);
            lblScreeningTime.Text = "Tid (hh:mm)";
            txtScreeningTime.Location = new System.Drawing.Point(150, 340);
            txtScreeningTime.Size = new System.Drawing.Size(250, 23);

            lblLanguageVersion.Location = new System.Drawing.Point(30, 380);
            lblLanguageVersion.Text = "Version";
            txtLanguageVersion.Location = new System.Drawing.Point(150, 380);
            txtLanguageVersion.Size = new System.Drawing.Size(250, 23);

            chkIs3D.Location = new System.Drawing.Point(150, 410);
            chkIs3D.Text = "3D";

            lblSoundSystem.Location = new System.Drawing.Point(30, 440);
            lblSoundSystem.Text = "Lydsystem";
            txtSoundSystem.Location = new System.Drawing.Point(150, 440);
            txtSoundSystem.Size = new System.Drawing.Size(250, 23);

            lblAuditorium.Location = new System.Drawing.Point(30, 480);
            lblAuditorium.Text = "Auditorium";
            cmbAuditorium.Location = new System.Drawing.Point(150, 480);
            cmbAuditorium.Size = new System.Drawing.Size(250, 23);
            cmbAuditorium.SelectedIndexChanged += new EventHandler(cmbAuditorium_SelectedIndexChanged);

            btnCreateMovie.Location = new System.Drawing.Point(150, 520);
            btnCreateMovie.Size = new System.Drawing.Size(120, 35);
            btnCreateMovie.Text = "Opret Film";
            btnCreateMovie.Click += new EventHandler(btnCreateMovie_Click);

            ClientSize = new System.Drawing.Size(450, 580);
            Controls.AddRange(new Control[] {
                lblTitle, txtTitle, lblGenre, txtGenre, lblDuration, txtDuration,
                lblDescription, txtDescription, lblLanguage, txtLanguage,
                lblAgeRating, txtAgeRating, lblPosterUrl, txtPosterUrl, btnChoosePoster,
                lblScreeningDate, dtpScreeningDate, lblScreeningTime, txtScreeningTime,
                lblLanguageVersion, txtLanguageVersion, chkIs3D, lblSoundSystem,
                txtSoundSystem, lblAuditorium, cmbAuditorium, btnCreateMovie
            });

            Name = "MovieForm";
            Text = "Opret ny film";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
