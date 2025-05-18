using System;
using System.Windows.Forms;



namespace BioProjekt.ClientApp.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnMovies_Click(object sender, EventArgs e)
        {
            var movieForm = new MovieForm();
            movieForm.ShowDialog();
        }

        private void btnTickets_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Billetfunktionen er ikke implementeret endnu.", "Info");
        }
    }
}
