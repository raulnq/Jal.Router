using Jal.Router.Model;
using System;
using System.Collections.Generic;

namespace Jal.Router.Impl
{
    public static class PostalOffice
    {
        public class MailBox
        {
            public event EventHandler<Message> OnPush;

            public void Push(Message message)
            {
                OnPush?.Invoke(this, message);
            }

        }

        private static readonly IDictionary<string, MailBox> _mailboxes = new Dictionary<string, MailBox>();

        public static void Create(string name)
        {
            _mailboxes.Add(name, new MailBox());
        }

        public static void Delete(string name)
        {
            _mailboxes.Remove(name);
        }

        public static bool Exists(string name)
        {
            return _mailboxes.ContainsKey(name);
        }

        public static void Push(string name, Message message)
        {
            var mailbox = _mailboxes[name];

            mailbox.Push(message);
        }

        public static void Subscribe(string name, Action<Message> action)
        {
            var mailbox = _mailboxes[name];

            mailbox.OnPush += (o,m)=> { action(m); };
        }      
    }
}