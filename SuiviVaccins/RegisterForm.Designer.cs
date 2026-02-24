namespace SuiviVaccins
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtNom = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnInscription = new System.Windows.Forms.Button();
            this.lblNom = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.labelTitre = new System.Windows.Forms.Label();
            this.SuspendLayout();

            this.labelTitre.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.labelTitre.ForeColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.labelTitre.Location = new System.Drawing.Point(50, 20);
            this.labelTitre.Name = "labelTitre";
            this.labelTitre.Size = new System.Drawing.Size(300, 40);
            this.labelTitre.TabIndex = 0;
            this.labelTitre.Text = "Nouvelle Maman";
            this.labelTitre.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            this.lblNom.AutoSize = true;
            this.lblNom.Location = new System.Drawing.Point(50, 80);
            this.lblNom.Name = "lblNom";
            this.lblNom.Size = new System.Drawing.Size(95, 17);
            this.lblNom.Text = "Nom complet :";

        
            this.txtNom.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtNom.Location = new System.Drawing.Point(50, 100);
            this.txtNom.Name = "txtNom";
            this.txtNom.Size = new System.Drawing.Size(300, 34);
            this.txtNom.TabIndex = 1;

   
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(50, 140);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(100, 17);
            this.lblEmail.Text = "Adresse Email :";

            this.txtEmail.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtEmail.Location = new System.Drawing.Point(50, 160);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(300, 34);
            this.txtEmail.TabIndex = 2;
 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(50, 200);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(102, 17);
            this.lblPassword.Text = "Mot de passe :";

            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPassword.Location = new System.Drawing.Point(50, 220);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(300, 34);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.UseSystemPasswordChar = true;

            this.btnInscription.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnInscription.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnInscription.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnInscription.ForeColor = System.Drawing.Color.White;
            this.btnInscription.Location = new System.Drawing.Point(50, 290);
            this.btnInscription.Name = "btnInscription";
            this.btnInscription.Size = new System.Drawing.Size(300, 45);
            this.btnInscription.TabIndex = 4;
            this.btnInscription.Text = "S\'INSCRIRE";
            this.btnInscription.UseVisualStyleBackColor = false;
            this.btnInscription.Click += new System.EventHandler(this.btnInscription_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 400);
            this.Controls.Add(this.labelTitre);
            this.Controls.Add(this.lblNom);
            this.Controls.Add(this.txtNom);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnInscription);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "RegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Création de compte";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtNom;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnInscription;
        private System.Windows.Forms.Label labelTitre;
        private System.Windows.Forms.Label lblNom;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPassword;
    }
}