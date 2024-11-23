using System.Collections.Generic;

namespace Application.Common.DTOs.Mail
{
    public class MailRequest
    {
        public MailRequest()
        {
            ToList = new List<string>();
            CcList = new List<string>();
        }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
      
        public string From { get; set; }
        public List<string> ToList { get; set; }
        public List<string> CcList { get; set; }
    }
}