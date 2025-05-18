using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BioProjekt.ClientApp.Forms.Services;
using BioProjekt.Shared.ClientDtos;

namespace BioProjekt.ClientApp.Forms
{
    public partial class MovieForm : Form
    {
        private readonly MovieApiClient _movieApiClient;

        public MovieForm()
        {
            InitializeComponent();
            _movieApiClient = new MovieApiClient();
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
                PosterUrl = txtPosterUrl.Text
            };

            var success = await _movieApiClient.CreateMovieAsync(movie);

            if (success)
            {
                MessageBox.Show("Film oprettet!", "Succes", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm();
            }
            else
            {
                MessageBox.Show("Noget gik galt ved oprettelse af filmen.", "Fejl", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }
    }
}