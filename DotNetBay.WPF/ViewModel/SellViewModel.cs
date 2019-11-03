using DotNetBay.Core;
using DotNetBay.Data.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DotNetBay.WPF.ViewModel
{
    class SellViewModel : ViewModelBase
    {
        private readonly AuctionService auctionService;

        private Auction newAuction;
        public Auction NewAuction { get => newAuction; }

        private string title;
        public string Title {
            get { return newAuction.Title; }
            set { newAuction.Title = value; OnPropertyChanged(nameof(Title)); }
        }
        public string Description {
            get { return newAuction.Description; }
            set { newAuction.Description = value; OnPropertyChanged(nameof(Description)); } }
             

        private int price;
        public double Price {
            get { return newAuction.StartPrice; }
            set { newAuction.StartPrice = value; OnPropertyChanged(nameof(Price)); } }

        public DateTime StartDateTimeUtc
        {
            get { return newAuction.StartDateTimeUtc; }
            set { newAuction.StartDateTimeUtc = value; OnPropertyChanged(nameof(StartDateTimeUtc)); }
        }

        public DateTime EndDateTimeUtc
        {
            get { return newAuction.EndDateTimeUtc; }
            set { newAuction.EndDateTimeUtc= value; OnPropertyChanged(nameof(EndDateTimeUtc)); }
        }

        private string filePathToImage;
        public string FilePathToImage
        {
            get
            {
                return filePathToImage;
            }
            set
            {
                if(File.Exists(value))
                {
                    var fileInfo = new FileInfo(value);

                    // allow only for jpg files
                    if (fileInfo.Extension.EndsWith("jpg"))
                    {    
                        filePathToImage = value;               
                        OnPropertyChanged(nameof(filePathToImage));
                    }
                }
                
            }
        }

        public SellViewModel()
        {
            newAuction = new Auction();
            FilePathToImage = "<select image with extension jpg>";
            var app = Application.Current as App;

            if (app != null)
            {
                var simpleMemberService = new SimpleMemberService(app.MainRepository);
                this.auctionService = new AuctionService(app.MainRepository, simpleMemberService);
//                this.newAuction = new Auction
  //              {
    //                Seller = simpleMemberService.GetCurrentMember(),
      //              StartDateTimeUtc = DateTime.UtcNow,
        //            EndDateTimeUtc = DateTime.UtcNow.AddDays(7)
          //      };
            }
        }

        public void Save()
        {
            NewAuction.Image = File.ReadAllBytes(filePathToImage);
            auctionService.Save(newAuction);
        }
    }
}
