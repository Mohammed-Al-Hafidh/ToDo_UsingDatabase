using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToDo_UsingDatabase
{
    public partial class MainWindow : Window
    {
        public string fileName = "..\\..\\mydata.txt";
        List<Task> tasks = new List<Task>();
        TaskDatabaseContext ctx = new TaskDatabaseContext();
        public MainWindow()
        {
            InitializeComponent();           
            LoadDate();
        }
        public void LoadDate()
        {            
            tasks = ctx.Tasks.ToList<Task>();

            lvList.ItemsSource = tasks;
        }             

        private void lvList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnDelete.IsEnabled = true;
            btnUpdate.IsEnabled = true;

            var selectedItem = lvList.SelectedItem;
            if (selectedItem is Task)
            {
                Task task = (Task)lvList.SelectedItem;
                txtTask.Text = task.TaskName;
                slDifficulty.Value = task.Difficulty;            
                dpDueDate.SelectedDate = DateTime.Parse(task.DueDate);
                comStatus.Text = task.Status;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if ((txtTask.Text.Trim() == "") || (dpDueDate.SelectedDate.ToString().Trim() == ""))
            {
                MessageBox.Show("Input string error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task task = new Task(txtTask.Text, dpDueDate.Text.ToString(), int.Parse(slDifficulty.Value.ToString()), comStatus.Text);            
            tasks.Add(task);           
            ResetValue();

            //Save to datebase
            ctx.Tasks.Add(task);
            ctx.SaveChanges();
        }

        private void ResetValue()
        {
            lvList.Items.Refresh();
            txtTask.Clear();
            dpDueDate.Text = "";
            slDifficulty.Value = 1;
            comStatus.Text = "";
            lvList.SelectedIndex = -1;
            btnDelete.IsEnabled = false;
            btnUpdate.IsEnabled = false;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lvList.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task taskToBeDeleted = (Task)lvList.SelectedItem;
            if (taskToBeDeleted != null)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure?", "CONFIRMATION", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    tasks.Remove(taskToBeDeleted);
                    ResetValue();

                    //Remove from database
                    ctx.Tasks.Remove(taskToBeDeleted);
                    ctx.SaveChanges();

                }

            }

        }

        

        private void btnExportToFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text file (*.txt)|*.txt";
            if (dialog.ShowDialog() == true)
            {
                string allData = "";
                foreach (Task myTaskk in tasks)
                {
                    allData += myTaskk.ToString() + "\n";
                }
                File.WriteAllText(dialog.FileName, allData);
            }
            else
            {
                MessageBox.Show("File error", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (lvList.SelectedIndex == -1)
            {
                MessageBox.Show("You need to select one item", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ((txtTask.Text == "") || (dpDueDate.SelectedDate.ToString().Trim() == ""))
            {
                MessageBox.Show("Input string error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            Task tasktoBeUpdated = (Task)lvList.SelectedItem;
            tasktoBeUpdated.TaskName = txtTask.Text;
            tasktoBeUpdated.Difficulty = int.Parse(slDifficulty.Value.ToString());
            tasktoBeUpdated.DueDate = dpDueDate.Text;
            tasktoBeUpdated.Status = comStatus.Text;
            ResetValue();
            int id = tasktoBeUpdated.Id;
            //Update in datebase
            Task task = ctx.Tasks.Where(p => p.Id == id).FirstOrDefault<Task>();
            if (task != null)
            {
                task.TaskName += txtTask.Text;
                task.Difficulty += int.Parse(slDifficulty.Value.ToString());
                task.DueDate += dpDueDate.Text;
                task.Status += comStatus.Text;
                ctx.SaveChanges();
            }
        }

        private void lvList_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListBoxItem))
                lvList.UnselectAll();
        }
    }
}
