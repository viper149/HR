using System.Collections.Generic;

namespace HRMS.ViewModels.Hubs
{
    public class ChatUsersMesaages<TMessageEntity, TListOf>
        where TMessageEntity : class
        where TListOf : class
    {
        public TListOf CurrentUser { get; }
        public IEnumerable<TMessageEntity> Messages { get; }
        public List<TListOf> Users { get; }

        public ChatUsersMesaages(IEnumerable<TMessageEntity> messages, List<TListOf> users, TListOf currentUser)
        {
            Messages = messages;
            Users = users;
            CurrentUser = currentUser;
        }
    }
}
