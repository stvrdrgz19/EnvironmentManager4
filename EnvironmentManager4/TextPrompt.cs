﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public partial class TextPrompt : Form
    {
        public TextPrompt()
        {
            InitializeComponent();
        }

        public static string title = "";
        public static string label = "";
        public static string output = "";
        public static bool isConfiguration = false;
        public static string product = "";

        private void TextPrompt_Load(object sender, EventArgs e)
        {
            this.Text = title;
            labelPrompt.Text = label;
            return;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            output = tbText.Text;
            if (isConfiguration)
            {
                if (Configuration.DoesConfigurationExist(product, output))
                {
                    string message = String.Format("A configuration with the name '{0}' already exists for the {1} configuration list. Please enter a new name.", output, product);
                    string caption = "ERROR";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    MessageBoxIcon icon = MessageBoxIcon.Error;
                    MessageBox.Show(message, caption, buttons, icon);
                    return;
                }
                //create configuration
            }
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }
    }
}
