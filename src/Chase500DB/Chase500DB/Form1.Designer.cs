namespace Chase500DB
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.bCrearDoc = new System.Windows.Forms.Button();
            this.cbxProject = new System.Windows.Forms.ComboBox();
            this.projectsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.chase500DataSet = new Chase500DB.chase500DataSet();
            this.projectsTableAdapter = new Chase500DB.chase500DataSetTableAdapters.projectsTableAdapter();
            this.observationsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.observationsTableAdapter = new Chase500DB.chase500DataSetTableAdapters.observationsTableAdapter();
            this.bGetId = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chase500DataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.observationsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // bCrearDoc
            // 
            this.bCrearDoc.Location = new System.Drawing.Point(172, 10);
            this.bCrearDoc.Name = "bCrearDoc";
            this.bCrearDoc.Size = new System.Drawing.Size(108, 23);
            this.bCrearDoc.TabIndex = 0;
            this.bCrearDoc.Text = "Crea Documento";
            this.bCrearDoc.UseVisualStyleBackColor = true;
            this.bCrearDoc.Click += new System.EventHandler(this.bCrearDoc_Click);
            // 
            // cbxProject
            // 
            this.cbxProject.DataSource = this.projectsBindingSource;
            this.cbxProject.DisplayMember = "name";
            this.cbxProject.FormattingEnabled = true;
            this.cbxProject.Location = new System.Drawing.Point(12, 12);
            this.cbxProject.Name = "cbxProject";
            this.cbxProject.Size = new System.Drawing.Size(154, 21);
            this.cbxProject.TabIndex = 1;
            this.cbxProject.ValueMember = "id";
            // 
            // projectsBindingSource
            // 
            this.projectsBindingSource.DataMember = "projects";
            this.projectsBindingSource.DataSource = this.chase500DataSet;
            // 
            // chase500DataSet
            // 
            this.chase500DataSet.DataSetName = "chase500DataSet";
            this.chase500DataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // projectsTableAdapter
            // 
            this.projectsTableAdapter.ClearBeforeFill = true;
            // 
            // observationsBindingSource
            // 
            this.observationsBindingSource.DataMember = "observations";
            this.observationsBindingSource.DataSource = this.chase500DataSet;
            // 
            // observationsTableAdapter
            // 
            this.observationsTableAdapter.ClearBeforeFill = true;
            // 
            // bGetId
            // 
            this.bGetId.Location = new System.Drawing.Point(27, 59);
            this.bGetId.Name = "bGetId";
            this.bGetId.Size = new System.Drawing.Size(75, 23);
            this.bGetId.TabIndex = 2;
            this.bGetId.Text = "button1";
            this.bGetId.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 93);
            this.Controls.Add(this.bGetId);
            this.Controls.Add(this.cbxProject);
            this.Controls.Add(this.bCrearDoc);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "DataBase to2 RTML2.3";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.projectsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chase500DataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.observationsBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bCrearDoc;
        private System.Windows.Forms.ComboBox cbxProject;
        private chase500DataSet chase500DataSet;
        private System.Windows.Forms.BindingSource projectsBindingSource;
        private Chase500DB.chase500DataSetTableAdapters.projectsTableAdapter projectsTableAdapter;
        private System.Windows.Forms.BindingSource observationsBindingSource;
        private Chase500DB.chase500DataSetTableAdapters.observationsTableAdapter observationsTableAdapter;
        private System.Windows.Forms.Button bGetId;
    }
}

