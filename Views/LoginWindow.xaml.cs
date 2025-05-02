using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskManager.Models;
using TaskManager.Data;
using TaskManager.ViewModels;
using TaskManager.Exceptions;

namespace TaskManager.Views;

public partial class LoginWindow : Window
{
    private LoginViewModel _viewModel;
    public LoginWindow()
    {
        InitializeComponent();
        _viewModel = new LoginViewModel();
    }

    private void LoginClick(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        try
        {
            var user = _viewModel.LoginUser(username);
            var main = new TaskViewWindow(user);
            main.Show();
            this.Close();
        }
        catch (InvalidUsernameException ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (UsernameNotExistException ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CreateUserClick(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        try
        {
            _viewModel.CreateUser(username);
            MessageBox.Show("Usuário criado com sucesso!", "Sucesso");
        }
        catch (UsernameAlreadyExistException ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        catch (InvalidUsernameException ex)
        {
            MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}

