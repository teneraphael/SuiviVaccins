using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SuiviVaccins
{
    public partial class RegisterForm : Form
    {
     
        private string connectionString = "server=127.0.0.1;database=gestion_vaccins;uid=root;pwd=;";

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnInscription_Click(object sender, EventArgs e)
        {
       
            if (string.IsNullOrWhiteSpace(txtNom.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs, y compris le mot de passe.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    string checkSql = "SELECT COUNT(*) FROM mamans WHERE email_maman = @email";
                    MySqlCommand checkCmd = new MySqlCommand(checkSql, conn);
                    checkCmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());

                    long count = (long)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        MessageBox.Show("Cet email est déjà utilisé par un autre compte.");
                        return;
                    }

                    string sql = "INSERT INTO mamans (nom_maman, email_maman, mot_de_passe) VALUES (@nom, @email, @pw)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
         
                        cmd.Parameters.AddWithValue("@nom", txtNom.Text.Trim());
                        cmd.Parameters.AddWithValue("@email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@pw", txtPassword.Text); 

                        int resultat = cmd.ExecuteNonQuery();

                        if (resultat > 0)
                        {
                            MessageBox.Show("Inscription réussie ! Bienvenue parmis nous.");

                            this.DialogResult = DialogResult.OK; 
                            this.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de l'inscription : " + ex.Message);
                }
            }
        }

        private void btnRetourLogin_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}