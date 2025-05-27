using System.Windows;
using TaskManager.Models; // Ou o namespace correto para UserTask

namespace TaskManager.Views // Ou o namespace correto
{
    public partial class EditTaskWindow : Window
    {
        // Não precisa de uma propriedade para a tarefa aqui se o DataContext for a própria tarefa.
        // Se precisar fazer cópia ou lógica mais complexa, aí sim.

        public EditTaskWindow(UserTask taskToEdit)
        {
            InitializeComponent();
            this.DataContext = taskToEdit;

            UserTask task = (UserTask)this.DataContext;
            if (task.CompletionDate.HasValue)
            {
                CompletionDatePickerEdit.SelectedDate = task.CompletionDate.Value.Date;
                HourTextBoxEdit.Text = task.CompletionDate.Value.Hour.ToString("D2"); // Formato "00"
                MinuteTextBoxEdit.Text = task.CompletionDate.Value.Minute.ToString("D2"); // Formato "00"
            }
            else
            {
                HourTextBoxEdit.Text = "00"; // Valor padrão opcional
                MinuteTextBoxEdit.Text = "00"; // Valor padrão opcional
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UserTask taskToSave = (UserTask)this.DataContext;

            // Validação básica (Título é obrigatório)
            if (string.IsNullOrWhiteSpace(taskToSave.Title))
            {
                MessageBox.Show("O título da tarefa é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleTextBoxEdit.Focus();
                return;
            }

            if (CompletionDatePickerEdit.SelectedDate.HasValue)
            {
                DateTime selectedDate = CompletionDatePickerEdit.SelectedDate.Value;
                int hour = 0;
                int minute = 0;

                bool timeEntered = !string.IsNullOrWhiteSpace(HourTextBoxEdit.Text) || !string.IsNullOrWhiteSpace(MinuteTextBoxEdit.Text);
                bool hourValid = int.TryParse(HourTextBoxEdit.Text, out hour) && hour >= 0 && hour <= 23;
                bool minuteValid = int.TryParse(MinuteTextBoxEdit.Text, out minute) && minute >= 0 && minute <= 59;

                if (timeEntered)
                {
                    if (hourValid && minuteValid)
                    {
                        taskToSave.CompletionDate = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, hour, minute, 0);
                    }
                    else
                    {
                        MessageBox.Show("Hora ou minuto inválido. Por favor, use HH (0-23) e mm (0-59).\nA hora não será salva com esta data.", "Hora Inválida", MessageBoxButton.OK, MessageBoxImage.Warning);
                        taskToSave.CompletionDate = selectedDate.Date;
                    }
                }
                else
                {
                    taskToSave.CompletionDate = selectedDate.Date;
                }
            }
            else
            {
                taskToSave.CompletionDate = null;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}