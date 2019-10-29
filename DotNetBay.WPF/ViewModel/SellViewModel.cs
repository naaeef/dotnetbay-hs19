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

        private readonly Auction newAuction;

        private string filePathToImage;

        // needed?
        private Window sellView;

        public Auction NewAuction
        {
            get
            {
                return newAuction;
            }
        }


        public string FilePathToImage {
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
                        
                        NewAuction.Image = File.ReadAllBytes(value);

                        // probably to viewmodel, right?
                        if (this.FilePathToImage != null)
                        {
                            this.OnPropertyChanged(nameof(this.FilePathToImage));
                        }
                    }
                }
                
            }
        }

        public SellViewModel()
        {
            FilePathToImage = "<select image with extension jpg>";
            var app = Application.Current as App;

            if (app != null)
            {
                var simpleMemberService = new SimpleMemberService(app.MainRepository);
                this.auctionService = new AuctionService(app.MainRepository, simpleMemberService);
                this.newAuction = new Auction
                {
                    Seller = simpleMemberService.GetCurrentMember(),
                    StartDateTimeUtc = DateTime.UtcNow,
                    EndDateTimeUtc = DateTime.UtcNow.AddDays(7)
                };
            }
        }

        public void Save()
        {
            auctionService.Save(newAuction);
        }
    }
}
