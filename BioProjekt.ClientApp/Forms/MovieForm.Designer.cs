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
        private System.Windows.Forms.Button btnCreateMovie;

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
            this.btnCreateMovie = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Text = "Titel";

            // txtTitle
            this.txtTitle.Location = new System.Drawing.Point(150, 20);
            this.txtTitle.Size = new System.Drawing.Size(250, 23);

            // lblGenre
            this.lblGenre.AutoSize = true;
            this.lblGenre.Location = new System.Drawing.Point(30, 60);
            this.lblGenre.Text = "Genre";

            // txtGenre
            this.txtGenre.Location = new System.Drawing.Point(150, 60);
            this.txtGenre.Size = new System.Drawing.Size(250, 23);

            // lblDuration
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(30, 100);
            this.lblDuration.Text = "Varighed (min)";

            // txtDuration
            this.txtDuration.Location = new System.Drawing.Point(150, 100);
            this.txtDuration.Size = new System.Drawing.Size(250, 23);

            // lblDescription
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(30, 140);
            this.lblDescription.Text = "Beskrivelse";

            // txtDescription
            this.txtDescription.Location = new System.Drawing.Point(150, 140);
            this.txtDescription.Size = new System.Drawing.Size(250, 23);

            // lblLanguage
            this.lblLanguage.AutoSize = true;
            this.lblLanguage.Location = new System.Drawing.Point(30, 180);
            this.lblLanguage.Text = "Sprog";

            // txtLanguage
            this.txtLanguage.Location = new System.Drawing.Point(150, 180);
            this.txtLanguage.Size = new System.Drawing.Size(250, 23);

            // lblAgeRating
            this.lblAgeRating.AutoSize = true;
            this.lblAgeRating.Location = new System.Drawing.Point(30, 220);
            this.lblAgeRating.Text = "Aldersgrænse";

            // txtAgeRating
            this.txtAgeRating.Location = new System.Drawing.Point(150, 220);
            this.txtAgeRating.Size = new System.Drawing.Size(250, 23);

            // lblPosterUrl
            this.lblPosterUrl.AutoSize = true;
            this.lblPosterUrl.Location = new System.Drawing.Point(30, 260);
            this.lblPosterUrl.Text = "Plakat URL";

            // txtPosterUrl
            this.txtPosterUrl.Location = new System.Drawing.Point(150, 260);
            this.txtPosterUrl.Size = new System.Drawing.Size(250, 23);

            // btnCreateMovie
            this.btnCreateMovie.Location = new System.Drawing.Point(150, 310);
            this.btnCreateMovie.Size = new System.Drawing.Size(120, 30);
            this.btnCreateMovie.Text = "Opret Film";
            this.btnCreateMovie.UseVisualStyleBackColor = true;
            this.btnCreateMovie.Click += new System.EventHandler(this.btnCreateMovie_Click);

            // MovieForm
            this.ClientSize = new System.Drawing.Size(450, 370);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.lblGenre);
            this.Controls.Add(this.txtGenre);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.txtDuration);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblLanguage);
            this.Controls.Add(this.txtLanguage);
            this.Controls.Add(this.lblAgeRating);
            this.Controls.Add(this.txtAgeRating);
            this.Controls.Add(this.lblPosterUrl);
            this.Controls.Add(this.txtPosterUrl);
            this.Controls.Add(this.btnCreateMovie);
            this.Name = "MovieForm";
            this.Text = "Opret ny film";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
