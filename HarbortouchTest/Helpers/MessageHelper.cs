using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarbortouchTest.Helpers
{
    public class ViewMessage
    {
        public string MessageType { get; set; }
        public string BoldMessage { get; set; }
        public string Message { get; set; }
    }

    public static class MessageTypes
    {
        public const string Success = "alert-success";
        public const string Info = "alert-info";
        public const string Warning = "alert-warning";
        public const string Danger = "alert-danger";
    }

    public class GDriveItems : MsgItems
    {        
        public IList<Google.Apis.Drive.v3.Data.File> Items {get; set;}
        public GDriveItems()
        {
            Messages = new List<ViewMessage>();
            Items = new List<Google.Apis.Drive.v3.Data.File>();
        }
    }

    public class RentalItems : MsgItems
    {        
        public IEnumerable<HarbortouchTest.Rental> Items { get; set; }
        public RentalItems()
        {
            Messages = new List<ViewMessage>();
            Items = new List<HarbortouchTest.Rental>();
        }
    }

    public class TenantItems : MsgItems
    {        
        public IEnumerable<HarbortouchTest.Tenant> Items { get; set; }
        public TenantItems()
        {
            Messages = new List<ViewMessage>();
            Items = new List<HarbortouchTest.Tenant>();
        }
    }

    public abstract class MsgItems
    {
        public List<ViewMessage> Messages { get; set; }
    }
}