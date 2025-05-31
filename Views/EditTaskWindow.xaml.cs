using System.Windows;
using TaskManager.Models;

namespace TaskManager.Views
{
    public partial class EditTaskWindow : Window
    {
        public EditTaskWindow(UserTask taskToEdit)
        {
            InitializeComponent();
            this.DataContext = taskToEdit;
            UserTask task = (UserTask)this.DataContext;

            if (task.CompletionDate.HasValue)
            {
                EnableCompletionDateCheckBox.IsChecked = true;
                CompletionDatePickerEdit.SelectedDate = task.CompletionDate.Value.Date;
                SpecifyTimeCheckBoxEdit.IsChecked = task.HasCompletionTime;

                if (task.HasCompletionTime)
                {
                    HourTextBoxEdit.Text = task.CompletionDate.Value.Hour.ToString("D2");
                    MinuteTextBoxEdit.Text = task.CompletionDate.Value.Minute.ToString("D2");
                }
                else
                {
                    HourTextBoxEdit.Text = "00";
                    MinuteTextBoxEdit.Text = "00";
                }
            }
            else
            {
                EnableCompletionDateCheckBox.IsChecked = false;
                SpecifyTimeCheckBoxEdit.IsChecked = false;
                CompletionDatePickerEdit.SelectedDate = null;
                HourTextBoxEdit.Text = "00";
                MinuteTextBoxEdit.Text = "00";
            }
        }

        // Salva as alterações feitas na tarefa, caso ocorra.
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UserTask taskToSave = (UserTask)this.DataContext;

            if (string.IsNullOrWhiteSpace(taskToSave.Title))
            {
                MessageBox.Show("O título da tarefa é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleTextBoxEdit.Focus();
                return;
            }

            taskToSave.HasCompletionTime = SpecifyTimeCheckBoxEdit.IsChecked == true;

            if (EnableCompletionDateCheckBox.IsChecked == true)
            {
                taskToSave.HasCompletionTime = SpecifyTimeCheckBoxEdit.IsChecked == true;

                if (CompletionDatePickerEdit.SelectedDate.HasValue)
                {
                    DateTime selectedDateOnly = CompletionDatePickerEdit.SelectedDate.Value.Date;
                    if (taskToSave.HasCompletionTime) 
                    {
                        int hour = 0; int minute = 0;
                        bool hourValid = int.TryParse(HourTextBoxEdit.Text, out hour) && hour >= 0 && hour <= 23;
                        bool minuteValid = int.TryParse(MinuteTextBoxEdit.Text, out minute) && minute >= 0 && minute <= 59;

                        if (hourValid && minuteValid)
                        {
                            taskToSave.CompletionDate = new DateTime(selectedDateOnly.Year, selectedDateOnly.Month, selectedDateOnly.Day, hour, minute, 0);
                        }
                        else
                        {
                            MessageBox.Show("Hora ou minuto inválido. Apenas a data será salva (sem hora específica).", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                            taskToSave.CompletionDate = selectedDateOnly;
                            taskToSave.HasCompletionTime = false; // Corrige o flag pois a hora específica falhou
                        }
                    }
                    else
                    {
                        taskToSave.CompletionDate = selectedDateOnly;
                    }
                }
                else
                {
                    taskToSave.CompletionDate = null;
                    taskToSave.HasCompletionTime = false;
                }
            }
            else
            {
                taskToSave.CompletionDate = null;
                taskToSave.HasCompletionTime = false;
            }

            this.DialogResult = true;
            this.Close();
        }

        // Cancela as alterações.
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        // Limpa as informações de hora e data da tarefa.
        private void ClearCompletionDateButton_Click(object sender, RoutedEventArgs e)
        {
            EnableCompletionDateCheckBox.IsChecked = false;
            CompletionDatePickerEdit.SelectedDate = null;
            SpecifyTimeCheckBoxEdit.IsChecked = false;
            HourTextBoxEdit.Text = "00";
            MinuteTextBoxEdit.Text = "00";
        }
    }
}