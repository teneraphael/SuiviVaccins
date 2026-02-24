using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SuiviVaccins
{
    public partial class Form1 : Form
    {
        private string connectionString = "server=127.0.0.1;database=gestion_vaccins;uid=root;pwd=;";
        private int idEnfantEnCours = -1;
        private System.Windows.Forms.Timer timerNotifications;

        public Form1()
        {
            InitializeComponent();
            ConfigurerTimerNotification();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (SessionMaman.IdMaman == 0)
            {
                MessageBox.Show("Session expirée. Veuillez vous reconnecter.");
                this.Close();
                return;
            }

            this.Text = "Ma Santé - Espace de " + SessionMaman.NomMaman;
            txtNomMaman.Text = SessionMaman.NomMaman;
            txtEmailMaman.Text = "Connecté(e)";

            txtNomMaman.ReadOnly = true;
            txtEmailMaman.ReadOnly = true;

            ChargerDonnees();

            // Premier scan au démarrage
            Task.Run(() => VerifierEtEnvoyerEmails());
        }

        // --- BACKEND : NOTIFICATIONS ---

        private void ConfigurerTimerNotification()
        {
            timerNotifications = new System.Windows.Forms.Timer();
            timerNotifications.Interval = 3600000; // 1 heure
            timerNotifications.Tick += (s, ev) => Task.Run(() => VerifierEtEnvoyerEmails());
            timerNotifications.Start();
        }

        private void VerifierEtEnvoyerEmails()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = @"SELECT v.id_vaccin, m.email_maman, e.nom_enfant, v.nom_vaccin, v.date_prevue 
                                   FROM vaccins v
                                   JOIN enfants e ON v.id_enfant = e.id_enfant
                                   JOIN mamans m ON e.id_maman = m.id_maman
                                   WHERE v.date_prevue = DATE_ADD(CURDATE(), INTERVAL 2 DAY)
                                   AND v.email_envoye = 0
                                   AND v.statut != 'Fait'";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    List<dynamic> listeEnvois = new List<dynamic>();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listeEnvois.Add(new
                            {
                                Id = dr.GetInt32("id_vaccin"),
                                Email = dr.GetString("email_maman"),
                                Enfant = dr.GetString("nom_enfant"),
                                Vaccin = dr.GetString("nom_vaccin"),
                                Date = dr.GetDateTime("date_prevue").ToShortDateString()
                            });
                        }
                    }

                    foreach (var item in listeEnvois)
                    {
                        if (EnvoyerEmailSMTP(item.Email, item.Enfant, item.Vaccin, item.Date))
                        {
                            MarquerCommeEnvoye(item.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log de l'erreur en mode debug si nécessaire
                    Console.WriteLine("Erreur Notification: " + ex.Message);
                }
            }
        }

        private void MarquerCommeEnvoye(int idVaccin)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string sql = "UPDATE vaccins SET email_envoye = 1 WHERE id_vaccin = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@id", idVaccin);
                    cmd.ExecuteNonQuery();
                }
                catch { }
            }
        }

        private bool EnvoyerEmailSMTP(string emailDest, string enfant, string vaccin, string dateV)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("teneraphael57@gmail.com", "VOTRE_CODE_16_LETTRES"), // Remplacez par votre mot de passe d'application
                    EnableSsl = true,
                };

                var mail = new MailMessage
                {
                    From = new MailAddress("teneraphael57@gmail.com", "Ma Santé - Rappel"),
                    Subject = "Rappel de Vaccination : " + enfant,
                    Body = $"Bonjour,\n\nCeci est un rappel automatique.\nLe vaccin '{vaccin}' de votre enfant {enfant} est prévu pour le {dateV}.\n\nCordialement.",
                };
                mail.To.Add(emailDest);
                smtpClient.Send(mail);
                return true;
            }
            catch { return false; }
        }

        // --- INTERFACE : GESTION DES DONNÉES ---

        private void ChargerDonnees()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT e.id_enfant, e.nom_enfant AS 'Enfant', 
                                            e.date_naissance AS 'Naissance',
                                            v.nom_vaccin AS 'Vaccin', 
                                            v.date_prevue AS 'Date Prévue',
                                            v.statut AS 'Statut'
                                     FROM vaccins v
                                     JOIN enfants e ON v.id_enfant = e.id_enfant
                                     WHERE e.id_maman = @idM
                                     ORDER BY e.nom_enfant, v.date_prevue ASC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idM", SessionMaman.IdMaman);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgvVaccins.DataSource = dt;
                    if (dgvVaccins.Columns.Contains("id_enfant"))
                        dgvVaccins.Columns["id_enfant"].Visible = false;

                    dgvVaccins.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                }
                catch (Exception ex) { MessageBox.Show("Erreur de chargement : " + ex.Message); }
            }
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomEnfant.Text))
            {
                MessageBox.Show("Veuillez saisir un nom.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (idEnfantEnCours == -1)
                    {
                        string sql = "INSERT INTO enfants (id_maman, nom_enfant, date_naissance) VALUES (@idM, @ne, @dn)";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@idM", SessionMaman.IdMaman);
                        cmd.Parameters.AddWithValue("@ne", txtNomEnfant.Text.Trim());
                        cmd.Parameters.AddWithValue("@dn", dtpNaissance.Value);
                        cmd.ExecuteNonQuery();

                        Planifier((int)cmd.LastInsertedId, dtpNaissance.Value, conn);
                        MessageBox.Show("Enfant et vaccins enregistrés !");
                    }
                    else
                    {
                        string sql = "UPDATE enfants SET nom_enfant=@ne, date_naissance=@dn WHERE id_enfant=@idE";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@ne", txtNomEnfant.Text.Trim());
                        cmd.Parameters.AddWithValue("@dn", dtpNaissance.Value);
                        cmd.Parameters.AddWithValue("@idE", idEnfantEnCours);
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Modifications enregistrées.");
                    }
                    ViderFormulaire();
                    ChargerDonnees();
                }
                catch (Exception ex) { MessageBox.Show("Erreur : " + ex.Message); }
            }
        }

        private void Planifier(int idE, DateTime naissance, MySqlConnection conn)
        {
            var programme = new List<Tuple<string, int>> {
                new Tuple<string, int>("BCG", 0),
                new Tuple<string, int>("PENTA-1", 2),
                new Tuple<string, int>("PENTA-2", 3),
                new Tuple<string, int>("PENTA-3", 4),
                new Tuple<string, int>("ROUGEOLE", 9)
            };

            foreach (var v in programme)
            {
                string sql = "INSERT INTO vaccins (id_enfant, nom_vaccin, date_prevue, statut, email_envoye) VALUES (@e, @n, @d, 'En attente', 0)";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@e", idE);
                cmd.Parameters.AddWithValue("@n", v.Item1);
                cmd.Parameters.AddWithValue("@d", naissance.AddMonths(v.Item2));
                cmd.ExecuteNonQuery();
            }
        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvVaccins.CurrentRow == null) return;

            idEnfantEnCours = Convert.ToInt32(dgvVaccins.CurrentRow.Cells["id_enfant"].Value);
            txtNomEnfant.Text = dgvVaccins.CurrentRow.Cells["Enfant"].Value.ToString();
            dtpNaissance.Value = Convert.ToDateTime(dgvVaccins.CurrentRow.Cells["Naissance"].Value);

            btnEnregistrer.Text = "MODIFIER";
            btnEnregistrer.BackColor = Color.Orange;
            tabControl1.SelectedIndex = 0;
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvVaccins.CurrentRow == null) return;

            int idE = Convert.ToInt32(dgvVaccins.CurrentRow.Cells["id_enfant"].Value);
            if (MessageBox.Show("Supprimer cet enfant et son suivi ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string sql = "DELETE FROM enfants WHERE id_enfant = @id";
                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        cmd.Parameters.AddWithValue("@id", idE);
                        cmd.ExecuteNonQuery();
                        ChargerDonnees();
                    }
                    catch (Exception ex) { MessageBox.Show("Erreur suppression: " + ex.Message); }
                }
            }
        }

        private void btnNouveau_Click(object sender, EventArgs e)
        {
            ViderFormulaire();
        }

        private void btnActualiser_Click(object sender, EventArgs e)
        {
            ChargerDonnees();
        }

        private void ViderFormulaire()
        {
            txtNomEnfant.Clear();
            dtpNaissance.Value = DateTime.Now;
            idEnfantEnCours = -1;
            btnEnregistrer.Text = "SAUVEGARDER";
            btnEnregistrer.BackColor = Color.SeaGreen;
        }
    }
}