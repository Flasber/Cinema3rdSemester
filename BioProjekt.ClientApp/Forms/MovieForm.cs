using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using BioProjekt.ClientApp.Forms.Services;
using BioProjekt.Shared.ClientDtos;

namespace BioProjekt.ClientApp.Forms
{
    public partial class MovieForm : Form
    {
        private readonly MovieApiClient _apiClient;

        private Dictionary<int, (string SoundSystem, bool Has3D)> _auditoriumDefaults = new()
        {
            { 1, ("Dolby Atmos", true) },
            { 2, ("Stereo", false) }
        };

        public MovieForm()
        {
            InitializeComponent();
            _apiClient = new MovieApiClient();

            cmbAuditorium.Items.Add("1 - Auditorium 1");
            cmbAuditorium.Items.Add("2 - Auditorium 2");
            cmbAuditorium.SelectedIndex = 0;
            ApplyAuditoriumDefaults(1);
        }

        private void cmbAuditorium_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAuditorium.SelectedIndex == -1)
                return;

            int selectedId = cmbAuditorium.SelectedIndex + 1;
            ApplyAuditoriumDefaults(selectedId);
        }

        private void ApplyAuditoriumDefaults(int auditoriumId)
        {
            if (_auditoriumDefaults.TryGetValue(auditoriumId, out var data))
            {
                txtSoundSystem.Text = data.SoundSystem;
                chkIs3D.Checked = data.Has3D;
            }
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
                AgeRating = txtAgeRating.Text,
            };

            int auditoriumId = cmbAuditorium.SelectedIndex + 1;

            var screening = new ScreeningCreateDto
            {
                Date = dtpScreeningDate.Value.Date,
                Time = TimeSpan.TryParse(txtScreeningTime.Text, out var time) ? time : TimeSpan.Zero,
                LanguageVersion = txtLanguageVersion.Text,
                Is3D = chkIs3D.Checked,
                SoundSystem = txtSoundSystem.Text,
                AuditoriumId = auditoriumId
            };

            movie.Screenings.Add(screening);

            var posterUrl = await _apiClient.UploadPosterAsync(txtPosterUrl.Text);
            if (posterUrl == null)
            {
                MessageBox.Show("Fejl ved upload af plakat.", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            movie.PosterUrl = posterUrl;

            var success = await _apiClient.CreateMovieAsync(movie);

            if (success)
            {
                MessageBox.Show("Film og forestilling oprettet!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            else
            {
                MessageBox.Show("Noget gik galt ved oprettelsen.", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChoosePoster_Click(object sender, EventArgs e)
        {
            using var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Billeder (*.jpg;*.png)|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                txtPosterUrl.Text = openFileDialog.FileName;
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
            cmbAuditorium.SelectedIndex = 0;
        }
    }
}
