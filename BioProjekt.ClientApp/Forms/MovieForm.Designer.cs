namespace BioProjekt.ClientApp.Forms
{
    partial class MovieForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblGenre;
        private System.Windows.Forms.TextBox txtGenre;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.TextBox txtDuration;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblLanguage;
        private System.Windows.Forms.TextBox txtLanguage;
        private System.Windows.Forms.Label lblAgeRating;
        private System.Windows.Forms.TextBox txtAgeRating;
        private System.Windows.Forms.Label lblPosterUrl;
        private System.Windows.Forms.TextBox txtPosterUrl;
        private System.Windows.Forms.Button btnChoosePoster;
        private System.Windows.Forms.Button btnCreateMovie;

        private System.Windows.Forms.Label lblScreeningDate;
        private System.Windows.Forms.DateTimePicker dtpScreeningDate;
        private System.Windows.Forms.Label lblScreeningTime;
        private System.Windows.Forms.TextBox txtScreeningTime;
        private System.Windows.Forms.Label lblLanguageVersion;
        private System.Windows.Forms.TextBox txtLanguageVersion;
        private System.Windows.Forms.CheckBox chkIs3D;
        private System.Windows.Forms.Label lblSoundSystem;
        private System.Windows.Forms.TextBox txtSoundSystem;
        private System.Windows.Forms.Label lblAuditorium;
        private System.Windows.Forms.ComboBox cmbAuditorium;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.lblGenre = new System.Windows.Forms.Label();
            this.txtGenre = new System.Windows.Forms.TextBox();
            this.lblDuration = new System.Windows.Forms.Label();
            this.txtDuration = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblLanguage = new System.Windows.Forms.Label();
            this.txtLanguage = new System.Windows.Forms.TextBox();
            this.lblAgeRating = new System.Windows.Forms.Label();
            this.txtAgeRating = new System.Windows.Forms.TextBox();
            this.lblPosterUrl = new System.Windows.Forms.Label();
            this.txtPosterUrl = new System.Windows.Forms.TextBox();
            this.btnChoosePoster = new System.Windows.Forms.Button();
            this.lblScreeningDate = new System.Windows.Forms.Label();
            this.dtpScreeningDate = new System.Windows.Forms.DateTimePicker();
            this.lblScreeningTime = new System.Windows.Forms.Label();
            this.txtScreeningTime = new System.Windows.Forms.TextBox();
            this.lblLanguageVersion = new System.Windows.Forms.Label();
            this.txtLanguageVersion = new System.Windows.Forms.TextBox();
            this.chkIs3D = new System.Windows.Forms.CheckBox();
            this.lblSoundSystem = new System.Windows.Forms.Label();
            this.txtSoundSystem = new System.Windows.Forms.TextBox();
            this.lblAuditorium = new System.Windows.Forms.Label();
            this.cmbAuditorium = new System.Windows.Forms.ComboBox();
            this.btnCreateMovie = new System.Windows.Forms.Button();

            this.SuspendLayout();

            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Text = "Titel";
            this.txtTitle.Location = new System.Drawing.Point(150, 20);
            this.txtTitle.Size = new System.Drawing.Size(250, 23);

            this.lblGenre.Location = new System.Drawing.Point(30, 60);
            this.lblGenre.Text = "Genre";
            this.txtGenre.Location = new System.Drawing.Point(150, 60);
            this.txtGenre.Size = new System.Drawing.Size(250, 23);

            this.lblDuration.Location = new System.Drawing.Point(30, 100);
            this.lblDuration.Text = "Varighed (min)";
            this.txtDuration.Location = new System.Drawing.Point(150, 100);
            this.txtDuration.Size = new System.Drawing.Size(250, 23);

            this.lblDescription.Location = new System.Drawing.Point(30, 140);
            this.lblDescription.Text = "Beskrivelse";
            this.txtDescription.Location = new System.Drawing.Point(150, 140);
            this.txtDescription.Size = new System.Drawing.Size(250, 23);

            this.lblLanguage.Location = new System.Drawing.Point(30, 180);
            this.lblLanguage.Text = "Sprog";
            this.txtLanguage.Location = new System.Drawing.Point(150, 180);
            this.txtLanguage.Size = new System.Drawing.Size(250, 23);

            this.lblAgeRating.Location = new System.Drawing.Point(30, 220);
            this.lblAgeRating.Text = "Aldersgrænse";
            this.txtAgeRating.Location = new System.Drawing.Point(150, 220);
            this.txtAgeRating.Size = new System.Drawing.Size(250, 23);

            this.lblPosterUrl.Location = new System.Drawing.Point(30, 260);
            this.lblPosterUrl.Text = "Plakat (sti)";
            this.txtPosterUrl.Location = new System.Drawing.Point(150, 260);
            this.txtPosterUrl.Size = new System.Drawing.Size(200, 23);
            this.btnChoosePoster.Location = new System.Drawing.Point(360, 260);
            this.btnChoosePoster.Size = new System.Drawing.Size(40, 23);
            this.btnChoosePoster.Text = "...";
            this.btnChoosePoster.Click += new System.EventHandler(this.btnChoosePoster_Click);

            this.lblScreeningDate.Location = new System.Drawing.Point(30, 300);
            this.lblScreeningDate.Text = "Dato";
            this.dtpScreeningDate.Location = new System.Drawing.Point(150, 300);
            this.dtpScreeningDate.Size = new System.Drawing.Size(250, 23);

            this.lblScreeningTime.Location = new System.Drawing.Point(30, 340);
            this.lblScreeningTime.Text = "Tid (hh:mm)";
            this.txtScreeningTime.Location = new System.Drawing.Point(150, 340);
            this.txtScreeningTime.Size = new System.Drawing.Size(250, 23);

            this.lblLanguageVersion.Location = new System.Drawing.Point(30, 380);
            this.lblLanguageVersion.Text = "Version";
            this.txtLanguageVersion.Location = new System.Drawing.Point(150, 380);
            this.txtLanguageVersion.Size = new System.Drawing.Size(250, 23);

            this.chkIs3D.Location = new System.Drawing.Point(150, 410);
            this.chkIs3D.Text = "3D";

            this.lblSoundSystem.Location = new System.Drawing.Point(30, 440);
            this.lblSoundSystem.Text = "Lydsystem";
            this.txtSoundSystem.Location = new System.Drawing.Point(150, 440);
            this.txtSoundSystem.Size = new System.Drawing.Size(250, 23);

            this.lblAuditorium.Location = new System.Drawing.Point(30, 480);
            this.lblAuditorium.Text = "Auditorium";
            this.cmbAuditorium.Location = new System.Drawing.Point(150, 480);
            this.cmbAuditorium.Size = new System.Drawing.Size(250, 23);
            this.cmbAuditorium.SelectedIndexChanged += new System.EventHandler(this.cmbAuditorium_SelectedIndexChanged);

            this.btnCreateMovie.Location = new System.Drawing.Point(150, 520);
            this.btnCreateMovie.Size = new System.Drawing.Size(120, 35);
            this.btnCreateMovie.Text = "Opret Film";
            this.btnCreateMovie.UseVisualStyleBackColor = true;
            this.btnCreateMovie.Click += new System.EventHandler(this.btnCreateMovie_Click);

            this.ClientSize = new System.Drawing.Size(450, 580);
            this.Controls.AddRange(new System.Windows.Forms.Control[] {
                lblTitle, txtTitle, lblGenre, txtGenre, lblDuration, txtDuration,
                lblDescription, txtDescription, lblLanguage, txtLanguage,
                lblAgeRating, txtAgeRating, lblPosterUrl, txtPosterUrl, btnChoosePoster,
                lblScreeningDate, dtpScreeningDate, lblScreeningTime, txtScreeningTime,
                lblLanguageVersion, txtLanguageVersion, chkIs3D, lblSoundSystem,
                txtSoundSystem, lblAuditorium, cmbAuditorium, btnCreateMovie
            });

            this.Name = "MovieForm";
            this.Text = "Opret ny film";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}