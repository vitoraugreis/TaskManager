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
            this.DataContext = taskToEdit; // Define o DataContext para a tarefa, os bindings no XAML funcionarão
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UserTask task = (UserTask)this.DataContext; // Pega a tarefa do DataContext

            // Validação básica (Título é obrigatório)
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                MessageBox.Show("O título da tarefa é obrigatório.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleTextBoxEdit.Focus();
                return;
            }

            // As alterações já foram aplicadas ao objeto 'task' devido ao TwoWay binding (implícito para TextBox/DatePicker).
            this.DialogResult = true; // Indica que as alterações devem ser salvas
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Indica que as alterações devem ser descartadas
            this.Close();
        }
    }
}