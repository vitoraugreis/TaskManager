using System.Windows;
using TaskManager.Models; // Ou o namespace correto para UserTask

namespace TaskManager.Views // Ou o namespace correto
{
    public partial class EditTaskWindow : Window
    {
        // N�o precisa de uma propriedade para a tarefa aqui se o DataContext for a pr�pria tarefa.
        // Se precisar fazer c�pia ou l�gica mais complexa, a� sim.

        public EditTaskWindow(UserTask taskToEdit)
        {
            InitializeComponent();
            this.DataContext = taskToEdit; // Define o DataContext para a tarefa, os bindings no XAML funcionar�o
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            UserTask task = (UserTask)this.DataContext; // Pega a tarefa do DataContext

            // Valida��o b�sica (T�tulo � obrigat�rio)
            if (string.IsNullOrWhiteSpace(task.Title))
            {
                MessageBox.Show("O t�tulo da tarefa � obrigat�rio.", "Valida��o", MessageBoxButton.OK, MessageBoxImage.Warning);
                TitleTextBoxEdit.Focus();
                return;
            }

            // As altera��es j� foram aplicadas ao objeto 'task' devido ao TwoWay binding (impl�cito para TextBox/DatePicker).
            this.DialogResult = true; // Indica que as altera��es devem ser salvas
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false; // Indica que as altera��es devem ser descartadas
            this.Close();
        }
    }
}