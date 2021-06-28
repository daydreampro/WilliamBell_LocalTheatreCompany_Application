using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models.ViewModels
{
    public class HomeViewModel
    {
        public PostInformationViewModel FeaturePost { get; set; }
        public PostInformationViewModel FeatureReview { get; set; }
        public PostInformationViewModel FeatureBlog { get; set; }
    }

}