using System;
using System.Collections.Generic;
using System.Text;

namespace HoloViewer
{
    public interface IBillingPage
    {
        protected const string BillingPageUrl = @"https://holoviewer-public.web.app/";

        void OpenPage ();
    }
}
