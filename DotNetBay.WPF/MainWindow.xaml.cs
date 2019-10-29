using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using DotNetBay.Core;
using DotNetBay.Data.Entity;

namespace DotNetBay.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Auction> Auctions {
            get { return this.auctions; }

            private set
            {
                this.auctions = value;
                this.OnPropertyChanged();
            }
        }

        private readonly AuctionService auctionService;

        ObservableCollection<Auction> auctions = new ObservableCollection<Auction>();

        public MainWindow()
        {
            var app = Application.Current as App;

            InitializeComponent();

            this.DataContext = this;

            // get list of auctions
            if (app != null)
            {
                this.auctionService = new AuctionService(app.MainRepository, new SimpleMemberService(app.MainRepository));
                // how to register for events?
                this.auctions = new ObservableCollection<Auction>(this.auctionService.GetAll());
                
            }


        }

        private void newBidBtn_Click(object sender, RoutedEventArgs args)
        {
            Button button = (Button) args.Source;
            Auction ac = (Auction)button.DataContext;
            var bidView = new BidView(ac);
            bidView.ShowDialog();

            // Calling anyone?
            // update current price, no of bids, currrent winner
            var allAuctionsFromService = this.auctionService.GetAll();
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);
        }

        private void newAuctionBtn_Click(object sender, RoutedEventArgs e)
        {
            var sellView = new View.SellView();
            sellView.ShowDialog(); // Blocking

            var allAuctionsFromService = this.auctionService.GetAll();

            /* Option A: Full Update via INotifyPropertyChanged, not performant */
            /* ================================================================ */
            this.Auctions = new ObservableCollection<Auction>(allAuctionsFromService);

            /////* Option B: Let WPF only update the List and detect the additions */
            /////* =============================================================== */
            ////var toAdd = allAuctionsFromService.Where(a => !this.auctions.Contains(a));
            ////foreach (var auction in toAdd)
            ////{
            ////    this.auctions.Add(auction);
            ////}
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }

    /// <summary>
    /// Lab02 class converts boolean values to auction status string
    /// </summary>
    public class BooleanToStatusTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Boolean auctionClosed = (Boolean)value;
            return auctionClosed ? "closed" : "open";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
