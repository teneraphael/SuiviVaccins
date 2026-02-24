using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace SuiviVaccins
{
    public partial class LoginForm : Form
    {
        private string connectionString = "server=127.0.0.1;database=gestion_vaccins;uid=root;pwd=;";

        public LoginForm()
        {
            InitializeComponent();
      
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void btnConnexion_Click(object sender, EventArgs e)
        {
          
            if (string.IsNullOrWhiteSpace(txtEmail.Text) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Veuillez saisir votre email et votre mot de passe.", "Champs vides", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                
                    string sql = "SELECT id_maman, nom_maman FROM mamans WHERE email_maman=@em AND mot_de_passe=@pw";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@em", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@pw", txtPassword.Text);

                        using (MySqlDataReader dr = cmd.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                            
                                SessionMaman.IdMaman = dr.GetInt32("id_maman");
                                SessionMaman.NomMaman = dr.GetString("nom_maman");

                               
                                Form1 dash = new Form1();
                                dash.Show();

                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Email ou mot de passe incorrect.", "Erreur d'authentification", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                txtPassword.Clear(); 
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Impossible de se connecter à la base de données : " + ex.Message, "Erreur de connexion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void lblCreerCompte_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
   
            RegisterForm reg = new RegisterForm();
            reg.ShowDialog();
        }
    }
}