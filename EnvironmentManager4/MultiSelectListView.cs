using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EnvironmentManager4
{
    public class MultiSelectListView : ListView
    {
        // Create a list to store the selected items
        private List<ListViewItem> selectedItems = new List<ListViewItem>();

        // Override the OnMouseDown event to handle item selection
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            //create a list to contain the Text of all selected modules
            List<string> selectedModules = new List<string>();

            if (e.Button == MouseButtons.Left)
            {
                // Get the item at the clicked location
                ListViewItem item = this.GetItemAt(e.X, e.Y);
                if (item != null)
                {
                    // Check if the item is already selected
                    bool isSelected = selectedItems.Contains(item);

                    if (!isSelected)
                    {
                        // If the item is not selected, add it to the selected items list
                        item.Checked = true;
                        selectedItems.Add(item);
                    }
                    else
                    {
                        // If the item is already selected, remove it from the selected items list
                        item.Checked = false;
                        selectedItems.Remove(item);
                    }
                }
            }
        }
    }
}
