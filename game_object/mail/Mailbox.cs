using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AscendedZ.game_object.mail
{
    public class Mailbox
    {
        public List<Mail> Mail { get; set; }

        public Mailbox()
        {
            Mail = new();
        }

        /// <summary>
        /// Looks for mail assuming all titles are unique.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool HasMail(string title)
        {
           var mail = Mail.Find(m => m.Title == title);

           return mail != null;
        }

        public void SetMailRead(int index)
        {
            Mail[index].IsRead = true;
        }

        public bool HasMailLeftUnread()
        {
            var mail = Mail.Find(m => m.IsRead == false);

            return mail != null;
        }
    }
}
