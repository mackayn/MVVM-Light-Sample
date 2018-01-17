using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using XamFormsTestApp.Data.Messages;
using XamFormsTestApp.Data.Model;
using XamFormsTestApp.Data.Model.Common;
using XamFormsTestApp.Data.Services;
using XamFormsTestApp.Data.Services.Common;

namespace XamFormsTestApp.Data.ViewModel
{
    public class StartViewModel : ViewModelBase, ICleanup
    {
        // Services
        private readonly IDialogService _dialogService;
        private readonly INavService _navService;
        private readonly IOrderService _ordService;

        // Commands
        private RelayCommand<Order> _selectCommand;
        private RelayCommand<Order> _toggleCommand;
        private RelayCommand _confirmCommand;
        private RelayCommand _refreshCommand;
        private RelayCommand _updateUiCommand;

        public StartViewModel(IOrderService ordServ, INavService nav, IDialogService dialog)
        {
            _ordService = ordServ;
            _navService = nav;
            _dialogService = dialog;

            Messenger.Default.Register<ClearOrderMessage>(this, action =>
            {
                Orders = new ObservableCollection<Order>();
            });

            SelectedOrder = null;
        }

        // Command implementations

         public RelayCommand UpdateUiCommand
        {
            get
            {
                return _updateUiCommand
                       ?? (_updateUiCommand = new RelayCommand(
                           () =>
                           {
                               try
                               {
                                   foreach (var ord in Orders)
                                   {
                                       ord.OrdName = "Updated from View Model...Easy isn't it";
                                   }

                               }
                               catch (Exception ex)
                               {
                                   // ReSharper disable once UnusedVariable (required as you cannot await in catch)
                                   var result = _dialogService.ShowMessage(ex.Message,
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
                               }

                           }));
            }
        }

        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                       ?? (_refreshCommand = new RelayCommand(
                           async () =>
                           {
                               try
                               {
                                   Orders = await _ordService.GetOrders();

                               }
                               catch (Exception ex)
                               {
                                   var result = _dialogService.ShowMessage(ex.Message,
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
                               }

                           }));
            }
        }

        public RelayCommand<Order> ToggleCommand
        {
            get
            {
                return _toggleCommand
                       ?? (_toggleCommand = new RelayCommand<Order>(
                           async ord =>
                           {
                               try
                               {
                                   if (!_toggleCommand.CanExecute(ord)) { return; }
                               }
                               catch (Exception ex)
                               {
                                   var res = _dialogService.ShowMessage(ex.Message.ToString(),
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
                               }
                           },
                           ord => ord != null));
            }
        }

        public RelayCommand<Order> SelectCommand
        {
            get
            {
                return _selectCommand
                       ?? (_selectCommand = new RelayCommand<Order>(
                           async ord =>
                           {
                               try
                               {
                                   if (!_selectCommand.CanExecute(ord)) { return; }

                                   SelectedOrder = ord;
                                   // Check for double tap if already awaiting nav
                                   if (_navService.CurrentPageKey == ViewModelLocator.PageKeyOrdDetails) return;
                                   await _navService.NavigateTo(ViewModelLocator.PageKeyOrdDetails, ord);
                               }
                               catch (Exception ex)
                               {
                                   var res  = _dialogService.ShowMessage(ex.Message.ToString(),
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
                               }
                           },
                           ord => ord != null));
            }
        }


        // Command implementations
        public RelayCommand ConfirmCommand
        {
            get
            {
                return _confirmCommand
                       ?? (_confirmCommand = new RelayCommand (
                           async () =>
                           {
                               try
                               {
                                   await _dialogService.ShowMessage("Does dialog callback work in MVVM Light",
                                      "Callback Test",
                                      "Yup",
                                      "Nope",
                                      DialogAction);   
                               }
                               catch (Exception ex)
                               {
                                   var res =_dialogService.ShowMessage(ex.Message.ToString(),
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
                               }
                           }));
            }
        }

        private async void DialogAction(bool value)
        {
            try
            {
                await _dialogService.ShowMessage("Result: " + value,
                                      "Result",
                                      ResourceConstants.PopupDialogOkButton,
                                      null);
            }
            catch (Exception ex)
            {
                var res = _dialogService.ShowMessage(ex.Message.ToString(),
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
            }
        }

        // Properties

        private Order _selOrder;
        public Order SelectedOrder
        {
            get { return _selOrder; }
            set { Set(() => SelectedOrder, ref _selOrder, value); }
        }

        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get { return _orders; }
            set { Set(() => Orders, ref _orders, value); }
        }

         // Override MVVM Light cleanup
        public override void Cleanup()
        {
            Messenger.Default.Unregister<ClearOrderMessage>(this);
        }

    }
}
