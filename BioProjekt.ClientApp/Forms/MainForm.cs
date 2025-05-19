using System;
using System.Windows.Forms;
using BioProjekt.ClientApp.BusinessLogic;
using BioProjekt.Shared.ClientDtos;

namespace BioProjekt.ClientApp.Forms
{
    public partial class MovieForm : Form
    {
        private readonly MovieManager _movieManager;

        public MovieForm()
        {
            InitializeComponent();
            _movieManager = new MovieManager();
        }

        private async void btnCreateMovie_Click(object sender, EventArgs e)
        {
            var movie = new MovieCreateDto
            {
                Title = txtTitle.Text,
                Genre = txtGenre.Text,
                Duration = int.TryParse(txtDuration.Text, out var dur) ? dur : 0,
                Description = txtDescription.Text,
                Language = txtLanguage.Text,
                AgeRating = txtAgeRating.Text
            };

            var screening = new ScreeningCreateDto
            {
                Date = dtpScreeningDate.Value.Date,
                Time = TimeSpan.TryParse(txtScreeningTime.Text, out var time) ? time : TimeSpan.Zero,
                LanguageVersion = txtLanguageVersion.Text,
                Is3D = chkIs3D.Checked,
                SoundSystem = txtSoundSystem.Text,
                AuditoriumId = int.TryParse(txtAuditoriumId.Text, out var audId) ? audId : 0
            };

            movie.Screenings.Add(screening);

            var success = await _movieManager.CreateMovieWithPosterAsync(movie, txtPosterUrl.Text);

            if (success)
            {
                MessageBox.Show("Film og forestilling oprettet!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            else
            {
                MessageBox.Show("Noget gik galt ved oprettelsen eller billedkopieringen.", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            txtTitle.Clear();
            txtGenre.Clear();
            txtDuration.Clear();
            txtDescription.Clear();
            txtLanguage.Clear();
            txtAgeRating.Clear();
            txtPosterUrl.Clear();
            dtpScreeningDate.Value = DateTime.Today;
            txtScreeningTime.Clear();
            txtLanguageVersion.Clear();
            chkIs3D.Checked = false;
            txtSoundSystem.Clear();
            txtAuditoriumId.Clear();
        }

        private void btnChoosePoster_Click(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog();
            dialog.Filter = "Billeder (*.jpg;*.jpeg;*.png)|*.jpg;*.jpeg;*.png|Alle filer (*.*)|*.*";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtPosterUrl.Text = dialog.FileName;
            }
        }
    }
}
