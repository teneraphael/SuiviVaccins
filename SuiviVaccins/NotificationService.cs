using System;
using System.Net;
using System.Net.Mail;
using MySql.Data.MySqlClient;

public class EmailService
{
    private string connString = "server=127.0.0.1;database=gestion_vaccins;uid=root;pwd=;";

    public void VerifierEtEnvoyer()
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            try
            {
                conn.Open();
                string sql = @"SELECT v.id_vaccin, m.email_maman, e.nom_enfant, v.nom_vaccin 
                               FROM vaccins v 
                               JOIN enfants e ON v.id_enfant = e.id_enfant 
                               JOIN mamans m ON e.id_maman = m.id_maman 
                               WHERE v.date_prevue = DATE_ADD(CURDATE(), INTERVAL 2 DAY) 
                               AND v.email_envoye = 0";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                using (MySqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        SendMail(dr.GetString("email_maman"), dr.GetString("nom_enfant"), dr.GetString("nom_vaccin"));
                        MarquerCommeEnvoye(dr.GetInt32("id_vaccin"));
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex);
                    }
        }
    }

    private void SendMail(string email, string enfant, string vaccin)
    {
        var client = new SmtpClient("smtp.gmail.com", 587)
        {
            Credentials = new NetworkCredential("teneraphael57@gmail.com", "qlke vecn ogua ibrr "),
            EnableSsl = true
        };
        client.Send("teneraphael57@gmail.com", email, "Rappel Vaccin", $"Le vaccin {vaccin} de {enfant} est dans 2 jours.");
    }

    private void MarquerCommeEnvoye(int id)
    {
        using (MySqlConnection conn = new MySqlConnection(connString))
        {
            conn.Open();
            new MySqlCommand($"UPDATE vaccins SET email_envoye = 1 WHERE id_vaccin = {id}", conn).ExecuteNonQuery();
        }
    }
}