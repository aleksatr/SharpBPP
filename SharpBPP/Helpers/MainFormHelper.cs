﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBPP.Helpers
{
    public class MainFormHelper
    {
        public static string DialogCombo(string caption, string text, List<string> comboSource/*, string DisplyMember, string ValueMember*/)
        {
            Form prompt = new Form();
            //prompt.RightToLeft = RightToLeft.Yes;
            prompt.Width = 500;
            prompt.Height = 200;
            prompt.FormBorderStyle = FormBorderStyle.FixedSingle;
            prompt.StartPosition = FormStartPosition.CenterScreen;
            prompt.Text = caption;
            prompt.MaximizeBox = false;
            prompt.MinimizeBox = false;
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            ComboBox combo = new ComboBox { Left = 50, Top = 50, Width = 400 };
            combo.DropDownStyle = ComboBoxStyle.DropDownList;
            combo.DataSource = comboSource;
            //combo.ValueMember = ValueMember;
            //combo.DisplayMember = DisplyMember;
            Button confirmation = new Button() { Text = "OK", Left = 350, Width = 100, Top = 80 };
            confirmation.Click += (sender, e) => { prompt.DialogResult = DialogResult.OK; prompt.Close(); };
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(combo);
            DialogResult res = prompt.ShowDialog();

            if (res == DialogResult.OK)
                return combo.SelectedItem.ToString();
            else
                return null;
        }
    }
}