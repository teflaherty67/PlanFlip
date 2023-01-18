using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanFlip
{
    public partial class ElevationSwapForm1 : System.Windows.Forms.Form
    {
        public ElevationSwapForm1(List<ViewSheet> elevSheets)
        {
            InitializeComponent();

            checkBox1.Checked = false;

            foreach(ViewSheet sheet in elevSheets)
            {
                comboBox1.Items.Add(sheet.SheetNumber);
                comboBox2.Items.Add(sheet.SheetNumber);
                comboBox3.Items.Add(sheet.SheetNumber);
                comboBox4.Items.Add(sheet.SheetNumber);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            // if checked show bottom combo boxes

            // not then hide bottom combo boxes

            CheckBox cBox = (CheckBox)sender;

            if(cBox.Checked == true) { comboBox3.Visible = true; comboBox4.Visible = true; }
            else { comboBox3.Visible = false; comboBox4.Visible = false; }
        }

        internal object GetComboBox1Item(System.Windows.Forms.Form curForm)
        {
            return comboBox1.SelectedItem;
        }

        private object GetComboBox2Item(System.Windows.Forms.Form curForm)
        {
            return comboBox2.SelectedItem;
        }

        private object GetComboBox3Item(System.Windows.Forms.Form curForm)
        {
            return comboBox3.SelectedItem;
        }

        private object GetComboBox4Item(System.Windows.Forms.Form curForm)
        {
            return comboBox4.SelectedItem;
        }
    }
}
