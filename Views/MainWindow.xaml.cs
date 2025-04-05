using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.ViewModels;

namespace TaskManager.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var vm = new TestViewModel();

        vm.CriarUsuarioT();       // cria um usuário "Vitor"
        vm.CriarTarefaT(1);       // tenta criar tarefa pro usuário com ID 1
        var tarefas = vm.ObterTarefas(); // método que você vai criar no ViewModel
        TaskListView.ItemsSource = tarefas;
    }
}
