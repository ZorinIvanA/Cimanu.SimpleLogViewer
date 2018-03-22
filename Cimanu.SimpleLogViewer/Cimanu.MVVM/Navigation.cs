using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Windows.UI.Xaml.Controls;
using System.Threading;

namespace Cimanu.MVVM
{
    public class Navigation
    {
        Page _currentPage;
        Frame _frame;

        public Navigation(Frame appFrame)
        {
            _frame = appFrame;
        }

        public void NavigateTo<TPage, TViewModel>(params object[] vmArgs)
            where TPage : Page
            where TViewModel : PageViewModelBase
        {
            if (_currentPage != null)
            {
                if (_currentPage.DataContext is IDisposable)
                    (_currentPage.DataContext as IDisposable).Dispose();
                _currentPage.DataContext = null;
                _currentPage = null;
            }

            var t = Task<TViewModel>.Run(() =>
            {
                return Activator.CreateInstance(typeof(TViewModel)) as TViewModel;
            });

            t.ContinueWith((viewModel) =>
            {                
                _frame.Navigate(typeof(TPage), viewModel.Result);
            },
            CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

            t.ContinueWith((viewModel) =>
            {
                //Error
            },
            CancellationToken.None, TaskContinuationOptions.NotOnRanToCompletion, TaskScheduler.FromCurrentSynchronizationContext());

        }
    }
}
