using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using CommonUse;
using System.ServiceModel;
using System.Collections.ObjectModel;
using System.Threading;
using System.ComponentModel;
using System.Windows.Threading;
using Telerik.Windows.Controls;
namespace Procesta.CvServer.Class.ClientChecker
{
    class ConnectFromServer// : IServerWithCallback, INotifyPropertyChanged
    {

        //#region static Members
        //private static RadTileView _counterInfoViewer;
        //private static List<CounterStatues> _countersStatues;
        //public static List<CounterStatues> countersStatues
        //{
        //    get { return _countersStatues; }
        //    set 
        //    {
        //        _countersStatues = value;
        //        new ConnectFromServer().OnPropertyChange("countersStatues");
        //    }
        //}
        //public event PropertyChangedEventHandler PropertyChanged;
        //public  void OnPropertyChange(string PropertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
        //    }
        //}
        //#endregion

        //#region Constrictor`s
        ///// <summary>
        ///// Constrictor to initial  List<CounterStatues>
        ///// </summary>
        //public ConnectFromServer()
        //{
        //    if (countersStatues == null)
        //    {
        //        countersStatues = new List<CounterStatues>();
        //    }
        //}

        ///// <summary>
        ///// Constrictor to initial  List And RadTileView
        ///// </summary>
        ///// <param name="counterInfoViewer">Type Telerik.Windows.Controls.RadTileView</param>
        //public ConnectFromServer(Telerik.Windows.Controls.RadTileView counterInfoViewer)
        //{
        //    if (countersStatues == null)
        //    {
        //        countersStatues = new List<CounterStatues>();
        //    }
        //    _counterInfoViewer = counterInfoViewer;
        //}
        //#endregion

        ///// <summary>
        ///// Call Clients to check connection IsAlive
        ///// </summary>
        ///// <param name="counterStatuse">Type of CounterStatues</param>
        //public void StartDataOutput(CounterStatues counterStatuse)
        //{
        //    try
        //    {
        //        CounterStatues tempCounter = countersStatues.Find(
        //            delegate(CounterStatues CountrIs)
        //            {
        //                return CountrIs.CounterName == counterStatuse.CounterName;
        //            }
        //            );
        //        if (tempCounter == null)
        //        {
        //            countersStatues.Add(counterStatuse);
        //            this.RefreshradTileView();
        //        }
        //        else
        //        {
        //            this.ItemRemove(counterStatuse);
        //        }
        //        IDataOutputCallback CheckClientAlive = OperationContext.Current.GetCallbackChannel<IDataOutputCallback>();
        //        for ( ; ; )
        //        {
        //            Random randomSleep = new Random();
        //            System.Threading.Thread.Sleep(randomSleep.Next(800, 4000));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        this.ItemRemove(counterStatuse);
        //    }
        //}

        //public void ClientCalls()
        //{
            
        //}
        //#region Private Method
        ///// <summary>
        ///// Remove a item form countersStatues List
        ///// </summary>
        ///// <param name="counterStatuse">Type of CounterStatues</param>
        //private void ItemRemove(CounterStatues counterStatuse)
        //{
        //    try
        //    {
        //        int tempIndex = countersStatues.FindIndex(
        //                               delegate(CounterStatues CounterIS)
        //                               {
        //                                   return CounterIS.CounterName == counterStatuse.CounterName;
        //                               }
        //                               );
        //        if (tempIndex > -1)
        //        {
        //            countersStatues.RemoveAt(tempIndex);
        //            this.RefreshradTileView();
        //        }
        //    }
        //    catch { }
        //}

        //private void RefreshradTileView()
        //{
        //    if (_counterInfoViewer != null)
        //    {
        //        _counterInfoViewer.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => { _counterInfoViewer.Items.Refresh(); }));
        //    }
        //}
        //#endregion

    }
}
