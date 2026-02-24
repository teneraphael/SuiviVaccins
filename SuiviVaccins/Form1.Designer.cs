namespace SuiviVaccins
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabInscription = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNouveau = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnEnregistrer = new System.Windows.Forms.Button();
            this.dtpNaissance = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.txtNomEnfant = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEmailMaman = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNomMaman = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabConsultation = new System.Windows.Forms.TabPage();
            this.dgvVaccins = new System.Windows.Forms.DataGridView();
            this.cmsOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.supprimerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnActualiser = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();

            this.tabControl1.SuspendLayout();
            this.tabInscription.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabConsultation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVaccins)).BeginInit();
            this.cmsOptions.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();

          
            this.tabControl1.Controls.Add(this.tabInscription);
            this.tabControl1.Controls.Add(this.tabConsultation);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(950, 600);
            this.tabControl1.TabIndex = 0;

            
            this.tabInscription.Controls.Add(this.panel1);
            this.tabInscription.Text = "➕ Gérer mes Enfants";
            this.tabInscription.BackColor = System.Drawing.Color.WhiteSmoke;

           
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnNouveau);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.btnEnregistrer);
            this.panel1.Controls.Add(this.dtpNaissance);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtNomEnfant);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtEmailMaman);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtNomMaman);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(225, 40);
            this.panel1.Size = new System.Drawing.Size(500, 480);
            
            this.label5.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(0, 123, 255);
            this.label5.Location = new System.Drawing.Point(30, 20);
            this.label5.Size = new System.Drawing.Size(350, 45);
            this.label5.Text = "Profil de l'enfant";
            
            this.btnNouveau.Location = new System.Drawing.Point(370, 25);
            this.btnNouveau.Size = new System.Drawing.Size(100, 30);
            this.btnNouveau.Text = "Vider / Nouveau";
            this.btnNouveau.Click += new System.EventHandler(this.btnNouveau_Click);
            
            this.label1.Text = "Nom de la Maman:";
            this.label1.Location = new System.Drawing.Point(50, 80);
            this.txtNomMaman.Location = new System.Drawing.Point(50, 105);
            this.txtNomMaman.Size = new System.Drawing.Size(400, 30);

            this.label2.Text = "Email Contact:";
            this.label2.Location = new System.Drawing.Point(50, 145);
            this.txtEmailMaman.Location = new System.Drawing.Point(50, 170);
            this.txtEmailMaman.Size = new System.Drawing.Size(400, 30);
            
            this.label3.Text = "Nom de l'Enfant:";
            this.label3.Location = new System.Drawing.Point(50, 215);
            this.txtNomEnfant.Location = new System.Drawing.Point(50, 240);
            this.txtNomEnfant.Size = new System.Drawing.Size(400, 30);

            this.label4.Text = "Date de Naissance:";
            this.label4.Location = new System.Drawing.Point(50, 285);
            this.dtpNaissance.Location = new System.Drawing.Point(50, 310);
            this.dtpNaissance.Size = new System.Drawing.Size(400, 30);
            
            this.btnEnregistrer.BackColor = System.Drawing.Color.FromArgb(40, 167, 69);
            this.btnEnregistrer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEnregistrer.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnEnregistrer.ForeColor = System.Drawing.Color.White;
            this.btnEnregistrer.Location = new System.Drawing.Point(100, 380);
            this.btnEnregistrer.Size = new System.Drawing.Size(300, 50);
            this.btnEnregistrer.Text = "SAUVEGARDER";
            this.btnEnregistrer.Click += new System.EventHandler(this.btnEnregistrer_Click);

            this.tabConsultation.Controls.Add(this.dgvVaccins);
            this.tabConsultation.Controls.Add(this.panelHeader);
            this.tabConsultation.Text = "📊 Mon Tableau de Bord";

   
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.panelHeader.Controls.Add(this.btnActualiser);
            this.panelHeader.Controls.Add(this.label6);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Height = 64;

            this.label6.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(20, 15);
            this.label6.Size = new System.Drawing.Size(500, 35);
            this.label6.Text = "Liste de mes enfants et vaccins";

            this.btnActualiser.Location = new System.Drawing.Point(780, 15);
            this.btnActualiser.Size = new System.Drawing.Size(140, 35);
            this.btnActualiser.Text = "Rafraîchir 🔄";
            this.btnActualiser.Click += new System.EventHandler(this.btnActualiser_Click);

            this.dgvVaccins.AllowUserToAddRows = false;
            this.dgvVaccins.BackgroundColor = System.Drawing.Color.White;
            this.dgvVaccins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVaccins.ContextMenuStrip = this.cmsOptions;
            this.dgvVaccins.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVaccins.ReadOnly = true;
            this.dgvVaccins.RowHeadersVisible = false;
            this.dgvVaccins.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            this.cmsOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifierToolStripMenuItem,
            this.supprimerToolStripMenuItem});
            this.cmsOptions.Size = new System.Drawing.Size(175, 48);

            this.modifierToolStripMenuItem.Text = "Modifier l'enfant";
            this.modifierToolStripMenuItem.Click += new System.EventHandler(this.modifierToolStripMenuItem_Click);

            this.supprimerToolStripMenuItem.Text = "Supprimer l'enfant";
            this.supprimerToolStripMenuItem.Click += new System.EventHandler(this.supprimerToolStripMenuItem_Click);

            this.ClientSize = new System.Drawing.Size(950, 600);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ma Santé - Espace Maman";
            this.Load += new System.EventHandler(this.Form1_Load);

            this.tabControl1.ResumeLayout(false);
            this.tabInscription.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabConsultation.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVaccins)).EndInit();
            this.cmsOptions.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);
        }
        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabInscription;
        private System.Windows.Forms.TabPage tabConsultation;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnEnregistrer;
        private System.Windows.Forms.DateTimePicker dtpNaissance;
        private System.Windows.Forms.TextBox txtNomEnfant;
        private System.Windows.Forms.TextBox txtEmailMaman;
        private System.Windows.Forms.TextBox txtNomMaman;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridView dgvVaccins;
        private System.Windows.Forms.ContextMenuStrip cmsOptions;
        private System.Windows.Forms.ToolStripMenuItem modifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem supprimerToolStripMenuItem;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnActualiser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnNouveau;
    }
}