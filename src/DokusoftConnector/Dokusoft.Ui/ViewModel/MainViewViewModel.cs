using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using Dokusoft.IO.Com;
using Microsoft.Win32;

namespace Dokusoft.Ui.ViewModel;

public class MainViewViewModel : BindableBase
{
    private string _filePath = string.Empty;
    private string _selectedComPort = string.Empty;
    
    private ObservableCollection<string> _comPorts = new();

    public MainViewViewModel()
    {
        RefreshCom();
    }

    public string FilePath
    {
        get => _filePath;
        set
        {
            SetField(ref _filePath, value);
            OnPropertyChanged(nameof(SaveToFileDisabled));
        }
    }

    public ObservableCollection<string> ComPorts
    {
        get => _comPorts;
        set => SetField(ref _comPorts, value);
    }

    public string SelectedComPort
    {
        get => _selectedComPort;
        set => SetField(ref _selectedComPort, value);
    }

    public RelayCommand OpenFolderPickerCommand => new (PickFolder);

    public RelayCommand RefreshComPortCommand => new(RefreshCom);

    public RelayCommand ReadDataCommand => new(ReadData);

    public RelayCommand SaveToFileCommand => new(SaveToFile);

    public bool SaveToFileDisabled => !Directory.Exists(_filePath);

    private void PickFolder()
    {
        var dialog = new OpenFolderDialog();

        _ = dialog.ShowDialog();
        
        var result = dialog.FolderName;

        if (!string.IsNullOrEmpty(result))
            FilePath = result;
    }

    private void RefreshCom()
    {
        ComPorts = new(SerialPortRepository.GetPorts());
        SelectedComPort = ComPorts.FirstOrDefault() ?? string.Empty;
    }
    
    private void ReadData()
    {
        var _ = MessageBox.Show(":)", ":)");
    }

    private void SaveToFile()
    {
        var _ = MessageBox.Show(":)", ":)");
    }
}