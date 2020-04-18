using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using RimWorldClothingManagerLibrary;

namespace RimWorldClothingManagerUi.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public MainWindowViewModel()
        {
            DefaultFilePath = (ConfigurationManager.AppSettings.GetValues("DefaultFilePath") ?? throw new InvalidOperationException()).First();

            GenerateDataCommand = new DelegateCommand(GenerateData);

            ApparelData = new ObservableCollection<ApparelModel>();
        }

        private void GenerateData()
        {
            var dataProcessor = new DataProcessor(DefaultFilePath);
            var output = dataProcessor.LoadData().OrderBy(x => x.Label);

            ApparelData = new ObservableCollection<ApparelModel>(output);
        }

        #region Property: DefaultFilePath

        private string _defaultFilePath;

        public string DefaultFilePath
        {
            get { return _defaultFilePath; }
            set
            {
                _defaultFilePath = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Property: DataSource

        public string DataSource
        {
            get { return $"Data Source: {DefaultFilePath}"; }
        }

        #endregion

        #region Property: GenerateDataCommand

        private ICommand _generateDataCommand;

        public ICommand GenerateDataCommand
        {
            get { return _generateDataCommand; }
            set
            {
                _generateDataCommand = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Property: ApparelData

        private ObservableCollection<ApparelModel> _apparelData;

        public ObservableCollection<ApparelModel> ApparelData
        {
            get { return _apparelData; }
            set
            {
                _apparelData = value;
                RaisePropertyChanged();
            }
        }

        #endregion
    }
}
