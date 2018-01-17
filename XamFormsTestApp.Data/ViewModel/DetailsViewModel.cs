using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using XamFormsTestApp.Data.Messages;
using XamFormsTestApp.Data.Model;
using XamFormsTestApp.Data.Model.Common;
using XamFormsTestApp.Data.Services;

namespace XamFormsTestApp.Data.ViewModel
{
    public class DetailsViewModel : ViewModelBase, ICleanup
    {
        // Services
        private readonly IDialogService _dialog;

        // Commands
        private RelayCommand<Order> _refreshCommand;
        private RelayCommand _clearCommand;

        public DetailsViewModel(IDialogService dlg)
        {
            _dialog = dlg;
        }

        public RelayCommand ClearCommand
        {
            get
            {
                return _clearCommand
                       ?? (_clearCommand = new RelayCommand(
                           () =>
                           {
                               try
                               {
                                   // Notify any other vm's the orders should be cleared
                                   Messenger.Default.Send(new ClearOrderMessage());

                               }
                               catch (Exception ex)
                               {
                                   var result = _dialog.ShowMessage(ex.Message,
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
                               }

                           }));
            }
        }

        public RelayCommand<Order> RefreshCommand
        {
            get
            {
                return _refreshCommand
                       ?? (_refreshCommand = new RelayCommand<Order>(
                           async ord =>
                           {
                               try
                               {
                                   SelectedOrder = ord;
                                   var ves = ServiceLocator.Current.GetInstance<IOrderService>();
                                   Details = await ves.GetOrderDetails(ord.OrdId);

                               }
                               catch (Exception ex)
                               {
                                   // ReSharper disable once UnusedVariable (required as you cannot await in catch)
                                   var result = _dialog.ShowMessage(ex.Message.ToString(),
                                       ResourceConstants.PopupDialogGeneralError,
                                       ResourceConstants.PopupDialogOkButton,
                                       null);
                               }

                           }));
            }
        }

        // Properties

        private Order _order;
        public Order SelectedOrder
        {
            get { return _order; }
            set { Set(() => SelectedOrder, ref _order, value); }
        }

        private OrderDetails _ordDetails;
        public OrderDetails Details
        {
            get { return _ordDetails; }
            set { Set(() => Details, ref _ordDetails, value); }
        }



        // Override MVVM Light cleanup
        public override void Cleanup()
        {
        }
    }
}
