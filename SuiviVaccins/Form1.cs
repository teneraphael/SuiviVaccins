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
            
            Task.Run(() => VerifierEtEnvoyerEmails());
        }

        private void ConfigurerTimerNotification()
        {
            timerNotifications = new System.Windows.Forms.Timer();
            timerNotifications.Interval = 3600000;
            timerNotifications.Tick += (s, e) => Task.Run(() => VerifierEtEnvoyerEmails());
            timerNotifications.Start();
        }

        private void VerifierEtEnvoyerEmails()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                   
                    string sql = @"SELECT s.id_suivi, m.email_maman, e.nom_enfant, t.nom_vaccin, s.date_prevue 
                                   FROM suivi_vaccins s
                                   JOIN enfants e ON s.id_enfant = e.id_enfant
                                   JOIN mamans m ON e.id_maman = m.id_maman
                                   JOIN type_vaccins t ON s.id_type = t.id_type
                                   WHERE s.date_prevue = DATE_ADD(CURDATE(), INTERVAL 2 DAY)
                                   AND s.email_envoye = 0";

                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    List<dynamic> listeEnvois = new List<dynamic>();

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listeEnvois.Add(new
                            {
                                Id = dr.GetInt32("id_suivi"),
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
                            MarquerEmailCommeEnvoye(item.Id);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

        private bool EnvoyerEmailSMTP(string emailDest, string enfant, string vaccin, string dateV)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("teneraphael57@gmail.com", "XXXXXXXXXXXXXXXX"),
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

        private void MarquerEmailCommeEnvoye(int idSuivi)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string sql = "UPDATE suivi_vaccins SET email_envoye = 1 WHERE id_suivi = @id";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", idSuivi);
                cmd.ExecuteNonQuery();
            }
        }

        private void ChargerDonnees()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"SELECT e.id_enfant, e.nom_enfant AS 'Enfant', 
                                            e.date_naissance AS 'Naissance',
                                            t.nom_vaccin AS 'Vaccin', 
                                            s.date_prevue AS 'Date Prévue'
                                     FROM suivi_vaccins s
                                     JOIN enfants e ON s.id_enfant = e.id_enfant
                                     JOIN type_vaccins t ON s.id_type = t.id_type
                                     WHERE e.id_maman = @idM
                                     ORDER BY e.nom_enfant, s.date_prevue ASC";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@idM", SessionMaman.IdMaman);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dgvVaccins != null)
                    {
                        dgvVaccins.DataSource = dt;
                        if (dgvVaccins.Columns.Contains("id_enfant"))
                            dgvVaccins.Columns["id_enfant"].Visible = false;

                        dgvVaccins.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur de chargement : " + ex.Message);
                }
            }
        }

        private void btnEnregistrer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNomEnfant.Text))
            {
                MessageBox.Show("Veuillez saisir le nom de l'enfant.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    using (MySqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            if (idEnfantEnCours == -1)
                            {
                                string sqlE = "INSERT INTO enfants (id_maman, nom_enfant, date_naissance) VALUES (@idM, @ne, @dn)";
                                MySqlCommand cmdE = new MySqlCommand(sqlE, conn, trans);
                                cmdE.Parameters.AddWithValue("@idM", SessionMaman.IdMaman);
                                cmdE.Parameters.AddWithValue("@ne", txtNomEnfant.Text.Trim());
                                cmdE.Parameters.AddWithValue("@dn", dtpNaissance.Value);
                                cmdE.ExecuteNonQuery();

                                int idE = (int)cmdE.LastInsertedId;
                                Planifier(idE, dtpNaissance.Value, conn, trans);

                                trans.Commit();
                                MessageBox.Show("Enfant ajouté et calendrier généré !");
                            }
                            else
                            {
                                string sqlU = "UPDATE enfants SET nom_enfant=@ne, date_naissance=@dn WHERE id_enfant=@idE";
                                MySqlCommand cmdU = new MySqlCommand(sqlU, conn, trans);
                                cmdU.Parameters.AddWithValue("@ne", txtNomEnfant.Text.Trim());
                                cmdU.Parameters.AddWithValue("@dn", dtpNaissance.Value);
                                cmdU.Parameters.AddWithValue("@idE", idEnfantEnCours);
                                cmdU.ExecuteNonQuery();

                                trans.Commit();
                                MessageBox.Show("Modifications enregistrées !");
                            }

                            ViderFormulaire();
                            ChargerDonnees();
                            tabControl1.SelectedIndex = 1;
                        }
                        catch (Exception) { trans.Rollback(); throw; }
                    }
                }
                catch (Exception ex) { MessageBox.Show("Erreur : " + ex.Message); }
            }
        }

        private void Planifier(int idE, DateTime naissance, MySqlConnection c, MySqlTransaction t)
        {
            List<Tuple<int, int>> types = new List<Tuple<int, int>>();
            using (MySqlCommand cmd = new MySqlCommand("SELECT id_type, age_mois FROM type_vaccins", c, t))
            {
                using (MySqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read())
                        types.Add(new Tuple<int, int>(r.GetInt32(0), r.GetInt32(1)));
                }
            }

            foreach (var v in types)
            {
                string sqlIns = "INSERT INTO suivi_vaccins (id_enfant, id_type, date_prevue, email_envoye) VALUES (@e, @t, @d, 0)";
                using (MySqlCommand ins = new MySqlCommand(sqlIns, c, t))
                {
                    ins.Parameters.AddWithValue("@e", idE);
                    ins.Parameters.AddWithValue("@t", v.Item1);
                    ins.Parameters.AddWithValue("@d", naissance.AddMonths(v.Item2));
                    ins.ExecuteNonQuery();
                }
            }
        }

        private void modifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvVaccins.CurrentRow == null) return;

            idEnfantEnCours = Convert.ToInt32(dgvVaccins.CurrentRow.Cells["id_enfant"].Value);
            txtNomEnfant.Text = dgvVaccins.CurrentRow.Cells["Enfant"].Value.ToString();
            dtpNaissance.Value = Convert.ToDateTime(dgvVaccins.CurrentRow.Cells["Naissance"].Value);

            btnEnregistrer.Text = "METTRE À JOUR";
            btnEnregistrer.BackColor = Color.DarkOrange;
            tabControl1.SelectedIndex = 0;
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvVaccins.CurrentRow == null) return;
            int idE = Convert.ToInt32(dgvVaccins.CurrentRow.Cells["id_enfant"].Value);

            if (MessageBox.Show("Supprimer cet enfant et son calendrier ?", "Confirmation", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand("DELETE FROM enfants WHERE id_enfant = @id", conn);
                    cmd.Parameters.AddWithValue("@id", idE);
                    cmd.ExecuteNonQuery();
                    ChargerDonnees();
                }
            }
        }

        private void btnNouveau_Click(object sender, EventArgs e) { ViderFormulaire(); }
        private void btnActualiser_Click(object sender, EventArgs e) { ChargerDonnees(); }

        private void ViderFormulaire()
        {
            txtNomEnfant.Clear();
            dtpNaissance.Value = DateTime.Now;
            idEnfantEnCours = -1;
            btnEnregistrer.Text = "SAUVEGARDER";
            btnEnregistrer.BackColor = Color.FromArgb(40, 167, 69);
        }
    }
}