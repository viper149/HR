namespace DenimERP.ViewModels.Tips
{
    public class UserSecrets
    {
        private readonly bool[] _hasItem;

        public UserSecrets(bool[] hasItem)
        {
            _hasItem = hasItem;
        }

        public bool[] ShowList()
        {
            return _hasItem;
        }
    }
}